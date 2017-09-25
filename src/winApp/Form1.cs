using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace winApp
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
	
		}

		private void draw(List<Line> lines, List<Point> polygon)
		{
			Bitmap b = new Bitmap(resPic.ClientRectangle.Width, resPic.ClientRectangle.Height);
			using (Graphics g = Graphics.FromImage(b))
			{
				Pen[] pens = new Pen[] { Pens.Red, Pens.Violet, Pens.Gray, Pens.Green };
				foreach (Line l in lines)
				{
					g.DrawLine(pens[new Random().Next(pens.Length)], l.P1, l.P2);
					g.DrawLine(Pens.Black, l.P1.X, l.P1.Y, l.P1.X+1, l.P1.Y);
					g.DrawLine(Pens.Magenta, l.P2.X, l.P2.Y, l.P2.X - 1, l.P2.Y);
				}
				g.DrawPolygon(Pens.Black, polygon.ToArray());
			}
			resPic.Image = b;
		}


		private void ProcessImage(PictureBox p)
		{
			pictureBox1.Visible = false;
			pictureBox3.Visible = false;
			pictureBox2.Visible = false;
			p.Visible = true;

			var f = new LineFinder();
			var lines = f.ScanBitmap(p.Image as Bitmap);

			var s = new ShapeMaker();
			List<Point> maxPolygon = new List<Point>();
			for (int n = 0; n < lines.Count * 2; n++)
			{
				var polygon = s.FindContour(lines, n);
				if (polygon.Count > maxPolygon.Count)
					maxPolygon = polygon;
				if (n == 10)
					break;
			}

			draw(lines, maxPolygon);
		}
		private void Proyectar()
		{
			/*var gf = new GeometryFactory(new PrecisionModel(), 22183);
			IGeometry g = gf.CreateLineString(new Coordinate[] { new Coordinate(4196789.4748 ,6147053.78931),
																				new Coordinate(4197763.83282 ,6148028.14733)});
			var res = Projections.UnprojectFromPosgar3(g);
			*/
			
		}

		private void button1_Click(object sender, EventArgs e)
		{
			ProcessImage(pictureBox1);
		}
		private void button2_Click(object sender, EventArgs e)
		{
			ProcessImage(pictureBox2);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			ProcessImage(pictureBox3);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			pictureBox1.Image = Image.FromFile(txtFile.Text);
		}
	}
}
