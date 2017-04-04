using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
	// Token: 0x020000CE RID: 206
	public class StreamManipulator
	{
		// Token: 0x060008BD RID: 2237 RVA: 0x00032270 File Offset: 0x00030470
		public int PeekBits(int bitCount)
		{
			if (this.bitsInBuffer_ < bitCount)
			{
				if (this.windowStart_ == this.windowEnd_)
				{
					return -1;
				}
				this.buffer_ |= (uint)((uint)((int)(this.window_[this.windowStart_++] & 255) | (int)(this.window_[this.windowStart_++] & 255) << 8) << this.bitsInBuffer_);
				this.bitsInBuffer_ += 16;
			}
			return (int)((ulong)this.buffer_ & (ulong)((long)((1 << bitCount) - 1)));
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0003230D File Offset: 0x0003050D
		public void DropBits(int bitCount)
		{
			this.buffer_ >>= bitCount;
			this.bitsInBuffer_ -= bitCount;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00032330 File Offset: 0x00030530
		public int GetBits(int bitCount)
		{
			int num = this.PeekBits(bitCount);
			if (num >= 0)
			{
				this.DropBits(bitCount);
			}
			return num;
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00032351 File Offset: 0x00030551
		public int AvailableBits
		{
			get
			{
				return this.bitsInBuffer_;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x00032359 File Offset: 0x00030559
		public int AvailableBytes
		{
			get
			{
				return this.windowEnd_ - this.windowStart_ + (this.bitsInBuffer_ >> 3);
			}
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00032371 File Offset: 0x00030571
		public void SkipToByteBoundary()
		{
			this.buffer_ >>= (this.bitsInBuffer_ & 7);
			this.bitsInBuffer_ &= -8;
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0003239A File Offset: 0x0003059A
		public bool IsNeedingInput
		{
			get
			{
				return this.windowStart_ == this.windowEnd_;
			}
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x000323AC File Offset: 0x000305AC
		public int CopyBytes(byte[] output, int offset, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			if ((this.bitsInBuffer_ & 7) != 0)
			{
				throw new InvalidOperationException("Bit buffer is not byte aligned!");
			}
			int num = 0;
			while (this.bitsInBuffer_ > 0 && length > 0)
			{
				output[offset++] = (byte)this.buffer_;
				this.buffer_ >>= 8;
				this.bitsInBuffer_ -= 8;
				length--;
				num++;
			}
			if (length == 0)
			{
				return num;
			}
			int num2 = this.windowEnd_ - this.windowStart_;
			if (length > num2)
			{
				length = num2;
			}
			Array.Copy(this.window_, this.windowStart_, output, offset, length);
			this.windowStart_ += length;
			if ((this.windowStart_ - this.windowEnd_ & 1) != 0)
			{
				this.buffer_ = (uint)(this.window_[this.windowStart_++] & 255);
				this.bitsInBuffer_ = 8;
			}
			return num + length;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x000324A0 File Offset: 0x000306A0
		public void Reset()
		{
			this.buffer_ = 0u;
			this.windowStart_ = (this.windowEnd_ = (this.bitsInBuffer_ = 0));
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x000324D0 File Offset: 0x000306D0
		public void SetInput(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Cannot be negative");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Cannot be negative");
			}
			if (this.windowStart_ < this.windowEnd_)
			{
				throw new InvalidOperationException("Old input was not completely processed");
			}
			int num = offset + count;
			if (offset > num || num > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if ((count & 1) != 0)
			{
				this.buffer_ |= (uint)((uint)(buffer[offset++] & 255) << this.bitsInBuffer_);
				this.bitsInBuffer_ += 8;
			}
			this.window_ = buffer;
			this.windowStart_ = offset;
			this.windowEnd_ = num;
		}

		// Token: 0x04000465 RID: 1125
		private byte[] window_;

		// Token: 0x04000466 RID: 1126
		private int windowStart_;

		// Token: 0x04000467 RID: 1127
		private int windowEnd_;

		// Token: 0x04000468 RID: 1128
		private uint buffer_;

		// Token: 0x04000469 RID: 1129
		private int bitsInBuffer_;
	}
}
