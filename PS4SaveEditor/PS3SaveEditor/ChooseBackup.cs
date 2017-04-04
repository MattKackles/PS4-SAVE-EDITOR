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
	// Token: 0x0200001A RID: 26
	public partial class ChooseBackup : Form
	{
		// Token: 0x06000117 RID: 279 RVA: 0x00009D7C File Offset: 0x00007F7C
		public ChooseBackup(string gameName, string save, string saveFolder)
		{
			this.m_save = save;
			this.m_saveFolder = saveFolder;
			this.InitializeComponent();
			this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.lblGameName.BackColor = Color.Transparent;
			this.lblGameName.ForeColor = Color.White;
			base.CenterToScreen();
			this.deleteToolStripMenuItem.Text = Resources.mnuDelete;
			this.lblGameName.Text = gameName;
			this.btnRestore.Text = Resources.btnRestore;
			this.btnCancel.Text = Resources.btnCancel;
			this.Text = Resources.titleChooseBackup;
			this.LoadBackups();
			this.lstBackups.DisplayMember = "Timestamp";
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00009E48 File Offset: 0x00008048
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00009EB0 File Offset: 0x000080B0
		private void LoadBackups()
		{
			List<BackupItem> list = new List<BackupItem>();
			string backupLocation = Util.GetBackupLocation();
			string[] files = Directory.GetFiles(backupLocation, this.m_save + "*");
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				string fileName = Path.GetFileName(text);
				int num = fileName.LastIndexOf('.');
				if (num > 19)
				{
					list.Add(new BackupItem
					{
						BackupFile = text,
						Timestamp = fileName.Substring(num - 19, 19)
					});
				}
			}
			this.lstBackups.DataSource = list;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00009F50 File Offset: 0x00008150
		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.lstBackups.SelectedItem == null)
			{
				return;
			}
			string backupFile = (this.lstBackups.SelectedItem as BackupItem).BackupFile;
			File.Delete(backupFile);
			this.LoadBackups();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00009F90 File Offset: 0x00008190
		private void btnRestore_Click(object sender, EventArgs e)
		{
			if (this.lstBackups.SelectedItem == null)
			{
				return;
			}
			string backupFile = (this.lstBackups.SelectedItem as BackupItem).BackupFile;
			if (MessageBox.Show(Resources.warnRestore, this.Text, MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			try
			{
				RestoreBackup restoreBackup = new RestoreBackup(backupFile, Path.GetDirectoryName(this.m_saveFolder));
				restoreBackup.ShowDialog();
				MessageBox.Show(Resources.msgRestored);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000A010 File Offset: 0x00008210
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0400008D RID: 141
		private string m_saveFolder;

		// Token: 0x0400008E RID: 142
		private string m_save;

        private void ChooseBackup_Load(object sender, EventArgs e)
        {

        }
    }
}
