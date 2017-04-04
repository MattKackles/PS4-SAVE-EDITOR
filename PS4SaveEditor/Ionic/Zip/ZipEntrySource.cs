using System;

namespace Ionic.Zip
{
	// Token: 0x02000153 RID: 339
	public enum ZipEntrySource
	{
		// Token: 0x040007BE RID: 1982
		None,
		// Token: 0x040007BF RID: 1983
		FileSystem,
		// Token: 0x040007C0 RID: 1984
		Stream,
		// Token: 0x040007C1 RID: 1985
		ZipFile,
		// Token: 0x040007C2 RID: 1986
		WriteDelegate,
		// Token: 0x040007C3 RID: 1987
		JitStream,
		// Token: 0x040007C4 RID: 1988
		ZipOutputStream
	}
}
