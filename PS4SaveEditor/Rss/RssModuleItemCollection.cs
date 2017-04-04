using System;
using System.Collections;

namespace Rss
{
	// Token: 0x02000075 RID: 117
	public class RssModuleItemCollection : CollectionBase
	{
		// Token: 0x170001D2 RID: 466
		public RssModuleItem this[int index]
		{
			get
			{
				return (RssModuleItem)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0002294F File Offset: 0x00020B4F
		public int Add(RssModuleItem rssModuleItem)
		{
			return base.List.Add(rssModuleItem);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0002295D File Offset: 0x00020B5D
		public bool Contains(RssModuleItem rssModuleItem)
		{
			return base.List.Contains(rssModuleItem);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0002296B File Offset: 0x00020B6B
		public void CopyTo(RssModuleItem[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0002297A File Offset: 0x00020B7A
		public int IndexOf(RssModuleItem rssModuleItem)
		{
			return base.List.IndexOf(rssModuleItem);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00022988 File Offset: 0x00020B88
		public void Insert(int index, RssModuleItem rssModuleItem)
		{
			base.List.Insert(index, rssModuleItem);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00022997 File Offset: 0x00020B97
		public void Remove(RssModuleItem rssModuleItem)
		{
			base.List.Remove(rssModuleItem);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000229A5 File Offset: 0x00020BA5
		public void BindTo(int itemHashCode)
		{
			this._alBindTo.Add(itemHashCode);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000229B9 File Offset: 0x00020BB9
		public bool IsBoundTo(int itemHashCode)
		{
			return this._alBindTo.BinarySearch(0, this._alBindTo.Count, itemHashCode, null) >= 0;
		}

		// Token: 0x040002BC RID: 700
		private ArrayList _alBindTo = new ArrayList();
	}
}
