using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000D7 RID: 215
	public enum EncryptionAlgorithm
	{
		// Token: 0x04000496 RID: 1174
		None,
		// Token: 0x04000497 RID: 1175
		PkzipClassic,
		// Token: 0x04000498 RID: 1176
		Des = 26113,
		// Token: 0x04000499 RID: 1177
		RC2,
		// Token: 0x0400049A RID: 1178
		TripleDes168,
		// Token: 0x0400049B RID: 1179
		TripleDes112 = 26121,
		// Token: 0x0400049C RID: 1180
		Aes128 = 26126,
		// Token: 0x0400049D RID: 1181
		Aes192,
		// Token: 0x0400049E RID: 1182
		Aes256,
		// Token: 0x0400049F RID: 1183
		RC2Corrected = 26370,
		// Token: 0x040004A0 RID: 1184
		Blowfish = 26400,
		// Token: 0x040004A1 RID: 1185
		Twofish,
		// Token: 0x040004A2 RID: 1186
		RC4 = 26625,
		// Token: 0x040004A3 RID: 1187
		Unknown = 65535
	}
}
