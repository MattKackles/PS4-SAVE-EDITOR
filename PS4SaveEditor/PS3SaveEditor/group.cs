using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
	// Token: 0x02000014 RID: 20
	public class group
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00008DD8 File Offset: 0x00006FD8
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00008DE0 File Offset: 0x00006FE0
		public string name
		{
			get;
			set;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00008DE9 File Offset: 0x00006FE9
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00008DF1 File Offset: 0x00006FF1
		public string note
		{
			get;
			set;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00008DFA File Offset: 0x00006FFA
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00008E02 File Offset: 0x00007002
		public string options
		{
			get;
			set;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00008E0B File Offset: 0x0000700B
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00008E13 File Offset: 0x00007013
		[XmlElement("cheat")]
		public List<cheat> cheats
		{
			get;
			set;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00008E1C File Offset: 0x0000701C
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00008E24 File Offset: 0x00007024
		[XmlElement(ElementName = "group")]
		public List<group> _group
		{
			get;
			set;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00008E30 File Offset: 0x00007030
		public int TotalCheats
		{
			get
			{
				int num = 0;
				if (this.cheats != null)
				{
					num = this.cheats.Count;
				}
				if (this._group != null)
				{
					foreach (group current in this._group)
					{
						num += current.TotalCheats;
					}
				}
				return num;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00008EA4 File Offset: 0x000070A4
		public List<cheat> GetAllCheats()
		{
			List<cheat> list = new List<cheat>();
			if (this._group != null)
			{
				foreach (group current in this._group)
				{
					list.AddRange(current.cheats);
				}
			}
			list.AddRange(this.cheats);
			return list;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00008F18 File Offset: 0x00007118
		public group()
		{
			this.cheats = new List<cheat>();
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00008F2C File Offset: 0x0000712C
		internal static group Copy(group g)
		{
			group group = new group();
			group.name = g.name;
			group.note = g.note;
			group.options = g.options;
			if (g._group != null)
			{
				group._group = new List<group>();
				foreach (group current in g._group)
				{
					group._group.Add(group.Copy(current));
				}
			}
			foreach (cheat current2 in g.cheats)
			{
				group.cheats.Add(cheat.Copy(current2));
			}
			return group;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00009014 File Offset: 0x00007214
		public cheat GetCheat(string cd)
		{
			foreach (cheat current in this.cheats)
			{
				if (cd == current.name)
				{
					cheat result = current;
					return result;
				}
			}
			if (this._group != null)
			{
				foreach (group current2 in this._group)
				{
					cheat cheat = current2.GetCheat(cd);
					if (cheat != null)
					{
						cheat result = cheat;
						return result;
					}
				}
			}
			return null;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000090CC File Offset: 0x000072CC
		internal int GetCheatsCount()
		{
			int count = this.cheats.Count;
			if (this._group != null)
			{
				using (List<group>.Enumerator enumerator = this._group.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						group current = enumerator.Current;
						return count + current.GetCheatsCount();
					}
				}
				return count;
			}
			return count;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000913C File Offset: 0x0000733C
		internal List<cheat> GetCheats()
		{
			List<cheat> list = new List<cheat>();
			list.AddRange(list);
			if (this._group != null)
			{
				foreach (group current in this._group)
				{
					list.AddRange(current.GetCheats());
				}
			}
			return list;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x000091AC File Offset: 0x000073AC
		public bool CheatsSelected
		{
			get
			{
				foreach (cheat current in this.cheats)
				{
					if (current.Selected)
					{
						bool result = true;
						return result;
					}
				}
				if (this._group != null)
				{
					foreach (group current2 in this._group)
					{
						if (current2.CheatsSelected)
						{
							bool result = true;
							return result;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00009258 File Offset: 0x00007458
		public string SelectedCheats
		{
			get
			{
				string text = "";
				foreach (cheat current in this.cheats)
				{
					if (current.Selected)
					{
						text = text + "<id>" + current.id + "</id>";
					}
				}
				if (this._group != null)
				{
					foreach (group current2 in this._group)
					{
						text += current2.SelectedCheats;
					}
				}
				return text;
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000931C File Offset: 0x0000751C
		internal List<cheat> GetGroupCheats()
		{
			List<cheat> list = new List<cheat>();
			list.AddRange(this.cheats);
			if (this._group != null)
			{
				foreach (group current in this._group)
				{
					list.AddRange(current.GetGroupCheats());
				}
			}
			return list;
		}
	}
}
