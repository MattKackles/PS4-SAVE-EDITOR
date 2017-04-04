using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
	// Token: 0x02000016 RID: 22
	[XmlRoot("files")]
	public class files
	{
		// Token: 0x060000DE RID: 222 RVA: 0x00009418 File Offset: 0x00007618
		public files()
		{
			this._files = new List<file>();
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000DF RID: 223 RVA: 0x0000942B File Offset: 0x0000762B
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00009433 File Offset: 0x00007633
		[XmlElement("file")]
		public List<file> _files
		{
			get;
			set;
		}
	}
}
