using System;

namespace Rss
{
	// Token: 0x0200007E RID: 126
	[Serializable]
	public class RssGuid : RssElement
	{
		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x000233ED File Offset: 0x000215ED
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x000233F5 File Offset: 0x000215F5
		public DBBool PermaLink
		{
			get
			{
				return this.permaLink;
			}
			set
			{
				this.permaLink = value;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x000233FE File Offset: 0x000215FE
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x00023406 File Offset: 0x00021606
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

		// Token: 0x040002EC RID: 748
		private DBBool permaLink = DBBool.Null;

		// Token: 0x040002ED RID: 749
		private string name = "";
	}
}
