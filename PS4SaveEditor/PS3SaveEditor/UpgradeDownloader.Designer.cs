namespace PS3SaveEditor
{
	// Token: 0x0200010C RID: 268
	public partial class UpgradeDownloader : global::System.Windows.Forms.Form
	{
		// Token: 0x06000B37 RID: 2871 RVA: 0x0003EB5C File Offset: 0x0003CD5C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0003EB7C File Offset: 0x0003CD7C
		private void InitializeComponent()
		{
			this.pbProgress = new global::PS3SaveEditor.PS4ProgressBar();
			this.lblStatus = new global::System.Windows.Forms.Label();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.pbProgress.Location = new global::System.Drawing.Point(8, 56);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new global::System.Drawing.Size(409, 23);
			this.pbProgress.TabIndex = 0;
			this.lblStatus.AutoSize = true;
			this.lblStatus.ForeColor = global::System.Drawing.Color.White;
			this.lblStatus.Location = new global::System.Drawing.Point(10, 39);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new global::System.Drawing.Size(143, 13);
			this.lblStatus.TabIndex = 1;
			this.lblStatus.Text = "Downloading latest version...";
			this.panel1.BackColor = global::System.Drawing.Color.FromArgb(102, 102, 102);
			this.panel1.Controls.Add(this.lblStatus);
			this.panel1.Controls.Add(this.pbProgress);
			this.panel1.Location = new global::System.Drawing.Point(10, 10);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(432, 131);
			this.panel1.TabIndex = 2;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.Black;
			base.ClientSize = new global::System.Drawing.Size(452, 155);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Fixed3D;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "UpgradeDownloader";
			base.ShowIcon = false;
			this.Text = "Downloading Latest Version";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x040005C2 RID: 1474
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040005C3 RID: 1475
		private global::PS3SaveEditor.PS4ProgressBar pbProgress;

		// Token: 0x040005C4 RID: 1476
		private global::System.Windows.Forms.Label lblStatus;

		// Token: 0x040005C5 RID: 1477
		private global::System.Windows.Forms.Panel panel1;
	}
}
