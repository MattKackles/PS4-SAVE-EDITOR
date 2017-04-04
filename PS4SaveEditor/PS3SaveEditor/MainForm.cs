using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using CSUST.Data;
using CustomControls;
using DGDev;
using Microsoft.Win32;
using PS3SaveEditor.Resources;
using Rss;

namespace PS3SaveEditor
{
	// Token: 0x0200005B RID: 91
	public partial class MainForm : Form
	{
		// Token: 0x060004C4 RID: 1220 RVA: 0x0001899C File Offset: 0x00016B9C
		public MainForm()
		{
			this.m_games = new List<game>();
			this.InitializeComponent();
			this.RegionMap = new Dictionary<int, string>();
			this.ClearDrivesFunc = new MainForm.ClearDrivesDelegate(this.ClearDrives);
			this.AddItemFunc = new MainForm.AddItemDelegate(this.AddItem);
			this.GetTrafficFunc = new MainForm.GetTrafficDelegate(this.GetTraffic);
			this.chkShowAll.CheckedChanged += new EventHandler(this.chkShowAll_CheckedChanged);
			base.ResizeBegin += delegate(object s, EventArgs e)
			{
				base.SuspendLayout();
			};
			base.ResizeEnd += delegate(object s, EventArgs e)
			{
				base.ResumeLayout(true);
				this.chkShowAll_CheckedChanged(null, null);
				base.Invalidate(true);
			};
			base.SizeChanged += delegate(object s, EventArgs e)
			{
				if (base.WindowState == FormWindowState.Maximized)
				{
					this.chkShowAll_CheckedChanged(null, null);
					base.Invalidate(true);
				}
			};
			this.txtBackupLocation.ReadOnly = true;
			this.MinimumSize = base.Size;
			this.dgServerGames.CellClick += new DataGridViewCellEventHandler(this.dgServerGames_CellClick);
			this.btnGamesInServer.Visible = false;
			this.btnRss.BackColor = SystemColors.ButtonFace;
			this.btnOpenFolder.BackColor = SystemColors.ButtonFace;
			this.btnBrowse.BackColor = SystemColors.ButtonFace;
			this.btnDeactivate.BackColor = SystemColors.ButtonFace;
			this.btnManageProfiles.BackColor = SystemColors.ButtonFace;
			this.btnApply.BackColor = SystemColors.ButtonFace;
			this.btnRss.ForeColor = Color.Black;
			this.btnOpenFolder.ForeColor = Color.Black;
			this.btnBrowse.ForeColor = Color.Black;
			this.btnDeactivate.ForeColor = Color.Black;
			this.btnManageProfiles.ForeColor = Color.Black;
			this.btnApply.ForeColor = Color.Black;
			this.btnApply.ForeColor = Color.Black;
			this.pnlBackup.BackColor = (this.pnlHome.BackColor = (this.pnlHome.BackColor = (this.pnlNoSaves.BackColor = Color.FromArgb(127, 204, 204, 204))));
			this.gbBackupLocation.BackColor = (this.gbManageProfile.BackColor = (this.groupBox1.BackColor = (this.groupBox2.BackColor = Color.Transparent)));
			this.chkShowAll.BackColor = Color.FromArgb(0, 204, 204, 204);
			this.chkShowAll.ForeColor = Color.White;
			this.panel2.Visible = false;
			this.registerPSNIDToolStripMenuItem.Visible = false;
			this.resignToolStripMenuItem.Visible = false;
			this.toolStripSeparator1.Visible = false;
			base.CenterToScreen();
			this.SetLabels();
			Util.SetForegroundWindow(base.Handle);
			this.cbDrives.SelectedIndexChanged += new EventHandler(this.cbDrives_SelectedIndexChanged);
			this.dgServerGames.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgServerGames_CellMouseDown);
			this.dgServerGames.CellDoubleClick += new DataGridViewCellEventHandler(this.dgServerGames_CellDoubleClick);
			this.dgServerGames.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgServerGames_ColumnHeaderMouseClick);
			this.dgServerGames.ShowCellToolTips = true;
			this.panel2.BackgroundImage = null;
			this.FillDrives();
			base.Load += new EventHandler(this.MainForm_Load);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00018D0C File Offset: 0x00016F0C
		private void MainForm_Resize(object sender, EventArgs e)
		{
			this.chkShowAll_CheckedChanged(null, null);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00018D18 File Offset: 0x00016F18
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00018D80 File Offset: 0x00016F80
		private void dgServerGames_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			if (e.Column.Index == 1)
			{
				SortOrder sortGlyphDirection = this.dgServerGames.Columns[1].HeaderCell.SortGlyphDirection;
				if (sortGlyphDirection == SortOrder.Descending)
				{
					e.SortResult = this.dgServerGames.Rows[e.RowIndex1].Cells[0].Tag.ToString().CompareTo(this.dgServerGames.Rows[e.RowIndex2].Cells[0].Tag.ToString());
					if (e.SortResult == 0)
					{
						if (this.dgServerGames.Rows[e.RowIndex1].Cells[1].Value.ToString().StartsWith("    "))
						{
							e.SortResult = -1;
						}
						if (this.dgServerGames.Rows[e.RowIndex2].Cells[1].Value.ToString().StartsWith("    "))
						{
							e.SortResult = 1;
						}
					}
				}
				else
				{
					e.SortResult = this.dgServerGames.Rows[e.RowIndex1].Cells[0].Tag.ToString().CompareTo(this.dgServerGames.Rows[e.RowIndex2].Cells[0].Tag.ToString());
					e.SortResult = this.dgServerGames.Rows[e.RowIndex1].Cells[0].Tag.ToString().CompareTo(this.dgServerGames.Rows[e.RowIndex2].Cells[0].Tag.ToString());
					if (e.SortResult == 0)
					{
						if (this.dgServerGames.Rows[e.RowIndex1].Cells[1].Value.ToString().StartsWith("    "))
						{
							e.SortResult = 1;
						}
						if (this.dgServerGames.Rows[e.RowIndex2].Cells[1].Value.ToString().StartsWith("    "))
						{
							e.SortResult = -1;
						}
					}
				}
				e.Handled = true;
				return;
			}
			e.Handled = false;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00019008 File Offset: 0x00017208
		private void dgServerGames_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			if (this.dgServerGames.SelectedCells.Count == 0 || this.dgServerGames.SelectedCells[0].RowIndex < 0)
			{
				return;
			}
			string toolTipText = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].ToolTipText;
			if (toolTipText == Resources.msgUnsupported)
			{
				MessageBox.Show(toolTipText);
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00019098 File Offset: 0x00017298
		private void chkShowAll_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkShowAll.Checked)
			{
				this.pnlNoSaves.Visible = false;
				this.pnlNoSaves.SendToBack();
				this.dgServerGames.Columns[3].Visible = false;
				this.ShowAllGames();
				return;
			}
			this.dgServerGames.Columns[3].Visible = true;
			this.dgServerGames.Columns[3].HeaderText = Resources.colGameCode;
			this.cbDrives_SelectedIndexChanged(null, null);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00019124 File Offset: 0x00017324
		private void ShowAllGames()
		{
			this.dgServerGames.Rows.Clear();
			this.dgServerGames.Columns[4].Visible = false;
			int width = this.dgServerGames.Width;
			if (width == 0)
			{
				return;
			}
			this.dgServerGames.Columns[0].Width = 25;
			this.dgServerGames.Columns[1].Width = (int)((float)(width - 25) * 0.8f);
			this.dgServerGames.Columns[2].Width = (int)((float)(width - 25) * 0.2f);
			foreach (game current in this.m_games)
			{
				int index = this.dgServerGames.Rows.Add();
				this.dgServerGames.Rows[index].Tag = current;
				this.dgServerGames.Rows[index].Cells[1].Value = current.name;
				this.dgServerGames.Rows[index].Cells[2].Value = current.GetCheatCount();
				string text = "";
				text = Util.GetRegion(this.RegionMap, current.region, text);
				List<string> list = new List<string>();
				list.Add(current.id);
				if (current.aliases != null && current.aliases._aliases.Count > 0)
				{
					foreach (alias current2 in current.aliases._aliases)
					{
						string region = Util.GetRegion(this.RegionMap, current2.region, text);
						if (text.IndexOf(region) < 0)
						{
							text += region;
						}
						list.Add(current2.id);
					}
				}
				list.Sort();
				this.dgServerGames.Rows[index].Cells[3].Value = text;
				this.dgServerGames.Rows[index].Cells[1].ToolTipText = "Supported List: " + string.Join(",", list.ToArray());
			}
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000193C8 File Offset: 0x000175C8
		private void dgServerGames_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			int arg_06_0 = e.ColumnIndex;
			if (this.chkShowAll.Checked && e.ColumnIndex == 2)
			{
				return;
			}
			this.SortGames(e.ColumnIndex, this.dgServerGames.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Descending);
			if (this.chkShowAll.Checked)
			{
				this.ShowAllGames();
				return;
			}
			this.FillLocalSaves(null, this.dgServerGames.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00019460 File Offset: 0x00017660
		private void dgServerGames_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			if (this.dgServerGames.SelectedCells.Count == 0 || this.dgServerGames.SelectedCells[0].RowIndex < 0)
			{
				return;
			}
			if (this._isFull)
			{
				return;
			}
			object arg_75_0 = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].Value;
			string toolTipText = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].ToolTipText;
			if (!(this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag is game))
			{
				if (!(this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag is List<game>))
				{
					if (toolTipText == Resources.msgUnsupported)
					{
						MessageBox.Show(toolTipText);
					}
					return;
				}
				int firstDisplayedScrollingRowIndex = this.dgServerGames.FirstDisplayedScrollingRowIndex;
				bool bSortedAsc = this.dgServerGames.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending;
				this.FillLocalSaves(this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].Value as string, bSortedAsc);
				if (this.dgServerGames.Rows.Count > e.RowIndex + 1)
				{
					this.dgServerGames.Rows[e.RowIndex + 1].Selected = true;
					this.dgServerGames.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
					return;
				}
				this.dgServerGames.Rows[Math.Min(e.RowIndex, this.dgServerGames.Rows.Count - 1)].Selected = true;
				try
				{
					this.dgServerGames.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
			this.simpleToolStripMenuItem_Click(null, null);
		}

		// Token: 0x060004CD RID: 1229
		[DllImport("user32.dll")]
		private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		// Token: 0x060004CE RID: 1230
		[DllImport("user32.dll")]
		private static extern bool InsertMenu(IntPtr hMenu, int wPosition, int wFlags, int wIDNewItem, string lpNewItem);

		// Token: 0x060004CF RID: 1231 RVA: 0x000196B0 File Offset: 0x000178B0
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 274)
			{
				int num = m.WParam.ToInt32();
				if (num == 1000)
				{
					AboutBox1 aboutBox = new AboutBox1();
					aboutBox.ShowDialog();
					return;
				}
			}
			else if (m.Msg == 537 && this.m_bSerialChecked)
			{
				if (m.WParam.ToInt32() == 32768)
				{
					if (m.LParam != IntPtr.Zero && ((MainForm.DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(MainForm.DEV_BROADCAST_HDR))).dbch_DeviceType == 2u)
					{
						MainForm.DEV_BROADCAST_VOLUME dEV_BROADCAST_VOLUME = (MainForm.DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(MainForm.DEV_BROADCAST_VOLUME));
						for (int i = 0; i < 26; i++)
						{
							if ((dEV_BROADCAST_VOLUME.dbcv_unitmask >> i & 1u) == 1u)
							{
								Thread thread = new Thread(new ParameterizedThreadStart(this.HandleDrive));
								thread.Start(string.Format("{0}:\\", (char)(65 + i)));
							}
						}
					}
				}
				else if (m.WParam.ToInt32() == 32772 && m.LParam != IntPtr.Zero && ((MainForm.DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(MainForm.DEV_BROADCAST_HDR))).dbch_DeviceType == 2u)
				{
					MainForm.DEV_BROADCAST_VOLUME dEV_BROADCAST_VOLUME2 = (MainForm.DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(MainForm.DEV_BROADCAST_VOLUME));
					for (int j = 0; j < 26; j++)
					{
						if ((dEV_BROADCAST_VOLUME2.dbcv_unitmask >> j & 1u) == 1u)
						{
							for (int k = 0; k < this.cbDrives.Items.Count; k++)
							{
								if (this.cbDrives.Items[k].ToString() == string.Format("{0}:\\", (char)(65 + j)))
								{
									this.cbDrives.Items.RemoveAt(k);
								}
							}
						}
					}
					if (this.cbDrives.Items.Count == 0)
					{
						this.chkShowAll.Checked = true;
						this.chkShowAll.Enabled = false;
					}
					else
					{
						this.cbDrives.SelectedIndex = 0;
					}
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00019910 File Offset: 0x00017B10
		private void HandleDrive(object drive)
		{
			string text = (string)drive;
			this.cbDrives.Invoke(this.AddItemFunc, new object[]
			{
				text
			});
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00019944 File Offset: 0x00017B44
		private void MainForm_Load(object sender, EventArgs e)
		{
			IntPtr systemMenu = MainForm.GetSystemMenu(base.Handle, false);
			MainForm.InsertMenu(systemMenu, 5, 3072, 0, string.Empty);
			MainForm.InsertMenu(systemMenu, 6, 1024, 1000, "About PS4 Save Editor...");
			try
			{
				SplashScreen.Current.SetInfo = "Checking for latest version...";
				if (!this.CheckForVersion())
				{
					return;
				}
			}
			catch (Exception)
			{
			}
			if (!this.CheckSerial())
			{
				base.Close();
			}
			else
			{
				this.m_bSerialChecked = true;
				try
				{
					WebClientEx webClientEx = new WebClientEx();
					webClientEx.Credentials = Util.GetNetworkCredential();
					string uID = Util.GetUID(false, false);
					if (string.IsNullOrEmpty(uID))
					{
						RegistryKey currentUser = Registry.CurrentUser;
						RegistryKey registryKey = currentUser.OpenSubKey(Util.GetRegistryBase(), true);
						try
						{
							registryKey.DeleteValue("Hash");
						}
						catch
						{
						}
						MessageBox.Show(Resources.errNotRegistered, Resources.msgError);
						base.Close();
						return;
					}
					byte[] bytes = webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.ASCII.GetBytes(string.Format("{{\"action\":\"START_SESSION\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}", Util.GetUserId(), uID)));
					string @string = Encoding.ASCII.GetString(bytes);
					JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
					Dictionary<string, object> dictionary = javaScriptSerializer.Deserialize(@string, typeof(Dictionary<string, object>)) as Dictionary<string, object>;
					if (dictionary.ContainsKey("update"))
					{
						Dictionary<string, object> dictionary2 = dictionary["update"] as Dictionary<string, object>;
						foreach (string current in dictionary2.Keys)
						{
							string text = (string)dictionary2[current];
							if (text.IndexOf("msi", StringComparison.CurrentCultureIgnoreCase) > 0)
							{
								UpgradeDownloader upgradeDownloader = new UpgradeDownloader(text);
								upgradeDownloader.ShowDialog();
								base.Close();
								return;
							}
						}
					}
					if (!dictionary.ContainsKey("token"))
					{
						Util.DeleteRegistryValue("User");
						MessageBox.Show(Resources.errNotRegistered);
						base.Close();
						return;
					}
					Util.SetAuthToken(dictionary["token"] as string);
					Thread thread = new Thread(new ParameterizedThreadStart(this.Pinger));
					thread.Start(Convert.ToInt32(dictionary["expiry_ts"]) - Convert.ToInt32(dictionary["current_ts"]));
					Thread thread2 = new Thread(new ParameterizedThreadStart(this.TrafficPoller));
					thread2.Start();
					this.GetPSNIDInfo();
					this.m_sessionInited = true;
				}
				catch (Exception)
				{
				}
				GameListDownloader gameListDownloader = new GameListDownloader();
				if (gameListDownloader.ShowDialog() == DialogResult.OK)
				{
					if (this.m_psnIDs.Count == 0)
					{
						ProfileChecker profileChecker = new ProfileChecker(0, null, null);
						if (profileChecker.ShowDialog() != DialogResult.OK)
						{
							base.Close();
							return;
						}
						this.GetPSNIDInfo();
					}
					try
					{
						this.FillSavesList(gameListDownloader.GameListXml);
					}
					catch (Exception)
					{
						MessageBox.Show(Resources.errInternal, Resources.msgError);
						base.Close();
						return;
					}
					this.GetTraffic();
					if (this.cbDrives.Items.Count == 0)
					{
						this.chkShowAll.Checked = true;
						this.chkShowAll.Enabled = false;
					}
					else
					{
						this.PrepareLocalSavesMap();
						this.FillLocalSaves(null, true);
						this.dgServerGames.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
					}
					this.btnHome_Click(null, null);
					return;
				}
				base.Close();
				return;
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00019D18 File Offset: 0x00017F18
		private void TrafficPoller(object ob)
		{
			this.evt2 = new AutoResetEvent(false);
			while (!this.evt2.WaitOne(60000))
			{
				base.Invoke(this.GetTrafficFunc, null);
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00019D48 File Offset: 0x00017F48
		private void Pinger(object tim)
		{
			int num = (int)tim;
			this.evt = new AutoResetEvent(false);
			string format = "{{\"action\":\"SESSION_REFRESH\",\"userid\":\"{0}\",\"token\":\"{1}\"}}";
			WebClientEx webClientEx = new WebClientEx();
			webClientEx.Credentials = Util.GetNetworkCredential();
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			while (!this.evt.WaitOne((num - 10) * 1000))
			{
				byte[] bytes = webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.ASCII.GetBytes(string.Format(format, Util.GetUserId(), Util.GetAuthToken())));
				string @string = Encoding.ASCII.GetString(bytes);
				if (@string.Contains("ERROR"))
				{
					return;
				}
				Dictionary<string, object> dictionary = javaScriptSerializer.Deserialize(@string, typeof(Dictionary<string, object>)) as Dictionary<string, object>;
				if (dictionary.ContainsKey("token"))
				{
					Util.SetAuthToken(dictionary["token"] as string);
				}
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00019E30 File Offset: 0x00018030
		private void PrepareLocalSavesMap()
		{
			this.m_dictLocalSaves.Clear();
			if (this.cbDrives.SelectedItem == null)
			{
				return;
			}
			string str = this.cbDrives.SelectedItem.ToString();
			if (!Directory.Exists(str + "PS4\\SAVEDATA"))
			{
				return;
			}
			string[] array = Directory.GetDirectories(str + "PS4\\SAVEDATA");
			List<string> list = new List<string>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string path = array2[i];
				string[] directories = Directory.GetDirectories(path);
				string[] array3 = directories;
				for (int j = 0; j < array3.Length; j++)
				{
					string path2 = array3[j];
					list.AddRange(Directory.GetFiles(path2, "*.bin"));
				}
			}
			array = list.ToArray();
			Array.Sort<string>(array);
			string[] array4 = array;
			for (int k = 0; k < array4.Length; k++)
			{
				string text = array4[k];
				string id;
				int onlineSaveIndex = this.GetOnlineSaveIndex(text, out id);
				if (onlineSaveIndex >= 0)
				{
					this.dgServerGames.Rows.Add();
					game game = game.Copy(this.m_games[onlineSaveIndex]);
					game.id = id;
					game.LocalCheatExists = true;
					game.LocalSaveFolder = text;
					if (game.GetTargetGameFolder() == null)
					{
						game.LocalCheatExists = false;
					}
					try
					{
						this.FillLocalCheats(ref game);
					}
					catch (Exception)
					{
					}
					if (!this.m_dictLocalSaves.ContainsKey(game.id))
					{
						List<game> list2 = new List<game>();
						list2.Add(game);
						this.m_dictLocalSaves.Add(game.id, list2);
					}
					else
					{
						this.m_dictLocalSaves[game.id].Add(game);
					}
				}
			}
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00019FEC File Offset: 0x000181EC
		private void FillLocalSaves(string expandGame, bool bSortedAsc)
		{
			if (this.m_expandedGame == expandGame)
			{
				expandGame = null;
				this.m_expandedGame = null;
			}
			this.dgServerGames.Rows.Clear();
			List<string> list = new List<string>();
			foreach (game current in this.m_games)
			{
				foreach (alias current2 in current.GetAllAliases())
				{
					string name = current2.name;
					string id = current2.id;
					if (this.m_dictLocalSaves.ContainsKey(current2.id))
					{
						List<game> list2 = this.m_dictLocalSaves[id];
						if (list.IndexOf(id) < 0)
						{
							list.Add(id);
							int index = this.dgServerGames.Rows.Add();
							this.dgServerGames.Rows[index].Cells[1].Value = current2.name;
							if (list2.Count == 1)
							{
								game game = list2[0];
								this.dgServerGames.Rows[index].Tag = game;
								container targetGameFolder = game.GetTargetGameFolder();
								if (targetGameFolder != null)
								{
									this.dgServerGames.Rows[index].Cells[2].Value = targetGameFolder.GetCheatsCount();
								}
								else
								{
									this.dgServerGames.Rows[index].Cells[2].Value = "N/A";
								}
								this.dgServerGames.Rows[index].Cells[0].ToolTipText = "";
								this.dgServerGames.Rows[index].Cells[0].Tag = id;
								this.dgServerGames.Rows[index].Cells[1].ToolTipText = Path.GetFileNameWithoutExtension(game.LocalSaveFolder);
								this.dgServerGames.Rows[index].Cells[3].Value = id;
								this.dgServerGames.Rows[index].Cells[4].Value = this.GetPSNID(game);
								this.dgServerGames.Rows[index].Cells[5].Value = true;
								if (!this.IsValidPSNID(game.PSN_ID))
								{
									this.dgServerGames.Rows[index].DefaultCellStyle = new DataGridViewCellStyle
									{
										ForeColor = Color.Gray
									};
									this.dgServerGames.Rows[index].Cells[1].Tag = "U";
								}
							}
							else
							{
								DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
								this.dgServerGames.Rows[index].Cells[0].Style.ApplyStyle(new DataGridViewCellStyle
								{
									Font = new Font("Arial", 7f)
								});
								this.dgServerGames.Rows[index].Cells[0].Value = "►";
								this.dgServerGames.Rows[index].Cells[1].Value = this.dgServerGames.Rows[index].Cells[1].Value;
								dataGridViewCellStyle.BackColor = Color.White;
								this.dgServerGames.Rows[index].Cells[0].Style.ApplyStyle(dataGridViewCellStyle);
								this.dgServerGames.Rows[index].Cells[1].Style.ApplyStyle(dataGridViewCellStyle);
								this.dgServerGames.Rows[index].Cells[2].Style.ApplyStyle(dataGridViewCellStyle);
								this.dgServerGames.Rows[index].Cells[3].Style.ApplyStyle(dataGridViewCellStyle);
								this.dgServerGames.Rows[index].Cells[4].Style.ApplyStyle(dataGridViewCellStyle);
								this.dgServerGames.Rows[index].Tag = list2;
								this.dgServerGames.Rows[index].Cells[5].Value = false;
								if (name == expandGame)
								{
									this.dgServerGames.Rows[index].Cells[0].Style.ApplyStyle(new DataGridViewCellStyle
									{
										Font = new Font("Arial", 7f)
									});
									this.dgServerGames.Rows[index].Cells[0].Value = "▼";
									this.dgServerGames.Rows[index].Cells[0].ToolTipText = "";
									this.dgServerGames.Rows[index].Cells[1].Value = current2.name;
									this.dgServerGames.Rows[index].Cells[0].Tag = id;
									foreach (game current3 in list2)
									{
										container targetGameFolder2 = current3.GetTargetGameFolder();
										int index2 = this.dgServerGames.Rows.Add();
										this.dgServerGames.Rows[index2].Cells[1].Value = "    " + (targetGameFolder2.name ?? Path.GetFileNameWithoutExtension(current3.LocalSaveFolder));
										this.dgServerGames.Rows[index2].Cells[0].Tag = id;
										this.dgServerGames.Rows[index2].Tag = current3;
										if (targetGameFolder2 != null)
										{
											this.dgServerGames.Rows[index2].Cells[2].Value = targetGameFolder2.GetCheatsCount();
										}
										else
										{
											this.dgServerGames.Rows[index2].Cells[2].Value = "N/A";
										}
										this.dgServerGames.Rows[index2].Cells[1].ToolTipText = Path.GetFileNameWithoutExtension(current3.LocalSaveFolder);
										this.dgServerGames.Rows[index2].Cells[3].Value = id;
										this.dgServerGames.Rows[index2].Cells[4].Value = this.GetPSNID(current3);
										this.dgServerGames.Rows[index2].Cells[5].Value = true;
										if (!this.IsValidPSNID(current3.PSN_ID))
										{
											this.dgServerGames.Rows[index2].DefaultCellStyle = new DataGridViewCellStyle
											{
												ForeColor = Color.Gray
											};
											this.dgServerGames.Rows[index2].Cells[1].Tag = "U";
										}
									}
									this.m_expandedGame = expandGame;
								}
							}
						}
					}
				}
			}
			this.FillUnavailableGames();
			this.dgServerGames.ClearSelection();
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001A858 File Offset: 0x00018A58
		private object GetPSNID(game item)
		{
			if (!this.IsValidPSNID(item.PSN_ID))
			{
				return Resources.lblUnregistered + " " + item.PSN_ID;
			}
			Dictionary<string, object> dictionary = this.m_psnIDs[item.PSN_ID] as Dictionary<string, object>;
			return dictionary["friendly_name"];
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001A8AC File Offset: 0x00018AAC
		private string GetProfileKey(string sfoPath, Dictionary<string, string> mapProfiles)
		{
			if (File.Exists(sfoPath))
			{
				int num;
				string text = Convert.ToBase64String(MainForm.GetParamInfo(sfoPath, out num));
				string key = string.Concat(new string[]
				{
					num.ToString(),
					":",
					text,
					":",
					Convert.ToBase64String(Util.GetPSNId(Path.GetDirectoryName(sfoPath)))
				});
				if (mapProfiles.ContainsKey(key))
				{
					return mapProfiles[key];
				}
				string key2 = num.ToString() + ":" + text;
				if (mapProfiles.ContainsKey(key2))
				{
					return mapProfiles[key2];
				}
			}
			return "";
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001A954 File Offset: 0x00018B54
		private bool CheckSerial()
		{
			if (Util.GetRegistryValue("User") == null)
			{
				SerialValidateGG serialValidateGG = new SerialValidateGG();
				if (serialValidateGG.ShowDialog(this) != DialogResult.OK)
				{
					return false;
				}
			}
			else
			{
				this.m_hash = Util.GetRegistryValue("User").ToString();
			}
			return true;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001A998 File Offset: 0x00018B98
		private void SetLabels()
		{
			this.picTraffic.BackgroundImage = Resources.blue;
			this.picTraffic.BackgroundImageLayout = ImageLayout.None;
			this.picVersion.BackgroundImageLayout = ImageLayout.None;
			this.picVersion.Visible = false;
			this.pictureBox2.BackgroundImage = Resources.company;
			this.pictureBox2.BackgroundImageLayout = ImageLayout.None;
			this.panel1.BackgroundImage = Resources.sel_drive;
			this.lblNoSaves.Text = Resources.lblNoSaves;
			base.Icon = Resources.ps3se;
			this.btnApply.Text = Resources.btnApply;
			this.btnBrowse.Text = Resources.btnBrowse;
			this.chkBackup.Text = Resources.chkBackupSaves;
			this.lblBackup.Text = Resources.gbBackupLocation;
			this.dgServerGames.Columns[0].HeaderText = "";
			this.dgServerGames.Columns[1].HeaderText = Resources.colGameName;
			this.dgServerGames.Columns[2].HeaderText = Resources.colCheats;
			this.dgServerGames.Columns[3].HeaderText = Resources.colGameCode;
			this.dgServerGames.Columns[4].HeaderText = Resources.colProfile;
			this.btnRss.Text = Resources.btnRss;
			this.btnDeactivate.Text = Resources.btnDeactivate;
			this.simpleToolStripMenuItem.Text = Resources.mnuSimple;
			this.advancedToolStripMenuItem.Text = Resources.mnuAdvanced;
			this.deleteSaveToolStripMenuItem.Text = Resources.mnuDeleteSave;
			this.resignToolStripMenuItem.Text = Resources.mnuResign;
			this.registerPSNIDToolStripMenuItem.Text = Resources.mnuRegisterPSN;
			this.restoreFromBackupToolStripMenuItem.Text = Resources.mnuRestore;
			this.Text = Resources.mainTitle;
			this.btnOpenFolder.Text = Resources.btnOpenFolder;
			this.lblDeactivate.Text = Resources.lblDeactivate;
			this.lblRSSSection.Text = Resources.lblRSSSection;
			this.btnManageProfiles.Text = Resources.btnUserAccount;
			this.lblManageProfiles.Text = Resources.lblUserAccount;
			this.panel3.BackgroundImage = Resources.bg_company;
			this.panel3.BackgroundImageLayout = ImageLayout.Tile;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001ABE0 File Offset: 0x00018DE0
		private void FillLocalCheats(ref game item)
		{
			string text = Util.GetBackupLocation() + Path.DirectorySeparatorChar + MainForm.USER_CHEATS_FILE;
			if (File.Exists(text))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				for (int i = 0; i < xmlDocument["usercheats"].ChildNodes.Count; i++)
				{
					container targetGameFolder = item.GetTargetGameFolder();
					if (targetGameFolder != null && item.id + targetGameFolder.key == xmlDocument["usercheats"].ChildNodes[i].Attributes["id"].Value && xmlDocument["usercheats"].ChildNodes[i].ChildNodes.Count > 0)
					{
						for (int j = 0; j < xmlDocument["usercheats"].ChildNodes[i].ChildNodes.Count; j++)
						{
							XmlNode xmlNode = xmlDocument["usercheats"].ChildNodes[i].ChildNodes[j];
							if ((xmlNode as XmlElement).Name == "file")
							{
								XmlElement xmlElement = xmlNode as XmlElement;
								string attribute = xmlElement.GetAttribute("name");
								file gameFile = item.GetGameFile(targetGameFolder, attribute);
								for (int k = 0; k < xmlElement.ChildNodes.Count; k++)
								{
									XmlNode xmlNode2 = xmlElement.ChildNodes[k];
									string value = xmlNode2.Attributes["desc"].Value;
									string value2 = xmlNode2.Attributes["comment"].Value;
									cheat cheat = new cheat("-1", value, value2);
									for (int l = 0; l < xmlNode2.ChildNodes.Count; l++)
									{
										string innerText = xmlNode2.ChildNodes[l].InnerText;
										string[] array = innerText.Split(new char[]
										{
											' '
										});
										if (array.Length % 2 == 0)
										{
											cheat.code = innerText;
										}
									}
									if (gameFile != null)
									{
										gameFile.Cheats.Add(cheat);
									}
								}
							}
							else
							{
								string value3 = xmlDocument["usercheats"].ChildNodes[i].ChildNodes[j].Attributes["desc"].Value;
								string value4 = xmlDocument["usercheats"].ChildNodes[i].ChildNodes[j].Attributes["comment"].Value;
								cheat cheat2 = new cheat("-1", value3, value4);
								for (int m = 0; m < xmlDocument["usercheats"].ChildNodes[i].ChildNodes[j].ChildNodes.Count; m++)
								{
									string innerText2 = xmlDocument["usercheats"].ChildNodes[i].ChildNodes[j].ChildNodes[m].InnerText;
									string[] array2 = innerText2.Split(new char[]
									{
										' '
									});
									if (array2.Length == 2)
									{
										cheat2.code = innerText2;
									}
								}
								if (!string.IsNullOrEmpty(cheat2.code) && targetGameFolder != null)
								{
									targetGameFolder.files._files[0].Cheats.Add(cheat2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001AF84 File Offset: 0x00019184
		private void FillServerGamesList()
		{
			this.dgServerGames.Rows.Clear();
			foreach (game current in this.m_games)
			{
				int index = this.dgServerGames.Rows.Add(new DataGridViewRow());
				this.dgServerGames.Rows[index].Cells[1].Value = current.name;
				this.dgServerGames.Rows[index].Cells[2].Value = current.GetCheatCount();
				this.dgServerGames.Rows[index].Cells[3].Value = current.id;
				this.dgServerGames.Rows[index].Cells[4].Value = current.version;
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001B0A0 File Offset: 0x000192A0
		private void FillUnavailableGames()
		{
			if (this.cbDrives.SelectedItem == null)
			{
				return;
			}
			string str = this.cbDrives.SelectedItem.ToString();
			if (!Directory.Exists(str + "PS4\\SAVEDATA"))
			{
				return;
			}
			string[] directories = Directory.GetDirectories(str + "PS4\\SAVEDATA");
			string[] array = directories;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				string text2;
				if (this.GetOnlineSaveIndex(text, out text2) == -1)
				{
					string text3 = text + Path.DirectorySeparatorChar + "PARAM.SFO";
					if (File.Exists(text3))
					{
						int index = this.dgServerGames.Rows.Add();
						Color lightSlateGray = Color.LightSlateGray;
						this.dgServerGames.Rows[index].Cells[0].ToolTipText = Resources.msgUnsupported;
						this.dgServerGames.Rows[index].Cells[1].ToolTipText = Resources.msgUnsupported;
						this.dgServerGames.Rows[index].Cells[2].ToolTipText = Resources.msgUnsupported;
						this.dgServerGames.Rows[index].Cells[3].ToolTipText = Resources.msgUnsupported;
						this.dgServerGames.Rows[index].Cells[0].Style.BackColor = lightSlateGray;
						this.dgServerGames.Rows[index].Cells[1].Style.BackColor = lightSlateGray;
						this.dgServerGames.Rows[index].Cells[2].Style.BackColor = lightSlateGray;
						this.dgServerGames.Rows[index].Cells[3].Style.BackColor = lightSlateGray;
						this.dgServerGames.Rows[index].Cells[4].Style.BackColor = lightSlateGray;
						this.dgServerGames.Rows[index].Cells[1].Value = this.GetSaveTitle(text3);
						this.dgServerGames.Rows[index].Cells[3].Value = Path.GetFileName(text).Substring(0, 9);
						this.dgServerGames.Rows[index].Cells[0].Tag = this.dgServerGames.Rows[index].Cells[3].Value;
						this.dgServerGames.Rows[index].Cells[4].Value = "";
						this.dgServerGames.Rows[index].Tag = text;
					}
				}
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001B3A3 File Offset: 0x000195A3
		private void dgServerGames_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			if (e.Button == MouseButtons.Right)
			{
				this.dgServerGames.ClearSelection();
				this.dgServerGames.Rows[e.RowIndex].Selected = true;
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001B460 File Offset: 0x00019660
		private void SortGames(int sortCol, bool bDesc)
		{
			this.m_games.Sort(delegate(game item1, game item2)
			{
				switch (sortCol)
				{
				case 2:
					return item1.GetCheatCount().CompareTo(item2.GetCheatCount());
				case 3:
					return item1.id.CompareTo(item2.id);
				default:
					return (item1.name + item1.LocalSaveFolder).CompareTo(item2.name + item1.LocalSaveFolder);
				}
			});
			if (bDesc)
			{
				this.m_games.Reverse();
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001B4A0 File Offset: 0x000196A0
		private void ClearDrives()
		{
			this.cbDrives.Items.Clear();
			if (this.cbDrives.Items.Count > 0)
			{
				this.cbDrives.SelectedIndex = 0;
				return;
			}
			if (this.m_games.Count > 0)
			{
				this.chkShowAll.Checked = true;
				this.chkShowAll.Enabled = false;
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001B504 File Offset: 0x00019704
		private void AddItem(string item)
		{
			if (item != null)
			{
				int selectedIndex = this.cbDrives.Items.Add(item);
				if (Directory.Exists(item + "PS4\\SAVEDATA") && Directory.GetDirectories(item + "PS4\\SAVEDATA").Length > 0)
				{
					this.pnlNoSaves.Visible = false;
					this.pnlNoSaves.SendToBack();
					if (this.cbDrives.SelectedIndex < 0)
					{
						this.cbDrives.SelectedIndex = selectedIndex;
					}
					else
					{
						string text = this.cbDrives.SelectedItem as string;
						if (!string.IsNullOrEmpty(text) && (!Directory.Exists(text + "PS4\\SAVEDATA") || Directory.GetDirectories(text + "PS4\\SAVEDATA").Length <= 0))
						{
							this.cbDrives.SelectedIndex = selectedIndex;
						}
					}
				}
				else if (this.cbDrives.SelectedIndex < 0)
				{
					this.pnlNoSaves.Visible = true;
					this.pnlNoSaves.BringToFront();
					this.cbDrives.SelectedIndex = selectedIndex;
				}
				if (!this.chkShowAll.Enabled)
				{
					this.chkShowAll.Enabled = true;
					this.chkShowAll.Checked = false;
					return;
				}
			}
			else if (this.m_games.Count > 0)
			{
				this.chkShowAll.Checked = true;
				this.chkShowAll.Enabled = false;
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001B654 File Offset: 0x00019854
		private void FillDrives()
		{
			this.cbDrives.Invoke(this.ClearDrivesFunc);
			DriveInfo[] drives = DriveInfo.GetDrives();
			DriveInfo[] array = drives;
			for (int i = 0; i < array.Length; i++)
			{
				DriveInfo driveInfo = array[i];
				if (driveInfo.IsReady && driveInfo.DriveType == DriveType.Removable)
				{
					if (driveInfo != null)
					{
						this.cbDrives.Invoke(this.AddItemFunc, new object[]
						{
							driveInfo.RootDirectory.FullName
						});
					}
					else
					{
						Control arg_79_0 = this.cbDrives;
						Delegate arg_79_1 = this.AddItemFunc;
						object[] args = new object[1];
						arg_79_0.Invoke(arg_79_1, args);
					}
				}
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001B714 File Offset: 0x00019914
		private void FillSavesList(string xml)
		{
			this.m_games = new List<game>();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(games));
				using (StringReader stringReader = new StringReader(xml))
				{
					games games = (games)xmlSerializer.Deserialize(stringReader);
					this.m_games = games._games;
				}
			}
			catch (Exception)
			{
				try
				{
					xml = xml.Replace("&", "&amp;");
					XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(games));
					using (StringReader stringReader2 = new StringReader(xml))
					{
						games games2 = (games)xmlSerializer2.Deserialize(stringReader2);
						this.m_games = games2._games;
					}
				}
				catch (Exception)
				{
					return;
				}
			}
			this.m_games.Sort((game item1, game item2) => (item1.name + item1.LocalSaveFolder).CompareTo(item2.name + item1.LocalSaveFolder));
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001B834 File Offset: 0x00019A34
		private int GetPSNIDInfo()
		{
			WebClientEx webClientEx = new WebClientEx();
			webClientEx.Credentials = Util.GetNetworkCredential();
			webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
			byte[] bytes = webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.UTF8.GetBytes(string.Format("{{\"action\":\"PSNID_INFO\",\"userid\":\"{0}\"}}", Util.GetUserId())));
			string @string = Encoding.UTF8.GetString(bytes);
			Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(@string, typeof(Dictionary<string, object>)) as Dictionary<string, object>;
			if (dictionary.ContainsKey("status") && (string)dictionary["status"] == "OK")
			{
				if (dictionary.ContainsKey("psnid"))
				{
					this.m_psnIDs = (dictionary["psnid"] as Dictionary<string, object>);
				}
				else
				{
					this.m_psnIDs = new Dictionary<string, object>();
				}
				this.m_psn_quota = Convert.ToInt32(dictionary["psnid_quota"]);
				this.m_psn_remaining = Convert.ToInt32(dictionary["psnid_remaining"]);
				if (this.m_psn_remaining <= 0)
				{
					this.btnManageProfiles.Enabled = false;
				}
				else
				{
					this.btnManageProfiles.Enabled = true;
				}
				bool enabled = Convert.ToBoolean(dictionary["psnid_unregister"]);
				this.btnDeactivate.Enabled = enabled;
				this.gbProfiles.Controls.Clear();
				this.gbProfiles.Width = this.m_psn_quota * 18 + 35;
				for (int i = 0; i < this.m_psn_quota; i++)
				{
					PictureBox pictureBox = new PictureBox();
					if (i < this.m_psn_quota - this.m_psn_remaining)
					{
						pictureBox.Image = Resources.check;
					}
					else
					{
						pictureBox.Image = Resources.uncheck;
					}
					pictureBox.Left = 8 + i * 18;
					pictureBox.Top = 8;
					pictureBox.Width = 18;
					this.gbProfiles.Controls.Add(pictureBox);
				}
				TextBox textBox = new TextBox();
				textBox.Text = string.Format("{0}/{1}", this.m_psn_quota - this.m_psn_remaining, this.m_psn_quota);
				textBox.Left = this.m_psn_quota * 18 + 8;
				textBox.Top = 9;
				textBox.Width = 26;
				textBox.ForeColor = Color.White;
				textBox.BorderStyle = BorderStyle.None;
				textBox.BackColor = Color.FromArgb(102, 132, 162);
				this.gbProfiles.Controls.Add(textBox);
				return this.m_psn_quota;
			}
			return 0;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001BAC5 File Offset: 0x00019CC5
		public bool IsValidPSNID(string psnId)
		{
			return this.m_psnIDs != null && this.m_psnIDs.ContainsKey(psnId);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001BAE0 File Offset: 0x00019CE0
		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			if (this.chkShowAll.Checked)
			{
				e.Cancel = true;
				return;
			}
			if (this.dgServerGames.SelectedCells.Count == 0 || this.cbDrives.Items.Count == 0)
			{
				e.Cancel = true;
				return;
			}
			this.simpleToolStripMenuItem.Visible = true;
			this.advancedToolStripMenuItem.Visible = true;
			int rowIndex = this.dgServerGames.SelectedCells[1].RowIndex;
			if (!(bool)this.dgServerGames.Rows[rowIndex].Cells[5].Value)
			{
				e.Cancel = true;
			}
			if (this.dgServerGames.Rows[rowIndex].Cells[1].Tag == "U")
			{
				this.registerPSNIDToolStripMenuItem.Visible = true;
				this.registerPSNIDToolStripMenuItem.Enabled = true;
				this.simpleToolStripMenuItem.Enabled = false;
				this.advancedToolStripMenuItem.Enabled = false;
				this.restoreFromBackupToolStripMenuItem.Enabled = false;
				return;
			}
			this.registerPSNIDToolStripMenuItem.Visible = false;
			this.registerPSNIDToolStripMenuItem.Enabled = false;
			this.restoreFromBackupToolStripMenuItem.Enabled = true;
			game game = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag as game;
			if (game == null)
			{
				e.Cancel = true;
				return;
			}
			container targetGameFolder = game.GetTargetGameFolder();
			if (targetGameFolder != null)
			{
				if (targetGameFolder.quickmode > 0)
				{
					this.advancedToolStripMenuItem.Enabled = false;
				}
				else
				{
					this.advancedToolStripMenuItem.Enabled = true;
				}
				this.simpleToolStripMenuItem.Enabled = true;
			}
			else
			{
				this.simpleToolStripMenuItem.Enabled = false;
				this.advancedToolStripMenuItem.Enabled = false;
			}
			this.deleteSaveToolStripMenuItem.Visible = true;
			this.restoreFromBackupToolStripMenuItem.Visible = true;
			if (this._isBusy)
			{
				this.advancedToolStripMenuItem.Enabled = false;
			}
			if (this._isFull)
			{
				this.simpleToolStripMenuItem.Enabled = false;
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001BCF8 File Offset: 0x00019EF8
		private void simpleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			object arg_36_0 = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].Value;
			string arg_6D_0 = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].ToolTipText;
			game game = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag as game;
			if (game == null || (game.PSN_ID != null && !this.IsValidPSNID(game.PSN_ID)))
			{
				return;
			}
			if (this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[2].Value as string == "N/A")
			{
				return;
			}
			List<string> list = null;
			if (!this.chkShowAll.Checked)
			{
				list = game.GetContainerFiles();
				if (list.Count < 2)
				{
					MessageBox.Show(Resources.errNoFile, Resources.msgError);
					return;
				}
			}
			container targetGameFolder = game.GetTargetGameFolder();
			if (targetGameFolder != null && targetGameFolder.locked > 0 && MessageBox.Show(Resources.errProfileLock, Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
			{
				return;
			}
			int rowIndex = this.dgServerGames.SelectedCells[0].RowIndex;
			List<string> list2 = new List<string>();
			if (!this.chkShowAll.Checked)
			{
				game.LocalSaveFolder.Substring(0, game.LocalSaveFolder.Length - 4);
				game.ToString(new List<string>(), "decrypt");
				Util.GetTempFolder();
				if (targetGameFolder.preprocess == 1)
				{
					AdvancedSaveUploaderForEncrypt advancedSaveUploaderForEncrypt = new AdvancedSaveUploaderForEncrypt(list.ToArray(), game, null, "list");
					if (advancedSaveUploaderForEncrypt.ShowDialog(this) != DialogResult.Abort && !string.IsNullOrEmpty(advancedSaveUploaderForEncrypt.ListResult))
					{
						ArrayList arrayList = new JavaScriptSerializer().Deserialize(advancedSaveUploaderForEncrypt.ListResult, typeof(ArrayList)) as ArrayList;
						using (IEnumerator enumerator = arrayList.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object current = enumerator.Current;
								list2.Add((string)current);
							}
							goto IL_27B;
						}
					}
					MessageBox.Show(Resources.errInvalidSave);
					return;
				}
			}
			IL_27B:
			SimpleEdit simpleEdit = new SimpleEdit(game, this.chkShowAll.Checked, list2);
			if (simpleEdit.ShowDialog() == DialogResult.OK)
			{
				this.dgServerGames.Rows[rowIndex].Tag = simpleEdit.GameItem;
				this.dgServerGames.Rows[rowIndex].Cells[2].Value = simpleEdit.GameItem.GetCheatCount();
				this.PrepareLocalSavesMap();
				string expandedGame = this.m_expandedGame;
				this.m_expandedGame = null;
				int firstDisplayedScrollingRowIndex = this.dgServerGames.FirstDisplayedScrollingRowIndex;
				this.FillLocalSaves(expandedGame, this.dgServerGames.Columns[1].HeaderCell.SortGlyphDirection == SortOrder.Ascending);
				this.dgServerGames.Rows[Math.Min(rowIndex, this.dgServerGames.Rows.Count - 1)].Selected = true;
				try
				{
					this.dgServerGames.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
			int firstDisplayedScrollingRowIndex2 = this.dgServerGames.FirstDisplayedScrollingRowIndex;
			this.cbDrives_SelectedIndexChanged(null, null);
			this.dgServerGames.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex2;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001C0B8 File Offset: 0x0001A2B8
		private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgServerGames.SelectedCells.Count == 0)
			{
				return;
			}
			Util.ClearTemp();
			object arg_4E_0 = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].Value;
			string arg_85_0 = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].ToolTipText;
			game game = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag as game;
			List<string> containerFiles = game.GetContainerFiles();
			if (containerFiles.Count < 2)
			{
				MessageBox.Show(Resources.errNoFile, Resources.msgError);
				return;
			}
			if (game.GetTargetGameFolder().locked > 0 && MessageBox.Show(Resources.errProfileLock, Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
			{
				return;
			}
			string item = game.LocalSaveFolder.Substring(0, game.LocalSaveFolder.Length - 4);
			game.ToString(new List<string>(), "decrypt");
			containerFiles.Remove(item);
			AdvancedSaveUploaderForEncrypt advancedSaveUploaderForEncrypt = new AdvancedSaveUploaderForEncrypt(containerFiles.ToArray(), game, null, "decrypt");
			if (advancedSaveUploaderForEncrypt.ShowDialog() != DialogResult.Abort && advancedSaveUploaderForEncrypt.DecryptedSaveData != null)
			{
				AdvancedEdit advancedEdit = new AdvancedEdit(game, advancedSaveUploaderForEncrypt.DecryptedSaveData, advancedSaveUploaderForEncrypt.DependentSaveData);
				if (advancedEdit.ShowDialog() == DialogResult.OK)
				{
					this.cbDrives_SelectedIndexChanged(null, null);
				}
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001C260 File Offset: 0x0001A460
		private void cbDrives_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cbDrives.SelectedItem == null)
			{
				return;
			}
			this.dgServerGames.Columns[0].Width = 25;
			int width = this.dgServerGames.Width;
			this.dgServerGames.Columns[1].Width = (int)((float)(width - 25) * 0.4f);
			this.dgServerGames.Columns[2].Width = (int)((float)(width - 25) * 0.17f);
			this.dgServerGames.Columns[3].Width = (int)((float)(width - 25) * 0.2f);
			this.dgServerGames.Columns[4].Width = (int)((float)(width - 25) * 0.23f);
			this.dgServerGames.Columns[4].Visible = true;
			string str = this.cbDrives.SelectedItem.ToString();
			if (!Directory.Exists(str + "PS4//SAVEDATA") || Directory.GetDirectories(str + "PS4//SAVEDATA").Length == 0)
			{
				if (!this.chkShowAll.Enabled)
				{
					this.chkShowAll.Enabled = true;
					this.chkShowAll.Checked = false;
				}
				if (!this.chkShowAll.Checked)
				{
					this.pnlNoSaves.Visible = true;
					this.pnlNoSaves.BringToFront();
					return;
				}
			}
			else
			{
				if (!this.chkShowAll.Checked)
				{
					this.pnlNoSaves.Visible = false;
					this.pnlNoSaves.SendToBack();
					this.PrepareLocalSavesMap();
					this.FillLocalSaves(null, true);
					this.dgServerGames.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
					return;
				}
				this.chkShowAll_CheckedChanged(null, null);
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001C418 File Offset: 0x0001A618
		private int GetOnlineSaveIndex(string save, out string saveId)
		{
			string fileName = Path.GetFileName(Path.GetDirectoryName(save));
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(save);
			for (int i = 0; i < this.m_games.Count; i++)
			{
				saveId = this.m_games[i].id;
				if (fileName.Equals(this.m_games[i].id) || this.m_games[i].IsAlias(fileName, out saveId))
				{
					for (int j = 0; j < this.m_games[i].containers._containers.Count; j++)
					{
						if (fileNameWithoutExtension == this.m_games[i].containers._containers[j].pfs || Util.IsMatch(fileNameWithoutExtension, this.m_games[i].containers._containers[j].pfs))
						{
							return i;
						}
					}
				}
			}
			saveId = null;
			return -1;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001C514 File Offset: 0x0001A714
		private int GetOnlineSaveIndexByGameName(string gameName)
		{
			for (int i = 0; i < this.m_games.Count; i++)
			{
				if (gameName.Equals(this.m_games[i].name))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001C554 File Offset: 0x0001A754
		public static string GetParamInfo(string sfoFile, string item)
		{
			if (!File.Exists(sfoFile))
			{
				return "";
			}
			byte[] array = File.ReadAllBytes(sfoFile);
			int num = BitConverter.ToInt32(array, 8);
			int num2 = BitConverter.ToInt32(array, 12);
			int num3 = BitConverter.ToInt32(array, 16);
			int num4 = 16;
			for (int i = 0; i < num3; i++)
			{
				short num5 = BitConverter.ToInt16(array, i * num4 + 20);
				int num6 = BitConverter.ToInt32(array, i * num4 + 12 + 20);
				if (Encoding.UTF8.GetString(array, num + (int)num5, item.Length) == item)
				{
					int num7 = 0;
					while (num7 < array.Length && array[num2 + num6 + num7] != 0)
					{
						num7++;
					}
					return Encoding.UTF8.GetString(array, num2 + num6, num7);
				}
			}
			return "";
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001C61C File Offset: 0x0001A81C
		public static byte[] GetParamInfo(string sfoFile, out int profileId)
		{
			profileId = 0;
			if (!File.Exists(sfoFile))
			{
				return null;
			}
			byte[] array = File.ReadAllBytes(sfoFile);
			int num = BitConverter.ToInt32(array, 8);
			int num2 = BitConverter.ToInt32(array, 12);
			int num3 = BitConverter.ToInt32(array, 16);
			int num4 = 16;
			for (int i = 0; i < num3; i++)
			{
				short num5 = BitConverter.ToInt16(array, i * num4 + 20);
				int num6 = BitConverter.ToInt32(array, i * num4 + 12 + 20);
				if (Encoding.UTF8.GetString(array, num + (int)num5, 5) == "PARAM")
				{
					byte[] array2 = new byte[16];
					Array.Copy(array, num2 + num6 + 28, array2, 0, 16);
					profileId = BitConverter.ToInt32(array, num2 + num6 + 28 + 16);
					return array2;
				}
			}
			return null;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001C6DB File Offset: 0x0001A8DB
		private string GetSaveDescription(string sfoFile)
		{
			return MainForm.GetParamInfo(sfoFile, "SUB_TITLE") + "\r\n" + MainForm.GetParamInfo(sfoFile, "DETAIL");
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001C6FD File Offset: 0x0001A8FD
		private string GetSaveTitle(string sfoFile)
		{
			return MainForm.GetParamInfo(sfoFile, "TITLE");
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001C70C File Offset: 0x0001A90C
		private void btnHome_Click(object sender, EventArgs e)
		{
			this.pnlHome.Visible = true;
			this.btnHome.BackgroundImage = Resources.home_gamelist_on;
			this.pnlBackup.Visible = false;
			this.btnOptions.BackgroundImage = Resources.home_settings_off;
			this.btnHelp.BackgroundImage = Resources.home_help_off;
			this.cbDrives_SelectedIndexChanged(null, null);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001C769 File Offset: 0x0001A969
		private void btnSaves_Click(object sender, EventArgs e)
		{
			this.pnlHome.Visible = false;
			this.pnlBackup.Visible = false;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001C784 File Offset: 0x0001A984
		private void btnBrowse_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.Description = Resources.lblSelectFolder;
			DialogResult dialogResult = folderBrowserDialog.ShowDialog();
			if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
			{
				this.txtBackupLocation.Text = folderBrowserDialog.SelectedPath;
				this.btnApply_Click(null, null);
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001C7CC File Offset: 0x0001A9CC
		private void chkBackup_CheckedChanged(object sender, EventArgs e)
		{
			this.txtBackupLocation.Enabled = this.chkBackup.Checked;
			this.btnBrowse.Enabled = this.chkBackup.Checked;
			Util.SetRegistryValue("BackupSaves", this.chkBackup.Checked ? "true" : "false");
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001C828 File Offset: 0x0001AA28
		private void btnBackup_Click(object sender, EventArgs e)
		{
			this.pnlBackup.Visible = true;
			this.pnlHome.Visible = false;
			this.btnOptions.BackgroundImage = Resources.home_settings_on;
			this.btnHome.BackgroundImage = Resources.home_gamelist_off;
			this.btnHelp.BackgroundImage = Resources.home_help_off;
			this.chkBackup.Checked = (Util.GetRegistryValue("BackupSaves") != "false");
			this.txtBackupLocation.Text = Util.GetBackupLocation();
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001C8AC File Offset: 0x0001AAAC
		private void btnApply_Click(object sender, EventArgs e)
		{
			Util.SetRegistryValue("Location", this.txtBackupLocation.Text);
			Util.SetRegistryValue("BackupSaves", this.chkBackup.Checked ? "true" : "false");
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001C8E8 File Offset: 0x0001AAE8
		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				if (this.evt != null)
				{
					this.evt.Set();
					this.evt2.Set();
					Directory.Delete(Util.GetTempFolder(), true);
					if (this.m_sessionInited)
					{
						try
						{
							WebClientEx webClientEx = new WebClientEx();
							webClientEx.Credentials = Util.GetNetworkCredential();
							webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
							webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth?token=" + Util.GetAuthToken(), Encoding.ASCII.GetBytes(string.Format("{{\"action\":\"DESTROY_SESSION\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}", Util.GetUserId(), Util.GetUID(false, false))));
						}
						catch (Exception)
						{
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001C9B4 File Offset: 0x0001ABB4
		private void SaveUserCheats()
		{
			string text = "<usercheats>";
			foreach (DataGridViewRow dataGridViewRow in ((IEnumerable)this.dgServerGames.Rows))
			{
				if (dataGridViewRow.Tag != null)
				{
					game game = dataGridViewRow.Tag as game;
					if (game != null && game.GetTargetGameFolder() != null)
					{
						text += string.Format("<game id=\"{0}\">", Path.GetFileName(game.LocalSaveFolder));
						foreach (cheat current in game.GetTargetGameFolder().files._files[0].Cheats)
						{
							if (current.id == "-1")
							{
								string text2 = text;
								text = string.Concat(new string[]
								{
									text2,
									"<cheat desc=\"",
									current.name,
									"\" comment=\"",
									current.note,
									"\">"
								});
								text += current.ToString(false);
								text += "</cheat>";
							}
						}
						text += "</game>";
					}
				}
			}
			text += "</usercheats>";
			File.WriteAllText(Util.GetBackupLocation() + Path.DirectorySeparatorChar + MainForm.USER_CHEATS_FILE, text);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001CB7C File Offset: 0x0001AD7C
		private bool CheckForVersion()
		{
			return true;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001CB80 File Offset: 0x0001AD80
		private void btnRss_Click(object sender, EventArgs e)
		{
			try
			{
				RssFeed rssFeed = RssFeed.Read(string.Format("{0}", "http://www.cybergadget.co.jp/PS4SE/"));
				RssChannel channel = rssFeed.Channels[0];
				RSSForm rSSForm = new RSSForm(channel);
				rSSForm.ShowDialog();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001CBD4 File Offset: 0x0001ADD4
		private void restoreFromBackupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgServerGames.SelectedRows.Count == 1)
			{
				game game = this.dgServerGames.SelectedRows[0].Tag as game;
				string searchPattern = string.Concat(new string[]
				{
					game.PSN_ID,
					"_",
					Path.GetFileName(Path.GetDirectoryName(game.LocalSaveFolder)),
					"_",
					Path.GetFileNameWithoutExtension(game.LocalSaveFolder),
					"_*"
				});
				string[] files = Directory.GetFiles(Util.GetBackupLocation(), searchPattern);
				if (files.Length == 1)
				{
					RestoreBackup restoreBackup = new RestoreBackup(files[0], Path.GetDirectoryName(game.LocalSaveFolder));
					restoreBackup.ShowDialog();
					MessageBox.Show(Resources.msgRestored);
					return;
				}
				if (files.Length == 0)
				{
					MessageBox.Show(Resources.errNoBackup);
					return;
				}
				ChooseBackup chooseBackup = new ChooseBackup(game.name, game.PSN_ID + "_" + Path.GetFileName(Path.GetDirectoryName(game.LocalSaveFolder)) + "_", game.LocalSaveFolder);
				chooseBackup.ShowDialog();
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001CCF8 File Offset: 0x0001AEF8
		private void btnDeactivate_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(Resources.msgDeactivate, Resources.msgInfo, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				CancelPSNIDs cancelPSNIDs = new CancelPSNIDs(this.m_psnIDs);
				if (cancelPSNIDs.ShowDialog() == DialogResult.Yes)
				{
					this.GetPSNIDInfo();
				}
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001CD3C File Offset: 0x0001AF3C
		private bool DeactivateLicense()
		{
			try
			{
				WebClientEx webClientEx = new WebClientEx();
				webClientEx.Credentials = Util.GetNetworkCredential();
				webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
				byte[] bytes = webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.ASCII.GetBytes(string.Format("{{\"action\":\"UNREGISTER_UUID\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}", Util.GetUserId(), Util.GetUID(false, false))));
				string @string = Encoding.ASCII.GetString(bytes);
				Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(@string, typeof(Dictionary<string, object>)) as Dictionary<string, object>;
				if ((string)dictionary["status"] == "OK")
				{
					RegistryKey currentUser = Registry.CurrentUser;
					RegistryKey registryKey = currentUser.OpenSubKey(Util.GetRegistryBase(), true);
					string[] valueNames = registryKey.GetValueNames();
					string[] array = valueNames;
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						if (text != "Location")
						{
							registryKey.DeleteValue(text);
						}
					}
					return true;
				}
				MessageBox.Show(Resources.errOffline, Resources.msgError);
			}
			catch (Exception)
			{
				MessageBox.Show(Resources.errConnection, Resources.msgError);
			}
			return false;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001CE84 File Offset: 0x0001B084
		private void btnOpenFolder_Click(object sender, EventArgs e)
		{
			new Process
			{
				StartInfo = new ProcessStartInfo("explorer", this.txtBackupLocation.Text),
				StartInfo = 
				{
					Verb = "open",
					UseShellExecute = true
				}
			}.Start();
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001CED8 File Offset: 0x0001B0D8
		private void btnHelp_Click(object sender, EventArgs e)
		{
			Path.GetDirectoryName(Application.ExecutablePath);
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				UseShellExecute = true,
				Verb = "open",
				FileName = "http://www.cybergadget.co.jp/assets/files/download/saveEditorPS4_manual.pdf"
			};
			Process.Start(startInfo);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001CF1C File Offset: 0x0001B11C
		private void MainForm_ResizeEnd(object sender, EventArgs e)
		{
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001CF20 File Offset: 0x0001B120
		private string FindGGUSB()
		{
			ManagementScope scope = new ManagementScope("root\\cimv2");
			WqlObjectQuery query = new WqlObjectQuery("SELECT * FROM Win32_DiskDrive where Model = 'dpdev GameGenie USB Device'");
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, query);
			ManagementBaseObject[] array = new ManagementBaseObject[1];
			ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
			if (managementObjectCollection.Count > 0)
			{
				managementObjectCollection.CopyTo(array, 0);
				string text = (string)array[0].Properties["DeviceID"].Value;
				text = text.Replace("\\\\", "\\\\\\\\");
				text = text.Replace(".\\", ".\\\\");
				string text2 = array[0].Properties["PNPDeviceID"].Value.ToString();
				string[] array2 = text2.Split(new char[]
				{
					'\\',
					'&'
				});
				string query2 = "ASSOCIATORS OF {Win32_DiskDrive.DeviceID=\"" + text + "\"} WHERE AssocClass = Win32_DiskDriveToDiskPartition";
				WqlObjectQuery query3 = new WqlObjectQuery(query2);
				ManagementObjectSearcher managementObjectSearcher2 = new ManagementObjectSearcher(scope, query3);
				managementObjectCollection = managementObjectSearcher2.Get();
				if (managementObjectCollection.Count == 1)
				{
					managementObjectCollection.CopyTo(array, 0);
					text = (string)array[0].Properties["DeviceID"].Value;
					WqlObjectQuery query4 = new WqlObjectQuery("ASSOCIATORS OF {Win32_DiskPartition.DeviceID=\"" + text + "\"} WHERE AssocClass = Win32_LogicalDiskToPartition");
					ManagementObjectSearcher managementObjectSearcher3 = new ManagementObjectSearcher(scope, query4);
					managementObjectCollection = managementObjectSearcher3.Get();
					if (managementObjectCollection.Count == 1)
					{
						managementObjectCollection.CopyTo(array, 0);
						string arg_181_0 = (string)array[0].Properties["DeviceID"].Value;
						managementObjectSearcher3.Dispose();
						managementObjectSearcher2.Dispose();
						managementObjectSearcher.Dispose();
						return array2[5];
					}
					managementObjectSearcher3.Dispose();
				}
				managementObjectSearcher2.Dispose();
			}
			managementObjectSearcher.Dispose();
			return null;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001D0E0 File Offset: 0x0001B2E0
		private void deleteSaveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			object arg_36_0 = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].Value;
			string arg_6D_0 = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].ToolTipText;
			game game = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag as game;
			string text;
			if (game == null)
			{
				text = (this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag as string);
			}
			else
			{
				text = game.LocalSaveFolder;
			}
			if (text == null)
			{
				return;
			}
			if (MessageBox.Show(Resources.msgConfirmDeleteSave, this.Text, MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			try
			{
				File.Delete(text);
				File.Delete(text.Substring(0, game.LocalSaveFolder.Length - 4));
			}
			catch (Exception)
			{
				MessageBox.Show(Resources.errDelete, Resources.msgError);
			}
			this.dgServerGames.Rows.Remove(this.dgServerGames.SelectedRows[0]);
			int firstDisplayedScrollingRowIndex = this.dgServerGames.FirstDisplayedScrollingRowIndex;
			this.cbDrives_SelectedIndexChanged(null, null);
			if (this.dgServerGames.Rows.Count > firstDisplayedScrollingRowIndex && firstDisplayedScrollingRowIndex >= 0)
			{
				this.dgServerGames.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001D280 File Offset: 0x0001B480
		private void btnGamesInServer_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001D282 File Offset: 0x0001B482
		private void chkBackup_Click(object sender, EventArgs e)
		{
			if (!this.chkBackup.Checked && MessageBox.Show(Resources.msgConfirmBackup, Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
			{
				this.chkBackup.Checked = true;
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001D2B8 File Offset: 0x0001B4B8
		private void btnManageProfiles_Click(object sender, EventArgs e)
		{
			string text = "";
			if (this.cbDrives.Items.Count == 0)
			{
				MessageBox.Show(Resources.msgNoPSNFolder);
				return;
			}
			string text2 = this.cbDrives.SelectedItem.ToString();
			string[] directories = Directory.GetDirectories(Path.Combine(text2, "PS4//SAVEDATA"));
			if (this.m_psn_remaining > 0)
			{
				for (int i = 0; i < directories.Length; i++)
				{
					string fileName = Path.GetFileName(directories[i]);
					if (!this.m_psnIDs.ContainsKey(fileName))
					{
						text = fileName;
						break;
					}
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				MessageBox.Show(Resources.msgNoPSNFolder);
				return;
			}
			ProfileChecker profileChecker = new ProfileChecker(2, text, text2);
			profileChecker.ShowDialog();
			this.GetPSNIDInfo();
			this.cbDrives_SelectedIndexChanged(null, null);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001D378 File Offset: 0x0001B578
		private void registerPSNIDToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.m_psnIDs.Count >= this.m_psn_quota || this.m_psn_remaining <= 0)
			{
				MessageBox.Show(Resources.errMaxProfiles, Resources.msgInfo);
				return;
			}
			if (this.dgServerGames.SelectedRows.Count == 1)
			{
				game game = this.dgServerGames.SelectedRows[0].Tag as game;
				ProfileChecker profileChecker = new ProfileChecker(1, game.PSN_ID, Path.GetPathRoot(game.LocalSaveFolder));
				if (profileChecker.ShowDialog() == DialogResult.OK)
				{
					this.GetPSNIDInfo();
					this.cbDrives_SelectedIndexChanged(null, null);
				}
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001D414 File Offset: 0x0001B614
		private void resignToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgServerGames.SelectedCells.Count == 0)
			{
				return;
			}
			game game = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag as game;
			string text;
			if (game == null)
			{
				text = (this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag as string);
			}
			else
			{
				text = game.LocalSaveFolder;
			}
			if (text == null)
			{
				return;
			}
			this.cbDrives_SelectedIndexChanged(null, null);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001D4B0 File Offset: 0x0001B6B0
		private bool RegisterSerial()
		{
			try
			{
				WebClientEx webClientEx = new WebClientEx();
				webClientEx.Credentials = Util.GetNetworkCredential();
				string registryValue = Util.GetRegistryValue("Serial");
				string text = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(registryValue)));
				text = text.Replace("-", "");
				string uID = Util.GetUID(false, true);
				bool result;
				if (string.IsNullOrEmpty(uID))
				{
					MessageBox.Show(Resources.errContactSupport);
					result = false;
					return result;
				}
				string uriString = string.Format("{0}/ps4auth", Util.GetBaseUrl(), uID, text);
				string text2 = webClientEx.DownloadString(new Uri(uriString, UriKind.Absolute));
				if (text2.IndexOf('#') > 0)
				{
					string[] array = text2.Split(new char[]
					{
						'#'
					});
					if (array.Length > 1)
					{
						if (array[0] == "4")
						{
							MessageBox.Show(Resources.errInvalidSerial, Resources.msgError);
							result = false;
							return result;
						}
						if (array[0] == "5")
						{
							MessageBox.Show(Resources.errTooManyTimes, Resources.msgError);
							result = false;
							return result;
						}
					}
				}
				else if (text2 == null || text2.ToLower().Contains("error") || text2.ToLower().Contains("not found"))
				{
					string text3 = text2.Replace("ERROR", "");
					if (text3.Contains("1007"))
					{
						Util.GetUID(true, true);
						result = this.RegisterSerial();
						return result;
					}
					if (text3.Contains("1004"))
					{
						MessageBox.Show(Resources.errNotRegistered + text3, Resources.msgError);
						result = false;
						return result;
					}
					if (text3.Contains("1005"))
					{
						MessageBox.Show(Resources.errTooManyTimes + text3, Resources.msgError);
						result = false;
						return result;
					}
					MessageBox.Show(Resources.errNotRegistered, Resources.msgError);
					result = false;
					return result;
				}
				result = true;
				return result;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.StackTrace);
				MessageBox.Show(Resources.errSerial, Resources.msgError);
			}
			MessageBox.Show(Resources.errNotRegistered, Resources.msgError);
			return false;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001D6F8 File Offset: 0x0001B8F8
		private void GetTraffic()
		{
			try
			{
				WebClientEx webClientEx = new WebClientEx();
				this._isBusy = false;
				this._isFull = false;
				webClientEx.Credentials = Util.GetNetworkCredential();
				byte[] bytes = webClientEx.DownloadData(Util.GetBaseUrl() + "/gettl");
				string @string = Encoding.ASCII.GetString(bytes);
				Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(@string, typeof(Dictionary<string, object>)) as Dictionary<string, object>;
				if (dictionary.ContainsKey("load"))
				{
					int num = Convert.ToInt32(dictionary["load"]);
					if (num < 70)
					{
						this.picTraffic.BackgroundImage = Resources.blue;
					}
					else if (num < 100)
					{
						this._isBusy = true;
						this.picTraffic.BackgroundImage = Resources.yellow;
					}
					else
					{
						this.picTraffic.BackgroundImage = Resources.red;
						this._isBusy = true;
						this._isFull = true;
					}
				}
			}
			catch
			{
				this.picTraffic.BackgroundImage = Resources.blue;
			}
		}

		// Token: 0x04000223 RID: 547
		internal const int WM_DEVICECHANGE = 537;

		// Token: 0x04000224 RID: 548
		public const int WM_SYSCOMMAND = 274;

		// Token: 0x04000225 RID: 549
		public const int MF_SEPARATOR = 2048;

		// Token: 0x04000226 RID: 550
		public const int MF_BYPOSITION = 1024;

		// Token: 0x04000227 RID: 551
		public const int MF_STRING = 0;

		// Token: 0x04000228 RID: 552
		public const int IDM_ABOUT = 1000;

		// Token: 0x04000229 RID: 553
		private const string SERIAL_DEACTIVATE_URL = "{{\"action\":\"UNREGISTER_UUID\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}";

		// Token: 0x0400022A RID: 554
		private const string DESTROY_SESSION = "{{\"action\":\"DESTROY_SESSION\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}";

		// Token: 0x0400022B RID: 555
		private const string SESSION_INIT_URL = "{{\"action\":\"START_SESSION\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}";

		// Token: 0x0400022C RID: 556
		private const string PSNID_INFO = "{{\"action\":\"PSNID_INFO\",\"userid\":\"{0}\"}}";

		// Token: 0x0400022D RID: 557
		private const string SESSION_CLOSAL = "{0}/?q=software_auth2/sessionclose&sessionid={1}";

		// Token: 0x0400022E RID: 558
		private const int INTERNAL_VERION_MAJOR = 1;

		// Token: 0x0400022F RID: 559
		private const int INTERNAL_VERION_MINOR = 0;

		// Token: 0x04000230 RID: 560
		private Dictionary<string, List<game>> m_dictLocalSaves = new Dictionary<string, List<game>>();

		// Token: 0x04000231 RID: 561
		private string m_expandedGame;

		// Token: 0x04000232 RID: 562
		private Dictionary<int, string> RegionMap;

		// Token: 0x04000233 RID: 563
		private string m_hash;

		// Token: 0x04000234 RID: 564
		private CustomVScrollbar verticalScroller;

		// Token: 0x04000235 RID: 565
		private CustomHScrollbar horizontalScroller;

		// Token: 0x04000236 RID: 566
		private MainForm.ClearDrivesDelegate ClearDrivesFunc;

		// Token: 0x04000237 RID: 567
		private MainForm.AddItemDelegate AddItemFunc;

		// Token: 0x04000238 RID: 568
		private MainForm.GetTrafficDelegate GetTrafficFunc;

		// Token: 0x04000239 RID: 569
		private List<game> m_games;

		// Token: 0x0400023A RID: 570
		public static string USER_CHEATS_FILE = "usercheats.xml";

		// Token: 0x0400023B RID: 571
		private bool m_bSerialChecked;

		// Token: 0x0400023C RID: 572
		private bool m_sessionInited;

		// Token: 0x0400023D RID: 573
		private AutoResetEvent evt;

		// Token: 0x0400023E RID: 574
		private AutoResetEvent evt2;

		// Token: 0x0400023F RID: 575
		private Dictionary<string, object> m_psnIDs;

		// Token: 0x04000240 RID: 576
		private int m_psn_quota;

		// Token: 0x04000241 RID: 577
		private int m_psn_remaining;

		// Token: 0x04000242 RID: 578
		private bool _isBusy;

		// Token: 0x04000243 RID: 579
		private bool _isFull;

		// Token: 0x04000245 RID: 581
		private DataGridViewTextBoxColumn _Name;

		// Token: 0x0200005C RID: 92
		// (Invoke) Token: 0x06000510 RID: 1296
		private delegate void ClearDrivesDelegate();

		// Token: 0x0200005D RID: 93
		// (Invoke) Token: 0x06000514 RID: 1300
		private delegate void AddItemDelegate(string item);

		// Token: 0x0200005E RID: 94
		// (Invoke) Token: 0x06000518 RID: 1304
		private delegate void GetTrafficDelegate();

		// Token: 0x0200005F RID: 95
		public struct DEV_BROADCAST_HDR
		{
			// Token: 0x0400027A RID: 634
			public uint dbch_Size;

			// Token: 0x0400027B RID: 635
			public uint dbch_DeviceType;

			// Token: 0x0400027C RID: 636
			public uint dbch_Reserved;
		}

		// Token: 0x02000060 RID: 96
		public struct DEV_BROADCAST_VOLUME
		{
			// Token: 0x0400027D RID: 637
			public uint dbch_Size;

			// Token: 0x0400027E RID: 638
			public uint dbch_DeviceType;

			// Token: 0x0400027F RID: 639
			public uint dbch_Reserved;

			// Token: 0x04000280 RID: 640
			public uint dbcv_unitmask;

			// Token: 0x04000281 RID: 641
			public ushort dbcv_flags;
		}
	}
}
