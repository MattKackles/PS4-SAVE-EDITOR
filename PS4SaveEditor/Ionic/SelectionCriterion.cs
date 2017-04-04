using System;
using System.Diagnostics;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000130 RID: 304
	internal abstract class SelectionCriterion
	{
		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00042F96 File Offset: 0x00041196
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x00042F9E File Offset: 0x0004119E
		internal virtual bool Verbose
		{
			get;
			set;
		}

		// Token: 0x06000C3C RID: 3132
		internal abstract bool Evaluate(string filename);

		// Token: 0x06000C3D RID: 3133 RVA: 0x00042FA7 File Offset: 0x000411A7
		[Conditional("SelectorTrace")]
		protected static void CriterionTrace(string format, params object[] args)
		{
		}

		// Token: 0x06000C3E RID: 3134
		internal abstract bool Evaluate(ZipEntry entry);
	}
}
