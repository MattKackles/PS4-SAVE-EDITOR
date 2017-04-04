using System;

namespace Be.Windows.Forms
{
	// Token: 0x02000044 RID: 68
	public class DefaultByteCharConverter : IByteCharConverter
	{
		// Token: 0x06000304 RID: 772 RVA: 0x0001186D File Offset: 0x0000FA6D
		public char ToChar(byte b)
		{
			if (b <= 31 || (b > 126 && b < 160))
			{
				return '.';
			}
			return (char)b;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00011885 File Offset: 0x0000FA85
		public byte ToByte(char c)
		{
			return (byte)c;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00011889 File Offset: 0x0000FA89
		public override string ToString()
		{
			return "Default";
		}
	}
}
