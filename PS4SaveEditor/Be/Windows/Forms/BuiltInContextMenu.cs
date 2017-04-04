using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Be.Windows.Forms
{
	// Token: 0x02000042 RID: 66
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public sealed class BuiltInContextMenu : Component
	{
		// Token: 0x060002E6 RID: 742 RVA: 0x0001153A File Offset: 0x0000F73A
		internal BuiltInContextMenu(HexBox hexBox)
		{
			this._hexBox = hexBox;
			this._hexBox.ByteProviderChanged += new EventHandler(this.HexBox_ByteProviderChanged);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00011560 File Offset: 0x0000F760
		private void HexBox_ByteProviderChanged(object sender, EventArgs e)
		{
			this.CheckBuiltInContextMenu();
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00011568 File Offset: 0x0000F768
		private void CheckBuiltInContextMenu()
		{
			if (Util.DesignMode)
			{
				return;
			}
			if (this._contextMenuStrip == null)
			{
				ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
				this._cutToolStripMenuItem = new ToolStripMenuItem(this.CutMenuItemTextInternal, this.CutMenuItemImage, new EventHandler(this.CutMenuItem_Click));
				contextMenuStrip.Items.Add(this._cutToolStripMenuItem);
				this._copyToolStripMenuItem = new ToolStripMenuItem(this.CopyMenuItemTextInternal, this.CopyMenuItemImage, new EventHandler(this.CopyMenuItem_Click));
				contextMenuStrip.Items.Add(this._copyToolStripMenuItem);
				this._pasteToolStripMenuItem = new ToolStripMenuItem(this.PasteMenuItemTextInternal, this.PasteMenuItemImage, new EventHandler(this.PasteMenuItem_Click));
				contextMenuStrip.Items.Add(this._pasteToolStripMenuItem);
				contextMenuStrip.Items.Add(new ToolStripSeparator());
				this._selectAllToolStripMenuItem = new ToolStripMenuItem(this.SelectAllMenuItemTextInternal, this.SelectAllMenuItemImage, new EventHandler(this.SelectAllMenuItem_Click));
				contextMenuStrip.Items.Add(this._selectAllToolStripMenuItem);
				contextMenuStrip.Opening += new CancelEventHandler(this.BuildInContextMenuStrip_Opening);
				this._contextMenuStrip = contextMenuStrip;
			}
			if (this._hexBox.ByteProvider == null && this._hexBox.ContextMenuStrip != null)
			{
				this._hexBox.ContextMenuStrip = null;
				return;
			}
			if (this._hexBox.ByteProvider != null && this._hexBox.ContextMenuStrip == null)
			{
				this._hexBox.ContextMenuStrip = this._contextMenuStrip;
			}
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000116E0 File Offset: 0x0000F8E0
		private void BuildInContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			this._cutToolStripMenuItem.Enabled = this._hexBox.CanCut();
			this._copyToolStripMenuItem.Enabled = this._hexBox.CanCopy();
			this._pasteToolStripMenuItem.Enabled = this._hexBox.CanPaste();
			this._selectAllToolStripMenuItem.Enabled = this._hexBox.CanSelectAll();
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00011745 File Offset: 0x0000F945
		private void CutMenuItem_Click(object sender, EventArgs e)
		{
			this._hexBox.Copy();
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00011752 File Offset: 0x0000F952
		private void CopyMenuItem_Click(object sender, EventArgs e)
		{
			this._hexBox.Copy();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0001175F File Offset: 0x0000F95F
		private void PasteMenuItem_Click(object sender, EventArgs e)
		{
			this._hexBox.Copy();
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0001176C File Offset: 0x0000F96C
		private void SelectAllMenuItem_Click(object sender, EventArgs e)
		{
			this._hexBox.SelectAll();
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00011779 File Offset: 0x0000F979
		// (set) Token: 0x060002EF RID: 751 RVA: 0x00011781 File Offset: 0x0000F981
		[Category("BuiltIn-ContextMenu"), DefaultValue(null), Localizable(true)]
		public string CopyMenuItemText
		{
			get
			{
				return this._copyMenuItemText;
			}
			set
			{
				this._copyMenuItemText = value;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0001178A File Offset: 0x0000F98A
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x00011792 File Offset: 0x0000F992
		[DefaultValue(null), Category("BuiltIn-ContextMenu"), Localizable(true)]
		public string CutMenuItemText
		{
			get
			{
				return this._cutMenuItemText;
			}
			set
			{
				this._cutMenuItemText = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0001179B File Offset: 0x0000F99B
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x000117A3 File Offset: 0x0000F9A3
		[DefaultValue(null), Localizable(true), Category("BuiltIn-ContextMenu")]
		public string PasteMenuItemText
		{
			get
			{
				return this._pasteMenuItemText;
			}
			set
			{
				this._pasteMenuItemText = value;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x000117AC File Offset: 0x0000F9AC
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x000117B4 File Offset: 0x0000F9B4
		[DefaultValue(null), Category("BuiltIn-ContextMenu"), Localizable(true)]
		public string SelectAllMenuItemText
		{
			get
			{
				return this._selectAllMenuItemText;
			}
			set
			{
				this._selectAllMenuItemText = value;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x000117BD File Offset: 0x0000F9BD
		internal string CutMenuItemTextInternal
		{
			get
			{
				if (string.IsNullOrEmpty(this.CutMenuItemText))
				{
					return "Cut";
				}
				return this.CutMenuItemText;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x000117D8 File Offset: 0x0000F9D8
		internal string CopyMenuItemTextInternal
		{
			get
			{
				if (string.IsNullOrEmpty(this.CopyMenuItemText))
				{
					return "Copy";
				}
				return this.CopyMenuItemText;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x000117F3 File Offset: 0x0000F9F3
		internal string PasteMenuItemTextInternal
		{
			get
			{
				if (string.IsNullOrEmpty(this.PasteMenuItemText))
				{
					return "Paste";
				}
				return this.PasteMenuItemText;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x0001180E File Offset: 0x0000FA0E
		internal string SelectAllMenuItemTextInternal
		{
			get
			{
				if (string.IsNullOrEmpty(this.SelectAllMenuItemText))
				{
					return "SelectAll";
				}
				return this.SelectAllMenuItemText;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00011829 File Offset: 0x0000FA29
		// (set) Token: 0x060002FB RID: 763 RVA: 0x00011831 File Offset: 0x0000FA31
		[Category("BuiltIn-ContextMenu"), DefaultValue(null)]
		public Image CutMenuItemImage
		{
			get
			{
				return this._cutMenuItemImage;
			}
			set
			{
				this._cutMenuItemImage = value;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0001183A File Offset: 0x0000FA3A
		// (set) Token: 0x060002FD RID: 765 RVA: 0x00011842 File Offset: 0x0000FA42
		[Category("BuiltIn-ContextMenu"), DefaultValue(null)]
		public Image CopyMenuItemImage
		{
			get
			{
				return this._copyMenuItemImage;
			}
			set
			{
				this._copyMenuItemImage = value;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0001184B File Offset: 0x0000FA4B
		// (set) Token: 0x060002FF RID: 767 RVA: 0x00011853 File Offset: 0x0000FA53
		[Category("BuiltIn-ContextMenu"), DefaultValue(null)]
		public Image PasteMenuItemImage
		{
			get
			{
				return this._pasteMenuItemImage;
			}
			set
			{
				this._pasteMenuItemImage = value;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0001185C File Offset: 0x0000FA5C
		// (set) Token: 0x06000301 RID: 769 RVA: 0x00011864 File Offset: 0x0000FA64
		[DefaultValue(null), Category("BuiltIn-ContextMenu")]
		public Image SelectAllMenuItemImage
		{
			get
			{
				return this._selectAllMenuItemImage;
			}
			set
			{
				this._selectAllMenuItemImage = value;
			}
		}

		// Token: 0x0400018D RID: 397
		private HexBox _hexBox;

		// Token: 0x0400018E RID: 398
		private ContextMenuStrip _contextMenuStrip;

		// Token: 0x0400018F RID: 399
		private ToolStripMenuItem _cutToolStripMenuItem;

		// Token: 0x04000190 RID: 400
		private ToolStripMenuItem _copyToolStripMenuItem;

		// Token: 0x04000191 RID: 401
		private ToolStripMenuItem _pasteToolStripMenuItem;

		// Token: 0x04000192 RID: 402
		private ToolStripMenuItem _selectAllToolStripMenuItem;

		// Token: 0x04000193 RID: 403
		private string _copyMenuItemText;

		// Token: 0x04000194 RID: 404
		private string _cutMenuItemText;

		// Token: 0x04000195 RID: 405
		private string _pasteMenuItemText;

		// Token: 0x04000196 RID: 406
		private string _selectAllMenuItemText;

		// Token: 0x04000197 RID: 407
		private Image _cutMenuItemImage;

		// Token: 0x04000198 RID: 408
		private Image _copyMenuItemImage;

		// Token: 0x04000199 RID: 409
		private Image _pasteMenuItemImage;

		// Token: 0x0400019A RID: 410
		private Image _selectAllMenuItemImage;
	}
}
