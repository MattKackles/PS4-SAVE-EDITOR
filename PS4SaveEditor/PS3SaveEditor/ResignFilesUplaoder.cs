using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x02000069 RID: 105
	public partial class ResignFilesUplaoder : Form
	{
		// Token: 0x0600055B RID: 1371 RVA: 0x00021258 File Offset: 0x0001F458
		public ResignFilesUplaoder(game game, string saveFolder, string profile)
		{
			this.m_saveFolder = saveFolder;
			this.m_profile = profile;
			this.InitializeComponent();
			this.Text = Resources.titleResign;
			base.CenterToScreen();
			this.saveUploadDownloder1.Profile = profile;
			this.saveUploadDownloder1.Files = this.PrepareZipFile(game).ToArray();
			this.saveUploadDownloder1.Action = "resign";
			this.saveUploadDownloder1.OutputFolder = this.m_saveFolder;
			this.CloseForm = new ResignFilesUplaoder.CloseDelegate(this.CloseFormSafe);
			base.Load += new EventHandler(this.SimpleSaveUploader_Load);
			this.saveUploadDownloder1.DownloadFinish += new SaveUploadDownloder.DownloadFinishEventHandler(this.saveUploadDownloder1_DownloadFinish);
			this.saveUploadDownloder1.UploadFinish += new SaveUploadDownloder.UploadFinishEventHandler(this.saveUploadDownloder1_UploadFinish);
			this.saveUploadDownloder1.UploadStart += new SaveUploadDownloder.UploadStartEventHandler(this.saveUploadDownloder1_UploadStart);
			this.saveUploadDownloder1.DownloadStart += new SaveUploadDownloder.DownloadStartEventHandler(this.saveUploadDownloder1_DownloadStart);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00021354 File Offset: 0x0001F554
		private List<string> PrepareZipFile(game game)
		{
			List<string> list = new List<string>();
			list.Add(Path.Combine(this.m_saveFolder, "PARAM.SFO"));
			list.Add(Path.Combine(this.m_saveFolder, "PARAM.PFD"));
			string tempFolder = Util.GetTempFolder();
			string text = Path.Combine(tempFolder, "ps3_files_list.xml");
			if (game != null)
			{
				File.WriteAllText(text, "<files><game>" + game.id + "</game><pfd>PARAM.PFD</pfd><sfo>PARAM.SFO</sfo></files>");
			}
			else
			{
				string text2 = MainForm.GetParamInfo(Path.Combine(this.m_saveFolder, "PARAM.SFO"), "SAVEDATA_DIRECTORY");
				if (string.IsNullOrEmpty(text2) || text2.Length < 9)
				{
					text2 = Path.GetDirectoryName(this.m_saveFolder);
				}
				File.WriteAllText(text, "<files><game>" + text2.Substring(0, 9) + "</game><pfd>PARAM.PFD</pfd><sfo>PARAM.SFO</sfo></files>");
			}
			list.Add(text);
			return list;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00021424 File Offset: 0x0001F624
		private void saveUploadDownloder1_DownloadStart(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.SetStatus(Resources.msgDownloadPatch);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00021436 File Offset: 0x0001F636
		private void saveUploadDownloder1_UploadStart(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.SetStatus(Resources.msgUploadPatch);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00021448 File Offset: 0x0001F648
		private void saveUploadDownloder1_UploadFinish(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.SetStatus(Resources.msgWait);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0002145C File Offset: 0x0001F65C
		private void saveUploadDownloder1_DownloadFinish(object sender, DownloadFinishEventArgs e)
		{
			if (!e.Status)
			{
				if (e.Error != null)
				{
					MessageBox.Show(e.Error, Resources.msgError);
				}
				else
				{
					MessageBox.Show(Resources.errServer, Resources.msgError);
				}
			}
			this.CloseThis(e.Status);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x000214A8 File Offset: 0x0001F6A8
		private void SimpleSaveUploader_Load(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.Start();
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x000214B8 File Offset: 0x0001F6B8
		private void CloseThis(bool status)
		{
			if (!base.IsDisposed)
			{
				base.Invoke(this.CloseForm, new object[]
				{
					status
				});
			}
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000214EB File Offset: 0x0001F6EB
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

		// Token: 0x06000564 RID: 1380 RVA: 0x0002150D File Offset: 0x0001F70D
		private void SimpleSaveUploader_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this.appClosing)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x040002A1 RID: 673
		private string m_saveFolder;

		// Token: 0x040002A2 RID: 674
		private ResignFilesUplaoder.CloseDelegate CloseForm;

		// Token: 0x040002A3 RID: 675
		private string m_profile;

		// Token: 0x040002A4 RID: 676
		private bool appClosing;

		// Token: 0x0200006A RID: 106
		// (Invoke) Token: 0x06000568 RID: 1384
		private delegate void CloseDelegate(bool bStatus);
	}
}
