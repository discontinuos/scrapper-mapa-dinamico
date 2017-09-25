using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using System.Globalization;
using System.IO;

namespace mapScrapper
{
	public class FraccionInfo : GeoBase
	{
		public List<RadioInfo> Radios = new List<RadioInfo>();

		public FraccionInfo(string prov, string dpto, string fra)
		{
			Prov = prov;
			Dpto = dpto;
			Fraccion = fra;
		}
		public virtual bool isDone(bool hiRes = false)
		{
			string file = getTxtName(hiRes);
			return (File.Exists(file));
		}
		public override string makeKey()
		{
			return Prov + "-" + Dpto + "-" + Fraccion;
		}

	}
}
