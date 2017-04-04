using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x02000167 RID: 359
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000E")]
	public class ZlibException : Exception
	{
		// Token: 0x06000F5A RID: 3930 RVA: 0x00057FE4 File Offset: 0x000561E4
		public ZlibException()
		{
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00057FEC File Offset: 0x000561EC
		public ZlibException(string s) : base(s)
		{
		}
	}
}
