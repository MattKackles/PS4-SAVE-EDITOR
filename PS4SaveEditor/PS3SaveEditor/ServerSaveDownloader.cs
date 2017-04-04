using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x02000020 RID: 32
	public partial class ServerSaveDownloader : Form
	{
		// Token: 0x06000217 RID: 535 RVA: 0x0000D220 File Offset: 0x0000B420
		public ServerSaveDownloader(string saveid, string saveFolder, game game)
		{
			this.m_saveFolder = saveFolder;
			this.m_game = game;
			this.InitializeComponent();
			this.Text = Resources.titleResign;
			base.CenterToScreen();
			this.saveUploadDownloder1.SaveId = saveid;
			this.saveUploadDownloder1.Action = "download";
			this.saveUploadDownloder1.OutputFolder = this.m_saveFolder;
			this.CloseForm = new ServerSaveDownloader.CloseDelegate(this.CloseFormSafe);
			base.Load += new EventHandler(this.SimpleSaveUploader_Load);
			this.saveUploadDownloder1.DownloadFinish += new SaveUploadDownloder.DownloadFinishEventHandler(this.saveUploadDownloder1_DownloadFinish);
			this.saveUploadDownloder1.UploadFinish += new SaveUploadDownloder.UploadFinishEventHandler(this.saveUploadDownloder1_UploadFinish);
			this.saveUploadDownloder1.UploadStart += new SaveUploadDownloder.UploadStartEventHandler(this.saveUploadDownloder1_UploadStart);
			this.saveUploadDownloder1.DownloadStart += new SaveUploadDownloder.DownloadStartEventHandler(this.saveUploadDownloder1_DownloadStart);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000D308 File Offset: 0x0000B508
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

		// Token: 0x06000219 RID: 537 RVA: 0x0000D3D8 File Offset: 0x0000B5D8
		private void saveUploadDownloder1_DownloadStart(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.SetStatus(Resources.msgDownloadPatch);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000D3EA File Offset: 0x0000B5EA
		private void saveUploadDownloder1_UploadStart(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.SetStatus(Resources.msgUploadPatch);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000D3FC File Offset: 0x0000B5FC
		private void saveUploadDownloder1_UploadFinish(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.SetStatus(Resources.msgWait);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000D410 File Offset: 0x0000B610
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
			else
			{
				Directory.Exists(this.m_game.LocalSaveFolder);
			}
			this.CloseThis(e.Status);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000D46F File Offset: 0x0000B66F
		private void SimpleSaveUploader_Load(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.Start();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000D47C File Offset: 0x0000B67C
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

		// Token: 0x0600021F RID: 543 RVA: 0x0000D4AF File Offset: 0x0000B6AF
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

		// Token: 0x06000220 RID: 544 RVA: 0x0000D4D1 File Offset: 0x0000B6D1
		private void SimpleSaveUploader_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this.appClosing)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x040000BD RID: 189
		private string m_saveFolder;

		// Token: 0x040000BE RID: 190
		private ServerSaveDownloader.CloseDelegate CloseForm;

		// Token: 0x040000BF RID: 191
		private string m_gamecode;

		// Token: 0x040000C0 RID: 192
		private bool appClosing;

		// Token: 0x040000C1 RID: 193
		private game m_game;

		// Token: 0x02000021 RID: 33
		// (Invoke) Token: 0x06000224 RID: 548
		private delegate void CloseDelegate(bool bStatus);
	}
}
