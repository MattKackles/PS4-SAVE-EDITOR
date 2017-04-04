using System;
using System.Runtime.InteropServices;

namespace Be.Windows.Forms
{
	// Token: 0x02000059 RID: 89
	internal static class NativeMethods
	{
		// Token: 0x060004BE RID: 1214
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool CreateCaret(IntPtr hWnd, IntPtr hBitmap, int nWidth, int nHeight);

		// Token: 0x060004BF RID: 1215
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool ShowCaret(IntPtr hWnd);

		// Token: 0x060004C0 RID: 1216
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool DestroyCaret();

		// Token: 0x060004C1 RID: 1217
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetCaretPos(int X, int Y);

		// Token: 0x0400021F RID: 543
		public const int WM_KEYDOWN = 256;

		// Token: 0x04000220 RID: 544
		public const int WM_KEYUP = 257;

		// Token: 0x04000221 RID: 545
		public const int WM_CHAR = 258;
	}
}
