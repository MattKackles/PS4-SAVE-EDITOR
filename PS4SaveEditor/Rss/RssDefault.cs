using System;

namespace Rss
{
	// Token: 0x02000092 RID: 146
	[Serializable]
	public class RssDefault
	{
		// Token: 0x060006D3 RID: 1747 RVA: 0x000269E6 File Offset: 0x00024BE6
		public static string Check(string input)
		{
			if (input != null)
			{
				return input;
			}
			return "";
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000269F2 File Offset: 0x00024BF2
		public static int Check(int input)
		{
			if (input >= -1)
			{
				return input;
			}
			return -1;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x000269FB File Offset: 0x00024BFB
		public static Uri Check(Uri input)
		{
			if (!(input == null))
			{
				return input;
			}
			return RssDefault.Uri;
		}

		// Token: 0x04000323 RID: 803
		public const string String = "";

		// Token: 0x04000324 RID: 804
		public const int Int = -1;

		// Token: 0x04000325 RID: 805
		public static readonly DateTime DateTime = DateTime.MinValue;

		// Token: 0x04000326 RID: 806
		public static readonly Uri Uri = null;
	}
}
