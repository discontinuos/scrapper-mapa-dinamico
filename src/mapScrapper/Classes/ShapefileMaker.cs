using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using System.Globalization;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.Features;
using System.IO;

namespace mapScrapper
{
	public class ShapefileMaker
	{
		public void Create(List<RadioInfo> list, string outputName, string projection)
		{
			var features = new List<IFeature>();
			foreach (RadioInfo radio in list)
			{
				///////
				var attributesTable = new AttributesTable();
                if (radio.Geometry != null)
                {
                    attributesTable.AddAttribute("Redcode", radio.Redcode);
                    attributesTable.AddAttribute("IsValid", radio.Geometry.IsValid);
                    attributesTable.AddAttribute("Points", radio.Geometry.Coordinates.Count());
                    attributesTable.AddAttribute("AreaKm2", radio.Geometry.Area * 1000 * 1000);
                    attributesTable.AddAttribute("Polygons", radio.Polygon.Count);
                    attributesTable.AddAttribute("PerimeterKm", radio.TotalPerimeter * 1000);
                    GeometryFactory fc = new GeometryFactory();
                    var geo = radio.Geometry;
                    features.Add(new Feature(geo, attributesTable));
                }
                else
                    Console.WriteLine("Empty geometry: " + radio.Redcode);
            }
			// Create the shapefile
			var outGeomFactory = GeometryFactory.Default;
			var writer = new ShapefileDataWriter(Context.ResolveFilename(outputName), outGeomFactory);
			var outDbaseHeader = ShapefileDataWriter.GetHeader(features[0], features.Count);
			writer.Header = outDbaseHeader;
			writer.Write(features);

			// Create the projection file
			using (var streamWriter = new StreamWriter(Context.ResolveFilename(outputName + ".prj")))
			{
				streamWriter.Write(projection);
			}
		}


	}
}
