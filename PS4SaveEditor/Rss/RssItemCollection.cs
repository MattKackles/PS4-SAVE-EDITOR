using System;
using System.Collections;

namespace Rss
{
	// Token: 0x02000073 RID: 115
	public class RssItemCollection : CollectionBase
	{
		// Token: 0x170001D0 RID: 464
		public RssItem this[int index]
		{
			get
			{
				return (RssItem)base.List[index];
			}
			set
			{
				this.pubDateChanged = true;
				base.List[index] = value;
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x000226A5 File Offset: 0x000208A5
		public int Add(RssItem item)
		{
			this.pubDateChanged = true;
			return base.List.Add(item);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x000226BA File Offset: 0x000208BA
		public bool Contains(RssItem rssItem)
		{
			return base.List.Contains(rssItem);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000226C8 File Offset: 0x000208C8
		public void CopyTo(RssItem[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x000226D7 File Offset: 0x000208D7
		public int IndexOf(RssItem rssItem)
		{
			return base.List.IndexOf(rssItem);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x000226E5 File Offset: 0x000208E5
		public void Insert(int index, RssItem item)
		{
			this.pubDateChanged = true;
			base.List.Insert(index, item);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x000226FB File Offset: 0x000208FB
		public void Remove(RssItem item)
		{
			this.pubDateChanged = true;
			base.List.Remove(item);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00022710 File Offset: 0x00020910
		public DateTime LatestPubDate()
		{
			this.CalculatePubDates();
			return this.latestPubDate;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0002271E File Offset: 0x0002091E
		public DateTime OldestPubDate()
		{
			this.CalculatePubDates();
			return this.oldestPubDate;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0002272C File Offset: 0x0002092C
		private void CalculatePubDates()
		{
			if (this.pubDateChanged)
			{
				this.pubDateChanged = false;
				this.latestPubDate = DateTime.MinValue;
				this.oldestPubDate = DateTime.MaxValue;
				foreach (RssItem rssItem in base.List)
				{
					if (rssItem.PubDate != RssDefault.DateTime & rssItem.PubDate > this.latestPubDate)
					{
						this.latestPubDate = rssItem.PubDate;
					}
				}
				if (this.latestPubDate == DateTime.MinValue)
				{
					this.latestPubDate = RssDefault.DateTime;
				}
				foreach (RssItem rssItem2 in base.List)
				{
					if (rssItem2.PubDate != RssDefault.DateTime & rssItem2.PubDate < this.oldestPubDate)
					{
						this.oldestPubDate = rssItem2.PubDate;
					}
				}
				if (this.oldestPubDate == DateTime.MaxValue)
				{
					this.oldestPubDate = RssDefault.DateTime;
				}
			}
		}

		// Token: 0x040002B9 RID: 697
		private DateTime latestPubDate = RssDefault.DateTime;

		// Token: 0x040002BA RID: 698
		private DateTime oldestPubDate = RssDefault.DateTime;

		// Token: 0x040002BB RID: 699
		private bool pubDateChanged = true;
	}
}
