namespace DGDev
{
	// Token: 0x02000109 RID: 265
	public partial class SplashScreen : global::System.Windows.Forms.Form
	{
		// Token: 0x06000B27 RID: 2855 RVA: 0x0003E7C8 File Offset: 0x0003C9C8
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.ProgramInfoLabel = new global::System.Windows.Forms.Label();
			this.StatusLabel = new global::System.Windows.Forms.Label();
			this.SplashTimer = new global::System.Windows.Forms.Timer(this.components);
			this.SplashTimer.Tick += new global::System.EventHandler(this.SplashTimer_Tick);
			base.SuspendLayout();
			this.ProgramInfoLabel.BackColor = global::System.Drawing.Color.Transparent;
			this.ProgramInfoLabel.Location = new global::System.Drawing.Point(56, 52);
			this.ProgramInfoLabel.Name = "ProgramInfoLabel";
			this.ProgramInfoLabel.Size = new global::System.Drawing.Size(100, 23);
			this.ProgramInfoLabel.TabIndex = 0;
			this.StatusLabel.BackColor = global::System.Drawing.Color.Transparent;
			this.StatusLabel.Location = new global::System.Drawing.Point(59, 135);
			this.StatusLabel.Name = "StatusLabel";
			this.StatusLabel.Size = new global::System.Drawing.Size(100, 23);
			this.StatusLabel.TabIndex = 1;
			base.ClientSize = new global::System.Drawing.Size(292, 273);
			base.Controls.Add(this.StatusLabel);
			base.Controls.Add(this.ProgramInfoLabel);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
			base.Name = "SplashScreen";
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			base.Load += new global::System.EventHandler(this.SplashScreen_Load);
			base.ResumeLayout(false);
		}

		// Token: 0x040005BC RID: 1468
		private global::System.Windows.Forms.Label ProgramInfoLabel;

		// Token: 0x040005BD RID: 1469
		private global::System.Windows.Forms.Label StatusLabel;

		// Token: 0x040005BE RID: 1470
		private global::System.Windows.Forms.Timer SplashTimer;

		// Token: 0x040005BF RID: 1471
		private global::System.ComponentModel.IContainer components;
	}
}
