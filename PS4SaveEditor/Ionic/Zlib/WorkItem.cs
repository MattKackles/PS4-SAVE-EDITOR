using System;

namespace Ionic.Zlib
{
	// Token: 0x02000144 RID: 324
	internal class WorkItem
	{
		// Token: 0x06000CC8 RID: 3272 RVA: 0x0004A1B8 File Offset: 0x000483B8
		public WorkItem(int size, CompressionLevel compressLevel, CompressionStrategy strategy, int ix)
		{
			this.buffer = new byte[size];
			int num = size + (size / 32768 + 1) * 5 * 2;
			this.compressed = new byte[num];
			this.compressor = new ZlibCodec();
			this.compressor.InitializeDeflate(compressLevel, false);
			this.compressor.OutputBuffer = this.compressed;
			this.compressor.InputBuffer = this.buffer;
			this.index = ix;
		}

		// Token: 0x04000702 RID: 1794
		public byte[] buffer;

		// Token: 0x04000703 RID: 1795
		public byte[] compressed;

		// Token: 0x04000704 RID: 1796
		public int crc;

		// Token: 0x04000705 RID: 1797
		public int index;

		// Token: 0x04000706 RID: 1798
		public int ordinal;

		// Token: 0x04000707 RID: 1799
		public int inputBytesAvailable;

		// Token: 0x04000708 RID: 1800
		public int compressedBytesAvailable;

		// Token: 0x04000709 RID: 1801
		public ZlibCodec compressor;
	}
}
