using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000F9 RID: 249
	public class StaticDiskDataSource : IStaticDataSource
	{
		// Token: 0x06000A5F RID: 2655 RVA: 0x00038683 File Offset: 0x00036883
		public StaticDiskDataSource(string fileName)
		{
			this.fileName_ = fileName;
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00038692 File Offset: 0x00036892
		public Stream GetSource()
		{
			return File.Open(this.fileName_, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		// Token: 0x04000570 RID: 1392
		private string fileName_;
	}
}
