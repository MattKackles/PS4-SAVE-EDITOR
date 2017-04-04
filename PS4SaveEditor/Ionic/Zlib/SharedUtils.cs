using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x02000168 RID: 360
	internal class SharedUtils
	{
		// Token: 0x06000F5C RID: 3932 RVA: 0x00057FF5 File Offset: 0x000561F5
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00058000 File Offset: 0x00056200
		public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			char[] array = new char[target.Length];
			int num = sourceTextReader.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (byte)array[i];
			}
			return num;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x00058042 File Offset: 0x00056242
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0005804F File Offset: 0x0005624F
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
