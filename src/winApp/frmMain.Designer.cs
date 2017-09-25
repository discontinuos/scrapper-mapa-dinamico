namespace winApp
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnGet = new System.Windows.Forms.Button();
            this.btnImprove = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnTests = new System.Windows.Forms.Button();
            this.btnGetter = new System.Windows.Forms.Button();
            this.txtProv = new System.Windows.Forms.TextBox();
            this.txtDepto = new System.Windows.Forms.TextBox();
            this.txtFrac = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRadio = new System.Windows.Forms.TextBox();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(234, 28);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(110, 36);
            this.btnGet.TabIndex = 8;
            this.btnGet.Text = "Descargar";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnImprove
            // 
            this.btnImprove.Location = new System.Drawing.Point(234, 70);
            this.btnImprove.Name = "btnImprove";
            this.btnImprove.Size = new System.Drawing.Size(110, 36);
            this.btnImprove.TabIndex = 9;
            this.btnImprove.Text = "Descargar 2nd-pass";
            this.btnImprove.UseVisualStyleBackColor = true;
            this.btnImprove.Click += new System.EventHandler(this.btnImprove_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(234, 112);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(110, 36);
            this.button3.TabIndex = 10;
            this.button3.Text = "Crear shapefiles";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnTests
            // 
            this.btnTests.Location = new System.Drawing.Point(40, 241);
            this.btnTests.Name = "btnTests";
            this.btnTests.Size = new System.Drawing.Size(110, 20);
            this.btnTests.TabIndex = 13;
            this.btnTests.Text = "Pruebas visuales";
            this.btnTests.UseVisualStyleBackColor = true;
            this.btnTests.Click += new System.EventHandler(this.btnTests_Click);
            // 
            // btnGetter
            // 
            this.btnGetter.Location = new System.Drawing.Point(156, 241);
            this.btnGetter.Name = "btnGetter";
            this.btnGetter.Size = new System.Drawing.Size(110, 20);
            this.btnGetter.TabIndex = 14;
            this.btnGetter.Text = "Pruebas descarga";
            this.btnGetter.UseVisualStyleBackColor = true;
            this.btnGetter.Click += new System.EventHandler(this.btnGetter_Click);
            // 
            // txtProv
            // 
            this.txtProv.Location = new System.Drawing.Point(137, 37);
            this.txtProv.Name = "txtProv";
            this.txtProv.Size = new System.Drawing.Size(50, 20);
            this.txtProv.TabIndex = 1;
            this.txtProv.Text = "06";
            // 
            // txtDepto
            // 
            this.txtDepto.Location = new System.Drawing.Point(137, 63);
            this.txtDepto.Name = "txtDepto";
            this.txtDepto.Size = new System.Drawing.Size(50, 20);
            this.txtDepto.TabIndex = 3;
            this.txtDepto.Text = "581";
            // 
            // txtFrac
            // 
            this.txtFrac.Location = new System.Drawing.Point(137, 89);
            this.txtFrac.Name = "txtFrac";
            this.txtFrac.Size = new System.Drawing.Size(50, 20);
            this.txtFrac.TabIndex = 5;
            this.txtFrac.Text = "01";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Provincia:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Departamento:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Fracción:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Radio:";
            // 
            // txtRadio
            // 
            this.txtRadio.Location = new System.Drawing.Point(137, 117);
            this.txtRadio.Name = "txtRadio";
            this.txtRadio.Size = new System.Drawing.Size(50, 20);
            this.txtRadio.TabIndex = 7;
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(150, 171);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(304, 20);
            this.txtFolder.TabIndex = 12;
            this.txtFolder.TextChanged += new System.EventHandler(this.txtFolder_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Carpeta de trabajo:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(350, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(141, 27);
            this.label6.TabIndex = 0;
            this.label6.Text = "Si se interrumpe, continúa desde donde estaba...";
            this.label6.Click += new System.EventHandler(this.label1_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(350, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(141, 27);
            this.label7.TabIndex = 0;
            this.label7.Text = "Elegir una vez completada la primera pasada.";
            this.label7.Click += new System.EventHandler(this.label1_Click);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(350, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 27);
            this.label8.TabIndex = 0;
            this.label8.Text = "Reconoce lo ya descargado y genera...";
            this.label8.Click += new System.EventHandler(this.label1_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 273);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRadio);
            this.Controls.Add(this.txtFrac);
            this.Controls.Add(this.txtDepto);
            this.Controls.Add(this.txtProv);
            this.Controls.Add(this.btnGetter);
            this.Controls.Add(this.btnTests);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnImprove);
            this.Controls.Add(this.btnGet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "MapScrapper";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnGet;
		private System.Windows.Forms.Button btnImprove;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button btnTests;
		private System.Windows.Forms.Button btnGetter;
        private System.Windows.Forms.TextBox txtProv;
        private System.Windows.Forms.TextBox txtDepto;
        private System.Windows.Forms.TextBox txtFrac;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRadio;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}