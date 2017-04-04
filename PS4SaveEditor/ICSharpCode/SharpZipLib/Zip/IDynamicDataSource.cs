using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000F8 RID: 248
	public interface IDynamicDataSource
	{
		// Token: 0x06000A5E RID: 2654
		Stream GetSource(ZipEntry entry, string name);
	}
}
