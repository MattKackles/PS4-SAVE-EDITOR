using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using Ionic.Zip;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x0200006B RID: 107
	public partial class RestoreBackup : Form
	{
		// Token: 0x0600056B RID: 1387 RVA: 0x00021690 File Offset: 0x0001F890
		public RestoreBackup(string backupFile, string destFolder)
		{
			this.m_backupFile = backupFile;
			this.m_destFolder = destFolder;
			this.UpdateProgress = new RestoreBackup.UpdateProgressDelegate(this.UpdateProgressSafe);
			this.CloseForm = new RestoreBackup.CloseDelegate(this.CloseFormSafe);
			this.InitializeComponent();
			this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.lblProgress.BackColor = Color.Transparent;
			this.lblProgress.ForeColor = Color.White;
			this.lblProgress.Text = Resources.lblRestoring;
			base.CenterToScreen();
			base.Load += new EventHandler(this.RestoreBackup_Load);
			base.Activated += new EventHandler(this.RestoreBackup_Activated);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00021758 File Offset: 0x0001F958
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x000217C0 File Offset: 0x0001F9C0
		private void RestoreBackup_Activated(object sender, EventArgs e)
		{
			if (!this.m_bActivated)
			{
				this.m_bActivated = true;
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x000217D4 File Offset: 0x0001F9D4
		private void RestoreBackup_Load(object sender, EventArgs e)
		{
			Thread thread = new Thread(new ThreadStart(this.ExtractBackup));
			thread.Start();
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x000217F9 File Offset: 0x0001F9F9
		private void UpdateProgressSafe(int val)
		{
			this.pbProgress.Value = val;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00021808 File Offset: 0x0001FA08
		private void ExtractBackup()
		{
			ZipFile zipFile = ZipFile.Read(this.m_backupFile);
			zipFile.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(this.zipFile_ExtractProgress);
			zipFile.ExtractAll(this.m_destFolder, ExtractExistingFileAction.InvokeExtractProgressEvent);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00021840 File Offset: 0x0001FA40
		private void zipFile_ExtractProgress(object sender, ExtractProgressEventArgs e)
		{
			if (e.EventType == ZipProgressEventType.Extracting_ExtractEntryWouldOverwrite)
			{
				e.CurrentEntry.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
			}
			if (e.TotalBytesToTransfer > 100L)
			{
				this.pbProgress.Invoke(this.UpdateProgress, new object[]
				{
					(int)(e.BytesTransferred * 100L / e.TotalBytesToTransfer)
				});
			}
			if (e.EventType == ZipProgressEventType.Extracting_AfterExtractAll)
			{
				base.Invoke(this.CloseForm, new object[]
				{
					true
				});
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x000218CA File Offset: 0x0001FACA
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
			base.Close();
		}

		// Token: 0x040002A7 RID: 679
		private string m_backupFile;

		// Token: 0x040002A8 RID: 680
		private string m_destFolder;

		// Token: 0x040002A9 RID: 681
		private bool m_bActivated;

		// Token: 0x040002AA RID: 682
		private RestoreBackup.UpdateProgressDelegate UpdateProgress;

		// Token: 0x040002AB RID: 683
		private RestoreBackup.CloseDelegate CloseForm;

		// Token: 0x0200006C RID: 108
		// (Invoke) Token: 0x06000576 RID: 1398
		private delegate void UpdateProgressDelegate(int value);

		// Token: 0x0200006D RID: 109
		// (Invoke) Token: 0x0600057A RID: 1402
		private delegate void CloseDelegate(bool bSuccess);
	}
}
