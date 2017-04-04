using System;

namespace Rss
{
	// Token: 0x0200007D RID: 125
	[Serializable]
	public class RssEnclosure : RssElement
	{
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x0002338D File Offset: 0x0002158D
		// (set) Token: 0x0600063A RID: 1594 RVA: 0x00023395 File Offset: 0x00021595
		public Uri Url
		{
			get
			{
				return this.uri;
			}
			set
			{
				this.uri = RssDefault.Check(value);
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x000233A3 File Offset: 0x000215A3
		// (set) Token: 0x0600063C RID: 1596 RVA: 0x000233AB File Offset: 0x000215AB
		public int Length
		{
			get
			{
				return this.length;
			}
			set
			{
				this.length = RssDefault.Check(value);
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x000233B9 File Offset: 0x000215B9
		// (set) Token: 0x0600063E RID: 1598 RVA: 0x000233C1 File Offset: 0x000215C1
		public string Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = RssDefault.Check(value);
			}
		}

		// Token: 0x040002E9 RID: 745
		private Uri uri = RssDefault.Uri;

		// Token: 0x040002EA RID: 746
		private int length = -1;

		// Token: 0x040002EB RID: 747
		private string type = "";
	}
}
