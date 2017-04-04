using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x02000143 RID: 323
	internal class OffsetStream : Stream, IDisposable
	{
		// Token: 0x06000CBA RID: 3258 RVA: 0x0004A0F2 File Offset: 0x000482F2
		public OffsetStream(Stream s)
		{
			this._originalPosition = s.Position;
			this._innerStream = s;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0004A10D File Offset: 0x0004830D
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._innerStream.Read(buffer, offset, count);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0004A11D File Offset: 0x0004831D
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x0004A124 File Offset: 0x00048324
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x0004A131 File Offset: 0x00048331
		public override bool CanSeek
		{
			get
			{
				return this._innerStream.CanSeek;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x0004A13E File Offset: 0x0004833E
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0004A141 File Offset: 0x00048341
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0004A14E File Offset: 0x0004834E
		public override long Length
		{
			get
			{
				return this._innerStream.Length;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x0004A15B File Offset: 0x0004835B
		// (set) Token: 0x06000CC3 RID: 3267 RVA: 0x0004A16F File Offset: 0x0004836F
		public override long Position
		{
			get
			{
				return this._innerStream.Position - this._originalPosition;
			}
			set
			{
				this._innerStream.Position = this._originalPosition + value;
			}
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0004A184 File Offset: 0x00048384
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._innerStream.Seek(this._originalPosition + offset, origin) - this._originalPosition;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0004A1A1 File Offset: 0x000483A1
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0004A1A8 File Offset: 0x000483A8
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0004A1B0 File Offset: 0x000483B0
		public override void Close()
		{
			base.Close();
		}

		// Token: 0x04000700 RID: 1792
		private long _originalPosition;

		// Token: 0x04000701 RID: 1793
		private Stream _innerStream;
	}
}
