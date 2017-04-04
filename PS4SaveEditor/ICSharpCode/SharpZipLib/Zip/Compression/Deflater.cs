using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	// Token: 0x020000BF RID: 191
	public class Deflater
	{
		// Token: 0x060007FB RID: 2043 RVA: 0x0002DAC5 File Offset: 0x0002BCC5
		public Deflater() : this(-1, false)
		{
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0002DACF File Offset: 0x0002BCCF
		public Deflater(int level) : this(level, false)
		{
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0002DADC File Offset: 0x0002BCDC
		public Deflater(int level, bool noZlibHeaderOrFooter)
		{
			if (level == -1)
			{
				level = 6;
			}
			else if (level < 0 || level > 9)
			{
				throw new ArgumentOutOfRangeException("level");
			}
			this.pending = new DeflaterPending();
			this.engine = new DeflaterEngine(this.pending);
			this.noZlibHeaderOrFooter = noZlibHeaderOrFooter;
			this.SetStrategy(DeflateStrategy.Default);
			this.SetLevel(level);
			this.Reset();
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0002DB43 File Offset: 0x0002BD43
		public void Reset()
		{
			this.state = (this.noZlibHeaderOrFooter ? 16 : 0);
			this.totalOut = 0L;
			this.pending.Reset();
			this.engine.Reset();
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0002DB76 File Offset: 0x0002BD76
		public int Adler
		{
			get
			{
				return this.engine.Adler;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x0002DB83 File Offset: 0x0002BD83
		public long TotalIn
		{
			get
			{
				return this.engine.TotalIn;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x0002DB90 File Offset: 0x0002BD90
		public long TotalOut
		{
			get
			{
				return this.totalOut;
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0002DB98 File Offset: 0x0002BD98
		public void Flush()
		{
			this.state |= 4;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0002DBA8 File Offset: 0x0002BDA8
		public void Finish()
		{
			this.state |= 12;
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x0002DBB9 File Offset: 0x0002BDB9
		public bool IsFinished
		{
			get
			{
				return this.state == 30 && this.pending.IsFlushed;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x0002DBD2 File Offset: 0x0002BDD2
		public bool IsNeedingInput
		{
			get
			{
				return this.engine.NeedsInput();
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0002DBDF File Offset: 0x0002BDDF
		public void SetInput(byte[] input)
		{
			this.SetInput(input, 0, input.Length);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0002DBEC File Offset: 0x0002BDEC
		public void SetInput(byte[] input, int offset, int count)
		{
			if ((this.state & 8) != 0)
			{
				throw new InvalidOperationException("Finish() already called");
			}
			this.engine.SetInput(input, offset, count);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0002DC11 File Offset: 0x0002BE11
		public void SetLevel(int level)
		{
			if (level == -1)
			{
				level = 6;
			}
			else if (level < 0 || level > 9)
			{
				throw new ArgumentOutOfRangeException("level");
			}
			if (this.level != level)
			{
				this.level = level;
				this.engine.SetLevel(level);
			}
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002DC4C File Offset: 0x0002BE4C
		public int GetLevel()
		{
			return this.level;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0002DC54 File Offset: 0x0002BE54
		public void SetStrategy(DeflateStrategy strategy)
		{
			this.engine.Strategy = strategy;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0002DC62 File Offset: 0x0002BE62
		public int Deflate(byte[] output)
		{
			return this.Deflate(output, 0, output.Length);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0002DC70 File Offset: 0x0002BE70
		public int Deflate(byte[] output, int offset, int length)
		{
			int num = length;
			if (this.state == 127)
			{
				throw new InvalidOperationException("Deflater closed");
			}
			if (this.state < 16)
			{
				int num2 = 30720;
				int num3 = this.level - 1 >> 1;
				if (num3 < 0 || num3 > 3)
				{
					num3 = 3;
				}
				num2 |= num3 << 6;
				if ((this.state & 1) != 0)
				{
					num2 |= 32;
				}
				num2 += 31 - num2 % 31;
				this.pending.WriteShortMSB(num2);
				if ((this.state & 1) != 0)
				{
					int adler = this.engine.Adler;
					this.engine.ResetAdler();
					this.pending.WriteShortMSB(adler >> 16);
					this.pending.WriteShortMSB(adler & 65535);
				}
				this.state = (16 | (this.state & 12));
			}
			while (true)
			{
				int num4 = this.pending.Flush(output, offset, length);
				offset += num4;
				this.totalOut += (long)num4;
				length -= num4;
				if (length == 0 || this.state == 30)
				{
					goto IL_1DE;
				}
				if (!this.engine.Deflate((this.state & 4) != 0, (this.state & 8) != 0))
				{
					if (this.state == 16)
					{
						break;
					}
					if (this.state == 20)
					{
						if (this.level != 0)
						{
							for (int i = 8 + (-this.pending.BitCount & 7); i > 0; i -= 10)
							{
								this.pending.WriteBits(2, 10);
							}
						}
						this.state = 16;
					}
					else if (this.state == 28)
					{
						this.pending.AlignToByte();
						if (!this.noZlibHeaderOrFooter)
						{
							int adler2 = this.engine.Adler;
							this.pending.WriteShortMSB(adler2 >> 16);
							this.pending.WriteShortMSB(adler2 & 65535);
						}
						this.state = 30;
					}
				}
			}
			return num - length;
			IL_1DE:
			return num - length;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0002DE5E File Offset: 0x0002C05E
		public void SetDictionary(byte[] dictionary)
		{
			this.SetDictionary(dictionary, 0, dictionary.Length);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0002DE6B File Offset: 0x0002C06B
		public void SetDictionary(byte[] dictionary, int index, int count)
		{
			if (this.state != 0)
			{
				throw new InvalidOperationException();
			}
			this.state = 1;
			this.engine.SetDictionary(dictionary, index, count);
		}

		// Token: 0x040003A1 RID: 929
		public const int BEST_COMPRESSION = 9;

		// Token: 0x040003A2 RID: 930
		public const int BEST_SPEED = 1;

		// Token: 0x040003A3 RID: 931
		public const int DEFAULT_COMPRESSION = -1;

		// Token: 0x040003A4 RID: 932
		public const int NO_COMPRESSION = 0;

		// Token: 0x040003A5 RID: 933
		public const int DEFLATED = 8;

		// Token: 0x040003A6 RID: 934
		private const int IS_SETDICT = 1;

		// Token: 0x040003A7 RID: 935
		private const int IS_FLUSHING = 4;

		// Token: 0x040003A8 RID: 936
		private const int IS_FINISHING = 8;

		// Token: 0x040003A9 RID: 937
		private const int INIT_STATE = 0;

		// Token: 0x040003AA RID: 938
		private const int SETDICT_STATE = 1;

		// Token: 0x040003AB RID: 939
		private const int BUSY_STATE = 16;

		// Token: 0x040003AC RID: 940
		private const int FLUSHING_STATE = 20;

		// Token: 0x040003AD RID: 941
		private const int FINISHING_STATE = 28;

		// Token: 0x040003AE RID: 942
		private const int FINISHED_STATE = 30;

		// Token: 0x040003AF RID: 943
		private const int CLOSED_STATE = 127;

		// Token: 0x040003B0 RID: 944
		private int level;

		// Token: 0x040003B1 RID: 945
		private bool noZlibHeaderOrFooter;

		// Token: 0x040003B2 RID: 946
		private int state;

		// Token: 0x040003B3 RID: 947
		private long totalOut;

		// Token: 0x040003B4 RID: 948
		private DeflaterPending pending;

		// Token: 0x040003B5 RID: 949
		private DeflaterEngine engine;
	}
}
