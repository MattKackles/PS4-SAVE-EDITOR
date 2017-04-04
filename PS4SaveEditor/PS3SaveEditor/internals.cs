using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
	// Token: 0x02000015 RID: 21
	public class internals
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00009390 File Offset: 0x00007590
		public internals()
		{
			this.files = new List<file>();
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000DB RID: 219 RVA: 0x000093A3 File Offset: 0x000075A3
		// (set) Token: 0x060000DC RID: 220 RVA: 0x000093AB File Offset: 0x000075AB
		[XmlElement("file")]
		public List<file> files
		{
			get;
			set;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000093B4 File Offset: 0x000075B4
		public static internals Copy(internals i)
		{
			internals internals = new internals();
			foreach (file current in i.files)
			{
				internals.files.Add(file.Copy(current));
			}
			return internals;
		}
	}
}
