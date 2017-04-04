using System;

namespace Ionic.Zlib
{
	// Token: 0x02000170 RID: 368
	public static class ZlibConstants
	{
		// Token: 0x040008C9 RID: 2249
		public const int WindowBitsMax = 15;

		// Token: 0x040008CA RID: 2250
		public const int WindowBitsDefault = 15;

		// Token: 0x040008CB RID: 2251
		public const int Z_OK = 0;

		// Token: 0x040008CC RID: 2252
		public const int Z_STREAM_END = 1;

		// Token: 0x040008CD RID: 2253
		public const int Z_NEED_DICT = 2;

		// Token: 0x040008CE RID: 2254
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x040008CF RID: 2255
		public const int Z_DATA_ERROR = -3;

		// Token: 0x040008D0 RID: 2256
		public const int Z_BUF_ERROR = -5;

		// Token: 0x040008D1 RID: 2257
		public const int WorkingBufferSizeDefault = 16384;

		// Token: 0x040008D2 RID: 2258
		public const int WorkingBufferSizeMin = 1024;
	}
}
