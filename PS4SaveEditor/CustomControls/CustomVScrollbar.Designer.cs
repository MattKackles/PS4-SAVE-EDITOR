using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PS3SaveEditor.CustomScrollbar;

namespace CustomControls
{
	// Token: 0x02000026 RID: 38
	public class CustomVScrollbar : UserControl
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000261 RID: 609 RVA: 0x0000EE20 File Offset: 0x0000D020
		// (remove) Token: 0x06000262 RID: 610 RVA: 0x0000EE58 File Offset: 0x0000D058
		public new event EventHandler Scroll;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000263 RID: 611 RVA: 0x0000EE90 File Offset: 0x0000D090
		// (remove) Token: 0x06000264 RID: 612 RVA: 0x0000EEC8 File Offset: 0x0000D0C8
		public event EventHandler ValueChanged;

		// Token: 0x06000265 RID: 613 RVA: 0x0000EF00 File Offset: 0x0000D100
		private int GetThumbHeight()
		{
			int num = base.Height - (this.UpArrowImage.Height + this.DownArrowImage.Height);
			float num2 = (float)this.LargeChange / (float)this.Maximum * (float)num;
			int num3 = (int)num2;
			if (num3 > num)
			{
				num3 = num;
				num2 = (float)num;
			}
			if (num3 < 56)
			{
				num3 = 56;
			}
			return num3;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000EF5C File Offset: 0x0000D15C
		public CustomVScrollbar()
		{
			this.InitializeComponent();
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			this.moChannelColor = Color.FromArgb(125, 45, 17);
			this.UpArrowImage = Resource.uparrow;
			this.DownArrowImage = Resource.downarrow;
			this.ThumbBottomImage = Resource.ThumbBottom;
			this.ThumbBottomSpanImage = Resource.ThumbSpanBottom;
			this.ThumbTopImage = Resource.ThumbTop;
			this.ThumbTopSpanImage = Resource.ThumbSpanTop;
			this.ThumbMiddleImage = Resource.ThumbMiddle;
			base.Width = this.UpArrowImage.Width;
			base.MinimumSize = new Size(this.UpArrowImage.Width, this.UpArrowImage.Height + this.DownArrowImage.Height + this.GetThumbHeight());
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000F05B File Offset: 0x0000D25B
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000F063 File Offset: 0x0000D263
		[DefaultValue(false), Browsable(true), Description("LargeChange"), EditorBrowsable(EditorBrowsableState.Always), Category("Behavior")]
		public int LargeChange
		{
			get
			{
				return this.moLargeChange;
			}
			set
			{
				this.moLargeChange = value;
				base.Invalidate();
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000F072 File Offset: 0x0000D272
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0000F07A File Offset: 0x0000D27A
		[DefaultValue(false), Category("Behavior"), Description("SmallChange"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public int SmallChange
		{
			get
			{
				return this.moSmallChange;
			}
			set
			{
				this.moSmallChange = value;
				base.Invalidate();
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000F089 File Offset: 0x0000D289
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000F091 File Offset: 0x0000D291
		[DefaultValue(false), EditorBrowsable(EditorBrowsableState.Always), Description("Minimum"), Category("Behavior"), Browsable(true)]
		public int Minimum
		{
			get
			{
				return this.moMinimum;
			}
			set
			{
				this.moMinimum = value;
				base.Invalidate();
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000F0A0 File Offset: 0x0000D2A0
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000F0A8 File Offset: 0x0000D2A8
		[DefaultValue(false), Category("Behavior"), Description("Maximum"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public int Maximum
		{
			get
			{
				return this.moMaximum;
			}
			set
			{
				this.moMaximum = value;
				base.Invalidate();
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000F0B7 File Offset: 0x0000D2B7
		// (set) Token: 0x06000270 RID: 624 RVA: 0x0000F0C0 File Offset: 0x0000D2C0
		[Browsable(true), Description("Value"), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(false), Category("Behavior")]
		public int Value
		{
			get
			{
				return this.moValue;
			}
			set
			{
				this.moValue = value;
				int num = base.Height - (this.UpArrowImage.Height + this.DownArrowImage.Height);
				float num2 = (float)this.LargeChange / (float)this.Maximum * (float)num;
				int num3 = (int)num2;
				if (num3 > num)
				{
					num3 = num;
					num2 = (float)num;
				}
				if (num3 < 56)
				{
					num3 = 56;
				}
				int num4 = num - num3;
				int num5 = this.Maximum - this.Minimum - this.LargeChange;
				float num6 = 0f;
				if (num5 != 0)
				{
					num6 = (float)this.moValue / (float)num5;
				}
				float num7 = num6 * (float)num4;
				this.moThumbTop = (int)num7;
				base.Invalidate();
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000F168 File Offset: 0x0000D368
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000F170 File Offset: 0x0000D370
		[Description("Channel Color"), EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin")]
		public Color ChannelColor
		{
			get
			{
				return this.moChannelColor;
			}
			set
			{
				this.moChannelColor = value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000F179 File Offset: 0x0000D379
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000F181 File Offset: 0x0000D381
		[Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Description("Up Arrow Graphic"), DefaultValue(false), Category("Skin")]
		public Image UpArrowImage
		{
			get
			{
				return this.moUpArrowImage;
			}
			set
			{
				this.moUpArrowImage = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000F18A File Offset: 0x0000D38A
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000F192 File Offset: 0x0000D392
		[EditorBrowsable(EditorBrowsableState.Always), Description("Up Arrow Graphic"), Browsable(true), DefaultValue(false), Category("Skin")]
		public Image DownArrowImage
		{
			get
			{
				return this.moDownArrowImage;
			}
			set
			{
				this.moDownArrowImage = value;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000F19B File Offset: 0x0000D39B
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000F1A3 File Offset: 0x0000D3A3
		[EditorBrowsable(EditorBrowsableState.Always), Description("Up Arrow Graphic"), Browsable(true), DefaultValue(false), Category("Skin")]
		public Image ThumbTopImage
		{
			get
			{
				return this.moThumbTopImage;
			}
			set
			{
				this.moThumbTopImage = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000F1AC File Offset: 0x0000D3AC
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000F1B4 File Offset: 0x0000D3B4
		[Category("Skin"), Browsable(true), Description("Up Arrow Graphic"), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(false)]
		public Image ThumbTopSpanImage
		{
			get
			{
				return this.moThumbTopSpanImage;
			}
			set
			{
				this.moThumbTopSpanImage = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000F1BD File Offset: 0x0000D3BD
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0000F1C5 File Offset: 0x0000D3C5
		[Browsable(true), Description("Up Arrow Graphic"), Category("Skin"), DefaultValue(false), EditorBrowsable(EditorBrowsableState.Always)]
		public Image ThumbBottomImage
		{
			get
			{
				return this.moThumbBottomImage;
			}
			set
			{
				this.moThumbBottomImage = value;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000F1CE File Offset: 0x0000D3CE
		// (set) Token: 0x0600027E RID: 638 RVA: 0x0000F1D6 File Offset: 0x0000D3D6
		[EditorBrowsable(EditorBrowsableState.Always), DefaultValue(false), Browsable(true), Description("Up Arrow Graphic"), Category("Skin")]
		public Image ThumbBottomSpanImage
		{
			get
			{
				return this.moThumbBottomSpanImage;
			}
			set
			{
				this.moThumbBottomSpanImage = value;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000F1DF File Offset: 0x0000D3DF
		// (set) Token: 0x06000280 RID: 640 RVA: 0x0000F1E7 File Offset: 0x0000D3E7
		[Description("Up Arrow Graphic"), DefaultValue(false), Category("Skin"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public Image ThumbMiddleImage
		{
			get
			{
				return this.moThumbMiddleImage;
			}
			set
			{
				this.moThumbMiddleImage = value;
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000F1F0 File Offset: 0x0000D3F0
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			if (this.UpArrowImage != null)
			{
				e.Graphics.DrawImage(this.UpArrowImage, new Rectangle(new Point(0, 0), new Size(base.Width, this.UpArrowImage.Height)));
			}
			Brush brush = new SolidBrush(this.moChannelColor);
			new SolidBrush(this.moChannelColor);
			e.Graphics.FillRectangle(brush, new Rectangle(0, this.UpArrowImage.Height, base.Width, base.Height - this.DownArrowImage.Height));
			int num = base.Height - (this.UpArrowImage.Height + this.DownArrowImage.Height);
			float num2 = (float)this.LargeChange / (float)this.Maximum * (float)num;
			int num3 = (int)num2;
			if (num3 > num)
			{
				num3 = num;
				num2 = (float)num;
			}
			if (num3 < 56)
			{
				num2 = 56f;
			}
			float num4 = (num2 - (float)(this.ThumbMiddleImage.Height + this.ThumbTopImage.Height + this.ThumbBottomImage.Height)) / 2f;
			int num5 = (int)num4;
			int num6 = this.moThumbTop;
			num6 += this.UpArrowImage.Height;
			e.Graphics.DrawImage(this.ThumbTopImage, new Rectangle(1, num6, base.Width - 2, this.ThumbTopImage.Height));
			num6 += this.ThumbTopImage.Height;
			Rectangle rect = new Rectangle(1, num6, base.Width - 2, num5);
			e.Graphics.DrawImage(this.ThumbTopSpanImage, 1f, (float)num6, (float)base.Width - 2f, num4 * 2f);
			num6 += num5;
			e.Graphics.DrawImage(this.ThumbMiddleImage, new Rectangle(1, num6, base.Width - 2, this.ThumbMiddleImage.Height));
			num6 += this.ThumbMiddleImage.Height;
			rect = new Rectangle(1, num6, base.Width - 2, num5 * 2);
			e.Graphics.DrawImage(this.ThumbBottomSpanImage, rect);
			num6 += num5;
			e.Graphics.DrawImage(this.ThumbBottomImage, new Rectangle(1, num6, base.Width - 2, num5));
			if (this.DownArrowImage != null)
			{
				e.Graphics.DrawImage(this.DownArrowImage, new Rectangle(new Point(0, base.Height - this.DownArrowImage.Height), new Size(base.Width, this.DownArrowImage.Height)));
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000F489 File Offset: 0x0000D689
		// (set) Token: 0x06000283 RID: 643 RVA: 0x0000F491 File Offset: 0x0000D691
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
				if (base.AutoSize)
				{
					base.Width = this.moUpArrowImage.Width;
				}
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000F4B4 File Offset: 0x0000D6B4
		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.Name = "CustomVScrollbar";
			base.MouseDown += new MouseEventHandler(this.CustomScrollbar_MouseDown);
			base.MouseMove += new MouseEventHandler(this.CustomScrollbar_MouseMove);
			base.MouseUp += new MouseEventHandler(this.CustomScrollbar_MouseUp);
			base.ResumeLayout(false);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000F510 File Offset: 0x0000D710
		private void CustomScrollbar_MouseDown(object sender, MouseEventArgs e)
		{
			Point pt = base.PointToClient(Cursor.Position);
			int num = base.Height - (this.UpArrowImage.Height + this.DownArrowImage.Height);
			float num2 = (float)this.LargeChange / (float)this.Maximum * (float)num;
			int num3 = (int)num2;
			if (num3 > num)
			{
				num3 = num;
				num2 = (float)num;
			}
			if (num3 < 56)
			{
				num3 = 56;
			}
			int num4 = this.moThumbTop;
			num4 += this.UpArrowImage.Height;
			Rectangle rectangle = new Rectangle(new Point(1, num4), new Size(this.ThumbMiddleImage.Width, num3));
			if (rectangle.Contains(pt))
			{
				this.nClickPoint = pt.Y - num4;
				this.moThumbDown = true;
			}
			Rectangle rectangle2 = new Rectangle(new Point(1, 0), new Size(this.UpArrowImage.Width, this.UpArrowImage.Height));
			if (rectangle2.Contains(pt))
			{
				int num5 = this.Maximum - this.Minimum - this.LargeChange;
				int num6 = num - num3;
				if (num5 > 0 && num6 > 0)
				{
					if (this.moThumbTop - this.SmallChange < 0)
					{
						this.moThumbTop = 0;
					}
					else
					{
						this.moThumbTop -= this.SmallChange;
					}
					float num7 = (float)this.moThumbTop / (float)num6;
					float num8 = num7 * (float)(this.Maximum - this.LargeChange);
					this.moValue = (int)num8;
					if (this.ValueChanged != null)
					{
						this.ValueChanged(this, new EventArgs());
					}
					if (this.Scroll != null)
					{
						this.Scroll(this, new EventArgs());
					}
					base.Invalidate();
				}
			}
			Rectangle rectangle3 = new Rectangle(new Point(1, this.UpArrowImage.Height + num), new Size(this.UpArrowImage.Width, this.UpArrowImage.Height));
			if (rectangle3.Contains(pt))
			{
				int num9 = this.Maximum - this.Minimum - this.LargeChange;
				int num10 = num - num3;
				if (num9 > 0 && num10 > 0)
				{
					if (this.moThumbTop + this.SmallChange > num10)
					{
						this.moThumbTop = num10;
					}
					else
					{
						this.moThumbTop += this.SmallChange;
					}
					float num11 = (float)this.moThumbTop / (float)num10;
					float num12 = num11 * (float)(this.Maximum - this.LargeChange);
					this.moValue = (int)num12;
					if (this.ValueChanged != null)
					{
						this.ValueChanged(this, new EventArgs());
					}
					if (this.Scroll != null)
					{
						this.Scroll(this, new EventArgs());
					}
					base.Invalidate();
				}
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000F7B8 File Offset: 0x0000D9B8
		private void CustomScrollbar_MouseUp(object sender, MouseEventArgs e)
		{
			this.moThumbDown = false;
			this.moThumbDragging = false;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000F7C8 File Offset: 0x0000D9C8
		private void MoveThumb(int y)
		{
			int num = this.Maximum - this.Minimum;
			int num2 = base.Height - (this.UpArrowImage.Height + this.DownArrowImage.Height);
			float num3 = (float)this.LargeChange / (float)this.Maximum * (float)num2;
			int num4 = (int)num3;
			if (num4 > num2)
			{
				num4 = num2;
				num3 = (float)num2;
			}
			if (num4 < 56)
			{
				num4 = 56;
			}
			int num5 = this.nClickPoint;
			int num6 = num2 - num4;
			if (this.moThumbDown && num > 0 && num6 > 0)
			{
				int num7 = y - (this.UpArrowImage.Height + num5);
				if (num7 < 0)
				{
					this.moThumbTop = 0;
				}
				else if (num7 > num6)
				{
					this.moThumbTop = num6;
				}
				else
				{
					this.moThumbTop = y - (this.UpArrowImage.Height + num5);
				}
				float num8 = (float)this.moThumbTop / (float)num6;
				float num9 = num8 * (float)(this.Maximum - this.LargeChange);
				this.moValue = (int)num9;
				Application.DoEvents();
				base.Invalidate();
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000F8D8 File Offset: 0x0000DAD8
		private void CustomScrollbar_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.moThumbDown)
			{
				this.moThumbDragging = true;
			}
			if (this.moThumbDragging)
			{
				this.MoveThumb(e.Y);
			}
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
			if (this.Scroll != null)
			{
				this.Scroll(this, new EventArgs());
			}
		}

		// Token: 0x040000E2 RID: 226
		protected Color moChannelColor = Color.Empty;

		// Token: 0x040000E3 RID: 227
		protected Image moUpArrowImage;

		// Token: 0x040000E4 RID: 228
		protected Image moDownArrowImage;

		// Token: 0x040000E5 RID: 229
		protected Image moThumbArrowImage;

		// Token: 0x040000E6 RID: 230
		protected Image moThumbTopImage;

		// Token: 0x040000E7 RID: 231
		protected Image moThumbTopSpanImage;

		// Token: 0x040000E8 RID: 232
		protected Image moThumbBottomImage;

		// Token: 0x040000E9 RID: 233
		protected Image moThumbBottomSpanImage;

		// Token: 0x040000EA RID: 234
		protected Image moThumbMiddleImage;

		// Token: 0x040000EB RID: 235
		protected int moLargeChange = 10;

		// Token: 0x040000EC RID: 236
		protected int moSmallChange = 1;

		// Token: 0x040000ED RID: 237
		protected int moMinimum;

		// Token: 0x040000EE RID: 238
		protected int moMaximum = 100;

		// Token: 0x040000EF RID: 239
		protected int moValue;

		// Token: 0x040000F0 RID: 240
		private int nClickPoint;

		// Token: 0x040000F1 RID: 241
		protected int moThumbTop;

		// Token: 0x040000F2 RID: 242
		protected bool moAutoSize;

		// Token: 0x040000F3 RID: 243
		private bool moThumbDown;

		// Token: 0x040000F4 RID: 244
		private bool moThumbDragging;
	}
}
