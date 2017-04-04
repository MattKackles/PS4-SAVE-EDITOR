using System;
using System.Collections;

namespace Rss
{
	// Token: 0x02000076 RID: 118
	public class RssModuleItemCollectionCollection : CollectionBase
	{
		// Token: 0x170001D3 RID: 467
		public RssModuleItemCollection this[int index]
		{
			get
			{
				return (RssModuleItemCollection)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00022A14 File Offset: 0x00020C14
		public int Add(RssModuleItemCollection rssModuleItemCollection)
		{
			return base.List.Add(rssModuleItemCollection);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00022A22 File Offset: 0x00020C22
		public bool Contains(RssModuleItemCollection rssModuleItemCollection)
		{
			return base.List.Contains(rssModuleItemCollection);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00022A30 File Offset: 0x00020C30
		public void CopyTo(RssModuleItemCollection[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00022A3F File Offset: 0x00020C3F
		public int IndexOf(RssModuleItemCollection rssModuleItemCollection)
		{
			return base.List.IndexOf(rssModuleItemCollection);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00022A4D File Offset: 0x00020C4D
		public void Insert(int index, RssModuleItemCollection rssModuleItemCollection)
		{
			base.List.Insert(index, rssModuleItemCollection);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00022A5C File Offset: 0x00020C5C
		public void Remove(RssModuleItemCollection rssModuleItemCollection)
		{
			base.List.Remove(rssModuleItemCollection);
		}
	}
}
