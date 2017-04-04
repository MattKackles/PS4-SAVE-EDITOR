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
	// Token: 0x02000009 RID: 9
	public partial class CancelPSNIDs : Form
	{
		// Token: 0x06000060 RID: 96 RVA: 0x00006EB0 File Offset: 0x000050B0
		public CancelPSNIDs(Dictionary<string, object> registered)
		{
			this.InitializeComponent();
			this.Text = Resources.titleCancelAccount;
			base.CenterToScreen();
			this.btnCancel.Text = Resources.btnCancellation;
			this.btnClose.Text = Resources.btnClose;
			this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.dataGridView1.SelectionChanged += new EventHandler(this.dataGridView1_SelectionChanged);
			this.dataGridView1.MultiSelect = false;
			this.btnCancel.Enabled = false;
			foreach (string current in registered.Keys)
			{
				Dictionary<string, object> dictionary = registered[current] as Dictionary<string, object>;
				int index = this.dataGridView1.Rows.Add();
				this.dataGridView1.Rows[index].Cells[0].Value = dictionary["friendly_name"];
				this.dataGridView1.Rows[index].Tag = current;
				this.dataGridView1.Rows[index].Cells[0].Tag = true;
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00007018 File Offset: 0x00005218
		private void dataGridView1_SelectionChanged(object sender, EventArgs e)
		{
			this.btnCancel.Enabled = false;
			foreach (DataGridViewRow dataGridViewRow in this.dataGridView1.SelectedRows)
			{
				if ((bool)dataGridViewRow.Cells[0].Tag)
				{
					this.btnCancel.Enabled = true;
					break;
				}
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000070A4 File Offset: 0x000052A4
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000710C File Offset: 0x0000530C
		private void CancelPSNIDs_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00007110 File Offset: 0x00005310
		private void btnCancel_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(Resources.msgConfirmDeactivateAccount, Resources.warnTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				foreach (DataGridViewRow dataGridViewRow in ((IEnumerable)this.dataGridView1.Rows))
				{
					if (dataGridViewRow.Selected && (bool)dataGridViewRow.Cells[0].Tag)
					{
						this.UnregisterPSNID((string)dataGridViewRow.Tag);
					}
				}
				base.DialogResult = DialogResult.Yes;
				base.Close();
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000071BC File Offset: 0x000053BC
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

		// Token: 0x06000066 RID: 102 RVA: 0x0000727A File Offset: 0x0000547A
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x04000047 RID: 71
		private const string UNREGISTER_PSNID = "{{\"action\":\"UNREGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\"}}";
	}
}
