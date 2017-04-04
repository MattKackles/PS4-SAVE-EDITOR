using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x020000B2 RID: 178
	public class PathFilter : IScanFilter
	{
		// Token: 0x060007AC RID: 1964 RVA: 0x0002CB58 File Offset: 0x0002AD58
		public PathFilter(string filter)
		{
			this.nameFilter_ = new NameFilter(filter);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0002CB6C File Offset: 0x0002AD6C
		public virtual bool IsMatch(string name)
		{
			bool result = false;
			if (name != null)
			{
				string name2 = (name.Length > 0) ? Path.GetFullPath(name) : "";
				result = this.nameFilter_.IsMatch(name2);
			}
			return result;
		}

		// Token: 0x04000384 RID: 900
		private NameFilter nameFilter_;
	}
}
