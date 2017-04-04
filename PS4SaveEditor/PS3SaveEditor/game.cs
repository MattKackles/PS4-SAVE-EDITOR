using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
	// Token: 0x02000011 RID: 17
	[XmlRoot("game", Namespace = "")]
	public class game
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00007828 File Offset: 0x00005A28
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00007830 File Offset: 0x00005A30
		public string id
		{
			get;
			set;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00007839 File Offset: 0x00005A39
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00007841 File Offset: 0x00005A41
		public string diskcode
		{
			get;
			set;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600008C RID: 140 RVA: 0x0000784A File Offset: 0x00005A4A
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00007852 File Offset: 0x00005A52
		public string aliasid
		{
			get;
			set;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000785B File Offset: 0x00005A5B
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00007863 File Offset: 0x00005A63
		public string name
		{
			get;
			set;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000786C File Offset: 0x00005A6C
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00007874 File Offset: 0x00005A74
		public string version
		{
			get;
			set;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000092 RID: 146 RVA: 0x0000787D File Offset: 0x00005A7D
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00007885 File Offset: 0x00005A85
		public aliases aliases
		{
			get;
			set;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000788E File Offset: 0x00005A8E
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00007896 File Offset: 0x00005A96
		public containers containers
		{
			get;
			set;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000096 RID: 150 RVA: 0x0000789F File Offset: 0x00005A9F
		// (set) Token: 0x06000097 RID: 151 RVA: 0x000078A7 File Offset: 0x00005AA7
		public int region
		{
			get;
			set;
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000078B0 File Offset: 0x00005AB0
		// (set) Token: 0x06000099 RID: 153 RVA: 0x000078B8 File Offset: 0x00005AB8
		public string Client
		{
			get;
			set;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000078C1 File Offset: 0x00005AC1
		// (set) Token: 0x0600009B RID: 155 RVA: 0x000078C9 File Offset: 0x00005AC9
		public long updated
		{
			get;
			set;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000078D2 File Offset: 0x00005AD2
		// (set) Token: 0x0600009D RID: 157 RVA: 0x000078DA File Offset: 0x00005ADA
		public string LocalSaveFolder
		{
			get;
			set;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000078E3 File Offset: 0x00005AE3
		public string PSN_ID
		{
			get
			{
				if (this.LocalSaveFolder != null)
				{
					return Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(this.LocalSaveFolder)));
				}
				return null;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00007904 File Offset: 0x00005B04
		public game()
		{
			this.containers = new containers();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00007917 File Offset: 0x00005B17
		public override string ToString()
		{
			return this.ToString(false, this.GetSaveFiles());
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00007928 File Offset: 0x00005B28
		public int GetCheatsCount()
		{
			int num = 0;
			foreach (container current in this.containers._containers)
			{
				num += current.GetCheatsCount();
			}
			return num;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00007988 File Offset: 0x00005B88
		public string ToString(List<string> selectedSaveFiles, string mode = "decrypt")
		{
			container targetGameFolder = this.GetTargetGameFolder();
			List<string> containerFiles = this.GetContainerFiles();
			string str = string.Format("<game id=\"{0}\" mode=\"{1}\"><key><name>{2}</name></key><pfs><name>{3}</name></pfs><files>", new object[]
			{
				this.id,
				mode,
				Path.GetFileName(containerFiles[0]),
				Path.GetFileName(containerFiles[1])
			});
			List<string> list = new List<string>();
			foreach (string current in selectedSaveFiles)
			{
				list.Add(Path.GetFileName(current));
				if (mode == "encrypt")
				{
					str = str + "<file><name>" + Path.GetFileName(current).Replace("_file_", "") + "</name></file>";
				}
				else
				{
					str = str + "<file><name>" + Path.GetFileName(current) + "</name></file>";
				}
			}
			return str += "</files></game>";
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00007A94 File Offset: 0x00005C94
		public string ToString(bool bSelectedCheatFilesOnly, List<string> lstSaveFiles)
		{
			container targetGameFolder = this.GetTargetGameFolder();
			List<string> containerFiles = this.GetContainerFiles();
			string text = string.Format("<game id=\"{0}\" mode=\"{1}\"><key><name>{2}</name></key><pfs><name>{3}</name></pfs><files>", new object[]
			{
				this.id,
				"patch",
				Path.GetFileName(containerFiles[0]),
				Path.GetFileName(containerFiles[1])
			});
			this.GetSaveFiles();
			if (targetGameFolder != null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				foreach (string current in lstSaveFiles)
				{
					file gameFile = file.GetGameFile(targetGameFolder, this.LocalSaveFolder, current);
					if (gameFile != null)
					{
						bool flag = false;
						if (!bSelectedCheatFilesOnly)
						{
							flag = true;
						}
						else
						{
							for (int i = 0; i < gameFile.Cheats.Count; i++)
							{
								if (gameFile.Cheats[i].Selected)
								{
									flag = true;
								}
							}
							if (gameFile.groups != null)
							{
								foreach (group current2 in gameFile.groups)
								{
									if (current2.CheatsSelected)
									{
										flag = true;
									}
								}
							}
						}
						if (flag)
						{
							string text2 = current;
							if (dictionary.ContainsKey(text2))
							{
								text = text.Replace(string.Concat(new string[]
								{
									"<file><fileid>",
									gameFile.id,
									"</fileid><name>",
									text2,
									"</name></file>"
								}), "");
								dictionary.Remove(text2);
							}
							if (!dictionary.ContainsKey(text2) && gameFile.GetParent(targetGameFolder) == null)
							{
								text += "<file>";
								text = text + "<name>" + text2 + "</name>";
								dictionary.Add(text2, gameFile.id);
								if (gameFile.GetAllCheats().Count > 0)
								{
									text += "<cheats>";
									foreach (cheat current3 in gameFile.Cheats)
									{
										if (current3.Selected)
										{
											text += current3.ToString(targetGameFolder.quickmode > 0);
										}
									}
									if (gameFile.groups != null)
									{
										foreach (group current4 in gameFile.groups)
										{
											text += current4.SelectedCheats;
										}
									}
									text += "</cheats>";
								}
								text += "</file>";
							}
							if (gameFile.GetParent(targetGameFolder) != null)
							{
								file parent = gameFile.GetParent(targetGameFolder);
								if (parent.internals != null)
								{
									foreach (file current5 in parent.internals.files)
									{
										if (!dictionary.ContainsValue(current5.id))
										{
											if (current.IndexOf(current5.filename) > 0)
											{
												text += "<file>";
												text = text + "<fileid>" + gameFile.id + "</fileid>";
												text = text + "<name>" + Path.GetFileName(text2) + "</name>";
												dictionary.Add(Path.GetFileName(text2), gameFile.id);
												if (gameFile.Cheats.Count > 0)
												{
													text += "<cheats>";
													foreach (cheat current6 in gameFile.Cheats)
													{
														text += current6.ToString(targetGameFolder.quickmode > 0);
													}
													text += "</cheats>";
												}
												text += "</file>";
											}
											else
											{
												string path = Path.Combine(this.LocalSaveFolder, current5.filename);
												text = text + "<file><fileid>" + current5.id + "</fileid>";
												text = text + "<name>" + current5.filename + "</name></file>";
												dictionary.Add(Path.GetFileName(path), current5.id);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			text = text.Replace("<cheats></cheats>", "");
			return text += "</files></game>";
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007FDC File Offset: 0x000061DC
		public List<string> GetContainerFiles()
		{
			if (!Directory.Exists(Path.GetDirectoryName(this.LocalSaveFolder)))
			{
				return null;
			}
			List<string> list = new List<string>();
			this.GetTargetGameFolder();
			list.Add(this.LocalSaveFolder);
			list.Add(this.LocalSaveFolder.Substring(0, this.LocalSaveFolder.Length - 4));
			return list;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00008038 File Offset: 0x00006238
		public container GetTargetGameFolder()
		{
			container result = null;
			if (!Directory.Exists(Path.GetDirectoryName(this.LocalSaveFolder)))
			{
				return null;
			}
			foreach (container current in this.containers._containers)
			{
				if (Path.GetFileNameWithoutExtension(this.LocalSaveFolder) == current.pfs || Util.IsMatch(Path.GetFileNameWithoutExtension(this.LocalSaveFolder), current.pfs))
				{
					bool flag = File.Exists(this.LocalSaveFolder);
					if (flag)
					{
						result = current;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000080E8 File Offset: 0x000062E8
		internal static game Copy(game gameItem)
		{
			game game = new game();
			game.id = gameItem.id;
			game.name = gameItem.name;
			game.diskcode = gameItem.diskcode;
			game.aliasid = gameItem.aliasid;
			game.updated = gameItem.updated;
			game.version = gameItem.version;
			game.region = gameItem.region;
			if (gameItem.aliases != null)
			{
				game.aliases = aliases.Copy(gameItem.aliases);
			}
			foreach (container current in gameItem.containers._containers)
			{
				game.containers._containers.Add(container.Copy(current));
			}
			game.Client = gameItem.Client;
			game.LocalCheatExists = gameItem.LocalCheatExists;
			game.LocalSaveFolder = gameItem.LocalSaveFolder;
			return game;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000081E8 File Offset: 0x000063E8
		internal int GetCheatCount()
		{
			int num = 0;
			foreach (container current in this.containers._containers)
			{
				if (current != null)
				{
					foreach (file current2 in current.files._files)
					{
						num += current2.Cheats.Count;
						if (current2.internals != null)
						{
							foreach (file current3 in current2.internals.files)
							{
								num += current3.TotalCheats;
							}
						}
						foreach (group current4 in current2.groups)
						{
							num += current4.TotalCheats;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00008338 File Offset: 0x00006538
		internal List<cheat> GetAllCheats()
		{
			List<cheat> list = new List<cheat>();
			foreach (container current in this.containers._containers)
			{
				foreach (file current2 in current.files._files)
				{
					list.AddRange(current2.Cheats);
					if (current2.internals != null)
					{
						foreach (file current3 in current2.internals.files)
						{
							list.AddRange(current3.Cheats);
						}
					}
					foreach (group current4 in current2.groups)
					{
						list.AddRange(current4.GetAllCheats());
					}
				}
			}
			return list;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000848C File Offset: 0x0000668C
		internal List<string> GetSaveFiles()
		{
			return this.GetSaveFiles(false);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00008498 File Offset: 0x00006698
		internal List<string> GetSaveFiles(bool bOnlySelectedCheats)
		{
			List<string> list = new List<string>();
			container targetGameFolder = this.GetTargetGameFolder();
			bool flag = false;
			if (targetGameFolder != null)
			{
				foreach (file current in targetGameFolder.files._files)
				{
					list.Add(current.filename);
				}
			}
			if (flag)
			{
				list.Clear();
			}
			return list;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00008514 File Offset: 0x00006714
		internal List<cheat> GetCheats(string saveFolder, string savefile)
		{
			List<cheat> list = new List<cheat>();
			foreach (container current in this.containers._containers)
			{
				string[] files = Directory.GetFiles(Path.GetDirectoryName(saveFolder));
				List<string> list2 = new List<string>();
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (Path.GetFileName(text) == current.pfs || Util.IsMatch(Path.GetFileName(text), current.pfs))
					{
						list2.Add(text);
					}
				}
				if (files.Length > 0 && list2.IndexOf(saveFolder) >= 0)
				{
					foreach (file current2 in current.files._files)
					{
						string[] array2 = Directory.GetFiles(Util.GetTempFolder(), "*");
						List<string> list3 = new List<string>();
						string[] array3 = array2;
						for (int j = 0; j < array3.Length; j++)
						{
							string text2 = array3[j];
							if (text2 == current2.filename || Util.IsMatch(text2, current2.filename))
							{
								list3.Add(text2);
							}
						}
						array2 = list3.ToArray();
						string[] array4 = array2;
						for (int k = 0; k < array4.Length; k++)
						{
							string path = array4[k];
							if (savefile == Path.GetFileName(path) && (current2.filename == Path.GetFileName(path) || Util.IsMatch(Path.GetFileName(savefile), current2.filename)))
							{
								list.AddRange(current2.Cheats);
								foreach (group current3 in current2.groups)
								{
									List<cheat> cheats = current3.GetCheats();
									if (cheats != null)
									{
										list.AddRange(cheats);
									}
								}
								return list;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00008784 File Offset: 0x00006984
		internal file GetGameFile(container folder, string savefile)
		{
			if (savefile == null)
			{
				return folder.files._files[0];
			}
			foreach (file current in folder.files._files)
			{
				if (savefile == current.filename)
				{
					file result = current;
					return result;
				}
			}
			foreach (file current2 in folder.files._files)
			{
				string[] files = Directory.GetFiles(Util.GetTempFolder(), "*");
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string path = array[i];
					if (Path.GetFileName(path) == current2.filename || Util.IsMatch(Path.GetFileName(path), current2.filename))
					{
						file result = current2;
						return result;
					}
				}
			}
			return null;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000088A0 File Offset: 0x00006AA0
		internal bool IsAlias(string gameCode, out string saveId)
		{
			if (this.aliases != null)
			{
				foreach (alias current in this.aliases._aliases)
				{
					if (gameCode.IndexOf(current.id) >= 0)
					{
						saveId = current.id;
						return true;
					}
				}
			}
			saveId = null;
			return false;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000891C File Offset: 0x00006B1C
		internal bool IsSupported(Dictionary<string, List<game>> m_dictLocalSaves, out string saveID)
		{
			if (this.aliases != null)
			{
				foreach (alias current in this.aliases._aliases)
				{
					if (m_dictLocalSaves.ContainsKey(current.id))
					{
						saveID = current.id;
						return true;
					}
				}
			}
			saveID = null;
			return false;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00008998 File Offset: 0x00006B98
		internal List<alias> GetAllAliases()
		{
			List<alias> list = new List<alias>();
			list.Add(new alias
			{
				id = this.id,
				name = this.name,
				region = this.region
			});
			if (this.aliases != null && this.aliases._aliases != null && this.aliases._aliases.Count > 0)
			{
				list.AddRange(this.aliases._aliases);
			}
			return list;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00008A18 File Offset: 0x00006C18
		internal cheat GetCheat(string id, string title)
		{
			foreach (container current in this.containers._containers)
			{
				foreach (file current2 in current.files._files)
				{
					foreach (cheat current3 in current2.GetAllCheats())
					{
						if (current3.id == id && current3.name == title)
						{
							return current3;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0400005F RID: 95
		public bool LocalCheatExists;
	}
}
