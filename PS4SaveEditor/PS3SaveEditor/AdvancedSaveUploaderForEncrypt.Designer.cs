namespace PS3SaveEditor
{
	// Token: 0x02000006 RID: 6
	public partial class AdvancedSaveUploaderForEncrypt : global::System.Windows.Forms.Form
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00006CA0 File Offset: 0x00004EA0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00006CC0 File Offset: 0x00004EC0
		private void InitializeComponent()
		{
			this.saveUploadDownloder1 = new global::PS3SaveEditor.SaveUploadDownloder();
			base.SuspendLayout();
			this.saveUploadDownloder1.Action = null;
			this.saveUploadDownloder1.BackColor = global::System.Drawing.Color.FromArgb(102, 102, 102);
			this.saveUploadDownloder1.FilePath = null;
			this.saveUploadDownloder1.Files = null;
			this.saveUploadDownloder1.Game = null;
			this.saveUploadDownloder1.IsUpload = false;
			this.saveUploadDownloder1.ListResult = null;
			this.saveUploadDownloder1.Location = new global::System.Drawing.Point(12, 12);
			this.saveUploadDownloder1.Name = "saveUploadDownloder1";
			this.saveUploadDownloder1.OrderedEntries = null;
			this.saveUploadDownloder1.OutputFolder = null;
			this.saveUploadDownloder1.Profile = null;
			this.saveUploadDownloder1.ProgressBar = null;
			this.saveUploadDownloder1.SaveId = null;
			this.saveUploadDownloder1.Size = new global::System.Drawing.Size(446, 146);
			this.saveUploadDownloder1.StatusLabel = null;
			this.saveUploadDownloder1.TabIndex = 0;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.Black;
			base.ClientSize = new global::System.Drawing.Size(472, 170);
			base.Controls.Add(this.saveUploadDownloder1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Fixed3D;
			base.Icon = global::PS3SaveEditor.Resources.Resources.ps3se;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AdvancedSaveUploaderForEncrypt";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Hide;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Save Downloader";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.AdvancedSaveUploaderForEncrypt_FormClosing);
			base.ResumeLayout(false);
		}

		// Token: 0x04000040 RID: 64
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000041 RID: 65
		private global::PS3SaveEditor.SaveUploadDownloder saveUploadDownloder1;
	}
}
