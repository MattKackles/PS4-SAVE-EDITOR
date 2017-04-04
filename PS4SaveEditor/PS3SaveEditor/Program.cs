using System;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace PS3SaveEditor
{
	// Token: 0x02000067 RID: 103
	internal static class Program
	{
		// Token: 0x06000553 RID: 1363 RVA: 0x00021178 File Offset: 0x0001F378
		[STAThread]
		private static void Main()
		{
			SingleInstanceApplication singleInstanceApplication = new SingleInstanceApplication();
			singleInstanceApplication.Startup += new StartupEventHandler(Program.app_Startup);
			singleInstanceApplication.StartupNextInstance += new StartupNextInstanceEventHandler(Program.OnAppStartupNextInstance);
			Program.mainForm = new MainForm();
			singleInstanceApplication.Run(Program.mainForm);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000211C4 File Offset: 0x0001F3C4
		private static void app_Startup(object sender, StartupEventArgs e)
		{
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x000211C6 File Offset: 0x0001F3C6
		private static void OnAppStartupNextInstance(object sender, StartupNextInstanceEventArgs e)
		{
			if (Program.mainForm.WindowState == FormWindowState.Minimized)
			{
				Program.mainForm.WindowState = FormWindowState.Normal;
			}
			Program.mainForm.Activate();
		}

		// Token: 0x0400029C RID: 668
		private static Form mainForm;

		// Token: 0x0400029D RID: 669
		public static string HTACCESS_USER = "seps4";

		// Token: 0x0400029E RID: 670
		public static string HTACCESS_PWD = "medsI]7r3Jik";
	}
}
