using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
	// Token: 0x020000CC RID: 204
	public class InflaterInputStream : Stream
	{
		// Token: 0x0600089B RID: 2203 RVA: 0x00031C6E File Offset: 0x0002FE6E
		public InflaterInputStream(Stream baseInputStream) : this(baseInputStream, new Inflater(), 4096)
		{
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00031C81 File Offset: 0x0002FE81
		public InflaterInputStream(Stream baseInputStream, Inflater inf) : this(baseInputStream, inf, 4096)
		{
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00031C90 File Offset: 0x0002FE90
		public InflaterInputStream(Stream baseInputStream, Inflater inflater, int bufferSize)
		{
			if (baseInputStream == null)
			{
				throw new ArgumentNullException("baseInputStream");
			}
			if (inflater == null)
			{
				throw new ArgumentNullException("inflater");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			this.baseInputStream = baseInputStream;
			this.inf = inflater;
			this.inputBuffer = new InflaterInputBuffer(baseInputStream, bufferSize);
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00031CF0 File Offset: 0x0002FEF0
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x00031CF8 File Offset: 0x0002FEF8
		public bool IsStreamOwner
		{
			get
			{
				return this.isStreamOwner;
			}
			set
			{
				this.isStreamOwner = value;
			}
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00031D04 File Offset: 0x0002FF04
		public long Skip(long count)
		{
			if (count <= 0L)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.baseInputStream.CanSeek)
			{
				this.baseInputStream.Seek(count, SeekOrigin.Current);
				return count;
			}
			int num = 2048;
			if (count < (long)num)
			{
				num = (int)count;
			}
			byte[] buffer = new byte[num];
			int num2 = 1;
			long num3 = count;
			while (num3 > 0L && num2 > 0)
			{
				if (num3 < (long)num)
				{
					num = (int)num3;
				}
				num2 = this.baseInputStream.Read(buffer, 0, num);
				num3 -= (long)num2;
			}
			return count - num3;
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00031D81 File Offset: 0x0002FF81
		protected void StopDecrypting()
		{
			this.inputBuffer.CryptoTransform = null;
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00031D8F File Offset: 0x0002FF8F
		public virtual int Available
		{
			get
			{
				if (!this.inf.IsFinished)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00031DA4 File Offset: 0x0002FFA4
		protected void Fill()
		{
			if (this.inputBuffer.Available <= 0)
			{
				this.inputBuffer.Fill();
				if (this.inputBuffer.Available <= 0)
				{
					throw new SharpZipBaseException("Unexpected EOF");
				}
			}
			this.inputBuffer.SetInflaterInput(this.inf);
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00031DF4 File Offset: 0x0002FFF4
		public override bool CanRead
		{
			get
			{
				return this.baseInputStream.CanRead;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x00031E01 File Offset: 0x00030001
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x00031E04 File Offset: 0x00030004
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x00031E07 File Offset: 0x00030007
		public override long Length
		{
			get
			{
				return (long)this.inputBuffer.RawLength;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00031E15 File Offset: 0x00030015
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x00031E22 File Offset: 0x00030022
		public override long Position
		{
			get
			{
				return this.baseInputStream.Position;
			}
			set
			{
				throw new NotSupportedException("InflaterInputStream Position not supported");
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00031E2E File Offset: 0x0003002E
		public override void Flush()
		{
			this.baseInputStream.Flush();
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00031E3B File Offset: 0x0003003B
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("Seek not supported");
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00031E47 File Offset: 0x00030047
		public override void SetLength(long value)
		{
			throw new NotSupportedException("InflaterInputStream SetLength not supported");
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00031E53 File Offset: 0x00030053
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("InflaterInputStream Write not supported");
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00031E5F File Offset: 0x0003005F
		public override void WriteByte(byte value)
		{
			throw new NotSupportedException("InflaterInputStream WriteByte not supported");
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00031E6B File Offset: 0x0003006B
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException("InflaterInputStream BeginWrite not supported");
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00031E77 File Offset: 0x00030077
		public override void Close()
		{
			if (!this.isClosed)
			{
				this.isClosed = true;
				if (this.isStreamOwner)
				{
					this.baseInputStream.Close();
				}
			}
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00031E9C File Offset: 0x0003009C
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.inf.IsNeedingDictionary)
			{
				throw new SharpZipBaseException("Need a dictionary");
			}
			int num = count;
			while (true)
			{
				int num2 = this.inf.Inflate(buffer, offset, num);
				offset += num2;
				num -= num2;
				if (num == 0 || this.inf.IsFinished)
				{
					goto IL_65;
				}
				if (this.inf.IsNeedingInput)
				{
					this.Fill();
				}
				else if (num2 == 0)
				{
					break;
				}
			}
			throw new ZipException("Dont know what to do");
			IL_65:
			return count - num;
		}

		// Token: 0x0400045A RID: 1114
		protected Inflater inf;

		// Token: 0x0400045B RID: 1115
		protected InflaterInputBuffer inputBuffer;

		// Token: 0x0400045C RID: 1116
		private Stream baseInputStream;

		// Token: 0x0400045D RID: 1117
		protected long csize;

		// Token: 0x0400045E RID: 1118
		private bool isClosed;

		// Token: 0x0400045F RID: 1119
		private bool isStreamOwner = true;
	}
}
