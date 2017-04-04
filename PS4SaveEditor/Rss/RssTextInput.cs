using System;

namespace Rss
{
	// Token: 0x0200007B RID: 123
	[Serializable]
	public class RssTextInput : RssElement
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x00022E85 File Offset: 0x00021085
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x00022E8D File Offset: 0x0002108D
		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = RssDefault.Check(value);
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00022E9B File Offset: 0x0002109B
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x00022EA3 File Offset: 0x000210A3
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = RssDefault.Check(value);
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00022EB1 File Offset: 0x000210B1
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x00022EB9 File Offset: 0x000210B9
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

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00022EC7 File Offset: 0x000210C7
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x00022ECF File Offset: 0x000210CF
		public Uri Link
		{
			get
			{
				return this.link;
			}
			set
			{
				this.link = RssDefault.Check(value);
			}
		}

		// Token: 0x040002DC RID: 732
		private string title = "";

		// Token: 0x040002DD RID: 733
		private string description = "";

		// Token: 0x040002DE RID: 734
		private string name = "";

		// Token: 0x040002DF RID: 735
		private Uri link = RssDefault.Uri;
	}
}
