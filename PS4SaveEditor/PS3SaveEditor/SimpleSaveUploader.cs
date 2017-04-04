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
	// Token: 0x02000107 RID: 263
	public partial class SimpleSaveUploader : Form
	{
		// Token: 0x06000B02 RID: 2818 RVA: 0x0003DF60 File Offset: 0x0003C160
		public SimpleSaveUploader(game gameItem, string profile, List<string> files)
		{
			this.m_game = gameItem;
			this.InitializeComponent();
			base.ControlBox = false;
			this.Text = Resources.titleSimpleEditUploader;
			base.CenterToScreen();
			this.saveUploadDownloder1.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.saveUploadDownloder1.Files = files.ToArray();
			this.saveUploadDownloder1.Action = "patch";
			this.saveUploadDownloder1.OutputFolder = Path.GetDirectoryName(gameItem.LocalSaveFolder);
			this.saveUploadDownloder1.Game = gameItem;
			this.CloseForm = new SimpleSaveUploader.CloseDelegate(this.CloseFormSafe);
			base.Load += new EventHandler(this.SimpleSaveUploader_Load);
			this.saveUploadDownloder1.DownloadFinish += new SaveUploadDownloder.DownloadFinishEventHandler(this.saveUploadDownloder1_DownloadFinish);
			this.saveUploadDownloder1.UploadFinish += new SaveUploadDownloder.UploadFinishEventHandler(this.saveUploadDownloder1_UploadFinish);
			this.saveUploadDownloder1.UploadStart += new SaveUploadDownloder.UploadStartEventHandler(this.saveUploadDownloder1_UploadStart);
			this.saveUploadDownloder1.DownloadStart += new SaveUploadDownloder.DownloadStartEventHandler(this.saveUploadDownloder1_DownloadStart);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0003E07C File Offset: 0x0003C27C
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0003E0E4 File Offset: 0x0003C2E4
		private List<string> PrepareZipFile(List<string> files)
		{
			List<string> list = new List<string>();
			List<string> containerFiles = this.m_game.GetContainerFiles();
			string text = this.m_game.LocalSaveFolder.Substring(0, this.m_game.LocalSaveFolder.Length - 4);
			string hash = Util.GetHash(text);
			bool cache = Util.GetCache(hash);
			string text2 = this.m_game.ToString(true, files);
			if (cache)
			{
				containerFiles.Remove(text);
				text2 = text2.Replace("<name>" + Path.GetFileNameWithoutExtension(this.m_game.LocalSaveFolder) + "</name>", string.Concat(new string[]
				{
					"<name>",
					Path.GetFileNameWithoutExtension(this.m_game.LocalSaveFolder),
					"</name><md5>",
					hash,
					"</md5>"
				}));
			}
			list.AddRange(containerFiles);
			string tempFolder = Util.GetTempFolder();
			string text3 = Path.Combine(tempFolder, "ps4_list.xml");
			File.WriteAllText(text3, text2);
			list.Add(text3);
			ZipUtil.GetAsZipFile(containerFiles.ToArray(), null);
			return list;
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0003E1F7 File Offset: 0x0003C3F7
		private void saveUploadDownloder1_DownloadStart(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.SetStatus(Resources.msgDownloadPatch);
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0003E209 File Offset: 0x0003C409
		private void saveUploadDownloder1_UploadStart(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.SetStatus(Resources.msgUploadPatch);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0003E21B File Offset: 0x0003C41B
		private void saveUploadDownloder1_UploadFinish(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.SetStatus(Resources.msgWait);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0003E230 File Offset: 0x0003C430
		private void saveUploadDownloder1_DownloadFinish(object sender, DownloadFinishEventArgs e)
		{
			if (!e.Status)
			{
				if (!string.IsNullOrEmpty(e.Error))
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

		// Token: 0x06000B09 RID: 2825 RVA: 0x0003E281 File Offset: 0x0003C481
		private void SimpleSaveUploader_Load(object sender, EventArgs e)
		{
			this.saveUploadDownloder1.Start();
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0003E290 File Offset: 0x0003C490
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

		// Token: 0x06000B0B RID: 2827 RVA: 0x0003E2C3 File Offset: 0x0003C4C3
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

		// Token: 0x06000B0C RID: 2828 RVA: 0x0003E2E5 File Offset: 0x0003C4E5
		private void SimpleSaveUploader_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this.appClosing)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x040005AC RID: 1452
		private SimpleSaveUploader.CloseDelegate CloseForm;

		// Token: 0x040005AD RID: 1453
		private game m_game;

		// Token: 0x040005AE RID: 1454
		private bool appClosing;

		// Token: 0x02000108 RID: 264
		// (Invoke) Token: 0x06000B10 RID: 2832
		private delegate void CloseDelegate(bool bStatus);
	}
}
