using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace mapScrapper
{
	[Serializable]
	public class RadioInfo : GeoBase
	{
		public string Radio;
		public List<Polygon> Polygon = new List<Polygon>();
		public System.Drawing.Size ImageExtents;
		public GeoAPI.Geometries.Envelope GeoExtents;
		public double TotalPerimeter;
		

		public double GeoExtentsProportion
		{
			get
			{
				return (GeoExtents.MaxY - GeoExtents.MinY) /
							(GeoExtents.MaxX - GeoExtents.MinX);
			}
		}

		public RadioInfo()
		{
		}

		public virtual bool isDone(bool hires = false)
		{
			if (SaveToTmp) return false;
			string file = getGifName(hires);
			string file2 = getTxtName(hires);
			return (File.Exists(file) && File.Exists(file2));
		}

		public virtual bool isNotRequired(bool hires = false)
		{
			if (SaveToTmp) return false;
			string file = getNotRequiredName(hires);
			return (File.Exists(file));
		}

		public RadioInfo(FraccionInfo frac)
		{
			Prov = frac.Prov;
			Dpto = frac.Dpto;
			Fraccion = frac.Fraccion;
		}


		public override string makeKey()
		{
			return Prov + "-" + Dpto + "-" + Fraccion + "-" + Radio;
		}
		public static RadioInfo ParseRedcode(string redcode)
		{
			var ret = new RadioInfo();
			ret.Prov = redcode.Substring(0, 2);
			ret.Dpto = redcode.Substring(2, 3);
			ret.Fraccion = redcode.Substring(5, 2);
			ret.Radio = redcode.Substring(7, 2);
			return ret;
		}

		public static RadioInfo FromFeature(NetTopologySuite.Features.Feature f)
		{
			var ret = new RadioInfo();
			string prov = f.Attributes["PROV"] as string;
			string dpto = f.Attributes["DEPTO"] as string;
			string frac = f.Attributes["FRAC"] as string;
			string rad = f.Attributes["RADIO"] as string;
			ret.Prov = prov;
			ret.Dpto = dpto;
			ret.Fraccion = frac;
			ret.Radio = rad;
			return ret;
		}
		public static RadioInfo FromFeatureRedcode(NetTopologySuite.Features.Feature f, string redcodeField = "REDCODE")
		{
			var ret = new RadioInfo();
			string data = f.Attributes[redcodeField] as string;
			string prov = data.Substring(0, 2);
			string dpto = data.Substring(2, 3);
			string frac = data.Substring(5, 2);
			string rad = data.Substring(7, 2);
			ret.Prov = prov;
			ret.Dpto = dpto;
			ret.Fraccion = frac;
			ret.Radio = rad;
			return ret;
		}
        public override string ToString()
        {
            return this.Redcode;
        }
        public string Redcode { get { return Prov + Dpto + Fraccion + Radio; } }

	}
}
