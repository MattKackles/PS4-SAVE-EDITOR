using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000FB RID: 251
	public interface IArchiveStorage
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000A63 RID: 2659
		FileUpdateMode UpdateMode
		{
			get;
		}

		// Token: 0x06000A64 RID: 2660
		Stream GetTemporaryOutput();

		// Token: 0x06000A65 RID: 2661
		Stream ConvertTemporaryToFinal();

		// Token: 0x06000A66 RID: 2662
		Stream MakeTemporaryCopy(Stream stream);

		// Token: 0x06000A67 RID: 2663
		Stream OpenForDirectUpdate(Stream stream);

		// Token: 0x06000A68 RID: 2664
		void Dispose();
	}
}
