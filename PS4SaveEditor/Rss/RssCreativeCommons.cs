using System;

namespace Rss
{
	// Token: 0x02000084 RID: 132
	public sealed class RssCreativeCommons : RssModule
	{
		// Token: 0x06000679 RID: 1657 RVA: 0x00023844 File Offset: 0x00021A44
		public RssCreativeCommons(Uri license, bool isChannelSubElement)
		{
			base.NamespacePrefix = "creativeCommons";
			base.NamespaceURL = new Uri("http://backend.userland.com/creativeCommonsRssModule");
			if (isChannelSubElement)
			{
				base.ChannelExtensions.Add(new RssModuleItem("license", true, RssDefault.Check(license.ToString())));
				return;
			}
			RssModuleItemCollection rssModuleItemCollection = new RssModuleItemCollection();
			rssModuleItemCollection.Add(new RssModuleItem("license", true, RssDefault.Check(license.ToString())));
			base.ItemExtensions.Add(rssModuleItemCollection);
		}
	}
}
