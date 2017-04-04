using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000E7 RID: 231
	public class KeysRequiredEventArgs : EventArgs
	{
		// Token: 0x060009A4 RID: 2468 RVA: 0x00034F9C File Offset: 0x0003319C
		public KeysRequiredEventArgs(string name)
		{
			this.fileName = name;
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00034FAB File Offset: 0x000331AB
		public KeysRequiredEventArgs(string name, byte[] keyValue)
		{
			this.fileName = name;
			this.key = keyValue;
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x00034FC1 File Offset: 0x000331C1
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x00034FC9 File Offset: 0x000331C9
		// (set) Token: 0x060009A8 RID: 2472 RVA: 0x00034FD1 File Offset: 0x000331D1
		public byte[] Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		// Token: 0x04000528 RID: 1320
		private string fileName;

		// Token: 0x04000529 RID: 1321
		private byte[] key;
	}
}
