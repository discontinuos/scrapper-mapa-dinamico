namespace winApp
{
	partial class Form1
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
			this.resPic = new System.Windows.Forms.PictureBox();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.txtFile = new System.Windows.Forms.TextBox();
			this.button4 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.resPic)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// resPic
			// 
			this.resPic.BackColor = System.Drawing.Color.White;
			this.resPic.Location = new System.Drawing.Point(535, 0);
			this.resPic.Name = "resPic";
			this.resPic.Size = new System.Drawing.Size(519, 640);
			this.resPic.TabIndex = 0;
			this.resPic.TabStop = false;
			// 
			// pictureBox3
			// 
			this.pictureBox3.Image = global::winApp.Properties.Resources.image1497612818466656;
			this.pictureBox3.Location = new System.Drawing.Point(-17, 12);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(529, 628);
			this.pictureBox3.TabIndex = 0;
			this.pictureBox3.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::winApp.Properties.Resources.image1497612764384771;
			this.pictureBox2.Location = new System.Drawing.Point(-54, 12);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(529, 562);
			this.pictureBox2.TabIndex = 0;
			this.pictureBox2.TabStop = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::winApp.Properties.Resources.image1497601651160063;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(529, 447);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(536, 658);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(92, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "process p1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(634, 658);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(92, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "button1";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(732, 658);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(92, 23);
			this.button3.TabIndex = 1;
			this.button3.Text = "button1";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// txtFile
			// 
			this.txtFile.Location = new System.Drawing.Point(13, 650);
			this.txtFile.Name = "txtFile";
			this.txtFile.Size = new System.Drawing.Size(275, 20);
			this.txtFile.TabIndex = 2;
			this.txtFile.Text = "D:\\Pablo\\Sociologia\\Mapa Social\\doc\\Capas externas de datos\\mapas 2001\\scrapeando" +
    "\\GifScrapper\\winApp\\bin\\Debug\\Output\\10\\prov10-007-04-03.gif";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(301, 651);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(126, 19);
			this.button4.TabIndex = 3;
			this.button4.Text = "load as P1";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1083, 747);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.txtFile);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.resPic);
			this.Controls.Add(this.pictureBox3);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.pictureBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.resPic)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox resPic;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox txtFile;
		private System.Windows.Forms.Button button4;
	}
}

