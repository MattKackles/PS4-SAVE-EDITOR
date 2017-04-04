using System;
using System.Collections;

namespace Rss
{
	// Token: 0x02000071 RID: 113
	[Serializable]
	public class RssChannelCollection : CollectionBase
	{
		// Token: 0x170001CD RID: 461
		public RssChannel this[int index]
		{
			get
			{
				return (RssChannel)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00022552 File Offset: 0x00020752
		public int Add(RssChannel channel)
		{
			return base.List.Add(channel);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00022560 File Offset: 0x00020760
		public bool Contains(RssChannel rssChannel)
		{
			return base.List.Contains(rssChannel);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0002256E File Offset: 0x0002076E
		public void CopyTo(RssChannel[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0002257D File Offset: 0x0002077D
		public int IndexOf(RssChannel rssChannel)
		{
			return base.List.IndexOf(rssChannel);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0002258B File Offset: 0x0002078B
		public void Insert(int index, RssChannel channel)
		{
			base.List.Insert(index, channel);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0002259A File Offset: 0x0002079A
		public void Remove(RssChannel channel)
		{
			base.List.Remove(channel);
		}
	}
}
