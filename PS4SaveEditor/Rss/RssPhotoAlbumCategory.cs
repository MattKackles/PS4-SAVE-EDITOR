using System;

namespace Rss
{
	// Token: 0x02000089 RID: 137
	public sealed class RssPhotoAlbumCategory : RssModuleItemCollection
	{
		// Token: 0x06000689 RID: 1673 RVA: 0x00023A7C File Offset: 0x00021C7C
		public RssPhotoAlbumCategory(string categoryName, string categoryDescription, DateTime categoryDateFrom, DateTime categoryDateTo, RssPhotoAlbumCategoryPhoto categoryPhoto)
		{
			this.Add(categoryName, categoryDescription, categoryDateFrom, categoryDateTo, categoryPhoto);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00023A94 File Offset: 0x00021C94
		private int Add(string categoryName, string categoryDescription, DateTime categoryDateFrom, DateTime categoryDateTo, RssPhotoAlbumCategoryPhoto categoryPhoto)
		{
			RssModuleItemCollection rssModuleItemCollection = new RssModuleItemCollection();
			rssModuleItemCollection.Add(new RssModuleItem("from", true, RssDefault.Check(categoryDateFrom.ToUniversalTime().ToString("r"))));
			rssModuleItemCollection.Add(new RssModuleItem("to", true, RssDefault.Check(categoryDateTo.ToUniversalTime().ToString("r"))));
			base.Add(new RssModuleItem("categoryName", true, RssDefault.Check(categoryName)));
			base.Add(new RssModuleItem("categoryDescription", true, RssDefault.Check(categoryDescription)));
			base.Add(new RssModuleItem("categoryDateRange", true, "", rssModuleItemCollection));
			base.Add(new RssModuleItem("categoryPhoto", true, "", categoryPhoto));
			return -1;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00023B5F File Offset: 0x00021D5F
		public RssPhotoAlbumCategory(string categoryName, string categoryDescription, string categoryDateFrom, string categoryDateTo, RssPhotoAlbumCategoryPhoto categoryPhoto)
		{
			this.Add(categoryName, categoryDescription, categoryDateFrom, categoryDateTo, categoryPhoto);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00023B78 File Offset: 0x00021D78
		private int Add(string categoryName, string categoryDescription, string categoryDateFrom, string categoryDateTo, RssPhotoAlbumCategoryPhoto categoryPhoto)
		{
			RssModuleItemCollection rssModuleItemCollection = new RssModuleItemCollection();
			rssModuleItemCollection.Add(new RssModuleItem("from", true, RssDefault.Check(categoryDateFrom)));
			rssModuleItemCollection.Add(new RssModuleItem("to", true, RssDefault.Check(categoryDateTo)));
			base.Add(new RssModuleItem("categoryName", true, RssDefault.Check(categoryName)));
			base.Add(new RssModuleItem("categoryDescription", true, RssDefault.Check(categoryDescription)));
			base.Add(new RssModuleItem("categoryDateRange", true, "", rssModuleItemCollection));
			base.Add(new RssModuleItem("categoryPhoto", true, "", categoryPhoto));
			return -1;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00023C1E File Offset: 0x00021E1E
		public RssPhotoAlbumCategory(string categoryName, string categoryDescription, DateTime categoryDateFrom, DateTime categoryDateTo, RssPhotoAlbumCategoryPhotos categoryPhotos)
		{
			this.Add(categoryName, categoryDescription, categoryDateFrom, categoryDateTo, categoryPhotos);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00023C34 File Offset: 0x00021E34
		private int Add(string categoryName, string categoryDescription, DateTime categoryDateFrom, DateTime categoryDateTo, RssPhotoAlbumCategoryPhotos categoryPhotos)
		{
			RssModuleItemCollection rssModuleItemCollection = new RssModuleItemCollection();
			rssModuleItemCollection.Add(new RssModuleItem("from", true, RssDefault.Check(categoryDateFrom.ToUniversalTime().ToString("r"))));
			rssModuleItemCollection.Add(new RssModuleItem("to", true, RssDefault.Check(categoryDateTo.ToUniversalTime().ToString("r"))));
			base.Add(new RssModuleItem("categoryName", true, RssDefault.Check(categoryName)));
			base.Add(new RssModuleItem("categoryDescription", true, RssDefault.Check(categoryDescription)));
			base.Add(new RssModuleItem("categoryDateRange", true, "", rssModuleItemCollection));
			foreach (RssPhotoAlbumCategoryPhoto subElements in categoryPhotos)
			{
				base.Add(new RssModuleItem("categoryPhoto", true, "", subElements));
			}
			return -1;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00023D48 File Offset: 0x00021F48
		public RssPhotoAlbumCategory(string categoryName, string categoryDescription, string categoryDateFrom, string categoryDateTo, RssPhotoAlbumCategoryPhotos categoryPhotos)
		{
			this.Add(categoryName, categoryDescription, categoryDateFrom, categoryDateTo, categoryPhotos);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00023D60 File Offset: 0x00021F60
		private int Add(string categoryName, string categoryDescription, string categoryDateFrom, string categoryDateTo, RssPhotoAlbumCategoryPhotos categoryPhotos)
		{
			RssModuleItemCollection rssModuleItemCollection = new RssModuleItemCollection();
			rssModuleItemCollection.Add(new RssModuleItem("from", true, RssDefault.Check(categoryDateFrom)));
			rssModuleItemCollection.Add(new RssModuleItem("to", true, RssDefault.Check(categoryDateTo)));
			base.Add(new RssModuleItem("categoryName", true, RssDefault.Check(categoryName)));
			base.Add(new RssModuleItem("categoryDescription", true, RssDefault.Check(categoryDescription)));
			base.Add(new RssModuleItem("categoryDateRange", true, "", rssModuleItemCollection));
			foreach (RssPhotoAlbumCategoryPhoto subElements in categoryPhotos)
			{
				base.Add(new RssModuleItem("categoryPhoto", true, "", subElements));
			}
			return -1;
		}
	}
}
