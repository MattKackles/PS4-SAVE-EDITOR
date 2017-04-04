using System;

namespace Rss
{
	// Token: 0x02000080 RID: 128
	[Serializable]
	public class RssSource : RssElement
	{
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x00023571 File Offset: 0x00021771
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x00023579 File Offset: 0x00021779
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

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00023587 File Offset: 0x00021787
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x0002358F File Offset: 0x0002178F
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

		// Token: 0x040002F8 RID: 760
		private string name = "";

		// Token: 0x040002F9 RID: 761
		private Uri uri = RssDefault.Uri;
	}
}
