using System;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x020000A4 RID: 164
	public class ScanEventArgs : EventArgs
	{
		// Token: 0x0600076B RID: 1899 RVA: 0x0002C3A4 File Offset: 0x0002A5A4
		public ScanEventArgs(string name)
		{
			this.name_ = name;
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x0002C3BA File Offset: 0x0002A5BA
		public string Name
		{
			get
			{
				return this.name_;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x0002C3C2 File Offset: 0x0002A5C2
		// (set) Token: 0x0600076E RID: 1902 RVA: 0x0002C3CA File Offset: 0x0002A5CA
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

		// Token: 0x0400036F RID: 879
		private string name_;

		// Token: 0x04000370 RID: 880
		private bool continueRunning_ = true;
	}
}
