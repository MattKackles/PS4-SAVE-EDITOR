using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace PS3SaveEditor.Properties
{
	// Token: 0x02000172 RID: 370
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0"), CompilerGenerated]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x00059CFC File Offset: 0x00057EFC
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x040008D5 RID: 2261
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
