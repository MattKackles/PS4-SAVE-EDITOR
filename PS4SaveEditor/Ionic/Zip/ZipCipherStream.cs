using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x0200014D RID: 333
	internal class ZipCipherStream : Stream
	{
		// Token: 0x06000D22 RID: 3362 RVA: 0x0004C3C6 File Offset: 0x0004A5C6
		public ZipCipherStream(Stream s, ZipCrypto cipher, CryptoMode mode)
		{
			this._cipher = cipher;
			this._s = s;
			this._mode = mode;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0004C3E4 File Offset: 0x0004A5E4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._mode == CryptoMode.Encrypt)
			{
				throw new NotSupportedException("This stream does not encrypt via Read()");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			byte[] array = new byte[count];
			int num = this._s.Read(array, 0, count);
			byte[] array2 = this._cipher.DecryptMessage(array, num);
			for (int i = 0; i < num; i++)
			{
				buffer[offset + i] = array2[i];
			}
			return num;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0004C44C File Offset: 0x0004A64C
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._mode == CryptoMode.Decrypt)
			{
				throw new NotSupportedException("This stream does not Decrypt via Write()");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count == 0)
			{
				return;
			}
			byte[] array;
			if (offset != 0)
			{
				array = new byte[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = buffer[offset + i];
				}
			}
			else
			{
				array = buffer;
			}
			byte[] array2 = this._cipher.EncryptMessage(array, count);
			this._s.Write(array2, 0, array2.Length);
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x0004C4C1 File Offset: 0x0004A6C1
		public override bool CanRead
		{
			get
			{
				return this._mode == CryptoMode.Decrypt;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000D26 RID: 3366 RVA: 0x0004C4CC File Offset: 0x0004A6CC
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x0004C4CF File Offset: 0x0004A6CF
		public override bool CanWrite
		{
			get
			{
				return this._mode == CryptoMode.Encrypt;
			}
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0004C4DA File Offset: 0x0004A6DA
		public override void Flush()
		{
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x0004C4DC File Offset: 0x0004A6DC
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0004C4E3 File Offset: 0x0004A6E3
		// (set) Token: 0x06000D2B RID: 3371 RVA: 0x0004C4EA File Offset: 0x0004A6EA
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0004C4F1 File Offset: 0x0004A6F1
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0004C4F8 File Offset: 0x0004A6F8
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400075F RID: 1887
		private ZipCrypto _cipher;

		// Token: 0x04000760 RID: 1888
		private Stream _s;

		// Token: 0x04000761 RID: 1889
		private CryptoMode _mode;
	}
}
