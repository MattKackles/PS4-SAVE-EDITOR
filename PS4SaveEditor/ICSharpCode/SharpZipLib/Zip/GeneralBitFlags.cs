using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000D8 RID: 216
	[Flags]
	public enum GeneralBitFlags
	{
		// Token: 0x040004A5 RID: 1189
		Encrypted = 1,
		// Token: 0x040004A6 RID: 1190
		Method = 6,
		// Token: 0x040004A7 RID: 1191
		Descriptor = 8,
		// Token: 0x040004A8 RID: 1192
		ReservedPKware4 = 16,
		// Token: 0x040004A9 RID: 1193
		Patched = 32,
		// Token: 0x040004AA RID: 1194
		StrongEncryption = 64,
		// Token: 0x040004AB RID: 1195
		Unused7 = 128,
		// Token: 0x040004AC RID: 1196
		Unused8 = 256,
		// Token: 0x040004AD RID: 1197
		Unused9 = 512,
		// Token: 0x040004AE RID: 1198
		Unused10 = 1024,
		// Token: 0x040004AF RID: 1199
		UnicodeText = 2048,
		// Token: 0x040004B0 RID: 1200
		EnhancedCompress = 4096,
		// Token: 0x040004B1 RID: 1201
		HeaderMasked = 8192,
		// Token: 0x040004B2 RID: 1202
		ReservedPkware14 = 16384,
		// Token: 0x040004B3 RID: 1203
		ReservedPkware15 = 32768
	}
}
