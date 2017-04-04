using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x020000B4 RID: 180
	[Obsolete("Use ExtendedPathFilter instead")]
	public class NameAndSizeFilter : PathFilter
	{
		// Token: 0x060007BA RID: 1978 RVA: 0x0002CD8A File Offset: 0x0002AF8A
		public NameAndSizeFilter(string filter, long minSize, long maxSize) : base(filter)
		{
			this.MinSize = minSize;
			this.MaxSize = maxSize;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0002CDB0 File Offset: 0x0002AFB0
		public override bool IsMatch(string name)
		{
			bool flag = base.IsMatch(name);
			if (flag)
			{
				FileInfo fileInfo = new FileInfo(name);
				long length = fileInfo.Length;
				flag = (this.MinSize <= length && this.MaxSize >= length);
			}
			return flag;
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x0002CDF0 File Offset: 0x0002AFF0
		// (set) Token: 0x060007BD RID: 1981 RVA: 0x0002CDF8 File Offset: 0x0002AFF8
		public long MinSize
		{
			get
			{
				return this.minSize_;
			}
			set
			{
				if (value < 0L || this.maxSize_ < value)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.minSize_ = value;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x0002CE1A File Offset: 0x0002B01A
		// (set) Token: 0x060007BF RID: 1983 RVA: 0x0002CE22 File Offset: 0x0002B022
		public long MaxSize
		{
			get
			{
				return this.maxSize_;
			}
			set
			{
				if (value < 0L || this.minSize_ > value)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.maxSize_ = value;
			}
		}

		// Token: 0x04000389 RID: 905
		private long minSize_;

		// Token: 0x0400038A RID: 906
		private long maxSize_ = 9223372036854775807L;
	}
}
