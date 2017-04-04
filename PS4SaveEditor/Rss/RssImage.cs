using System;

namespace Rss
{
	// Token: 0x0200007A RID: 122
	[Serializable]
	public class RssImage : RssElement
	{
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x00022DCD File Offset: 0x00020FCD
		// (set) Token: 0x0600060E RID: 1550 RVA: 0x00022DD5 File Offset: 0x00020FD5
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

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x00022DE3 File Offset: 0x00020FE3
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x00022DEB File Offset: 0x00020FEB
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

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x00022DF9 File Offset: 0x00020FF9
		// (set) Token: 0x06000612 RID: 1554 RVA: 0x00022E01 File Offset: 0x00021001
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

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x00022E0F File Offset: 0x0002100F
		// (set) Token: 0x06000614 RID: 1556 RVA: 0x00022E17 File Offset: 0x00021017
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

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x00022E25 File Offset: 0x00021025
		// (set) Token: 0x06000616 RID: 1558 RVA: 0x00022E2D File Offset: 0x0002102D
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = RssDefault.Check(value);
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x00022E3B File Offset: 0x0002103B
		// (set) Token: 0x06000618 RID: 1560 RVA: 0x00022E43 File Offset: 0x00021043
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = RssDefault.Check(value);
			}
		}

		// Token: 0x040002D6 RID: 726
		private string title = "";

		// Token: 0x040002D7 RID: 727
		private string description = "";

		// Token: 0x040002D8 RID: 728
		private Uri uri = RssDefault.Uri;

		// Token: 0x040002D9 RID: 729
		private Uri link = RssDefault.Uri;

		// Token: 0x040002DA RID: 730
		private int width = -1;

		// Token: 0x040002DB RID: 731
		private int height = -1;
	}
}
