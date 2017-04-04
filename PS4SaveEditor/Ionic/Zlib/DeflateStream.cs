using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x02000119 RID: 281
	public class DeflateStream : Stream
	{
		// Token: 0x06000BC6 RID: 3014 RVA: 0x000426F2 File Offset: 0x000408F2
		public DeflateStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x000426FE File Offset: 0x000408FE
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0004270A File Offset: 0x0004090A
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x00042716 File Offset: 0x00040916
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0004273A File Offset: 0x0004093A
		// (set) Token: 0x06000BCB RID: 3019 RVA: 0x00042747 File Offset: 0x00040947
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
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000BCC RID: 3020 RVA: 0x00042768 File Offset: 0x00040968
		// (set) Token: 0x06000BCD RID: 3021 RVA: 0x00042778 File Offset: 0x00040978
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
					throw new ObjectDisposedException("DeflateStream");
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

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000BCE RID: 3022 RVA: 0x000427E4 File Offset: 0x000409E4
		// (set) Token: 0x06000BCF RID: 3023 RVA: 0x000427F1 File Offset: 0x000409F1
		public CompressionStrategy Strategy
		{
			get
			{
				return this._baseStream.Strategy;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream.Strategy = value;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x00042812 File Offset: 0x00040A12
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x00042824 File Offset: 0x00040A24
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00042838 File Offset: 0x00040A38
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x00042884 File Offset: 0x00040A84
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000BD4 RID: 3028 RVA: 0x000428A9 File Offset: 0x00040AA9
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x000428AC File Offset: 0x00040AAC
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x000428D1 File Offset: 0x00040AD1
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x000428F1 File Offset: 0x00040AF1
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x000428F8 File Offset: 0x00040AF8
		// (set) Token: 0x06000BD9 RID: 3033 RVA: 0x00042944 File Offset: 0x00040B44
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x0004294B File Offset: 0x00040B4B
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x0004296E File Offset: 0x00040B6E
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00042975 File Offset: 0x00040B75
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x0004297C File Offset: 0x00040B7C
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x000429A0 File Offset: 0x00040BA0
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x000429E8 File Offset: 0x00040BE8
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00042A30 File Offset: 0x00040C30
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00042A74 File Offset: 0x00040C74
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0400062B RID: 1579
		internal ZlibBaseStream _baseStream;

		// Token: 0x0400062C RID: 1580
		internal Stream _innerStream;

		// Token: 0x0400062D RID: 1581
		private bool _disposed;
	}
}
