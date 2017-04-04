using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x02000171 RID: 369
	public class ZlibStream : Stream
	{
		// Token: 0x06000F97 RID: 3991 RVA: 0x0005996E File Offset: 0x00057B6E
		public ZlibStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0005997A File Offset: 0x00057B7A
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00059986 File Offset: 0x00057B86
		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00059992 File Offset: 0x00057B92
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000F9B RID: 3995 RVA: 0x000599AF File Offset: 0x00057BAF
		// (set) Token: 0x06000F9C RID: 3996 RVA: 0x000599BC File Offset: 0x00057BBC
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
					throw new ObjectDisposedException("ZlibStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000F9D RID: 3997 RVA: 0x000599DD File Offset: 0x00057BDD
		// (set) Token: 0x06000F9E RID: 3998 RVA: 0x000599EC File Offset: 0x00057BEC
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
					throw new ObjectDisposedException("ZlibStream");
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

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000F9F RID: 3999 RVA: 0x00059A58 File Offset: 0x00057C58
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x00059A6A File Offset: 0x00057C6A
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00059A7C File Offset: 0x00057C7C
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

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x00059AC8 File Offset: 0x00057CC8
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x00059AED File Offset: 0x00057CED
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x00059AF0 File Offset: 0x00057CF0
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00059B15 File Offset: 0x00057D15
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x00059B35 File Offset: 0x00057D35
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x00059B3C File Offset: 0x00057D3C
		// (set) Token: 0x06000FA8 RID: 4008 RVA: 0x00059B88 File Offset: 0x00057D88
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
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00059B8F File Offset: 0x00057D8F
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00059BB2 File Offset: 0x00057DB2
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00059BB9 File Offset: 0x00057DB9
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00059BC0 File Offset: 0x00057DC0
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00059BE4 File Offset: 0x00057DE4
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x00059C2C File Offset: 0x00057E2C
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x00059C74 File Offset: 0x00057E74
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00059CB8 File Offset: 0x00057EB8
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x040008D3 RID: 2259
		internal ZlibBaseStream _baseStream;

		// Token: 0x040008D4 RID: 2260
		private bool _disposed;
	}
}
