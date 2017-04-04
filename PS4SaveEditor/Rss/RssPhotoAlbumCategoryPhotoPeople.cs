using System;

namespace Rss
{
	// Token: 0x02000085 RID: 133
	public sealed class RssPhotoAlbumCategoryPhotoPeople : RssModuleItemCollection
	{
		// Token: 0x0600067A RID: 1658 RVA: 0x000238C8 File Offset: 0x00021AC8
		public RssPhotoAlbumCategoryPhotoPeople()
		{
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x000238D0 File Offset: 0x00021AD0
		public RssPhotoAlbumCategoryPhotoPeople(string value)
		{
			this.Add(value);
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x000238E0 File Offset: 0x00021AE0
		public int Add(string value)
		{
			return base.Add(new RssModuleItem("person", true, value));
		}
	}
}
