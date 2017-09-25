using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mapScrapper;
using System.IO;

namespace winApp
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();
            txtFolder.Text = Context.CalculateDataDirectory();

        }

		private void btnTests_Click(object sender, EventArgs e)
		{
			frmTests f = new frmTests();
			f.ShowDialog(this);
		}

		private void btnGet_Click(object sender, EventArgs e)
		{
			Scrapper scrapper = new Scrapper();

            scrapper.ProvFilter = txtProv.Text;
            scrapper.DptoFilter = txtDepto.Text;
            scrapper.FracFilter = txtFrac.Text;
            scrapper.RadioFilter = txtRadio.Text;

            //LoadSkipRadios(scrapper);
            string file = Path.Combine(Context.InputDataDirectory, @"todos2001.dbf");

            scrapper.GetMapsFromDbfList(file);
            Console.WriteLine("Listo");
			MessageBox.Show(this, "Listo");
		}

		private static void LoadSkipRadios(Scrapper scrapper)
		{
			string skip = Path.Combine(Context.InputDataDirectory, @"radios-no-urbanos.dbf");
			scrapper.loadSkipRadios(skip);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			List<RadioInfo> radios = new List<RadioInfo>();
		
			string folderBase = Context.OutputDataDirectory;
			foreach (string folder in Directory.GetDirectories(folderBase))
			{
				string folderName = Path.GetFileName(folder);
				if (folderName != "Input")
				{
					Recognizer reco = new Recognizer();
					radios = reco.RecognizeFolder(folder);

					ShapefileMaker shaper = new ShapefileMaker();
					shaper.Create(radios, folderName, Context.Geo);
				}
			}
            Console.WriteLine("Listo");
            MessageBox.Show("Listo");
		}

		private void btnImprove_Click(object sender, EventArgs e)
		{
			Scrapper scrapper = new Scrapper();
			LoadSkipRadios(scrapper);

			string file = Path.Combine(Context.InputDataDirectory, @"todos2001.dbf");
			scrapper.GetHiResMapsFromDbfList(file, false);
            Console.WriteLine("Listo");
            MessageBox.Show(this, "Listo");
		}

		private void btnGetter_Click(object sender, EventArgs e)
		{
			frmGetter f = new frmGetter();
			f.ShowDialog(this);
		}

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            Context.DataDirectory = txtFolder.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
