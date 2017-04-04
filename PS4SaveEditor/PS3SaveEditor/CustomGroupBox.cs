using System;
using System.Drawing;
using System.Windows.Forms;

namespace PS3SaveEditor
{
	// Token: 0x0200001B RID: 27
	public class CustomGroupBox : GroupBox
	{
		// Token: 0x06000120 RID: 288 RVA: 0x0000A49C File Offset: 0x0000869C
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawRectangle(Pens.White, new Rectangle(base.ClientRectangle.Left, base.ClientRectangle.Top + 4, base.ClientRectangle.Width - 1, base.ClientRectangle.Height - 6));
		}
	}
}
