using System;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000D3 RID: 211
	public interface IEntryFactory
	{
		// Token: 0x060008F0 RID: 2288
		ZipEntry MakeFileEntry(string fileName);

		// Token: 0x060008F1 RID: 2289
		ZipEntry MakeFileEntry(string fileName, bool useFileSystem);

		// Token: 0x060008F2 RID: 2290
		ZipEntry MakeDirectoryEntry(string directoryName);

		// Token: 0x060008F3 RID: 2291
		ZipEntry MakeDirectoryEntry(string directoryName, bool useFileSystem);

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060008F4 RID: 2292
		// (set) Token: 0x060008F5 RID: 2293
		INameTransform NameTransform
		{
			get;
			set;
		}
	}
}
