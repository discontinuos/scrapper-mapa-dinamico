using System;
using System.IO;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using System.Reflection;

namespace mapScrapper
{
	public class Context
	{

		const string posgar3 = "PROJCS[\"POSGAR 94 / Argentina 3\",GEOGCS[\"POSGAR 94\",DATUM[\"D_POSGAR_1994\",SPHEROID[\"WGS_1984\",6378137,298.257223563]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",-90],PARAMETER[\"central_meridian\",-66],PARAMETER[\"scale_factor\",1],PARAMETER[\"false_easting\",3500000],PARAMETER[\"false_northing\",0],UNIT[\"Meter\",1]]";
        const string krugger3 = "PROJCS[\"DHDN / Gauss-Kruger zone 3\", GEOGCS[\"DHDN\", DATUM[\"D_Deutsches_Hauptdreiecksnetz\", SPHEROID[\"Bessel_1841\", 6377397.155,299.1528128]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Transverse_Mercator\"], PARAMETER[\"latitude_of_origin\",-90],PARAMETER[\"central_meridian\",-66],PARAMETER[\"scale_factor\",1],PARAMETER[\"false_easting\",3500000],PARAMETER[\"false_northing\",0],UNIT[\"Meter\",1]]";
        const string wkt4326 = "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4326\"]]";
        
        public static string url = "http://www.sig.indec.gov.ar/index.php";
		public static string url2 = "http://www.sig.indec.gov.ar";
        public static string Projection = krugger3;// posgar3;
		public static string Geo = wkt4326;

        public static string OutputDataDirectory
        {
            get {  return DataDirectory + "\\output"; }
        }

        public static string InputDataDirectory
        {
            get { return DataDirectory + "\\input"; }
        }
        public static string DataDirectory;
        public static string CalculateDataDirectory()
		{
			string file = Assembly.GetExecutingAssembly().Location;
            var f = Path.GetDirectoryName(file);
            if (f.ToLower().EndsWith("\\release") ||
                f.ToLower().EndsWith("/release") ||
                f.ToLower().EndsWith("\\debug") ||
                f.ToLower().EndsWith("/debug"))
            {
                f = EatOneLevel(f);
                f = EatOneLevel(f);
                f = EatOneLevel(f);
                f = EatOneLevel(f);
            }
            DataDirectory = f + "\\data";
            return DataDirectory;
        }

        private static string EatOneLevel(string f)
        {
            var parent = Path.GetDirectoryName(f);
            if (Directory.Exists(parent))
                return parent;
            else
                return f;
        }

        public static void Log(string file, string text)
		{
			string filename = ResolveFilename(file + ".txt");
			File.AppendAllText(filename, text + Environment.NewLine);
			Console.WriteLine(file + ": " + text); 
		}

		public static string ResolveFilename(string file)
		{
			string path = Path.Combine(OutputDataDirectory, file);
			return path;
		}

		public static void Log(string file, GeoBase item, string text = "")
		{
			Log(file, item.makeName() + text);
		}

		static DateTime now = DateTime.Now;
		public static void WriteTimer()
		{
			Console.WriteLine("Secs: " + ((DateTime.Now - now).TotalSeconds).ToString());
			now = DateTime.Now;
		}
	}
}
