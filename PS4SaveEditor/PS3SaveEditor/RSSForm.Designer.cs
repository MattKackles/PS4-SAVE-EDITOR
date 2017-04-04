namespace PS3SaveEditor
{
	// Token: 0x0200006E RID: 110
	public partial class RSSForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000587 RID: 1415 RVA: 0x00021EAA File Offset: 0x000200AA
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00021ECC File Offset: 0x000200CC
		private void InitializeComponent()
		{
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.panel2 = new global::System.Windows.Forms.Panel();
			this.lblTitle = new global::System.Windows.Forms.Label();
			this.webBrowser1 = new global::System.Windows.Forms.WebBrowser();
			this.lnkTitle = new global::System.Windows.Forms.LinkLabel();
			this.btnOk = new global::System.Windows.Forms.Button();
			this.lstRSSFeeds = new global::System.Windows.Forms.ListBox();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.panel1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.panel1.BackColor = global::System.Drawing.Color.FromArgb(102, 102, 102);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.btnOk);
			this.panel1.Controls.Add(this.lstRSSFeeds);
			this.panel1.Location = new global::System.Drawing.Point(10, 10);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(604, 420);
			this.panel1.TabIndex = 0;
			this.panel2.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.panel2.BackColor = global::System.Drawing.Color.White;
			this.panel2.Controls.Add(this.lblTitle);
			this.panel2.Controls.Add(this.webBrowser1);
			this.panel2.Controls.Add(this.lnkTitle);
			this.panel2.Location = new global::System.Drawing.Point(12, 97);
			this.panel2.Name = "panel2";
			this.panel2.Size = new global::System.Drawing.Size(581, 284);
			this.panel2.TabIndex = 2;
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14f, global::System.Drawing.FontStyle.Bold);
			this.lblTitle.ForeColor = global::System.Drawing.Color.Black;
			this.lblTitle.Location = new global::System.Drawing.Point(9, 10);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new global::System.Drawing.Size(0, 24);
			this.lblTitle.TabIndex = 4;
			this.webBrowser1.AllowWebBrowserDrop = false;
			this.webBrowser1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
			this.webBrowser1.Location = new global::System.Drawing.Point(3, 40);
			this.webBrowser1.MinimumSize = new global::System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.ScriptErrorsSuppressed = true;
			this.webBrowser1.Size = new global::System.Drawing.Size(575, 244);
			this.webBrowser1.TabIndex = 3;
			this.lnkTitle.AutoSize = true;
			this.lnkTitle.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14f, global::System.Drawing.FontStyle.Bold);
			this.lnkTitle.ForeColor = global::System.Drawing.Color.White;
			this.lnkTitle.Location = new global::System.Drawing.Point(9, 10);
			this.lnkTitle.Name = "lnkTitle";
			this.lnkTitle.Size = new global::System.Drawing.Size(0, 24);
			this.lnkTitle.TabIndex = 2;
			this.btnOk.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.btnOk.Location = new global::System.Drawing.Point(263, 389);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new global::System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = false;
			this.lstRSSFeeds.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.lstRSSFeeds.FormattingEnabled = true;
			this.lstRSSFeeds.Location = new global::System.Drawing.Point(12, 12);
			this.lstRSSFeeds.Name = "lstRSSFeeds";
			this.lstRSSFeeds.Size = new global::System.Drawing.Size(581, 82);
			this.lstRSSFeeds.TabIndex = 0;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.Black;
			base.ClientSize = new global::System.Drawing.Size(624, 442);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Fixed3D;
			base.Icon = global::PS3SaveEditor.Resources.Resources.ps3se;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "RSSForm";
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "RSSForm";
			base.ResizeEnd += new global::System.EventHandler(this.RSSForm_ResizeEnd);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x040002B0 RID: 688
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040002B1 RID: 689
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x040002B2 RID: 690
		private global::System.Windows.Forms.ListBox lstRSSFeeds;

		// Token: 0x040002B3 RID: 691
		private global::System.Windows.Forms.Button btnOk;

		// Token: 0x040002B4 RID: 692
		private global::System.Windows.Forms.Panel panel2;

		// Token: 0x040002B5 RID: 693
		private global::System.Windows.Forms.LinkLabel lnkTitle;

		// Token: 0x040002B6 RID: 694
		private global::System.Windows.Forms.WebBrowser webBrowser1;

		// Token: 0x040002B7 RID: 695
		private global::System.Windows.Forms.Label lblTitle;
	}
}
