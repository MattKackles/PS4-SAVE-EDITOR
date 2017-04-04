using System;

namespace Rss
{
	// Token: 0x02000083 RID: 131
	public sealed class RssBlogChannel : RssModule
	{
		// Token: 0x06000678 RID: 1656 RVA: 0x0002378C File Offset: 0x0002198C
		public RssBlogChannel(Uri blogRoll, Uri mySubscriptions, Uri blink, Uri changes)
		{
			base.NamespacePrefix = "blogChannel";
			base.NamespaceURL = new Uri("http://backend.userland.com/blogChannelModule");
			base.ChannelExtensions.Add(new RssModuleItem("blogRoll", true, RssDefault.Check(blogRoll.ToString())));
			base.ChannelExtensions.Add(new RssModuleItem("mySubscriptions", true, RssDefault.Check(mySubscriptions.ToString())));
			base.ChannelExtensions.Add(new RssModuleItem("blink", true, RssDefault.Check(blink.ToString())));
			base.ChannelExtensions.Add(new RssModuleItem("changes", true, RssDefault.Check(changes.ToString())));
		}
	}
}
