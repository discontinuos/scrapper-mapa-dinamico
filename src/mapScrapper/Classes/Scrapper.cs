using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using System.Globalization;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using NetTopologySuite.Geometries;
using System.IO;

namespace mapScrapper
{
	public class Scrapper
	{
		SortedDictionary<string, bool> skipRadios = new SortedDictionary<string, bool>();
        public string ProvFilter;
        public string DptoFilter;
        public string RadioFilter;
        public string FracFilter;

        public void GetMapsFromDbfList(string file)
		{
			List<Feature> features;
			List<string> noCovered;
			features = PrepareLists(file, out noCovered);

			Downloader downloader = new Downloader(); 
			foreach (var f in features)
			{
				string redcode = f.Attributes["REDCODE"] as string;
				RadioInfo r = RadioInfo.ParseRedcode(redcode);
				if (skipRadios.ContainsKey(r.makeKey()) == false &&
					noCovered.Contains(r.makeKey()) == false)
				{
					FraccionInfo fraccion = new FraccionInfo(r.Prov, r.Dpto, r.Fraccion);
					if (downloader.getFraccionInfo(fraccion))
						downloader.getMapaRadio(r, fraccion.Extents);
				}
			}
		}

		public void GetHiResMapsFromDbfList(string file, bool deepSearch = true)
		{
			List<Feature> features;
			List<string> noCovered;
			features = PrepareLists(file, out noCovered, true);

			Downloader downloader = new Downloader();
			Recognizer reco = new Recognizer();
			foreach (var f in features)
			{
				string redcode = f.Attributes["REDCODE"] as string;
				RadioInfo r = RadioInfo.ParseRedcode(redcode);
				if (skipRadios.ContainsKey(r.makeKey()) == false &&
					noCovered.Contains(r.makeKey()) == false )
				//if (r.Prov == "02" && (r.Dpto != "01" || r.Fraccion != "01"))
				{
					FraccionInfo fraccion = new FraccionInfo(r.Prov, r.Dpto, r.Fraccion);
					if (r.isDone(true) == false && r.isNotRequired(true) == false && downloader.getFraccionInfo(fraccion))
					{
						// le recalcula el extent
						reco.RecognizeRadio(r, false, deepSearch);
						// le pone una proporción igual a la de la fracción
						if (r.Geometry != null && r.GeoExtents.Width < fraccion.ExtentsEnvelope.Width * .75F &&
							r.GeoExtents.Height < fraccion.ExtentsEnvelope.Height * .75F)
						{
							string extents = CalculateZoomedExtents(r, fraccion);
							downloader.getMapaRadio(r, extents, true);
						}
						else
							File.WriteAllText(r.getNotRequiredName(true), "-");
					}
					else
						Console.Write(".");
				}
			}
		}

		private static string CalculateZoomedExtents(RadioInfo r, FraccionInfo fraccion)
		{
			double fraccionRatio = fraccion.ExtentsProportion;
			double radioRatio = r.GeoExtentsProportion;

			string extents;
			Envelope newExtent;
			if (radioRatio < fraccionRatio)
			{
				// hay que agrandarla verticalmente
				double extra = fraccionRatio * r.GeoExtents.Width - r.GeoExtents.Height;
				newExtent = new Envelope(r.GeoExtents.MinX,
													r.GeoExtents.MaxX,
													r.GeoExtents.MinY - extra / 2,
													r.GeoExtents.MaxY + extra / 2);
			}
			else
			{
				// hay que agrandarla horizontalmente
				double extra = r.GeoExtents.Height / fraccionRatio - r.GeoExtents.Width;
				newExtent = new Envelope(r.GeoExtents.MinX - extra / 2,
													r.GeoExtents.MaxX + extra / 2,
													r.GeoExtents.MinY,
													r.GeoExtents.MaxY);
			}
			double xMargin = newExtent.Width * .4F;
			double yMargin = newExtent.Height * .4F;

			extents = (newExtent.MinX - xMargin) + " " +
													(newExtent.MinY - yMargin) + " " +
													(newExtent.MaxX + xMargin) + " " +
													(newExtent.MaxY + yMargin);

			extents = extents.Replace(",", ".");
			return extents;
		}

		private List<Feature> PrepareLists(string file, out List<string> noCovered, bool hiRes = false)
		{
			List<Feature> allFeatures = new List<Feature>();
            allFeatures = FileReaders.ReadDbasefile(file);

            var features = FilterFeatures(allFeatures);

            Console.WriteLine("");
			Console.WriteLine("Calculando radios pendientes...");
			int total = 0;
			int pendientes = 0;

			noCovered = new List<string>();
            string nocoveredFile = Context.ResolveFilename("no_covered.txt");
            if (File.Exists(nocoveredFile))
                noCovered.AddRange(File.ReadAllLines(nocoveredFile));
			
			foreach (var f in features)
			{
				string redcode = f.Attributes["REDCODE"] as string;
				RadioInfo r = RadioInfo.ParseRedcode(redcode);
				if (skipRadios.ContainsKey(r.makeKey()) == false)
				{
					total++;
					string name = r.makeName();
					if (!r.isDone(hiRes) && !r.isNotRequired(hiRes) &&
						noCovered.Contains(r.makeKey()) == false) pendientes++;
				}
			}
			Console.WriteLine("Pendientes: " + pendientes + ".");
			Console.WriteLine("Descubiertos sin cobertura: " + noCovered.Count + ".");
			Console.WriteLine("Total: " + total + ".");
			return features;
		}

        private List<Feature> FilterFeatures(List<Feature> features)
        {
            List<Feature> ret = new List<Feature>();
            foreach (var f in features)
            {
                if (ShouldAddFeature(f))
                    ret.Add(f);
            }
            return ret;
        }

        private bool ShouldAddFeature(Feature f)
        {
            string redcode = f.Attributes["REDCODE"] as string;
            RadioInfo r = RadioInfo.ParseRedcode(redcode);
            if (String.IsNullOrEmpty(this.ProvFilter))
                return true;
            if (this.ProvFilter != r.Prov)
                return false;
            if (String.IsNullOrEmpty(this.DptoFilter))
                return true;
            if (this.DptoFilter != r.Dpto)
                return false;
            if (String.IsNullOrEmpty(this.FracFilter))
                return true;
            if (this.FracFilter != r.Fraccion)
                return false;
            if (String.IsNullOrEmpty(this.RadioFilter))
                return true;
            if (this.RadioFilter != r.Radio)
                return false;
            else
                return true;
        }

        public void loadSkipRadios(string file)
		{
			Console.WriteLine("Cargando radios conocidos...");
			List<Feature> features = FileReaders.ReadDbasefile(file);
			foreach (var f in features)
			{
				RadioInfo ri = RadioInfo.FromFeature(f);
				if (ri.Radio != "" && ri.Fraccion != "" && ri.Radio != "00" && ri.Fraccion != "00")
					skipRadios[ri.makeKey()] = true;
			}
			Console.WriteLine(features.Count + " radios agregados.");
		}

		
		
	}
}
