using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace winApp
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			frmMain main = new frmMain();
			main.ShowDialog();
		}
	}
}
