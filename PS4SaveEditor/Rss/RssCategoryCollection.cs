using System;
using System.Collections;

namespace Rss
{
	// Token: 0x02000070 RID: 112
	[Serializable]
	public class RssCategoryCollection : CollectionBase
	{
		// Token: 0x170001CC RID: 460
		public RssCategory this[int index]
		{
			get
			{
				return (RssCategory)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x000224D2 File Offset: 0x000206D2
		public int Add(RssCategory rssCategory)
		{
			return base.List.Add(rssCategory);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x000224E0 File Offset: 0x000206E0
		public bool Contains(RssCategory rssCategory)
		{
			return base.List.Contains(rssCategory);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x000224EE File Offset: 0x000206EE
		public void CopyTo(RssCategory[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x000224FD File Offset: 0x000206FD
		public int IndexOf(RssCategory rssCategory)
		{
			return base.List.IndexOf(rssCategory);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0002250B File Offset: 0x0002070B
		public void Insert(int index, RssCategory rssCategory)
		{
			base.List.Insert(index, rssCategory);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0002251A File Offset: 0x0002071A
		public void Remove(RssCategory rssCategory)
		{
			base.List.Remove(rssCategory);
		}
	}
}
