using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace winApp
{
	public class LineFinder
	{
		const int MERGE_LIMIT = 20;

		public int width;
		public int height;
		List<Line> Lines;
		
		bool[] scannedPixels;
		Bitmap b;

		public List<Line> ScanBitmap(Bitmap bit)
		{
			b = bit;
			Lines = new List<Line>();
			width = b.Width;
			height = b.Height;
			scannedPixels = new bool[width * height];

			for(int x = 0; x < width; x++)
				for(int y = 0; y < height; y++)
				{
					if (!isUsed(x,y) && isRed(x, y) && isRed(x+1, y+1))
					{
						this.Lines.Add(downTrace(x,y));
					}
				}

			while(mergeNearest());

			while (mergeOverlap()) ;

			return Lines;
		}

		private bool mergeNearest()
		{
			for (int n = 0; n < Lines.Count; n++)
			{
				Line l1 = Lines[n];
				for (int i = 0; i < Lines.Count; i++)
				{
					if (i != n)
					{
						Line l2 = Lines[i];
						var dx = l2.P1.X - l1.P2.X;
						var dy = l2.P1.Y - l1.P2.Y;
						if (dx > 0 && dx == dy && dx < MERGE_LIMIT)
						{
							if (!pathIsWhite(l1.P2, l2.P1))
							{
								Lines.RemoveAt(Math.Max(i, n));
								Lines.RemoveAt(Math.Min(i, n));
								Lines.Add(new Line(l1.P1, l2.P2));
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		private bool mergeOverlap()
		{
			for (int n = 0; n < Lines.Count; n++)
			{
				Line l1 = Lines[n];
				for (int i = 0; i < Lines.Count; i++)
				{
					if (i != n)
					{
						Line l2 = Lines[i];
						if(ShapeMaker.doLinesIntersect(l1, l2))
						{	
								Lines.RemoveAt(Math.Max(i, n));
								Lines.RemoveAt(Math.Min(i, n));
							if (l1.P1.X < l2.P1.X)
								Lines.Add(new Line(l1.P1, l2.P2));
							else
								Lines.Add(new Line(l2.P1, (l1.P2.X < l2.P2.X ? l2.P2 : l1.P2)));
								return true;
						}
					}
				}
			}
			return false;
		}

		private bool pathIsWhite(Point p1, Point p2)
		{
			int whites = 0;
			int size = p2.X - p1.X;
			for (int n = 1; n < size; n++)
			{
				if (isWhite(p1.X + n, p2.Y + n))
					whites++;
				if (isBlack(p1.X + n, p2.Y + n))
					whites++;
			}
			if (size < 6)
				return (whites > size * .7F);
			else
				return (whites > size *.9F);

		}

		private bool isWhite(int x, int y)
		{
			return checkColor(x, y, Color.White);
		}

		private bool isBlack(int x, int y)
		{
			return checkColor(x, y, Color.Black);
		}

		private bool isRed(int x, int y)
		{
			return checkColor(x, y, Color.Red);
		}

		private bool checkColor(int x, int y, Color c)
		{
			if (x < 0 || x >= width) return false;
			if (y < 0 || y >= height) return false;
			return equalColor(b.GetPixel(x, y), c);
		}


		private bool equalColor(Color color, Color color_2)
		{
			return color.R == color_2.R && color.G == color_2.G && color.B == color_2.B;
		}


		Line downTrace(int x, int y)
		{
			Line l = new Line();
			if (isBlack(x - 1, y - 1))
				l.Start(x - 1, y - 1);
			else
				l.Start(x, y);
			downAdd(l, x, y);
			return l;
		}
		void downAdd(Line l, int x, int y)
		{
			l.End(x, y);
			markAsUsed(x, y);
			if (isRed(x + 1, y + 1))
				downAdd(l, x + 1, y + 1);
			else if (isBlack(x + 1, y + 1))
				l.End(x + 1, y + 1);
		}

		private bool isUsed(int x, int y)
		{
			return scannedPixels[x * width + y];
		}

		private void markAsUsed(int x, int y)
		{
			scannedPixels[x * width + y] = true;
		}
	}
}
