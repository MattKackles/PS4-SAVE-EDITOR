using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x020000B3 RID: 179
	public class ExtendedPathFilter : PathFilter
	{
		// Token: 0x060007AE RID: 1966 RVA: 0x0002CBA3 File Offset: 0x0002ADA3
		public ExtendedPathFilter(string filter, long minSize, long maxSize) : base(filter)
		{
			this.MinSize = minSize;
			this.MaxSize = maxSize;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0002CBDF File Offset: 0x0002ADDF
		public ExtendedPathFilter(string filter, DateTime minDate, DateTime maxDate) : base(filter)
		{
			this.MinDate = minDate;
			this.MaxDate = maxDate;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0002CC1C File Offset: 0x0002AE1C
		public ExtendedPathFilter(string filter, long minSize, long maxSize, DateTime minDate, DateTime maxDate) : base(filter)
		{
			this.MinSize = minSize;
			this.MaxSize = maxSize;
			this.MinDate = minDate;
			this.MaxDate = maxDate;
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0002CC74 File Offset: 0x0002AE74
		public override bool IsMatch(string name)
		{
			bool flag = base.IsMatch(name);
			if (flag)
			{
				FileInfo fileInfo = new FileInfo(name);
				flag = (this.MinSize <= fileInfo.Length && this.MaxSize >= fileInfo.Length && this.MinDate <= fileInfo.LastWriteTime && this.MaxDate >= fileInfo.LastWriteTime);
			}
			return flag;
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0002CCD8 File Offset: 0x0002AED8
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x0002CCE0 File Offset: 0x0002AEE0
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

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0002CD02 File Offset: 0x0002AF02
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x0002CD0A File Offset: 0x0002AF0A
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

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0002CD2C File Offset: 0x0002AF2C
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x0002CD34 File Offset: 0x0002AF34
		public DateTime MinDate
		{
			get
			{
				return this.minDate_;
			}
			set
			{
				if (value > this.maxDate_)
				{
					throw new ArgumentOutOfRangeException("value", "Exceeds MaxDate");
				}
				this.minDate_ = value;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x0002CD5B File Offset: 0x0002AF5B
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x0002CD63 File Offset: 0x0002AF63
		public DateTime MaxDate
		{
			get
			{
				return this.maxDate_;
			}
			set
			{
				if (this.minDate_ > value)
				{
					throw new ArgumentOutOfRangeException("value", "Exceeds MinDate");
				}
				this.maxDate_ = value;
			}
		}

		// Token: 0x04000385 RID: 901
		private long minSize_;

		// Token: 0x04000386 RID: 902
		private long maxSize_ = 9223372036854775807L;

		// Token: 0x04000387 RID: 903
		private DateTime minDate_ = DateTime.MinValue;

		// Token: 0x04000388 RID: 904
		private DateTime maxDate_ = DateTime.MaxValue;
	}
}
