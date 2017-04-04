using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
using Microsoft.Win32;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x02000005 RID: 5
	public partial class AdvancedEdit : Form
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00003DD0 File Offset: 0x00001FD0
		public AdvancedEdit(game game, Dictionary<string, byte[]> data, byte[] dependentData)
		{
			this.InitializeComponent();
			base.KeyDown += new KeyEventHandler(this.AdvancedEdit_KeyDown);
			this.btnFindPrev.Click += new EventHandler(this.button1_Click);
			this.txtSearchValue.TextChanged += new EventHandler(this.txtSearchValue_TextChanged);
			this.txtSearchValue.KeyDown += new KeyEventHandler(this.txtSearchValue_KeyDown);
			this.txtSearchValue.KeyPress += new KeyPressEventHandler(this.txtSearchValue_KeyPress);
			this.btnFind.Click += new EventHandler(this.btnFind_Click);
			this.hexBox1.KeyDown += new KeyEventHandler(this.hexBox1_KeyDown);
			this.hexBox1.SelectionBackColor = Color.FromArgb(0, 175, 255);
			this.hexBox1.ShadowSelectionColor = Color.FromArgb(204, 240, 255);
			this.lstCheats.DrawMode = DrawMode.OwnerDrawFixed;
			this.lstCheats.DrawItem += new DrawItemEventHandler(this.lstCheats_DrawItem);
			this.lstValues.DrawMode = DrawMode.OwnerDrawFixed;
			this.lstValues.DrawItem += new DrawItemEventHandler(this.lstValues_DrawItem);
			this.DoubleBuffered = true;
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
			this.btnApply.BackColor = SystemColors.ButtonFace;
			this.btnApply.ForeColor = Color.Black;
			this.btnClose.BackColor = SystemColors.ButtonFace;
			this.btnClose.ForeColor = Color.Black;
			this.btnFind.BackColor = SystemColors.ButtonFace;
			this.btnFind.ForeColor = Color.Black;
			this.btnFindPrev.BackColor = SystemColors.ButtonFace;
			this.btnFindPrev.ForeColor = Color.Black;
			this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.label1.BackColor = (this.lblAddress.BackColor = (this.lblCheatCodes.BackColor = (this.lblCheats.BackColor = (this.lblGameName.BackColor = (this.lblOffset.BackColor = (this.lblOffsetValue.BackColor = (this.lblProfile.BackColor = Color.Transparent)))))));
			this.lblProfile.Visible = false;
			this.cbProfile.Visible = false;
			this.m_DirtyFiles = new List<string>();
			this.m_saveFilesData = data;
			this.btnFind.Text = Resources.btnFind;
			this.btnFindPrev.Text = Resources.btnFindPrev;
			this.lblProfile.Text = Resources.lblProfile;
			this.DependentData = dependentData;
			this.SetLabels();
			this.FillProfiles();
			this.lblGameName.Text = game.name;
			this.m_game = game;
			base.CenterToScreen();
			this.btnApply.Enabled = false;
			this.m_bTextMode = false;
			this.lstValues.SelectedIndexChanged += new EventHandler(this.lstValues_SelectedIndexChanged);
			this.lstCheats.SelectedIndexChanged += new EventHandler(this.lstCheats_SelectedIndexChanged);
			this.cbSaveFiles.SelectedIndexChanged += new EventHandler(this.cbSaveFiles_SelectedIndexChanged);
			container targetGameFolder = this.m_game.GetTargetGameFolder();
			if (targetGameFolder != null)
			{
				this.cbSaveFiles.Sorted = false;
				foreach (string current in data.Keys)
				{
					this.cbSaveFiles.Items.Add(current);
				}
				if (this.cbSaveFiles.Items.Count > 0)
				{
					this.cbSaveFiles.SelectedIndex = 0;
				}
			}
			if (this.cbSaveFiles.Items.Count == 1)
			{
				this.cbSaveFiles.Enabled = false;
			}
			this.btnApply.Click += new EventHandler(this.btnApply_Click);
			this.btnClose.Click += new EventHandler(this.btnClose_Click);
			if (this.lstCheats.Items.Count > 0)
			{
				this.lstCheats.SelectedIndex = 0;
			}
			base.ResizeBegin += delegate(object s, EventArgs e)
			{
				base.SuspendLayout();
				this.panel1.BackColor = Color.FromArgb(0, 138, 213);
				this._resizeInProgress = true;
			};
			base.ResizeEnd += delegate(object s, EventArgs e)
			{
				base.ResumeLayout(true);
				this._resizeInProgress = false;
				this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
				base.Invalidate(true);
			};
			base.SizeChanged += delegate(object s, EventArgs e)
			{
				if (base.WindowState == FormWindowState.Maximized)
				{
					this._resizeInProgress = false;
					this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
					base.Invalidate(true);
				}
			};
			this.cbSaveFiles.Width = Math.Min(200, this.ComboBoxWidth(this.cbSaveFiles));
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00004284 File Offset: 0x00002484
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 274 && m.WParam == new IntPtr(61488))
			{
				this.panel1.BackColor = Color.FromArgb(0, 138, 213);
				this._resizeInProgress = true;
			}
			base.WndProc(ref m);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000042E0 File Offset: 0x000024E0
		private int ComboBoxWidth(ComboBox myCombo)
		{
			int num = 0;
			foreach (object current in myCombo.Items)
			{
				int width = TextRenderer.MeasureText(myCombo.GetItemText(current), myCombo.Font).Width;
				if (width > num)
				{
					num = width;
				}
			}
			return num + SystemInformation.VerticalScrollBarWidth;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00004360 File Offset: 0x00002560
		private void lstValues_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0)
			{
				return;
			}
			e.DrawBackground();
			Graphics graphics = e.Graphics;
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				graphics.FillRectangle(new SolidBrush(Color.FromArgb(72, 187, 97)), new Rectangle(e.Bounds.Left, e.Bounds.Top, this.lstValues.Width, e.Bounds.Height));
				e.Graphics.DrawString((string)this.lstValues.Items[e.Index], e.Font, new SolidBrush(Color.White), e.Bounds, StringFormat.GenericDefault);
			}
			else
			{
				e.Graphics.DrawString((string)this.lstValues.Items[e.Index], e.Font, new SolidBrush(Color.Black), new Rectangle(e.Bounds.Left, e.Bounds.Top, this.lstValues.Width, e.Bounds.Height), StringFormat.GenericDefault);
			}
			e.DrawFocusRectangle();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000044B0 File Offset: 0x000026B0
		private void lstCheats_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0)
			{
				return;
			}
			e.DrawBackground();
			Graphics graphics = e.Graphics;
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 175, 255)), e.Bounds);
				e.Graphics.DrawString((string)this.lstCheats.Items[e.Index], e.Font, new SolidBrush(Color.White), e.Bounds, StringFormat.GenericDefault);
			}
			else
			{
				e.Graphics.DrawString((string)this.lstCheats.Items[e.Index], e.Font, new SolidBrush(Color.Black), e.Bounds, StringFormat.GenericDefault);
			}
			e.DrawFocusRectangle();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00004594 File Offset: 0x00002794
		protected override void OnPaint(PaintEventArgs e)
		{
			if (this._resizeInProgress)
			{
				return;
			}
			base.OnPaint(e);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000045A8 File Offset: 0x000027A8
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if (this._resizeInProgress)
			{
				return;
			}
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00004618 File Offset: 0x00002818
		private void txtSaveData_TextChanged(object sender, EventArgs e)
		{
			this.btnApply.Enabled = true;
			if (this.m_DirtyFiles.IndexOf(this.m_cursaveFile) < 0)
			{
				this.m_DirtyFiles.Add(this.m_cursaveFile);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000464C File Offset: 0x0000284C
		private void SetLabels()
		{
			this.lblOffset.Text = Resources.lblOffset;
			this.lblCheatCodes.Text = Resources.lblCodes;
			this.lblCheats.Text = Resources.lblCheats;
			this.btnApply.Text = Resources.btnApplyDownload;
			this.btnClose.Text = Resources.btnClose;
			this.Text = Resources.titleAdvEdit;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000046B4 File Offset: 0x000028B4
		private void hexBox1_SelectionStartChanged(object sender, EventArgs e)
		{
			long num = (long)this.hexBox1.BytesPerLine * (this.hexBox1.CurrentLine - 1L) + (this.hexBox1.CurrentPositionInLine - 1L);
			this.lblOffsetValue.Text = "0x" + string.Format("{0:X}", num).PadLeft(8, '0');
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00004719 File Offset: 0x00002919
		protected override void OnClosed(EventArgs e)
		{
			this.hexBox1.Dispose();
			base.OnClosed(e);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000472D File Offset: 0x0000292D
		private void provider_LengthChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000472F File Offset: 0x0000292F
		private void provider_Changed(object sender, EventArgs e)
		{
			this.btnApply.Enabled = true;
			if (this.m_DirtyFiles.IndexOf(this.m_cursaveFile) < 0)
			{
				this.m_DirtyFiles.Add(this.m_cursaveFile);
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00004770 File Offset: 0x00002970
		private void lstCheats_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.m_bTextMode)
			{
				return;
			}
			this.lstValues.Items.Clear();
			int selectedIndex = this.lstCheats.SelectedIndex;
			string text = this.cbSaveFiles.SelectedItem.ToString();
			if (selectedIndex >= 0)
			{
				container targetGameFolder = this.m_game.GetTargetGameFolder();
				if (targetGameFolder != null)
				{
					foreach (file current in targetGameFolder.files._files)
					{
						List<string> saveFiles = this.m_game.GetSaveFiles();
						if (saveFiles != null)
						{
							foreach (string current2 in saveFiles)
							{
								if (Path.GetFileName(current2) == text || Util.IsMatch(text, current.filename))
								{
									cheat cheat = current.GetCheat(this.lstCheats.Items[selectedIndex].ToString());
									if (cheat != null)
									{
										string code = cheat.code;
										if (!string.IsNullOrEmpty(code))
										{
											string[] array = code.Trim().Split(new char[]
											{
												' ',
												'\r',
												'\n'
											});
											for (int i = 0; i < array.Length - 1; i += 2)
											{
												this.lstValues.Items.Add(array[i] + " " + array[i + 1]);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00004934 File Offset: 0x00002B34
		private void lstValues_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lstValues.SelectedIndex < 0 || this.m_bTextMode)
			{
				return;
			}
			if (this.lstValues.Items[0].ToString()[0] == 'F')
			{
				return;
			}
			this.hexBox1.SelectAddresses.Clear();
			string text = this.lstValues.Items[this.lstValues.SelectedIndex].ToString();
			string[] array = text.Split(new char[]
			{
				' '
			});
			int bitCode;
			long memLocation = cheat.GetMemLocation(array[0], out bitCode);
			if (this.provider.Length > memLocation)
			{
				this.hexBox1.SelectAddresses.Add(memLocation, cheat.GetBitCodeBytes(bitCode));
				this.hexBox1.ScrollByteIntoView(memLocation);
				this.hexBox1.Invalidate();
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00004A08 File Offset: 0x00002C08
		private void btnApply_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(Resources.warnOverwriteAdv, Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
			{
				return;
			}
			if (!this.m_bTextMode)
			{
				this.provider.ApplyChanges();
				if (this.m_cursaveFile == null)
				{
					this.m_cursaveFile = this.cbSaveFiles.SelectedItem.ToString();
				}
				this.m_saveFilesData[this.m_cursaveFile] = this.provider.Bytes.ToArray();
			}
			else
			{
				container targetGameFolder = this.m_game.GetTargetGameFolder();
				file gameFile = this.m_game.GetGameFile(targetGameFolder, this.m_cursaveFile);
				if (gameFile.TextMode == 1)
				{
					this.m_saveFilesData[this.m_cursaveFile] = Encoding.UTF8.GetBytes(this.txtSaveData.Text);
				}
				else if (gameFile.TextMode == 3)
				{
					this.m_saveFilesData[this.m_cursaveFile] = Encoding.Unicode.GetBytes(this.txtSaveData.Text);
				}
				else
				{
					this.m_saveFilesData[this.m_cursaveFile] = Encoding.ASCII.GetBytes(this.txtSaveData.Text);
				}
			}
			if (this.m_game.GetTargetGameFolder() == null)
			{
				MessageBox.Show(Resources.errSaveData, Resources.msgError);
				return;
			}
			this.m_game.GetTargetGameFolder();
			List<string> dirtyFiles = this.m_DirtyFiles;
			List<string> list = new List<string>();
			foreach (string current in dirtyFiles)
			{
				string text = Path.Combine(ZipUtil.GetPs3SeTempFolder(), "_file_" + Path.GetFileName(current));
				File.WriteAllBytes(text, this.m_saveFilesData[Path.GetFileName(current)]);
				if (list.IndexOf(text) < 0)
				{
					list.Add(text);
				}
			}
			List<string> containerFiles = this.m_game.GetContainerFiles();
			string text2 = this.m_game.LocalSaveFolder.Substring(0, this.m_game.LocalSaveFolder.Length - 4);
			string hash = Util.GetHash(text2);
			bool cache = Util.GetCache(hash);
			string text3 = this.m_game.ToString(list, "encrypt");
			if (cache)
			{
				containerFiles.Remove(text2);
				text3 = text3.Replace("<pfs><name>" + Path.GetFileNameWithoutExtension(this.m_game.LocalSaveFolder) + "</name></pfs>", string.Concat(new string[]
				{
					"<pfs><name>",
					Path.GetFileNameWithoutExtension(this.m_game.LocalSaveFolder),
					"</name><md5>",
					hash,
					"</md5></pfs>"
				}));
			}
			list.AddRange(containerFiles);
			string tempFolder = Util.GetTempFolder();
			string text4 = tempFolder + "ps4_list.xml";
			File.WriteAllText(text4, text3);
			list.Add(text4);
			string profile = (string)this.cbProfile.SelectedItem;
			AdvancedSaveUploaderForEncrypt advancedSaveUploaderForEncrypt = new AdvancedSaveUploaderForEncrypt(list.ToArray(), this.m_game, profile, "encrypt");
			advancedSaveUploaderForEncrypt.ShowDialog();
			File.Delete(text4);
			Directory.Delete(ZipUtil.GetPs3SeTempFolder(), true);
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00004D40 File Offset: 0x00002F40
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00004D4F File Offset: 0x00002F4F
		private void txtSearchValue_KeyPress(object sender, KeyPressEventArgs e)
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00004D51 File Offset: 0x00002F51
		private void hexBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F3)
			{
				this.Search(e.Shift, false);
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00004D6C File Offset: 0x00002F6C
		private void txtSearchValue_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.Search(false, true);
			}
			if (this.m_bTextMode)
			{
				return;
			}
			if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back || e.Control || e.KeyCode == Keys.Home || e.KeyCode == Keys.End || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
			{
				return;
			}
			if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9 && !e.Shift) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9 && !e.Shift) || (this.txtSearchValue.SelectionStart == 1 && e.KeyCode == Keys.X && this.txtSearchValue.Text[0] == '0') || (this.txtSearchValue.Text.StartsWith("0x") && e.KeyCode >= Keys.A && e.KeyCode <= Keys.F))
			{
				return;
			}
			e.SuppressKeyPress = true;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00004E70 File Offset: 0x00003070
		private void Search(bool bBackward, bool bStart)
		{
			if (this.m_bTextMode)
			{
				this.SerachText(bBackward, bStart);
				return;
			}
			byte[] bytes = (this.hexBox1.ByteProvider as DynamicByteProvider).Bytes.GetBytes();
			MemoryStream memoryStream = new MemoryStream(bytes);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			if (bStart)
			{
				binaryReader.BaseStream.Position = 0L;
				this.hexBox1.SelectionStart = 0L;
				this.hexBox1.SelectionLength = 0L;
			}
			else if (this.hexBox1.SelectionStart >= 0L)
			{
				binaryReader.BaseStream.Position = this.hexBox1.SelectionStart + this.hexBox1.SelectionLength;
			}
			long num = binaryReader.BaseStream.Position;
			uint num2;
			uint num3;
			int searchValues = this.GetSearchValues(out num2, out num3);
			if (searchValues == 0)
			{
				MessageBox.Show(Resources.errInvalidHex, Resources.msgError);
				this.txtSearchValue.Focus();
				return;
			}
			if (searchValues < 0)
			{
				MessageBox.Show(Resources.errIncorrectValue, Resources.msgError);
				this.txtSearchValue.Focus();
				return;
			}
			while (binaryReader.BaseStream.Position >= 0L && binaryReader.BaseStream.Position < binaryReader.BaseStream.Length + (long)(bBackward ? searchValues : (1 - searchValues)))
			{
				uint num4 = this.ReadValue(binaryReader, searchValues, bBackward);
				if (num4 == num2 || num4 == num3)
				{
					this.hexBox1.Select(binaryReader.BaseStream.Position - (long)searchValues, (long)searchValues);
					this.hexBox1.ScrollByteIntoView(binaryReader.BaseStream.Position);
					this.hexBox1.Focus();
					break;
				}
				if (bBackward)
				{
					num -= 1L;
					if (num < 0L)
					{
						break;
					}
				}
				else
				{
					num += 1L;
					if (num > binaryReader.BaseStream.Length)
					{
						break;
					}
				}
				binaryReader.BaseStream.Position = num;
			}
			binaryReader.Close();
			memoryStream.Close();
			memoryStream.Dispose();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00005044 File Offset: 0x00003244
		public int FindMyText(string text, int start, bool bReverse)
		{
			int result = -1;
			if (text.Length > 0 && start >= 0)
			{
				RichTextBoxFinds richTextBoxFinds = RichTextBoxFinds.None;
				int num = this.txtSaveData.Text.Length;
				if (bReverse)
				{
					richTextBoxFinds |= RichTextBoxFinds.Reverse;
					num = start - text.Length;
					start = 0;
					if (num < 0)
					{
						num = this.txtSaveData.Text.Length - 1;
					}
				}
				int num2 = this.txtSaveData.Find(text, start, num, richTextBoxFinds);
				if (num2 >= 0)
				{
					result = num2;
				}
			}
			return result;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000050B8 File Offset: 0x000032B8
		private void SerachText(bool bBackward, bool bStart)
		{
			int start = 0;
			if (!bStart)
			{
				start = this.txtSaveData.SelectionStart + this.txtSaveData.SelectionLength;
			}
			int num = this.FindMyText(this.txtSearchValue.Text, start, bBackward);
			if (num < 0)
			{
				this.txtSaveData.Select(0, 0);
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00005108 File Offset: 0x00003308
		private uint ReadValue(BinaryReader reader, int size, bool bBackward)
		{
			if (bBackward)
			{
				if (reader.BaseStream.Position < (long)(2 * size))
				{
					reader.BaseStream.Position = reader.BaseStream.Length - 1L;
				}
				reader.BaseStream.Position -= (long)(2 * size);
			}
			if (size == 1)
			{
				return (uint)reader.ReadByte();
			}
			if (size == 2)
			{
				return (uint)reader.ReadUInt16();
			}
			if (size == 3)
			{
				return (uint)((int)reader.ReadUInt16() << 8 | (int)reader.ReadByte());
			}
			return reader.ReadUInt32();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00005188 File Offset: 0x00003388
		private int GetSearchValues(out uint val1, out uint val2)
		{
			uint num;
			int num2;
			try
			{
				if (this.txtSearchValue.Text.StartsWith("0x"))
				{
					num = uint.Parse(this.txtSearchValue.Text.Substring(2), NumberStyles.HexNumber);
					num2 = this.txtSearchValue.Text.Length - 2;
					if (num2 != 1 && num2 != 2 && num2 != 4 && num2 != 6 && num2 != 8)
					{
						val1 = (val2 = 0u);
						int result = 0;
						return result;
					}
				}
				else
				{
					num = uint.Parse(this.txtSearchValue.Text);
					num2 = num.ToString("X").Length;
				}
			}
			catch (Exception)
			{
				val1 = 0u;
				val2 = 0u;
				int result = -1;
				return result;
			}
			int result2;
			switch (num2)
			{
			case 1:
			case 2:
				result2 = 1;
				break;
			case 3:
			case 4:
				result2 = 2;
				break;
			case 5:
			case 6:
				result2 = 3;
				break;
			case 7:
			case 8:
				result2 = 4;
				break;
			default:
				result2 = 4;
				break;
			}
			val1 = num;
			switch (result2)
			{
			case 2:
				val2 = ((num & 255u) << 8 | (num & 65280u) >> 8);
				break;
			case 3:
				val2 = ((num & 65280u) << 8 | (num & 16711680u) >> 8 | (num & 255u));
				break;
			case 4:
				val2 = ((num & 255u) << 24 | (num & 65280u) << 8 | (num & 16711680u) >> 8 | (num & 4278190080u) >> 24);
				break;
			default:
				val2 = num;
				break;
			}
			return result2;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000530C File Offset: 0x0000350C
		private void txtSearchValue_TextChanged(object sender, EventArgs e)
		{
			if (this.m_bTextMode)
			{
				return;
			}
			if (!this.txtSearchValue.Text.StartsWith("0x"))
			{
				try
				{
					int.Parse(this.txtSearchValue.Text);
				}
				catch (OverflowException)
				{
					this.txtSearchValue.Text = this.txtSearchValue.Text.Substring(0, this.txtSearchValue.Text.Length - 1);
					this.txtSearchValue.SelectionStart = this.txtSearchValue.Text.Length;
				}
				catch (Exception)
				{
					this.txtSearchValue.Text = "";
				}
			}
			if (this.txtSearchValue.Text.Length > 0)
			{
				this.btnFind.Enabled = true;
				this.btnFindPrev.Enabled = true;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000053F4 File Offset: 0x000035F4
		private void btnFind_Click(object sender, EventArgs e)
		{
			this.Search(false, false);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000053FE File Offset: 0x000035FE
		private void button1_Click(object sender, EventArgs e)
		{
			this.Search(true, false);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00005408 File Offset: 0x00003608
		private void AdvancedEdit_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.m_bTextMode)
			{
				return;
			}
			if (e.KeyCode == Keys.G && e.Modifiers == Keys.Control)
			{
				Goto @goto = new Goto(this.provider.Length);
				if (@goto.ShowDialog() == DialogResult.OK)
				{
					if (@goto.AddressLocation < this.provider.Length)
					{
						this.hexBox1.ScrollByteIntoView(@goto.AddressLocation);
						this.hexBox1.Select(@goto.AddressLocation, 1L);
						this.hexBox1.Invalidate();
						return;
					}
					MessageBox.Show(Resources.errInvalidAddress);
				}
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000054A0 File Offset: 0x000036A0
		private void FillProfiles()
		{
			this.cbProfile.Items.Add("None");
			using (RegistryKey currentUser = Registry.CurrentUser)
			{
				using (RegistryKey registryKey = currentUser.CreateSubKey(Util.GetRegistryBase() + "\\Profiles"))
				{
					string b = (string)registryKey.GetValue(null);
					string[] valueNames = registryKey.GetValueNames();
					string[] array = valueNames;
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						if (!string.IsNullOrEmpty(text))
						{
							int selectedIndex = this.cbProfile.Items.Add(text);
							if ((string)registryKey.GetValue(text) == b)
							{
								this.cbProfile.SelectedIndex = selectedIndex;
							}
						}
					}
				}
			}
			if (this.cbProfile.SelectedIndex < 0)
			{
				this.cbProfile.SelectedIndex = 0;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000055A0 File Offset: 0x000037A0
		private void cbSaveFiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!this.m_bTextMode && this.provider != null && this.provider.Length > 0L)
			{
				this.provider.ApplyChanges();
			}
			container targetGameFolder = this.m_game.GetTargetGameFolder();
			if (!string.IsNullOrEmpty(this.m_cursaveFile) && this.m_saveFilesData.ContainsKey(this.m_cursaveFile))
			{
				file gameFile = this.m_game.GetGameFile(targetGameFolder, this.m_cursaveFile);
				if (gameFile.TextMode == 0)
				{
					this.m_saveFilesData[this.m_cursaveFile] = this.provider.Bytes.ToArray();
				}
				else if (gameFile.TextMode == 2)
				{
					this.m_saveFilesData[this.m_cursaveFile] = Encoding.ASCII.GetBytes(this.txtSaveData.Text);
				}
				else if (gameFile.TextMode == 3)
				{
					this.m_saveFilesData[this.m_cursaveFile] = Encoding.Unicode.GetBytes(this.txtSaveData.Text);
				}
				else
				{
					this.m_saveFilesData[this.m_cursaveFile] = Encoding.UTF8.GetBytes(this.txtSaveData.Text);
				}
			}
			this.lstCheats.Items.Clear();
			this.lstValues.Items.Clear();
			this.m_cursaveFile = this.cbSaveFiles.SelectedItem.ToString();
			List<cheat> cheats = this.m_game.GetCheats(this.m_game.LocalSaveFolder.Substring(0, this.m_game.LocalSaveFolder.Length - 4), this.m_cursaveFile);
			if (cheats != null)
			{
				foreach (cheat current in cheats)
				{
					if (current.id == "-1")
					{
						this.lstCheats.Items.Add(current.name);
					}
				}
			}
			if (this.lstCheats.Items.Count > 0)
			{
				this.lstCheats.SelectedIndex = 0;
			}
			file gameFile2 = this.m_game.GetGameFile(targetGameFolder, this.m_cursaveFile);
			if (gameFile2 != null && gameFile2.TextMode > 0)
			{
				this.txtSaveData.Visible = true;
				this.hexBox1.Visible = false;
				if (gameFile2.TextMode == 1)
				{
					this.txtSaveData.Text = Encoding.UTF8.GetString(this.m_saveFilesData[this.m_cursaveFile]);
				}
				if (gameFile2.TextMode == 3)
				{
					this.txtSaveData.Text = Encoding.Unicode.GetString(this.m_saveFilesData[this.m_cursaveFile]);
				}
				else
				{
					this.txtSaveData.Text = Encoding.ASCII.GetString(this.m_saveFilesData[this.m_cursaveFile]);
				}
				this.m_bTextMode = true;
				this.txtSaveData.TextChanged += new EventHandler(this.txtSaveData_TextChanged);
				this.lblAddress.Visible = false;
				this.lblOffset.Visible = false;
				this.txtSaveData.HideSelection = false;
				return;
			}
			this.hexBox1.Visible = true;
			this.lblAddress.Visible = true;
			this.lblOffset.Visible = true;
			this.txtSaveData.HideSelection = true;
			this.txtSaveData.Visible = false;
			this.provider = new DynamicByteProvider(this.m_saveFilesData[this.m_cursaveFile]);
			this.provider.Changed += new EventHandler(this.provider_Changed);
			this.provider.LengthChanged += new EventHandler(this.provider_LengthChanged);
			this.hexBox1.ByteProvider = this.provider;
			this.hexBox1.BytesPerLine = 16;
			this.hexBox1.UseFixedBytesPerLine = true;
			this.hexBox1.VScrollBarVisible = true;
			this.hexBox1.LineInfoVisible = true;
			this.hexBox1.StringViewVisible = true;
			this.hexBox1.SelectionStartChanged += new EventHandler(this.hexBox1_SelectionStartChanged);
		}

		// Token: 0x0400001F RID: 31
		private DynamicByteProvider provider;

		// Token: 0x04000020 RID: 32
		private game m_game;

		// Token: 0x04000021 RID: 33
		private bool m_bTextMode;

		// Token: 0x04000022 RID: 34
		private byte[] DependentData;

		// Token: 0x04000023 RID: 35
		private Dictionary<string, byte[]> m_saveFilesData;

		// Token: 0x04000024 RID: 36
		private List<string> m_DirtyFiles;

		// Token: 0x04000025 RID: 37
		private string m_cursaveFile;

		// Token: 0x04000026 RID: 38
		private bool _resizeInProgress;
	}
}
