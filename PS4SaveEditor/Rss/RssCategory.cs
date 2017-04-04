using System;

namespace Rss
{
	// Token: 0x0200008E RID: 142
	[Serializable]
	public class RssCategory : RssElement
	{
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x000269AA File Offset: 0x00024BAA
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x000269B2 File Offset: 0x00024BB2
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = RssDefault.Check(value);
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x000269C0 File Offset: 0x00024BC0
		// (set) Token: 0x060006D0 RID: 1744 RVA: 0x000269C8 File Offset: 0x00024BC8
		public string Domain
		{
			get
			{
				return this.domain;
			}
			set
			{
				this.domain = RssDefault.Check(value);
			}
		}

		// Token: 0x0400031E RID: 798
		private string name = "";

		// Token: 0x0400031F RID: 799
		private string domain = "";
	}
}
