namespace PS3SaveEditor
{
	// Token: 0x02000107 RID: 263
	public partial class SimpleSaveUploader : global::System.Windows.Forms.Form
	{
		// Token: 0x06000B0D RID: 2829 RVA: 0x0003E2F6 File Offset: 0x0003C4F6
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0003E318 File Offset: 0x0003C518
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
			this.saveUploadDownloder1.SaveId = null;
			this.saveUploadDownloder1.Size = new global::System.Drawing.Size(446, 146);
			this.saveUploadDownloder1.TabIndex = 0;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.Black;
			base.ClientSize = new global::System.Drawing.Size(472, 170);
			base.Controls.Add(this.saveUploadDownloder1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Fixed3D;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SimpleSaveUploader";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Simple Save Patcher";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.SimpleSaveUploader_FormClosing);
			base.ResumeLayout(false);
		}

		// Token: 0x040005AF RID: 1455
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040005B0 RID: 1456
		private global::PS3SaveEditor.SaveUploadDownloder saveUploadDownloder1;
	}
}
