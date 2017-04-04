using System;

namespace Rss
{
	// Token: 0x0200008A RID: 138
	public sealed class RssPhotoAlbum : RssModule
	{
		// Token: 0x06000691 RID: 1681 RVA: 0x00023E48 File Offset: 0x00022048
		public RssPhotoAlbum(Uri link, RssPhotoAlbumCategory photoAlbumCategory)
		{
			base.NamespacePrefix = "photoAlbum";
			base.NamespaceURL = new Uri("http://xml.innothinx.com/photoAlbum");
			base.ChannelExtensions.Add(new RssModuleItem("link", true, RssDefault.Check(link).ToString()));
			base.ItemExtensions.Add(photoAlbumCategory);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00023EA8 File Offset: 0x000220A8
		public RssPhotoAlbum(Uri link, RssPhotoAlbumCategories photoAlbumCategories)
		{
			base.NamespacePrefix = "photoAlbum";
			base.NamespaceURL = new Uri("http://xml.innothinx.com/photoAlbum");
			base.ChannelExtensions.Add(new RssModuleItem("link", true, RssDefault.Check(link).ToString()));
			foreach (RssModuleItemCollection rssModuleItemCollection in photoAlbumCategories)
			{
				base.ItemExtensions.Add(rssModuleItemCollection);
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x00023F48 File Offset: 0x00022148
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x00023F84 File Offset: 0x00022184
		public Uri Link
		{
			get
			{
				if (!(RssDefault.Check(base.ChannelExtensions[0].Text) == ""))
				{
					return new Uri(base.ChannelExtensions[0].Text);
				}
				return null;
			}
			set
			{
				base.ChannelExtensions[0].Text = ((RssDefault.Check(value) == RssDefault.Uri) ? "" : value.ToString());
			}
		}
	}
}
