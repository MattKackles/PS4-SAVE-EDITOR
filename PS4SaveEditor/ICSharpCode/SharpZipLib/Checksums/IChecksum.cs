using System;

namespace ICSharpCode.SharpZipLib.Checksums
{
	// Token: 0x020000A0 RID: 160
	public interface IChecksum
	{
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000750 RID: 1872
		long Value
		{
			get;
		}

		// Token: 0x06000751 RID: 1873
		void Reset();

		// Token: 0x06000752 RID: 1874
		void Update(int value);

		// Token: 0x06000753 RID: 1875
		void Update(byte[] buffer);

		// Token: 0x06000754 RID: 1876
		void Update(byte[] buffer, int offset, int count);
	}
}
