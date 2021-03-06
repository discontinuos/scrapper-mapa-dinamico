﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using System.Globalization;
using System.Drawing;

namespace mapScrapper
{
	public class PolygonFinder
	{
		//List<Point> polygon;
		List<Point> unused;
		List<Line> allLines;
		const int LIMIT = 20;

		public List<Polygon> FindContour(List<Line> list, out int pointsCovered, out double polygonLength, int seed = 0)
		{
			List<Polygon> ret = new List<Polygon>();
			allLines = new List<Line>();
			allLines.AddRange(list);
			unused = new List<Point>();
			pointsCovered = 0;
			polygonLength = 0;
			foreach (var l in list)
			{
				unused.Add(l.P1);
				unused.Add(l.P2);
			}

			while (unused.Count > 0)
			{
				int next = seed % unused.Count;
				Polygon polygon = new Polygon();
				while ((next = VisitNext(polygon, next)) != -1);
				if (polygon.Count == 1)
					break;
				pointsCovered += polygon.Count;
				polygon = optimize(polygon);
				polygonLength += AddLength(polygon);
				polygon.PolygonLength = polygonLength;
				ret.Add(polygon);
			}
			
			return ret;
		}

		private double AddLength(Polygon polygon)
		{
			double polygonLength;
			polygonLength = 0;
			if (polygon.Count > 1)
			{
				for (int i = 0; i < polygon.Count - 1; i++)
				{
					polygonLength += EuclideanDist(polygon[i], polygon[i + 1]);
				}
				polygonLength += EuclideanDist(polygon[0], polygon[polygon.Count - 1]);
			}
			return polygonLength;
		}

		private Polygon optimize(Polygon polygon)
		{
			Polygon ret = new Polygon();
			for (int n = 0; n < polygon.Count; n++)
			{
				var p = polygon[n];
				if (n > 0 && n < polygon.Count - 1)
				{
					if (Math.Abs(polygon[n + 1].X - polygon[n - 1].X) / 2 == polygon[n].X &&
						Math.Abs(polygon[n + 1].Y - polygon[n - 1].Y) / 2 == polygon[n].Y)
						continue;
				}
				ret.Add(p);
			}
			return ret;
		}

		private int VisitNext(Polygon polygon, int index)
		{
			Point p1 = unused[index];
			polygon.Add(p1);
			unused.RemoveAt(index);
			// Busca dentro de LIMIT pixels de distancia al más cercano
			double min_dist = double.MaxValue;
			int min_index = -1;
			for (int n = 0; n < unused.Count; n++)
			{
				var p2 = unused[n];
				double dist = EuclideanDist(p1, p2);
				double distBands = Math.Abs(p1.X - (p2.X - (p2.Y - p1.Y)));
				if (distBands <= LIMIT && dist < min_dist && !linesIntersect(p1, p2))
				{
					min_dist = dist;
					min_index = n;
				}
			}
			if (min_index != -1)
				allLines.Add(new Line(p1, unused[min_index]));
			return min_index;
		}

		public static double EuclideanDist(Point p1, Point p2)
		{
			return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
		}

		private bool linesIntersect(Point p1, Point p2)
		{
			foreach (Line l in allLines)
			{
				if (p1 != l.P1 && p2 != l.P1
						&& p1 != l.P2 && p2 != l.P2
						&& doLinesIntersect(l, new Line(p1, p2)))
					return true;
			}
			return false;
		}
		public static bool doLinesIntersect(Line line1, Line line2)
		{
			return Intersection(line1.P1, line1.P2,
				line2.P1, line2.P2);
		}

		public static bool Intersection(PointF a1, PointF a2, PointF b1, PointF b2)
		{
			const double MyEpsilon = 0.00001;
			float ua_t = (b2.X - b1.X) * (a1.Y - b1.Y) - (b2.Y - b1.Y) * (a1.X - b1.X);
			float ub_t = (a2.X - a1.X) * (a1.Y - b1.Y) - (a2.Y - a1.Y) * (a1.X - b1.X);
			float u_b = (b2.Y - b1.Y) * (a2.X - a1.X) - (b2.X - b1.X) * (a2.Y - a1.Y);

			// Infinite lines intersect somewhere
			if (!(-MyEpsilon < u_b && u_b < MyEpsilon))   // e.g. u_b != 0.0
			{
				float ua = ua_t / u_b;
				float ub = ub_t / u_b;
				if (0.0f <= ua && ua <= 1.0f && 0.0f <= ub && ub <= 1.0f)
				{
					// Intersection
					var ret = new PointF(a1.X + ua * (a2.X - a1.X),
												a1.Y + ua * (a2.Y - a1.Y));
					if (ret.X < Math.Min(a1.X, a2.X) ||
						ret.X > Math.Max(a1.X, a2.X) ||
						ret.Y < Math.Min(a1.Y, a2.Y) ||
						ret.Y > Math.Max(a1.Y, a2.Y))
						return false;
					else
						return true;
				}
				else
				{
					// No Intersection
					return false;
				}
			}
			else // lines (not just segments) are parallel or the same line
			{
				if (a1.X >= b1.X
					&& a1.X <= b2.X &&
					(a1.X - a1.Y == b1.X - b1.Y)
					)
					return true;
				else
					return false;
			}
		}
		public static double EuclideanDist(IPoint p1, IPoint p2)
		{
			return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
		}
	}
}
