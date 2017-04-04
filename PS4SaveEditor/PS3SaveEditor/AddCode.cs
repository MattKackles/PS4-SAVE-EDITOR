using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x02000003 RID: 3
	public partial class AddCode : Form
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002657 File Offset: 0x00000857
		// (set) Token: 0x0600000D RID: 13 RVA: 0x0000265F File Offset: 0x0000085F
		public string Description
		{
			get;
			set;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002668 File Offset: 0x00000868
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002670 File Offset: 0x00000870
		public string Comment
		{
			get;
			set;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000267C File Offset: 0x0000087C
		public AddCode(List<string> existingCodes)
		{
			this.InitializeComponent();
			this.m_existingCodes = existingCodes;
			this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.lblCodes.BackColor = Color.Transparent;
			this.lblComment.BackColor = Color.Transparent;
			this.lblDescription.BackColor = Color.Transparent;
			this.lblDescription.Text = Resources.lblDescription;
			this.lblComment.Text = Resources.lblComment;
			this.lblCodes.Text = Resources.lblCodes;
			this.btnSave.Text = Resources.btnSave;
			this.btnCancel.Text = Resources.btnCancel;
			base.CenterToScreen();
			this.dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
			this.dataGridView1.KeyDown += new KeyEventHandler(this.dataGridView1_KeyDown);
			this.m_bMode = AddCode.Mode.ADD_MODE;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002778 File Offset: 0x00000978
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000027E0 File Offset: 0x000009E0
		public AddCode(cheat item, List<string> existingCodes)
		{
			this.m_bMode = AddCode.Mode.EDIT_MODE;
			this.m_existingCodes = existingCodes;
			this.InitializeComponent();
			this.lblDescription.Text = Resources.lblDescription;
			this.lblComment.Text = Resources.lblComment;
			this.lblCodes.Text = Resources.lblCodes;
			this.btnSave.Text = Resources.btnSave;
			this.btnCancel.Text = Resources.btnCancel;
			this.Text = Resources.titleCodeEntry;
			base.CenterToScreen();
			this.dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
			this.dataGridView1.KeyDown += new KeyEventHandler(this.dataGridView1_KeyDown);
			this.txtCode.Text = item.ToEditableString();
			this.txtDescription.Text = item.name;
			this.txtComment.Text = item.note;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000028CC File Offset: 0x00000ACC
		private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.KeyCode < Keys.A || e.KeyCode > Keys.F) && (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) && (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
			{
				if (e.KeyCode == Keys.Delete)
				{
					return;
				}
				e.SuppressKeyPress = true;
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002930 File Offset: 0x00000B30
		private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
			{
				string s = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
				int num = 0;
				if (!int.TryParse(s, NumberStyles.HexNumber, null, out num))
				{
					this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
					MessageBox.Show(Resources.errInvalidHexCode, Resources.msgError);
					return;
				}
				this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = num.ToString("X8");
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002A24 File Offset: 0x00000C24
		public static byte[] ConvertHexStringToByteArray(string hexString)
		{
			if (hexString.Length % 2 != 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", new object[]
				{
					hexString
				}));
			}
			byte[] array = new byte[hexString.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				string s = hexString.Substring(i * 2, 2);
				array[i] = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			return array;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002A98 File Offset: 0x00000C98
		private void btnSave_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtDescription.Text.Trim()))
			{
				MessageBox.Show(Resources.errInvalidDesc, Resources.msgError);
				return;
			}
			if (this.m_existingCodes.IndexOf(this.txtDescription.Text) >= 0)
			{
				MessageBox.Show(Resources.errCheatExists, Resources.msgError);
				return;
			}
			if (this.txtCode.Text.Trim().Length == 0)
			{
				MessageBox.Show(Resources.errInvalidCode, Resources.msgError);
				return;
			}
			string[] lines = this.txtCode.Lines;
			for (int i = 0; i < lines.Length; i++)
			{
				string text = lines[i];
				if (text.Trim().Length != 17 && text.Trim().Length != 0)
				{
					MessageBox.Show(Resources.errInvalidCode, Resources.msgError);
					return;
				}
			}
			if (this.txtCode.Lines[0][0] == 'F')
			{
				if (this.txtCode.Lines.Length > 16)
				{
					MessageBox.Show(Resources.errInvalidFCode, Resources.msgError);
					return;
				}
				string text2 = this.txtCode.Text.Replace(" ", "");
				text2 = text2.Replace("\r\n", "");
				byte[] bytes = Encoding.ASCII.GetBytes(text2.Substring(0, text2.Length - 8));
				uint cRC = this.GetCRC(bytes);
				string s = text2.Substring(text2.Length - 8, 8);
				uint num = uint.Parse(s, NumberStyles.HexNumber);
				if (cRC != num)
				{
					MessageBox.Show(Resources.errInvalidCode, Resources.msgError);
					return;
				}
			}
			if (MessageBox.Show(Resources.msgConfirmCode, Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
			{
				return;
			}
			this.Description = this.txtDescription.Text;
			this.Comment = this.txtComment.Text;
			this.Code = this.txtCode.Text.Replace("\r\n", " ").TrimEnd(new char[0]);
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002CA6 File Offset: 0x00000EA6
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002CB8 File Offset: 0x00000EB8
		private void txtCheatCode_TextChanged(object sender, EventArgs e)
		{
			int arg_0B_0 = this.txtCode.SelectionStart;
			int num = this.txtCode.Lines.Length;
			if (num > 1 && (this.txtCode.Lines[num - 2].Length < 17 || this.txtCode.Lines[num - 1].Length == 0))
			{
				num--;
			}
			if (num > 128)
			{
				string[] array = new string[128];
				Array.Copy(this.txtCode.Lines, array, 128);
				this.SetLinesToCode(array);
				MessageBox.Show(string.Format(Resources.errMaxCodes, 128), this.Text);
				this.txtCode.SelectionStart = this.txtCode.TextLength;
				this.txtCode.SelectionLength = 0;
				return;
			}
			if (num > 0)
			{
				string[] lines = this.txtCode.Lines;
				this.SetLinesToCode(lines);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002DA0 File Offset: 0x00000FA0
		private void txtCheatCode_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				int num = this.txtCode.SelectionStart - this.txtCode.GetFirstCharIndexOfCurrentLine();
				int lineFromCharIndex = this.txtCode.GetLineFromCharIndex(this.txtCode.SelectionStart);
				string[] lines = this.txtCode.Lines;
				if (lines.Length > 0)
				{
					string text = lines[lineFromCharIndex];
					if (num > 0 && num >= text.Length)
					{
						e.SuppressKeyPress = true;
						return;
					}
				}
				if (num >= 17)
				{
					e.SuppressKeyPress = true;
				}
				if (num == 8)
				{
					this.txtCode.SelectionStart++;
				}
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002E34 File Offset: 0x00001034
		private void SetLinesToCode(string[] lines)
		{
			string text = "";
			int num = 0;
			int num2 = this.txtCode.SelectionStart;
			for (int i = 0; i < lines.Length; i++)
			{
				if (i < lines.Length - 1 || lines[i].Length > 0)
				{
					string text2 = lines[num].Replace(" ", "");
					for (int j = 0; j < text2.Length; j++)
					{
						if ((text2[j] < '0' || text2[j] > '9') && (text2[j] < 'a' || text2[j] > 'f') && (text2[j] < 'A' || text2[j] > 'F'))
						{
							text2 = text2.Remove(j, 1);
						}
					}
					if (text2.Length > 8)
					{
						string str = text2.Substring(0, 8);
						string str2 = text2.Substring(8, Math.Min(8, text2.Length - 8));
						text2 = str + " " + str2 + Environment.NewLine;
					}
					else
					{
						text2 += Environment.NewLine;
					}
					text += text2;
					num++;
				}
			}
			this.txtCode.GetLineFromCharIndex(num2);
			lines = this.txtCode.Lines;
			int num3 = 0;
			string[] array = lines;
			for (int k = 0; k < array.Length; k++)
			{
				string text3 = array[k];
				if (text3.Length > 0 && text3.Length > 17)
				{
					num2 = (num3 + 1) * 18;
				}
				num3++;
			}
			this.txtCode.Text = text;
			if (num2 > 0)
			{
				this.txtCode.SelectionStart = num2;
				this.txtCode.ScrollToCaret();
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002FE8 File Offset: 0x000011E8
		private void HandleCodeBackSpace(ref KeyPressEventArgs e)
		{
			int num = this.txtCode.SelectionStart - this.txtCode.GetFirstCharIndexOfCurrentLine();
			if (num < 0)
			{
				num = this.txtCode.SelectionStart;
			}
			string[] lines = this.txtCode.Lines;
			int lineFromCharIndex = this.txtCode.GetLineFromCharIndex(this.txtCode.SelectionStart);
			if (lines.Length == 0)
			{
				return;
			}
			if (num == 0 && this.txtCode.SelectionStart > 0 && lines[lineFromCharIndex].Length > 0)
			{
				e.Handled = true;
				this.txtCode.SelectionStart -= 2;
				return;
			}
			if (num == 9)
			{
				this.txtCode.SelectionStart--;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003098 File Offset: 0x00001298
		private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\b')
			{
				this.HandleCodeBackSpace(ref e);
				return;
			}
			if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= 'A' && e.KeyChar <= 'F'))
			{
				int num = this.txtCode.Lines.Length;
				if (num > 1 && this.txtCode.Lines[num - 2].Length < 17)
				{
					num--;
				}
				if (num > 128)
				{
					e.Handled = true;
					MessageBox.Show(string.Format(Resources.msgMaxCheats, 128), this.Text);
					return;
				}
				int lineFromCharIndex = this.txtCode.GetLineFromCharIndex(this.txtCode.SelectionStart);
				string text = "";
				string[] array = this.txtCode.Lines;
				if (this.txtCode.Lines.Length > 0)
				{
					text = this.txtCode.Lines[lineFromCharIndex];
				}
				else
				{
					array = new string[1];
				}
				int num2 = this.txtCode.SelectionStart - this.txtCode.GetFirstCharIndexOfCurrentLine();
				if (num2 < 0)
				{
					num2 = this.txtCode.SelectionStart;
				}
				if (this.txtCode.TextLength > this.txtCode.SelectionStart && this.txtCode.Text[this.txtCode.SelectionStart] == '\n')
				{
					num2--;
				}
				int arg_175_0 = this.txtCode.SelectionStart;
				if (num2 == 17)
				{
					this.txtCode.GetFirstCharIndexFromLine(lineFromCharIndex + 1);
					char[] array2 = array[lineFromCharIndex + 1].ToCharArray();
					if (array2.Length == 0)
					{
						array2 = new char[1];
					}
					array2[0] = e.KeyChar;
					array[lineFromCharIndex + 1] = new string(array2);
					this.SetLinesToCode(array);
					this.txtCode.SelectionStart = this.txtCode.GetFirstCharIndexFromLine(lineFromCharIndex + 1) + 1;
					if (this.txtCode.SelectionStart > 0)
					{
						this.txtCode.ScrollToCaret();
					}
					e.Handled = true;
					return;
				}
				char[] array3 = text.ToCharArray();
				if (array3.Length == 17)
				{
					if (num2 == 8)
					{
						array3[num2 + 1] = e.KeyChar;
						array[lineFromCharIndex] = new string(array3);
						this.SetLinesToCode(array);
						this.txtCode.SelectionStart += 2;
						e.Handled = true;
						return;
					}
					array3[num2] = e.KeyChar;
					array[lineFromCharIndex] = new string(array3);
					this.SetLinesToCode(array);
					this.txtCode.SelectionStart++;
					e.Handled = true;
					return;
				}
				else
				{
					if (num2 == 8 && array3.Length == 8)
					{
						char[] array4 = new char[array3.Length + 2];
						Array.Copy(array3, array4, 8);
						array4[8] = ' ';
						array4[9] = e.KeyChar;
						array[lineFromCharIndex] = new string(array4);
						this.SetLinesToCode(array);
						this.txtCode.SelectionStart += 2;
						e.Handled = true;
						return;
					}
					if (num2 == 8 && array3.Length > 8)
					{
						char[] array5 = new char[array3.Length + 1];
						Array.Copy(array3, array5, 8);
						array5[8] = ' ';
						array5[9] = e.KeyChar;
						Array.Copy(array3, 9, array5, 10, array3.Length - 9);
						array[lineFromCharIndex] = new string(array5);
						this.SetLinesToCode(array);
						this.txtCode.SelectionStart += 2;
						e.Handled = true;
						return;
					}
					if (num2 > 8)
					{
						char[] array6 = new char[array3.Length + 1];
						Array.Copy(array3, array6, num2);
						array6[num2] = e.KeyChar;
						Array.Copy(array3, num2, array6, num2 + 1, array3.Length - num2);
						array[lineFromCharIndex] = new string(array6);
						this.SetLinesToCode(array);
						this.txtCode.SelectionStart++;
						e.Handled = true;
					}
					return;
				}
			}
			else
			{
				if (e.KeyChar == '\u0001')
				{
					this.txtCode.SelectAll();
					return;
				}
				if (e.KeyChar != '\u0003' && e.KeyChar != '\r' && e.KeyChar != '\u0018' && e.KeyChar != '\u0016')
				{
					if (e.KeyChar == '\u001a')
					{
						return;
					}
					e.Handled = true;
				}
				return;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000034B8 File Offset: 0x000016B8
		private uint GetCRC(byte[] data)
		{
			Crc32Net crc32Net = new Crc32Net();
			crc32Net.ComputeHash(data);
			return crc32Net.CrcValue;
		}

		// Token: 0x04000008 RID: 8
		private const int MAX_CHEAT_CODES = 128;

		// Token: 0x04000009 RID: 9
		private AddCode.Mode m_bMode;

		// Token: 0x0400000A RID: 10
		private string m_cheatFile;

		// Token: 0x0400000B RID: 11
		public string Code;

		// Token: 0x0400000C RID: 12
		private List<string> m_existingCodes;

		// Token: 0x02000004 RID: 4
		private enum Mode
		{
			// Token: 0x0400001D RID: 29
			ADD_MODE,
			// Token: 0x0400001E RID: 30
			EDIT_MODE
		}
	}
}
