using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000D6 RID: 214
	public enum CompressionMethod
	{
		// Token: 0x04000490 RID: 1168
		Stored,
		// Token: 0x04000491 RID: 1169
		Deflated = 8,
		// Token: 0x04000492 RID: 1170
		Deflate64,
		// Token: 0x04000493 RID: 1171
		BZip2 = 11,
		// Token: 0x04000494 RID: 1172
		WinZipAES = 99
	}
}
