using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace PS3SaveEditor
{
	// Token: 0x02000066 RID: 102
	public class SingleInstanceApplication : WindowsFormsApplicationBase
	{
		// Token: 0x0600054E RID: 1358 RVA: 0x00021100 File Offset: 0x0001F300
		public SingleInstanceApplication(AuthenticationMode mode) : base(mode)
		{
			this.InitializeAppProperties();
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0002110F File Offset: 0x0001F30F
		public SingleInstanceApplication()
		{
			this.InitializeAppProperties();
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0002111D File Offset: 0x0001F31D
		protected virtual void InitializeAppProperties()
		{
			base.IsSingleInstance = true;
			base.EnableVisualStyles = true;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0002112D File Offset: 0x0001F32D
		public virtual void Run(Form mainForm)
		{
			base.MainForm = mainForm;
			this.Run(base.CommandLineArgs);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00021144 File Offset: 0x0001F344
		private void Run(ReadOnlyCollection<string> commandLineArgs)
		{
			ArrayList arrayList = new ArrayList(commandLineArgs);
			string[] commandLine = (string[])arrayList.ToArray(typeof(string));
			base.Run(commandLine);
		}
	}
}
