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
	public class FileReaders
	{
	
		public static List<Feature> ReadDbasefile(string dbfFilename)
		{
			var features = new List<Feature>();

			DbaseFileReader dr = new DbaseFileReader(dbfFilename);

			DbaseFileHeader header = dr.GetHeader();
			foreach (System.Collections.ArrayList atts in dr)
			{
				AttributesTable attributesTable = new AttributesTable();
				for (int i = 0; i < header.NumFields; i++)
					attributesTable.AddAttribute(header.Fields[i].Name, atts[i]);

				features.Add(new Feature(null, attributesTable));
			}

			return features;
		}

		public static List<Feature> ReadShapefile(string shpFilename, int max = int.MaxValue)
		{
			var features = new List<Feature>();
			using (ShapefileDataReader dr = new ShapefileDataReader(shpFilename, new GeometryFactory()))
			{
				DbaseFileHeader header = dr.DbaseHeader;
				int n = 0;
				while (dr.Read())
				{
					n++;
					if (n > max) break;
					AttributesTable attributesTable = new AttributesTable();
					for (int i = 0; i < header.NumFields; i++)
						attributesTable.AddAttribute(header.Fields[i].Name, dr.GetValue(i));

					features.Add(new Feature(dr.Geometry, attributesTable));
				}
			}
			return features;
		}


		
		
	}
}
