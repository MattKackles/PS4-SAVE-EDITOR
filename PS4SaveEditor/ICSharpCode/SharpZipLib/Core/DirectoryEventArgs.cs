using System;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x020000A6 RID: 166
	public class DirectoryEventArgs : ScanEventArgs
	{
		// Token: 0x06000776 RID: 1910 RVA: 0x0002C456 File Offset: 0x0002A656
		public DirectoryEventArgs(string name, bool hasMatchingFiles) : base(name)
		{
			this.hasMatchingFiles_ = hasMatchingFiles;
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x0002C466 File Offset: 0x0002A666
		public bool HasMatchingFiles
		{
			get
			{
				return this.hasMatchingFiles_;
			}
		}

		// Token: 0x04000375 RID: 885
		private bool hasMatchingFiles_;
	}
}
