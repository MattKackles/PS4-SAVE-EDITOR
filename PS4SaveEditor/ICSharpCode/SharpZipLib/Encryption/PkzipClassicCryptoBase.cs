using System;
using ICSharpCode.SharpZipLib.Checksums;

namespace ICSharpCode.SharpZipLib.Encryption
{
	// Token: 0x020000B8 RID: 184
	internal class PkzipClassicCryptoBase
	{
		// Token: 0x060007CA RID: 1994 RVA: 0x0002D2F0 File Offset: 0x0002B4F0
		protected byte TransformByte()
		{
			uint num = (this.keys[2] & 65535u) | 2u;
			return (byte)(num * (num ^ 1u) >> 8);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0002D318 File Offset: 0x0002B518
		protected void SetKeys(byte[] keyData)
		{
			if (keyData == null)
			{
				throw new ArgumentNullException("keyData");
			}
			if (keyData.Length != 12)
			{
				throw new InvalidOperationException("Key length is not valid");
			}
			this.keys = new uint[3];
			this.keys[0] = (uint)((int)keyData[3] << 24 | (int)keyData[2] << 16 | (int)keyData[1] << 8 | (int)keyData[0]);
			this.keys[1] = (uint)((int)keyData[7] << 24 | (int)keyData[6] << 16 | (int)keyData[5] << 8 | (int)keyData[4]);
			this.keys[2] = (uint)((int)keyData[11] << 24 | (int)keyData[10] << 16 | (int)keyData[9] << 8 | (int)keyData[8]);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002D3B4 File Offset: 0x0002B5B4
		protected void UpdateKeys(byte ch)
		{
			this.keys[0] = Crc32.ComputeCrc32(this.keys[0], ch);
			this.keys[1] = this.keys[1] + (uint)((byte)this.keys[0]);
			this.keys[1] = this.keys[1] * 134775813u + 1u;
			this.keys[2] = Crc32.ComputeCrc32(this.keys[2], (byte)(this.keys[1] >> 24));
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0002D42A File Offset: 0x0002B62A
		protected void Reset()
		{
			this.keys[0] = 0u;
			this.keys[1] = 0u;
			this.keys[2] = 0u;
		}

		// Token: 0x0400038B RID: 907
		private uint[] keys;
	}
}
