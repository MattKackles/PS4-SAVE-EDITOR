namespace PS3SaveEditor
{
	// Token: 0x0200006B RID: 107
	public partial class RestoreBackup : global::System.Windows.Forms.Form
	{
		// Token: 0x06000573 RID: 1395 RVA: 0x000218E5 File Offset: 0x0001FAE5
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00021904 File Offset: 0x0001FB04
		private void InitializeComponent()
		{
			this.pbProgress = new global::PS3SaveEditor.PS4ProgressBar();
			this.lblProgress = new global::System.Windows.Forms.Label();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.pbProgress.Location = new global::System.Drawing.Point(3, 27);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new global::System.Drawing.Size(257, 20);
			this.pbProgress.TabIndex = 0;
			this.lblProgress.AutoSize = true;
			this.lblProgress.Location = new global::System.Drawing.Point(5, 9);
			this.lblProgress.Name = "lblProgress";
			this.lblProgress.Size = new global::System.Drawing.Size(0, 13);
			this.lblProgress.TabIndex = 1;
			this.panel1.Controls.Add(this.lblProgress);
			this.panel1.Controls.Add(this.pbProgress);
			this.panel1.Location = new global::System.Drawing.Point(9, 9);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(263, 73);
			this.panel1.TabIndex = 2;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(284, 94);
			base.Controls.Add(this.panel1);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "RestoreBackup";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Hide;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Restore Backup";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x040002AC RID: 684
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040002AD RID: 685
		private global::PS3SaveEditor.PS4ProgressBar pbProgress;

		// Token: 0x040002AE RID: 686
		private global::System.Windows.Forms.Label lblProgress;

		// Token: 0x040002AF RID: 687
		private global::System.Windows.Forms.Panel panel1;
	}
}
