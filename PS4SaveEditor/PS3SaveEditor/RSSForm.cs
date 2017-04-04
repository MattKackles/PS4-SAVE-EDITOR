using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PS3SaveEditor.Resources;
using Rss;

namespace PS3SaveEditor
{
	// Token: 0x0200006E RID: 110
	public partial class RSSForm : Form
	{
		// Token: 0x0600057D RID: 1405 RVA: 0x00021AE8 File Offset: 0x0001FCE8
		public RSSForm(RssChannel channel)
		{
			this.InitializeComponent();
			this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.lstRSSFeeds.DrawMode = DrawMode.OwnerDrawFixed;
			this.lstRSSFeeds.DrawItem += new DrawItemEventHandler(this.lstRSSFeeds_DrawItem);
			base.CenterToScreen();
			this.Text = Resources.rssTitle;
			base.Load += new EventHandler(this.RSSForm_Load);
			this.btnOk.Click += new EventHandler(this.btnOk_Click);
			base.LostFocus += new EventHandler(this.RSSForm_LostFocus);
			this.lstRSSFeeds.SelectedIndexChanged += new EventHandler(this.lstRSSFeeds_SelectedIndexChanged);
			this.lnkTitle.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkTitle_LinkClicked);
			try
			{
				if (channel.Items.Count > 0)
				{
					this.lstRSSFeeds.DataSource = channel.Items;
				}
				else
				{
					this.lstRSSFeeds.DataSource = null;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00021C04 File Offset: 0x0001FE04
		private void lstRSSFeeds_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0)
			{
				return;
			}
			e.DrawBackground();
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.FromArgb(0, 133, 255));
				e.Graphics.DrawString(this.lstRSSFeeds.Items[e.Index].ToString(), e.Font, Brushes.White, e.Bounds, StringFormat.GenericDefault);
			}
			else
			{
				e.Graphics.DrawString(this.lstRSSFeeds.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
			}
			e.DrawFocusRectangle();
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00021CF8 File Offset: 0x0001FEF8
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00021D60 File Offset: 0x0001FF60
		private void lnkTitle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo(this.lnkTitle.Links[0].LinkData as string);
			Process.Start(startInfo);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00021D95 File Offset: 0x0001FF95
		private void RSSForm_LostFocus(object sender, EventArgs e)
		{
			base.Focus();
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00021DA0 File Offset: 0x0001FFA0
		private void lstRSSFeeds_SelectedIndexChanged(object sender, EventArgs e)
		{
			RssItem rssItem = (RssItem)this.lstRSSFeeds.SelectedItem;
			if (rssItem.Link != null)
			{
				this.lnkTitle.Text = rssItem.Title;
				this.lnkTitle.Links.Clear();
				this.lnkTitle.Links.Add(0, rssItem.Title.Length, rssItem.Link.ToString());
				this.lnkTitle.Visible = true;
				this.lblTitle.Visible = false;
			}
			else
			{
				this.lblTitle.Text = rssItem.Title;
				this.lnkTitle.Visible = false;
				this.lblTitle.Visible = true;
			}
			this.webBrowser1.DocumentText = "<html><body style='font-size:12px;overflow-y:auto'>" + rssItem.Description + "</body></html>";
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00021E78 File Offset: 0x00020078
		private void btnOk_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000584 RID: 1412
		[DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetForegroundWindow(IntPtr hwnd);

		// Token: 0x06000585 RID: 1413 RVA: 0x00021E80 File Offset: 0x00020080
		private void RSSForm_Load(object sender, EventArgs e)
		{
			if (this.lstRSSFeeds.DataSource == null)
			{
				base.Close();
				return;
			}
			base.Show();
			RSSForm.SetForegroundWindow(base.Handle);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00021EA8 File Offset: 0x000200A8
		private void RSSForm_ResizeEnd(object sender, EventArgs e)
		{
		}
	}
}
