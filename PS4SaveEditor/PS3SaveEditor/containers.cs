using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
	// Token: 0x0200000D RID: 13
	public class containers
	{
		// Token: 0x06000070 RID: 112 RVA: 0x00007671 File Offset: 0x00005871
		public containers()
		{
			this._containers = new List<container>();
		}

		// Token: 0x04000054 RID: 84
		[XmlElement("container")]
		public List<container> _containers;
	}
}
