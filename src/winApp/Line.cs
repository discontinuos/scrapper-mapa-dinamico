using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace winApp
{
	public class Line
	{
		public Point P1;
		public Point P2;
		
		public Line()
		{
		}
		public Line(Point p1, Point p2)
		{
			this.P1 = p1;
			this.P2 = p2;
		}
		public override string ToString()
		{
			return P1.ToString() + "; " + P2.ToString();
		}
		public void Start(int x, int y)
		{
			P1 = new Point(x, y);
		}
		public void End(int x, int y)
		{
			P2 = new Point(x, y);
		}
	}
}
