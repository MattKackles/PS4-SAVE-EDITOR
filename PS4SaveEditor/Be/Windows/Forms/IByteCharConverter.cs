using System;

namespace Be.Windows.Forms
{
	// Token: 0x02000043 RID: 67
	public interface IByteCharConverter
	{
		// Token: 0x06000302 RID: 770
		char ToChar(byte b);

		// Token: 0x06000303 RID: 771
		byte ToByte(char c);
	}
}
