using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x02000148 RID: 328
	public class CountingStream : Stream
	{
		// Token: 0x06000D02 RID: 3330 RVA: 0x0004B438 File Offset: 0x00049638
		public CountingStream(Stream stream)
		{
			this._s = stream;
			try
			{
				this._initialOffset = this._s.Position;
			}
			catch
			{
				this._initialOffset = 0L;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x0004B480 File Offset: 0x00049680
		public Stream WrappedStream
		{
			get
			{
				return this._s;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x0004B488 File Offset: 0x00049688
		public long BytesWritten
		{
			get
			{
				return this._bytesWritten;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x0004B490 File Offset: 0x00049690
		public long BytesRead
		{
			get
			{
				return this._bytesRead;
			}
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0004B498 File Offset: 0x00049698
		public void Adjust(long delta)
		{
			this._bytesWritten -= delta;
			if (this._bytesWritten < 0L)
			{
				throw new InvalidOperationException();
			}
			if (this._s is CountingStream)
			{
				((CountingStream)this._s).Adjust(delta);
			}
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0004B4D8 File Offset: 0x000496D8
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this._s.Read(buffer, offset, count);
			this._bytesRead += (long)num;
			return num;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0004B504 File Offset: 0x00049704
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			this._s.Write(buffer, offset, count);
			this._bytesWritten += (long)count;
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x0004B527 File Offset: 0x00049727
		public override bool CanRead
		{
			get
			{
				return this._s.CanRead;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x0004B534 File Offset: 0x00049734
		public override bool CanSeek
		{
			get
			{
				return this._s.CanSeek;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x0004B541 File Offset: 0x00049741
		public override bool CanWrite
		{
			get
			{
				return this._s.CanWrite;
			}
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0004B54E File Offset: 0x0004974E
		public override void Flush()
		{
			this._s.Flush();
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x0004B55B File Offset: 0x0004975B
		public override long Length
		{
			get
			{
				return this._s.Length;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x0004B568 File Offset: 0x00049768
		public long ComputedPosition
		{
			get
			{
				return this._initialOffset + this._bytesWritten;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x0004B577 File Offset: 0x00049777
		// (set) Token: 0x06000D10 RID: 3344 RVA: 0x0004B584 File Offset: 0x00049784
		public override long Position
		{
			get
			{
				return this._s.Position;
			}
			set
			{
				this._s.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0004B594 File Offset: 0x00049794
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._s.Seek(offset, origin);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0004B5A3 File Offset: 0x000497A3
		public override void SetLength(long value)
		{
			this._s.SetLength(value);
		}

		// Token: 0x0400073C RID: 1852
		private Stream _s;

		// Token: 0x0400073D RID: 1853
		private long _bytesWritten;

		// Token: 0x0400073E RID: 1854
		private long _bytesRead;

		// Token: 0x0400073F RID: 1855
		private long _initialOffset;
	}
}
