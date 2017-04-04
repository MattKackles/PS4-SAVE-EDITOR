using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DGDev;
using PS3SaveEditor.Resources;
using Rss;

namespace PS3SaveEditor
{
	// Token: 0x02000061 RID: 97
	public partial class GameListDownloader : Form
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0001F666 File Offset: 0x0001D866
		// (set) Token: 0x0600051C RID: 1308 RVA: 0x0001F66E File Offset: 0x0001D86E
		public string GameListXml
		{
			get;
			set;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001F678 File Offset: 0x0001D878
		public GameListDownloader()
		{
			this.InitializeComponent();
			base.ControlBox = false;
			this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.lblStatus.BackColor = Color.Transparent;
			this.DoubleBuffered = true;
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.CenterToScreen();
			this.UpdateProgress = new GameListDownloader.UpdateProgressDelegate(this.UpdateProgressSafe);
			this.UpdateStatus = new GameListDownloader.UpdateStatusDelegate(this.UpdateStatusSafe);
			this.CloseForm = new GameListDownloader.CloseDelegate(this.CloseFormSafe);
			this.lblStatus.Text = Resources.gamelistDownloaderMsg;
			this.Text = Resources.gamelistDownloaderTitle;
			base.Load += new EventHandler(this.GameListDownloader_Load);
			base.Visible = false;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001F74C File Offset: 0x0001D94C
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001F7B4 File Offset: 0x0001D9B4
		private void CloseThisForm(bool bSuccess)
		{
			if (base.IsHandleCreated)
			{
				base.Invoke(this.CloseForm, new object[]
				{
					bSuccess
				});
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001F7E7 File Offset: 0x0001D9E7
		private void CloseFormSafe(bool bSuccess)
		{
			if (!bSuccess)
			{
				base.DialogResult = DialogResult.Abort;
			}
			else
			{
				base.DialogResult = DialogResult.OK;
			}
			this.appClosing = true;
			base.Close();
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001F80C File Offset: 0x0001DA0C
		private void SetStatus(string status)
		{
			this.lblStatus.Invoke(this.UpdateStatus, new object[]
			{
				status
			});
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001F837 File Offset: 0x0001DA37
		private void UpdateStatusSafe(string status)
		{
			this.lblStatus.Text = status;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001F848 File Offset: 0x0001DA48
		private void SetProgress(int val)
		{
			this.pbProgress.Invoke(this.UpdateProgress, new object[]
			{
				val
			});
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001F878 File Offset: 0x0001DA78
		private void UpdateProgressSafe(int val)
		{
			this.pbProgress.Value = val;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001F888 File Offset: 0x0001DA88
		private void GameListDownloader_Load(object sender, EventArgs e)
		{
			Thread thread = new Thread(new ThreadStart(this.GetOnlineGamesList));
			thread.Start();
			try
			{
				SplashScreen.Current.SetInfo = "Downloading news feed...";
				long arg_35_0 = DateTime.Now.Ticks;
				RssFeed rssFeed = RssFeed.Read(string.Format("{0}", "http://www.cybergadget.co.jp/PS4SE/"));
				RssChannel rssChannel = rssFeed.Channels[0];
				Control.CheckForIllegalCrossThreadCalls = false;
				SplashScreen.Current.Hide();
				if (rssChannel.Items.Count > 0)
				{
					RSSForm rSSForm = new RSSForm(rssChannel);
					rSSForm.ShowDialog();
				}
			}
			catch (Exception)
			{
				Control.CheckForIllegalCrossThreadCalls = false;
				SplashScreen.Current.Hide();
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001F940 File Offset: 0x0001DB40
		private void GetOnlineGamesList()
		{
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format(GameListDownloader.GAME_LIST_URL, Util.GetBaseUrl(), Util.GetAuthToken()));
				httpWebRequest.Method = "GET";
				httpWebRequest.Credentials = Util.GetNetworkCredential();
				string value = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
				httpWebRequest.UserAgent = Util.GetUserAgent();
				httpWebRequest.Headers.Add("Authorization", value);
				httpWebRequest.Headers.Add("accept-charset", "UTF-8");
				string text = "";
				text += "form_build_id=af33c2669bb1dc77eb5b3fcdc4526938&";
				text = text + "license=" + Util.GetUserId() + "&";
				text = text + "mac=" + Util.GetUID(false, false) + "&";
				text += "gameid=&";
				text += "version=v2&";
				text += "action=list";
				Encoding.UTF8.GetBytes(text);
				this.SetStatus(Resources.msgConnecting);
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				if (HttpStatusCode.OK == httpWebResponse.StatusCode)
				{
					this.SetStatus(Resources.msgDownloadingList);
					Stream responseStream = httpWebResponse.GetResponseStream();
					int num = 0;
					string tempFileName = Path.GetTempFileName();
					FileStream fileStream = new FileStream(tempFileName, FileMode.OpenOrCreate, FileAccess.Write);
					byte[] buffer = new byte[1024];
					if (httpWebResponse.ContentLength != -1L)
					{
						goto IL_1F2;
					}
					using (StreamReader streamReader = new StreamReader(responseStream))
					{
						using (StreamWriter streamWriter = new StreamWriter(fileStream))
						{
							streamWriter.Write(streamReader.ReadToEnd());
						}
						goto IL_1FE;
					}
					IL_1AA:
					int num2 = responseStream.Read(buffer, 0, Math.Min(1024, (int)httpWebResponse.ContentLength - num));
					fileStream.Write(buffer, 0, num2);
					num += num2;
					this.SetProgress((int)((long)(num * 100) / httpWebResponse.ContentLength));
					IL_1F2:
					if ((long)num < httpWebResponse.ContentLength)
					{
						goto IL_1AA;
					}
					IL_1FE:
					this.SetProgress(100);
					fileStream.Close();
					this.GameListXml = File.ReadAllText(tempFileName);
					if (this.GameListXml.IndexOf("ERROR") > 0)
					{
						MessageBox.Show(Resources.errServer);
						this.GameListXml = "";
						this.CloseThisForm(false);
						return;
					}
					httpWebResponse.Close();
					File.Delete(tempFileName);
				}
				else
				{
					MessageBox.Show(Resources.errInvalidResponse);
				}
				Thread.Sleep(1000);
				this.CloseThisForm(true);
			}
			catch (Exception ex)
			{
				MessageBox.Show(Resources.errConnection, Resources.msgError);
				this.CloseThisForm(false);
				throw ex;
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001FC34 File Offset: 0x0001DE34
		private void GameListDownloader_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this.appClosing)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x04000282 RID: 642
		public const string RSS_URL = "http://www.cybergadget.co.jp/PS4SE/";

		// Token: 0x04000283 RID: 643
		private GameListDownloader.UpdateProgressDelegate UpdateProgress;

		// Token: 0x04000284 RID: 644
		private GameListDownloader.UpdateStatusDelegate UpdateStatus;

		// Token: 0x04000285 RID: 645
		private GameListDownloader.CloseDelegate CloseForm;

		// Token: 0x04000286 RID: 646
		private static string GAME_LIST_URL = "{0}/games?token={1}";

		// Token: 0x04000287 RID: 647
		private bool appClosing;

		// Token: 0x02000062 RID: 98
		// (Invoke) Token: 0x0600052C RID: 1324
		private delegate void UpdateProgressDelegate(int value);

		// Token: 0x02000063 RID: 99
		// (Invoke) Token: 0x06000530 RID: 1328
		private delegate void UpdateStatusDelegate(string status);

		// Token: 0x02000064 RID: 100
		// (Invoke) Token: 0x06000534 RID: 1332
		private delegate void CloseDelegate(bool bSuccess);
	}
}
