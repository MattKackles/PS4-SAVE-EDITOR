using System;
using System.IO;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Encryption
{
	// Token: 0x020000BC RID: 188
	internal class ZipAESStream : CryptoStream
	{
		// Token: 0x060007EA RID: 2026 RVA: 0x0002D660 File Offset: 0x0002B860
		public ZipAESStream(Stream stream, ZipAESTransform transform, CryptoStreamMode mode) : base(stream, transform, mode)
		{
			this._stream = stream;
			this._transform = transform;
			this._slideBuffer = new byte[1024];
			this._blockAndAuth = 26;
			if (mode != CryptoStreamMode.Read)
			{
				throw new Exception("ZipAESStream only for read");
			}
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0002D6A0 File Offset: 0x0002B8A0
		public override int Read(byte[] outBuffer, int offset, int count)
		{
			int i = 0;
			while (i < count)
			{
				int num = this._slideBufFreePos - this._slideBufStartPos;
				int num2 = this._blockAndAuth - num;
				if (this._slideBuffer.Length - this._slideBufFreePos < num2)
				{
					int num3 = 0;
					int j = this._slideBufStartPos;
					while (j < this._slideBufFreePos)
					{
						this._slideBuffer[num3] = this._slideBuffer[j];
						j++;
						num3++;
					}
					this._slideBufFreePos -= this._slideBufStartPos;
					this._slideBufStartPos = 0;
				}
				int num4 = this._stream.Read(this._slideBuffer, this._slideBufFreePos, num2);
				this._slideBufFreePos += num4;
				num = this._slideBufFreePos - this._slideBufStartPos;
				if (num < this._blockAndAuth)
				{
					if (num > 10)
					{
						int num5 = num - 10;
						this._transform.TransformBlock(this._slideBuffer, this._slideBufStartPos, num5, outBuffer, offset);
						i += num5;
						this._slideBufStartPos += num5;
					}
					else if (num < 10)
					{
						throw new Exception("Internal error missed auth code");
					}
					byte[] authCode = this._transform.GetAuthCode();
					for (int k = 0; k < 10; k++)
					{
						if (authCode[k] != this._slideBuffer[this._slideBufStartPos + k])
						{
							throw new Exception("AES Authentication Code does not match. This is a super-CRC check on the data in the file after compression and encryption. \r\nThe file may be damaged.");
						}
					}
					break;
				}
				this._transform.TransformBlock(this._slideBuffer, this._slideBufStartPos, 16, outBuffer, offset);
				i += 16;
				offset += 16;
				this._slideBufStartPos += 16;
			}
			return i;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0002D83A File Offset: 0x0002BA3A
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400038D RID: 909
		private const int AUTH_CODE_LENGTH = 10;

		// Token: 0x0400038E RID: 910
		private const int CRYPTO_BLOCK_SIZE = 16;

		// Token: 0x0400038F RID: 911
		private Stream _stream;

		// Token: 0x04000390 RID: 912
		private ZipAESTransform _transform;

		// Token: 0x04000391 RID: 913
		private byte[] _slideBuffer;

		// Token: 0x04000392 RID: 914
		private int _slideBufStartPos;

		// Token: 0x04000393 RID: 915
		private int _slideBufFreePos;

		// Token: 0x04000394 RID: 916
		private int _blockAndAuth;
	}
}
