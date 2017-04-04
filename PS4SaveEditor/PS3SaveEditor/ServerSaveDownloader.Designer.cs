namespace PS3SaveEditor
{
	// Token: 0x02000020 RID: 32
	public partial class ServerSaveDownloader : global::System.Windows.Forms.Form
	{
		// Token: 0x06000221 RID: 545 RVA: 0x0000D4E2 File Offset: 0x0000B6E2
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000D504 File Offset: 0x0000B704
		private void InitializeComponent()
		{
			this.saveUploadDownloder1 = new global::PS3SaveEditor.SaveUploadDownloder();
			base.SuspendLayout();
			this.saveUploadDownloder1.Action = null;
			this.saveUploadDownloder1.BackColor = global::System.Drawing.Color.FromArgb(200, 100, 10);
			this.saveUploadDownloder1.FilePath = null;
			this.saveUploadDownloder1.Files = null;
			this.saveUploadDownloder1.Game = null;
			this.saveUploadDownloder1.IsUpload = false;
			this.saveUploadDownloder1.Location = new global::System.Drawing.Point(13, 12);
			this.saveUploadDownloder1.Name = "saveUploadDownloder1";
			this.saveUploadDownloder1.OutputFolder = null;
			this.saveUploadDownloder1.Profile = null;
			this.saveUploadDownloder1.Size = new global::System.Drawing.Size(446, 113);
			this.saveUploadDownloder1.TabIndex = 0;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(473, 138);
			base.Controls.Add(this.saveUploadDownloder1);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ServerSaveDownloader";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			this.Text = "ServerSaveDownloader";
			base.ResumeLayout(false);
		}

		// Token: 0x040000C2 RID: 194
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040000C3 RID: 195
		private global::PS3SaveEditor.SaveUploadDownloder saveUploadDownloder1;
	}
}
