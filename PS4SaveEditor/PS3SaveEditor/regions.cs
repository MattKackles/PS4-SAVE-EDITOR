using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
	// Token: 0x0200000B RID: 11
	public class regions
	{
		// Token: 0x04000051 RID: 81
		[XmlElement("region")]
		public List<region> _regions;
	}
}
