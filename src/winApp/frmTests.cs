using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using mapScrapper;

namespace winApp
{
	public partial class frmTests : Form
	{
		public frmTests()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
	
		}

		private void draw(List<Line> lines, List<Polygon> polygons)
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
				if (chBorder.Checked)
				{
					foreach (var p in polygons)
						g.DrawPolygon(Pens.Black, p.ToArray());
				}
			}
			resPic.Image = b;
		}


		private void ProcessImage(PictureBox p)
		{
			pictureBox1.Visible = false;
			pictureBox3.Visible = false;
			pictureBox2.Visible = false;
			p.Visible = true;
			Recognizer r = new Recognizer();
			List<Line> lines;
			var polygon = r.RecognizeImagePolygon(p.Image, out lines);

			draw(lines, polygon);
			if (polygon.Count > 1) MessageBox.Show(this, "count", polygon.Count.ToString());
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
			pictureBox1.Image = Image.FromFile(txtFile.Text.Replace("\"", ""));
			pictureBox1.BringToFront();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			
		}

		private string to2s(int n)
		{
			if (n < 10)
				return "0" + n.ToString();
			else
				return n.ToString();
		}

		private void button5_Click_1(object sender, EventArgs e)
		{
			ProcessRedcode(txtRedcode.Text);
		}

		private void ProcessRedcode(string redcode)
		{
			RadioInfo r = RadioInfo.ParseRedcode(redcode);
			bool hiRes = (File.Exists(r.getGifName(true)));
			pictureBox1.Image = Image.FromFile(r.getGifName(hiRes));
			pictureBox1.BringToFront();
			ProcessImage(pictureBox1);
		}

		List<RadioInfo> radiosFolder = new List<RadioInfo>();
		int nRadiosFolder = 0;
		private void cmdFolder_Click(object sender, EventArgs e)
		{
			radiosFolder = Recognizer.ReadRadiosFromFolder(txtFolder.Text);
			nRadiosFolder = 0;
			UpdateRadiosFolder();
		}

		private void cmdNext_Click(object sender, EventArgs e)
		{
			if (nRadiosFolder < radiosFolder.Count - 1)
				nRadiosFolder++;
			UpdateRadiosFolder();
		}

		private void cmdPrev_Click(object sender, EventArgs e)
		{
			if (nRadiosFolder > 0)
				nRadiosFolder--;
			UpdateRadiosFolder();
		}

		private void UpdateRadiosFolder()
		{
			txtRedcodeFolder.Text = radiosFolder[nRadiosFolder].Redcode;
			ProcessRedcode(txtRedcodeFolder.Text);
			cmdPrev.Enabled = nRadiosFolder > 0;
			cmdNext.Enabled = (nRadiosFolder < radiosFolder.Count - 1);
		}
	}
}
