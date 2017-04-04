using System;
using System.IO;
using System.Text;
using Ionic.Zlib;

namespace Ionic.Zip
{
	// Token: 0x02000160 RID: 352
	internal class ZipContainer
	{
		// Token: 0x06000F2C RID: 3884 RVA: 0x000576BE File Offset: 0x000558BE
		public ZipContainer(object o)
		{
			this._zf = (o as ZipFile);
			this._zos = (o as ZipOutputStream);
			this._zis = (o as ZipInputStream);
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x000576EA File Offset: 0x000558EA
		public ZipFile ZipFile
		{
			get
			{
				return this._zf;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x000576F2 File Offset: 0x000558F2
		public ZipOutputStream ZipOutputStream
		{
			get
			{
				return this._zos;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x000576FA File Offset: 0x000558FA
		public string Name
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.Name;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return this._zos.Name;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x00057729 File Offset: 0x00055929
		public string Password
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf._Password;
				}
				if (this._zis != null)
				{
					return this._zis._Password;
				}
				return this._zos._password;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000F31 RID: 3889 RVA: 0x0005775E File Offset: 0x0005595E
		public Zip64Option Zip64
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf._zip64;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return this._zos._zip64;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000F32 RID: 3890 RVA: 0x0005778D File Offset: 0x0005598D
		public int BufferSize
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.BufferSize;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return 0;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000F33 RID: 3891 RVA: 0x000577B2 File Offset: 0x000559B2
		// (set) Token: 0x06000F34 RID: 3892 RVA: 0x000577DD File Offset: 0x000559DD
		public ParallelDeflateOutputStream ParallelDeflater
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflater;
				}
				if (this._zis != null)
				{
					return null;
				}
				return this._zos.ParallelDeflater;
			}
			set
			{
				if (this._zf != null)
				{
					this._zf.ParallelDeflater = value;
					return;
				}
				if (this._zos != null)
				{
					this._zos.ParallelDeflater = value;
				}
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000F35 RID: 3893 RVA: 0x00057808 File Offset: 0x00055A08
		public long ParallelDeflateThreshold
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflateThreshold;
				}
				return this._zos.ParallelDeflateThreshold;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x00057829 File Offset: 0x00055A29
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflateMaxBufferPairs;
				}
				return this._zos.ParallelDeflateMaxBufferPairs;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000F37 RID: 3895 RVA: 0x0005784A File Offset: 0x00055A4A
		public int CodecBufferSize
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.CodecBufferSize;
				}
				if (this._zis != null)
				{
					return this._zis.CodecBufferSize;
				}
				return this._zos.CodecBufferSize;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0005787F File Offset: 0x00055A7F
		public CompressionStrategy Strategy
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.Strategy;
				}
				return this._zos.Strategy;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000F39 RID: 3897 RVA: 0x000578A0 File Offset: 0x00055AA0
		public Zip64Option UseZip64WhenSaving
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.UseZip64WhenSaving;
				}
				return this._zos.EnableZip64;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000F3A RID: 3898 RVA: 0x000578C1 File Offset: 0x00055AC1
		public Encoding AlternateEncoding
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.AlternateEncoding;
				}
				if (this._zos != null)
				{
					return this._zos.AlternateEncoding;
				}
				return null;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000F3B RID: 3899 RVA: 0x000578EC File Offset: 0x00055AEC
		public Encoding DefaultEncoding
		{
			get
			{
				if (this._zf != null)
				{
					return ZipFile.DefaultEncoding;
				}
				if (this._zos != null)
				{
					return ZipOutputStream.DefaultEncoding;
				}
				return null;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000F3C RID: 3900 RVA: 0x0005790B File Offset: 0x00055B0B
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.AlternateEncodingUsage;
				}
				if (this._zos != null)
				{
					return this._zos.AlternateEncodingUsage;
				}
				return ZipOption.Default;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000F3D RID: 3901 RVA: 0x00057936 File Offset: 0x00055B36
		public Stream ReadStream
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ReadStream;
				}
				return this._zis.ReadStream;
			}
		}

		// Token: 0x0400085C RID: 2140
		private ZipFile _zf;

		// Token: 0x0400085D RID: 2141
		private ZipOutputStream _zos;

		// Token: 0x0400085E RID: 2142
		private ZipInputStream _zis;
	}
}
