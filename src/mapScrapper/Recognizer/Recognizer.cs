using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using System.Globalization;
using System.IO;
using NetTopologySuite.Geometries;
using System.Drawing;

namespace mapScrapper
{
	public class Recognizer
	{
		public List<RadioInfo> RecognizeFolder(string folder)
		{
			List<RadioInfo> radios = ReadRadiosFromFolder(folder);

			foreach (var r in radios)
			{
			    RecognizeRadio(r);
			}
			return radios;
		}

		public static List<RadioInfo> ReadRadiosFromFolder(string folder)
		{
			List<RadioInfo> radios = new List<RadioInfo>();
			foreach (string file in Directory.GetFiles(folder, "*.gif"))
			{
				string name = Path.GetFileNameWithoutExtension(file);
				name = name.Replace("prov", "");
				string[] parts = name.Split('-');
				if (parts.Length == 4)
				{
					RadioInfo r1 = new RadioInfo();
					r1.Prov = parts[0];
					r1.Dpto = parts[1];
					r1.Fraccion = parts[2];
					r1.Radio = parts[3];
					radios.Add(r1);
				}
			}
			return radios;
		}

		public void RecognizeRadio(RadioInfo r, bool project = true, bool deepSearch = true)
		{
			RecognizeRadioPolygon(r, deepSearch);
			RecognizeRadioGeometry(r, project);
		}
		public void RecognizeRadioPolygon(RadioInfo r, bool deepSearch = true)
		{
			Console.Write("Analyzing " + r.makeKey() + "...");
			string file = r.getGifName();
			string fileHiRes = r.getGifName(true);
			Image pImage = null;
			bool useHiRes = File.Exists(fileHiRes);

			if (useHiRes)
				pImage = Bitmap.FromFile(fileHiRes);
			else
				pImage = Bitmap.FromFile(file);
			var polygon = RecognizeImagePolygon(pImage);
			r.Polygon = polygon;
			r.ImageExtents = pImage.Size;
			r.Extents = Downloader.ReadExtents(r.getTxtName(useHiRes));
			
			Console.WriteLine("Done.");
		}
		public List<Polygon> RecognizeImagePolygon(Image pImage, bool deepSearch = true)
		{
			List<Line> lines;
			return RecognizeImagePolygon(pImage, out lines, deepSearch);
		}
		public List<Polygon> RecognizeImagePolygon(Image pImage, out List<Line> lines, bool deepSearch = true)
		{
			var f = new LineFinder();
			lines = f.ScanBitmap(pImage as Bitmap);

			var s = new PolygonFinder();
			List<Polygon> maxPolygon = new List<Polygon>();
			int maxPoints = 0;
			double maxPolygonLength = 0;
			for (int n = 0; n < lines.Count * 2; n++)
			{
				int pointsCovered = 0;
				double polygonLength = 0;
				var polygon = s.FindContour(lines, out pointsCovered, out polygonLength, n);
				if (pointsCovered > maxPoints || 
					(pointsCovered == maxPoints &&
					polygonLength < maxPolygonLength))
				{ // si cubre más puntos, o cubre los mismos con menos tramos
					maxPolygon = polygon;
					maxPolygonLength = polygonLength;
					maxPoints = pointsCovered;
				}
				if (n == 10 && deepSearch == false)
					break;
			}
			// si tiene pendientes, retoma la búsqueda
			return maxPolygon;
		}


		private void RecognizeRadioGeometry(RadioInfo radio, bool project = true)
		{
			if (radio.Polygon.Count == 0 || radio.Polygon[0].Count == 0)
				return;
			radio.TotalPerimeter = 0;
			List<List<Coordinate>> puntosTotal = new List<List<Coordinate>>();
			double xMin = double.MaxValue, xMax = double.MinValue, yMin = double.MaxValue, yMax = double.MinValue;
			foreach (var polygon in radio.Polygon)
			{
				List<Coordinate> puntos = CreatePointsSet(radio, project, ref xMin, ref xMax, ref yMin, ref yMax, polygon);
				puntosTotal.Add(puntos);
			}
			
			///////
			GeometryFactory fc = new GeometryFactory();
			if (puntosTotal.Count > 1)
			{
				List<IPolygon> geoms = new List<IPolygon>();
				foreach(var puntos in puntosTotal)
					geoms.Add(fc.CreatePolygon(puntos.ToArray()));
				radio.Geometry = fc.CreateMultiPolygon(geoms.ToArray());
			}
			else
				radio.Geometry = fc.CreatePolygon(puntosTotal[0].ToArray());

			radio.GeoExtents = new Envelope(xMin, xMax, yMin, yMax);
		}

		private static List<Coordinate> CreatePointsSet(RadioInfo radio, bool project, ref double xMin, ref double xMax, ref double yMin, ref double yMax, Polygon polygon)
		{
			if (polygon[0] != polygon[polygon.Count - 1])
			{
				if (polygon.Count == 2)
				{
					// le hace triángulo
					polygon.Add(new System.Drawing.Point(polygon[0].X, polygon[1].Y));
				}
				polygon.Add(polygon[0]);
			}
			//////////////////////
			Coordinate from = radio.GetFrom();
			Coordinate to = radio.GetTo();
			double xScale = (to.X - from.X) / radio.ImageExtents.Width;
			double yScale = (to.Y - from.Y) / radio.ImageExtents.Height;

			List<Coordinate> puntos = new List<Coordinate>();
			IPoint lastP = null;
			foreach (System.Drawing.Point p in polygon)
			{
				var newPoint = new NetTopologySuite.Geometries.Point(xScale * p.X +
							from.X, yScale * (radio.ImageExtents.Height - p.Y) + from.Y);
				IPoint point;
				if (project)
					point = Projections.UnprojectPoint(newPoint, Context.Projection);
				else
					point = newPoint;

				xMin = Math.Min(point.X, xMin);
				xMax = Math.Max(point.X, xMax);

				yMin = Math.Min(point.Y, yMin);
				yMax = Math.Max(point.Y, yMax);

				puntos.Add(new Coordinate(point.X, point.Y));
				if (lastP != null)
					radio.TotalPerimeter += PolygonFinder.EuclideanDist(point, lastP);
	
				lastP = point;
			}
			return puntos;
		}

	}
}
