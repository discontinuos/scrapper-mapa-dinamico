using System.IO;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;



namespace mapScrapper
{
	public class Projections
	{
		const string wkt4326 = "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4326\"]]";
		const string wkt3857 = "PROJCS[\"Popular Visualisation CRS / Mercator\", GEOGCS[\"Popular Visualisation CRS\", DATUM[\"Popular Visualisation Datum\", SPHEROID[\"Popular Visualisation Sphere\", 6378137, 0, AUTHORITY[\"EPSG\",\"7059\"]], TOWGS84[0, 0, 0, 0, 0, 0, 0], AUTHORITY[\"EPSG\",\"6055\"] ], PRIMEM[\"Greenwich\", 0, AUTHORITY[\"EPSG\", \"8901\"]], UNIT[\"degree\", 0.0174532925199433, AUTHORITY[\"EPSG\", \"9102\"]], AXIS[\"E\", EAST], AXIS[\"N\", NORTH], AUTHORITY[\"EPSG\",\"4055\"] ], PROJECTION[\"Mercator\"], PARAMETER[\"False_Easting\", 0], PARAMETER[\"False_Northing\", 0], PARAMETER[\"Central_Meridian\", 0], PARAMETER[\"Latitude_of_origin\", 0], UNIT[\"metre\", 1, AUTHORITY[\"EPSG\", \"9001\"]], AXIS[\"East\", EAST], AXIS[\"North\", NORTH], AUTHORITY[\"EPSG\",\"3785\"]]";
		const string wkt22183 = "PROJCS[\"POSGAR 94 / Argentina 3\",GEOGCS[\"POSGAR 94\",DATUM[\"D_POSGAR_1994\",SPHEROID[\"WGS_1984\",6378137,298.257223563]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",-90],PARAMETER[\"central_meridian\",-66],PARAMETER[\"scale_factor\",1],PARAMETER[\"false_easting\",3500000],PARAMETER[\"false_northing\",0],UNIT[\"Meter\",1]]";
		const string albers = "PROJCS[\"Albers Equal Area\", GEOGCS[\"WGS 84\", DATUM[\"World Geodetic System 1984\", SPHEROID[\"WGS 84\", 6378137.0, 298.257223563, AUTHORITY[\"EPSG\",\"7030\"]], AUTHORITY[\"EPSG\",\"6326\"]], PRIMEM[\"Greenwich\", 0.0, AUTHORITY[\"EPSG\",\"8901\"]], UNIT[\"degree\", 0.017453292519943295], AXIS[\"Geodetic longitude\", EAST], AXIS[\"Geodetic latitude\", NORTH], AUTHORITY[\"EPSG\",\"4326\"]], PROJECTION[\"Albers_Conic_Equal_Area\"], PARAMETER[\"central_meridian\", -96.0], PARAMETER[\"latitude_of_origin\", 37.5], PARAMETER[\"standard_parallel_1\", 29.833333333333336], PARAMETER[\"false_easting\", 0.0], PARAMETER[\"false_northing\", 0.0], PARAMETER[\"standard_parallel_2\", 45.833333333333336], UNIT[\"m\", 1.0], AXIS[\"Easting\", EAST], AXIS[\"Northing\", NORTH], AUTHORITY[\"EPSG\",\"41111\"]]";
		const string albers2 = "PROJCS[\"South_America_Albers_Equal_Area_Conic\",GEOGCS[\"GCS_South_American_1969\",DATUM[\"D_South_American_1969\",SPHEROID[\"GRS_1967_Truncated\",6378160,298.25]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Albers\"],PARAMETER[\"False_Easting\",0],PARAMETER[\"False_Northing\",0],PARAMETER[\"central_meridian\",-60],PARAMETER[\"Standard_Parallel_1\",-5],PARAMETER[\"Standard_Parallel_2\",-42],PARAMETER[\"latitude_of_origin\",-32],UNIT[\"Meter\",1]]";
	
		public static IPoint UnprojectPoint(IPoint p, string zone)
		{
			ICoordinateSystem csSource = ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(zone) as ICoordinateSystem;
			ICoordinateSystem csTarget = ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(wkt4326) as ICoordinateSystem;
			ICoordinateTransformation trans = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory().CreateFromCoordinateSystems(csSource, csTarget);

			IPoint pOut = NetTopologySuite.CoordinateSystems.Transformations.GeometryTransform.TransformPoint(GeometryFactory.Default, p, trans.MathTransform);
			return pOut;
		}
		static ICoordinateTransformation transUn;
		static ICoordinateTransformation trans;
        
		const string proj = "GCS_WGS_1984";
		const string descr = "Geographic coordinate system > Word > WGS1984";
		public static bool Validate(string file)
		{
			var text = File.ReadAllText(file).Trim();
			if (text.StartsWith("GEOGCS[\"" + proj + "\""))
				return true;
			else
				return false;
		}
        
	}
}
