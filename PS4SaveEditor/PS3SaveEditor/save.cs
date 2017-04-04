using System;

namespace PS3SaveEditor
{
	// Token: 0x02000010 RID: 16
	public class save
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00007721 File Offset: 0x00005921
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00007729 File Offset: 0x00005929
		public string id
		{
			get;
			set;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00007732 File Offset: 0x00005932
		// (set) Token: 0x06000079 RID: 121 RVA: 0x0000773A File Offset: 0x0000593A
		public string gamecode
		{
			get;
			set;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00007743 File Offset: 0x00005943
		// (set) Token: 0x0600007B RID: 123 RVA: 0x0000774B File Offset: 0x0000594B
		public string title
		{
			get;
			set;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00007754 File Offset: 0x00005954
		// (set) Token: 0x0600007D RID: 125 RVA: 0x0000775C File Offset: 0x0000595C
		public string description
		{
			get;
			set;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00007765 File Offset: 0x00005965
		// (set) Token: 0x0600007F RID: 127 RVA: 0x0000776D File Offset: 0x0000596D
		public string note
		{
			get;
			set;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00007776 File Offset: 0x00005976
		// (set) Token: 0x06000081 RID: 129 RVA: 0x0000777E File Offset: 0x0000597E
		public string folder
		{
			get;
			set;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00007787 File Offset: 0x00005987
		// (set) Token: 0x06000083 RID: 131 RVA: 0x0000778F File Offset: 0x0000598F
		public string region
		{
			get;
			set;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00007798 File Offset: 0x00005998
		// (set) Token: 0x06000085 RID: 133 RVA: 0x000077A0 File Offset: 0x000059A0
		public long updated
		{
			get;
			set;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000077AC File Offset: 0x000059AC
		internal static save Copy(save save)
		{
			return new save
			{
				folder = save.folder,
				region = save.region,
				updated = save.updated,
				description = save.description,
				gamecode = save.gamecode,
				note = save.note,
				title = save.title,
				id = save.id
			};
		}
	}
}
