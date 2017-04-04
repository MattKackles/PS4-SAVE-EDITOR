using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
	// Token: 0x0200000F RID: 15
	[XmlRoot("saves")]
	public class saves
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00007708 File Offset: 0x00005908
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00007710 File Offset: 0x00005910
		[XmlElement("save")]
		public List<save> _saves
		{
			get;
			set;
		}
	}
}
