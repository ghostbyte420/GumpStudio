using System;
using System.Reflection;
using System.Windows.Forms;

using GumpStudio.Properties;

namespace GumpStudio.Forms
{
	[System.ComponentModel.DesignerCategory("")]
	public class AboutBox : Form
	{
		private Label Label1;
		private LinkLabel lblHomepage;
		private Label lblVersion;
		private PictureBox PictureBox1;
		private FlowLayoutPanel flowLayoutPanel1;
		private TextBox txtAbout;

		public AboutBox()
		{
			Load += frmAboutBox_Load;
			InitializeComponent();
		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void frmAboutBox_Load(object sender, EventArgs e)
		{
			lblVersion.Text = Resources.Core_Version__ + Assembly.GetExecutingAssembly().GetName().Version;
		}

		private void InitializeComponent()
		{
			var resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
			PictureBox1 = new PictureBox();
			txtAbout = new TextBox();
			Label1 = new Label();
			lblVersion = new Label();
			lblHomepage = new LinkLabel();
			flowLayoutPanel1 = new FlowLayoutPanel();
			((System.ComponentModel.ISupportInitialize)PictureBox1).BeginInit();
			flowLayoutPanel1.SuspendLayout();
			SuspendLayout();
			// 
			// PictureBox1
			// 
			PictureBox1.Dock = DockStyle.Top;
			PictureBox1.Image = (System.Drawing.Image)resources.GetObject("PictureBox1.Image");
			PictureBox1.Location = new System.Drawing.Point(0, 0);
			PictureBox1.Name = "PictureBox1";
			PictureBox1.Size = new System.Drawing.Size(454, 158);
			PictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
			PictureBox1.TabIndex = 0;
			PictureBox1.TabStop = false;
			// 
			// txtAbout
			// 
			txtAbout.BorderStyle = BorderStyle.FixedSingle;
			txtAbout.Dock = DockStyle.Fill;
			txtAbout.Location = new System.Drawing.Point(0, 158);
			txtAbout.Multiline = true;
			txtAbout.Name = "txtAbout";
			txtAbout.ReadOnly = true;
			txtAbout.ScrollBars = ScrollBars.Vertical;
			txtAbout.Size = new System.Drawing.Size(454, 152);
			txtAbout.TabIndex = 1;
			txtAbout.TabStop = false;
			txtAbout.Text = "           ** Gump Studio was written by Bradley Uffner in January of 2003 **\r\n          =============================================";
			// 
			// Label1
			// 
			Label1.Anchor = AnchorStyles.None;
			Label1.AutoSize = true;
			Label1.Location = new System.Drawing.Point(6, 3);
			Label1.Name = "Label1";
			Label1.Size = new System.Drawing.Size(131, 15);
			Label1.TabIndex = 3;
			Label1.Text = "(C) Bradley Uffner, 2003";
			// 
			// lblVersion
			// 
			lblVersion.AutoSize = true;
			lblVersion.Location = new System.Drawing.Point(149, 3);
			lblVersion.Name = "lblVersion";
			lblVersion.Size = new System.Drawing.Size(45, 15);
			lblVersion.TabIndex = 4;
			lblVersion.Text = "Version";
			// 
			// lblHomepage
			// 
			lblHomepage.Anchor = AnchorStyles.None;
			lblHomepage.AutoSize = true;
			lblHomepage.Location = new System.Drawing.Point(143, 3);
			lblHomepage.Name = "lblHomepage";
			lblHomepage.Size = new System.Drawing.Size(0, 15);
			lblHomepage.TabIndex = 5;
			// 
			// flowLayoutPanel1
			// 
			flowLayoutPanel1.AutoSize = true;
			flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowLayoutPanel1.Controls.Add(Label1);
			flowLayoutPanel1.Controls.Add(lblHomepage);
			flowLayoutPanel1.Controls.Add(lblVersion);
			flowLayoutPanel1.Dock = DockStyle.Bottom;
			flowLayoutPanel1.Location = new System.Drawing.Point(0, 310);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Padding = new Padding(3);
			flowLayoutPanel1.Size = new System.Drawing.Size(454, 21);
			flowLayoutPanel1.TabIndex = 6;
			flowLayoutPanel1.WrapContents = false;
			// 
			// AboutBox
			// 
			AutoScaleMode = AutoScaleMode.None;
			AutoSize = true;
			ClientSize = new System.Drawing.Size(454, 331);
			Controls.Add(txtAbout);
			Controls.Add(flowLayoutPanel1);
			Controls.Add(PictureBox1);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			Name = "AboutBox";
			Text = "About Gump Studio.NET";
			Load += frmAboutBox_Load;
			((System.ComponentModel.ISupportInitialize)PictureBox1).EndInit();
			flowLayoutPanel1.ResumeLayout(false);
			flowLayoutPanel1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();

		}

		private void lblHomepage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			/*Process.Start(new ProcessStartInfo
			{
				UseShellExecute = true,
				FileName = "http://www.orbsydia.net"
			});*/
		}

		private const string _Text = @"________________________________________________________________________________
Gump Studio was originally designed and written by Bradley Uffner in 2003
==================================================

[★]_ UI artwork by Melanius.
[★]_ Ultima SDK by Krrios.
[★]_ UOFonts by DarkStorm.

---------------------------------------------------------
Update to .NET 10.0 by the uoAvox development team
---------------------------------------------------------
";
		public void SetText(string text)
		{
			txtAbout.Text = $"{_Text}{Environment.NewLine}{Environment.NewLine}==============Plugin Development Information=============={Environment.NewLine}{Environment.NewLine}" + text;
		}
	}
}