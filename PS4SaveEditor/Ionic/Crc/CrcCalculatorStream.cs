using System;
using System.IO;

namespace Ionic.Crc
{
	// Token: 0x02000113 RID: 275
	public class CrcCalculatorStream : Stream, IDisposable
	{
		// Token: 0x06000B7E RID: 2942 RVA: 0x0003FF3A File Offset: 0x0003E13A
		public CrcCalculatorStream(Stream stream) : this(true, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0003FF4A File Offset: 0x0003E14A
		public CrcCalculatorStream(Stream stream, bool leaveOpen) : this(leaveOpen, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0003FF5A File Offset: 0x0003E15A
		public CrcCalculatorStream(Stream stream, long length) : this(true, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0003FF76 File Offset: 0x0003E176
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen) : this(leaveOpen, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0003FF92 File Offset: 0x0003E192
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32) : this(leaveOpen, length, stream, crc32)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0003FFAF File Offset: 0x0003E1AF
		private CrcCalculatorStream(bool leaveOpen, long length, Stream stream, CRC32 crc32)
		{
			this._innerStream = stream;
			this._Crc32 = (crc32 ?? new CRC32());
			this._lengthLimit = length;
			this._leaveOpen = leaveOpen;
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x0003FFE6 File Offset: 0x0003E1E6
		public long TotalBytesSlurped
		{
			get
			{
				return this._Crc32.TotalBytesRead;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x0003FFF3 File Offset: 0x0003E1F3
		public int Crc
		{
			get
			{
				return this._Crc32.Crc32Result;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x00040000 File Offset: 0x0003E200
		// (set) Token: 0x06000B87 RID: 2951 RVA: 0x00040008 File Offset: 0x0003E208
		public bool LeaveOpen
		{
			get
			{
				return this._leaveOpen;
			}
			set
			{
				this._leaveOpen = value;
			}
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x00040014 File Offset: 0x0003E214
		public override int Read(byte[] buffer, int offset, int count)
		{
			int count2 = count;
			if (this._lengthLimit != CrcCalculatorStream.UnsetLengthLimit)
			{
				if (this._Crc32.TotalBytesRead >= this._lengthLimit)
				{
					return 0;
				}
				long num = this._lengthLimit - this._Crc32.TotalBytesRead;
				if (num < (long)count)
				{
					count2 = (int)num;
				}
			}
			int num2 = this._innerStream.Read(buffer, offset, count2);
			if (num2 > 0)
			{
				this._Crc32.SlurpBlock(buffer, offset, num2);
			}
			return num2;
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x00040082 File Offset: 0x0003E282
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				this._Crc32.SlurpBlock(buffer, offset, count);
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x000400A4 File Offset: 0x0003E2A4
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x000400B1 File Offset: 0x0003E2B1
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x000400B4 File Offset: 0x0003E2B4
		public override bool CanWrite
		{
			get
			{
				return this._innerStream.CanWrite;
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x000400C1 File Offset: 0x0003E2C1
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x000400CE File Offset: 0x0003E2CE
		public override long Length
		{
			get
			{
				if (this._lengthLimit == CrcCalculatorStream.UnsetLengthLimit)
				{
					return this._innerStream.Length;
				}
				return this._lengthLimit;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x000400EF File Offset: 0x0003E2EF
		// (set) Token: 0x06000B90 RID: 2960 RVA: 0x000400FC File Offset: 0x0003E2FC
		public override long Position
		{
			get
			{
				return this._Crc32.TotalBytesRead;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00040103 File Offset: 0x0003E303
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0004010A File Offset: 0x0003E30A
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00040111 File Offset: 0x0003E311
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00040119 File Offset: 0x0003E319
		public override void Close()
		{
			base.Close();
			if (!this._leaveOpen)
			{
				this._innerStream.Close();
			}
		}

		// Token: 0x040005CD RID: 1485
		private static readonly long UnsetLengthLimit = -99L;

		// Token: 0x040005CE RID: 1486
		internal Stream _innerStream;

		// Token: 0x040005CF RID: 1487
		private CRC32 _Crc32;

		// Token: 0x040005D0 RID: 1488
		private long _lengthLimit = -99L;

		// Token: 0x040005D1 RID: 1489
		private bool _leaveOpen;
	}
}
