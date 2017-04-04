using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DGDev
{
	// Token: 0x02000109 RID: 265
	public partial class SplashScreen : Form
	{
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0003E4B4 File Offset: 0x0003C6B4
		// (set) Token: 0x06000B14 RID: 2836 RVA: 0x0003E4BC File Offset: 0x0003C6BC
		public string SetInfo
		{
			get
			{
				return SplashScreen.Information;
			}
			set
			{
				SplashScreen.Information = value;
				if (this.ProgramInfoLabel.InvokeRequired)
				{
					Delegate method = new SplashScreen.UpdateLabel(this.UpdateInfo);
					base.Invoke(method);
					return;
				}
				this.UpdateInfo();
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0003E4F8 File Offset: 0x0003C6F8
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x0003E500 File Offset: 0x0003C700
		public string SetStatus
		{
			get
			{
				return SplashScreen.Status;
			}
			set
			{
				SplashScreen.Status = value;
				if (this.StatusLabel.InvokeRequired)
				{
					Delegate method = new SplashScreen.UpdateLabel(this.UpdateStatus);
					base.Invoke(method);
					return;
				}
				this.UpdateStatus();
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0003E53C File Offset: 0x0003C73C
		// (set) Token: 0x06000B18 RID: 2840 RVA: 0x0003E543 File Offset: 0x0003C743
		public Image SetBackgroundImage
		{
			get
			{
				return SplashScreen.BGImage;
			}
			set
			{
				SplashScreen.BGImage = value;
				if (value != null)
				{
					this.BackgroundImage = SplashScreen.BGImage;
					base.ClientSize = this.BackgroundImage.Size;
				}
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x0003E56A File Offset: 0x0003C76A
		// (set) Token: 0x06000B1A RID: 2842 RVA: 0x0003E571 File Offset: 0x0003C771
		public Color SetTransparentKey
		{
			get
			{
				return SplashScreen.TransparentKey;
			}
			set
			{
				SplashScreen.TransparentKey = value;
				if (value != Color.Empty)
				{
					base.TransparencyKey = this.SetTransparentKey;
				}
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x0003E592 File Offset: 0x0003C792
		// (set) Token: 0x06000B1C RID: 2844 RVA: 0x0003E599 File Offset: 0x0003C799
		public bool SetFade
		{
			get
			{
				return SplashScreen.FadeInOut;
			}
			set
			{
				SplashScreen.FadeInOut = value;
				base.Opacity = (value ? 0.0 : 1.0);
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x0003E5BE File Offset: 0x0003C7BE
		public static SplashScreen Current
		{
			get
			{
				if (SplashScreen.SplashScreenForm == null)
				{
					SplashScreen.SplashScreenForm = new SplashScreen();
				}
				return SplashScreen.SplashScreenForm;
			}
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0003E5D8 File Offset: 0x0003C7D8
		public void SetStatusLabel(Point StatusLabelLocation, int StatusLabelWidth, int StatusLabelHeight)
		{
			if (StatusLabelLocation != Point.Empty)
			{
				this.StatusLabel.Location = StatusLabelLocation;
			}
			if (StatusLabelWidth == 0 && StatusLabelHeight == 0)
			{
				this.StatusLabel.AutoSize = true;
				return;
			}
			if (StatusLabelWidth > 0)
			{
				this.StatusLabel.Width = StatusLabelWidth;
			}
			if (StatusLabelHeight > 0)
			{
				this.StatusLabel.Height = StatusLabelHeight;
			}
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0003E634 File Offset: 0x0003C834
		public void SetInfoLabel(Point InfoLabelLocation, int InfoLabelWidth, int InfoLabelHeight)
		{
			this.ProgramInfoLabel.TextAlign = ContentAlignment.MiddleRight;
			if (InfoLabelLocation != Point.Empty)
			{
				this.ProgramInfoLabel.Location = InfoLabelLocation;
			}
			if (InfoLabelWidth == 0 && InfoLabelHeight == 0)
			{
				this.ProgramInfoLabel.AutoSize = true;
				return;
			}
			if (InfoLabelWidth > 0)
			{
				this.ProgramInfoLabel.Width = InfoLabelWidth;
			}
			if (InfoLabelHeight > 0)
			{
				this.ProgramInfoLabel.Height = InfoLabelHeight;
			}
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0003E69A File Offset: 0x0003C89A
		public void ShowSplashScreen()
		{
			SplashScreen.SplashScreenThread = new Thread(new ThreadStart(SplashScreen.ShowForm));
			SplashScreen.SplashScreenThread.IsBackground = true;
			SplashScreen.SplashScreenThread.Name = "SplashScreenThread";
			SplashScreen.SplashScreenThread.Start();
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0003E6D8 File Offset: 0x0003C8D8
		public void CloseSplashScreen()
		{
			if (SplashScreen.SplashScreenForm != null)
			{
				if (base.InvokeRequired)
				{
					Delegate method = new SplashScreen.CloseSplash(this.HideSplash);
					base.Invoke(method);
					return;
				}
				this.HideSplash();
			}
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0003E710 File Offset: 0x0003C910
		public SplashScreen()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0003E71E File Offset: 0x0003C91E
		private static void ShowForm()
		{
			Application.Run(SplashScreen.SplashScreenForm);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0003E72A File Offset: 0x0003C92A
		private void UpdateStatus()
		{
			this.StatusLabel.Text = this.SetStatus;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0003E73D File Offset: 0x0003C93D
		private void UpdateInfo()
		{
			this.ProgramInfoLabel.Text = this.SetInfo;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0003E750 File Offset: 0x0003C950
		private void SplashTimer_Tick(object sender, EventArgs e)
		{
			if (SplashScreen.FadeMode)
			{
				if (base.Opacity < 1.0)
				{
					base.Opacity += 0.05;
					return;
				}
				this.SplashTimer.Stop();
				return;
			}
			else
			{
				if (base.Opacity > 0.0)
				{
					base.Opacity -= 0.08;
					return;
				}
				base.Dispose();
				return;
			}
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0003E93F File Offset: 0x0003CB3F
		private void SplashScreen_Load(object sender, EventArgs e)
		{
			if (this.SetFade)
			{
				SplashScreen.FadeMode = true;
				this.SplashTimer.Interval = 50;
				this.SplashTimer.Start();
			}
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0003E967 File Offset: 0x0003CB67
		private void HideSplash()
		{
			if (this.SetFade)
			{
				SplashScreen.FadeMode = false;
				this.SplashTimer.Start();
				return;
			}
			base.Dispose();
		}

		// Token: 0x040005B1 RID: 1457
		private const double OpacityDecrement = 0.08;

		// Token: 0x040005B2 RID: 1458
		private const double OpacityIncrement = 0.05;

		// Token: 0x040005B3 RID: 1459
		private const int TimerInterval = 50;

		// Token: 0x040005B4 RID: 1460
		private static bool FadeMode;

		// Token: 0x040005B5 RID: 1461
		private static bool FadeInOut;

		// Token: 0x040005B6 RID: 1462
		private static Image BGImage;

		// Token: 0x040005B7 RID: 1463
		private static string Information;

		// Token: 0x040005B8 RID: 1464
		private static string Status;

		// Token: 0x040005B9 RID: 1465
		private static SplashScreen SplashScreenForm;

		// Token: 0x040005BA RID: 1466
		private static Thread SplashScreenThread;

		// Token: 0x040005BB RID: 1467
		private static Color TransparentKey;

		// Token: 0x0200010A RID: 266
		// (Invoke) Token: 0x06000B2B RID: 2859
		private delegate void UpdateLabel();

		// Token: 0x0200010B RID: 267
		// (Invoke) Token: 0x06000B2F RID: 2863
		private delegate void CloseSplash();
	}
}
