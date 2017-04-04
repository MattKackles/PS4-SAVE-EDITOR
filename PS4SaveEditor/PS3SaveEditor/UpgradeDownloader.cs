using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Windows.Forms;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x0200010C RID: 268
	public partial class UpgradeDownloader : Form
	{
		// Token: 0x06000B32 RID: 2866 RVA: 0x0003E98C File Offset: 0x0003CB8C
		public UpgradeDownloader(string url)
		{
			this.InitializeComponent();
			this.m_url = url;
			this.lblStatus.Text = Resources.lblDownloadStatus;
			this.Text = Resources.titleUpgrader;
			base.CenterToScreen();
			this.lblStatus.BackColor = Color.Transparent;
			this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
			base.Load += new EventHandler(this.UpgradeDownloader_Load);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0003EA10 File Offset: 0x0003CC10
		private void UpgradeDownloader_Load(object sender, EventArgs e)
		{
			WebClientEx webClientEx = new WebClientEx();
			webClientEx.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0)");
			this.tempFile = Path.GetTempFileName();
			webClientEx.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.client_DownloadProgressChanged);
			webClientEx.DownloadFileCompleted += new AsyncCompletedEventHandler(this.client_DownloadFileCompleted);
			webClientEx.DownloadFileAsync(new Uri(this.m_url, UriKind.Absolute), this.tempFile, this.tempFile);
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0003EA88 File Offset: 0x0003CC88
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0003EAF0 File Offset: 0x0003CCF0
		private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				MessageBox.Show(Resources.errUpgrade);
				return;
			}
			new Process
			{
				StartInfo = new ProcessStartInfo("msiexec", "/i \"" + this.tempFile + "\"")
			}.Start();
			base.Close();
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0003EB49 File Offset: 0x0003CD49
		private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			this.pbProgress.Value = e.ProgressPercentage;
		}

		// Token: 0x040005C0 RID: 1472
		private string m_url;

		// Token: 0x040005C1 RID: 1473
		private string tempFile;
	}
}
