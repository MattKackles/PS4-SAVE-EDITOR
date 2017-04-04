using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace PS3SaveEditor.Properties
{
	// Token: 0x02000068 RID: 104
	[DebuggerNonUserCode, CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		// Token: 0x06000557 RID: 1367 RVA: 0x00021200 File Offset: 0x0001F400
		internal Resources()
		{
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00021208 File Offset: 0x0001F408
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("PS3SaveEditor.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x00021247 File Offset: 0x0001F447
		// (set) Token: 0x0600055A RID: 1370 RVA: 0x0002124E File Offset: 0x0001F44E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x0400029F RID: 671
		private static ResourceManager resourceMan;

		// Token: 0x040002A0 RID: 672
		private static CultureInfo resourceCulture;
	}
}
