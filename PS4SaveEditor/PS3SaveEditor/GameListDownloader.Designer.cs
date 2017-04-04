namespace PS3SaveEditor
{
	// Token: 0x02000061 RID: 97
	public partial class GameListDownloader : global::System.Windows.Forms.Form
	{
		// Token: 0x06000528 RID: 1320 RVA: 0x0001FC45 File Offset: 0x0001DE45
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001FC64 File Offset: 0x0001DE64
		private void InitializeComponent()
		{
			this.lblStatus = new global::System.Windows.Forms.Label();
			this.pbProgress = new global::PS3SaveEditor.PS4ProgressBar();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.lblStatus.AutoSize = true;
			this.lblStatus.ForeColor = global::System.Drawing.Color.White;
			this.lblStatus.Location = new global::System.Drawing.Point(11, 21);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new global::System.Drawing.Size(246, 13);
			this.lblStatus.TabIndex = 0;
			this.lblStatus.Text = "Please wait while the game list being downloaded..";
			this.pbProgress.Location = new global::System.Drawing.Point(11, 46);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new global::System.Drawing.Size(402, 19);
			this.pbProgress.TabIndex = 1;
			this.panel1.BackColor = global::System.Drawing.Color.FromArgb(102, 102, 102);
			this.panel1.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.lblStatus);
			this.panel1.Controls.Add(this.pbProgress);
			this.panel1.Location = new global::System.Drawing.Point(12, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(427, 104);
			this.panel1.TabIndex = 2;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.Black;
			base.ClientSize = new global::System.Drawing.Size(453, 130);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Fixed3D;
			base.Icon = global::PS3SaveEditor.Resources.Resources.ps3se;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "GameListDownloader";
			base.ShowInTaskbar = false;
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Downloading Games List from Server";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.GameListDownloader_FormClosing);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04000288 RID: 648
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000289 RID: 649
		private global::System.Windows.Forms.Label lblStatus;

		// Token: 0x0400028A RID: 650
		private global::PS3SaveEditor.PS4ProgressBar pbProgress;

		// Token: 0x0400028B RID: 651
		private global::System.Windows.Forms.Panel panel1;
	}
}
