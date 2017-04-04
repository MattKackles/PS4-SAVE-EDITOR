using System;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	// Token: 0x020000C7 RID: 199
	public class Inflater
	{
		// Token: 0x0600084C RID: 2124 RVA: 0x00030222 File Offset: 0x0002E422
		public Inflater() : this(false)
		{
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0003022B File Offset: 0x0002E42B
		public Inflater(bool noHeader)
		{
			this.noHeader = noHeader;
			this.adler = new Adler32();
			this.input = new StreamManipulator();
			this.outputWindow = new OutputWindow();
			this.mode = (noHeader ? 2 : 0);
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x00030268 File Offset: 0x0002E468
		public void Reset()
		{
			this.mode = (this.noHeader ? 2 : 0);
			this.totalIn = 0L;
			this.totalOut = 0L;
			this.input.Reset();
			this.outputWindow.Reset();
			this.dynHeader = null;
			this.litlenTree = null;
			this.distTree = null;
			this.isLastBlock = false;
			this.adler.Reset();
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x000302D4 File Offset: 0x0002E4D4
		private bool DecodeHeader()
		{
			int num = this.input.PeekBits(16);
			if (num < 0)
			{
				return false;
			}
			this.input.DropBits(16);
			num = ((num << 8 | num >> 8) & 65535);
			if (num % 31 != 0)
			{
				throw new SharpZipBaseException("Header checksum illegal");
			}
			if ((num & 3840) != 2048)
			{
				throw new SharpZipBaseException("Compression Method unknown");
			}
			if ((num & 32) == 0)
			{
				this.mode = 2;
			}
			else
			{
				this.mode = 1;
				this.neededBits = 32;
			}
			return true;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0003035C File Offset: 0x0002E55C
		private bool DecodeDict()
		{
			while (this.neededBits > 0)
			{
				int num = this.input.PeekBits(8);
				if (num < 0)
				{
					return false;
				}
				this.input.DropBits(8);
				this.readAdler = (this.readAdler << 8 | num);
				this.neededBits -= 8;
			}
			return false;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x000303B4 File Offset: 0x0002E5B4
		private bool DecodeHuffman()
		{
			int i = this.outputWindow.GetFreeSpace();
			while (i >= 258)
			{
				int symbol;
				switch (this.mode)
				{
				case 7:
					while (((symbol = this.litlenTree.GetSymbol(this.input)) & -256) == 0)
					{
						this.outputWindow.Write(symbol);
						if (--i < 258)
						{
							return true;
						}
					}
					if (symbol >= 257)
					{
						try
						{
							this.repLength = Inflater.CPLENS[symbol - 257];
							this.neededBits = Inflater.CPLEXT[symbol - 257];
						}
						catch (Exception)
						{
							throw new SharpZipBaseException("Illegal rep length code");
						}
						goto IL_C5;
					}
					if (symbol < 0)
					{
						return false;
					}
					this.distTree = null;
					this.litlenTree = null;
					this.mode = 2;
					return true;
				case 8:
					goto IL_C5;
				case 9:
					goto IL_114;
				case 10:
					break;
				default:
					throw new SharpZipBaseException("Inflater unknown mode");
				}
				IL_154:
				if (this.neededBits > 0)
				{
					this.mode = 10;
					int num = this.input.PeekBits(this.neededBits);
					if (num < 0)
					{
						return false;
					}
					this.input.DropBits(this.neededBits);
					this.repDist += num;
				}
				this.outputWindow.Repeat(this.repLength, this.repDist);
				i -= this.repLength;
				this.mode = 7;
				continue;
				IL_114:
				symbol = this.distTree.GetSymbol(this.input);
				if (symbol < 0)
				{
					return false;
				}
				try
				{
					this.repDist = Inflater.CPDIST[symbol];
					this.neededBits = Inflater.CPDEXT[symbol];
				}
				catch (Exception)
				{
					throw new SharpZipBaseException("Illegal rep dist code");
				}
				goto IL_154;
				IL_C5:
				if (this.neededBits > 0)
				{
					this.mode = 8;
					int num2 = this.input.PeekBits(this.neededBits);
					if (num2 < 0)
					{
						return false;
					}
					this.input.DropBits(this.neededBits);
					this.repLength += num2;
				}
				this.mode = 9;
				goto IL_114;
			}
			return true;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x000305BC File Offset: 0x0002E7BC
		private bool DecodeChksum()
		{
			while (this.neededBits > 0)
			{
				int num = this.input.PeekBits(8);
				if (num < 0)
				{
					return false;
				}
				this.input.DropBits(8);
				this.readAdler = (this.readAdler << 8 | num);
				this.neededBits -= 8;
			}
			if ((int)this.adler.Value != this.readAdler)
			{
				throw new SharpZipBaseException(string.Concat(new object[]
				{
					"Adler chksum doesn't match: ",
					(int)this.adler.Value,
					" vs. ",
					this.readAdler
				}));
			}
			this.mode = 12;
			return false;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00030674 File Offset: 0x0002E874
		private bool Decode()
		{
			switch (this.mode)
			{
			case 0:
				return this.DecodeHeader();
			case 1:
				return this.DecodeDict();
			case 2:
				if (this.isLastBlock)
				{
					if (this.noHeader)
					{
						this.mode = 12;
						return false;
					}
					this.input.SkipToByteBoundary();
					this.neededBits = 32;
					this.mode = 11;
					return true;
				}
				else
				{
					int num = this.input.PeekBits(3);
					if (num < 0)
					{
						return false;
					}
					this.input.DropBits(3);
					if ((num & 1) != 0)
					{
						this.isLastBlock = true;
					}
					switch (num >> 1)
					{
					case 0:
						this.input.SkipToByteBoundary();
						this.mode = 3;
						break;
					case 1:
						this.litlenTree = InflaterHuffmanTree.defLitLenTree;
						this.distTree = InflaterHuffmanTree.defDistTree;
						this.mode = 7;
						break;
					case 2:
						this.dynHeader = new InflaterDynHeader();
						this.mode = 6;
						break;
					default:
						throw new SharpZipBaseException("Unknown block type " + num);
					}
					return true;
				}
				break;
			case 3:
				if ((this.uncomprLen = this.input.PeekBits(16)) < 0)
				{
					return false;
				}
				this.input.DropBits(16);
				this.mode = 4;
				break;
			case 4:
				break;
			case 5:
				goto IL_1A9;
			case 6:
				if (!this.dynHeader.Decode(this.input))
				{
					return false;
				}
				this.litlenTree = this.dynHeader.BuildLitLenTree();
				this.distTree = this.dynHeader.BuildDistTree();
				this.mode = 7;
				goto IL_22D;
			case 7:
			case 8:
			case 9:
			case 10:
				goto IL_22D;
			case 11:
				return this.DecodeChksum();
			case 12:
				return false;
			default:
				throw new SharpZipBaseException("Inflater.Decode unknown mode");
			}
			int num2 = this.input.PeekBits(16);
			if (num2 < 0)
			{
				return false;
			}
			this.input.DropBits(16);
			if (num2 != (this.uncomprLen ^ 65535))
			{
				throw new SharpZipBaseException("broken uncompressed block");
			}
			this.mode = 5;
			IL_1A9:
			int num3 = this.outputWindow.CopyStored(this.input, this.uncomprLen);
			this.uncomprLen -= num3;
			if (this.uncomprLen == 0)
			{
				this.mode = 2;
				return true;
			}
			return !this.input.IsNeedingInput;
			IL_22D:
			return this.DecodeHuffman();
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x000308C1 File Offset: 0x0002EAC1
		public void SetDictionary(byte[] buffer)
		{
			this.SetDictionary(buffer, 0, buffer.Length);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x000308D0 File Offset: 0x0002EAD0
		public void SetDictionary(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (!this.IsNeedingDictionary)
			{
				throw new InvalidOperationException("Dictionary is not needed");
			}
			this.adler.Update(buffer, index, count);
			if ((int)this.adler.Value != this.readAdler)
			{
				throw new SharpZipBaseException("Wrong adler checksum");
			}
			this.adler.Reset();
			this.outputWindow.CopyDict(buffer, index, count);
			this.mode = 2;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00030969 File Offset: 0x0002EB69
		public void SetInput(byte[] buffer)
		{
			this.SetInput(buffer, 0, buffer.Length);
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00030976 File Offset: 0x0002EB76
		public void SetInput(byte[] buffer, int index, int count)
		{
			this.input.SetInput(buffer, index, count);
			this.totalIn += (long)count;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00030995 File Offset: 0x0002EB95
		public int Inflate(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			return this.Inflate(buffer, 0, buffer.Length);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x000309B0 File Offset: 0x0002EBB0
		public int Inflate(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "count cannot be negative");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "offset cannot be negative");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentException("count exceeds buffer bounds");
			}
			if (count == 0)
			{
				if (!this.IsFinished)
				{
					this.Decode();
				}
				return 0;
			}
			int num = 0;
			while (true)
			{
				if (this.mode != 11)
				{
					int num2 = this.outputWindow.CopyOutput(buffer, offset, count);
					if (num2 > 0)
					{
						this.adler.Update(buffer, offset, num2);
						offset += num2;
						num += num2;
						this.totalOut += (long)num2;
						count -= num2;
						if (count == 0)
						{
							break;
						}
					}
				}
				if (!this.Decode() && (this.outputWindow.GetAvailable() <= 0 || this.mode == 11))
				{
					return num;
				}
			}
			return num;
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x00030A8A File Offset: 0x0002EC8A
		public bool IsNeedingInput
		{
			get
			{
				return this.input.IsNeedingInput;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x00030A97 File Offset: 0x0002EC97
		public bool IsNeedingDictionary
		{
			get
			{
				return this.mode == 1 && this.neededBits == 0;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x00030AAD File Offset: 0x0002ECAD
		public bool IsFinished
		{
			get
			{
				return this.mode == 12 && this.outputWindow.GetAvailable() == 0;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x00030AC9 File Offset: 0x0002ECC9
		public int Adler
		{
			get
			{
				if (!this.IsNeedingDictionary)
				{
					return (int)this.adler.Value;
				}
				return this.readAdler;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x00030AE6 File Offset: 0x0002ECE6
		public long TotalOut
		{
			get
			{
				return this.totalOut;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00030AEE File Offset: 0x0002ECEE
		public long TotalIn
		{
			get
			{
				return this.totalIn - (long)this.RemainingInput;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x00030AFE File Offset: 0x0002ECFE
		public int RemainingInput
		{
			get
			{
				return this.input.AvailableBytes;
			}
		}

		// Token: 0x04000410 RID: 1040
		private const int DECODE_HEADER = 0;

		// Token: 0x04000411 RID: 1041
		private const int DECODE_DICT = 1;

		// Token: 0x04000412 RID: 1042
		private const int DECODE_BLOCKS = 2;

		// Token: 0x04000413 RID: 1043
		private const int DECODE_STORED_LEN1 = 3;

		// Token: 0x04000414 RID: 1044
		private const int DECODE_STORED_LEN2 = 4;

		// Token: 0x04000415 RID: 1045
		private const int DECODE_STORED = 5;

		// Token: 0x04000416 RID: 1046
		private const int DECODE_DYN_HEADER = 6;

		// Token: 0x04000417 RID: 1047
		private const int DECODE_HUFFMAN = 7;

		// Token: 0x04000418 RID: 1048
		private const int DECODE_HUFFMAN_LENBITS = 8;

		// Token: 0x04000419 RID: 1049
		private const int DECODE_HUFFMAN_DIST = 9;

		// Token: 0x0400041A RID: 1050
		private const int DECODE_HUFFMAN_DISTBITS = 10;

		// Token: 0x0400041B RID: 1051
		private const int DECODE_CHKSUM = 11;

		// Token: 0x0400041C RID: 1052
		private const int FINISHED = 12;

		// Token: 0x0400041D RID: 1053
		private static readonly int[] CPLENS = new int[]
		{
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			13,
			15,
			17,
			19,
			23,
			27,
			31,
			35,
			43,
			51,
			59,
			67,
			83,
			99,
			115,
			131,
			163,
			195,
			227,
			258
		};

		// Token: 0x0400041E RID: 1054
		private static readonly int[] CPLEXT = new int[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			1,
			2,
			2,
			2,
			2,
			3,
			3,
			3,
			3,
			4,
			4,
			4,
			4,
			5,
			5,
			5,
			5,
			0
		};

		// Token: 0x0400041F RID: 1055
		private static readonly int[] CPDIST = new int[]
		{
			1,
			2,
			3,
			4,
			5,
			7,
			9,
			13,
			17,
			25,
			33,
			49,
			65,
			97,
			129,
			193,
			257,
			385,
			513,
			769,
			1025,
			1537,
			2049,
			3073,
			4097,
			6145,
			8193,
			12289,
			16385,
			24577
		};

		// Token: 0x04000420 RID: 1056
		private static readonly int[] CPDEXT = new int[]
		{
			0,
			0,
			0,
			0,
			1,
			1,
			2,
			2,
			3,
			3,
			4,
			4,
			5,
			5,
			6,
			6,
			7,
			7,
			8,
			8,
			9,
			9,
			10,
			10,
			11,
			11,
			12,
			12,
			13,
			13
		};

		// Token: 0x04000421 RID: 1057
		private int mode;

		// Token: 0x04000422 RID: 1058
		private int readAdler;

		// Token: 0x04000423 RID: 1059
		private int neededBits;

		// Token: 0x04000424 RID: 1060
		private int repLength;

		// Token: 0x04000425 RID: 1061
		private int repDist;

		// Token: 0x04000426 RID: 1062
		private int uncomprLen;

		// Token: 0x04000427 RID: 1063
		private bool isLastBlock;

		// Token: 0x04000428 RID: 1064
		private long totalOut;

		// Token: 0x04000429 RID: 1065
		private long totalIn;

		// Token: 0x0400042A RID: 1066
		private bool noHeader;

		// Token: 0x0400042B RID: 1067
		private StreamManipulator input;

		// Token: 0x0400042C RID: 1068
		private OutputWindow outputWindow;

		// Token: 0x0400042D RID: 1069
		private InflaterDynHeader dynHeader;

		// Token: 0x0400042E RID: 1070
		private InflaterHuffmanTree litlenTree;

		// Token: 0x0400042F RID: 1071
		private InflaterHuffmanTree distTree;

		// Token: 0x04000430 RID: 1072
		private Adler32 adler;
	}
}
