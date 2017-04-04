using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
	// Token: 0x02000018 RID: 24
	[XmlRoot("file")]
	public class file
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x000094E0 File Offset: 0x000076E0
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x000094E8 File Offset: 0x000076E8
		public string filename
		{
			get;
			set;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000094F1 File Offset: 0x000076F1
		// (set) Token: 0x060000EA RID: 234 RVA: 0x000094F9 File Offset: 0x000076F9
		public string id
		{
			get;
			set;
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00009502 File Offset: 0x00007702
		// (set) Token: 0x060000EC RID: 236 RVA: 0x0000950A File Offset: 0x0000770A
		public string title
		{
			get;
			set;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00009513 File Offset: 0x00007713
		// (set) Token: 0x060000EE RID: 238 RVA: 0x0000951B File Offset: 0x0000771B
		public string dependency
		{
			get;
			set;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00009524 File Offset: 0x00007724
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x0000952C File Offset: 0x0000772C
		public string Option
		{
			get;
			set;
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00009535 File Offset: 0x00007735
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x0000953D File Offset: 0x0000773D
		public string altname
		{
			get;
			set;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00009546 File Offset: 0x00007746
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x0000954E File Offset: 0x0000774E
		public cheats cheats
		{
			get;
			set;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00009557 File Offset: 0x00007757
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00009564 File Offset: 0x00007764
		[XmlIgnore]
		public List<cheat> Cheats
		{
			get
			{
				return this.cheats._cheats;
			}
			set
			{
				this.cheats._cheats = value;
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00009574 File Offset: 0x00007774
		public List<cheat> GetAllCheats()
		{
			List<cheat> list = new List<cheat>();
			list.AddRange(this.Cheats);
			foreach (group current in this.groups)
			{
				list.AddRange(current.GetGroupCheats());
			}
			return list;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000095E0 File Offset: 0x000077E0
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x000095ED File Offset: 0x000077ED
		[XmlIgnore]
		public List<group> groups
		{
			get
			{
				return this.cheats.groups;
			}
			set
			{
				this.cheats.groups = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000095FB File Offset: 0x000077FB
		public int TotalCheats
		{
			get
			{
				return this.cheats.TotalCheats;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00009608 File Offset: 0x00007808
		public string VisibleFileName
		{
			get
			{
				if (!string.IsNullOrEmpty(this.altname))
				{
					return string.Format("{0} ({1})", this.altname, this.filename);
				}
				return this.filename;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00009634 File Offset: 0x00007834
		public int TextMode
		{
			get
			{
				string a;
				if ((a = this.textmode) == null)
				{
					return 0;
				}
				if (a == "")
				{
					return 0;
				}
				if (a == "utf-8")
				{
					return 1;
				}
				if (a == "ascii")
				{
					return 2;
				}
				if (!(a == "utf-16"))
				{
					return 0;
				}
				return 3;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000FD RID: 253 RVA: 0x0000968C File Offset: 0x0000788C
		public bool IsHidden
		{
			get
			{
				return this.type == "hidden";
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000969E File Offset: 0x0000789E
		public file()
		{
			this.cheats = new cheats();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000096B4 File Offset: 0x000078B4
		public string GetDependencyFile(container gameFolder, string folder)
		{
			if (string.IsNullOrEmpty(this.dependency))
			{
				return null;
			}
			foreach (file current in gameFolder.files._files)
			{
				if (current.id == this.dependency)
				{
					string text = current.GetSaveFile(folder);
					if (text == null)
					{
						foreach (file current2 in gameFolder.files._files)
						{
							if (current2.id == current.dependency)
							{
								text = current2.filename;
							}
						}
					}
					if (text != null)
					{
						return Path.Combine(folder, text);
					}
				}
			}
			return null;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000097A4 File Offset: 0x000079A4
		internal static file Copy(file gameFile)
		{
			file file = new file();
			file.filename = gameFile.filename;
			file.dependency = gameFile.dependency;
			file.title = gameFile.title;
			file.id = gameFile.id;
			file.Option = gameFile.Option;
			file.altname = gameFile.altname;
			if (gameFile.internals != null)
			{
				file.internals = new internals();
				foreach (file current in gameFile.internals.files)
				{
					file.internals.files.Add(file.Copy(current));
				}
			}
			file.cheats = new cheats();
			foreach (group current2 in gameFile.groups)
			{
				file.groups.Add(group.Copy(current2));
			}
			file.textmode = gameFile.textmode;
			file.type = gameFile.type;
			foreach (cheat current3 in gameFile.Cheats)
			{
				file.Cheats.Add(cheat.Copy(current3));
			}
			return file;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000992C File Offset: 0x00007B2C
		internal string GetSaveFile(string saveFolder)
		{
			string[] files = Directory.GetFiles(saveFolder, this.filename);
			if (files.Length > 0)
			{
				return Path.GetFileName(files[0]);
			}
			return null;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00009958 File Offset: 0x00007B58
		internal List<string> GetSaveFiles(string saveFolder)
		{
			string[] files = Directory.GetFiles(saveFolder, this.filename);
			if (files.Length > 0)
			{
				List<string> list = new List<string>(files);
				list.Sort();
				return list;
			}
			return null;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00009988 File Offset: 0x00007B88
		internal static file GetGameFile(container gameFolder, string folder, string file)
		{
			foreach (file current in gameFolder.files._files)
			{
				if (current.filename == file || Util.IsMatch(file, current.filename))
				{
					return current;
				}
			}
			return null;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000099FC File Offset: 0x00007BFC
		internal cheat GetCheat(string cd)
		{
			foreach (cheat current in this.Cheats)
			{
				if (cd == current.name)
				{
					cheat result = current;
					return result;
				}
			}
			foreach (group current2 in this.groups)
			{
				cheat cheat = current2.GetCheat(cd);
				if (cheat != null)
				{
					cheat result = cheat;
					return result;
				}
			}
			return null;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00009AAC File Offset: 0x00007CAC
		public file GetParent(container gamefolder)
		{
			foreach (file current in gamefolder.files._files)
			{
				if (current.id == this.id)
				{
					file result = null;
					return result;
				}
				if (current.internals != null)
				{
					foreach (file current2 in current.internals.files)
					{
						if (current2.id == this.id)
						{
							file result = current;
							return result;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0400007E RID: 126
		public internals internals;

		// Token: 0x0400007F RID: 127
		public string type;

		// Token: 0x04000080 RID: 128
		public string textmode;
	}
}
