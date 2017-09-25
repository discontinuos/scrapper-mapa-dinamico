using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace mapScrapper
{
	[Serializable]
	public class Polygon : List<System.Drawing.Point>
	{
		public double PolygonLength;
		}
}
