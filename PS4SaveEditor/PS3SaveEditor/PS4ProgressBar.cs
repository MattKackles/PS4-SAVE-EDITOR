using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PS3SaveEditor
{
	// Token: 0x0200001E RID: 30
	public class PS4ProgressBar : ProgressBar
	{
		// Token: 0x06000134 RID: 308 RVA: 0x0000BCC8 File Offset: 0x00009EC8
		public PS4ProgressBar()
		{
			this.InitializeComponent();
			this.DoubleBuffered = true;
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.UserPaint, true);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000BCF4 File Offset: 0x00009EF4
		protected override void OnPaint(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 181, 255), Color.FromArgb(0, 62, 207), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, 0f, 0f, (float)base.ClientRectangle.Width * (float)base.Value / (float)base.Maximum, (float)base.ClientRectangle.Height);
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000BD90 File Offset: 0x00009F90
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000BDAF File Offset: 0x00009FAF
		private void InitializeComponent()
		{
			this.components = new Container();
		}

		// Token: 0x040000BA RID: 186
		private IContainer components;
	}
}
