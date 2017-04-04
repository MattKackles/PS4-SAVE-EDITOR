using System;
using System.Diagnostics;

namespace Be.Windows.Forms
{
	// Token: 0x0200005A RID: 90
	internal static class Util
	{
		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0001895A File Offset: 0x00016B5A
		public static bool DesignMode
		{
			get
			{
				return Util._designMode;
			}
		}

		// Token: 0x04000222 RID: 546
		private static bool _designMode = Process.GetCurrentProcess().ProcessName.ToLower() == "devenv";
	}
}
