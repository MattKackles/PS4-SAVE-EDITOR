using System;
using System.Collections;

namespace Rss
{
	// Token: 0x02000081 RID: 129
	[Serializable]
	public abstract class RssModule
	{
		// Token: 0x0600065E RID: 1630 RVA: 0x0002359D File Offset: 0x0002179D
		public RssModule()
		{
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x000235DC File Offset: 0x000217DC
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x000235E4 File Offset: 0x000217E4
		internal RssModuleItemCollection ChannelExtensions
		{
			get
			{
				return this._rssChannelExtensions;
			}
			set
			{
				this._rssChannelExtensions = value;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x000235ED File Offset: 0x000217ED
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x000235F5 File Offset: 0x000217F5
		internal RssModuleItemCollectionCollection ItemExtensions
		{
			get
			{
				return this._rssItemExtensions;
			}
			set
			{
				this._rssItemExtensions = value;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x000235FE File Offset: 0x000217FE
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x00023606 File Offset: 0x00021806
		public string NamespacePrefix
		{
			get
			{
				return this._sNamespacePrefix;
			}
			set
			{
				this._sNamespacePrefix = RssDefault.Check(value);
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00023614 File Offset: 0x00021814
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x0002361C File Offset: 0x0002181C
		public Uri NamespaceURL
		{
			get
			{
				return this._uriNamespaceURL;
			}
			set
			{
				this._uriNamespaceURL = RssDefault.Check(value);
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0002362A File Offset: 0x0002182A
		public void BindTo(int channelHashCode)
		{
			this._alBindTo.Add(channelHashCode);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0002363E File Offset: 0x0002183E
		public bool IsBoundTo(int channelHashCode)
		{
			return this._alBindTo.BinarySearch(0, this._alBindTo.Count, channelHashCode, null) >= 0;
		}

		// Token: 0x040002FA RID: 762
		private ArrayList _alBindTo = new ArrayList();

		// Token: 0x040002FB RID: 763
		private RssModuleItemCollection _rssChannelExtensions = new RssModuleItemCollection();

		// Token: 0x040002FC RID: 764
		private RssModuleItemCollectionCollection _rssItemExtensions = new RssModuleItemCollectionCollection();

		// Token: 0x040002FD RID: 765
		private string _sNamespacePrefix = "";

		// Token: 0x040002FE RID: 766
		private Uri _uriNamespaceURL = RssDefault.Uri;
	}
}
