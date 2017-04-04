using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
	// Token: 0x020000CD RID: 205
	public class OutputWindow
	{
		// Token: 0x060008B2 RID: 2226 RVA: 0x00031F14 File Offset: 0x00030114
		public void Write(int value)
		{
			if (this.windowFilled++ == 32768)
			{
				throw new InvalidOperationException("Window full");
			}
			this.window[this.windowEnd++] = (byte)value;
			this.windowEnd &= 32767;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00031F70 File Offset: 0x00030170
		private void SlowRepeat(int repStart, int length, int distance)
		{
			while (length-- > 0)
			{
				this.window[this.windowEnd++] = this.window[repStart++];
				this.windowEnd &= 32767;
				repStart &= 32767;
			}
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00031FC8 File Offset: 0x000301C8
		public void Repeat(int length, int distance)
		{
			if ((this.windowFilled += length) > 32768)
			{
				throw new InvalidOperationException("Window full");
			}
			int num = this.windowEnd - distance & 32767;
			int num2 = 32768 - length;
			if (num > num2 || this.windowEnd >= num2)
			{
				this.SlowRepeat(num, length, distance);
				return;
			}
			if (length <= distance)
			{
				Array.Copy(this.window, num, this.window, this.windowEnd, length);
				this.windowEnd += length;
				return;
			}
			while (length-- > 0)
			{
				this.window[this.windowEnd++] = this.window[num++];
			}
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00032080 File Offset: 0x00030280
		public int CopyStored(StreamManipulator input, int length)
		{
			length = Math.Min(Math.Min(length, 32768 - this.windowFilled), input.AvailableBytes);
			int num = 32768 - this.windowEnd;
			int num2;
			if (length > num)
			{
				num2 = input.CopyBytes(this.window, this.windowEnd, num);
				if (num2 == num)
				{
					num2 += input.CopyBytes(this.window, 0, length - num);
				}
			}
			else
			{
				num2 = input.CopyBytes(this.window, this.windowEnd, length);
			}
			this.windowEnd = (this.windowEnd + num2 & 32767);
			this.windowFilled += num2;
			return num2;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00032124 File Offset: 0x00030324
		public void CopyDict(byte[] dictionary, int offset, int length)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			if (this.windowFilled > 0)
			{
				throw new InvalidOperationException();
			}
			if (length > 32768)
			{
				offset += length - 32768;
				length = 32768;
			}
			Array.Copy(dictionary, offset, this.window, 0, length);
			this.windowEnd = (length & 32767);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00032184 File Offset: 0x00030384
		public int GetFreeSpace()
		{
			return 32768 - this.windowFilled;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00032192 File Offset: 0x00030392
		public int GetAvailable()
		{
			return this.windowFilled;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0003219C File Offset: 0x0003039C
		public int CopyOutput(byte[] output, int offset, int len)
		{
			int num = this.windowEnd;
			if (len > this.windowFilled)
			{
				len = this.windowFilled;
			}
			else
			{
				num = (this.windowEnd - this.windowFilled + len & 32767);
			}
			int num2 = len;
			int num3 = len - num;
			if (num3 > 0)
			{
				Array.Copy(this.window, 32768 - num3, output, offset, num3);
				offset += num3;
				len = num;
			}
			Array.Copy(this.window, num - len, output, offset, len);
			this.windowFilled -= num2;
			if (this.windowFilled < 0)
			{
				throw new InvalidOperationException();
			}
			return num2;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00032230 File Offset: 0x00030430
		public void Reset()
		{
			this.windowFilled = (this.windowEnd = 0);
		}

		// Token: 0x04000460 RID: 1120
		private const int WindowSize = 32768;

		// Token: 0x04000461 RID: 1121
		private const int WindowMask = 32767;

		// Token: 0x04000462 RID: 1122
		private byte[] window = new byte[32768];

		// Token: 0x04000463 RID: 1123
		private int windowEnd;

		// Token: 0x04000464 RID: 1124
		private int windowFilled;
	}
}
