using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
	// Token: 0x0200000E RID: 14
	public class aliases
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00007684 File Offset: 0x00005884
		public static aliases Copy(aliases a)
		{
			aliases aliases = new aliases();
			if (a != null && a._aliases != null)
			{
				aliases._aliases = new List<alias>();
				foreach (alias current in a._aliases)
				{
					aliases._aliases.Add(alias.Copy(current));
				}
			}
			return aliases;
		}

		// Token: 0x04000055 RID: 85
		[XmlElement("alias")]
		public List<alias> _aliases;
	}
}
