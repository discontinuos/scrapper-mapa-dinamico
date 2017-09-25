using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Specialized;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Net;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using NetTopologySuite.Geometries;

namespace mapScrapper
{
	public class Downloader
	{
		List<string> provincias = new List<string>();
		int mapCount = 0;

		public void getMapaRadio(RadioInfo radio, string extent, bool useHiResNames = false)
		{
			mapCount++;
			if (radio.isDone(useHiResNames))
			{
				if (mapCount % 50 == 0) Console.Write(".");
				return;
			}
			Console.WriteLine("Getting MAP " + radio.makeName() + " (" + mapCount.ToString() + ")...");
			//return;
			List<string> radios = new List<string>();
			var args = new NameValueCollection() {
						{ "la", ""}, {
					"qProvincias", radio.Prov},{
					"qDepartamentos", radio.Dpto},{
					"qFracciones", radio.Fraccion},{
					"qRadios", radio.Radio},{
					"extent", extent},{
					"old_mapa_x", ""},{
					"old_mapa_y", ""},{
					"mode", "14"},{
					"togglecapa", "i_selectores"},{
					"zsize", "1"},{
					"zoomdir", "pan"},{
					"ver_escala", "2"},{
					"escala", "26000"} };
		//extent:1688948 3601619.5 5996706 7909377.5
		// 4026334.96104 6061347.65263 4031832.35085 6066845.04243
		retry3:
			var response = Post(Context.url, args);
			if (response == null)
			{
				Context.Log("timeouts", radio);
				return;
			}
			string htmlRoot = Saver(response, radio, useHiResNames);
			var notFound = htmlRoot.IndexOf("No encontr&eacute; cobertura para");
			if (notFound > 0)
			{
				Context.Log("no_covered", radio.makeKey());
				return;
			}
			Console.WriteLine("Gotfile!");
			int i = htmlRoot.IndexOf("/temp/image");
			if (i == -1)
			{
				Context.Log("errors", "No map for radio " + radio.makeName());
				return;
			}

			int iEnd = htmlRoot.IndexOf(".gif", i);
			string gif = Context.url2 + htmlRoot.Substring(i, iEnd - i + 4);
			var argument = gif;

			byte[] response2 = null;
			using (WebDownload client = new WebDownload())
			{
				int tries = 0;
			retry2:
				try
				{
					response2 = client.DownloadData(gif);
					Thread.Sleep(1 * 1000);
				}
				catch
				{
					tries++;
					if (tries == 8)
						throw;
					Thread.Sleep(30 * 1000);
					goto retry3;
				}
			}
			GifSaver(response2, radio, useHiResNames);
			//Process.Start("iexplore.exe", argument);
			//MessageBox.Show("Traje " + radios.Count.ToString() + " radios");
		}

		public bool getFraccionInfo(FraccionInfo fraccion)
		{
			var args = new NameValueCollection() {
						{ "la", "layer_g_Provincias%7Clayer_g_Departamentos%7Clayer_g_Im%E1genes_Satelitales%7Clayer_g_Lagos_y_Lagunas%7Clayer_g_L%EDmites_Rurales%7Clayer_g_Rutas_y_Caminos%7Clayer_g_R%EDos_y_Arroyos%7Clayer_g_Fracciones_Rurales%7Clayer_g_Radios_Rurales%7Clayer_g_Localidades"}, {
					"qProvincias", fraccion.Prov},{
					"qDepartamentos", fraccion.Dpto},{
					"qFracciones", fraccion.Fraccion},{
					"extent", "3527386.625+6810267.5+3544652.625+6827533.5"},{
					"old_mapa_x", ""},{
					"old_mapa_y", ""},{
					"mode", "13"},{
					"togglecapa", "i_selectores"},{
					"zsize", "1"},{
					"zoomdir", "pan"},{
					"ver_escala", "1"},{
					"escala", "20217000"} };
			//extent:1688948 3601619.5 5996706 7909377.5

			byte[] response;
			if (fraccion.isDone())
			{
				response = File.ReadAllBytes(fraccion.getTxtName());
			}
			else
			{
				Console.WriteLine("Getting radio list " + fraccion.makeName() + "...");
				response = Post(Context.url, args);
			}
			if (response == null)
			{
				Context.Log("timeouts",fraccion);
				return false;
			}
			string htmlRoot = Saver(response, fraccion);
			// extra el extent
			string extent = getHidden(htmlRoot, "extent");
			if (extent == null)
			{
				File.Delete(fraccion.getTxtName());
				Context.Log("errors", "No data for fraccion " + fraccion.makeName());
				return false;
			}

			// lista los radios
			int i = htmlRoot.IndexOf(">Seleccione el radio");
			int iEnd = htmlRoot.IndexOf("</select>", i);
			string subText = htmlRoot.Substring(i, iEnd - i);
			string[] parts = subText.Split(new string[] { "<option value=\"" }, StringSplitOptions.None);
			
			fraccion.Extents = extent; 
			
			for (int n = 1; n < parts.Length; n++)
			{
				string p = parts[n];
				string[] tmp = p.Split(new string[] { "\"" }, StringSplitOptions.None);
				RadioInfo ri = new RadioInfo(fraccion);
				ri.Radio =tmp[0];
				fraccion.Radios.Add(ri);
			}
			return true;
		}

		private static string getHidden(string htmlRoot, string p)
		{
			int i = htmlRoot.IndexOf("<input type=\"hidden\" name=\"" + p);
			if (i == -1) return null;
			int iEnd = htmlRoot.IndexOf("value=\"", i);
			int iEndValue = htmlRoot.IndexOf("\"", iEnd + 7);

			string subText = htmlRoot.Substring(iEnd + 7, iEndValue - (iEnd + 7));
			return subText;
		}
		/*private List<string> getDepartamento(string prov, string dpto)
		{
			List<string> fracciones = new List<string>();
			var args = new NameValueCollection() {
						{ "la", "layer_g_Provincias%7Clayer_g_Departamentos%7Clayer_g_Im%E1genes_Satelitales%7Clayer_g_Lagos_y_Lagunas%7Clayer_g_L%EDmites_Rurales%7Clayer_g_Rutas_y_Caminos%7Clayer_g_R%EDos_y_Arroyos%7Clayer_g_Fracciones_Rurales%7Clayer_g_Radios_Rurales%7Clayer_g_Localidades"}, {
					"qProvincias", prov},{
					"qDepartamentos", dpto},{
					"extent", "3527386.625+6810267.5+3544652.625+6827533.5"},{
					"old_mapa_x", ""},{
					"old_mapa_y", ""},{
					"mode", "13"},{
					"togglecapa", "i_selectores"},{
					"zsize", "1"},{
					"zoomdir", "pan"},{
					"ver_escala", "1"},{
					"escala", "20217000"} };
			//extent:1688948 3601619.5 5996706 7909377.5

			string name = "prov" + prov + "-" + dpto;
			Console.WriteLine("Getting fracciones list " + name + "...");
			byte[] response;
			if (isDone(name, prov))
				response = File.ReadAllBytes(resolveFilename(name, prov));
			else
				response = Post(url, args);
			if (response == null)
			{
				string file = resolveFilename("timeouts", "");
				Console.WriteLine("Timeout");
				File.AppendAllText(file, name + Environment.NewLine);
				return new List<string>();
			}

			string htmlRoot = Saver(response, name, prov);
			int i = htmlRoot.IndexOf(">Seleccione la fracc");
			if (i == -1)
			{
				string file = resolveFilename("errors", "");
				File.AppendAllText(file, "El departamento " + dpto + " de la provincia " + prov + " no tiene localidades." + Environment.NewLine);
				return new List<string>();
			}
			int iEnd = htmlRoot.IndexOf("</select>", i);
			string subText = htmlRoot.Substring(i, iEnd - i);
			string[] parts = subText.Split(new string[] { "<option value=\"" }, StringSplitOptions.None);
			for (int n = 1; n < parts.Length; n++)
			{
				string p = parts[n];
				string[] tmp = p.Split(new string[] { "\"" }, StringSplitOptions.None);
				fracciones.Add(tmp[0]);
			}
			return fracciones;
		}

		private List<string> getProvincia(string prov)
		{
			List<string> departamentos = new List<string>();
			var args = new NameValueCollection() {
						{ "la", "layer_g_Provincias%7Clayer_g_Departamentos%7Clayer_g_Im%E1genes_Satelitales%7Clayer_g_Lagos_y_Lagunas%7Clayer_g_L%EDmites_Rurales%7Clayer_g_Rutas_y_Caminos%7Clayer_g_R%EDos_y_Arroyos%7Clayer_g_Fracciones_Rurales%7Clayer_g_Radios_Rurales%7Clayer_g_Localidades"}, {
					"qProvincias", prov},{
					"extent", "3527386.625+6810267.5+3544652.625+6827533.5"},{
					"old_mapa_x", ""},{
					"old_mapa_y", ""},{
					"mode", "10"},{
					"togglecapa", "i_selectores"},{
					"zsize", "1"},{
					"zoomdir", "pan"},{
					"ver_escala", "1"},{
					"escala", "20217000"} };
			//extent:1688948 3601619.5 5996706 7909377.5

			string name = "prov" + prov;
			Console.WriteLine("Getting departamentos list " + name + "...");
			byte[] response;
			if (isDone(name, prov))
				response = File.ReadAllBytes(resolveFilename(name, prov));
			else
				response = Post(url, args);
			if (response == null)
			{
				string file = resolveFilename("timeouts", "");
				Console.WriteLine("Timeout");
				File.AppendAllText(file, name + Environment.NewLine);
				return new List<string>();
			}
			if (response.Length == 0)
			{
				string file = resolveFilename("errors", "");
				File.AppendAllText(file, "No data for fraccion " + name + Environment.NewLine);
				return new List<string>();
			}

			string htmlRoot = Saver(response, name, prov);
			int i = htmlRoot.IndexOf(">Seleccione el departamento</option>");
			if (i == -1)
			{
				string file = resolveFilename("errors", "");
				File.AppendAllText(file, "No data for departamento " + name + Environment.NewLine);
				return new List<string>();
			}
			int iEnd = htmlRoot.IndexOf("</select>", i);
			string subText = htmlRoot.Substring(i, iEnd - i);
			string[] parts = subText.Split(new string[] { "<option value=\"" }, StringSplitOptions.None);
			for (int n = 1; n < parts.Length; n++)
			{
				string p = parts[n];
				string[] tmp = p.Split(new string[] { "\"" }, StringSplitOptions.None);
				departamentos.Add(tmp[0]);
			}
			return departamentos;
		}

		private void getProvincias()
		{
			var args = new NameValueCollection();
			provincias = new List<string>();
			var response = Post(url, args);
			string htmlRoot = Saver(response, "root");
			int i = htmlRoot.IndexOf(">Seleccione la provincia</option>");
			int iEnd = htmlRoot.IndexOf("</select>", i);
			string subText = htmlRoot.Substring(i, iEnd - i);
			string[] parts = subText.Split(new string[] { "<option value=\"" }, StringSplitOptions.None);
			for (int n = 1; n < parts.Length; n++)
			{
				string p = parts[n];
				string[] tmp = p.Split(new string[] { "\"" }, StringSplitOptions.None);
				provincias.Add(tmp[0]);
			}
		}
		*/
		private string Saver(byte[] response, GeoBase g, bool useHiResNames = false)
		{
			string path = g.getTxtName(useHiResNames);
			using (FileStream fs = new FileStream(path, FileMode.Create))
			{
				fs.Write(response, 0, response.Length);
			}
			return File.ReadAllText(path);
		}
		private void GifSaver(byte[] response, GeoBase g, bool useHiResNames = false)
		{
			string path = g.getGifName(useHiResNames);
			using (FileStream fs = new FileStream(path, FileMode.Create))
			{
				fs.Write(response, 0, response.Length);
			}
		}
		public byte[] Post(string uri, NameValueCollection pairs)
		{
			byte[] response = null;
			int tries = 0;
			using (WebDownload client = new WebDownload())
			{
			retry:
				try
				{
					response = client.UploadValues(uri, pairs);
				}
				catch (Exception ex)
				{
					if (ex.Message == "The operation has timed out")
						return null;
					tries++;
					if (tries == 1)
						return null;
					Thread.Sleep(20 * tries * 1000);
					goto retry;
				}
			}
			return response;
		}


		


		public static string ReadExtents(string filename)
		{
			// extra el extent
			string text = File.ReadAllText(filename);
			string extent = getHidden(text, "extent");
			return extent;
		}
	}
}
