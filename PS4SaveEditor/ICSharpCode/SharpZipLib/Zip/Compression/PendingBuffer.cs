using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	// Token: 0x020000C5 RID: 197
	public class PendingBuffer
	{
		// Token: 0x0600083D RID: 2109 RVA: 0x0002FE59 File Offset: 0x0002E059
		public PendingBuffer() : this(4096)
		{
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0002FE66 File Offset: 0x0002E066
		public PendingBuffer(int bufferSize)
		{
			this.buffer_ = new byte[bufferSize];
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0002FE7C File Offset: 0x0002E07C
		public void Reset()
		{
			this.start = (this.end = (this.bitCount = 0));
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0002FEA4 File Offset: 0x0002E0A4
		public void WriteByte(int value)
		{
			this.buffer_[this.end++] = (byte)value;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0002FECC File Offset: 0x0002E0CC
		public void WriteShort(int value)
		{
			this.buffer_[this.end++] = (byte)value;
			this.buffer_[this.end++] = (byte)(value >> 8);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0002FF10 File Offset: 0x0002E110
		public void WriteInt(int value)
		{
			this.buffer_[this.end++] = (byte)value;
			this.buffer_[this.end++] = (byte)(value >> 8);
			this.buffer_[this.end++] = (byte)(value >> 16);
			this.buffer_[this.end++] = (byte)(value >> 24);
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0002FF8D File Offset: 0x0002E18D
		public void WriteBlock(byte[] block, int offset, int length)
		{
			Array.Copy(block, offset, this.buffer_, this.end, length);
			this.end += length;
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x0002FFB1 File Offset: 0x0002E1B1
		public int BitCount
		{
			get
			{
				return this.bitCount;
			}
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002FFBC File Offset: 0x0002E1BC
		public void AlignToByte()
		{
			if (this.bitCount > 0)
			{
				this.buffer_[this.end++] = (byte)this.bits;
				if (this.bitCount > 8)
				{
					this.buffer_[this.end++] = (byte)(this.bits >> 8);
				}
			}
			this.bits = 0u;
			this.bitCount = 0;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0003002C File Offset: 0x0002E22C
		public void WriteBits(int b, int count)
		{
			this.bits |= (uint)((uint)b << this.bitCount);
			this.bitCount += count;
			if (this.bitCount >= 16)
			{
				this.buffer_[this.end++] = (byte)this.bits;
				this.buffer_[this.end++] = (byte)(this.bits >> 8);
				this.bits >>= 16;
				this.bitCount -= 16;
			}
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x000300C8 File Offset: 0x0002E2C8
		public void WriteShortMSB(int s)
		{
			this.buffer_[this.end++] = (byte)(s >> 8);
			this.buffer_[this.end++] = (byte)s;
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x0003010B File Offset: 0x0002E30B
		public bool IsFlushed
		{
			get
			{
				return this.end == 0;
			}
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00030118 File Offset: 0x0002E318
		public int Flush(byte[] output, int offset, int length)
		{
			if (this.bitCount >= 8)
			{
				this.buffer_[this.end++] = (byte)this.bits;
				this.bits >>= 8;
				this.bitCount -= 8;
			}
			if (length > this.end - this.start)
			{
				length = this.end - this.start;
				Array.Copy(this.buffer_, this.start, output, offset, length);
				this.start = 0;
				this.end = 0;
			}
			else
			{
				Array.Copy(this.buffer_, this.start, output, offset, length);
				this.start += length;
			}
			return length;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x000301D0 File Offset: 0x0002E3D0
		public byte[] ToByteArray()
		{
			byte[] array = new byte[this.end - this.start];
			Array.Copy(this.buffer_, this.start, array, 0, array.Length);
			this.start = 0;
			this.end = 0;
			return array;
		}

		// Token: 0x0400040B RID: 1035
		private byte[] buffer_;

		// Token: 0x0400040C RID: 1036
		private int start;

		// Token: 0x0400040D RID: 1037
		private int end;

		// Token: 0x0400040E RID: 1038
		private uint bits;

		// Token: 0x0400040F RID: 1039
		private int bitCount;
	}
}
