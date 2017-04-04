namespace PS3SaveEditor
{
	// Token: 0x02000002 RID: 2
	internal partial class AboutBox1 : global::System.Windows.Forms.Form
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002249 File Offset: 0x00000449
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002268 File Offset: 0x00000468
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::PS3SaveEditor.AboutBox1));
			this.lblVersion = new global::System.Windows.Forms.Label();
			this.lblDesc = new global::System.Windows.Forms.Label();
			this.lblCopyright = new global::System.Windows.Forms.Label();
			this.linkLabel1 = new global::System.Windows.Forms.LinkLabel();
			this.btnOk = new global::System.Windows.Forms.Button();
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.lblVersion.AutoSize = true;
			this.lblVersion.Location = new global::System.Drawing.Point(59, 11);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new global::System.Drawing.Size(0, 13);
			this.lblVersion.TabIndex = 2;
			this.lblDesc.AutoSize = true;
			this.lblDesc.Location = new global::System.Drawing.Point(59, 30);
			this.lblDesc.Name = "lblDesc";
			this.lblDesc.Size = new global::System.Drawing.Size(124, 13);
			this.lblDesc.TabIndex = 3;
			this.lblDesc.Text = "CYBER PS4 Save Editor";
			this.lblCopyright.AutoSize = true;
			this.lblCopyright.Location = new global::System.Drawing.Point(59, 51);
			this.lblCopyright.Name = "lblCopyright";
			this.lblCopyright.Size = new global::System.Drawing.Size(232, 13);
			this.lblCopyright.TabIndex = 4;
			this.lblCopyright.Text = "Copyright © CYBER Gadget. All rights reserved.";
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new global::System.Drawing.Point(59, 72);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new global::System.Drawing.Size(123, 13);
			this.linkLabel1.TabIndex = 5;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "http://cybergadget.co.jp";
			this.linkLabel1.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			this.btnOk.Location = new global::System.Drawing.Point(291, 70);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new global::System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new global::System.EventHandler(this.btnOk_Click);
			this.pictureBox1.Image = (global::System.Drawing.Image)componentResourceManager.GetObject("pictureBox1.Image");
			this.pictureBox1.Location = new global::System.Drawing.Point(13, 11);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new global::System.Drawing.Size(32, 32);
			this.pictureBox1.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(378, 105);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.linkLabel1);
			base.Controls.Add(this.lblCopyright);
			base.Controls.Add(this.lblDesc);
			base.Controls.Add(this.lblVersion);
			base.Controls.Add(this.pictureBox1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AboutBox1";
			base.Padding = new global::System.Windows.Forms.Padding(9);
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About PS4 Save Editor";
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000001 RID: 1
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000002 RID: 2
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x04000003 RID: 3
		private global::System.Windows.Forms.Label lblVersion;

		// Token: 0x04000004 RID: 4
		private global::System.Windows.Forms.Label lblDesc;

		// Token: 0x04000005 RID: 5
		private global::System.Windows.Forms.Label lblCopyright;

		// Token: 0x04000006 RID: 6
		private global::System.Windows.Forms.LinkLabel linkLabel1;

		// Token: 0x04000007 RID: 7
		private global::System.Windows.Forms.Button btnOk;
	}
}
