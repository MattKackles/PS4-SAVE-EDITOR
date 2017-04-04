using System;
using System.Text;

namespace Be.Windows.Forms
{
	// Token: 0x02000045 RID: 69
	public class EbcdicByteCharProvider : IByteCharConverter
	{
		// Token: 0x06000308 RID: 776 RVA: 0x00011898 File Offset: 0x0000FA98
		public char ToChar(byte b)
		{
			string @string = this._ebcdicEncoding.GetString(new byte[]
			{
				b
			});
			if (@string.Length <= 0)
			{
				return '.';
			}
			return @string[0];
		}

		// Token: 0x06000309 RID: 777 RVA: 0x000118D0 File Offset: 0x0000FAD0
		public byte ToByte(char c)
		{
			byte[] bytes = this._ebcdicEncoding.GetBytes(new char[]
			{
				c
			});
			if (bytes.Length <= 0)
			{
				return 0;
			}
			return bytes[0];
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00011900 File Offset: 0x0000FB00
		public override string ToString()
		{
			return "EBCDIC (Code Page 500)";
		}

		// Token: 0x0400019B RID: 411
		private Encoding _ebcdicEncoding = Encoding.GetEncoding(500);
	}
}
