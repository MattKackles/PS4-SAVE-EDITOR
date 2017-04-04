using System;

namespace Ionic.Zip
{
	// Token: 0x0200014A RID: 330
	internal static class ZipConstants
	{
		// Token: 0x0400074D RID: 1869
		public const uint PackedToRemovableMedia = 808471376u;

		// Token: 0x0400074E RID: 1870
		public const uint Zip64EndOfCentralDirectoryRecordSignature = 101075792u;

		// Token: 0x0400074F RID: 1871
		public const uint Zip64EndOfCentralDirectoryLocatorSignature = 117853008u;

		// Token: 0x04000750 RID: 1872
		public const uint EndOfCentralDirectorySignature = 101010256u;

		// Token: 0x04000751 RID: 1873
		public const int ZipEntrySignature = 67324752;

		// Token: 0x04000752 RID: 1874
		public const int ZipEntryDataDescriptorSignature = 134695760;

		// Token: 0x04000753 RID: 1875
		public const int SplitArchiveSignature = 134695760;

		// Token: 0x04000754 RID: 1876
		public const int ZipDirEntrySignature = 33639248;

		// Token: 0x04000755 RID: 1877
		public const int AesKeySize = 192;

		// Token: 0x04000756 RID: 1878
		public const int AesBlockSize = 128;

		// Token: 0x04000757 RID: 1879
		public const ushort AesAlgId128 = 26126;

		// Token: 0x04000758 RID: 1880
		public const ushort AesAlgId192 = 26127;

		// Token: 0x04000759 RID: 1881
		public const ushort AesAlgId256 = 26128;
	}
}
