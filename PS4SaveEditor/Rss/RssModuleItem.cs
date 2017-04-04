using System;

namespace Rss
{
	// Token: 0x02000082 RID: 130
	[Serializable]
	public class RssModuleItem : RssElement
	{
		// Token: 0x06000669 RID: 1641 RVA: 0x00023664 File Offset: 0x00021864
		public RssModuleItem()
		{
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0002368D File Offset: 0x0002188D
		public RssModuleItem(string name)
		{
			this._sElementName = RssDefault.Check(name);
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000236C2 File Offset: 0x000218C2
		public RssModuleItem(string name, bool required) : this(name)
		{
			this._bRequired = required;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x000236D2 File Offset: 0x000218D2
		public RssModuleItem(string name, string text) : this(name)
		{
			this._sElementText = RssDefault.Check(text);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x000236E7 File Offset: 0x000218E7
		public RssModuleItem(string name, bool required, string text) : this(name, required)
		{
			this._sElementText = RssDefault.Check(text);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x000236FD File Offset: 0x000218FD
		public RssModuleItem(string name, string text, RssModuleItemCollection subElements) : this(name, text)
		{
			this._rssSubElements = subElements;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0002370E File Offset: 0x0002190E
		public RssModuleItem(string name, bool required, string text, RssModuleItemCollection subElements) : this(name, required, text)
		{
			this._rssSubElements = subElements;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00023721 File Offset: 0x00021921
		public override string ToString()
		{
			if (this._sElementName != null)
			{
				return this._sElementName;
			}
			if (this._sElementText != null)
			{
				return this._sElementText;
			}
			return "RssModuleItem";
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00023746 File Offset: 0x00021946
		// (set) Token: 0x06000672 RID: 1650 RVA: 0x0002374E File Offset: 0x0002194E
		public string Name
		{
			get
			{
				return this._sElementName;
			}
			set
			{
				this._sElementName = RssDefault.Check(value);
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0002375C File Offset: 0x0002195C
		// (set) Token: 0x06000674 RID: 1652 RVA: 0x00023764 File Offset: 0x00021964
		public string Text
		{
			get
			{
				return this._sElementText;
			}
			set
			{
				this._sElementText = RssDefault.Check(value);
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x00023772 File Offset: 0x00021972
		// (set) Token: 0x06000676 RID: 1654 RVA: 0x0002377A File Offset: 0x0002197A
		public RssModuleItemCollection SubElements
		{
			get
			{
				return this._rssSubElements;
			}
			set
			{
				this._rssSubElements = value;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x00023783 File Offset: 0x00021983
		public bool IsRequired
		{
			get
			{
				return this._bRequired;
			}
		}

		// Token: 0x040002FF RID: 767
		private bool _bRequired;

		// Token: 0x04000300 RID: 768
		private string _sElementName = "";

		// Token: 0x04000301 RID: 769
		private string _sElementText = "";

		// Token: 0x04000302 RID: 770
		private RssModuleItemCollection _rssSubElements = new RssModuleItemCollection();
	}
}
