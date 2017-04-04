using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x0200013B RID: 315
	public class GZipStream : Stream
	{
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x00044960 File Offset: 0x00042B60
		// (set) Token: 0x06000C7A RID: 3194 RVA: 0x00044968 File Offset: 0x00042B68
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._Comment = value;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x00044984 File Offset: 0x00042B84
		// (set) Token: 0x06000C7C RID: 3196 RVA: 0x0004498C File Offset: 0x00042B8C
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._FileName = value;
				if (this._FileName == null)
				{
					return;
				}
				if (this._FileName.IndexOf("/") != -1)
				{
					this._FileName = this._FileName.Replace("/", "\\");
				}
				if (this._FileName.EndsWith("\\"))
				{
					throw new Exception("Illegal filename");
				}
				if (this._FileName.IndexOf("\\") != -1)
				{
					this._FileName = Path.GetFileName(this._FileName);
				}
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x00044A2B File Offset: 0x00042C2B
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00044A33 File Offset: 0x00042C33
		public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00044A3F File Offset: 0x00042C3F
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00044A4B File Offset: 0x00042C4B
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00044A57 File Offset: 0x00042C57
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x00044A74 File Offset: 0x00042C74
		// (set) Token: 0x06000C83 RID: 3203 RVA: 0x00044A81 File Offset: 0x00042C81
		public virtual FlushType FlushMode
		{
			get
			{
				return this._baseStream._flushMode;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x00044AA2 File Offset: 0x00042CA2
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x00044AB0 File Offset: 0x00042CB0
		public int BufferSize
		{
			get
			{
				return this._baseStream._bufferSize;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				if (this._baseStream._workingBuffer != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this._baseStream._bufferSize = value;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x00044B1C File Offset: 0x00042D1C
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x00044B2E File Offset: 0x00042D2E
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00044B40 File Offset: 0x00042D40
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
						this._Crc32 = this._baseStream.Crc32;
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x00044BA0 File Offset: 0x00042DA0
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x00044BC5 File Offset: 0x00042DC5
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x00044BC8 File Offset: 0x00042DC8
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00044BED File Offset: 0x00042DED
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x00044C0D File Offset: 0x00042E0D
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x00044C14 File Offset: 0x00042E14
		// (set) Token: 0x06000C8F RID: 3215 RVA: 0x00044C75 File Offset: 0x00042E75
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut + (long)this._headerByteCount;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn + (long)this._baseStream._gzipHeaderByteCount;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00044C7C File Offset: 0x00042E7C
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = this._baseStream.Read(buffer, offset, count);
			if (!this._firstReadDone)
			{
				this._firstReadDone = true;
				this.FileName = this._baseStream._GzipFileName;
				this.Comment = this._baseStream._GzipComment;
			}
			return result;
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00044CDD File Offset: 0x00042EDD
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00044CE4 File Offset: 0x00042EE4
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x00044CEC File Offset: 0x00042EEC
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._baseStream._wantCompress)
				{
					throw new InvalidOperationException();
				}
				this._headerByteCount = this.EmitHeader();
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x00044D4C File Offset: 0x00042F4C
		private int EmitHeader()
		{
			byte[] array = (this.Comment == null) ? null : GZipStream.iso8859dash1.GetBytes(this.Comment);
			byte[] array2 = (this.FileName == null) ? null : GZipStream.iso8859dash1.GetBytes(this.FileName);
			int num = (this.Comment == null) ? 0 : (array.Length + 1);
			int num2 = (this.FileName == null) ? 0 : (array2.Length + 1);
			int num3 = 10 + num + num2;
			byte[] array3 = new byte[num3];
			int num4 = 0;
			array3[num4++] = 31;
			array3[num4++] = 139;
			array3[num4++] = 8;
			byte b = 0;
			if (this.Comment != null)
			{
				b ^= 16;
			}
			if (this.FileName != null)
			{
				b ^= 8;
			}
			array3[num4++] = b;
			if (!this.LastModified.HasValue)
			{
				this.LastModified = new DateTime?(DateTime.Now);
			}
			int value = (int)(this.LastModified.Value - GZipStream._unixEpoch).TotalSeconds;
			Array.Copy(BitConverter.GetBytes(value), 0, array3, num4, 4);
			num4 += 4;
			array3[num4++] = 0;
			array3[num4++] = 255;
			if (num2 != 0)
			{
				Array.Copy(array2, 0, array3, num4, num2 - 1);
				num4 += num2 - 1;
				array3[num4++] = 0;
			}
			if (num != 0)
			{
				Array.Copy(array, 0, array3, num4, num - 1);
				num4 += num - 1;
				array3[num4++] = 0;
			}
			this._baseStream._stream.Write(array3, 0, array3.Length);
			return array3.Length;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00044EF0 File Offset: 0x000430F0
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x00044F38 File Offset: 0x00043138
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x00044F80 File Offset: 0x00043180
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x00044FC4 File Offset: 0x000431C4
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x04000688 RID: 1672
		public DateTime? LastModified;

		// Token: 0x04000689 RID: 1673
		private int _headerByteCount;

		// Token: 0x0400068A RID: 1674
		internal ZlibBaseStream _baseStream;

		// Token: 0x0400068B RID: 1675
		private bool _disposed;

		// Token: 0x0400068C RID: 1676
		private bool _firstReadDone;

		// Token: 0x0400068D RID: 1677
		private string _FileName;

		// Token: 0x0400068E RID: 1678
		private string _Comment;

		// Token: 0x0400068F RID: 1679
		private int _Crc32;

		// Token: 0x04000690 RID: 1680
		internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04000691 RID: 1681
		internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");
	}
}
