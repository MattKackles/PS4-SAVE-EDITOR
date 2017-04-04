using System;

namespace Ionic.Zip
{
	// Token: 0x02000151 RID: 337
	[Flags]
	public enum ZipEntryTimestamp
	{
		// Token: 0x040007B5 RID: 1973
		None = 0,
		// Token: 0x040007B6 RID: 1974
		DOS = 1,
		// Token: 0x040007B7 RID: 1975
		Windows = 2,
		// Token: 0x040007B8 RID: 1976
		Unix = 4,
		// Token: 0x040007B9 RID: 1977
		InfoZip1 = 8
	}
}
