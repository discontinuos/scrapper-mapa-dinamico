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

namespace winApp
{
	public class MapsDownloader
	{
		string url = "http://www.sig.indec.gov.ar/index.php";
		string url2 = "http://www.sig.indec.gov.ar";
		List<string> provincias = new List<string>();
		int mapCount = 0;

		Dictionary<string, bool> skipRadios = new Dictionary<string, bool>();
		public void loadSkipRadios(string file)
		{
			Console.WriteLine("Cargando radios conocidos...");
			List<Feature> features = ReadShapefile(file);
			skipRadios.Clear();
			foreach (var f in features)
			{
				string prov = f.Attributes["PROV"] as string;
				string dpto = f.Attributes["DEPTO"] as string;
				string frac = f.Attributes["FRAC"] as string;
				string rad = f.Attributes["RADIO"] as string;
				skipRadios[makeKey(prov, dpto, frac, rad)] = true;
			}
		}
		public void getFromShapeFile(string file)
		{
			List<Feature> features = ReadShapefile(file);
			foreach (var f in features)
			{
				string prov = f.Attributes["PROV"] as string;
				string dpto = f.Attributes["DEPTO"] as string;
				string frac = f.Attributes["FRAC"] as string;
				//if (prov != "06")
				{
					getDepartamentos(prov);
					if (getFracciones(prov, dpto).Count() == 0)
						continue;
					var radios = getRadios(prov, dpto, frac);
					foreach (var rad in radios)
					{
						if (skipRadios.ContainsKey(makeKey(prov, dpto, frac, rad.Key)) == false)
							getMapaRadio(prov, dpto, frac, rad.Key, rad.Value);
					}
				}
			}
		}

		private string makeKey(string prov, string dpto, string frac, string rad)
		{
			return prov + "-" + dpto + "-" + frac + "-" + rad;
		}
		private List<Feature> ReadShapefile(string shpFilename)
		{
			var features = new List<Feature>();
			using (ShapefileDataReader dr = new ShapefileDataReader(shpFilename, new GeometryFactory()))
			{
				DbaseFileHeader header = dr.DbaseHeader;
				while (dr.Read())
				{
					AttributesTable attributesTable = new AttributesTable();
					for (int i = 0; i < header.NumFields; i++)
						attributesTable.AddAttribute(header.Fields[i].Name, dr.GetValue(i));

					features.Add(new Feature(dr.Geometry, attributesTable));
				}
			}
			return features;
		}

		public void startGetting()
		{
			getProvincias();
			foreach (string prov in provincias)
			{
				var dptos = getDepartamentos(prov);
				foreach (string dpto in dptos)
				{
					var fracciones = getFracciones(prov, dpto);
					foreach (string frac in fracciones)
					{
						var radios = getRadios(prov, dpto, frac);
						foreach (var rad in radios)
						{
							getMapaRadio(prov, dpto, frac, rad.Key, rad.Value);

						}
					}
				}
			}
		}

		/*		private List<string> getMapaRadio(string prov, string dpto, string frac, string radio, string extent)
				{
					List<string> radios = new List<string>();
					var args = new NameValueCollection() {
								{ "la", ""}, {
							"qProvincias", prov},{
							"qDepartamentos", dpto},{
							"qFracciones", frac},{
							"qRadios", radio},{
							"extent", extent},{
							"old_mapa_x", ""},{
							"old_mapa_y", ""},{
							"mode", "9"},{
							"layer_g_Provincias", "on"},					{
							"togglecapa", "i_consultas"},{
							"zsize", "1"},{
							"zoomdir", "eco"},{
							"ver_escala", "1"},{
							"escala", "26000"} };
					//extent:1688948 3601619.5 5996706 7909377.5
					// 4026334.96104 6061347.65263 4031832.35085 6066845.04243

					var response = Post(url, args);
					string name = "prov" + prov + "-" + dpto + "-" + frac + "-" + radio;
					string htmlRoot = Saver(response, name, prov);
					int i = htmlRoot.IndexOf("/temp/image");
					int iEnd = htmlRoot.IndexOf(".gif", i);
					string gif = url2 + htmlRoot.Substring(i, iEnd - i + 4);
					var argument = gif;
			
					byte[] response2 = null;
					using (WebDownload client = new WebDownload())
					{
						int tries = 0;
					retry2:
						try
						{
							response2 = client.DownloadData(gif);
						}
						catch
						{
							tries++;
							if (tries == 4)
								throw;
							Thread.Sleep(20 * 1000);
							goto retry2;
						}
					}
					Saver(response2, name + ".gif", prov);
					//Process.Start("iexplore.exe", argument);
					//MessageBox.Show("Traje " + radios.Count.ToString() + " radios");
					Thread.Sleep(5 * 1000);
					return radios;
				}
	
		 */
		private void getMapaRadio(string prov, string dpto, string frac, string radio, string extent)
		{
			string name = "prov" + prov + "-" + dpto + "-" + frac + "-" + radio;
			mapCount++;
			Console.WriteLine("Getting MAP " + name + " (" + mapCount.ToString() + ")...");
			if (isDone(name + ".gif", prov)) return;
			//return;
			List<string> radios = new List<string>();
			var args = new NameValueCollection() {
						{ "la", ""}, {
					"qProvincias", prov},{
					"qDepartamentos", dpto},{
					"qFracciones", frac},{
					"qRadios", radio},{
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
			var response = Post(url, args);
			if (response == null)
				{
					string file = resolveFilename("timeouts", "");
					Console.WriteLine("Timeout");
					File.AppendAllText(file, name + Environment.NewLine);
					return;
				}
			string htmlRoot = Saver(response, name, prov);
			var notFound = htmlRoot.IndexOf("No encontr&eacute; cobertura para");
			if (notFound > 0)
			{
				Console.WriteLine("Error");
				string file = resolveFilename("errors", "");
				File.AppendAllText(file, htmlRoot.Substring(notFound) + Environment.NewLine);
				return;
			}
			Console.WriteLine("Gotfile!");
			int i = htmlRoot.IndexOf("/temp/image");
			int iEnd = htmlRoot.IndexOf(".gif", i);
			string gif = url2 + htmlRoot.Substring(i, iEnd - i + 4);
			var argument = gif;

			byte[] response2 = null;
			using (WebDownload client = new WebDownload())
			{
				int tries = 0;
			retry2:
				try
				{
					response2 = client.DownloadData(gif);
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
			Saver(response2, name + ".gif", prov);
			//Process.Start("iexplore.exe", argument);
			//MessageBox.Show("Traje " + radios.Count.ToString() + " radios");
			Thread.Sleep(1 * 1000);
		}

		private bool isDone(string name, string subFolder)
		{
			string path = resolveFilename(name, subFolder);
			return File.Exists(path);
		}

		private bool isDone(string name)
		{
			throw new NotImplementedException();
		}

		private List<KeyValuePair<string, string>> getRadios(string prov, string dpto, string frac)
		{
			List<KeyValuePair<string, string>> radios = new List<KeyValuePair<string, string>>();
			var args = new NameValueCollection() {
						{ "la", "layer_g_Provincias%7Clayer_g_Departamentos%7Clayer_g_Im%E1genes_Satelitales%7Clayer_g_Lagos_y_Lagunas%7Clayer_g_L%EDmites_Rurales%7Clayer_g_Rutas_y_Caminos%7Clayer_g_R%EDos_y_Arroyos%7Clayer_g_Fracciones_Rurales%7Clayer_g_Radios_Rurales%7Clayer_g_Localidades"}, {
					"qProvincias", prov},{
					"qDepartamentos", dpto},{
					"qFracciones", frac},{
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
			string name = "prov" + prov + "-" + dpto + "-" + frac;
			
			Console.WriteLine("Getting radio list " + name + "...");

			byte[] response;
			if (isDone(name, prov))
				response = File.ReadAllBytes(resolveFilename(name, prov));
			else
				response = Post(url, args);
			if (response == null)
			{
				Console.WriteLine("Timeout");
				string file = resolveFilename("timeouts", "");
				File.AppendAllText(file, name + Environment.NewLine);
				return new List<KeyValuePair<string, string>>();
			}
			string htmlRoot = Saver(response, name, prov);
			// extra el extent
			string extent = getHidden(htmlRoot, "extent");
			if (extent == null)
			{
				string file = resolveFilename("errors", "");
				File.AppendAllText(file, "No data for fraccion " + name + Environment.NewLine);
				return new List<KeyValuePair<string,string>>();
			}

			// lista los radios
			int i = htmlRoot.IndexOf(">Seleccione el radio");
			int iEnd = htmlRoot.IndexOf("</select>", i);
			string subText = htmlRoot.Substring(i, iEnd - i);
			string[] parts = subText.Split(new string[] { "<option value=\"" }, StringSplitOptions.None);
			for (int n = 1; n < parts.Length; n++)
			{
				string p = parts[n];
				string[] tmp = p.Split(new string[] { "\"" }, StringSplitOptions.None);
				radios.Add(new KeyValuePair<string, string>(tmp[0], extent));
			}
			return radios;
		}

		private string getHidden(string htmlRoot, string p)
		{
			int i = htmlRoot.IndexOf("<input type=\"hidden\" name=\"" + p);
			if (i == -1) return null;
			int iEnd = htmlRoot.IndexOf("value=\"", i);
			int iEndValue = htmlRoot.IndexOf("\"", iEnd + 7);

			string subText = htmlRoot.Substring(iEnd + 7, iEndValue - (iEnd + 7));
			return subText;
		}
		private List<string> getFracciones(string prov, string dpto)
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

		private List<string> getDepartamentos(string prov)
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

		private string Saver(byte[] response, string name, string subFolder = "")
		{
			string path = resolveFilename(name, subFolder);
			using (FileStream fs = new FileStream(path, FileMode.Create))
			{
				fs.Write(response, 0, response.Length);
			}
			return File.ReadAllText(path);
		}

		private static string resolveFilename(string name, string subFolder)
		{
			string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Output";
			if (subFolder != "") dir += "\\" + subFolder;
			if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
			string path = Path.Combine(dir,
								name);
			if (path.EndsWith(".gif") == false)
				path += ".txt";
			return path;
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
				catch(Exception ex)
				{
					if (ex.Message == "The operation has timed out")
						return null;
					tries++;
					System.Windows.Forms.Application.DoEvents();
					if (tries == 12)
						throw;
					Thread.Sleep(20 * tries * 1000);
					System.Windows.Forms.Application.DoEvents();
					goto retry;
				}
			}
			return response;
		}

		private void frmGetter_Load(object sender, EventArgs e)
		{
			startGetting();
		}

	}
}
