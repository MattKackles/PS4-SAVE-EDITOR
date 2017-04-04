using System;

namespace Rss
{
	// Token: 0x0200007F RID: 127
	[Serializable]
	public class RssItem : RssElement
	{
		// Token: 0x06000645 RID: 1605 RVA: 0x00023474 File Offset: 0x00021674
		public override string ToString()
		{
			if (this.title != null)
			{
				return this.title;
			}
			if (this.description != null)
			{
				return this.description;
			}
			return "RssItem";
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x00023499 File Offset: 0x00021699
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x000234A1 File Offset: 0x000216A1
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

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x000234AF File Offset: 0x000216AF
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x000234B7 File Offset: 0x000216B7
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

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x000234C5 File Offset: 0x000216C5
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x000234CD File Offset: 0x000216CD
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

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x000234DB File Offset: 0x000216DB
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x000234E3 File Offset: 0x000216E3
		public string Author
		{
			get
			{
				return this.author;
			}
			set
			{
				this.author = RssDefault.Check(value);
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x000234F1 File Offset: 0x000216F1
		public RssCategoryCollection Categories
		{
			get
			{
				return this.categories;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x000234F9 File Offset: 0x000216F9
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x00023501 File Offset: 0x00021701
		public string Comments
		{
			get
			{
				return this.comments;
			}
			set
			{
				this.comments = RssDefault.Check(value);
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x0002350F File Offset: 0x0002170F
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x00023517 File Offset: 0x00021717
		public RssSource Source
		{
			get
			{
				return this.source;
			}
			set
			{
				this.source = value;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x00023520 File Offset: 0x00021720
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x00023528 File Offset: 0x00021728
		public RssEnclosure Enclosure
		{
			get
			{
				return this.enclosure;
			}
			set
			{
				this.enclosure = value;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00023531 File Offset: 0x00021731
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x00023539 File Offset: 0x00021739
		public RssGuid Guid
		{
			get
			{
				return this.guid;
			}
			set
			{
				this.guid = value;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00023542 File Offset: 0x00021742
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x0002354A File Offset: 0x0002174A
		public DateTime PubDate
		{
			get
			{
				return this.pubDate;
			}
			set
			{
				this.pubDate = value;
			}
		}

		// Token: 0x040002EE RID: 750
		private string title = "";

		// Token: 0x040002EF RID: 751
		private Uri link = RssDefault.Uri;

		// Token: 0x040002F0 RID: 752
		private string description = "";

		// Token: 0x040002F1 RID: 753
		private string author = "";

		// Token: 0x040002F2 RID: 754
		private RssCategoryCollection categories = new RssCategoryCollection();

		// Token: 0x040002F3 RID: 755
		private string comments = "";

		// Token: 0x040002F4 RID: 756
		private RssEnclosure enclosure;

		// Token: 0x040002F5 RID: 757
		private RssGuid guid;

		// Token: 0x040002F6 RID: 758
		private DateTime pubDate = RssDefault.DateTime;

		// Token: 0x040002F7 RID: 759
		private RssSource source;
	}
}
