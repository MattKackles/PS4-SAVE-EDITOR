using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000FA RID: 250
	public class DynamicDiskDataSource : IDynamicDataSource
	{
		// Token: 0x06000A62 RID: 2658 RVA: 0x000386AC File Offset: 0x000368AC
		public Stream GetSource(ZipEntry entry, string name)
		{
			Stream result = null;
			if (name != null)
			{
				result = File.Open(name, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			return result;
		}
	}
}
