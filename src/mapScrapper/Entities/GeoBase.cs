using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using System.Globalization;
using System.IO;

namespace mapScrapper
{
		[Serializable]
	public abstract class GeoBase
	{
		public string Prov;
		public string Dpto;
		public string Fraccion;
		public string Extents;
		public abstract string makeKey();
		public IGeometry Geometry;
		public bool SaveToTmp;

		public Coordinate GetFrom()
		{
			string[] parts = Extents.Split(' ');
			return new Coordinate(double.Parse(parts[0], CultureInfo.InvariantCulture), double.Parse(parts[1], CultureInfo.InvariantCulture));
		}

		public Coordinate GetTo()
		{
			string[] parts = Extents.Split(' ');
			return new Coordinate(double.Parse(parts[2], CultureInfo.InvariantCulture), double.Parse(parts[3], CultureInfo.InvariantCulture));
		}
		public Envelope ExtentsEnvelope
		{
			get
			{
				return new Envelope(GetFrom(), GetTo());
			}
		}
		public double ExtentsProportion
		{
			get
			{
				var from = GetFrom();
				var to = GetTo();
				return (to.Y - from.Y) / (to.X - from.X);
			}
		}

		public string makeName()
		{
			return "prov" + makeKey();
		}
		public string getGifName(bool hiRes = false)
		{
			return resolveFilename("gif", (hiRes ? "-hi" : ""));
		}
		public string getTxtName(bool hiRes = false)
		{
			return resolveFilename("txt", (hiRes ? "-hi" : ""));
		}
		public string getNotRequiredName(bool hiRes = false)
		{
			return resolveFilename("notReq", (hiRes ? "-hi" : ""));
		}
		private string resolveFilename(string ext, string suffix = "")
		{
			string subFolder = this.Prov;
			string dir = Path.Combine(Context.OutputDataDirectory, subFolder);
			if (SaveToTmp)
				dir = Path.GetTempPath();
			if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

			string name = makeName() + suffix + "." + ext;
			string path = Path.Combine(dir, name);
			return path;
		}
	}
}
