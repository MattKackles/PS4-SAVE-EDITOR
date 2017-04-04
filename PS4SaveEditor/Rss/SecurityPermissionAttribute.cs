using System;

namespace Rss
{
	// Token: 0x02000090 RID: 144
	internal class SecurityPermissionAttribute : Attribute
	{
		// Token: 0x060006D2 RID: 1746 RVA: 0x000269DE File Offset: 0x00024BDE
		public SecurityPermissionAttribute(SecurityAction securityAction)
		{
		}

		// Token: 0x04000320 RID: 800
		public bool Execution;
	}
}
