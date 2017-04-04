using System;
using System.Collections.Generic;

namespace PS3SaveEditor
{
	// Token: 0x02000013 RID: 19
	public class container
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00008B50 File Offset: 0x00006D50
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00008B58 File Offset: 0x00006D58
		public string key
		{
			get;
			set;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00008B61 File Offset: 0x00006D61
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00008B69 File Offset: 0x00006D69
		public string pfs
		{
			get;
			set;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00008B72 File Offset: 0x00006D72
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00008B7A File Offset: 0x00006D7A
		public string name
		{
			get;
			set;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00008B83 File Offset: 0x00006D83
		// (set) Token: 0x060000BA RID: 186 RVA: 0x00008B8B File Offset: 0x00006D8B
		public int preprocess
		{
			get;
			set;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00008B94 File Offset: 0x00006D94
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00008B9C File Offset: 0x00006D9C
		public files files
		{
			get;
			set;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00008BA5 File Offset: 0x00006DA5
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00008BAD File Offset: 0x00006DAD
		public int? quickmode
		{
			get;
			set;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00008BB6 File Offset: 0x00006DB6
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00008BBE File Offset: 0x00006DBE
		public int? locked
		{
			get;
			set;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00008BC7 File Offset: 0x00006DC7
		public container()
		{
			this.files = new files();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00008BDC File Offset: 0x00006DDC
		public List<cheat> GetAllCheats()
		{
			List<cheat> list = new List<cheat>();
			foreach (file current in this.files._files)
			{
				list.AddRange(current.GetAllCheats());
			}
			return list;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00008C40 File Offset: 0x00006E40
		internal static container Copy(container folder)
		{
			container container = new container();
			container.key = folder.key;
			container.pfs = folder.pfs;
			container.name = folder.name;
			container.preprocess = folder.preprocess;
			container.files = new files();
			foreach (file current in folder.files._files)
			{
				container.files._files.Add(file.Copy(current));
			}
			container.quickmode = folder.quickmode;
			container.locked = folder.locked;
			return container;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00008D04 File Offset: 0x00006F04
		internal int GetCheatsCount()
		{
			int num = 0;
			foreach (file current in this.files._files)
			{
				num += current.TotalCheats;
			}
			return num;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00008D64 File Offset: 0x00006F64
		internal file GetSaveFile(string fileName)
		{
			foreach (file current in this.files._files)
			{
				if (current.filename == fileName || Util.IsMatch(fileName, current.filename))
				{
					return current;
				}
			}
			return null;
		}
	}
}
