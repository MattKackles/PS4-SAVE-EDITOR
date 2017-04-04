using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x02000006 RID: 6
	public partial class AdvancedSaveUploaderForEncrypt : Form
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00006760 File Offset: 0x00004960
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00006768 File Offset: 0x00004968
		public Dictionary<string, byte[]> DecryptedSaveData
		{
			get;
			set;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00006771 File Offset: 0x00004971
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00006779 File Offset: 0x00004979
		public byte[] DependentSaveData
		{
			get;
			set;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00006782 File Offset: 0x00004982
		// (set) Token: 0x06000049 RID: 73 RVA: 0x0000678A File Offset: 0x0000498A
		public string ListResult
		{
			get;
			set;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00006794 File Offset: 0x00004994
		public AdvancedSaveUploaderForEncrypt(string[] files, game gameItem, string profile, string action)
		{
			this.m_files = files;
			this.InitializeComponent();
			base.ControlBox = false;
			this.DecryptedSaveData = new Dictionary<string, byte[]>();
			this.saveUploadDownloder1.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.SetLabels();
			this.m_action = action;
			base.CenterToScreen();
			this.saveUploadDownloder1.Files = files;
			this.saveUploadDownloder1.Action = action;
			if (this.m_action == "encrypt")
			{
				this.saveUploadDownloder1.OutputFolder = Path.GetDirectoryName(gameItem.LocalSaveFolder);
			}
			else
			{
				this.saveUploadDownloder1.OutputFolder = ZipUtil.GetPs3SeTempFolder();
			}
			this.saveUploadDownloder1.Game = gameItem;
			this.CloseForm = new AdvancedSaveUploaderForEncrypt.CloseDelegate(this.CloseFormSafe);
			base.Load += new EventHandler(this.AdvancedSaveUploaderForEncrypt_Load);
			this.saveUploadDownloder1.DownloadFinish += new SaveUploadDownloder.DownloadFinishEventHandler(this.saveUploadDownloder1_DownloadFinish);
			this.saveUploadDownloder1.UploadFinish += new SaveUploadDownloder.UploadFinishEventHandler(this.saveUploadDownloder1_UploadFinish);
			this.saveUploadDownloder1.UploadStart += new SaveUploadDownloder.UploadStartEventHandler(this.saveUploadDownloder1_UploadStart);
			this.saveUploadDownloder1.DownloadStart += new SaveUploadDownloder.DownloadStartEventHandler(this.saveUploadDownloder1_DownloadStart);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000068DC File Offset: 0x00004ADC
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00006944 File Offset: 0x00004B44
		private void SetLabels()
		{
			this.Text = Resources.titleAdvDownloader;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00006951 File Offset: 0x00004B51
		private void saveUploadDownloder1_DownloadStart(object sender, EventArgs e)
		{
			if (this.m_action == "encrypt")
			{
				this.saveUploadDownloder1.SetStatus(Resources.msgDownloadEnc);
				return;
			}
			this.saveUploadDownloder1.SetStatus(Resources.msgDownloadDec);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00006986 File Offset: 0x00004B86
		private void saveUploadDownloder1_UploadStart(object sender, EventArgs e)
		{
			if (this.m_action == "encrypt")
			{
				this.saveUploadDownloder1.SetStatus(Resources.msgUploadEnc);
				return;
			}
			this.saveUploadDownloder1.SetStatus(Resources.msgUploadDec);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000069BB File Offset: 0x00004BBB
		private void saveUploadDownloder1_UploadFinish(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.SetStatus(Resources.msgWait);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000069D0 File Offset: 0x00004BD0
		private void saveUploadDownloder1_DownloadFinish(object sender, DownloadFinishEventArgs e)
		{
			if (this.m_action == "list" && !string.IsNullOrEmpty(this.saveUploadDownloder1.ListResult))
			{
				this.ListResult = this.saveUploadDownloder1.ListResult;
				if (!base.IsDisposed)
				{
					this.CloseThis(e.Status);
				}
				return;
			}
			List<string> saveFiles = this.saveUploadDownloder1.Game.GetSaveFiles();
			string[] files = Directory.GetFiles(this.saveUploadDownloder1.OutputFolder, "*");
			foreach (string current in saveFiles)
			{
				this.saveUploadDownloder1.Game.GetTargetGameFolder();
				if (e.Status)
				{
					if (!(this.m_action == "decrypt"))
					{
						continue;
					}
					using (List<string>.Enumerator enumerator2 = this.saveUploadDownloder1.OrderedEntries.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							string current2 = enumerator2.Current;
							string text = Path.Combine(Util.GetTempFolder(), current2);
							if (Array.IndexOf<string>(files, text) >= 0 && !Path.GetFileName(text).Equals("param.sfo", StringComparison.CurrentCultureIgnoreCase) && !Path.GetFileName(text).Equals("param.pfd", StringComparison.CurrentCultureIgnoreCase) && !Path.GetFileName(text).Equals("devlog.txt", StringComparison.CurrentCultureIgnoreCase) && !Path.GetFileName(text).Equals("ps4_list.xml", StringComparison.CurrentCultureIgnoreCase) && !this.DecryptedSaveData.ContainsKey(Path.GetFileName(text)) && (current == Path.GetFileName(text) || Util.IsMatch(Path.GetFileName(text), current)))
							{
								this.DecryptedSaveData.Add(Path.GetFileName(text), File.ReadAllBytes(text));
							}
						}
						continue;
					}
				}
				if (!string.IsNullOrEmpty(e.Error))
				{
					SaveUploadDownloder.ErrorMessage(this, e.Error, Resources.msgError);
					break;
				}
				SaveUploadDownloder.ErrorMessage(this, Resources.errServer, Resources.msgError);
				break;
			}
			if (!base.IsDisposed)
			{
				this.CloseThis(e.Status);
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00006C20 File Offset: 0x00004E20
		private void CloseThis(bool bStatus)
		{
			try
			{
				base.Invoke(this.CloseForm, new object[]
				{
					bStatus
				});
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00006C60 File Offset: 0x00004E60
		private void CloseFormSafe(bool bStatus)
		{
			if (bStatus)
			{
				base.DialogResult = DialogResult.OK;
			}
			else
			{
				base.DialogResult = DialogResult.Abort;
			}
			this.appClosing = true;
			base.Close();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00006C82 File Offset: 0x00004E82
		private void AdvancedSaveUploaderForEncrypt_Load(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.Start();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00006C8F File Offset: 0x00004E8F
		private void AdvancedSaveUploaderForEncrypt_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this.appClosing)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x0400003C RID: 60
		private AdvancedSaveUploaderForEncrypt.CloseDelegate CloseForm;

		// Token: 0x0400003D RID: 61
		private string m_action;

		// Token: 0x0400003E RID: 62
		private string[] m_files;

		// Token: 0x0400003F RID: 63
		private bool appClosing;

		// Token: 0x02000007 RID: 7
		// (Invoke) Token: 0x06000058 RID: 88
		private delegate void CloseDelegate(bool bStatus);
	}
}
