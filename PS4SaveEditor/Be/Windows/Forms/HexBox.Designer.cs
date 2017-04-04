using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Be.Windows.Forms
{
	// Token: 0x02000051 RID: 81
	[ToolboxBitmap(typeof(HexBox), "HexBox.bmp")]
	public class HexBox : Control
	{
		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00012FE0 File Offset: 0x000111E0
		public long ScrollVMax
		{
			get
			{
				return this._scrollVmax;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00012FE8 File Offset: 0x000111E8
		public long ScrollVMin
		{
			get
			{
				return this._scrollVmin;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00012FF0 File Offset: 0x000111F0
		public long ScrollHMax
		{
			get
			{
				return this._scrollHmax;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00012FF8 File Offset: 0x000111F8
		public long ScrollHMin
		{
			get
			{
				return this._scrollHmin;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00013000 File Offset: 0x00011200
		public VScrollBar VScrollBar
		{
			get
			{
				return this._vScrollBar;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00013008 File Offset: 0x00011208
		public HScrollBar HScrollBar
		{
			get
			{
				return this._hScrollBar;
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060003A2 RID: 930 RVA: 0x00013010 File Offset: 0x00011210
		// (remove) Token: 0x060003A3 RID: 931 RVA: 0x00013048 File Offset: 0x00011248
		[Description("Occurs, when the value of InsertActive property has changed.")]
		public event EventHandler InsertActiveChanged;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060003A4 RID: 932 RVA: 0x00013080 File Offset: 0x00011280
		// (remove) Token: 0x060003A5 RID: 933 RVA: 0x000130B8 File Offset: 0x000112B8
		[Description("Occurs, when the value of ReadOnly property has changed.")]
		public event EventHandler ReadOnlyChanged;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060003A6 RID: 934 RVA: 0x000130F0 File Offset: 0x000112F0
		// (remove) Token: 0x060003A7 RID: 935 RVA: 0x00013128 File Offset: 0x00011328
		[Description("Occurs, when the value of ByteProvider property has changed.")]
		public event EventHandler ByteProviderChanged;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060003A8 RID: 936 RVA: 0x00013160 File Offset: 0x00011360
		// (remove) Token: 0x060003A9 RID: 937 RVA: 0x00013198 File Offset: 0x00011398
		[Description("Occurs, when the value of SelectionStart property has changed.")]
		public event EventHandler SelectionStartChanged;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060003AA RID: 938 RVA: 0x000131D0 File Offset: 0x000113D0
		// (remove) Token: 0x060003AB RID: 939 RVA: 0x00013208 File Offset: 0x00011408
		public event EventHandler VScroll;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060003AC RID: 940 RVA: 0x00013240 File Offset: 0x00011440
		// (remove) Token: 0x060003AD RID: 941 RVA: 0x00013278 File Offset: 0x00011478
		public event EventHandler HScroll;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060003AE RID: 942 RVA: 0x000132B0 File Offset: 0x000114B0
		// (remove) Token: 0x060003AF RID: 943 RVA: 0x000132E8 File Offset: 0x000114E8
		[Description("Occurs, when the value of SelectionLength property has changed.")]
		public event EventHandler SelectionLengthChanged;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060003B0 RID: 944 RVA: 0x00013320 File Offset: 0x00011520
		// (remove) Token: 0x060003B1 RID: 945 RVA: 0x00013358 File Offset: 0x00011558
		[Description("Occurs, when the value of LineInfoVisible property has changed.")]
		public event EventHandler LineInfoVisibleChanged;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060003B2 RID: 946 RVA: 0x00013390 File Offset: 0x00011590
		// (remove) Token: 0x060003B3 RID: 947 RVA: 0x000133C8 File Offset: 0x000115C8
		[Description("Occurs, when the value of StringViewVisible property has changed.")]
		public event EventHandler StringViewVisibleChanged;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060003B4 RID: 948 RVA: 0x00013400 File Offset: 0x00011600
		// (remove) Token: 0x060003B5 RID: 949 RVA: 0x00013438 File Offset: 0x00011638
		[Description("Occurs, when the value of BorderStyle property has changed.")]
		public event EventHandler BorderStyleChanged;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060003B6 RID: 950 RVA: 0x00013470 File Offset: 0x00011670
		// (remove) Token: 0x060003B7 RID: 951 RVA: 0x000134A8 File Offset: 0x000116A8
		[Description("Occurs, when the value of BytesPerLine property has changed.")]
		public event EventHandler BytesPerLineChanged;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060003B8 RID: 952 RVA: 0x000134E0 File Offset: 0x000116E0
		// (remove) Token: 0x060003B9 RID: 953 RVA: 0x00013518 File Offset: 0x00011718
		[Description("Occurs, when the value of UseFixedBytesPerLine property has changed.")]
		public event EventHandler UseFixedBytesPerLineChanged;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060003BA RID: 954 RVA: 0x00013550 File Offset: 0x00011750
		// (remove) Token: 0x060003BB RID: 955 RVA: 0x00013588 File Offset: 0x00011788
		[Description("Occurs, when the value of VScrollBarVisible property has changed.")]
		public event EventHandler VScrollBarVisibleChanged;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060003BC RID: 956 RVA: 0x000135C0 File Offset: 0x000117C0
		// (remove) Token: 0x060003BD RID: 957 RVA: 0x000135F8 File Offset: 0x000117F8
		public event EventHandler HScrollBarVisibleChanged;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060003BE RID: 958 RVA: 0x00013630 File Offset: 0x00011830
		// (remove) Token: 0x060003BF RID: 959 RVA: 0x00013668 File Offset: 0x00011868
		[Description("Occurs, when the value of HexCasing property has changed.")]
		public event EventHandler HexCasingChanged;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x060003C0 RID: 960 RVA: 0x000136A0 File Offset: 0x000118A0
		// (remove) Token: 0x060003C1 RID: 961 RVA: 0x000136D8 File Offset: 0x000118D8
		[Description("Occurs, when the value of HorizontalByteCount property has changed.")]
		public event EventHandler HorizontalByteCountChanged;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x060003C2 RID: 962 RVA: 0x00013710 File Offset: 0x00011910
		// (remove) Token: 0x060003C3 RID: 963 RVA: 0x00013748 File Offset: 0x00011948
		[Description("Occurs, when the value of VerticalByteCount property has changed.")]
		public event EventHandler VerticalByteCountChanged;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060003C4 RID: 964 RVA: 0x00013780 File Offset: 0x00011980
		// (remove) Token: 0x060003C5 RID: 965 RVA: 0x000137B8 File Offset: 0x000119B8
		[Description("Occurs, when the value of CurrentLine property has changed.")]
		public event EventHandler CurrentLineChanged;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060003C6 RID: 966 RVA: 0x000137F0 File Offset: 0x000119F0
		// (remove) Token: 0x060003C7 RID: 967 RVA: 0x00013828 File Offset: 0x00011A28
		[Description("Occurs, when the value of CurrentPositionInLine property has changed.")]
		public event EventHandler CurrentPositionInLineChanged;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x060003C8 RID: 968 RVA: 0x00013860 File Offset: 0x00011A60
		// (remove) Token: 0x060003C9 RID: 969 RVA: 0x00013898 File Offset: 0x00011A98
		[Description("Occurs, when Copy method was invoked and ClipBoardData changed.")]
		public event EventHandler Copied;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x060003CA RID: 970 RVA: 0x000138D0 File Offset: 0x00011AD0
		// (remove) Token: 0x060003CB RID: 971 RVA: 0x00013908 File Offset: 0x00011B08
		[Description("Occurs, when CopyHex method was invoked and ClipBoardData changed.")]
		public event EventHandler CopiedHex;

		// Token: 0x060003CC RID: 972 RVA: 0x00013940 File Offset: 0x00011B40
		public HexBox()
		{
			this.SelectAddresses = new Dictionary<long, byte>();
			this._vScrollBar = new VScrollBar();
			this._vScrollBar.Scroll += new ScrollEventHandler(this._vScrollBar_Scroll);
			this._hScrollBar = new HScrollBar();
			this._hScrollBar.Scroll += new ScrollEventHandler(this._hScrollBar_Scroll);
			this.tip = new ToolTip();
			this.tip.ReshowDelay = 0;
			this.tip.Disposed += new EventHandler(this.tip_Disposed);
			this._builtInContextMenu = null;
			this.BackColor = Color.White;
			this.Font = new Font("Courier New", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this._stringFormat = new StringFormat(StringFormat.GenericTypographic);
			this._stringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
			this.ActivateEmptyKeyInterpreter();
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			this._thumbTrackTimer.Interval = 50;
			this._thumbTrackTimer.Tick += new EventHandler(this.PerformScrollThumbTrack);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00013B4C File Offset: 0x00011D4C
		private void _hScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			switch (e.Type)
			{
			case ScrollEventType.SmallDecrement:
			case ScrollEventType.SmallIncrement:
			case ScrollEventType.LargeDecrement:
			case ScrollEventType.LargeIncrement:
			{
				long pos = this.FromHScrollPos(e.NewValue);
				this.PerformHScrollThumpPosition(pos);
				break;
			}
			case ScrollEventType.ThumbPosition:
			{
				long pos2 = this.FromHScrollPos(e.NewValue);
				this.PerformHScrollThumpPosition(pos2);
				break;
			}
			case ScrollEventType.ThumbTrack:
			{
				long pos3 = this.FromHScrollPos(e.NewValue);
				this.PerformHScrollThumpPosition(pos3);
				break;
			}
			}
			e.NewValue = this.ToHScrollPos(this._scrollHpos);
			this.OnHScroll(EventArgs.Empty);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00013BE1 File Offset: 0x00011DE1
		private void tip_Disposed(object sender, EventArgs e)
		{
			this.m_bTipVisible = false;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00013BEC File Offset: 0x00011DEC
		private void _vScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			switch (e.Type)
			{
			case ScrollEventType.SmallDecrement:
				this.PerformScrollLineUp();
				break;
			case ScrollEventType.SmallIncrement:
				this.PerformScrollLineDown();
				break;
			case ScrollEventType.LargeDecrement:
				this.PerformScrollPageUp();
				break;
			case ScrollEventType.LargeIncrement:
				this.PerformScrollPageDown();
				break;
			case ScrollEventType.ThumbPosition:
			{
				long pos = this.FromVScrollPos(e.NewValue);
				this.PerformVScrollThumpPosition(pos);
				break;
			}
			case ScrollEventType.ThumbTrack:
			{
				if (this._thumbTrackTimer.Enabled)
				{
					this._thumbTrackTimer.Enabled = false;
				}
				int tickCount = Environment.TickCount;
				if (tickCount - this._lastThumbtrack > 50)
				{
					this.PerformScrollThumbTrack(null, null);
					this._lastThumbtrack = tickCount;
				}
				else
				{
					this._thumbTrackPosition = this.FromVScrollPos(e.NewValue);
					this._thumbTrackTimer.Enabled = true;
				}
				break;
			}
			}
			e.NewValue = this.ToVScrollPos(this._scrollVpos);
			this.OnScroll(EventArgs.Empty);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00013CE2 File Offset: 0x00011EE2
		private void PerformScrollThumbTrack(object sender, EventArgs e)
		{
			this._thumbTrackTimer.Enabled = false;
			this.PerformVScrollThumpPosition(this._thumbTrackPosition);
			this._lastThumbtrack = Environment.TickCount;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00013D08 File Offset: 0x00011F08
		private void UpdateVScrollSize()
		{
			if (!this.VScrollBarVisible || this._byteProvider == null || this._byteProvider.Length <= 0L || this._iHexMaxHBytes == 0)
			{
				if (this.VScrollBarVisible)
				{
					this._scrollVmin = 0L;
					this._scrollVmax = 0L;
					this._scrollVpos = 0L;
					this.UpdateVScroll();
				}
				return;
			}
			long num = (long)Math.Ceiling((double)(this._byteProvider.Length + 1L) / (double)this._iHexMaxHBytes - (double)this._iHexMaxVBytes);
			num = Math.Max(0L, num);
			long num2 = this._startByte / (long)this._iHexMaxHBytes;
			if (num < this._scrollVmax && this._scrollVpos == this._scrollVmax)
			{
				this.PerformScrollLineUp();
			}
			if (num == this._scrollVmax && num2 == this._scrollVpos)
			{
				return;
			}
			this._scrollVmin = 0L;
			this._scrollVmax = num;
			this._scrollVpos = Math.Min(num2, num);
			this.UpdateVScroll();
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00013E00 File Offset: 0x00012000
		private void UpdateHScrollSize()
		{
			if (!this.HScrollBarVisible || this._byteProvider == null || this._byteProvider.Length <= 0L || this._iHexMaxHBytes == 0)
			{
				if (this.HScrollBarVisible)
				{
					this._scrollHmin = 0L;
					this._scrollHmax = 0L;
					this._scrollHpos = 0L;
					this.UpdateHScroll();
				}
				return;
			}
			long num = (long)(this._recHex.Width + (this.StringViewVisible ? this._recStringView.Width : 0) + (this.LineInfoVisible ? this._recLineInfo.Width : 0) - this._recContent.Width + 15);
			num = Math.Max(0L, num);
			long num2 = 0L;
			long arg_90_0 = this._scrollHmax;
			if (num == this._scrollHmax && num2 == this._scrollHpos)
			{
				return;
			}
			this._scrollHmin = 0L;
			this._scrollHmax = num;
			this._scrollHpos = Math.Min(num2, num);
			this.UpdateHScroll();
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00013EFC File Offset: 0x000120FC
		private void UpdateVScroll()
		{
			int num = this.ToScrollMax(this._scrollVmax);
			if (num > 0)
			{
				this._vScrollBar.Minimum = 0;
				this._vScrollBar.Maximum = num;
				this._vScrollBar.Value = this.ToVScrollPos(this._scrollVpos);
				this._vScrollBar.Enabled = true;
				return;
			}
			this._vScrollBar.Enabled = false;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00013F64 File Offset: 0x00012164
		private void UpdateHScroll()
		{
			int num = this.ToHScrollMax(this._scrollHmax);
			if (num > 0)
			{
				this._hScrollBar.Minimum = 0;
				this._hScrollBar.Maximum = num;
				this._hScrollBar.Value = this.ToHScrollPos(this._scrollHpos);
				this._hScrollBar.Enabled = true;
				return;
			}
			this._hScrollBar.Enabled = false;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00013FCC File Offset: 0x000121CC
		private int ToHScrollPos(long value)
		{
			int num = 150;
			if (this._scrollHmax < (long)num)
			{
				return (int)value;
			}
			double num2 = (double)value / (double)this._scrollHmax * 100.0;
			int num3 = (int)Math.Floor((double)num / 100.0 * num2);
			num3 = (int)Math.Max(this._scrollHmin, (long)num3);
			return (int)Math.Min(this._scrollHmax, (long)num3);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00014038 File Offset: 0x00012238
		private int ToVScrollPos(long value)
		{
			int num = 65535;
			if (this._scrollVmax < (long)num)
			{
				return (int)value;
			}
			double num2 = (double)value / (double)this._scrollVmax * 100.0;
			int num3 = (int)Math.Floor((double)num / 100.0 * num2);
			num3 = (int)Math.Max(this._scrollVmin, (long)num3);
			return (int)Math.Min(this._scrollVmax, (long)num3);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000140A4 File Offset: 0x000122A4
		private long FromVScrollPos(int value)
		{
			int num = 65535;
			if (this._scrollVmax < (long)num)
			{
				return (long)value;
			}
			double num2 = (double)value / (double)num * 100.0;
			return (long)((int)Math.Floor((double)this._scrollVmax / 100.0 * num2));
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000140F0 File Offset: 0x000122F0
		private long FromHScrollPos(int value)
		{
			int num = 150;
			if (this._scrollHmax < (long)num)
			{
				return (long)value;
			}
			double num2 = (double)value / (double)num * 100.0;
			return (long)((int)Math.Floor((double)this._scrollHmax / 100.0 * num2));
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001413C File Offset: 0x0001233C
		private int ToScrollMax(long value)
		{
			long num = 65535L;
			if (value > num)
			{
				return (int)num;
			}
			return (int)value;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001415C File Offset: 0x0001235C
		private int ToHScrollMax(long value)
		{
			long num = 150L;
			if (value > num)
			{
				return (int)num;
			}
			return (int)value;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00014179 File Offset: 0x00012379
		public void PerformScrollToLine(long pos)
		{
			if (pos < this._scrollVmin || pos > this._scrollVmax || pos == this._scrollVpos)
			{
				return;
			}
			this._scrollVpos = pos;
			this.UpdateVScroll();
			this.UpdateVisibilityBytes();
			this.UpdateCaret();
			base.Invalidate();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000141B6 File Offset: 0x000123B6
		public void PerformHScroll(long pos)
		{
			if (pos < this._scrollHmin || pos > this._scrollHmax || pos == this._scrollHpos)
			{
				return;
			}
			this._scrollHpos = pos;
			this.UpdateHScroll();
			this.UpdateVisibilityBytes();
			this.UpdateCaret();
			base.Invalidate();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000141F4 File Offset: 0x000123F4
		private void PerformScrollLines(int lines)
		{
			long pos;
			if (lines > 0)
			{
				pos = Math.Min(this._scrollVmax, this._scrollVpos + (long)lines);
			}
			else
			{
				if (lines >= 0)
				{
					return;
				}
				pos = Math.Max(this._scrollVmin, this._scrollVpos + (long)lines);
			}
			this.PerformScrollToLine(pos);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0001423F File Offset: 0x0001243F
		private void PerformScrollLineDown()
		{
			this.PerformScrollLines(1);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00014248 File Offset: 0x00012448
		private void PerformScrollLineUp()
		{
			this.PerformScrollLines(-1);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00014251 File Offset: 0x00012451
		private void PerformScrollPageDown()
		{
			this.PerformScrollLines(this._iHexMaxVBytes);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001425F File Offset: 0x0001245F
		private void PerformScrollPageUp()
		{
			this.PerformScrollLines(-this._iHexMaxVBytes);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00014270 File Offset: 0x00012470
		private void PerformVScrollThumpPosition(long pos)
		{
			int num = (this._scrollVmax > 65535L) ? 10 : 9;
			if (this.ToVScrollPos(pos) == this.ToScrollMax(this._scrollVmax) - num)
			{
				pos = this._scrollVmax;
			}
			this.PerformScrollToLine(pos);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000142B8 File Offset: 0x000124B8
		private void PerformHScrollThumpPosition(long pos)
		{
			if (this.ToHScrollPos(pos) == this.ToHScrollMax(this._scrollHmax))
			{
				pos = this._scrollHmax;
			}
			this.PerformHScroll(pos);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000142DE File Offset: 0x000124DE
		public void ScrollByteIntoView()
		{
			this.ScrollByteIntoView(this._bytePos);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000142EC File Offset: 0x000124EC
		public void ScrollByteIntoView(long index)
		{
			if (this._byteProvider == null || this._keyInterpreter == null)
			{
				return;
			}
			if (index < this._startByte)
			{
				long pos = (long)Math.Floor((double)index / (double)this._iHexMaxHBytes);
				this.PerformVScrollThumpPosition(pos);
				return;
			}
			if (index > this._endByte)
			{
				long num = (long)Math.Floor((double)index / (double)this._iHexMaxHBytes);
				num -= (long)(this._iHexMaxVBytes - 1);
				this.PerformVScrollThumpPosition(num);
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00014359 File Offset: 0x00012559
		private void ReleaseSelection()
		{
			if (this._selectionLength == 0L)
			{
				return;
			}
			this._selectionLength = 0L;
			this.OnSelectionLengthChanged(EventArgs.Empty);
			if (!this._caretVisible)
			{
				this.CreateCaret();
			}
			else
			{
				this.UpdateCaret();
			}
			base.Invalidate();
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00014395 File Offset: 0x00012595
		public bool CanSelectAll()
		{
			return base.Enabled && this._byteProvider != null;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000143AC File Offset: 0x000125AC
		public void SelectAll()
		{
			if (this.ByteProvider == null)
			{
				return;
			}
			this.Select(0L, this.ByteProvider.Length);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x000143CA File Offset: 0x000125CA
		public void Select(long start, long length)
		{
			if (this.ByteProvider == null)
			{
				return;
			}
			if (!base.Enabled)
			{
				return;
			}
			this.InternalSelect(start, length);
			this.ScrollByteIntoView();
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x000143EC File Offset: 0x000125EC
		private void InternalSelect(long start, long length)
		{
			int byteCharacterPos = 0;
			if (length > 0L && this._caretVisible)
			{
				this.DestroyCaret();
			}
			else if (length == 0L && !this._caretVisible)
			{
				this.CreateCaret();
			}
			this.SetPosition(start, byteCharacterPos);
			this.SetSelectionLength(length);
			this.UpdateCaret();
			base.Invalidate();
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00014444 File Offset: 0x00012644
		private void ActivateEmptyKeyInterpreter()
		{
			if (this._eki == null)
			{
				this._eki = new HexBox.EmptyKeyInterpreter(this);
			}
			if (this._eki == this._keyInterpreter)
			{
				return;
			}
			if (this._keyInterpreter != null)
			{
				this._keyInterpreter.Deactivate();
			}
			this._keyInterpreter = this._eki;
			this._keyInterpreter.Activate();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x000144A0 File Offset: 0x000126A0
		private void ActivateKeyInterpreter()
		{
			if (this._ki == null)
			{
				this._ki = new HexBox.KeyInterpreter(this);
			}
			if (this._ki == this._keyInterpreter)
			{
				return;
			}
			if (this._keyInterpreter != null)
			{
				this._keyInterpreter.Deactivate();
			}
			this._keyInterpreter = this._ki;
			this._keyInterpreter.Activate();
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x000144FC File Offset: 0x000126FC
		private void ActivateStringKeyInterpreter()
		{
			if (this._ski == null)
			{
				this._ski = new HexBox.StringKeyInterpreter(this);
			}
			if (this._ski == this._keyInterpreter)
			{
				return;
			}
			if (this._keyInterpreter != null)
			{
				this._keyInterpreter.Deactivate();
			}
			this._keyInterpreter = this._ski;
			this._keyInterpreter.Activate();
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00014558 File Offset: 0x00012758
		private void CreateCaret()
		{
			if (this._byteProvider == null || this._keyInterpreter == null || this._caretVisible || !this.Focused)
			{
				return;
			}
			int nWidth = this.InsertActive ? 1 : ((int)this._charSize.Width);
			int nHeight = (int)this._charSize.Height;
			NativeMethods.CreateCaret(base.Handle, this.m_hCaret, nWidth, nHeight);
			this.UpdateCaret();
			NativeMethods.ShowCaret(base.Handle);
			this._caretVisible = true;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x000145D8 File Offset: 0x000127D8
		private void UpdateCaret()
		{
			if (this._byteProvider == null || this._keyInterpreter == null)
			{
				return;
			}
			long byteIndex = this._bytePos - this._startByte;
			PointF caretPointF = this._keyInterpreter.GetCaretPointF(byteIndex);
			caretPointF.X += (float)this._byteCharacterPos * this._charSize.Width;
			NativeMethods.SetCaretPos((int)caretPointF.X, (int)caretPointF.Y);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00014648 File Offset: 0x00012848
		private void DestroyCaret()
		{
			if (!this._caretVisible)
			{
				return;
			}
			NativeMethods.DestroyCaret();
			this._caretVisible = false;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00014660 File Offset: 0x00012860
		private void SetCaretPosition(Point p)
		{
			if (this._byteProvider == null || this._keyInterpreter == null)
			{
				return;
			}
			long bytePos = this._bytePos;
			int byteCharacterPos = this._byteCharacterPos;
			if (this._recHex.Contains(p))
			{
				BytePositionInfo hexBytePositionInfo = this.GetHexBytePositionInfo(p);
				if (hexBytePositionInfo.Index - (long)hexBytePositionInfo.CharacterPosition < this._byteProvider.Length)
				{
					bytePos = hexBytePositionInfo.Index;
					byteCharacterPos = hexBytePositionInfo.CharacterPosition;
					this.SetPosition(bytePos, byteCharacterPos);
					this.ActivateKeyInterpreter();
					this.UpdateCaret();
					base.Invalidate();
					return;
				}
			}
			else if (this._recStringView.Contains(p))
			{
				BytePositionInfo stringBytePositionInfo = this.GetStringBytePositionInfo(p);
				if (stringBytePositionInfo.Index - (long)stringBytePositionInfo.CharacterPosition < this._byteProvider.Length)
				{
					bytePos = stringBytePositionInfo.Index;
					byteCharacterPos = stringBytePositionInfo.CharacterPosition;
					this.SetPosition(bytePos, byteCharacterPos);
					this.ActivateStringKeyInterpreter();
					this.UpdateCaret();
					base.Invalidate();
				}
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0001474C File Offset: 0x0001294C
		private BytePositionInfo GetHexBytePositionInfo(Point p)
		{
			float num = (float)(p.X - this._recHex.X) / this._charSize.Width;
			float num2 = (float)(p.Y - this._recHex.Y) / this._charSize.Height;
			int num3 = (int)num;
			int num4 = (int)num2;
			int num5 = num3 / 3 + 1;
			long num6 = Math.Min(this._byteProvider.Length, this._startByte + (long)(this._iHexMaxHBytes * (num4 + 1) - this._iHexMaxHBytes) + (long)num5 - 1L);
			int num7 = num3 % 3;
			if (num7 > 1)
			{
				num7 = 1;
			}
			if (num6 == this._byteProvider.Length)
			{
				num7 = 0;
			}
			if (num6 < 0L)
			{
				return new BytePositionInfo(0L, 0);
			}
			return new BytePositionInfo(num6, num7);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00014810 File Offset: 0x00012A10
		private BytePositionInfo GetStringBytePositionInfo(Point p)
		{
			float num = (float)(p.X - this._recStringView.X) / this._charSize.Width;
			float num2 = (float)(p.Y - this._recStringView.Y) / this._charSize.Height;
			int num3 = (int)num;
			int num4 = (int)num2;
			int num5 = num3 + 1;
			long num6 = Math.Min(this._byteProvider.Length, this._startByte + (long)(this._iHexMaxHBytes * (num4 + 1) - this._iHexMaxHBytes) + (long)num5 - 1L);
			int characterPosition = 0;
			if (num6 < 0L)
			{
				return new BytePositionInfo(0L, 0);
			}
			return new BytePositionInfo(num6, characterPosition);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000148B8 File Offset: 0x00012AB8
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
		public override bool PreProcessMessage(ref Message m)
		{
			switch (m.Msg)
			{
			case 256:
				return this._keyInterpreter.PreProcessWmKeyDown(ref m);
			case 257:
				return this._keyInterpreter.PreProcessWmKeyUp(ref m);
			case 258:
				return this._keyInterpreter.PreProcessWmChar(ref m);
			default:
				return base.PreProcessMessage(ref m);
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00014914 File Offset: 0x00012B14
		private bool BasePreProcessMessage(ref Message m)
		{
			return base.PreProcessMessage(ref m);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00014920 File Offset: 0x00012B20
		public long Find(byte[] bytes, long startIndex)
		{
			int num = 0;
			int num2 = bytes.Length;
			this._abortFind = false;
			for (long num3 = startIndex; num3 < this._byteProvider.Length; num3 += 1L)
			{
				if (this._abortFind)
				{
					return -2L;
				}
				if (num3 % 1000L == 0L)
				{
					Application.DoEvents();
				}
				if (this._byteProvider.ReadByte(num3) != bytes[num])
				{
					num3 -= (long)num;
					num = 0;
					this._findingPos = num3;
				}
				else
				{
					num++;
					if (num == num2)
					{
						long num4 = num3 - (long)num2 + 1L;
						this.Select(num4, (long)num2);
						this.ScrollByteIntoView(this._bytePos + this._selectionLength);
						this.ScrollByteIntoView(this._bytePos);
						return num4;
					}
				}
			}
			return -1L;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x000149CE File Offset: 0x00012BCE
		public void AbortFind()
		{
			this._abortFind = true;
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x000149D7 File Offset: 0x00012BD7
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public long CurrentFindingPosition
		{
			get
			{
				return this._findingPos;
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x000149E0 File Offset: 0x00012BE0
		private byte[] GetCopyData()
		{
			if (!this.CanCopy())
			{
				return new byte[0];
			}
			byte[] array = new byte[this._selectionLength];
			int num = -1;
			for (long num2 = this._bytePos; num2 < this._bytePos + this._selectionLength; num2 += 1L)
			{
				num++;
				array[num] = this._byteProvider.ReadByte(num2);
			}
			return array;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00014A40 File Offset: 0x00012C40
		public void Copy()
		{
			if (!this.CanCopy())
			{
				return;
			}
			byte[] copyData = this.GetCopyData();
			DataObject dataObject = new DataObject();
			string @string = Encoding.ASCII.GetString(copyData, 0, copyData.Length);
			dataObject.SetData(typeof(string), @string);
			MemoryStream data = new MemoryStream(copyData, 0, copyData.Length, false, true);
			dataObject.SetData("BinaryData", data);
			Clipboard.SetDataObject(dataObject, true);
			this.UpdateCaret();
			this.ScrollByteIntoView();
			base.Invalidate();
			this.OnCopied(EventArgs.Empty);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00014AC1 File Offset: 0x00012CC1
		public bool CanCopy()
		{
			return this._selectionLength >= 1L && this._byteProvider != null;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00014AD8 File Offset: 0x00012CD8
		public void Cut()
		{
			if (!this.CanCut())
			{
				return;
			}
			this.Copy();
			this._byteProvider.DeleteBytes(this._bytePos, this._selectionLength);
			this._byteCharacterPos = 0;
			this.UpdateCaret();
			this.ScrollByteIntoView();
			this.ReleaseSelection();
			base.Invalidate();
			this.Refresh();
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00014B30 File Offset: 0x00012D30
		public bool CanCut()
		{
			return !this.ReadOnly && base.Enabled && this._byteProvider != null && this._selectionLength >= 1L && this._byteProvider.SupportsDeleteBytes();
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00014B68 File Offset: 0x00012D68
		public void Paste()
		{
			if (!this.CanPaste())
			{
				return;
			}
			if (this._selectionLength > 0L)
			{
				this._byteProvider.DeleteBytes(this._bytePos, this._selectionLength);
			}
			IDataObject dataObject = Clipboard.GetDataObject();
			byte[] array;
			if (dataObject.GetDataPresent("BinaryData"))
			{
				MemoryStream memoryStream = (MemoryStream)dataObject.GetData("BinaryData");
				array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, array.Length);
			}
			else
			{
				if (!dataObject.GetDataPresent(typeof(string)))
				{
					return;
				}
				string s = (string)dataObject.GetData(typeof(string));
				array = Encoding.ASCII.GetBytes(s);
			}
			this._byteProvider.InsertBytes(this._bytePos, array);
			this.SetPosition(this._bytePos + (long)array.Length, 0);
			this.ReleaseSelection();
			this.ScrollByteIntoView();
			this.UpdateCaret();
			base.Invalidate();
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00014C54 File Offset: 0x00012E54
		public bool CanPaste()
		{
			if (this.ReadOnly || !base.Enabled)
			{
				return false;
			}
			if (this._byteProvider == null || !this._byteProvider.SupportsInsertBytes())
			{
				return false;
			}
			if (!this._byteProvider.SupportsDeleteBytes() && this._selectionLength > 0L)
			{
				return false;
			}
			IDataObject dataObject = Clipboard.GetDataObject();
			return dataObject.GetDataPresent("BinaryData") || dataObject.GetDataPresent(typeof(string));
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00014CD0 File Offset: 0x00012ED0
		public bool CanPasteHex()
		{
			if (!this.CanPaste())
			{
				return false;
			}
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject.GetDataPresent(typeof(string)))
			{
				string hex = (string)dataObject.GetData(typeof(string));
				byte[] array = this.ConvertHexToBytes(hex);
				return array != null;
			}
			return false;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00014D28 File Offset: 0x00012F28
		public void PasteHex()
		{
			if (!this.CanPaste())
			{
				return;
			}
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject.GetDataPresent(typeof(string)))
			{
				string hex = (string)dataObject.GetData(typeof(string));
				byte[] array = this.ConvertHexToBytes(hex);
				if (array != null)
				{
					if (this._selectionLength > 0L)
					{
						this._byteProvider.DeleteBytes(this._bytePos, this._selectionLength);
					}
					this._byteProvider.InsertBytes(this._bytePos, array);
					this.SetPosition(this._bytePos + (long)array.Length, 0);
					this.ReleaseSelection();
					this.ScrollByteIntoView();
					this.UpdateCaret();
					base.Invalidate();
					return;
				}
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00014DD8 File Offset: 0x00012FD8
		public void CopyHex()
		{
			if (!this.CanCopy())
			{
				return;
			}
			byte[] copyData = this.GetCopyData();
			DataObject dataObject = new DataObject();
			string data = this.ConvertBytesToHex(copyData);
			dataObject.SetData(typeof(string), data);
			MemoryStream data2 = new MemoryStream(copyData, 0, copyData.Length, false, true);
			dataObject.SetData("BinaryData", data2);
			Clipboard.SetDataObject(dataObject, true);
			this.UpdateCaret();
			this.ScrollByteIntoView();
			base.Invalidate();
			this.OnCopiedHex(EventArgs.Empty);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00014E54 File Offset: 0x00013054
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			switch (this._borderStyle)
			{
			case BorderStyle.FixedSingle:
				e.Graphics.FillRectangle(new SolidBrush(this.BackColor), base.ClientRectangle);
				ControlPaint.DrawBorder(e.Graphics, base.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
				return;
			case BorderStyle.Fixed3D:
				if (TextBoxRenderer.IsSupported)
				{
					VisualStyleElement element = VisualStyleElement.TextBox.TextEdit.Normal;
					Color color = this.BackColor;
					if (base.Enabled)
					{
						if (this.ReadOnly)
						{
							element = VisualStyleElement.TextBox.TextEdit.ReadOnly;
						}
						else if (this.Focused)
						{
							element = VisualStyleElement.TextBox.TextEdit.Focused;
						}
					}
					else
					{
						element = VisualStyleElement.TextBox.TextEdit.Disabled;
						color = this.BackColorDisabled;
					}
					VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(element);
					visualStyleRenderer.DrawBackground(e.Graphics, base.ClientRectangle);
					Rectangle backgroundContentRectangle = visualStyleRenderer.GetBackgroundContentRectangle(e.Graphics, base.ClientRectangle);
					e.Graphics.FillRectangle(new SolidBrush(color), backgroundContentRectangle);
					return;
				}
				e.Graphics.FillRectangle(new SolidBrush(this.BackColor), base.ClientRectangle);
				ControlPaint.DrawBorder3D(e.Graphics, base.ClientRectangle, Border3DStyle.Sunken);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00014F6C File Offset: 0x0001316C
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (this._byteProvider == null)
			{
				return;
			}
			Region region = new Region(base.ClientRectangle);
			region.Exclude(this._recContent);
			e.Graphics.ExcludeClip(region);
			this.UpdateVisibilityBytes();
			if (this._lineInfoVisible)
			{
				this.PaintLineInfo(e.Graphics, this._startByte, this._endByte);
			}
			if (!this._stringViewVisible)
			{
				this.PaintHex(e.Graphics, this._startByte, this._endByte);
				return;
			}
			if (this._shadowSelectionVisible)
			{
				this.PaintCurrentBytesSign(e.Graphics);
			}
			this.PaintHexAndStringView(e.Graphics, this._startByte, this._endByte);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00015020 File Offset: 0x00013220
		private void PaintLineInfo(Graphics g, long startByte, long endByte)
		{
			endByte = Math.Min(this._byteProvider.Length - 1L, endByte);
			Color color = (this.LineInfoForeColor != Color.Empty) ? this.LineInfoForeColor : this.ForeColor;
			Brush brush = new SolidBrush(color);
			int num = this.GetGridBytePoint(endByte - startByte).Y + 1;
			for (int i = 0; i < num; i++)
			{
				long num2 = startByte + (long)(this._iHexMaxHBytes * i) + this._lineInfoOffset;
				PointF bytePointF = this.GetBytePointF(new Point(0, i));
				string text = num2.ToString(this._hexStringFormat, Thread.CurrentThread.CurrentCulture);
				int num3 = 8 - text.Length;
				string s;
				if (num3 > -1)
				{
					s = new string('0', 8 - text.Length) + text;
				}
				else
				{
					s = new string('~', 8);
				}
				g.DrawString(s, this.Font, brush, new PointF((float)((long)this._recLineInfo.X - this._scrollHpos), bytePointF.Y), this._stringFormat);
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00015138 File Offset: 0x00013338
		private void PaintHex(Graphics g, long startByte, long endByte)
		{
			Brush brush = new SolidBrush(this.GetDefaultForeColor());
			Brush brush2 = new SolidBrush(this._selectionForeColor);
			Brush brushBack = new SolidBrush(this._selectionBackColor);
			int num = -1;
			long num2 = Math.Min(this._byteProvider.Length - 1L, endByte + (long)this._iHexMaxHBytes);
			bool flag = this._keyInterpreter == null || this._keyInterpreter.GetType() == typeof(HexBox.KeyInterpreter);
			for (long num3 = startByte; num3 < num2 + 1L; num3 += 1L)
			{
				num++;
				Point gridBytePoint = this.GetGridBytePoint((long)num);
				byte b = this._byteProvider.ReadByte(num3);
				byte b2 = this._originalByteProvider.ReadByte(num3);
				bool flag2 = num3 >= this._bytePos && num3 <= this._bytePos + this._selectionLength - 1L && this._selectionLength != 0L;
				bool bLastSel = num3 == this._bytePos + this._selectionLength - 1L;
				if (flag2 && flag)
				{
					this.PaintHexStringSelected(g, b, brush2, brushBack, gridBytePoint, bLastSel);
				}
				else
				{
					this.PaintHexString(g, b, brush, gridBytePoint, b2 != b);
				}
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00015270 File Offset: 0x00013470
		private void PaintHexString(Graphics g, byte b, Brush brush, Point gridPoint, bool bChanged)
		{
			PointF bytePointF = this.GetBytePointF(gridPoint);
			string text = this.ConvertByteToHex(b);
			if (bChanged)
			{
				g.DrawString(text.Substring(0, 1), this.BoldFont, brush, bytePointF, this._stringFormat);
				bytePointF.X += this._charSize.Width;
				g.DrawString(text.Substring(1, 1), this.BoldFont, brush, bytePointF, this._stringFormat);
				return;
			}
			g.DrawString(text.Substring(0, 1), this.Font, brush, bytePointF, this._stringFormat);
			bytePointF.X += this._charSize.Width;
			g.DrawString(text.Substring(1, 1), this.Font, brush, bytePointF, this._stringFormat);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00015338 File Offset: 0x00013538
		private void PaintHexStringSelected(Graphics g, byte b, Brush brush, Brush brushBack, Point gridPoint, bool bLastSel)
		{
			string text = b.ToString(this._hexStringFormat, Thread.CurrentThread.CurrentCulture);
			if (text.Length == 1)
			{
				text = "0" + text;
			}
			PointF bytePointF = this.GetBytePointF(gridPoint);
			float width = (gridPoint.X + 1 == this._iHexMaxHBytes || bLastSel) ? (this._charSize.Width * 2f) : (this._charSize.Width * 3f);
			g.FillRectangle(brushBack, bytePointF.X, bytePointF.Y, width, this._charSize.Height);
			g.DrawString(text.Substring(0, 1), this.Font, brush, bytePointF, this._stringFormat);
			bytePointF.X += this._charSize.Width;
			g.DrawString(text.Substring(1, 1), this.Font, brush, bytePointF, this._stringFormat);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001542C File Offset: 0x0001362C
		private void PaintHexAndStringView(Graphics g, long startByte, long endByte)
		{
			Brush brush = new SolidBrush(this.GetDefaultForeColor());
			Brush brush2 = new SolidBrush(this._selectionForeColor);
			Brush brush3 = new SolidBrush(this._selectionBackColor);
			int num = -1;
			long num2 = Math.Min(this._byteProvider.Length - 1L, endByte + (long)this._iHexMaxHBytes);
			bool flag = this._keyInterpreter == null || this._keyInterpreter.GetType() == typeof(HexBox.KeyInterpreter);
			bool flag2 = this._keyInterpreter != null && this._keyInterpreter.GetType() == typeof(HexBox.StringKeyInterpreter);
			for (long num3 = startByte; num3 < num2 + 1L; num3 += 1L)
			{
				num++;
				Point gridBytePoint = this.GetGridBytePoint((long)num);
				PointF byteStringPointF = this.GetByteStringPointF(gridBytePoint);
				byte b = this._byteProvider.ReadByte(num3);
				bool flag3 = num3 >= this._bytePos && num3 <= this._bytePos + this._selectionLength - 1L && this._selectionLength != 0L;
				bool bLastSel = num3 == this._bytePos + this._selectionLength - 1L;
				bool bChanged = false;
				if (num3 < this._originalByteProvider.Length)
				{
					byte b2 = this._originalByteProvider.ReadByte(num3);
					bChanged = (b2 != b);
					if (this.IsInSelectedAddresses(num3))
					{
						brush2 = new SolidBrush(Color.Black);
						brush3 = new SolidBrush(Color.FromArgb(218, 241, 223));
						flag3 = true;
					}
					else
					{
						brush = new SolidBrush(this.GetDefaultForeColor());
					}
				}
				if (flag3 && flag)
				{
					this.PaintHexStringSelected(g, b, brush2, brush3, gridBytePoint, bLastSel);
				}
				else
				{
					this.PaintHexString(g, b, brush, gridBytePoint, bChanged);
				}
				string s = new string(this.ByteCharConverter.ToChar(b), 1);
				if (flag3 && flag2)
				{
					g.FillRectangle(brush3, byteStringPointF.X, byteStringPointF.Y, this._charSize.Width, this._charSize.Height);
					g.DrawString(s, this.Font, brush2, byteStringPointF, this._stringFormat);
				}
				else
				{
					g.DrawString(s, this.Font, brush, byteStringPointF, this._stringFormat);
				}
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00015664 File Offset: 0x00013864
		private bool IsInSelectedAddresses(long bytePos)
		{
			foreach (long current in this.SelectAddresses.Keys)
			{
				if (bytePos >= current && bytePos < current + (long)((ulong)this.SelectAddresses[current]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000156D4 File Offset: 0x000138D4
		private void PaintCurrentBytesSign(Graphics g)
		{
			if (this._keyInterpreter != null && this.Focused && this._bytePos != -1L && base.Enabled)
			{
				if (this._keyInterpreter.GetType() == typeof(HexBox.KeyInterpreter))
				{
					if (this._selectionLength == 0L)
					{
						Point gridBytePoint = this.GetGridBytePoint(this._bytePos - this._startByte);
						PointF byteStringPointF = this.GetByteStringPointF(gridBytePoint);
						Size size = new Size((int)this._charSize.Width, (int)this._charSize.Height);
						Rectangle rec = new Rectangle((int)byteStringPointF.X, (int)byteStringPointF.Y, size.Width, size.Height);
						if (rec.IntersectsWith(this._recStringView))
						{
							rec.Intersect(this._recStringView);
							this.PaintCurrentByteSign(g, rec);
							return;
						}
					}
					else
					{
						int num = (int)((float)this._recStringView.Width - this._charSize.Width);
						Point gridBytePoint2 = this.GetGridBytePoint(this._bytePos - this._startByte);
						PointF byteStringPointF2 = this.GetByteStringPointF(gridBytePoint2);
						Point gridBytePoint3 = this.GetGridBytePoint(this._bytePos - this._startByte + this._selectionLength - 1L);
						PointF byteStringPointF3 = this.GetByteStringPointF(gridBytePoint3);
						int num2 = gridBytePoint3.Y - gridBytePoint2.Y;
						if (num2 == 0)
						{
							Rectangle rec2 = new Rectangle((int)byteStringPointF2.X, (int)byteStringPointF2.Y, (int)(byteStringPointF3.X - byteStringPointF2.X + this._charSize.Width), (int)this._charSize.Height);
							if (rec2.IntersectsWith(this._recStringView))
							{
								rec2.Intersect(this._recStringView);
								this.PaintCurrentByteSign(g, rec2);
								return;
							}
						}
						else
						{
							Rectangle rec3 = new Rectangle((int)byteStringPointF2.X, (int)byteStringPointF2.Y, (int)((float)(this._recStringView.X + num) - byteStringPointF2.X + this._charSize.Width), (int)this._charSize.Height);
							if (rec3.IntersectsWith(this._recStringView))
							{
								rec3.Intersect(this._recStringView);
								this.PaintCurrentByteSign(g, rec3);
							}
							if (num2 > 1)
							{
								Rectangle rec4 = new Rectangle(this._recStringView.X, (int)(byteStringPointF2.Y + this._charSize.Height), this._recStringView.Width, (int)(this._charSize.Height * (float)(num2 - 1)));
								if (rec4.IntersectsWith(this._recStringView))
								{
									rec4.Intersect(this._recStringView);
									this.PaintCurrentByteSign(g, rec4);
								}
							}
							Rectangle rec5 = new Rectangle(this._recStringView.X, (int)byteStringPointF3.Y, (int)(byteStringPointF3.X - (float)this._recStringView.X + this._charSize.Width), (int)this._charSize.Height);
							if (rec5.IntersectsWith(this._recStringView))
							{
								rec5.Intersect(this._recStringView);
								this.PaintCurrentByteSign(g, rec5);
								return;
							}
						}
					}
				}
				else
				{
					if (this._selectionLength == 0L)
					{
						Point gridBytePoint4 = this.GetGridBytePoint(this._bytePos - this._startByte);
						PointF bytePointF = this.GetBytePointF(gridBytePoint4);
						Size size2 = new Size((int)this._charSize.Width * 2, (int)this._charSize.Height);
						Rectangle rec6 = new Rectangle((int)bytePointF.X, (int)bytePointF.Y, size2.Width, size2.Height);
						this.PaintCurrentByteSign(g, rec6);
						return;
					}
					int num3 = (int)((float)this._recHex.Width - this._charSize.Width * 5f);
					Point gridBytePoint5 = this.GetGridBytePoint(this._bytePos - this._startByte);
					PointF bytePointF2 = this.GetBytePointF(gridBytePoint5);
					Point gridBytePoint6 = this.GetGridBytePoint(this._bytePos - this._startByte + this._selectionLength - 1L);
					PointF bytePointF3 = this.GetBytePointF(gridBytePoint6);
					int num4 = gridBytePoint6.Y - gridBytePoint5.Y;
					if (num4 == 0)
					{
						Rectangle rec7 = new Rectangle((int)bytePointF2.X, (int)bytePointF2.Y, (int)(bytePointF3.X - bytePointF2.X + this._charSize.Width * 2f), (int)this._charSize.Height);
						if (rec7.IntersectsWith(this._recHex))
						{
							rec7.Intersect(this._recHex);
							this.PaintCurrentByteSign(g, rec7);
							return;
						}
					}
					else
					{
						Rectangle rec8 = new Rectangle((int)bytePointF2.X, (int)bytePointF2.Y, (int)((float)(this._recHex.X + num3) - bytePointF2.X + this._charSize.Width * 2f), (int)this._charSize.Height);
						if (rec8.IntersectsWith(this._recHex))
						{
							rec8.Intersect(this._recHex);
							this.PaintCurrentByteSign(g, rec8);
						}
						if (num4 > 1)
						{
							Rectangle rec9 = new Rectangle(this._recHex.X, (int)(bytePointF2.Y + this._charSize.Height), (int)((float)num3 + this._charSize.Width * 2f), (int)(this._charSize.Height * (float)(num4 - 1)));
							if (rec9.IntersectsWith(this._recHex))
							{
								rec9.Intersect(this._recHex);
								this.PaintCurrentByteSign(g, rec9);
							}
						}
						Rectangle rec10 = new Rectangle(this._recHex.X, (int)bytePointF3.Y, (int)(bytePointF3.X - (float)this._recHex.X + this._charSize.Width * 2f), (int)this._charSize.Height);
						if (rec10.IntersectsWith(this._recHex))
						{
							rec10.Intersect(this._recHex);
							this.PaintCurrentByteSign(g, rec10);
						}
					}
				}
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00015CB0 File Offset: 0x00013EB0
		private void PaintCurrentByteSign(Graphics g, Rectangle rec)
		{
			if (rec.Top < 0 || rec.Left < 0 || rec.Width <= 0 || rec.Height <= 0)
			{
				return;
			}
			using (Bitmap bitmap = new Bitmap(rec.Width, rec.Height))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					SolidBrush brush = new SolidBrush(this._shadowSelectionColor);
					graphics.FillRectangle(brush, 0, 0, rec.Width, rec.Height);
					g.DrawImage(bitmap, rec.Left, rec.Top);
				}
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00015D70 File Offset: 0x00013F70
		private Color GetDefaultForeColor()
		{
			if (base.Enabled)
			{
				return this.ForeColor;
			}
			return Color.Gray;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00015D88 File Offset: 0x00013F88
		private void UpdateVisibilityBytes()
		{
			if (this._byteProvider == null || this._byteProvider.Length == 0L)
			{
				return;
			}
			this._startByte = (this._scrollVpos + 1L) * (long)this._iHexMaxHBytes - (long)this._iHexMaxHBytes;
			this._endByte = Math.Min(this._byteProvider.Length - 1L, this._startByte + (long)this._iHexMaxBytes);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00015DF4 File Offset: 0x00013FF4
		private void UpdateRectanglePositioning()
		{
			SizeF sizeF = base.CreateGraphics().MeasureString("A", this.Font, 100, this._stringFormat);
			this._charSize = new SizeF((float)Math.Ceiling((double)sizeF.Width), (float)Math.Ceiling((double)sizeF.Height));
			if (this.m_hCaret != IntPtr.Zero)
			{
				Image.FromHbitmap(this.m_hCaret).Dispose();
				this.m_hCaret = IntPtr.Zero;
			}
			Bitmap bitmap = new Bitmap((int)this._charSize.Width, (int)this._charSize.Height);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.FillRectangle(new SolidBrush(Color.FromArgb(153, 153, 153)), new Rectangle(0, 0, (int)this._charSize.Width, (int)this._charSize.Height));
			}
			this.m_hCaret = bitmap.GetHbitmap();
			this._recContent = base.ClientRectangle;
			this._recContent.X = this._recContent.X + this._recBorderLeft;
			this._recContent.Y = this._recContent.Y + this._recBorderTop;
			this._recContent.Width = this._recContent.Width - (this._recBorderRight + this._recBorderLeft);
			this._recContent.Height = this._recContent.Height - (this._recBorderBottom + this._recBorderTop);
			if (this._vScrollBarVisible)
			{
				this._recContent.Width = this._recContent.Width - this._vScrollBar.Width;
				this._vScrollBar.Left = this._recContent.X + this._recContent.Width;
				this._vScrollBar.Top = this._recContent.Y;
				this._vScrollBar.Height = this._recContent.Height;
			}
			if (this._hScrollBarVisible)
			{
				this._recContent.Height = this._recContent.Height - this._hScrollBar.Height;
				this._hScrollBar.Left = this._recContent.X;
				this._hScrollBar.Top = this._recContent.Y + this._recContent.Height;
				this._hScrollBar.Width = this._recContent.Width;
			}
			int num = 4;
			if (this._lineInfoVisible)
			{
				this._recLineInfo = new Rectangle(this._recContent.X + num, this._recContent.Y, (int)(this._charSize.Width * 10f), this._recContent.Height);
			}
			else
			{
				this._recLineInfo = Rectangle.Empty;
				this._recLineInfo.X = num;
			}
			this._recHex = new Rectangle(this._recLineInfo.X + this._recLineInfo.Width, this._recLineInfo.Y, this._recContent.Width - this._recLineInfo.Width, this._recContent.Height);
			if (this.UseFixedBytesPerLine)
			{
				this.SetHorizontalByteCount(this._bytesPerLine);
				this._recHex.Width = (int)Math.Floor((double)this._iHexMaxHBytes * (double)this._charSize.Width * 3.0 + (double)(2f * this._charSize.Width));
			}
			else
			{
				int num2 = (int)Math.Floor((double)this._recHex.Width / (double)this._charSize.Width);
				if (num2 > 1)
				{
					this.SetHorizontalByteCount((int)Math.Floor((double)num2 / 3.0));
				}
				else
				{
					this.SetHorizontalByteCount(num2);
				}
			}
			if (this._stringViewVisible)
			{
				this._recStringView = new Rectangle(this._recHex.X + this._recHex.Width, this._recHex.Y, (int)(this._charSize.Width * (float)this._iHexMaxHBytes), this._recHex.Height);
			}
			else
			{
				this._recStringView = Rectangle.Empty;
			}
			int verticalByteCount = (int)Math.Floor((double)this._recHex.Height / (double)this._charSize.Height);
			this.SetVerticalByteCount(verticalByteCount);
			this._iHexMaxBytes = this._iHexMaxHBytes * this._iHexMaxVBytes;
			this.UpdateVScrollSize();
			this.UpdateHScrollSize();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001625C File Offset: 0x0001445C
		private PointF GetBytePointF(long byteIndex)
		{
			Point gridBytePoint = this.GetGridBytePoint(byteIndex);
			return this.GetBytePointF(gridBytePoint);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00016278 File Offset: 0x00014478
		private PointF GetBytePointF(Point gp)
		{
			float num = 3f * this._charSize.Width * (float)gp.X + (float)this._recHex.X;
			float y = (float)(gp.Y + 1) * this._charSize.Height - this._charSize.Height + (float)this._recHex.Y;
			return new PointF(num - (float)this._scrollHpos, y);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x000162EC File Offset: 0x000144EC
		private PointF GetByteStringPointF(Point gp)
		{
			float num = this._charSize.Width * (float)gp.X + (float)this._recStringView.X;
			float y = (float)(gp.Y + 1) * this._charSize.Height - this._charSize.Height + (float)this._recStringView.Y;
			return new PointF(num - (float)this._scrollHpos, y);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001635C File Offset: 0x0001455C
		private Point GetGridBytePoint(long byteIndex)
		{
			int num = (int)Math.Floor((double)byteIndex / (double)this._iHexMaxHBytes);
			int x = (int)(byteIndex + (long)this._iHexMaxHBytes - (long)(this._iHexMaxHBytes * (num + 1)));
			Point result = new Point(x, num);
			return result;
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0001639B File Offset: 0x0001459B
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x000163A3 File Offset: 0x000145A3
		[DefaultValue(typeof(Color), "White")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x000163AC File Offset: 0x000145AC
		// (set) Token: 0x06000417 RID: 1047 RVA: 0x000163B4 File Offset: 0x000145B4
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				this._boldFont = new Font(base.Font, FontStyle.Underline);
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x000163CF File Offset: 0x000145CF
		public Font BoldFont
		{
			get
			{
				return this._boldFont;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x000163D7 File Offset: 0x000145D7
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x000163DF File Offset: 0x000145DF
		[Bindable(false), EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x000163E8 File Offset: 0x000145E8
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x000163F0 File Offset: 0x000145F0
		[Bindable(false), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x000163F9 File Offset: 0x000145F9
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x00016401 File Offset: 0x00014601
		[DefaultValue(typeof(Color), "WhiteSmoke"), Category("Appearance")]
		public Color BackColorDisabled
		{
			get
			{
				return this._backColorDisabled;
			}
			set
			{
				this._backColorDisabled = value;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0001640A File Offset: 0x0001460A
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x00016412 File Offset: 0x00014612
		[Category("Hex"), DefaultValue(false), Description("Gets or sets if the count of bytes in one line is fix.")]
		public bool ReadOnly
		{
			get
			{
				return this._readOnly;
			}
			set
			{
				if (this._readOnly == value)
				{
					return;
				}
				this._readOnly = value;
				this.OnReadOnlyChanged(EventArgs.Empty);
				base.Invalidate();
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x00016436 File Offset: 0x00014636
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x0001643E File Offset: 0x0001463E
		[Description("Gets or sets the maximum count of bytes in one line."), DefaultValue(16), Category("Hex")]
		public int BytesPerLine
		{
			get
			{
				return this._bytesPerLine;
			}
			set
			{
				if (this._bytesPerLine == value)
				{
					return;
				}
				this._bytesPerLine = value;
				this.OnBytesPerLineChanged(EventArgs.Empty);
				this.UpdateRectanglePositioning();
				base.Invalidate();
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x00016468 File Offset: 0x00014668
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x00016470 File Offset: 0x00014670
		[Category("Hex"), DefaultValue(false), Description("Gets or sets if the count of bytes in one line is fix.")]
		public bool UseFixedBytesPerLine
		{
			get
			{
				return this._useFixedBytesPerLine;
			}
			set
			{
				if (this._useFixedBytesPerLine == value)
				{
					return;
				}
				this._useFixedBytesPerLine = value;
				this.OnUseFixedBytesPerLineChanged(EventArgs.Empty);
				this.UpdateRectanglePositioning();
				base.Invalidate();
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x0001649A File Offset: 0x0001469A
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x000164A4 File Offset: 0x000146A4
		[Category("Hex"), DefaultValue(false), Description("Gets or sets the visibility of a vertical scroll bar.")]
		public bool VScrollBarVisible
		{
			get
			{
				return this._vScrollBarVisible;
			}
			set
			{
				if (this._vScrollBarVisible == value)
				{
					return;
				}
				this._vScrollBarVisible = value;
				if (this._vScrollBarVisible)
				{
					base.Controls.Add(this._vScrollBar);
				}
				else
				{
					base.Controls.Remove(this._vScrollBar);
				}
				this.UpdateRectanglePositioning();
				this.UpdateVScrollSize();
				this.OnVScrollBarVisibleChanged(EventArgs.Empty);
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00016505 File Offset: 0x00014705
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x00016510 File Offset: 0x00014710
		public bool HScrollBarVisible
		{
			get
			{
				return this._hScrollBarVisible;
			}
			set
			{
				if (this._hScrollBarVisible == value)
				{
					return;
				}
				this._hScrollBarVisible = value;
				if (this._hScrollBarVisible)
				{
					base.Controls.Add(this._hScrollBar);
				}
				else
				{
					base.Controls.Remove(this._hScrollBar);
				}
				this.OnHScrollBarVisibleChanged(EventArgs.Empty);
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x00016565 File Offset: 0x00014765
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x00016570 File Offset: 0x00014770
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IByteProvider ByteProvider
		{
			get
			{
				return this._byteProvider;
			}
			set
			{
				if (this._byteProvider == value)
				{
					return;
				}
				if (value == null)
				{
					this.ActivateEmptyKeyInterpreter();
				}
				else
				{
					this.ActivateKeyInterpreter();
				}
				if (this._byteProvider != null)
				{
					this._byteProvider.LengthChanged -= new EventHandler(this._byteProvider_LengthChanged);
				}
				this._byteProvider = value;
				this._originalByteProvider = new DynamicByteProvider(((DynamicByteProvider)this._byteProvider).Bytes.ToArray());
				if (this._byteProvider != null)
				{
					this._byteProvider.LengthChanged += new EventHandler(this._byteProvider_LengthChanged);
				}
				this.OnByteProviderChanged(EventArgs.Empty);
				if (value == null)
				{
					this._bytePos = -1L;
					this._byteCharacterPos = 0;
					this._selectionLength = 0L;
					this.DestroyCaret();
				}
				else
				{
					this.SetPosition(0L, 0);
					this.SetSelectionLength(0L);
					if (this._caretVisible && this.Focused)
					{
						this.UpdateCaret();
					}
					else
					{
						this.CreateCaret();
					}
				}
				this.CheckCurrentLineChanged();
				this.CheckCurrentPositionInLineChanged();
				this._scrollVpos = 0L;
				this._scrollHpos = 0L;
				this.UpdateVisibilityBytes();
				this.UpdateRectanglePositioning();
				base.Invalidate();
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x00016687 File Offset: 0x00014887
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x0001668F File Offset: 0x0001488F
		[Category("Hex"), DefaultValue(false), Description("Gets or sets the visibility of a line info.")]
		public bool LineInfoVisible
		{
			get
			{
				return this._lineInfoVisible;
			}
			set
			{
				if (this._lineInfoVisible == value)
				{
					return;
				}
				this._lineInfoVisible = value;
				this.OnLineInfoVisibleChanged(EventArgs.Empty);
				this.UpdateRectanglePositioning();
				base.Invalidate();
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x000166B9 File Offset: 0x000148B9
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x000166C1 File Offset: 0x000148C1
		[Description("Gets or sets the offset of the line info."), DefaultValue(0L), Category("Hex")]
		public long LineInfoOffset
		{
			get
			{
				return this._lineInfoOffset;
			}
			set
			{
				if (this._lineInfoOffset == value)
				{
					return;
				}
				this._lineInfoOffset = value;
				base.Invalidate();
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x000166DA File Offset: 0x000148DA
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x000166E4 File Offset: 0x000148E4
		[DefaultValue(typeof(BorderStyle), "Fixed3D"), Description("Gets or sets the hex box´s border style."), Category("Hex")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this._borderStyle;
			}
			set
			{
				if (this._borderStyle == value)
				{
					return;
				}
				this._borderStyle = value;
				switch (this._borderStyle)
				{
				case BorderStyle.None:
					this._recBorderLeft = (this._recBorderTop = (this._recBorderRight = (this._recBorderBottom = 0)));
					break;
				case BorderStyle.FixedSingle:
					this._recBorderLeft = (this._recBorderTop = (this._recBorderRight = (this._recBorderBottom = 1)));
					break;
				case BorderStyle.Fixed3D:
					this._recBorderLeft = (this._recBorderRight = SystemInformation.Border3DSize.Width);
					this._recBorderTop = (this._recBorderBottom = SystemInformation.Border3DSize.Height);
					break;
				}
				this.UpdateRectanglePositioning();
				this.OnBorderStyleChanged(EventArgs.Empty);
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x000167BD File Offset: 0x000149BD
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x000167C5 File Offset: 0x000149C5
		[Description("Gets or sets the visibility of the string view."), DefaultValue(false), Category("Hex")]
		public bool StringViewVisible
		{
			get
			{
				return this._stringViewVisible;
			}
			set
			{
				if (this._stringViewVisible == value)
				{
					return;
				}
				this._stringViewVisible = value;
				this.OnStringViewVisibleChanged(EventArgs.Empty);
				this.UpdateRectanglePositioning();
				base.Invalidate();
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x000167EF File Offset: 0x000149EF
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x00016808 File Offset: 0x00014A08
		[Description("Gets or sets whether the HexBox control displays the hex characters in upper or lower case."), DefaultValue(typeof(HexCasing), "Upper"), Category("Hex")]
		public HexCasing HexCasing
		{
			get
			{
				if (this._hexStringFormat == "X")
				{
					return HexCasing.Upper;
				}
				return HexCasing.Lower;
			}
			set
			{
				string text;
				if (value == HexCasing.Upper)
				{
					text = "X";
				}
				else
				{
					text = "x";
				}
				if (this._hexStringFormat == text)
				{
					return;
				}
				this._hexStringFormat = text;
				this.OnHexCasingChanged(EventArgs.Empty);
				base.Invalidate();
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x0001684D File Offset: 0x00014A4D
		// (set) Token: 0x06000436 RID: 1078 RVA: 0x00016855 File Offset: 0x00014A55
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public long SelectionStart
		{
			get
			{
				return this._bytePos;
			}
			set
			{
				this.SetPosition(value, 0);
				this.ScrollByteIntoView();
				base.Invalidate();
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x0001686B File Offset: 0x00014A6B
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x00016873 File Offset: 0x00014A73
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public long SelectionLength
		{
			get
			{
				return this._selectionLength;
			}
			set
			{
				this.SetSelectionLength(value);
				this.ScrollByteIntoView();
				base.Invalidate();
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00016888 File Offset: 0x00014A88
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x00016890 File Offset: 0x00014A90
		[Description("Gets or sets the line info color. When this property is null, then ForeColor property is used."), DefaultValue(typeof(Color), "Empty"), Category("Hex")]
		public Color LineInfoForeColor
		{
			get
			{
				return this._lineInfoForeColor;
			}
			set
			{
				this._lineInfoForeColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x0001689F File Offset: 0x00014A9F
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x000168A7 File Offset: 0x00014AA7
		[Description("Gets or sets the background color for the selected bytes."), DefaultValue(typeof(Color), "Blue"), Category("Hex")]
		public Color SelectionBackColor
		{
			get
			{
				return this._selectionBackColor;
			}
			set
			{
				this._selectionBackColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x000168B6 File Offset: 0x00014AB6
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x000168BE File Offset: 0x00014ABE
		[Category("Hex"), Description("Gets or sets the foreground color for the selected bytes."), DefaultValue(typeof(Color), "White")]
		public Color SelectionForeColor
		{
			get
			{
				return this._selectionForeColor;
			}
			set
			{
				this._selectionForeColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x000168CD File Offset: 0x00014ACD
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x000168D5 File Offset: 0x00014AD5
		[DefaultValue(true), Category("Hex"), Description("Gets or sets the visibility of a shadow selection.")]
		public bool ShadowSelectionVisible
		{
			get
			{
				return this._shadowSelectionVisible;
			}
			set
			{
				if (this._shadowSelectionVisible == value)
				{
					return;
				}
				this._shadowSelectionVisible = value;
				base.Invalidate();
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x000168EE File Offset: 0x00014AEE
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x000168F6 File Offset: 0x00014AF6
		[Description("Gets or sets the color of the shadow selection."), Category("Hex")]
		public Color ShadowSelectionColor
		{
			get
			{
				return this._shadowSelectionColor;
			}
			set
			{
				this._shadowSelectionColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x00016905 File Offset: 0x00014B05
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int HorizontalByteCount
		{
			get
			{
				return this._iHexMaxHBytes;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x0001690D File Offset: 0x00014B0D
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int VerticalByteCount
		{
			get
			{
				return this._iHexMaxVBytes;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00016915 File Offset: 0x00014B15
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public long CurrentLine
		{
			get
			{
				return this._currentLine;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x0001691D File Offset: 0x00014B1D
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long CurrentPositionInLine
		{
			get
			{
				return (long)this._currentPositionInLine;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x00016926 File Offset: 0x00014B26
		// (set) Token: 0x06000448 RID: 1096 RVA: 0x0001692E File Offset: 0x00014B2E
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool InsertActive
		{
			get
			{
				return this._insertActive;
			}
			set
			{
				if (this._insertActive == value)
				{
					return;
				}
				this._insertActive = value;
				this.DestroyCaret();
				this.CreateCaret();
				this.OnInsertActiveChanged(EventArgs.Empty);
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x00016958 File Offset: 0x00014B58
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public BuiltInContextMenu BuiltInContextMenu
		{
			get
			{
				return this._builtInContextMenu;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00016960 File Offset: 0x00014B60
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x0001697B File Offset: 0x00014B7B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public IByteCharConverter ByteCharConverter
		{
			get
			{
				if (this._byteCharConverter == null)
				{
					this._byteCharConverter = new DefaultByteCharConverter();
				}
				return this._byteCharConverter;
			}
			set
			{
				if (value != null && value != this._byteCharConverter)
				{
					this._byteCharConverter = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x00016996 File Offset: 0x00014B96
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x0001699E File Offset: 0x00014B9E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public Dictionary<long, byte> SelectAddresses
		{
			get;
			set;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x000169A8 File Offset: 0x00014BA8
		private string ConvertBytesToHex(byte[] data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				byte b = data[i];
				string value = this.ConvertByteToHex(b);
				stringBuilder.Append(value);
				stringBuilder.Append(" ");
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00016A14 File Offset: 0x00014C14
		private string ConvertByteToHex(byte b)
		{
			string text = b.ToString(this._hexStringFormat, Thread.CurrentThread.CurrentCulture);
			if (text.Length == 1)
			{
				text = "0" + text;
			}
			return text;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00016A50 File Offset: 0x00014C50
		private byte[] ConvertHexToBytes(string hex)
		{
			if (string.IsNullOrEmpty(hex))
			{
				return null;
			}
			hex = hex.Trim();
			string[] array = hex.Split(new char[]
			{
				' '
			});
			byte[] array2 = new byte[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				string hex2 = array[i];
				byte b;
				if (!this.ConvertHexToByte(hex2, out b))
				{
					return null;
				}
				array2[i] = b;
			}
			return array2;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00016AB8 File Offset: 0x00014CB8
		private bool ConvertHexToByte(string hex, out byte b)
		{
			return byte.TryParse(hex, NumberStyles.HexNumber, Thread.CurrentThread.CurrentCulture, out b);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00016ADD File Offset: 0x00014CDD
		private void SetPosition(long bytePos)
		{
			this.SetPosition(bytePos, this._byteCharacterPos);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00016AEC File Offset: 0x00014CEC
		private void SetPosition(long bytePos, int byteCharacterPos)
		{
			if (this._byteCharacterPos != byteCharacterPos)
			{
				this._byteCharacterPos = byteCharacterPos;
			}
			if (bytePos != this._bytePos)
			{
				this._bytePos = bytePos;
				this.CheckCurrentLineChanged();
				this.CheckCurrentPositionInLineChanged();
				this.OnSelectionStartChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00016B25 File Offset: 0x00014D25
		private void SetSelectionLength(long selectionLength)
		{
			if (selectionLength != this._selectionLength)
			{
				this._selectionLength = selectionLength;
				this.OnSelectionLengthChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00016B42 File Offset: 0x00014D42
		private void SetHorizontalByteCount(int value)
		{
			if (this._iHexMaxHBytes == value)
			{
				return;
			}
			this._iHexMaxHBytes = value;
			this.OnHorizontalByteCountChanged(EventArgs.Empty);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00016B60 File Offset: 0x00014D60
		private void SetVerticalByteCount(int value)
		{
			if (this._iHexMaxVBytes == value)
			{
				return;
			}
			this._iHexMaxVBytes = value;
			this.OnVerticalByteCountChanged(EventArgs.Empty);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00016B80 File Offset: 0x00014D80
		private void CheckCurrentLineChanged()
		{
			long num = (long)Math.Floor((double)this._bytePos / (double)this._iHexMaxHBytes) + 1L;
			if (this._byteProvider == null && this._currentLine != 0L)
			{
				this._currentLine = 0L;
				this.OnCurrentLineChanged(EventArgs.Empty);
				return;
			}
			if (num != this._currentLine)
			{
				this._currentLine = num;
				this.OnCurrentLineChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00016BE8 File Offset: 0x00014DE8
		private void CheckCurrentPositionInLineChanged()
		{
			int num = this.GetGridBytePoint(this._bytePos).X + 1;
			if (this._byteProvider == null && this._currentPositionInLine != 0)
			{
				this._currentPositionInLine = 0;
				this.OnCurrentPositionInLineChanged(EventArgs.Empty);
				return;
			}
			if (num != this._currentPositionInLine)
			{
				this._currentPositionInLine = num;
				this.OnCurrentPositionInLineChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00016C4A File Offset: 0x00014E4A
		protected virtual void OnInsertActiveChanged(EventArgs e)
		{
			if (this.InsertActiveChanged != null)
			{
				this.InsertActiveChanged(this, e);
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00016C61 File Offset: 0x00014E61
		protected virtual void OnReadOnlyChanged(EventArgs e)
		{
			if (this.ReadOnlyChanged != null)
			{
				this.ReadOnlyChanged(this, e);
			}
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00016C78 File Offset: 0x00014E78
		protected virtual void OnByteProviderChanged(EventArgs e)
		{
			if (this.ByteProviderChanged != null)
			{
				this.ByteProviderChanged(this, e);
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00016C8F File Offset: 0x00014E8F
		protected virtual void OnScroll(EventArgs e)
		{
			if (this.VScroll != null)
			{
				this.VScroll(this, e);
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00016CA6 File Offset: 0x00014EA6
		protected virtual void OnHScroll(EventArgs e)
		{
			if (this.HScroll != null)
			{
				this.HScroll(this, e);
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00016CBD File Offset: 0x00014EBD
		protected virtual void OnSelectionStartChanged(EventArgs e)
		{
			if (this.SelectionStartChanged != null)
			{
				this.SelectionStartChanged(this, e);
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00016CD4 File Offset: 0x00014ED4
		protected virtual void OnSelectionLengthChanged(EventArgs e)
		{
			if (this.SelectionLengthChanged != null)
			{
				this.SelectionLengthChanged(this, e);
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00016CEB File Offset: 0x00014EEB
		protected virtual void OnLineInfoVisibleChanged(EventArgs e)
		{
			if (this.LineInfoVisibleChanged != null)
			{
				this.LineInfoVisibleChanged(this, e);
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00016D02 File Offset: 0x00014F02
		protected virtual void OnStringViewVisibleChanged(EventArgs e)
		{
			if (this.StringViewVisibleChanged != null)
			{
				this.StringViewVisibleChanged(this, e);
			}
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00016D19 File Offset: 0x00014F19
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			if (this.BorderStyleChanged != null)
			{
				this.BorderStyleChanged(this, e);
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00016D30 File Offset: 0x00014F30
		protected virtual void OnUseFixedBytesPerLineChanged(EventArgs e)
		{
			if (this.UseFixedBytesPerLineChanged != null)
			{
				this.UseFixedBytesPerLineChanged(this, e);
			}
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00016D47 File Offset: 0x00014F47
		protected virtual void OnBytesPerLineChanged(EventArgs e)
		{
			if (this.BytesPerLineChanged != null)
			{
				this.BytesPerLineChanged(this, e);
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00016D5E File Offset: 0x00014F5E
		protected virtual void OnVScrollBarVisibleChanged(EventArgs e)
		{
			if (this.VScrollBarVisibleChanged != null)
			{
				this.VScrollBarVisibleChanged(this, e);
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00016D75 File Offset: 0x00014F75
		protected virtual void OnHScrollBarVisibleChanged(EventArgs e)
		{
			if (this.HScrollBarVisibleChanged != null)
			{
				this.HScrollBarVisibleChanged(this, e);
			}
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00016D8C File Offset: 0x00014F8C
		protected virtual void OnHexCasingChanged(EventArgs e)
		{
			if (this.HexCasingChanged != null)
			{
				this.HexCasingChanged(this, e);
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00016DA3 File Offset: 0x00014FA3
		protected virtual void OnHorizontalByteCountChanged(EventArgs e)
		{
			if (this.HorizontalByteCountChanged != null)
			{
				this.HorizontalByteCountChanged(this, e);
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00016DBA File Offset: 0x00014FBA
		protected virtual void OnVerticalByteCountChanged(EventArgs e)
		{
			if (this.VerticalByteCountChanged != null)
			{
				this.VerticalByteCountChanged(this, e);
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00016DD1 File Offset: 0x00014FD1
		protected virtual void OnCurrentLineChanged(EventArgs e)
		{
			if (this.CurrentLineChanged != null)
			{
				this.CurrentLineChanged(this, e);
			}
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00016DE8 File Offset: 0x00014FE8
		protected virtual void OnCurrentPositionInLineChanged(EventArgs e)
		{
			if (this.CurrentPositionInLineChanged != null)
			{
				this.CurrentPositionInLineChanged(this, e);
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00016DFF File Offset: 0x00014FFF
		protected virtual void OnCopied(EventArgs e)
		{
			if (this.Copied != null)
			{
				this.Copied(this, e);
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00016E16 File Offset: 0x00015016
		protected virtual void OnCopiedHex(EventArgs e)
		{
			if (this.CopiedHex != null)
			{
				this.CopiedHex(this, e);
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00016E30 File Offset: 0x00015030
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!this.Focused)
			{
				base.Focus();
			}
			if (e.Button == MouseButtons.Left)
			{
				this.SetCaretPosition(new Point(e.X + (int)this._scrollHpos, e.Y));
			}
			base.OnMouseDown(e);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00016E80 File Offset: 0x00015080
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			int lines = -(e.Delta * SystemInformation.MouseWheelScrollLines / 120);
			this.PerformScrollLines(lines);
			this.OnScroll(EventArgs.Empty);
			base.OnMouseWheel(e);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00016EB8 File Offset: 0x000150B8
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (this._byteProvider == null)
			{
				base.OnMouseMove(e);
				return;
			}
			BytePositionInfo hexBytePositionInfo = this.GetHexBytePositionInfo(base.PointToClient(new Point(Control.MousePosition.X - (int)this._scrollHpos, Control.MousePosition.Y)));
			if (hexBytePositionInfo.Index < this._byteProvider.Length && hexBytePositionInfo.Index < this._originalByteProvider.Length)
			{
				if (this._byteProvider.ReadByte(hexBytePositionInfo.Index) != this._originalByteProvider.ReadByte(hexBytePositionInfo.Index))
				{
					if (this.tipIndex != hexBytePositionInfo.Index)
					{
						this.tipIndex = hexBytePositionInfo.Index;
						this.tip.Show("Original Value: " + this._originalByteProvider.ReadByte(hexBytePositionInfo.Index).ToString("X2"), this, base.PointToClient(Control.MousePosition), 1000);
					}
				}
				else
				{
					this.tipIndex = -1L;
				}
			}
			base.OnMouseMove(e);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00016FCF File Offset: 0x000151CF
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.UpdateRectanglePositioning();
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00016FDE File Offset: 0x000151DE
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.CreateCaret();
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00016FED File Offset: 0x000151ED
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.DestroyCaret();
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00016FFC File Offset: 0x000151FC
		private void _byteProvider_LengthChanged(object sender, EventArgs e)
		{
			this.UpdateVScrollSize();
		}

		// Token: 0x040001BD RID: 445
		private const int THUMPTRACKDELAY = 50;

		// Token: 0x040001BE RID: 446
		private ToolTip tip;

		// Token: 0x040001BF RID: 447
		private bool m_bTipVisible;

		// Token: 0x040001C0 RID: 448
		private Rectangle _recContent;

		// Token: 0x040001C1 RID: 449
		private Rectangle _recLineInfo;

		// Token: 0x040001C2 RID: 450
		private Rectangle _recHex;

		// Token: 0x040001C3 RID: 451
		private Rectangle _recStringView;

		// Token: 0x040001C4 RID: 452
		private StringFormat _stringFormat;

		// Token: 0x040001C5 RID: 453
		private SizeF _charSize;

		// Token: 0x040001C6 RID: 454
		private int _iHexMaxHBytes;

		// Token: 0x040001C7 RID: 455
		private int _iHexMaxVBytes;

		// Token: 0x040001C8 RID: 456
		private int _iHexMaxBytes;

		// Token: 0x040001C9 RID: 457
		private long _scrollVmin;

		// Token: 0x040001CA RID: 458
		private long _scrollHmin;

		// Token: 0x040001CB RID: 459
		private long _scrollVmax;

		// Token: 0x040001CC RID: 460
		private long _scrollHmax;

		// Token: 0x040001CD RID: 461
		private long _scrollVpos;

		// Token: 0x040001CE RID: 462
		private long _scrollHpos;

		// Token: 0x040001CF RID: 463
		private VScrollBar _vScrollBar;

		// Token: 0x040001D0 RID: 464
		private HScrollBar _hScrollBar;

		// Token: 0x040001D1 RID: 465
		private System.Windows.Forms.Timer _thumbTrackTimer = new System.Windows.Forms.Timer();

		// Token: 0x040001D2 RID: 466
		private long _thumbTrackPosition;

		// Token: 0x040001D3 RID: 467
		private int _lastThumbtrack;

		// Token: 0x040001D4 RID: 468
		private int _recBorderLeft = SystemInformation.Border3DSize.Width;

		// Token: 0x040001D5 RID: 469
		private int _recBorderRight = SystemInformation.Border3DSize.Width;

		// Token: 0x040001D6 RID: 470
		private int _recBorderTop = SystemInformation.Border3DSize.Height;

		// Token: 0x040001D7 RID: 471
		private int _recBorderBottom = SystemInformation.Border3DSize.Height;

		// Token: 0x040001D8 RID: 472
		private long _startByte;

		// Token: 0x040001D9 RID: 473
		private long _endByte;

		// Token: 0x040001DA RID: 474
		private long _bytePos = -1L;

		// Token: 0x040001DB RID: 475
		private int _byteCharacterPos;

		// Token: 0x040001DC RID: 476
		private string _hexStringFormat = "X";

		// Token: 0x040001DD RID: 477
		private HexBox.IKeyInterpreter _keyInterpreter;

		// Token: 0x040001DE RID: 478
		private HexBox.EmptyKeyInterpreter _eki;

		// Token: 0x040001DF RID: 479
		private HexBox.KeyInterpreter _ki;

		// Token: 0x040001E0 RID: 480
		private HexBox.StringKeyInterpreter _ski;

		// Token: 0x040001E1 RID: 481
		private bool _caretVisible;

		// Token: 0x040001E2 RID: 482
		private bool _abortFind;

		// Token: 0x040001E3 RID: 483
		private long _findingPos;

		// Token: 0x040001E4 RID: 484
		private bool _insertActive;

		// Token: 0x040001FA RID: 506
		private IntPtr m_hCaret = IntPtr.Zero;

		// Token: 0x040001FB RID: 507
		private Font _boldFont;

		// Token: 0x040001FC RID: 508
		private Color _backColorDisabled = Color.FromName("WhiteSmoke");

		// Token: 0x040001FD RID: 509
		private bool _readOnly;

		// Token: 0x040001FE RID: 510
		private int _bytesPerLine = 16;

		// Token: 0x040001FF RID: 511
		private bool _useFixedBytesPerLine;

		// Token: 0x04000200 RID: 512
		private bool _vScrollBarVisible;

		// Token: 0x04000201 RID: 513
		private bool _hScrollBarVisible;

		// Token: 0x04000202 RID: 514
		private IByteProvider _byteProvider;

		// Token: 0x04000203 RID: 515
		private IByteProvider _originalByteProvider;

		// Token: 0x04000204 RID: 516
		private bool _lineInfoVisible;

		// Token: 0x04000205 RID: 517
		private long _lineInfoOffset;

		// Token: 0x04000206 RID: 518
		private BorderStyle _borderStyle = BorderStyle.Fixed3D;

		// Token: 0x04000207 RID: 519
		private bool _stringViewVisible;

		// Token: 0x04000208 RID: 520
		private long _selectionLength;

		// Token: 0x04000209 RID: 521
		private Color _lineInfoForeColor = Color.Empty;

		// Token: 0x0400020A RID: 522
		private Color _selectionBackColor = Color.Blue;

		// Token: 0x0400020B RID: 523
		private Color _selectionForeColor = Color.White;

		// Token: 0x0400020C RID: 524
		private bool _shadowSelectionVisible = true;

		// Token: 0x0400020D RID: 525
		private Color _shadowSelectionColor = Color.FromArgb(100, 60, 188, 255);

		// Token: 0x0400020E RID: 526
		private long _currentLine;

		// Token: 0x0400020F RID: 527
		private int _currentPositionInLine;

		// Token: 0x04000210 RID: 528
		private BuiltInContextMenu _builtInContextMenu;

		// Token: 0x04000211 RID: 529
		private IByteCharConverter _byteCharConverter;

		// Token: 0x04000212 RID: 530
		private long tipIndex = -1L;

		// Token: 0x02000052 RID: 82
		private interface IKeyInterpreter
		{
			// Token: 0x06000475 RID: 1141
			void Activate();

			// Token: 0x06000476 RID: 1142
			void Deactivate();

			// Token: 0x06000477 RID: 1143
			bool PreProcessWmKeyUp(ref Message m);

			// Token: 0x06000478 RID: 1144
			bool PreProcessWmChar(ref Message m);

			// Token: 0x06000479 RID: 1145
			bool PreProcessWmKeyDown(ref Message m);

			// Token: 0x0600047A RID: 1146
			PointF GetCaretPointF(long byteIndex);
		}

		// Token: 0x02000053 RID: 83
		private class EmptyKeyInterpreter : HexBox.IKeyInterpreter
		{
			// Token: 0x0600047B RID: 1147 RVA: 0x00017004 File Offset: 0x00015204
			public EmptyKeyInterpreter(HexBox hexBox)
			{
				this._hexBox = hexBox;
			}

			// Token: 0x0600047C RID: 1148 RVA: 0x00017013 File Offset: 0x00015213
			public void Activate()
			{
			}

			// Token: 0x0600047D RID: 1149 RVA: 0x00017015 File Offset: 0x00015215
			public void Deactivate()
			{
			}

			// Token: 0x0600047E RID: 1150 RVA: 0x00017017 File Offset: 0x00015217
			public bool PreProcessWmKeyUp(ref Message m)
			{
				return this._hexBox.BasePreProcessMessage(ref m);
			}

			// Token: 0x0600047F RID: 1151 RVA: 0x00017025 File Offset: 0x00015225
			public bool PreProcessWmChar(ref Message m)
			{
				return this._hexBox.BasePreProcessMessage(ref m);
			}

			// Token: 0x06000480 RID: 1152 RVA: 0x00017033 File Offset: 0x00015233
			public bool PreProcessWmKeyDown(ref Message m)
			{
				return this._hexBox.BasePreProcessMessage(ref m);
			}

			// Token: 0x06000481 RID: 1153 RVA: 0x00017044 File Offset: 0x00015244
			public PointF GetCaretPointF(long byteIndex)
			{
				return default(PointF);
			}

			// Token: 0x04000214 RID: 532
			private HexBox _hexBox;
		}

		// Token: 0x02000054 RID: 84
		private class KeyInterpreter : HexBox.IKeyInterpreter
		{
			// Token: 0x06000482 RID: 1154 RVA: 0x0001705A File Offset: 0x0001525A
			public KeyInterpreter(HexBox hexBox)
			{
				this._hexBox = hexBox;
			}

			// Token: 0x06000483 RID: 1155 RVA: 0x0001706C File Offset: 0x0001526C
			public virtual void Activate()
			{
				this._hexBox.MouseDown += new MouseEventHandler(this.BeginMouseSelection);
				this._hexBox.MouseMove += new MouseEventHandler(this.UpdateMouseSelection);
				this._hexBox.MouseUp += new MouseEventHandler(this.EndMouseSelection);
			}

			// Token: 0x06000484 RID: 1156 RVA: 0x000170C0 File Offset: 0x000152C0
			public virtual void Deactivate()
			{
				this._hexBox.MouseDown -= new MouseEventHandler(this.BeginMouseSelection);
				this._hexBox.MouseMove -= new MouseEventHandler(this.UpdateMouseSelection);
				this._hexBox.MouseUp -= new MouseEventHandler(this.EndMouseSelection);
			}

			// Token: 0x06000485 RID: 1157 RVA: 0x00017114 File Offset: 0x00015314
			private void BeginMouseSelection(object sender, MouseEventArgs e)
			{
				if (e.Button != MouseButtons.Left)
				{
					return;
				}
				this._mouseDown = true;
				if (!this._shiftDown)
				{
					this._bpiStart = new BytePositionInfo(this._hexBox._bytePos, this._hexBox._byteCharacterPos);
					this._hexBox.ReleaseSelection();
					return;
				}
				this.UpdateMouseSelection(this, e);
			}

			// Token: 0x06000486 RID: 1158 RVA: 0x00017174 File Offset: 0x00015374
			private void UpdateMouseSelection(object sender, MouseEventArgs e)
			{
				if (!this._mouseDown)
				{
					return;
				}
				this._bpi = this.GetBytePositionInfo(new Point(e.X, e.Y));
				long index = this._bpi.Index;
				long num;
				long num2;
				if (index < this._bpiStart.Index)
				{
					num = index;
					num2 = this._bpiStart.Index - index;
				}
				else if (index > this._bpiStart.Index)
				{
					num = this._bpiStart.Index;
					num2 = index - num;
				}
				else
				{
					num = this._hexBox._bytePos;
					num2 = 0L;
				}
				if (num != this._hexBox._bytePos || num2 != this._hexBox._selectionLength)
				{
					this._hexBox.InternalSelect(num, num2);
					this._hexBox.ScrollByteIntoView(this._bpi.Index);
				}
			}

			// Token: 0x06000487 RID: 1159 RVA: 0x00017241 File Offset: 0x00015441
			private void EndMouseSelection(object sender, MouseEventArgs e)
			{
				this._mouseDown = false;
			}

			// Token: 0x06000488 RID: 1160 RVA: 0x0001724C File Offset: 0x0001544C
			public virtual bool PreProcessWmKeyDown(ref Message m)
			{
				Keys keys = (Keys)m.WParam.ToInt32();
				Keys keys2 = keys | Control.ModifierKeys;
				bool flag = this.MessageHandlers.ContainsKey(keys2);
				if (flag && this.RaiseKeyDown(keys2))
				{
					return true;
				}
				HexBox.KeyInterpreter.MessageDelegate messageDelegate = flag ? this.MessageHandlers[keys2] : new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_Default);
				return messageDelegate(ref m);
			}

			// Token: 0x06000489 RID: 1161 RVA: 0x000172B3 File Offset: 0x000154B3
			protected bool PreProcessWmKeyDown_Default(ref Message m)
			{
				this._hexBox.ScrollByteIntoView();
				return this._hexBox.BasePreProcessMessage(ref m);
			}

			// Token: 0x0600048A RID: 1162 RVA: 0x000172CC File Offset: 0x000154CC
			protected bool RaiseKeyDown(Keys keyData)
			{
				KeyEventArgs keyEventArgs = new KeyEventArgs(keyData);
				this._hexBox.OnKeyDown(keyEventArgs);
				return keyEventArgs.Handled;
			}

			// Token: 0x0600048B RID: 1163 RVA: 0x000172F2 File Offset: 0x000154F2
			protected virtual bool PreProcessWmKeyDown_Left(ref Message m)
			{
				return this.PerformPosMoveLeft();
			}

			// Token: 0x0600048C RID: 1164 RVA: 0x000172FC File Offset: 0x000154FC
			protected virtual bool PreProcessWmKeyDown_Up(ref Message m)
			{
				long num = this._hexBox._bytePos;
				int byteCharacterPos = this._hexBox._byteCharacterPos;
				if (num != 0L || byteCharacterPos != 0)
				{
					num = Math.Max(-1L, num - (long)this._hexBox._iHexMaxHBytes);
					if (num == -1L)
					{
						return true;
					}
					this._hexBox.SetPosition(num);
					if (num < this._hexBox._startByte)
					{
						this._hexBox.PerformScrollLineUp();
					}
					this._hexBox.UpdateCaret();
					this._hexBox.Invalidate();
				}
				this._hexBox.ScrollByteIntoView();
				this._hexBox.ReleaseSelection();
				return true;
			}

			// Token: 0x0600048D RID: 1165 RVA: 0x00017398 File Offset: 0x00015598
			protected virtual bool PreProcessWmKeyDown_Right(ref Message m)
			{
				return this.PerformPosMoveRight();
			}

			// Token: 0x0600048E RID: 1166 RVA: 0x000173A0 File Offset: 0x000155A0
			protected virtual bool PreProcessWmKeyDown_Down(ref Message m)
			{
				long num = this._hexBox._bytePos;
				int byteCharacterPos = this._hexBox._byteCharacterPos;
				if (num >= this._hexBox._byteProvider.Length - (long)this._hexBox._bytesPerLine)
				{
					return true;
				}
				num = Math.Min(this._hexBox._byteProvider.Length, num + (long)this._hexBox._iHexMaxHBytes);
				if (num == this._hexBox._byteProvider.Length)
				{
					byteCharacterPos = 0;
				}
				this._hexBox.SetPosition(num, byteCharacterPos);
				if (num > this._hexBox._endByte - 1L)
				{
					this._hexBox.PerformScrollLineDown();
				}
				this._hexBox.UpdateCaret();
				this._hexBox.ScrollByteIntoView();
				this._hexBox.ReleaseSelection();
				this._hexBox.Invalidate();
				return true;
			}

			// Token: 0x0600048F RID: 1167 RVA: 0x00017478 File Offset: 0x00015678
			protected virtual bool PreProcessWmKeyDown_PageUp(ref Message m)
			{
				long num = this._hexBox._bytePos;
				int byteCharacterPos = this._hexBox._byteCharacterPos;
				if (num == 0L && byteCharacterPos == 0)
				{
					return true;
				}
				num = Math.Max(0L, num - (long)this._hexBox._iHexMaxBytes);
				if (num == 0L)
				{
					return true;
				}
				this._hexBox.SetPosition(num);
				if (num < this._hexBox._startByte)
				{
					this._hexBox.PerformScrollPageUp();
				}
				this._hexBox.ReleaseSelection();
				this._hexBox.UpdateCaret();
				this._hexBox.Invalidate();
				return true;
			}

			// Token: 0x06000490 RID: 1168 RVA: 0x0001750C File Offset: 0x0001570C
			protected virtual bool PreProcessWmKeyDown_PageDown(ref Message m)
			{
				long num = this._hexBox._bytePos;
				int num2 = this._hexBox._byteCharacterPos;
				if (num == this._hexBox._byteProvider.Length && num2 == 0)
				{
					return true;
				}
				num = Math.Min(this._hexBox._byteProvider.Length, num + (long)this._hexBox._iHexMaxBytes);
				if (num == this._hexBox._byteProvider.Length)
				{
					num2 = 0;
				}
				this._hexBox.SetPosition(num, num2);
				if (num > this._hexBox._endByte - 1L)
				{
					this._hexBox.PerformScrollPageDown();
				}
				this._hexBox.ReleaseSelection();
				this._hexBox.UpdateCaret();
				this._hexBox.Invalidate();
				return true;
			}

			// Token: 0x06000491 RID: 1169 RVA: 0x000175D0 File Offset: 0x000157D0
			protected virtual bool PreProcessWmKeyDown_ShiftLeft(ref Message m)
			{
				long num = this._hexBox._bytePos;
				long num2 = this._hexBox._selectionLength;
				if (num + num2 < 1L)
				{
					return true;
				}
				if (num + num2 <= this._bpiStart.Index)
				{
					if (num == 0L)
					{
						return true;
					}
					num -= 1L;
					num2 += 1L;
				}
				else
				{
					num2 = Math.Max(0L, num2 - 1L);
				}
				this._hexBox.ScrollByteIntoView();
				this._hexBox.InternalSelect(num, num2);
				return true;
			}

			// Token: 0x06000492 RID: 1170 RVA: 0x00017648 File Offset: 0x00015848
			protected virtual bool PreProcessWmKeyDown_ShiftUp(ref Message m)
			{
				long num = this._hexBox._bytePos;
				long num2 = this._hexBox._selectionLength;
				if (num - (long)this._hexBox._iHexMaxHBytes < 0L && num <= this._bpiStart.Index)
				{
					return true;
				}
				if (this._bpiStart.Index >= num + num2)
				{
					num -= (long)this._hexBox._iHexMaxHBytes;
					num2 += (long)this._hexBox._iHexMaxHBytes;
					this._hexBox.InternalSelect(num, num2);
					this._hexBox.ScrollByteIntoView();
				}
				else
				{
					num2 -= (long)this._hexBox._iHexMaxHBytes;
					if (num2 < 0L)
					{
						num = this._bpiStart.Index + num2;
						num2 = -num2;
						this._hexBox.InternalSelect(num, num2);
						this._hexBox.ScrollByteIntoView();
					}
					else
					{
						num2 -= (long)this._hexBox._iHexMaxHBytes;
						this._hexBox.InternalSelect(num, num2);
						this._hexBox.ScrollByteIntoView(num + num2);
					}
				}
				return true;
			}

			// Token: 0x06000493 RID: 1171 RVA: 0x00017744 File Offset: 0x00015944
			protected virtual bool PreProcessWmKeyDown_ShiftRight(ref Message m)
			{
				long num = this._hexBox._bytePos;
				long num2 = this._hexBox._selectionLength;
				if (num + num2 >= this._hexBox._byteProvider.Length)
				{
					return true;
				}
				if (this._bpiStart.Index <= num)
				{
					num2 += 1L;
					this._hexBox.InternalSelect(num, num2);
					this._hexBox.ScrollByteIntoView(num + num2);
				}
				else
				{
					num += 1L;
					num2 = Math.Max(0L, num2 - 1L);
					this._hexBox.InternalSelect(num, num2);
					this._hexBox.ScrollByteIntoView();
				}
				return true;
			}

			// Token: 0x06000494 RID: 1172 RVA: 0x000177DC File Offset: 0x000159DC
			protected virtual bool PreProcessWmKeyDown_ShiftDown(ref Message m)
			{
				long num = this._hexBox._bytePos;
				long num2 = this._hexBox._selectionLength;
				long length = this._hexBox._byteProvider.Length;
				if (num + num2 + (long)this._hexBox._iHexMaxHBytes > length)
				{
					return true;
				}
				if (this._bpiStart.Index <= num)
				{
					num2 += (long)this._hexBox._iHexMaxHBytes;
					this._hexBox.InternalSelect(num, num2);
					this._hexBox.ScrollByteIntoView(num + num2);
				}
				else
				{
					num2 -= (long)this._hexBox._iHexMaxHBytes;
					if (num2 < 0L)
					{
						num = this._bpiStart.Index;
						num2 = -num2;
					}
					else
					{
						num += (long)this._hexBox._iHexMaxHBytes;
					}
					this._hexBox.InternalSelect(num, num2);
					this._hexBox.ScrollByteIntoView();
				}
				return true;
			}

			// Token: 0x06000495 RID: 1173 RVA: 0x000178B0 File Offset: 0x00015AB0
			protected virtual bool PreProcessWmKeyDown_Tab(ref Message m)
			{
				if (this._hexBox._stringViewVisible && this._hexBox._keyInterpreter.GetType() == typeof(HexBox.KeyInterpreter))
				{
					this._hexBox.ActivateStringKeyInterpreter();
					this._hexBox.ScrollByteIntoView();
					this._hexBox.ReleaseSelection();
					this._hexBox.UpdateCaret();
					this._hexBox.Invalidate();
					return true;
				}
				if (this._hexBox.Parent == null)
				{
					return true;
				}
				this._hexBox.Parent.SelectNextControl(this._hexBox, true, true, true, true);
				return true;
			}

			// Token: 0x06000496 RID: 1174 RVA: 0x00017950 File Offset: 0x00015B50
			protected virtual bool PreProcessWmKeyDown_ShiftTab(ref Message m)
			{
				if (this._hexBox._keyInterpreter is HexBox.StringKeyInterpreter)
				{
					this._shiftDown = false;
					this._hexBox.ActivateKeyInterpreter();
					this._hexBox.ScrollByteIntoView();
					this._hexBox.ReleaseSelection();
					this._hexBox.UpdateCaret();
					this._hexBox.Invalidate();
					return true;
				}
				if (this._hexBox.Parent == null)
				{
					return true;
				}
				this._hexBox.Parent.SelectNextControl(this._hexBox, false, true, true, true);
				return true;
			}

			// Token: 0x06000497 RID: 1175 RVA: 0x000179DC File Offset: 0x00015BDC
			protected virtual bool PreProcessWmKeyDown_Back(ref Message m)
			{
				if (!this._hexBox._byteProvider.SupportsDeleteBytes())
				{
					return true;
				}
				if (this._hexBox.ReadOnly)
				{
					return true;
				}
				long bytePos = this._hexBox._bytePos;
				long selectionLength = this._hexBox._selectionLength;
				long num = (this._hexBox._byteCharacterPos == 0 && selectionLength == 0L) ? (bytePos - 1L) : bytePos;
				if (num < 0L && selectionLength < 1L)
				{
					return true;
				}
				long length = (selectionLength > 0L) ? selectionLength : 1L;
				this._hexBox._byteProvider.DeleteBytes(Math.Max(0L, num), length);
				this._hexBox.UpdateVScrollSize();
				if (selectionLength == 0L)
				{
					this.PerformPosMoveLeftByte();
				}
				this._hexBox.ReleaseSelection();
				this._hexBox.Invalidate();
				return true;
			}

			// Token: 0x06000498 RID: 1176 RVA: 0x00017AA0 File Offset: 0x00015CA0
			protected virtual bool PreProcessWmKeyDown_Delete(ref Message m)
			{
				if (!this._hexBox._byteProvider.SupportsDeleteBytes())
				{
					return true;
				}
				if (this._hexBox.ReadOnly)
				{
					return true;
				}
				long bytePos = this._hexBox._bytePos;
				long selectionLength = this._hexBox._selectionLength;
				if (bytePos >= this._hexBox._byteProvider.Length)
				{
					return true;
				}
				long length = (selectionLength > 0L) ? selectionLength : 1L;
				this._hexBox._byteProvider.DeleteBytes(bytePos, length);
				this._hexBox.UpdateVScrollSize();
				this._hexBox.ReleaseSelection();
				this._hexBox.Invalidate();
				return true;
			}

			// Token: 0x06000499 RID: 1177 RVA: 0x00017B3C File Offset: 0x00015D3C
			protected virtual bool PreProcessWmKeyDown_Home(ref Message m)
			{
				long num = this._hexBox._bytePos;
				int byteCharacterPos = this._hexBox._byteCharacterPos;
				if (num < 1L)
				{
					return true;
				}
				num = 0L;
				byteCharacterPos = 0;
				this._hexBox.SetPosition(num, byteCharacterPos);
				this._hexBox.ScrollByteIntoView();
				this._hexBox.UpdateCaret();
				this._hexBox.ReleaseSelection();
				return true;
			}

			// Token: 0x0600049A RID: 1178 RVA: 0x00017B9C File Offset: 0x00015D9C
			protected virtual bool PreProcessWmKeyDown_End(ref Message m)
			{
				long num = this._hexBox._bytePos;
				int byteCharacterPos = this._hexBox._byteCharacterPos;
				if (num >= this._hexBox._byteProvider.Length - 1L)
				{
					return true;
				}
				num = this._hexBox._byteProvider.Length - 1L;
				byteCharacterPos = 0;
				this._hexBox.SetPosition(num, byteCharacterPos);
				this._hexBox.ScrollByteIntoView();
				this._hexBox.UpdateCaret();
				this._hexBox.ReleaseSelection();
				return true;
			}

			// Token: 0x0600049B RID: 1179 RVA: 0x00017C20 File Offset: 0x00015E20
			protected virtual bool PreProcessWmKeyDown_ShiftShiftKey(ref Message m)
			{
				if (this._mouseDown)
				{
					return true;
				}
				if (this._shiftDown)
				{
					return true;
				}
				this._shiftDown = true;
				if (this._hexBox._selectionLength > 0L)
				{
					return true;
				}
				this._bpiStart = new BytePositionInfo(this._hexBox._bytePos, this._hexBox._byteCharacterPos);
				return true;
			}

			// Token: 0x0600049C RID: 1180 RVA: 0x00017C7B File Offset: 0x00015E7B
			protected virtual bool PreProcessWmKeyDown_ControlC(ref Message m)
			{
				this._hexBox.Copy();
				return true;
			}

			// Token: 0x0600049D RID: 1181 RVA: 0x00017C89 File Offset: 0x00015E89
			protected virtual bool PreProcessWmKeyDown_ControlX(ref Message m)
			{
				this._hexBox.Cut();
				return true;
			}

			// Token: 0x0600049E RID: 1182 RVA: 0x00017C97 File Offset: 0x00015E97
			protected virtual bool PreProcessWmKeyDown_ControlV(ref Message m)
			{
				this._hexBox.Paste();
				return true;
			}

			// Token: 0x0600049F RID: 1183 RVA: 0x00017CA8 File Offset: 0x00015EA8
			public virtual bool PreProcessWmChar(ref Message m)
			{
				if (Control.ModifierKeys == Keys.Control)
				{
					return this._hexBox.BasePreProcessMessage(ref m);
				}
				bool flag = this._hexBox._byteProvider.SupportsWriteByte();
				bool flag2 = this._hexBox._byteProvider.SupportsInsertBytes();
				bool flag3 = this._hexBox._byteProvider.SupportsDeleteBytes();
				long bytePos = this._hexBox._bytePos;
				long selectionLength = this._hexBox._selectionLength;
				int num = this._hexBox._byteCharacterPos;
				if ((!flag && bytePos != this._hexBox._byteProvider.Length) || (!flag2 && bytePos == this._hexBox._byteProvider.Length))
				{
					return this._hexBox.BasePreProcessMessage(ref m);
				}
				char c = (char)m.WParam.ToInt32();
				if (!Uri.IsHexDigit(c))
				{
					return this._hexBox.BasePreProcessMessage(ref m);
				}
				if (this.RaiseKeyPress(c))
				{
					return true;
				}
				if (this._hexBox.ReadOnly)
				{
					return true;
				}
				bool flag4 = bytePos == this._hexBox._byteProvider.Length;
				if (!flag4 && flag2 && this._hexBox.InsertActive && num == 0)
				{
					flag4 = true;
				}
				if (flag3 && flag2 && selectionLength > 0L)
				{
					this._hexBox._byteProvider.DeleteBytes(bytePos, selectionLength);
					flag4 = true;
					num = 0;
					this._hexBox.SetPosition(bytePos, num);
				}
				this._hexBox.ReleaseSelection();
				byte b;
				if (flag4)
				{
					b = 0;
				}
				else
				{
					b = this._hexBox._byteProvider.ReadByte(bytePos);
				}
				string text = b.ToString("X", Thread.CurrentThread.CurrentCulture);
				if (text.Length == 1)
				{
					text = "0" + text;
				}
				string text2 = c.ToString();
				if (num == 0)
				{
					text2 += text.Substring(1, 1);
				}
				else
				{
					text2 = text.Substring(0, 1) + text2;
				}
				byte b2 = byte.Parse(text2, NumberStyles.AllowHexSpecifier, Thread.CurrentThread.CurrentCulture);
				if (flag4)
				{
					this._hexBox._byteProvider.InsertBytes(bytePos, new byte[]
					{
						b2
					});
				}
				else
				{
					this._hexBox._byteProvider.WriteByte(bytePos, b2);
				}
				this.PerformPosMoveRight();
				this._hexBox.Invalidate();
				return true;
			}

			// Token: 0x060004A0 RID: 1184 RVA: 0x00017EF8 File Offset: 0x000160F8
			protected bool RaiseKeyPress(char keyChar)
			{
				KeyPressEventArgs keyPressEventArgs = new KeyPressEventArgs(keyChar);
				this._hexBox.OnKeyPress(keyPressEventArgs);
				return keyPressEventArgs.Handled;
			}

			// Token: 0x060004A1 RID: 1185 RVA: 0x00017F20 File Offset: 0x00016120
			public virtual bool PreProcessWmKeyUp(ref Message m)
			{
				Keys keys = (Keys)m.WParam.ToInt32();
				Keys keys2 = keys | Control.ModifierKeys;
				Keys keys3 = keys2;
				if ((keys3 == Keys.ShiftKey || keys3 == Keys.Insert) && this.RaiseKeyUp(keys2))
				{
					return true;
				}
				Keys keys4 = keys2;
				if (keys4 == Keys.ShiftKey)
				{
					this._shiftDown = false;
					return true;
				}
				if (keys4 != Keys.Insert)
				{
					return this._hexBox.BasePreProcessMessage(ref m);
				}
				return this.PreProcessWmKeyUp_Insert(ref m);
			}

			// Token: 0x060004A2 RID: 1186 RVA: 0x00017F89 File Offset: 0x00016189
			protected virtual bool PreProcessWmKeyUp_Insert(ref Message m)
			{
				this._hexBox.InsertActive = !this._hexBox.InsertActive;
				return true;
			}

			// Token: 0x060004A3 RID: 1187 RVA: 0x00017FA8 File Offset: 0x000161A8
			protected bool RaiseKeyUp(Keys keyData)
			{
				KeyEventArgs keyEventArgs = new KeyEventArgs(keyData);
				this._hexBox.OnKeyUp(keyEventArgs);
				return keyEventArgs.Handled;
			}

			// Token: 0x170001C3 RID: 451
			// (get) Token: 0x060004A4 RID: 1188 RVA: 0x00017FD0 File Offset: 0x000161D0
			private Dictionary<Keys, HexBox.KeyInterpreter.MessageDelegate> MessageHandlers
			{
				get
				{
					if (this._messageHandlers == null)
					{
						this._messageHandlers = new Dictionary<Keys, HexBox.KeyInterpreter.MessageDelegate>();
						this._messageHandlers.Add(Keys.Left, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_Left));
						this._messageHandlers.Add(Keys.Up, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_Up));
						this._messageHandlers.Add(Keys.Right, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_Right));
						this._messageHandlers.Add(Keys.Down, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_Down));
						this._messageHandlers.Add(Keys.Prior, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_PageUp));
						this._messageHandlers.Add(Keys.Next, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_PageDown));
						this._messageHandlers.Add(Keys.LButton | Keys.MButton | Keys.Space | Keys.Shift, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_ShiftLeft));
						this._messageHandlers.Add(Keys.RButton | Keys.MButton | Keys.Space | Keys.Shift, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_ShiftUp));
						this._messageHandlers.Add(Keys.LButton | Keys.RButton | Keys.MButton | Keys.Space | Keys.Shift, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_ShiftRight));
						this._messageHandlers.Add(Keys.Back | Keys.Space | Keys.Shift, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_ShiftDown));
						this._messageHandlers.Add(Keys.Tab, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_Tab));
						this._messageHandlers.Add(Keys.Back, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_Back));
						this._messageHandlers.Add(Keys.Delete, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_Delete));
						this._messageHandlers.Add(Keys.Home, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_Home));
						this._messageHandlers.Add(Keys.End, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_End));
						this._messageHandlers.Add(Keys.ShiftKey | Keys.Shift, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_ShiftShiftKey));
						this._messageHandlers.Add((Keys)131139, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_ControlC));
						this._messageHandlers.Add((Keys)131160, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_ControlX));
						this._messageHandlers.Add((Keys)131158, new HexBox.KeyInterpreter.MessageDelegate(this.PreProcessWmKeyDown_ControlV));
					}
					return this._messageHandlers;
				}
			}

			// Token: 0x060004A5 RID: 1189 RVA: 0x00018200 File Offset: 0x00016400
			protected virtual bool PerformPosMoveLeft()
			{
				long num = this._hexBox._bytePos;
				long selectionLength = this._hexBox._selectionLength;
				int num2 = this._hexBox._byteCharacterPos;
				if (selectionLength != 0L)
				{
					num2 = 0;
					this._hexBox.SetPosition(num, num2);
					this._hexBox.ReleaseSelection();
				}
				else
				{
					if (num == 0L && num2 == 0)
					{
						return true;
					}
					if (num2 > 0)
					{
						num2--;
					}
					else
					{
						num = Math.Max(0L, num - 1L);
						num2++;
					}
					this._hexBox.SetPosition(num, num2);
					if (num < this._hexBox._startByte)
					{
						this._hexBox.PerformScrollLineUp();
					}
					this._hexBox.UpdateCaret();
					this._hexBox.Invalidate();
				}
				this._hexBox.ScrollByteIntoView();
				return true;
			}

			// Token: 0x060004A6 RID: 1190 RVA: 0x000182C0 File Offset: 0x000164C0
			protected virtual bool PerformPosMoveRight()
			{
				long num = this._hexBox._bytePos;
				int num2 = this._hexBox._byteCharacterPos;
				long selectionLength = this._hexBox._selectionLength;
				if (selectionLength != 0L)
				{
					num += selectionLength;
					num2 = 0;
					this._hexBox.SetPosition(num, num2);
					this._hexBox.ReleaseSelection();
				}
				else if (num != this._hexBox._byteProvider.Length || num2 != 0)
				{
					if (num2 > 0)
					{
						num = Math.Min(this._hexBox._byteProvider.Length, num + 1L);
						num2 = 0;
					}
					else
					{
						num2++;
					}
					if (num >= this._hexBox._byteProvider.Length)
					{
						return true;
					}
					this._hexBox.SetPosition(num, num2);
					if (num > this._hexBox._endByte - 1L)
					{
						this._hexBox.PerformScrollLineDown();
					}
					this._hexBox.UpdateCaret();
					this._hexBox.Invalidate();
				}
				this._hexBox.ScrollByteIntoView();
				return true;
			}

			// Token: 0x060004A7 RID: 1191 RVA: 0x000183B8 File Offset: 0x000165B8
			protected virtual bool PerformPosMoveLeftByte()
			{
				long num = this._hexBox._bytePos;
				int byteCharacterPos = this._hexBox._byteCharacterPos;
				if (num == 0L)
				{
					return true;
				}
				num = Math.Max(0L, num - 1L);
				byteCharacterPos = 0;
				this._hexBox.SetPosition(num, byteCharacterPos);
				if (num < this._hexBox._startByte)
				{
					this._hexBox.PerformScrollLineUp();
				}
				this._hexBox.UpdateCaret();
				this._hexBox.ScrollByteIntoView();
				this._hexBox.Invalidate();
				return true;
			}

			// Token: 0x060004A8 RID: 1192 RVA: 0x0001843C File Offset: 0x0001663C
			protected virtual bool PerformPosMoveRightByte()
			{
				long num = this._hexBox._bytePos;
				int byteCharacterPos = this._hexBox._byteCharacterPos;
				if (num == this._hexBox._byteProvider.Length)
				{
					return true;
				}
				num = Math.Min(this._hexBox._byteProvider.Length, num + 1L);
				byteCharacterPos = 0;
				this._hexBox.SetPosition(num, byteCharacterPos);
				if (num > this._hexBox._endByte - 1L)
				{
					this._hexBox.PerformScrollLineDown();
				}
				this._hexBox.UpdateCaret();
				this._hexBox.ScrollByteIntoView();
				this._hexBox.Invalidate();
				return true;
			}

			// Token: 0x060004A9 RID: 1193 RVA: 0x000184DD File Offset: 0x000166DD
			public virtual PointF GetCaretPointF(long byteIndex)
			{
				return this._hexBox.GetBytePointF(byteIndex);
			}

			// Token: 0x060004AA RID: 1194 RVA: 0x000184EB File Offset: 0x000166EB
			protected virtual BytePositionInfo GetBytePositionInfo(Point p)
			{
				return this._hexBox.GetHexBytePositionInfo(p);
			}

			// Token: 0x04000215 RID: 533
			protected HexBox _hexBox;

			// Token: 0x04000216 RID: 534
			protected bool _shiftDown;

			// Token: 0x04000217 RID: 535
			private bool _mouseDown;

			// Token: 0x04000218 RID: 536
			private BytePositionInfo _bpiStart;

			// Token: 0x04000219 RID: 537
			private BytePositionInfo _bpi;

			// Token: 0x0400021A RID: 538
			private Dictionary<Keys, HexBox.KeyInterpreter.MessageDelegate> _messageHandlers;

			// Token: 0x02000055 RID: 85
			// (Invoke) Token: 0x060004AC RID: 1196
			private delegate bool MessageDelegate(ref Message m);
		}

		// Token: 0x02000056 RID: 86
		private class StringKeyInterpreter : HexBox.KeyInterpreter
		{
			// Token: 0x060004AF RID: 1199 RVA: 0x000184F9 File Offset: 0x000166F9
			public StringKeyInterpreter(HexBox hexBox) : base(hexBox)
			{
				this._hexBox._byteCharacterPos = 0;
			}

			// Token: 0x060004B0 RID: 1200 RVA: 0x00018510 File Offset: 0x00016710
			public override bool PreProcessWmKeyDown(ref Message m)
			{
				Keys keys = (Keys)m.WParam.ToInt32();
				Keys keys2 = keys | Control.ModifierKeys;
				Keys keys3 = keys2;
				if ((keys3 == Keys.Tab || keys3 == (Keys.LButton | Keys.Back | Keys.Shift)) && base.RaiseKeyDown(keys2))
				{
					return true;
				}
				Keys keys4 = keys2;
				if (keys4 == Keys.Tab)
				{
					return this.PreProcessWmKeyDown_Tab(ref m);
				}
				if (keys4 == (Keys.LButton | Keys.Back | Keys.Shift))
				{
					return this.PreProcessWmKeyDown_ShiftTab(ref m);
				}
				return base.PreProcessWmKeyDown(ref m);
			}

			// Token: 0x060004B1 RID: 1201 RVA: 0x00018577 File Offset: 0x00016777
			protected override bool PreProcessWmKeyDown_Left(ref Message m)
			{
				return this.PerformPosMoveLeftByte();
			}

			// Token: 0x060004B2 RID: 1202 RVA: 0x0001857F File Offset: 0x0001677F
			protected override bool PreProcessWmKeyDown_Right(ref Message m)
			{
				return this.PerformPosMoveRightByte();
			}

			// Token: 0x060004B3 RID: 1203 RVA: 0x00018588 File Offset: 0x00016788
			public override bool PreProcessWmChar(ref Message m)
			{
				if (Control.ModifierKeys == Keys.Control)
				{
					return this._hexBox.BasePreProcessMessage(ref m);
				}
				bool flag = this._hexBox._byteProvider.SupportsWriteByte();
				bool flag2 = this._hexBox._byteProvider.SupportsInsertBytes();
				bool flag3 = this._hexBox._byteProvider.SupportsDeleteBytes();
				long bytePos = this._hexBox._bytePos;
				long selectionLength = this._hexBox._selectionLength;
				int byteCharacterPos = this._hexBox._byteCharacterPos;
				if ((!flag && bytePos != this._hexBox._byteProvider.Length) || (!flag2 && bytePos == this._hexBox._byteProvider.Length))
				{
					return this._hexBox.BasePreProcessMessage(ref m);
				}
				char c = (char)m.WParam.ToInt32();
				if (base.RaiseKeyPress(c))
				{
					return true;
				}
				if (this._hexBox.ReadOnly)
				{
					return true;
				}
				bool flag4 = bytePos == this._hexBox._byteProvider.Length;
				if (!flag4 && flag2 && this._hexBox.InsertActive)
				{
					flag4 = true;
				}
				if (flag3 && flag2 && selectionLength > 0L)
				{
					this._hexBox._byteProvider.DeleteBytes(bytePos, selectionLength);
					flag4 = true;
					byteCharacterPos = 0;
					this._hexBox.SetPosition(bytePos, byteCharacterPos);
				}
				this._hexBox.ReleaseSelection();
				byte b = this._hexBox.ByteCharConverter.ToByte(c);
				if (flag4)
				{
					this._hexBox._byteProvider.InsertBytes(bytePos, new byte[]
					{
						b
					});
				}
				else
				{
					this._hexBox._byteProvider.WriteByte(bytePos, b);
				}
				this.PerformPosMoveRightByte();
				this._hexBox.Invalidate();
				return true;
			}

			// Token: 0x060004B4 RID: 1204 RVA: 0x00018738 File Offset: 0x00016938
			public override PointF GetCaretPointF(long byteIndex)
			{
				Point gridBytePoint = this._hexBox.GetGridBytePoint(byteIndex);
				return this._hexBox.GetByteStringPointF(gridBytePoint);
			}

			// Token: 0x060004B5 RID: 1205 RVA: 0x0001875E File Offset: 0x0001695E
			protected override BytePositionInfo GetBytePositionInfo(Point p)
			{
				return this._hexBox.GetStringBytePositionInfo(p);
			}
		}
	}
}
