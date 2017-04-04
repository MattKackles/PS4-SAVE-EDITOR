using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	// Token: 0x020000C0 RID: 192
	public class DeflaterConstants
	{
		// Token: 0x040003B6 RID: 950
		public const bool DEBUGGING = false;

		// Token: 0x040003B7 RID: 951
		public const int STORED_BLOCK = 0;

		// Token: 0x040003B8 RID: 952
		public const int STATIC_TREES = 1;

		// Token: 0x040003B9 RID: 953
		public const int DYN_TREES = 2;

		// Token: 0x040003BA RID: 954
		public const int PRESET_DICT = 32;

		// Token: 0x040003BB RID: 955
		public const int DEFAULT_MEM_LEVEL = 8;

		// Token: 0x040003BC RID: 956
		public const int MAX_MATCH = 258;

		// Token: 0x040003BD RID: 957
		public const int MIN_MATCH = 3;

		// Token: 0x040003BE RID: 958
		public const int MAX_WBITS = 15;

		// Token: 0x040003BF RID: 959
		public const int WSIZE = 32768;

		// Token: 0x040003C0 RID: 960
		public const int WMASK = 32767;

		// Token: 0x040003C1 RID: 961
		public const int HASH_BITS = 15;

		// Token: 0x040003C2 RID: 962
		public const int HASH_SIZE = 32768;

		// Token: 0x040003C3 RID: 963
		public const int HASH_MASK = 32767;

		// Token: 0x040003C4 RID: 964
		public const int HASH_SHIFT = 5;

		// Token: 0x040003C5 RID: 965
		public const int MIN_LOOKAHEAD = 262;

		// Token: 0x040003C6 RID: 966
		public const int MAX_DIST = 32506;

		// Token: 0x040003C7 RID: 967
		public const int PENDING_BUF_SIZE = 65536;

		// Token: 0x040003C8 RID: 968
		public const int DEFLATE_STORED = 0;

		// Token: 0x040003C9 RID: 969
		public const int DEFLATE_FAST = 1;

		// Token: 0x040003CA RID: 970
		public const int DEFLATE_SLOW = 2;

		// Token: 0x040003CB RID: 971
		public static int MAX_BLOCK_SIZE = Math.Min(65535, 65531);

		// Token: 0x040003CC RID: 972
		public static int[] GOOD_LENGTH = new int[]
		{
			0,
			4,
			4,
			4,
			4,
			8,
			8,
			8,
			32,
			32
		};

		// Token: 0x040003CD RID: 973
		public static int[] MAX_LAZY = new int[]
		{
			0,
			4,
			5,
			6,
			4,
			16,
			16,
			32,
			128,
			258
		};

		// Token: 0x040003CE RID: 974
		public static int[] NICE_LENGTH = new int[]
		{
			0,
			8,
			16,
			32,
			16,
			32,
			128,
			128,
			258,
			258
		};

		// Token: 0x040003CF RID: 975
		public static int[] MAX_CHAIN = new int[]
		{
			0,
			4,
			8,
			32,
			16,
			32,
			128,
			256,
			1024,
			4096
		};

		// Token: 0x040003D0 RID: 976
		public static int[] COMPR_FUNC = new int[]
		{
			0,
			1,
			1,
			1,
			1,
			2,
			2,
			2,
			2,
			2
		};
	}
}
