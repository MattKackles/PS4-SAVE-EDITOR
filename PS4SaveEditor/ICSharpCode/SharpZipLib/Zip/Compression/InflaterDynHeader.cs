using System;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	// Token: 0x020000C8 RID: 200
	internal class InflaterDynHeader
	{
		// Token: 0x06000863 RID: 2147 RVA: 0x00030D64 File Offset: 0x0002EF64
		public bool Decode(StreamManipulator input)
		{
			while (true)
			{
				switch (this.mode)
				{
				case 0:
					this.lnum = input.PeekBits(5);
					if (this.lnum < 0)
					{
						return false;
					}
					this.lnum += 257;
					input.DropBits(5);
					this.mode = 1;
					goto IL_61;
				case 1:
					goto IL_61;
				case 2:
					goto IL_B9;
				case 3:
					break;
				case 4:
					goto IL_1A8;
				case 5:
					goto IL_1EE;
				default:
					continue;
				}
				IL_13B:
				while (this.ptr < this.blnum)
				{
					int num = input.PeekBits(3);
					if (num < 0)
					{
						return false;
					}
					input.DropBits(3);
					this.blLens[InflaterDynHeader.BL_ORDER[this.ptr]] = (byte)num;
					this.ptr++;
				}
				this.blTree = new InflaterHuffmanTree(this.blLens);
				this.blLens = null;
				this.ptr = 0;
				this.mode = 4;
				IL_1A8:
				int symbol;
				while (((symbol = this.blTree.GetSymbol(input)) & -16) == 0)
				{
					this.litdistLens[this.ptr++] = (this.lastLen = (byte)symbol);
					if (this.ptr == this.num)
					{
						return true;
					}
				}
				if (symbol < 0)
				{
					return false;
				}
				if (symbol >= 17)
				{
					this.lastLen = 0;
				}
				else if (this.ptr == 0)
				{
					goto Block_10;
				}
				this.repSymbol = symbol - 16;
				this.mode = 5;
				IL_1EE:
				int bitCount = InflaterDynHeader.repBits[this.repSymbol];
				int num2 = input.PeekBits(bitCount);
				if (num2 < 0)
				{
					return false;
				}
				input.DropBits(bitCount);
				num2 += InflaterDynHeader.repMin[this.repSymbol];
				if (this.ptr + num2 > this.num)
				{
					goto Block_12;
				}
				while (num2-- > 0)
				{
					this.litdistLens[this.ptr++] = this.lastLen;
				}
				if (this.ptr == this.num)
				{
					return true;
				}
				this.mode = 4;
				continue;
				IL_B9:
				this.blnum = input.PeekBits(4);
				if (this.blnum < 0)
				{
					return false;
				}
				this.blnum += 4;
				input.DropBits(4);
				this.blLens = new byte[19];
				this.ptr = 0;
				this.mode = 3;
				goto IL_13B;
				IL_61:
				this.dnum = input.PeekBits(5);
				if (this.dnum < 0)
				{
					return false;
				}
				this.dnum++;
				input.DropBits(5);
				this.num = this.lnum + this.dnum;
				this.litdistLens = new byte[this.num];
				this.mode = 2;
				goto IL_B9;
			}
			return false;
			Block_10:
			throw new SharpZipBaseException();
			Block_12:
			throw new SharpZipBaseException();
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00030FEC File Offset: 0x0002F1EC
		public InflaterHuffmanTree BuildLitLenTree()
		{
			byte[] array = new byte[this.lnum];
			Array.Copy(this.litdistLens, 0, array, 0, this.lnum);
			return new InflaterHuffmanTree(array);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00031020 File Offset: 0x0002F220
		public InflaterHuffmanTree BuildDistTree()
		{
			byte[] array = new byte[this.dnum];
			Array.Copy(this.litdistLens, this.lnum, array, 0, this.dnum);
			return new InflaterHuffmanTree(array);
		}

		// Token: 0x04000431 RID: 1073
		private const int LNUM = 0;

		// Token: 0x04000432 RID: 1074
		private const int DNUM = 1;

		// Token: 0x04000433 RID: 1075
		private const int BLNUM = 2;

		// Token: 0x04000434 RID: 1076
		private const int BLLENS = 3;

		// Token: 0x04000435 RID: 1077
		private const int LENS = 4;

		// Token: 0x04000436 RID: 1078
		private const int REPS = 5;

		// Token: 0x04000437 RID: 1079
		private static readonly int[] repMin = new int[]
		{
			3,
			3,
			11
		};

		// Token: 0x04000438 RID: 1080
		private static readonly int[] repBits = new int[]
		{
			2,
			3,
			7
		};

		// Token: 0x04000439 RID: 1081
		private static readonly int[] BL_ORDER = new int[]
		{
			16,
			17,
			18,
			0,
			8,
			7,
			9,
			6,
			10,
			5,
			11,
			4,
			12,
			3,
			13,
			2,
			14,
			1,
			15
		};

		// Token: 0x0400043A RID: 1082
		private byte[] blLens;

		// Token: 0x0400043B RID: 1083
		private byte[] litdistLens;

		// Token: 0x0400043C RID: 1084
		private InflaterHuffmanTree blTree;

		// Token: 0x0400043D RID: 1085
		private int mode;

		// Token: 0x0400043E RID: 1086
		private int lnum;

		// Token: 0x0400043F RID: 1087
		private int dnum;

		// Token: 0x04000440 RID: 1088
		private int blnum;

		// Token: 0x04000441 RID: 1089
		private int num;

		// Token: 0x04000442 RID: 1090
		private int repSymbol;

		// Token: 0x04000443 RID: 1091
		private byte lastLen;

		// Token: 0x04000444 RID: 1092
		private int ptr;
	}
}
