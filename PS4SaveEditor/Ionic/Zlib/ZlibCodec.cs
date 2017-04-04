using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x0200016F RID: 367
	[ComVisible(true), Guid("ebc25cf6-9120-4283-b972-0e5520d0000D"), ClassInterface(ClassInterfaceType.AutoDispatch)]
	public sealed class ZlibCodec
	{
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000F80 RID: 3968 RVA: 0x000594DC File Offset: 0x000576DC
		public int Adler32
		{
			get
			{
				return (int)this._Adler32;
			}
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x000594E4 File Offset: 0x000576E4
		public ZlibCodec()
		{
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x000594FC File Offset: 0x000576FC
		public ZlibCodec(CompressionMode mode)
		{
			if (mode == CompressionMode.Compress)
			{
				int num = this.InitializeDeflate();
				if (num != 0)
				{
					throw new ZlibException("Cannot initialize for deflate.");
				}
			}
			else
			{
				if (mode != CompressionMode.Decompress)
				{
					throw new ZlibException("Invalid ZlibStreamFlavor.");
				}
				int num2 = this.InitializeInflate();
				if (num2 != 0)
				{
					throw new ZlibException("Cannot initialize for inflate.");
				}
			}
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0005955A File Offset: 0x0005775A
		public int InitializeInflate()
		{
			return this.InitializeInflate(this.WindowBits);
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x00059568 File Offset: 0x00057768
		public int InitializeInflate(bool expectRfc1950Header)
		{
			return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00059577 File Offset: 0x00057777
		public int InitializeInflate(int windowBits)
		{
			this.WindowBits = windowBits;
			return this.InitializeInflate(windowBits, true);
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x00059588 File Offset: 0x00057788
		public int InitializeInflate(int windowBits, bool expectRfc1950Header)
		{
			this.WindowBits = windowBits;
			if (this.dstate != null)
			{
				throw new ZlibException("You may not call InitializeInflate() after calling InitializeDeflate().");
			}
			this.istate = new InflateManager(expectRfc1950Header);
			return this.istate.Initialize(this, windowBits);
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x000595BD File Offset: 0x000577BD
		public int Inflate(FlushType flush)
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Inflate(flush);
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x000595E0 File Offset: 0x000577E0
		public int EndInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			int result = this.istate.End();
			this.istate = null;
			return result;
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x00059614 File Offset: 0x00057814
		public int SyncInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Sync();
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x00059634 File Offset: 0x00057834
		public int InitializeDeflate()
		{
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0005963D File Offset: 0x0005783D
		public int InitializeDeflate(CompressionLevel level)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0005964D File Offset: 0x0005784D
		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0005965D File Offset: 0x0005785D
		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x00059674 File Offset: 0x00057874
		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0005968C File Offset: 0x0005788C
		private int _InternalInitializeDeflate(bool wantRfc1950Header)
		{
			if (this.istate != null)
			{
				throw new ZlibException("You may not call InitializeDeflate() after calling InitializeInflate().");
			}
			this.dstate = new DeflateManager();
			this.dstate.WantRfc1950HeaderBytes = wantRfc1950Header;
			return this.dstate.Initialize(this, this.CompressLevel, this.WindowBits, this.Strategy);
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x000596E1 File Offset: 0x000578E1
		public int Deflate(FlushType flush)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.Deflate(flush);
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00059702 File Offset: 0x00057902
		public int EndDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate = null;
			return 0;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0005971F File Offset: 0x0005791F
		public void ResetDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate.Reset();
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0005973F File Offset: 0x0005793F
		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.SetParams(level, strategy);
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00059761 File Offset: 0x00057961
		public int SetDictionary(byte[] dictionary)
		{
			if (this.istate != null)
			{
				return this.istate.SetDictionary(dictionary);
			}
			if (this.dstate != null)
			{
				return this.dstate.SetDictionary(dictionary);
			}
			throw new ZlibException("No Inflate or Deflate state!");
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00059798 File Offset: 0x00057998
		internal void flush_pending()
		{
			int num = this.dstate.pendingCount;
			if (num > this.AvailableBytesOut)
			{
				num = this.AvailableBytesOut;
			}
			if (num == 0)
			{
				return;
			}
			if (this.dstate.pending.Length <= this.dstate.nextPending || this.OutputBuffer.Length <= this.NextOut || this.dstate.pending.Length < this.dstate.nextPending + num || this.OutputBuffer.Length < this.NextOut + num)
			{
				throw new ZlibException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.dstate.pending.Length, this.dstate.pendingCount));
			}
			Array.Copy(this.dstate.pending, this.dstate.nextPending, this.OutputBuffer, this.NextOut, num);
			this.NextOut += num;
			this.dstate.nextPending += num;
			this.TotalBytesOut += (long)num;
			this.AvailableBytesOut -= num;
			this.dstate.pendingCount -= num;
			if (this.dstate.pendingCount == 0)
			{
				this.dstate.nextPending = 0;
			}
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x000598E4 File Offset: 0x00057AE4
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.AvailableBytesIn;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			this.AvailableBytesIn -= num;
			if (this.dstate.WantRfc1950HeaderBytes)
			{
				this._Adler32 = Adler.Adler32(this._Adler32, this.InputBuffer, this.NextIn, num);
			}
			Array.Copy(this.InputBuffer, this.NextIn, buf, start, num);
			this.NextIn += num;
			this.TotalBytesIn += (long)num;
			return num;
		}

		// Token: 0x040008BA RID: 2234
		public byte[] InputBuffer;

		// Token: 0x040008BB RID: 2235
		public int NextIn;

		// Token: 0x040008BC RID: 2236
		public int AvailableBytesIn;

		// Token: 0x040008BD RID: 2237
		public long TotalBytesIn;

		// Token: 0x040008BE RID: 2238
		public byte[] OutputBuffer;

		// Token: 0x040008BF RID: 2239
		public int NextOut;

		// Token: 0x040008C0 RID: 2240
		public int AvailableBytesOut;

		// Token: 0x040008C1 RID: 2241
		public long TotalBytesOut;

		// Token: 0x040008C2 RID: 2242
		public string Message;

		// Token: 0x040008C3 RID: 2243
		internal DeflateManager dstate;

		// Token: 0x040008C4 RID: 2244
		internal InflateManager istate;

		// Token: 0x040008C5 RID: 2245
		internal uint _Adler32;

		// Token: 0x040008C6 RID: 2246
		public CompressionLevel CompressLevel = CompressionLevel.Default;

		// Token: 0x040008C7 RID: 2247
		public int WindowBits = 15;

		// Token: 0x040008C8 RID: 2248
		public CompressionStrategy Strategy;
	}
}
