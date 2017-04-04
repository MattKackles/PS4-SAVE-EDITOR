using System;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x020000AF RID: 175
	public interface INameTransform
	{
		// Token: 0x060007A0 RID: 1952
		string TransformFile(string name);

		// Token: 0x060007A1 RID: 1953
		string TransformDirectory(string name);
	}
}
