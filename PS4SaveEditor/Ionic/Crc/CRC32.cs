using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ionic.Crc
{
	// Token: 0x02000112 RID: 274
	[ClassInterface(ClassInterfaceType.AutoDispatch), ComVisible(true), Guid("ebc25cf6-9120-4283-b972-0e5520d0000C")]
	public class CRC32
	{
		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0003FA81 File Offset: 0x0003DC81
		public long TotalBytesRead
		{
			get
			{
				return this._TotalBytesRead;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x0003FA89 File Offset: 0x0003DC89
		public int Crc32Result
		{
			get
			{
				return (int)(~(int)this._register);
			}
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0003FA92 File Offset: 0x0003DC92
		public int GetCrc32(Stream input)
		{
			return this.GetCrc32AndCopy(input, null);
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0003FA9C File Offset: 0x0003DC9C
		public int GetCrc32AndCopy(Stream input, Stream output)
		{
			if (input == null)
			{
				throw new Exception("The input stream must not be null.");
			}
			byte[] array = new byte[8192];
			int count = 8192;
			this._TotalBytesRead = 0L;
			int i = input.Read(array, 0, count);
			if (output != null)
			{
				output.Write(array, 0, i);
			}
			this._TotalBytesRead += (long)i;
			while (i > 0)
			{
				this.SlurpBlock(array, 0, i);
				i = input.Read(array, 0, count);
				if (output != null)
				{
					output.Write(array, 0, i);
				}
				this._TotalBytesRead += (long)i;
			}
			return (int)(~(int)this._register);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0003FB30 File Offset: 0x0003DD30
		public int ComputeCrc32(int W, byte B)
		{
			return this._InternalComputeCrc32((uint)W, B);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0003FB3A File Offset: 0x0003DD3A
		internal int _InternalComputeCrc32(uint W, byte B)
		{
			return (int)(this.crc32Table[(int)((UIntPtr)((W ^ (uint)B) & 255u))] ^ W >> 8);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0003FB54 File Offset: 0x0003DD54
		public void SlurpBlock(byte[] block, int offset, int count)
		{
			if (block == null)
			{
				throw new Exception("The data buffer must not be null.");
			}
			for (int i = 0; i < count; i++)
			{
				int num = offset + i;
				byte b = block[num];
				if (this.reverseBits)
				{
					uint num2 = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)((UIntPtr)num2)]);
				}
				else
				{
					uint num3 = (this._register & 255u) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)((UIntPtr)num3)]);
				}
			}
			this._TotalBytesRead += (long)count;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0003FBEC File Offset: 0x0003DDEC
		public void UpdateCRC(byte b)
		{
			if (this.reverseBits)
			{
				uint num = this._register >> 24 ^ (uint)b;
				this._register = (this._register << 8 ^ this.crc32Table[(int)((UIntPtr)num)]);
				return;
			}
			uint num2 = (this._register & 255u) ^ (uint)b;
			this._register = (this._register >> 8 ^ this.crc32Table[(int)((UIntPtr)num2)]);
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0003FC50 File Offset: 0x0003DE50
		public void UpdateCRC(byte b, int n)
		{
			while (n-- > 0)
			{
				if (this.reverseBits)
				{
					uint num = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)((UIntPtr)((num >= 0u) ? num : (num + 256u)))]);
				}
				else
				{
					uint num2 = (this._register & 255u) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)((UIntPtr)((num2 >= 0u) ? num2 : (num2 + 256u)))]);
				}
			}
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0003FCD8 File Offset: 0x0003DED8
		private static uint ReverseBits(uint data)
		{
			uint num = (data & 1431655765u) << 1 | (data >> 1 & 1431655765u);
			num = ((num & 858993459u) << 2 | (num >> 2 & 858993459u));
			num = ((num & 252645135u) << 4 | (num >> 4 & 252645135u));
			return num << 24 | (num & 65280u) << 8 | (num >> 8 & 65280u) | num >> 24;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0003FD44 File Offset: 0x0003DF44
		private static byte ReverseBits(byte data)
		{
			uint num = (uint)data * 131586u;
			uint num2 = 17055760u;
			uint num3 = num & num2;
			uint num4 = num << 2 & num2 << 1;
			return (byte)(16781313u * (num3 + num4) >> 24);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0003FD78 File Offset: 0x0003DF78
		private void GenerateLookupTable()
		{
			this.crc32Table = new uint[256];
			byte b = 0;
			do
			{
				uint num = (uint)b;
				for (byte b2 = 8; b2 > 0; b2 -= 1)
				{
					if ((num & 1u) == 1u)
					{
						num = (num >> 1 ^ this.dwPolynomial);
					}
					else
					{
						num >>= 1;
					}
				}
				if (this.reverseBits)
				{
					this.crc32Table[(int)CRC32.ReverseBits(b)] = CRC32.ReverseBits(num);
				}
				else
				{
					this.crc32Table[(int)b] = num;
				}
				b += 1;
			}
			while (b != 0);
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0003FDEC File Offset: 0x0003DFEC
		private uint gf2_matrix_times(uint[] matrix, uint vec)
		{
			uint num = 0u;
			int num2 = 0;
			while (vec != 0u)
			{
				if ((vec & 1u) == 1u)
				{
					num ^= matrix[num2];
				}
				vec >>= 1;
				num2++;
			}
			return num;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0003FE18 File Offset: 0x0003E018
		private void gf2_matrix_square(uint[] square, uint[] mat)
		{
			for (int i = 0; i < 32; i++)
			{
				square[i] = this.gf2_matrix_times(mat, mat[i]);
			}
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0003FE40 File Offset: 0x0003E040
		public void Combine(int crc, int length)
		{
			uint[] array = new uint[32];
			uint[] array2 = new uint[32];
			if (length == 0)
			{
				return;
			}
			uint num = ~this._register;
			array2[0] = this.dwPolynomial;
			uint num2 = 1u;
			for (int i = 1; i < 32; i++)
			{
				array2[i] = num2;
				num2 <<= 1;
			}
			this.gf2_matrix_square(array, array2);
			this.gf2_matrix_square(array2, array);
			uint num3 = (uint)length;
			do
			{
				this.gf2_matrix_square(array, array2);
				if ((num3 & 1u) == 1u)
				{
					num = this.gf2_matrix_times(array, num);
				}
				num3 >>= 1;
				if (num3 == 0u)
				{
					break;
				}
				this.gf2_matrix_square(array2, array);
				if ((num3 & 1u) == 1u)
				{
					num = this.gf2_matrix_times(array2, num);
				}
				num3 >>= 1;
			}
			while (num3 != 0u);
			num ^= (uint)crc;
			this._register = ~num;
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0003FEF7 File Offset: 0x0003E0F7
		public CRC32() : this(false)
		{
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0003FF00 File Offset: 0x0003E100
		public CRC32(bool reverseBits) : this(-306674912, reverseBits)
		{
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0003FF0E File Offset: 0x0003E10E
		public CRC32(int polynomial, bool reverseBits)
		{
			this.reverseBits = reverseBits;
			this.dwPolynomial = (uint)polynomial;
			this.GenerateLookupTable();
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0003FF31 File Offset: 0x0003E131
		public void Reset()
		{
			this._register = 4294967295u;
		}

		// Token: 0x040005C7 RID: 1479
		private const int BUFFER_SIZE = 8192;

		// Token: 0x040005C8 RID: 1480
		private uint dwPolynomial;

		// Token: 0x040005C9 RID: 1481
		private long _TotalBytesRead;

		// Token: 0x040005CA RID: 1482
		private bool reverseBits;

		// Token: 0x040005CB RID: 1483
		private uint[] crc32Table;

		// Token: 0x040005CC RID: 1484
		private uint _register = 4294967295u;
	}
}
