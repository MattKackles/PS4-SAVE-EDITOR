using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using CSUST.Data;
using Microsoft.Win32;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x02000106 RID: 262
	public partial class SimpleEdit : Form
	{
		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x0003AE64 File Offset: 0x00039064
		public game GameItem
		{
			get
			{
				return this.m_game;
			}
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0003AE6C File Offset: 0x0003906C
		public SimpleEdit(game gameItem, bool bShowOnly, List<string> files = null)
		{
			this.m_bShowOnly = bShowOnly;
			this.m_game = game.Copy(gameItem);
			this.m_gameFiles = files;
			this.InitializeComponent();
			this.DoubleBuffered = true;
			base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.btnApply.Enabled = false;
			this.btnApply.BackColor = SystemColors.ButtonFace;
			this.btnApply.ForeColor = Color.Black;
			this.btnClose.BackColor = SystemColors.ButtonFace;
			this.btnClose.ForeColor = Color.Black;
			this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.lblProfile.Visible = false;
			this.cbProfile.Visible = false;
			this.label1.Visible = false;
			this.dgCheatCodes.Visible = false;
			this.lblGameName.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.lblGameName.ForeColor = Color.White;
			this.lblGameName.Visible = false;
			this.SetLabels();
			base.CenterToScreen();
			this.FillProfiles();
			this.lblProfile.Text = Resources.lblProfile;
			this.lblGameName.Text = gameItem.name;
			this.dgCheats.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dgCheats_CellMouseClick);
			this.dgCheats.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgCheats_CellMouseDown);
			this.dgCheats.CellValidated += new DataGridViewCellEventHandler(this.dgCheats_CellValidated);
			this.dgCheats.CellValueChanged += new DataGridViewCellEventHandler(this.dgCheats_CellValueChanged);
			this.dgCheats.CurrentCellDirtyStateChanged += new EventHandler(this.dgCheats_CurrentCellDirtyStateChanged);
			this.dgCheats.CellMouseUp += new DataGridViewCellMouseEventHandler(this.dgCheats_CellMouseUp);
			this.dgCheats.MouseDown += new MouseEventHandler(this.dgCheats_MouseDown);
			this.btnApply.Click += new EventHandler(this.btnApply_Click);
			this.btnApplyCodes.Click += new EventHandler(this.btnApplyCodes_Click);
			this.btnClose.Click += new EventHandler(this.btnClose_Click);
			base.Resize += new EventHandler(this.SimpleEdit_ResizeEnd);
			this.SimpleEdit_ResizeEnd(null, null);
			this.FillCheats(null);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0003B0C6 File Offset: 0x000392C6
		protected override void OnResizeBegin(EventArgs e)
		{
			base.SuspendLayout();
			base.OnResizeBegin(e);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0003B0D5 File Offset: 0x000392D5
		protected override void OnResizeEnd(EventArgs e)
		{
			base.OnResizeEnd(e);
			base.ResumeLayout();
			this.SimpleEdit_ResizeEnd(null, null);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0003B0EC File Offset: 0x000392EC
		private void dgCheats_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				this.dgCheats.EndEdit();
			}
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0003B104 File Offset: 0x00039304
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0003B16C File Offset: 0x0003936C
		private void dgCheats_MouseDown(object sender, MouseEventArgs e)
		{
			Point location = e.Location;
			DataGridView.HitTestInfo hitTestInfo = this.dgCheats.HitTest(location.X, location.Y);
			if (hitTestInfo.Type != DataGridViewHitTestType.Cell)
			{
				this.dgCheats.ClearSelection();
			}
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0003B1B0 File Offset: 0x000393B0
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

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0003B2B0 File Offset: 0x000394B0
		private void dgCheats_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			if (this.dgCheats.IsCurrentCellDirty)
			{
				this.dgCheats.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0003B2D0 File Offset: 0x000394D0
		private bool ValidateOneGroup(string curChecked)
		{
			foreach (DataGridViewRow dataGridViewRow in ((IEnumerable)this.dgCheats.Rows))
			{
				if ("one".Equals(dataGridViewRow.Tag) && dataGridViewRow.Cells[0].Value != null && (bool)dataGridViewRow.Cells[0].Value && dataGridViewRow.Cells[1].Tag != curChecked)
				{
					dataGridViewRow.Cells[0].Value = false;
				}
			}
			return true;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0003B390 File Offset: 0x00039590
		private void dgCheats_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			bool enabled = false;
			if (e.ColumnIndex == 0)
			{
				foreach (DataGridViewRow dataGridViewRow in ((IEnumerable)this.dgCheats.Rows))
				{
					if (dataGridViewRow.Cells[0].Value != null && (bool)dataGridViewRow.Cells[0].Value && (string)dataGridViewRow.Cells[0].Tag != "GameFile" && (string)dataGridViewRow.Cells[0].Tag != "CheatGroup")
					{
						enabled = true;
						break;
					}
					dataGridViewRow.Cells[0].Value = false;
				}
			}
			this.btnApply.Enabled = enabled;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0003B490 File Offset: 0x00039690
		private void dgCheats_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 0 && this.m_game.GetTargetGameFolder() == null)
			{
				MessageBox.Show(Resources.errNoSavedata, Resources.msgError);
			}
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0003B4C4 File Offset: 0x000396C4
		private void SimpleEdit_ResizeEnd(object sender, EventArgs e)
		{
			this.btnApply.Left = base.Width / 2 - this.btnApply.Width - 2;
			this.btnClose.Left = base.Width / 2 + 2;
			this.lblProfile.Left = this.btnApply.Left - this.cbProfile.Width - this.lblProfile.Width - 30;
			this.cbProfile.Left = this.lblProfile.Left + this.lblProfile.Width + 5;
			this.dgCheats.Columns[1].Width = (this.dgCheats.Width - 2 - this.dgCheats.Columns[0].Width) / 2;
			this.dgCheats.Columns[2].Width = (this.dgCheats.Width - 2 - this.dgCheats.Columns[0].Width) / 2;
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0003B5D4 File Offset: 0x000397D4
		protected override void OnClosing(CancelEventArgs e)
		{
			if (this.m_bCheatsModified)
			{
				base.DialogResult = DialogResult.OK;
			}
			else
			{
				base.DialogResult = DialogResult.Cancel;
			}
			base.OnClosing(e);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0003B5F5 File Offset: 0x000397F5
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0003B600 File Offset: 0x00039800
		private void SetLabels()
		{
			this.Text = Resources.titleSimpleEdit;
			this.btnApply.Text = Resources.btnApplyPatch;
			this.btnClose.Text = Resources.btnClose;
			this.dgCheats.Columns[0].HeaderText = "";
			this.dgCheats.Columns[1].HeaderText = Resources.colDesc;
			this.dgCheats.Columns[2].HeaderText = Resources.colComment;
			this.addCodeToolStripMenuItem.Text = Resources.mnuAddCheatCode;
			this.editCodeToolStripMenuItem.Text = Resources.mnuEditCheatCode;
			this.deleteCodeToolStripMenuItem.Text = Resources.mnuDeleteCheatCode;
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0003B6BC File Offset: 0x000398BC
		private void FillCheats(string highlight)
		{
			this.dgCheats.Rows.Clear();
			container targetGameFolder = this.m_game.GetTargetGameFolder();
			if (targetGameFolder != null)
			{
				this.Select.Visible = true;
				List<cheat> allCheats = this.m_game.GetAllCheats();
				if (allCheats.Count == 0)
				{
					int index = this.dgCheats.Rows.Add(new DataGridViewRow());
					DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
					dataGridViewCellStyle.ForeColor = Color.Gray;
					this.dgCheats.Rows[index].Cells[0].Tag = "NoCheats";
					dataGridViewCellStyle.Font = new Font(this.dgCheats.Font, FontStyle.Italic);
					this.dgCheats.Rows[index].Cells[1].Style.ApplyStyle(dataGridViewCellStyle);
					this.dgCheats.Rows[index].Cells[1].Value = Resources.lblNoCheats;
				}
				if (targetGameFolder.preprocess == 1 && this.m_gameFiles != null && this.m_gameFiles.Count > 0)
				{
					container container = container.Copy(targetGameFolder);
					List<file> arg_132_0 = container.files._files;
					targetGameFolder.files._files = new List<file>();
					foreach (string current in this.m_gameFiles)
					{
						file file = file.GetGameFile(container, this.m_game.LocalSaveFolder, current);
						if (file != null)
						{
							file = file.Copy(file);
							file.filename = current;
							targetGameFolder.files._files.Add(file);
						}
					}
				}
				using (List<file>.Enumerator enumerator2 = targetGameFolder.files._files.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						file current2 = enumerator2.Current;
						if (targetGameFolder.files._files.Count > 1)
						{
							int index2 = this.dgCheats.Rows.Add(new DataGridViewRow());
							this.dgCheats.Rows[index2].Cells[1].Value = current2.VisibleFileName;
							this.dgCheats.Rows[index2].Cells[2].Value = "";
							this.dgCheats.Rows[index2].Cells[1].Tag = current2.id;
							this.dgCheats.Rows[index2].Cells[0].Tag = "GameFile";
						}
						foreach (cheat current3 in current2.cheats._cheats)
						{
							int index2 = this.dgCheats.Rows.Add(new DataGridViewRow());
							this.dgCheats.Rows[index2].Cells[1].Value = current3.name;
							this.dgCheats.Rows[index2].Cells[2].Value = current3.note;
							this.dgCheats.Rows[index2].Cells[1].Tag = current3.id;
							this.dgCheats.Rows[index2].Cells[0].Tag = current2.filename;
							if (current3.id == "-1")
							{
								this.dgCheats.Rows[index2].Tag = "UserCheat";
								this.dgCheats.Rows[index2].Cells[1].Tag = current3.code;
							}
						}
						foreach (group current4 in current2.groups)
						{
							this.FillGroupCheats(current2, current4, current2.filename, 0);
						}
					}
					goto IL_5D8;
				}
			}
			if (this.m_bShowOnly)
			{
				this.Select.Visible = false;
				this.btnApply.Enabled = false;
				foreach (container current5 in this.m_game.containers._containers)
				{
					foreach (file current6 in current5.files._files)
					{
						foreach (cheat current7 in current6.cheats._cheats)
						{
							int index3 = this.dgCheats.Rows.Add();
							this.dgCheats.Rows[index3].Cells[1].Value = current7.name;
							this.dgCheats.Rows[index3].Cells[2].Value = current7.note;
						}
						foreach (group current8 in current6.groups)
						{
							this.FillGroupCheats(current6, current8, current6.filename, 0);
						}
					}
				}
			}
			IL_5D8:
			this.RefreshValue();
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0003BD6C File Offset: 0x00039F6C
		private void FillFileCheats(container target, file file, string saveFile)
		{
			for (int i = 0; i < file.Cheats.Count; i++)
			{
				cheat cheat = file.Cheats[i];
				int index = this.dgCheats.Rows.Add(new DataGridViewRow());
				this.dgCheats.Rows[index].Cells[1].Value = cheat.name;
				this.dgCheats.Rows[index].Cells[2].Value = cheat.note;
				if (cheat.id == "-1")
				{
					DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
					dataGridViewCellStyle.ForeColor = Color.Blue;
					this.dgCheats.Rows[index].Cells[1].Style.ApplyStyle(dataGridViewCellStyle);
					this.dgCheats.Rows[index].Cells[0].Tag = "UserCheat";
					this.dgCheats.Rows[index].Cells[1].Tag = Path.GetFileName(saveFile);
					this.dgCheats.Rows[index].Tag = file.GetParent(target);
				}
				else
				{
					this.dgCheats.Rows[index].Cells[0].Tag = saveFile;
					this.dgCheats.Rows[index].Cells[1].Tag = cheat.id;
				}
			}
			if (file.groups.Count > 0)
			{
				foreach (group current in file.groups)
				{
					this.FillGroupCheats(file, current, saveFile, 0);
				}
			}
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0003BF64 File Offset: 0x0003A164
		private void FillGroupCheats(file file, group g, string saveFile, int level)
		{
			int index = this.dgCheats.Rows.Add(new DataGridViewRow());
			this.dgCheats.Rows[index].Cells[0].Tag = "CheatGroup";
			if (level > 0)
			{
				this.dgCheats.Rows[index].Cells[1].Value = new string(' ', level * 4) + g.name;
			}
			else
			{
				this.dgCheats.Rows[index].Cells[1].Value = g.name;
			}
			this.dgCheats.Rows[index].Cells[2].Value = g.note;
			this.dgCheats.Rows[index].Cells[2].Value = "";
			foreach (cheat current in g.cheats)
			{
				index = this.dgCheats.Rows.Add(new DataGridViewRow());
				this.dgCheats.Rows[index].Cells[1].Value = new string(' ', (level + 1) * 4) + current.name;
				this.dgCheats.Rows[index].Cells[0].Tag = saveFile;
				this.dgCheats.Rows[index].Cells[1].Tag = current.id;
				this.dgCheats.Rows[index].Tag = g.options;
			}
			if (g._group != null)
			{
				foreach (group current2 in g._group)
				{
					this.FillGroupCheats(file, current2, saveFile, level + 1);
				}
			}
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0003C1AC File Offset: 0x0003A3AC
		private bool ContainsGameFile(List<file> allGameFile, file @internal)
		{
			foreach (file current in allGameFile)
			{
				if (current.id == @internal.id)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0003C210 File Offset: 0x0003A410
		private void dgCheats_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			this.RefreshValue();
			if (e.RowIndex < 0)
			{
				return;
			}
			if (e.ColumnIndex == 2)
			{
				string text = this.dgCheats.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;
				if (text != null && text.IndexOf("http://") >= 0)
				{
					int num = text.IndexOf("http://");
					int num2 = text.IndexOf(' ', num);
					if (num2 > 0)
					{
						Process.Start(text.Substring(text.IndexOf("http"), num2 - num));
						return;
					}
					Process.Start(text.Substring(text.IndexOf("http")));
				}
			}
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0003C2C8 File Offset: 0x0003A4C8
		private void RefreshValue()
		{
			this.dgCheatCodes.Rows.Clear();
			int num = -1;
			if (this.dgCheats.SelectedCells.Count == 1)
			{
				num = this.dgCheats.SelectedCells[0].RowIndex;
			}
			if (this.dgCheats.SelectedRows.Count == 1)
			{
				num = this.dgCheats.SelectedRows[0].Index;
			}
			if (num >= 0 || this.dgCheats.Rows.Count > 0)
			{
			}
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0003C354 File Offset: 0x0003A554
		private void btnApply_Click(object sender, EventArgs e)
		{
			bool flag = false;
			container targetGameFolder = this.m_game.GetTargetGameFolder();
			if (targetGameFolder == null)
			{
				MessageBox.Show(Resources.errNoSavedata, Resources.msgError);
				return;
			}
			List<string> list = new List<string>();
			for (int i = 0; i < this.dgCheats.Rows.Count; i++)
			{
				if (this.dgCheats.Rows[i].Cells[0].Value != null && (bool)this.dgCheats.Rows[i].Cells[0].Value)
				{
					List<file> list2 = new List<file>(targetGameFolder.files._files);
					foreach (file current in list2)
					{
						foreach (cheat current2 in current.GetAllCheats())
						{
							if ((this.dgCheats.Rows[i].Cells[1].Tag == current2.id || ((string)this.dgCheats.Rows[i].Tag == "UserCheat" && current2.id == "-1" && current2.name == this.dgCheats.Rows[i].Cells[1].Value)) && (this.m_gameFiles == null || this.dgCheats.Rows[i].Cells[0].Tag == current.filename))
							{
								current2.Selected = true;
								if (list.IndexOf(current.filename) < 0)
								{
									list.Add(current.filename);
								}
							}
						}
					}
					flag = true;
				}
			}
			if (!flag)
			{
				MessageBox.Show(Resources.msgSelectCheat, Resources.msgError);
				return;
			}
			if (MessageBox.Show(Resources.warnOverwrite, Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
			{
				return;
			}
			string profile = (string)this.cbProfile.SelectedItem;
			SimpleSaveUploader simpleSaveUploader = new SimpleSaveUploader(this.m_game, profile, list);
			simpleSaveUploader.ShowDialog();
			base.Close();
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0003C5FC File Offset: 0x0003A7FC
		private void button1_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0003C60B File Offset: 0x0003A80B
		private void dgCheats_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
		{
			this.RefreshValue();
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0003C613 File Offset: 0x0003A813
		private void btnApplyCodes_Click(object sender, EventArgs e)
		{
			if (this.dgCheatCodes.Tag == null)
			{
				MessageBox.Show(Resources.msgNoCheats, Resources.msgError);
				return;
			}
			this.m_game.GetTargetGameFolder();
			int arg_3A_0 = (int)this.dgCheatCodes.Tag;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0003C650 File Offset: 0x0003A850
		private void addCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<string> list = new List<string>();
			container targetGameFolder = this.m_game.GetTargetGameFolder();
			foreach (file current in targetGameFolder.files._files)
			{
				foreach (cheat current2 in current.Cheats)
				{
					list.Add(current2.name);
				}
			}
			AddCode addCode = new AddCode(list);
			if (addCode.ShowDialog() == DialogResult.OK)
			{
				cheat cheat = new cheat("-1", addCode.Description, addCode.Comment);
				cheat.code = addCode.Code;
				if (this.m_game.GetTargetGameFolder() == null)
				{
					MessageBox.Show(Resources.errNoSavedata, Resources.msgError);
					return;
				}
				string selectedSaveFile = this.GetSelectedSaveFile();
				container targetGameFolder2 = this.m_game.GetTargetGameFolder();
				file gameFile = this.m_game.GetGameFile(targetGameFolder2, selectedSaveFile);
				gameFile.Cheats.Add(cheat);
				this.SaveUserCheats();
				this.m_bCheatsModified = true;
			}
			this.FillCheats(addCode.Description);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0003C7A8 File Offset: 0x0003A9A8
		private string GetSelectedSaveFile()
		{
			int index = this.dgCheats.SelectedRows[0].Index;
			if (this.dgCheats.Rows[index].Cells[0].Tag != null && this.dgCheats.Rows[index].Cells[0].Tag == "GameFile")
			{
				return this.dgCheats.Rows[index].Cells[1].Value.ToString();
			}
			for (int i = index; i >= 0; i--)
			{
				if (this.dgCheats.Rows[i].Cells[0].Tag == "GameFile")
				{
					return this.dgCheats.Rows[i].Cells[1].Value.ToString();
				}
			}
			return null;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0003C89C File Offset: 0x0003AA9C
		private void SaveUserCheats()
		{
			if (this.m_game.GetTargetGameFolder() == null)
			{
				MessageBox.Show(Resources.errNoSavedata, Resources.msgError);
				return;
			}
			string xml = "<usercheats></usercheats>";
			string text = Util.GetBackupLocation() + Path.DirectorySeparatorChar + MainForm.USER_CHEATS_FILE;
			if (File.Exists(text))
			{
				xml = File.ReadAllText(text);
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			bool flag = false;
			container targetGameFolder = this.m_game.GetTargetGameFolder();
			for (int i = 0; i < xmlDocument["usercheats"].ChildNodes.Count; i++)
			{
				if (this.m_game.id + targetGameFolder.key == xmlDocument["usercheats"].ChildNodes[i].Attributes["id"].Value)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				XmlElement xmlElement = xmlDocument.CreateElement("game");
				xmlElement.SetAttribute("id", this.m_game.id + targetGameFolder.key);
				xmlDocument["usercheats"].AppendChild(xmlElement);
			}
			for (int j = 0; j < xmlDocument["usercheats"].ChildNodes.Count; j++)
			{
				if (this.m_game.id + targetGameFolder.key == xmlDocument["usercheats"].ChildNodes[j].Attributes["id"].Value)
				{
					XmlElement xmlElement2 = xmlDocument["usercheats"].ChildNodes[j] as XmlElement;
					xmlElement2.InnerXml = "";
					List<file> list = new List<file>(targetGameFolder.files._files);
					foreach (file current in targetGameFolder.files._files)
					{
						if (current.internals != null && current.internals.files.Count > 0)
						{
							list.AddRange(current.internals.files);
						}
					}
					foreach (file current2 in list)
					{
						XmlElement xmlElement3 = xmlDocument.CreateElement("file");
						xmlElement3.SetAttribute("name", current2.filename);
						xmlElement2.AppendChild(xmlElement3);
						foreach (cheat current3 in current2.Cheats)
						{
							if (current3.id == "-1")
							{
								XmlElement xmlElement4 = xmlDocument.CreateElement("cheat");
								xmlElement4.SetAttribute("desc", current3.name);
								xmlElement4.SetAttribute("comment", current3.note);
								xmlElement3.AppendChild(xmlElement4);
								XmlElement xmlElement5 = xmlDocument.CreateElement("code");
								xmlElement5.InnerText = current3.code;
								xmlElement4.AppendChild(xmlElement5);
							}
						}
					}
				}
			}
			xmlDocument.Save(text);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0003CC18 File Offset: 0x0003AE18
		private void editCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int index = this.dgCheats.SelectedRows[0].Index;
			file gameFile = this.m_game.GetGameFile(this.m_game.GetTargetGameFolder(), this.dgCheats.Rows[index].Cells[0].Tag.ToString());
			cheat cheat = null;
			foreach (cheat current in gameFile.Cheats)
			{
				if (current.name == this.dgCheats.Rows[index].Cells[1].Value.ToString())
				{
					cheat = current;
					break;
				}
			}
			if (cheat == null)
			{
				return;
			}
			List<string> list = new List<string>();
			container targetGameFolder = this.m_game.GetTargetGameFolder();
			foreach (file current2 in targetGameFolder.files._files)
			{
				foreach (cheat current3 in current2.Cheats)
				{
					if (current3.name != this.dgCheats.Rows[index].Cells[1].Value.ToString())
					{
						list.Add(current3.name);
					}
				}
			}
			AddCode addCode = new AddCode(cheat, list);
			if (addCode.ShowDialog() == DialogResult.OK)
			{
				cheat cheat2 = new cheat("-1", addCode.Description, addCode.Comment);
				cheat2.code = addCode.Code;
				for (int i = 0; i < gameFile.Cheats.Count; i++)
				{
					if (gameFile.Cheats[i].name == this.dgCheats.Rows[index].Cells[1].Value.ToString())
					{
						gameFile.Cheats[i] = cheat2;
					}
				}
				this.SaveUserCheats();
				this.m_bCheatsModified = true;
			}
			this.FillCheats(addCode.Description);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0003CE94 File Offset: 0x0003B094
		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			if (this.m_game.GetTargetGameFolder() == null)
			{
				e.Cancel = true;
				return;
			}
			if (this.dgCheats.SelectedRows.Count != 1)
			{
				e.Cancel = true;
				return;
			}
			container targetGameFolder = this.m_game.GetTargetGameFolder();
			if (targetGameFolder.quickmode > 0)
			{
				e.Cancel = true;
				return;
			}
			int index = this.dgCheats.SelectedRows[0].Index;
			if (this.dgCheats.Rows[index].Cells[0].Tag != null && (this.dgCheats.Rows[index].Cells[0].Tag.ToString() == "NoCheats" || this.dgCheats.Rows[index].Cells[0].Tag.ToString() == "GameFile"))
			{
				e.Cancel = false;
			}
			else
			{
				int arg_115_0 = targetGameFolder.files._files.Count;
			}
			this.editCodeToolStripMenuItem.Enabled = (this.dgCheats.Rows[index].Tag == "UserCheat");
			this.deleteCodeToolStripMenuItem.Enabled = (this.dgCheats.Rows[index].Tag == "UserCheat");
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0003D00F File Offset: 0x0003B20F
		private void dgCheats_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			if (e.Button == MouseButtons.Right)
			{
				this.dgCheats.ClearSelection();
				this.dgCheats.Rows[e.RowIndex].Selected = true;
			}
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0003D050 File Offset: 0x0003B250
		private void deleteCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int index = this.dgCheats.SelectedRows[0].Index;
			if (index >= 0 && MessageBox.Show(Resources.msgConfirmDelete, Resources.warnTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				container targetGameFolder = this.m_game.GetTargetGameFolder();
				file gameFile = this.m_game.GetGameFile(targetGameFolder, this.dgCheats.Rows[index].Cells[0].Tag.ToString());
				for (int i = 0; i < gameFile.Cheats.Count; i++)
				{
					if (gameFile.Cheats[i].name == this.dgCheats.Rows[index].Cells[1].Value.ToString())
					{
						gameFile.Cheats.RemoveAt(i);
						break;
					}
				}
				this.SaveUserCheats();
				this.FillCheats(null);
				this.m_bCheatsModified = true;
			}
		}

		// Token: 0x04000594 RID: 1428
		private game m_game;

		// Token: 0x04000595 RID: 1429
		private bool m_bCheatsModified;

		// Token: 0x04000596 RID: 1430
		private bool m_bShowOnly;

		// Token: 0x04000597 RID: 1431
		private List<string> m_gameFiles;
	}
}
