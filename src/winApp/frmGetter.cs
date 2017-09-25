using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Reflection;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Threading;
using mapScrapper;

namespace winApp
{
	public partial class frmGetter : Form
	{
		public frmGetter()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (pictureBox1.Image != null)
				pictureBox1.Image.Dispose();
			pictureBox1.Image = null;
			Downloader d = new Downloader();

			var r = RadioInfo.ParseRedcode(textBox1.Text);
			r.SaveToTmp = true;

			FraccionInfo fraccion = new FraccionInfo(r.Prov, r.Dpto, r.Fraccion);
			if (d.getFraccionInfo(fraccion))
			{
				d.getMapaRadio(r, fraccion.Extents);
				pictureBox1.Image = Bitmap.FromFile(r.getGifName());
			}
		}


	}
}
