using System;
using System.Collections;

namespace Rss
{
	// Token: 0x02000074 RID: 116
	public class RssModuleCollection : CollectionBase
	{
		// Token: 0x170001D1 RID: 465
		public RssModule this[int index]
		{
			get
			{
				return (RssModule)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x000228CF File Offset: 0x00020ACF
		public int Add(RssModule rssModule)
		{
			return base.List.Add(rssModule);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000228DD File Offset: 0x00020ADD
		public bool Contains(RssModule rssModule)
		{
			return base.List.Contains(rssModule);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x000228EB File Offset: 0x00020AEB
		public void CopyTo(RssModule[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000228FA File Offset: 0x00020AFA
		public int IndexOf(RssModule rssModule)
		{
			return base.List.IndexOf(rssModule);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00022908 File Offset: 0x00020B08
		public void Insert(int index, RssModule rssModule)
		{
			base.List.Insert(index, rssModule);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00022917 File Offset: 0x00020B17
		public void Remove(RssModule rssModule)
		{
			base.List.Remove(rssModule);
		}
	}
}
