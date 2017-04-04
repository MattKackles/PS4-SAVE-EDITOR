using System;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x020000A7 RID: 167
	public class ScanFailureEventArgs : EventArgs
	{
		// Token: 0x06000778 RID: 1912 RVA: 0x0002C46E File Offset: 0x0002A66E
		public ScanFailureEventArgs(string name, Exception e)
		{
			this.name_ = name;
			this.exception_ = e;
			this.continueRunning_ = true;
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x0002C48B File Offset: 0x0002A68B
		public string Name
		{
			get
			{
				return this.name_;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x0002C493 File Offset: 0x0002A693
		public Exception Exception
		{
			get
			{
				return this.exception_;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x0002C49B File Offset: 0x0002A69B
		// (set) Token: 0x0600077C RID: 1916 RVA: 0x0002C4A3 File Offset: 0x0002A6A3
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

		// Token: 0x04000376 RID: 886
		private string name_;

		// Token: 0x04000377 RID: 887
		private Exception exception_;

		// Token: 0x04000378 RID: 888
		private bool continueRunning_;
	}
}
