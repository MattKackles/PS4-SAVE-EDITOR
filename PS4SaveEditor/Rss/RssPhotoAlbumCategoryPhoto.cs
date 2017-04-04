using System;

namespace Rss
{
	// Token: 0x02000087 RID: 135
	public sealed class RssPhotoAlbumCategoryPhoto : RssModuleItemCollection
	{
		// Token: 0x0600067F RID: 1663 RVA: 0x00023905 File Offset: 0x00021B05
		public RssPhotoAlbumCategoryPhoto(DateTime photoDate, string photoDescription, Uri photoLink)
		{
			this.Add(photoDate, photoDescription, photoLink);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00023917 File Offset: 0x00021B17
		public RssPhotoAlbumCategoryPhoto(DateTime photoDate, string photoDescription, Uri photoLink, RssPhotoAlbumCategoryPhotoPeople photoPeople)
		{
			this.Add(photoDate, photoDescription, photoLink, photoPeople);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0002392B File Offset: 0x00021B2B
		private int Add(DateTime photoDate, string photoDescription, Uri photoLink, RssPhotoAlbumCategoryPhotoPeople photoPeople)
		{
			this.Add(photoDate, photoDescription, photoLink);
			base.Add(new RssModuleItem("photoPeople", true, "", photoPeople));
			return -1;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00023954 File Offset: 0x00021B54
		private int Add(DateTime photoDate, string photoDescription, Uri photoLink)
		{
			base.Add(new RssModuleItem("photoDate", true, RssDefault.Check(photoDate.ToUniversalTime().ToString("r"))));
			base.Add(new RssModuleItem("photoDescription", false, RssDefault.Check(photoDescription)));
			base.Add(new RssModuleItem("photoLink", true, RssDefault.Check(photoLink).ToString()));
			return -1;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x000239C2 File Offset: 0x00021BC2
		public RssPhotoAlbumCategoryPhoto(string photoDate, string photoDescription, Uri photoLink)
		{
			this.Add(photoDate, photoDescription, photoLink);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x000239D4 File Offset: 0x00021BD4
		public RssPhotoAlbumCategoryPhoto(string photoDate, string photoDescription, Uri photoLink, RssPhotoAlbumCategoryPhotoPeople photoPeople)
		{
			this.Add(photoDate, photoDescription, photoLink, photoPeople);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x000239E8 File Offset: 0x00021BE8
		private int Add(string photoDate, string photoDescription, Uri photoLink, RssPhotoAlbumCategoryPhotoPeople photoPeople)
		{
			this.Add(photoDate, photoDescription, photoLink);
			base.Add(new RssModuleItem("photoPeople", true, "", photoPeople));
			return -1;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00023A10 File Offset: 0x00021C10
		private int Add(string photoDate, string photoDescription, Uri photoLink)
		{
			base.Add(new RssModuleItem("photoDate", true, RssDefault.Check(photoDate)));
			base.Add(new RssModuleItem("photoDescription", false, RssDefault.Check(photoDescription)));
			base.Add(new RssModuleItem("photoLink", true, RssDefault.Check(photoLink).ToString()));
			return -1;
		}
	}
}
