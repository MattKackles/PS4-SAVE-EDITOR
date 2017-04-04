using System;

namespace ICSharpCode.SharpZipLib.Checksums
{
	// Token: 0x020000A1 RID: 161
	public sealed class Adler32 : IChecksum
	{
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x0002B7D7 File Offset: 0x000299D7
		public long Value
		{
			get
			{
				return (long)((ulong)this.checksum);
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0002B7E0 File Offset: 0x000299E0
		public Adler32()
		{
			this.Reset();
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0002B7EE File Offset: 0x000299EE
		public void Reset()
		{
			this.checksum = 1u;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0002B7F8 File Offset: 0x000299F8
		public void Update(int value)
		{
			uint num = this.checksum & 65535u;
			uint num2 = this.checksum >> 16;
			num = (num + (uint)(value & 255)) % 65521u;
			num2 = (num + num2) % 65521u;
			this.checksum = (num2 << 16) + num;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0002B842 File Offset: 0x00029A42
		public void Update(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.Update(buffer, 0, buffer.Length);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0002B860 File Offset: 0x00029A60
		public void Update(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "cannot be negative");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "cannot be negative");
			}
			if (offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset", "not a valid index into buffer");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count", "exceeds buffer size");
			}
			uint num = this.checksum & 65535u;
			uint num2 = this.checksum >> 16;
			while (count > 0)
			{
				int num3 = 3800;
				if (num3 > count)
				{
					num3 = count;
				}
				count -= num3;
				while (--num3 >= 0)
				{
					num += (uint)(buffer[offset++] & 255);
					num2 += num;
				}
				num %= 65521u;
				num2 %= 65521u;
			}
			this.checksum = (num2 << 16 | num);
		}

		// Token: 0x04000368 RID: 872
		private const uint BASE = 65521u;

		// Token: 0x04000369 RID: 873
		private uint checksum;
	}
}
