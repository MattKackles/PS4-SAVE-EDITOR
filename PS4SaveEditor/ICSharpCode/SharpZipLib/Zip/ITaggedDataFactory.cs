using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000E5 RID: 229
	internal interface ITaggedDataFactory
	{
		// Token: 0x06000984 RID: 2436
		ITaggedData Create(short tag, byte[] data, int offset, int count);
	}
}
