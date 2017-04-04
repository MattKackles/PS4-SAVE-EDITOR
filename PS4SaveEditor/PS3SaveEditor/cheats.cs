using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
	// Token: 0x02000017 RID: 23
	[XmlRoot("cheats")]
	public class cheats
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000943C File Offset: 0x0000763C
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00009444 File Offset: 0x00007644
		[XmlElement("cheat")]
		public List<cheat> _cheats
		{
			get;
			set;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x0000944D File Offset: 0x0000764D
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00009455 File Offset: 0x00007655
		[XmlElement("group")]
		public List<group> groups
		{
			get;
			set;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000945E File Offset: 0x0000765E
		public cheats()
		{
			this._cheats = new List<cheat>();
			this.groups = new List<group>();
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000947C File Offset: 0x0000767C
		public int TotalCheats
		{
			get
			{
				int num = this._cheats.Count;
				foreach (group current in this.groups)
				{
					num += current.TotalCheats;
				}
				return num;
			}
		}
	}
}
