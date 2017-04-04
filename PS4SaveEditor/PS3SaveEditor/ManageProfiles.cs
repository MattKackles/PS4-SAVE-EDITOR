using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x02000065 RID: 101
	public partial class ManageProfiles : Form
	{
		// Token: 0x06000537 RID: 1335 RVA: 0x0001FEC0 File Offset: 0x0001E0C0
		public ManageProfiles(string psnid, Dictionary<string, object> registered)
		{
			this.InitializeComponent();
			this.m_registered = registered;
			this.btnSave.Text = Resources.btnApply;
			this.btnClose.Text = Resources.btnClose;
			this.Text = Resources.titleManageProfiles;
			this.dgProfiles.Columns[0].HeaderText = Resources.colProfileName;
			this.deleteToolStripMenuItem.Text = Resources.lblDeleteProfile;
			this.renameToolStripMenuItem.Text = Resources.mnuRenameProfile;
			base.CenterToScreen();
			this.btnSave.BackColor = SystemColors.ButtonFace;
			this.btnClose.BackColor = SystemColors.ButtonFace;
			this.btnSave.ForeColor = Color.Black;
			this.btnClose.ForeColor = Color.Black;
			this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.dgProfiles.CellValidated += new DataGridViewCellEventHandler(this.dgProfiles_CellValidated);
			this.dgProfiles.CurrentCellDirtyStateChanged += new EventHandler(this.dgProfiles_CurrentCellDirtyStateChanged);
			this.dgProfiles.CellValueChanged += new DataGridViewCellEventHandler(this.dgProfiles_CellValueChanged);
			this.dgProfiles.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgProfiles_EditingControlShowing);
			this.dgProfiles.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgProfiles_CellMouseDown);
			this.m_newPSN_ID = psnid;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00020028 File Offset: 0x0001E228
		private void dgProfiles_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			e.Control.KeyPress -= new KeyPressEventHandler(this.Control_KeyPress);
			if (this.dgProfiles.CurrentCell.ColumnIndex == 0)
			{
				e.Control.KeyPress += new KeyPressEventHandler(this.Control_KeyPress);
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00020078 File Offset: 0x0001E278
		private void Control_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '.' || e.KeyChar == '/' || e.KeyChar == '\\' || e.KeyChar == '%' || e.KeyChar == '[' || e.KeyChar == ']' || e.KeyChar == ':' || e.KeyChar == ';' || e.KeyChar == '|' || e.KeyChar == '=' || e.KeyChar == ',' || e.KeyChar == '?' || e.KeyChar == '<' || e.KeyChar == '>' || e.KeyChar == '&')
			{
				e.KeyChar = '\0';
				e.Handled = true;
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00020130 File Offset: 0x0001E330
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00020198 File Offset: 0x0001E398
		private void dgProfiles_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.Button == MouseButtons.Right)
			{
				this.dgProfiles.ClearSelection();
				this.dgProfiles.Rows[e.RowIndex].Selected = true;
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x000201EC File Offset: 0x0001E3EC
		private void dgProfiles_MouseClick(object sender, MouseEventArgs e)
		{
			int rowIndex = this.dgProfiles.HitTest(e.X, e.Y).RowIndex;
			this.dgProfiles.ClearSelection();
			this.dgProfiles.Rows[rowIndex].Selected = true;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00020238 File Offset: 0x0001E438
		private void dgProfiles_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			if (e.Row.Index == 0)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0002024E File Offset: 0x0001E44E
		private void dgProfiles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00020250 File Offset: 0x0001E450
		private void dgProfiles_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00020254 File Offset: 0x0001E454
		private void dgProfiles_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			if (this.dgProfiles.IsCurrentCellDirty)
			{
				this.dgProfiles.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
			if (this.dgProfiles.CurrentCell.ColumnIndex == 2)
			{
				foreach (DataGridViewRow dataGridViewRow in ((IEnumerable)this.dgProfiles.Rows))
				{
					dataGridViewRow.Cells[2].Value = false;
				}
			}
			if (this.dgProfiles.CurrentCell.ColumnIndex == 2)
			{
				foreach (DataGridViewRow dataGridViewRow2 in ((IEnumerable)this.dgProfiles.Rows))
				{
					if (dataGridViewRow2.Index == this.dgProfiles.CurrentCell.RowIndex)
					{
						dataGridViewRow2.Cells[2].Value = true;
					}
				}
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00020384 File Offset: 0x0001E584
		private void ManageProfiles_Load(object sender, EventArgs e)
		{
			foreach (string current in this.m_registered.Keys)
			{
				Dictionary<string, object> dictionary = this.m_registered[current] as Dictionary<string, object>;
				int index = this.dgProfiles.Rows.Add();
				this.dgProfiles.Rows[index].Cells[0].Value = dictionary["friendly_name"];
				this.dgProfiles.Rows[index].Cells[1].Value = current;
				this.dgProfiles.Rows[index].Tag = (dictionary.ContainsKey("replaceable") && (bool)dictionary["replaceable"]);
			}
			if (!string.IsNullOrEmpty(this.m_newPSN_ID))
			{
				int index2 = this.dgProfiles.Rows.Add();
				this.dgProfiles.Rows[index2].Cells[0].Value = "Enter Name";
				this.dgProfiles.Rows[index2].Cells[1].Value = this.m_newPSN_ID;
				this.dgProfiles.CurrentCell = this.dgProfiles.Rows[index2].Cells[0];
				this.dgProfiles.BeginEdit(true);
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0002052C File Offset: 0x0001E72C
		private int CheckExistingKey(byte[] key)
		{
			foreach (DataGridViewRow dataGridViewRow in ((IEnumerable)this.dgProfiles.Rows))
			{
				if (dataGridViewRow.Tag.ToString() == Convert.ToBase64String(key))
				{
					return dataGridViewRow.Index;
				}
			}
			for (int i = 0; i < key.Length; i++)
			{
				if (key[i] != 0)
				{
					return -2;
				}
			}
			return -1;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x000205C0 File Offset: 0x0001E7C0
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000205D0 File Offset: 0x0001E7D0
		private void btnSave_Click(object sender, EventArgs e)
		{
			if (!this.ValidateProfiles())
			{
				MessageBox.Show(Resources.errDuplicateProfile);
				return;
			}
			foreach (DataGridViewRow dataGridViewRow in ((IEnumerable)this.dgProfiles.Rows))
			{
				string text = (string)dataGridViewRow.Cells[1].Value;
				string text2 = (string)dataGridViewRow.Cells[0].Value;
				if (text2.Trim().Length == 0)
				{
					this.dgProfiles.CurrentCell = dataGridViewRow.Cells[0];
					MessageBox.Show("Please enter valid name for the profile.");
					return;
				}
				if (this.m_registered.ContainsKey(text))
				{
					Dictionary<string, object> dictionary = this.m_registered[text] as Dictionary<string, object>;
					if ((string)dictionary["friendly_name"] != text2)
					{
						this.RenamePSNID(text, text2);
					}
				}
				else if (!this.RegisterPSNID(text, text2))
				{
					MessageBox.Show("Error occurred while updating PSN ID " + dataGridViewRow.Cells[1].Value);
				}
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00020728 File Offset: 0x0001E928
		private bool RegisterPSNID(string psnId, string name)
		{
			WebClientEx webClientEx = new WebClientEx();
			webClientEx.Credentials = Util.GetNetworkCredential();
			webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
			webClientEx.Encoding = Encoding.UTF8;
			byte[] bytes = webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.UTF8.GetBytes(string.Format("{{\"action\":\"REGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\",\"friendly_name\":\"{2}\"}}", Util.GetUserId(), psnId.Trim(), name.Trim())));
			string @string = Encoding.UTF8.GetString(bytes);
			Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(@string, typeof(Dictionary<string, object>)) as Dictionary<string, object>;
			return dictionary.ContainsKey("status") && (string)dictionary["status"] == "OK";
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x000207F4 File Offset: 0x0001E9F4
		private bool UnregisterPSNID(string psnId)
		{
			WebClientEx webClientEx = new WebClientEx();
			webClientEx.Credentials = Util.GetNetworkCredential();
			webClientEx.Encoding = Encoding.UTF8;
			webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
			byte[] bytes = webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.UTF8.GetBytes(string.Format("{{\"action\":\"UNREGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\"}}", Util.GetUserId(), psnId)));
			string @string = Encoding.UTF8.GetString(bytes);
			Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(@string, typeof(Dictionary<string, object>)) as Dictionary<string, object>;
			return dictionary.ContainsKey("status") && (string)dictionary["status"] == "OK";
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x000208B4 File Offset: 0x0001EAB4
		private bool ValidateProfiles()
		{
			for (int i = 0; i < this.dgProfiles.Rows.Count; i++)
			{
				for (int j = i + 1; j < this.dgProfiles.Rows.Count; j++)
				{
					if (this.dgProfiles.Rows[i].Cells[0].Value.ToString() == this.dgProfiles.Rows[j].Cells[0].Value.ToString())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00020954 File Offset: 0x0001EB54
		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgProfiles.SelectedRows.Count == 1)
			{
				if (this.UnregisterPSNID((string)this.dgProfiles.SelectedRows[0].Cells[1].Value))
				{
					this.dgProfiles.Rows.Remove(this.dgProfiles.SelectedRows[0]);
					return;
				}
				MessageBox.Show("Can not unregister PSN ID");
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x000209D0 File Offset: 0x0001EBD0
		private void renameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgProfiles.SelectedRows.Count == 1)
			{
				this.dgProfiles.CurrentCell = this.dgProfiles.SelectedRows[0].Cells[0];
				this.dgProfiles.BeginEdit(true);
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00020A24 File Offset: 0x0001EC24
		private bool RenamePSNID(string psnId, string name)
		{
			WebClientEx webClientEx = new WebClientEx();
			webClientEx.Credentials = Util.GetNetworkCredential();
			webClientEx.Encoding = Encoding.UTF8;
			webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
			byte[] bytes = webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.UTF8.GetBytes(string.Format("{{\"action\":\"RENAME_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\",\"friendly_name\":\"{2}\"}}", Util.GetUserId(), psnId, name)));
			string @string = Encoding.UTF8.GetString(bytes);
			new JavaScriptSerializer().Deserialize(@string, typeof(Dictionary<string, object>));
			return true;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00020AB4 File Offset: 0x0001ECB4
		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			if (this.dgProfiles.SelectedRows.Count != 1)
			{
				e.Cancel = true;
				return;
			}
			if (!(bool)this.dgProfiles.SelectedRows[0].Tag)
			{
				this.deleteToolStripMenuItem.Enabled = false;
				return;
			}
			this.deleteToolStripMenuItem.Enabled = true;
		}

		// Token: 0x0400028D RID: 653
		private const string REGISTER_PSNID = "{{\"action\":\"REGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\",\"friendly_name\":\"{2}\"}}";

		// Token: 0x0400028E RID: 654
		private const string UNREGISTER_PSNID = "{{\"action\":\"UNREGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\"}}";

		// Token: 0x0400028F RID: 655
		private const string RENAME_PSNID = "{{\"action\":\"RENAME_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\",\"friendly_name\":\"{2}\"}}";

		// Token: 0x04000290 RID: 656
		private string m_newPSN_ID;

		// Token: 0x04000291 RID: 657
		private Dictionary<string, object> m_registered;
	}
}
