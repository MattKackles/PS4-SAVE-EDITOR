using System;
using System.Collections;

namespace Rss
{
	// Token: 0x02000072 RID: 114
	[Serializable]
	public class RssFeedCollection : CollectionBase
	{
		// Token: 0x170001CE RID: 462
		public RssFeed this[int index]
		{
			get
			{
				return (RssFeed)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x170001CF RID: 463
		public RssFeed this[string url]
		{
			get
			{
				for (int i = 0; i < base.List.Count; i++)
				{
					if (((RssFeed)base.List[i]).Url == url)
					{
						return this[i];
					}
				}
				return null;
			}
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0002261E File Offset: 0x0002081E
		public int Add(RssFeed feed)
		{
			return base.List.Add(feed);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0002262C File Offset: 0x0002082C
		public bool Contains(RssFeed rssFeed)
		{
			return base.List.Contains(rssFeed);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0002263A File Offset: 0x0002083A
		public void CopyTo(RssFeed[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00022649 File Offset: 0x00020849
		public int IndexOf(RssFeed rssFeed)
		{
			return base.List.IndexOf(rssFeed);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00022657 File Offset: 0x00020857
		public void Insert(int index, RssFeed feed)
		{
			base.List.Insert(index, feed);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00022666 File Offset: 0x00020866
		public void Remove(RssFeed feed)
		{
			base.List.Remove(feed);
		}
	}
}
