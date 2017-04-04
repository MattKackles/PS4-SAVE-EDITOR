using System;
using System.IO;
using System.Security.Cryptography;
using ICSharpCode.SharpZipLib.Encryption;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
	// Token: 0x020000CA RID: 202
	public class DeflaterOutputStream : Stream
	{
		// Token: 0x0600086B RID: 2155 RVA: 0x0003149E File Offset: 0x0002F69E
		public DeflaterOutputStream(Stream baseOutputStream) : this(baseOutputStream, new Deflater(), 512)
		{
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x000314B1 File Offset: 0x0002F6B1
		public DeflaterOutputStream(Stream baseOutputStream, Deflater deflater) : this(baseOutputStream, deflater, 512)
		{
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x000314C0 File Offset: 0x0002F6C0
		public DeflaterOutputStream(Stream baseOutputStream, Deflater deflater, int bufferSize)
		{
			if (baseOutputStream == null)
			{
				throw new ArgumentNullException("baseOutputStream");
			}
			if (!baseOutputStream.CanWrite)
			{
				throw new ArgumentException("Must support writing", "baseOutputStream");
			}
			if (deflater == null)
			{
				throw new ArgumentNullException("deflater");
			}
			if (bufferSize < 512)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			this.baseOutputStream_ = baseOutputStream;
			this.buffer_ = new byte[bufferSize];
			this.deflater_ = deflater;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0003153C File Offset: 0x0002F73C
		public virtual void Finish()
		{
			this.deflater_.Finish();
			while (!this.deflater_.IsFinished)
			{
				int num = this.deflater_.Deflate(this.buffer_, 0, this.buffer_.Length);
				if (num <= 0)
				{
					break;
				}
				if (this.cryptoTransform_ != null)
				{
					this.EncryptBlock(this.buffer_, 0, num);
				}
				this.baseOutputStream_.Write(this.buffer_, 0, num);
			}
			if (!this.deflater_.IsFinished)
			{
				throw new SharpZipBaseException("Can't deflate all input?");
			}
			this.baseOutputStream_.Flush();
			if (this.cryptoTransform_ != null)
			{
				if (this.cryptoTransform_ is ZipAESTransform)
				{
					this.AESAuthCode = ((ZipAESTransform)this.cryptoTransform_).GetAuthCode();
				}
				this.cryptoTransform_.Dispose();
				this.cryptoTransform_ = null;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x0003160B File Offset: 0x0002F80B
		// (set) Token: 0x06000870 RID: 2160 RVA: 0x00031613 File Offset: 0x0002F813
		public bool IsStreamOwner
		{
			get
			{
				return this.isStreamOwner_;
			}
			set
			{
				this.isStreamOwner_ = value;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0003161C File Offset: 0x0002F81C
		public bool CanPatchEntries
		{
			get
			{
				return this.baseOutputStream_.CanSeek;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x00031629 File Offset: 0x0002F829
		// (set) Token: 0x06000873 RID: 2163 RVA: 0x00031631 File Offset: 0x0002F831
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				if (value != null && value.Length == 0)
				{
					this.password = null;
					return;
				}
				this.password = value;
			}
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0003164D File Offset: 0x0002F84D
		protected void EncryptBlock(byte[] buffer, int offset, int length)
		{
			this.cryptoTransform_.TransformBlock(buffer, 0, length, buffer, 0);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00031660 File Offset: 0x0002F860
		protected void InitializePassword(string password)
		{
			PkzipClassicManaged pkzipClassicManaged = new PkzipClassicManaged();
			byte[] rgbKey = PkzipClassic.GenerateKeys(ZipConstants.ConvertToArray(password));
			this.cryptoTransform_ = pkzipClassicManaged.CreateEncryptor(rgbKey, null);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00031690 File Offset: 0x0002F890
		protected void InitializeAESPassword(ZipEntry entry, string rawPassword, out byte[] salt, out byte[] pwdVerifier)
		{
			salt = new byte[entry.AESSaltLen];
			if (DeflaterOutputStream._aesRnd == null)
			{
				DeflaterOutputStream._aesRnd = new RNGCryptoServiceProvider();
			}
			DeflaterOutputStream._aesRnd.GetBytes(salt);
			int blockSize = entry.AESKeySize / 8;
			this.cryptoTransform_ = new ZipAESTransform(rawPassword, salt, blockSize, true);
			pwdVerifier = ((ZipAESTransform)this.cryptoTransform_).PwdVerifier;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x000316F4 File Offset: 0x0002F8F4
		protected void Deflate()
		{
			while (!this.deflater_.IsNeedingInput)
			{
				int num = this.deflater_.Deflate(this.buffer_, 0, this.buffer_.Length);
				if (num <= 0)
				{
					break;
				}
				if (this.cryptoTransform_ != null)
				{
					this.EncryptBlock(this.buffer_, 0, num);
				}
				this.baseOutputStream_.Write(this.buffer_, 0, num);
			}
			if (!this.deflater_.IsNeedingInput)
			{
				throw new SharpZipBaseException("DeflaterOutputStream can't deflate all input?");
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x00031770 File Offset: 0x0002F970
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x00031773 File Offset: 0x0002F973
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x00031776 File Offset: 0x0002F976
		public override bool CanWrite
		{
			get
			{
				return this.baseOutputStream_.CanWrite;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x00031783 File Offset: 0x0002F983
		public override long Length
		{
			get
			{
				return this.baseOutputStream_.Length;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x00031790 File Offset: 0x0002F990
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x0003179D File Offset: 0x0002F99D
		public override long Position
		{
			get
			{
				return this.baseOutputStream_.Position;
			}
			set
			{
				throw new NotSupportedException("Position property not supported");
			}
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x000317A9 File Offset: 0x0002F9A9
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("DeflaterOutputStream Seek not supported");
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x000317B5 File Offset: 0x0002F9B5
		public override void SetLength(long value)
		{
			throw new NotSupportedException("DeflaterOutputStream SetLength not supported");
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x000317C1 File Offset: 0x0002F9C1
		public override int ReadByte()
		{
			throw new NotSupportedException("DeflaterOutputStream ReadByte not supported");
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x000317CD File Offset: 0x0002F9CD
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("DeflaterOutputStream Read not supported");
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x000317D9 File Offset: 0x0002F9D9
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException("DeflaterOutputStream BeginRead not currently supported");
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x000317E5 File Offset: 0x0002F9E5
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException("BeginWrite is not supported");
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x000317F1 File Offset: 0x0002F9F1
		public override void Flush()
		{
			this.deflater_.Flush();
			this.Deflate();
			this.baseOutputStream_.Flush();
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00031810 File Offset: 0x0002FA10
		public override void Close()
		{
			if (!this.isClosed_)
			{
				this.isClosed_ = true;
				try
				{
					this.Finish();
					if (this.cryptoTransform_ != null)
					{
						this.GetAuthCodeIfAES();
						this.cryptoTransform_.Dispose();
						this.cryptoTransform_ = null;
					}
				}
				finally
				{
					if (this.isStreamOwner_)
					{
						this.baseOutputStream_.Close();
					}
				}
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00031878 File Offset: 0x0002FA78
		private void GetAuthCodeIfAES()
		{
			if (this.cryptoTransform_ is ZipAESTransform)
			{
				this.AESAuthCode = ((ZipAESTransform)this.cryptoTransform_).GetAuthCode();
			}
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x000318A0 File Offset: 0x0002FAA0
		public override void WriteByte(byte value)
		{
			this.Write(new byte[]
			{
				value
			}, 0, 1);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x000318C1 File Offset: 0x0002FAC1
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.deflater_.SetInput(buffer, offset, count);
			this.Deflate();
		}

		// Token: 0x04000449 RID: 1097
		private string password;

		// Token: 0x0400044A RID: 1098
		private ICryptoTransform cryptoTransform_;

		// Token: 0x0400044B RID: 1099
		protected byte[] AESAuthCode;

		// Token: 0x0400044C RID: 1100
		private byte[] buffer_;

		// Token: 0x0400044D RID: 1101
		protected Deflater deflater_;

		// Token: 0x0400044E RID: 1102
		protected Stream baseOutputStream_;

		// Token: 0x0400044F RID: 1103
		private bool isClosed_;

		// Token: 0x04000450 RID: 1104
		private bool isStreamOwner_ = true;

		// Token: 0x04000451 RID: 1105
		private static RNGCryptoServiceProvider _aesRnd;
	}
}
