using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
	// Token: 0x0200000A RID: 10
	[XmlRoot("games", Namespace = "")]
	public class games
	{
		// Token: 0x0400004E RID: 78
		[XmlElement("regions")]
		public regions regions;

		// Token: 0x0400004F RID: 79
		[XmlElement("game")]
		public List<game> _games;

		// Token: 0x04000050 RID: 80
		[XmlElement("saves")]
		public saves _saves;
	}
}
