using System;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x020000A5 RID: 165
	public class ProgressEventArgs : EventArgs
	{
		// Token: 0x0600076F RID: 1903 RVA: 0x0002C3D3 File Offset: 0x0002A5D3
		public ProgressEventArgs(string name, long processed, long target)
		{
			this.name_ = name;
			this.processed_ = processed;
			this.target_ = target;
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x0002C3F7 File Offset: 0x0002A5F7
		public string Name
		{
			get
			{
				return this.name_;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x0002C3FF File Offset: 0x0002A5FF
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x0002C407 File Offset: 0x0002A607
		public bool ContinueRunning
		{
			get
			{
				return this.continueRunning_;
			}
			set
			{
				this.continueRunning_ = value;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x0002C410 File Offset: 0x0002A610
		public float PercentComplete
		{
			get
			{
				float result;
				if (this.target_ <= 0L)
				{
					result = 0f;
				}
				else
				{
					result = (float)this.processed_ / (float)this.target_ * 100f;
				}
				return result;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x0002C446 File Offset: 0x0002A646
		public long Processed
		{
			get
			{
				return this.processed_;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x0002C44E File Offset: 0x0002A64E
		public long Target
		{
			get
			{
				return this.target_;
			}
		}

		// Token: 0x04000371 RID: 881
		private string name_;

		// Token: 0x04000372 RID: 882
		private long processed_;

		// Token: 0x04000373 RID: 883
		private long target_;

		// Token: 0x04000374 RID: 884
		private bool continueRunning_ = true;
	}
}
