using System;

namespace Rss
{
	// Token: 0x02000078 RID: 120
	[Serializable]
	public class RssChannel : RssElement
	{
		// Token: 0x060005DA RID: 1498 RVA: 0x00022B49 File Offset: 0x00020D49
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
			return "RssChannel";
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x00022B6E File Offset: 0x00020D6E
		// (set) Token: 0x060005DC RID: 1500 RVA: 0x00022B76 File Offset: 0x00020D76
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

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x00022B84 File Offset: 0x00020D84
		// (set) Token: 0x060005DE RID: 1502 RVA: 0x00022B8C File Offset: 0x00020D8C
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

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x00022B9A File Offset: 0x00020D9A
		// (set) Token: 0x060005E0 RID: 1504 RVA: 0x00022BA2 File Offset: 0x00020DA2
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

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x00022BB0 File Offset: 0x00020DB0
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x00022BB8 File Offset: 0x00020DB8
		public string Language
		{
			get
			{
				return this.language;
			}
			set
			{
				this.language = RssDefault.Check(value);
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x00022BC6 File Offset: 0x00020DC6
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x00022BCE File Offset: 0x00020DCE
		public RssImage Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.image = value;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x00022BD7 File Offset: 0x00020DD7
		// (set) Token: 0x060005E6 RID: 1510 RVA: 0x00022BDF File Offset: 0x00020DDF
		public string Copyright
		{
			get
			{
				return this.copyright;
			}
			set
			{
				this.copyright = RssDefault.Check(value);
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x00022BED File Offset: 0x00020DED
		// (set) Token: 0x060005E8 RID: 1512 RVA: 0x00022BF5 File Offset: 0x00020DF5
		public string ManagingEditor
		{
			get
			{
				return this.managingEditor;
			}
			set
			{
				this.managingEditor = RssDefault.Check(value);
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x00022C03 File Offset: 0x00020E03
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x00022C0B File Offset: 0x00020E0B
		public string WebMaster
		{
			get
			{
				return this.webMaster;
			}
			set
			{
				this.webMaster = RssDefault.Check(value);
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x00022C19 File Offset: 0x00020E19
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x00022C21 File Offset: 0x00020E21
		public string Rating
		{
			get
			{
				return this.rating;
			}
			set
			{
				this.rating = RssDefault.Check(value);
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x00022C2F File Offset: 0x00020E2F
		// (set) Token: 0x060005EE RID: 1518 RVA: 0x00022C37 File Offset: 0x00020E37
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

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x00022C40 File Offset: 0x00020E40
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x00022C48 File Offset: 0x00020E48
		public DateTime LastBuildDate
		{
			get
			{
				return this.lastBuildDate;
			}
			set
			{
				this.lastBuildDate = value;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x00022C51 File Offset: 0x00020E51
		public RssCategoryCollection Categories
		{
			get
			{
				return this.categories;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x00022C59 File Offset: 0x00020E59
		// (set) Token: 0x060005F3 RID: 1523 RVA: 0x00022C61 File Offset: 0x00020E61
		public string Generator
		{
			get
			{
				return this.generator;
			}
			set
			{
				this.generator = RssDefault.Check(value);
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x00022C6F File Offset: 0x00020E6F
		// (set) Token: 0x060005F5 RID: 1525 RVA: 0x00022C77 File Offset: 0x00020E77
		public string Docs
		{
			get
			{
				return this.docs;
			}
			set
			{
				this.docs = RssDefault.Check(value);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x00022C85 File Offset: 0x00020E85
		// (set) Token: 0x060005F7 RID: 1527 RVA: 0x00022C8D File Offset: 0x00020E8D
		public RssTextInput TextInput
		{
			get
			{
				return this.textInput;
			}
			set
			{
				this.textInput = value;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x00022C96 File Offset: 0x00020E96
		// (set) Token: 0x060005F9 RID: 1529 RVA: 0x00022C9E File Offset: 0x00020E9E
		public bool[] SkipDays
		{
			get
			{
				return this.skipDays;
			}
			set
			{
				this.skipDays = value;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x00022CA7 File Offset: 0x00020EA7
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x00022CAF File Offset: 0x00020EAF
		public bool[] SkipHours
		{
			get
			{
				return this.skipHours;
			}
			set
			{
				this.skipHours = value;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x00022CB8 File Offset: 0x00020EB8
		// (set) Token: 0x060005FD RID: 1533 RVA: 0x00022CC0 File Offset: 0x00020EC0
		public RssCloud Cloud
		{
			get
			{
				return this.cloud;
			}
			set
			{
				this.cloud = value;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x00022CC9 File Offset: 0x00020EC9
		// (set) Token: 0x060005FF RID: 1535 RVA: 0x00022CD1 File Offset: 0x00020ED1
		public int TimeToLive
		{
			get
			{
				return this.timeToLive;
			}
			set
			{
				this.timeToLive = RssDefault.Check(value);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x00022CDF File Offset: 0x00020EDF
		public RssItemCollection Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x040002BD RID: 701
		private string title = "";

		// Token: 0x040002BE RID: 702
		private Uri link = RssDefault.Uri;

		// Token: 0x040002BF RID: 703
		private string description = "";

		// Token: 0x040002C0 RID: 704
		private string language = "";

		// Token: 0x040002C1 RID: 705
		private string copyright = "";

		// Token: 0x040002C2 RID: 706
		private string managingEditor = "";

		// Token: 0x040002C3 RID: 707
		private string webMaster = "";

		// Token: 0x040002C4 RID: 708
		private DateTime pubDate = RssDefault.DateTime;

		// Token: 0x040002C5 RID: 709
		private DateTime lastBuildDate = RssDefault.DateTime;

		// Token: 0x040002C6 RID: 710
		private RssCategoryCollection categories = new RssCategoryCollection();

		// Token: 0x040002C7 RID: 711
		private string generator = "";

		// Token: 0x040002C8 RID: 712
		private string docs = "";

		// Token: 0x040002C9 RID: 713
		private RssCloud cloud;

		// Token: 0x040002CA RID: 714
		private int timeToLive = -1;

		// Token: 0x040002CB RID: 715
		private RssImage image;

		// Token: 0x040002CC RID: 716
		private RssTextInput textInput;

		// Token: 0x040002CD RID: 717
		private bool[] skipHours = new bool[24];

		// Token: 0x040002CE RID: 718
		private bool[] skipDays = new bool[7];

		// Token: 0x040002CF RID: 719
		private string rating = "";

		// Token: 0x040002D0 RID: 720
		private RssItemCollection items = new RssItemCollection();
	}
}
