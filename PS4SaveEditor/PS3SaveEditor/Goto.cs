using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x02000041 RID: 65
	public partial class Goto : Form
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00010F27 File Offset: 0x0000F127
		// (set) Token: 0x060002DD RID: 733 RVA: 0x00010F2F File Offset: 0x0000F12F
		public long AddressLocation
		{
			get;
			set;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00010F38 File Offset: 0x0000F138
		public Goto(long maxLength)
		{
			this.m_maxLength = maxLength;
			this.InitializeComponent();
			this.Text = Resources.titleGoto;
			this.lblEnterLoc.Text = Resources.lblEnterLoc;
			base.CenterToScreen();
			this.btnOk.Enabled = false;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00010F88 File Offset: 0x0000F188
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (this.txtLocation.Text.StartsWith("0x"))
			{
				this.AddressLocation = long.Parse(this.txtLocation.Text.Substring(2), NumberStyles.HexNumber);
			}
			else
			{
				this.AddressLocation = long.Parse(this.txtLocation.Text);
			}
			base.Close();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00010FEB File Offset: 0x0000F1EB
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00010FF4 File Offset: 0x0000F1F4
		private void txtLocation_TextChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtLocation.Text))
			{
				this.btnOk.Enabled = false;
				return;
			}
			long num2;
			if (this.txtLocation.Text.StartsWith("0x"))
			{
				if (this.txtLocation.Text.Length <= 2)
				{
					this.btnOk.Enabled = false;
					return;
				}
				long num = long.Parse(this.txtLocation.Text.Substring(2), NumberStyles.HexNumber);
				if (num > this.m_maxLength)
				{
					this.btnOk.Enabled = false;
					return;
				}
			}
			else if (long.TryParse(this.txtLocation.Text.Trim(), out num2))
			{
				if (num2 > this.m_maxLength)
				{
					this.btnOk.Enabled = false;
					return;
				}
			}
			else if (long.TryParse(this.txtLocation.Text.Trim(), NumberStyles.HexNumber, null, out num2))
			{
				this.txtLocation.Text = "0x" + this.txtLocation.Text.Trim();
				if (num2 > this.m_maxLength)
				{
					this.btnOk.Enabled = false;
					return;
				}
			}
			else
			{
				this.txtLocation.Text = "";
			}
			this.btnOk.Enabled = true;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00011134 File Offset: 0x0000F334
		private void txtLocation_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back || e.Control || e.KeyCode == Keys.Home || e.KeyCode == Keys.End || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
			{
				return;
			}
			if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9 && !e.Shift) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9 && !e.Shift) || (this.txtLocation.SelectionStart == 1 && e.KeyCode == Keys.X && this.txtLocation.Text[0] == '0') || (this.txtLocation.Text.StartsWith("0x") && e.KeyCode >= Keys.A && e.KeyCode <= Keys.F))
			{
				return;
			}
			e.SuppressKeyPress = true;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0001121D File Offset: 0x0000F41D
		private void txtLocation_KeyPress(object sender, KeyPressEventArgs e)
		{
		}

		// Token: 0x04000186 RID: 390
		private long m_maxLength;
	}
}
