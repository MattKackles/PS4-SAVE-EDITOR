using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000E0 RID: 224
	public interface ITaggedData
	{
		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000962 RID: 2402
		short TagID
		{
			get;
		}

		// Token: 0x06000963 RID: 2403
		void SetData(byte[] data, int offset, int count);

		// Token: 0x06000964 RID: 2404
		byte[] GetData();
	}
}
