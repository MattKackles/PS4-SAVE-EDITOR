using System;

namespace Rss
{
	// Token: 0x02000079 RID: 121
	[Serializable]
	public class RssCloud : RssElement
	{
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x00022D17 File Offset: 0x00020F17
		// (set) Token: 0x06000603 RID: 1539 RVA: 0x00022D1F File Offset: 0x00020F1F
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

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x00022D2D File Offset: 0x00020F2D
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x00022D35 File Offset: 0x00020F35
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				this.port = RssDefault.Check(value);
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00022D43 File Offset: 0x00020F43
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x00022D4B File Offset: 0x00020F4B
		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				this.path = RssDefault.Check(value);
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x00022D59 File Offset: 0x00020F59
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x00022D61 File Offset: 0x00020F61
		public string RegisterProcedure
		{
			get
			{
				return this.registerProcedure;
			}
			set
			{
				this.registerProcedure = RssDefault.Check(value);
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x00022D6F File Offset: 0x00020F6F
		// (set) Token: 0x0600060B RID: 1547 RVA: 0x00022D77 File Offset: 0x00020F77
		public RssCloudProtocol Protocol
		{
			get
			{
				return this.protocol;
			}
			set
			{
				this.protocol = value;
			}
		}

		// Token: 0x040002D1 RID: 721
		private RssCloudProtocol protocol;

		// Token: 0x040002D2 RID: 722
		private string domain = "";

		// Token: 0x040002D3 RID: 723
		private string path = "";

		// Token: 0x040002D4 RID: 724
		private string registerProcedure = "";

		// Token: 0x040002D5 RID: 725
		private int port = -1;
	}
}
