using System;

namespace Ionic.Zlib
{
	// Token: 0x02000169 RID: 361
	internal static class InternalConstants
	{
		// Token: 0x0400088A RID: 2186
		internal static readonly int MAX_BITS = 15;

		// Token: 0x0400088B RID: 2187
		internal static readonly int BL_CODES = 19;

		// Token: 0x0400088C RID: 2188
		internal static readonly int D_CODES = 30;

		// Token: 0x0400088D RID: 2189
		internal static readonly int LITERALS = 256;

		// Token: 0x0400088E RID: 2190
		internal static readonly int LENGTH_CODES = 29;

		// Token: 0x0400088F RID: 2191
		internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;

		// Token: 0x04000890 RID: 2192
		internal static readonly int MAX_BL_BITS = 7;

		// Token: 0x04000891 RID: 2193
		internal static readonly int REP_3_6 = 16;

		// Token: 0x04000892 RID: 2194
		internal static readonly int REPZ_3_10 = 17;

		// Token: 0x04000893 RID: 2195
		internal static readonly int REPZ_11_138 = 18;
	}
}
