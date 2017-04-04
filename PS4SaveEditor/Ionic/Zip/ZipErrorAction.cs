using System;

namespace Ionic.Zip
{
	// Token: 0x02000154 RID: 340
	public enum ZipErrorAction
	{
		// Token: 0x040007C6 RID: 1990
		Throw,
		// Token: 0x040007C7 RID: 1991
		Skip,
		// Token: 0x040007C8 RID: 1992
		Retry,
		// Token: 0x040007C9 RID: 1993
		InvokeErrorEvent
	}
}
