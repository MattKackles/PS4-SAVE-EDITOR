using System;

namespace PS3SaveEditor
{
	// Token: 0x02000008 RID: 8
	internal class BackupItem
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00006E86 File Offset: 0x00005086
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00006E8E File Offset: 0x0000508E
		public string BackupFile
		{
			get;
			set;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00006E97 File Offset: 0x00005097
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00006E9F File Offset: 0x0000509F
		public string Timestamp
		{
			get;
			set;
		}
	}
}
