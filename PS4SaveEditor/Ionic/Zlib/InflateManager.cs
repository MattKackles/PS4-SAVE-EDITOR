using System;

namespace Ionic.Zlib
{
	// Token: 0x02000140 RID: 320
	internal sealed class InflateManager
	{
		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x00047453 File Offset: 0x00045653
		// (set) Token: 0x06000CA8 RID: 3240 RVA: 0x0004745B File Offset: 0x0004565B
		internal bool HandleRfc1950HeaderBytes
		{
			get
			{
				return this._handleRfc1950HeaderBytes;
			}
			set
			{
				this._handleRfc1950HeaderBytes = value;
			}
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00047464 File Offset: 0x00045664
		public InflateManager()
		{
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00047473 File Offset: 0x00045673
		public InflateManager(bool expectRfc1950HeaderBytes)
		{
			this._handleRfc1950HeaderBytes = expectRfc1950HeaderBytes;
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0004748C File Offset: 0x0004568C
		internal int Reset()
		{
			this._codec.TotalBytesIn = (this._codec.TotalBytesOut = 0L);
			this._codec.Message = null;
			this.mode = (this.HandleRfc1950HeaderBytes ? InflateManager.InflateManagerMode.METHOD : InflateManager.InflateManagerMode.BLOCKS);
			this.blocks.Reset();
			return 0;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x000474DF File Offset: 0x000456DF
		internal int End()
		{
			if (this.blocks != null)
			{
				this.blocks.Free();
			}
			this.blocks = null;
			return 0;
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x000474FC File Offset: 0x000456FC
		internal int Initialize(ZlibCodec codec, int w)
		{
			this._codec = codec;
			this._codec.Message = null;
			this.blocks = null;
			if (w < 8 || w > 15)
			{
				this.End();
				throw new ZlibException("Bad window size.");
			}
			this.wbits = w;
			this.blocks = new InflateBlocks(codec, this.HandleRfc1950HeaderBytes ? this : null, 1 << w);
			this.Reset();
			return 0;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0004756C File Offset: 0x0004576C
		internal int Inflate(FlushType flush)
		{
			if (this._codec.InputBuffer == null)
			{
				throw new ZlibException("InputBuffer is null. ");
			}
			int num = 0;
			int num2 = -5;
			while (true)
			{
				switch (this.mode)
				{
				case InflateManager.InflateManagerMode.METHOD:
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					if (((this.method = (int)this._codec.InputBuffer[this._codec.NextIn++]) & 15) != 8)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = string.Format("unknown compression method (0x{0:X2})", this.method);
						this.marker = 5;
						continue;
					}
					if ((this.method >> 4) + 8 > this.wbits)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = string.Format("invalid window size ({0})", (this.method >> 4) + 8);
						this.marker = 5;
						continue;
					}
					this.mode = InflateManager.InflateManagerMode.FLAG;
					continue;
				case InflateManager.InflateManagerMode.FLAG:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					int num3 = (int)(this._codec.InputBuffer[this._codec.NextIn++] & 255);
					if (((this.method << 8) + num3) % 31 != 0)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = "incorrect header check";
						this.marker = 5;
						continue;
					}
					this.mode = (((num3 & 32) == 0) ? InflateManager.InflateManagerMode.BLOCKS : InflateManager.InflateManagerMode.DICT4);
					continue;
				}
				case InflateManager.InflateManagerMode.DICT4:
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck = (uint)((long)((long)this._codec.InputBuffer[this._codec.NextIn++] << 24) & (long)((ulong)-16777216));
					this.mode = InflateManager.InflateManagerMode.DICT3;
					continue;
				case InflateManager.InflateManagerMode.DICT3:
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck += (uint)((int)this._codec.InputBuffer[this._codec.NextIn++] << 16 & 16711680);
					this.mode = InflateManager.InflateManagerMode.DICT2;
					continue;
				case InflateManager.InflateManagerMode.DICT2:
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck += (uint)((int)this._codec.InputBuffer[this._codec.NextIn++] << 8 & 65280);
					this.mode = InflateManager.InflateManagerMode.DICT1;
					continue;
				case InflateManager.InflateManagerMode.DICT1:
					goto IL_37E;
				case InflateManager.InflateManagerMode.DICT0:
					goto IL_407;
				case InflateManager.InflateManagerMode.BLOCKS:
					num2 = this.blocks.Process(num2);
					if (num2 == -3)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this.marker = 0;
						continue;
					}
					if (num2 == 0)
					{
						num2 = num;
					}
					if (num2 != 1)
					{
						return num2;
					}
					num2 = num;
					this.computedCheck = this.blocks.Reset();
					if (!this.HandleRfc1950HeaderBytes)
					{
						goto Block_16;
					}
					this.mode = InflateManager.InflateManagerMode.CHECK4;
					continue;
				case InflateManager.InflateManagerMode.CHECK4:
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck = (uint)((long)((long)this._codec.InputBuffer[this._codec.NextIn++] << 24) & (long)((ulong)-16777216));
					this.mode = InflateManager.InflateManagerMode.CHECK3;
					continue;
				case InflateManager.InflateManagerMode.CHECK3:
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck += (uint)((int)this._codec.InputBuffer[this._codec.NextIn++] << 16 & 16711680);
					this.mode = InflateManager.InflateManagerMode.CHECK2;
					continue;
				case InflateManager.InflateManagerMode.CHECK2:
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck += (uint)((int)this._codec.InputBuffer[this._codec.NextIn++] << 8 & 65280);
					this.mode = InflateManager.InflateManagerMode.CHECK1;
					continue;
				case InflateManager.InflateManagerMode.CHECK1:
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck += (uint)(this._codec.InputBuffer[this._codec.NextIn++] & 255);
					if (this.computedCheck != this.expectedCheck)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = "incorrect data check";
						this.marker = 5;
						continue;
					}
					goto IL_6A4;
				case InflateManager.InflateManagerMode.DONE:
					return 1;
				case InflateManager.InflateManagerMode.BAD:
					goto IL_6B0;
				}
				break;
			}
			throw new ZlibException("Stream error.");
			IL_37E:
			if (this._codec.AvailableBytesIn == 0)
			{
				return num2;
			}
			this._codec.AvailableBytesIn--;
			this._codec.TotalBytesIn += 1L;
			this.expectedCheck += (uint)(this._codec.InputBuffer[this._codec.NextIn++] & 255);
			this._codec._Adler32 = this.expectedCheck;
			this.mode = InflateManager.InflateManagerMode.DICT0;
			return 2;
			IL_407:
			this.mode = InflateManager.InflateManagerMode.BAD;
			this._codec.Message = "need dictionary";
			this.marker = 0;
			return -2;
			Block_16:
			this.mode = InflateManager.InflateManagerMode.DONE;
			return 1;
			IL_6A4:
			this.mode = InflateManager.InflateManagerMode.DONE;
			return 1;
			IL_6B0:
			throw new ZlibException(string.Format("Bad state ({0})", this._codec.Message));
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00047C50 File Offset: 0x00045E50
		internal int SetDictionary(byte[] dictionary)
		{
			int start = 0;
			int num = dictionary.Length;
			if (this.mode != InflateManager.InflateManagerMode.DICT0)
			{
				throw new ZlibException("Stream error.");
			}
			if (Adler.Adler32(1u, dictionary, 0, dictionary.Length) != this._codec._Adler32)
			{
				return -3;
			}
			this._codec._Adler32 = Adler.Adler32(0u, null, 0, 0);
			if (num >= 1 << this.wbits)
			{
				num = (1 << this.wbits) - 1;
				start = dictionary.Length - num;
			}
			this.blocks.SetDictionary(dictionary, start, num);
			this.mode = InflateManager.InflateManagerMode.BLOCKS;
			return 0;
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00047CE0 File Offset: 0x00045EE0
		internal int Sync()
		{
			if (this.mode != InflateManager.InflateManagerMode.BAD)
			{
				this.mode = InflateManager.InflateManagerMode.BAD;
				this.marker = 0;
			}
			int num;
			if ((num = this._codec.AvailableBytesIn) == 0)
			{
				return -5;
			}
			int num2 = this._codec.NextIn;
			int num3 = this.marker;
			while (num != 0 && num3 < 4)
			{
				if (this._codec.InputBuffer[num2] == InflateManager.mark[num3])
				{
					num3++;
				}
				else if (this._codec.InputBuffer[num2] != 0)
				{
					num3 = 0;
				}
				else
				{
					num3 = 4 - num3;
				}
				num2++;
				num--;
			}
			this._codec.TotalBytesIn += (long)(num2 - this._codec.NextIn);
			this._codec.NextIn = num2;
			this._codec.AvailableBytesIn = num;
			this.marker = num3;
			if (num3 != 4)
			{
				return -3;
			}
			long totalBytesIn = this._codec.TotalBytesIn;
			long totalBytesOut = this._codec.TotalBytesOut;
			this.Reset();
			this._codec.TotalBytesIn = totalBytesIn;
			this._codec.TotalBytesOut = totalBytesOut;
			this.mode = InflateManager.InflateManagerMode.BLOCKS;
			return 0;
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00047DF6 File Offset: 0x00045FF6
		internal int SyncPoint(ZlibCodec z)
		{
			return this.blocks.SyncPoint();
		}

		// Token: 0x040006CC RID: 1740
		private const int PRESET_DICT = 32;

		// Token: 0x040006CD RID: 1741
		private const int Z_DEFLATED = 8;

		// Token: 0x040006CE RID: 1742
		private InflateManager.InflateManagerMode mode;

		// Token: 0x040006CF RID: 1743
		internal ZlibCodec _codec;

		// Token: 0x040006D0 RID: 1744
		internal int method;

		// Token: 0x040006D1 RID: 1745
		internal uint computedCheck;

		// Token: 0x040006D2 RID: 1746
		internal uint expectedCheck;

		// Token: 0x040006D3 RID: 1747
		internal int marker;

		// Token: 0x040006D4 RID: 1748
		private bool _handleRfc1950HeaderBytes = true;

		// Token: 0x040006D5 RID: 1749
		internal int wbits;

		// Token: 0x040006D6 RID: 1750
		internal InflateBlocks blocks;

		// Token: 0x040006D7 RID: 1751
		private static readonly byte[] mark = new byte[]
		{
			0,
			0,
			255,
			255
		};

		// Token: 0x02000141 RID: 321
		private enum InflateManagerMode
		{
			// Token: 0x040006D9 RID: 1753
			METHOD,
			// Token: 0x040006DA RID: 1754
			FLAG,
			// Token: 0x040006DB RID: 1755
			DICT4,
			// Token: 0x040006DC RID: 1756
			DICT3,
			// Token: 0x040006DD RID: 1757
			DICT2,
			// Token: 0x040006DE RID: 1758
			DICT1,
			// Token: 0x040006DF RID: 1759
			DICT0,
			// Token: 0x040006E0 RID: 1760
			BLOCKS,
			// Token: 0x040006E1 RID: 1761
			CHECK4,
			// Token: 0x040006E2 RID: 1762
			CHECK3,
			// Token: 0x040006E3 RID: 1763
			CHECK2,
			// Token: 0x040006E4 RID: 1764
			CHECK1,
			// Token: 0x040006E5 RID: 1765
			DONE,
			// Token: 0x040006E6 RID: 1766
			BAD
		}
	}
}
