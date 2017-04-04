using System;

namespace PS3SaveEditor
{
	// Token: 0x02000012 RID: 18
	public class alias
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00008B10 File Offset: 0x00006D10
		public static alias Copy(alias alias)
		{
			return new alias
			{
				id = alias.id,
				region = alias.region,
				name = alias.name
			};
		}

		// Token: 0x0400006B RID: 107
		public string id;

		// Token: 0x0400006C RID: 108
		public string name;

		// Token: 0x0400006D RID: 109
		public int region;
	}
}
