using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PS3SaveEditor.CustomScrollbar;

namespace CustomControls
{
	// Token: 0x02000025 RID: 37
	public class CustomHScrollbar : UserControl
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000239 RID: 569 RVA: 0x0000E280 File Offset: 0x0000C480
		// (remove) Token: 0x0600023A RID: 570 RVA: 0x0000E2B8 File Offset: 0x0000C4B8
		public new event EventHandler Scroll;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600023B RID: 571 RVA: 0x0000E2F0 File Offset: 0x0000C4F0
		// (remove) Token: 0x0600023C RID: 572 RVA: 0x0000E328 File Offset: 0x0000C528
		public event EventHandler ValueChanged;

		// Token: 0x0600023D RID: 573 RVA: 0x0000E360 File Offset: 0x0000C560
		private int GetThumbWidth()
		{
			int num = base.Width - (this.LeftArrowImage.Width + this.RightArrowImage.Width);
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

		// Token: 0x0600023E RID: 574 RVA: 0x0000E3BC File Offset: 0x0000C5BC
		public CustomHScrollbar()
		{
			this.InitializeComponent();
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			this.moChannelColor = Color.FromArgb(51, 166, 3);
			this.LeftArrowImage = Resource.leftarrow;
			this.RightArrowImage = Resource.rightarrow;
			this.ThumbLeftImage = Resource.ThumbLeft;
			this.ThumbLeftSpanImage = Resource.ThumbSpanLeft;
			this.ThumbRightImage = Resource.ThumbRight;
			this.ThumbRightSpanImage = Resource.ThumbSpanRight;
			this.ThumbMiddleImage = Resource.ThumbMiddleH;
			base.Height = this.LeftArrowImage.Height;
			base.MinimumSize = new Size(this.LeftArrowImage.Width + this.RightArrowImage.Width + this.GetThumbWidth(), this.LeftArrowImage.Height);
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000E4BD File Offset: 0x0000C6BD
		// (set) Token: 0x06000240 RID: 576 RVA: 0x0000E4C5 File Offset: 0x0000C6C5
		[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Description("LargeChange"), Category("Behavior")]
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

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000E4D4 File Offset: 0x0000C6D4
		// (set) Token: 0x06000242 RID: 578 RVA: 0x0000E4DC File Offset: 0x0000C6DC
		[Description("SmallChange"), Category("Behavior"), EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false)]
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

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000E4EB File Offset: 0x0000C6EB
		// (set) Token: 0x06000244 RID: 580 RVA: 0x0000E4F3 File Offset: 0x0000C6F3
		[DefaultValue(false), Browsable(true), Category("Behavior"), Description("Minimum"), EditorBrowsable(EditorBrowsableState.Always)]
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000E502 File Offset: 0x0000C702
		// (set) Token: 0x06000246 RID: 582 RVA: 0x0000E50A File Offset: 0x0000C70A
		[Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(false), Category("Behavior"), Description("Maximum")]
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

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000E519 File Offset: 0x0000C719
		// (set) Token: 0x06000248 RID: 584 RVA: 0x0000E524 File Offset: 0x0000C724
		[DefaultValue(false), Browsable(true), Category("Behavior"), Description("Value"), EditorBrowsable(EditorBrowsableState.Always)]
		public int Value
		{
			get
			{
				return this.moValue;
			}
			set
			{
				this.moValue = value;
				int num = base.Width - (this.LeftArrowImage.Width + this.RightArrowImage.Width);
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
				this.moThumbRight = (int)num7;
				base.Invalidate();
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000E5CC File Offset: 0x0000C7CC
		// (set) Token: 0x0600024A RID: 586 RVA: 0x0000E5D4 File Offset: 0x0000C7D4
		[Description("Channel Color"), DefaultValue(false), Category("Skin"), EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
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

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000E5DD File Offset: 0x0000C7DD
		// (set) Token: 0x0600024C RID: 588 RVA: 0x0000E5E5 File Offset: 0x0000C7E5
		[Category("Skin"), Browsable(true), DefaultValue(false), EditorBrowsable(EditorBrowsableState.Always), Description("Up Arrow Graphic")]
		public Image LeftArrowImage
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

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000E5EE File Offset: 0x0000C7EE
		// (set) Token: 0x0600024E RID: 590 RVA: 0x0000E5F6 File Offset: 0x0000C7F6
		[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
		public Image RightArrowImage
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

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000E5FF File Offset: 0x0000C7FF
		// (set) Token: 0x06000250 RID: 592 RVA: 0x0000E607 File Offset: 0x0000C807
		[Category("Skin"), Browsable(true), DefaultValue(false), EditorBrowsable(EditorBrowsableState.Always), Description("Up Arrow Graphic")]
		public Image ThumbRightImage
		{
			get
			{
				return this.moThumbRightImage;
			}
			set
			{
				this.moThumbRightImage = value;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000E610 File Offset: 0x0000C810
		// (set) Token: 0x06000252 RID: 594 RVA: 0x0000E618 File Offset: 0x0000C818
		[DefaultValue(false), EditorBrowsable(EditorBrowsableState.Always), Description("Up Arrow Graphic"), Category("Skin"), Browsable(true)]
		public Image ThumbRightSpanImage
		{
			get
			{
				return this.moThumbRightSpanImage;
			}
			set
			{
				this.moThumbRightSpanImage = value;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000E621 File Offset: 0x0000C821
		// (set) Token: 0x06000254 RID: 596 RVA: 0x0000E629 File Offset: 0x0000C829
		[DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public Image ThumbLeftImage
		{
			get
			{
				return this.moThumbLeftImage;
			}
			set
			{
				this.moThumbLeftImage = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000E632 File Offset: 0x0000C832
		// (set) Token: 0x06000256 RID: 598 RVA: 0x0000E63A File Offset: 0x0000C83A
		[DefaultValue(false), EditorBrowsable(EditorBrowsableState.Always), Description("Up Arrow Graphic"), Category("Skin"), Browsable(true)]
		public Image ThumbLeftSpanImage
		{
			get
			{
				return this.moThumbLeftSpanImage;
			}
			set
			{
				this.moThumbLeftSpanImage = value;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000E643 File Offset: 0x0000C843
		// (set) Token: 0x06000258 RID: 600 RVA: 0x0000E64B File Offset: 0x0000C84B
		[DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
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

		// Token: 0x06000259 RID: 601 RVA: 0x0000E654 File Offset: 0x0000C854
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			if (this.LeftArrowImage != null)
			{
				e.Graphics.DrawImage(this.LeftArrowImage, new Rectangle(new Point(0, 0), new Size(this.LeftArrowImage.Width, base.Height)));
			}
			Brush brush = new SolidBrush(this.moChannelColor);
			Brush brush2 = new SolidBrush(Color.FromArgb(255, 255, 255));
			e.Graphics.FillRectangle(brush2, new Rectangle(this.LeftArrowImage.Width, 0, base.Width - this.RightArrowImage.Width, 1));
			e.Graphics.FillRectangle(brush2, new Rectangle(this.LeftArrowImage.Width, base.Height - 1, base.Width - this.RightArrowImage.Width, base.Height));
			e.Graphics.FillRectangle(brush, new Rectangle(this.LeftArrowImage.Width, 1, base.Width - this.RightArrowImage.Width, base.Height - 2));
			int num = base.Width - (this.LeftArrowImage.Width + this.RightArrowImage.Width);
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
			float num4 = (num2 - (float)(this.ThumbMiddleImage.Width + this.ThumbRightImage.Width + this.ThumbRightImage.Width)) / 2f;
			int num5 = (int)num4;
			int num6 = this.moThumbRight;
			num6 += this.LeftArrowImage.Width;
			e.Graphics.DrawImage(this.ThumbLeftImage, new Rectangle(num6, 1, this.ThumbLeftImage.Width, base.Height - 2));
			num6 += this.ThumbLeftImage.Width;
			Rectangle rect = new Rectangle(num6, 1, num5, base.Height - 2);
			e.Graphics.DrawImage(this.ThumbLeftSpanImage, (float)num6, 1f, num4 * 2f, (float)base.Height - 2f);
			num6 += num5;
			e.Graphics.DrawImage(this.ThumbMiddleImage, new Rectangle(num6, 1, this.ThumbMiddleImage.Width, base.Height - 2));
			num6 += this.ThumbMiddleImage.Width;
			rect = new Rectangle(num6, 1, num5 * 2, base.Height - 2);
			e.Graphics.DrawImage(this.ThumbRightSpanImage, rect);
			num6 += num5;
			e.Graphics.DrawImage(this.ThumbRightImage, new Rectangle(num6, 1, num5, base.Height - 2));
			if (this.RightArrowImage != null)
			{
				e.Graphics.DrawImage(this.RightArrowImage, new Rectangle(new Point(base.Width - this.RightArrowImage.Width, 0), new Size(this.RightArrowImage.Width, base.Height)));
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000E96E File Offset: 0x0000CB6E
		// (set) Token: 0x0600025B RID: 603 RVA: 0x0000E976 File Offset: 0x0000CB76
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

		// Token: 0x0600025C RID: 604 RVA: 0x0000E998 File Offset: 0x0000CB98
		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.Name = "CustomHScrollbar";
			base.MouseDown += new MouseEventHandler(this.CustomScrollbar_MouseDown);
			base.MouseMove += new MouseEventHandler(this.CustomScrollbar_MouseMove);
			base.MouseUp += new MouseEventHandler(this.CustomScrollbar_MouseUp);
			base.ResumeLayout(false);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000E9F4 File Offset: 0x0000CBF4
		private void CustomScrollbar_MouseDown(object sender, MouseEventArgs e)
		{
			Point pt = base.PointToClient(Cursor.Position);
			int num = base.Width - (this.LeftArrowImage.Width + this.RightArrowImage.Width);
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
			int num4 = this.moThumbRight;
			num4 += this.LeftArrowImage.Width;
			Rectangle rectangle = new Rectangle(new Point(num4, 1), new Size(num3, this.ThumbMiddleImage.Height));
			if (rectangle.Contains(pt))
			{
				this.nClickPoint = pt.Y - num4;
				this.moThumbDown = true;
			}
			Rectangle rectangle2 = new Rectangle(new Point(1, 0), new Size(this.LeftArrowImage.Width, this.LeftArrowImage.Height));
			if (rectangle2.Contains(pt))
			{
				int num5 = this.Maximum - this.Minimum - this.LargeChange;
				int num6 = num - num3;
				if (num5 > 0 && num6 > 0)
				{
					if (this.moThumbRight - this.SmallChange < 0)
					{
						this.moThumbRight = 0;
					}
					else
					{
						this.moThumbRight -= this.SmallChange;
					}
					float num7 = (float)this.moThumbRight / (float)num6;
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
			Rectangle rectangle3 = new Rectangle(new Point(this.LeftArrowImage.Width + num, 1), new Size(this.LeftArrowImage.Width, this.LeftArrowImage.Height));
			if (rectangle3.Contains(pt))
			{
				int num9 = this.Maximum - this.Minimum - this.LargeChange;
				int num10 = num - num3;
				if (num9 > 0 && num10 > 0)
				{
					if (this.moThumbRight + this.SmallChange > num10)
					{
						this.moThumbRight = num10;
					}
					else
					{
						this.moThumbRight += this.SmallChange;
					}
					float num11 = (float)this.moThumbRight / (float)num10;
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

		// Token: 0x0600025E RID: 606 RVA: 0x0000EC9C File Offset: 0x0000CE9C
		private void CustomScrollbar_MouseUp(object sender, MouseEventArgs e)
		{
			this.moThumbDown = false;
			this.moThumbDragging = false;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000ECAC File Offset: 0x0000CEAC
		private void MoveThumb(int x)
		{
			int num = this.Maximum - this.Minimum;
			int num2 = base.Width - (this.LeftArrowImage.Width + this.RightArrowImage.Width);
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
				int num7 = x - (this.LeftArrowImage.Width + num5);
				if (num7 < 0)
				{
					this.moThumbRight = 0;
				}
				else if (num7 > num6)
				{
					this.moThumbRight = num6;
				}
				else
				{
					this.moThumbRight = x - (this.LeftArrowImage.Width + num5);
				}
				float num8 = (float)this.moThumbRight / (float)num6;
				float num9 = num8 * (float)(this.Maximum - this.LargeChange);
				this.moValue = (int)num9;
				Application.DoEvents();
				base.Invalidate();
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000EDBC File Offset: 0x0000CFBC
		private void CustomScrollbar_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.moThumbDown)
			{
				this.moThumbDragging = true;
			}
			if (this.moThumbDragging)
			{
				this.MoveThumb(e.X);
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

		// Token: 0x040000CD RID: 205
		protected Color moChannelColor = Color.Empty;

		// Token: 0x040000CE RID: 206
		protected Image moUpArrowImage;

		// Token: 0x040000CF RID: 207
		protected Image moDownArrowImage;

		// Token: 0x040000D0 RID: 208
		protected Image moThumbArrowImage;

		// Token: 0x040000D1 RID: 209
		protected Image moThumbRightImage;

		// Token: 0x040000D2 RID: 210
		protected Image moThumbRightSpanImage;

		// Token: 0x040000D3 RID: 211
		protected Image moThumbLeftImage;

		// Token: 0x040000D4 RID: 212
		protected Image moThumbLeftSpanImage;

		// Token: 0x040000D5 RID: 213
		protected Image moThumbMiddleImage;

		// Token: 0x040000D6 RID: 214
		protected int moLargeChange = 10;

		// Token: 0x040000D7 RID: 215
		protected int moSmallChange = 1;

		// Token: 0x040000D8 RID: 216
		protected int moMinimum;

		// Token: 0x040000D9 RID: 217
		protected int moMaximum = 100;

		// Token: 0x040000DA RID: 218
		protected int moValue;

		// Token: 0x040000DB RID: 219
		private int nClickPoint;

		// Token: 0x040000DC RID: 220
		protected int moThumbRight;

		// Token: 0x040000DD RID: 221
		protected bool moAutoSize;

		// Token: 0x040000DE RID: 222
		private bool moThumbDown;

		// Token: 0x040000DF RID: 223
		private bool moThumbDragging;
	}
}
