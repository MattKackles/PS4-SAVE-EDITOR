using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000FF RID: 255
	public class DescriptorData
	{
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x00038A46 File Offset: 0x00036C46
		// (set) Token: 0x06000A81 RID: 2689 RVA: 0x00038A4E File Offset: 0x00036C4E
		public long CompressedSize
		{
			get
			{
				return this.compressedSize;
			}
			set
			{
				this.compressedSize = value;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x00038A57 File Offset: 0x00036C57
		// (set) Token: 0x06000A83 RID: 2691 RVA: 0x00038A5F File Offset: 0x00036C5F
		public long Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x00038A68 File Offset: 0x00036C68
		// (set) Token: 0x06000A85 RID: 2693 RVA: 0x00038A70 File Offset: 0x00036C70
		public long Crc
		{
			get
			{
				return this.crc;
			}
			set
			{
				this.crc = (value & (long)((ulong)-1));
			}
		}

		// Token: 0x04000577 RID: 1399
		private long size;

		// Token: 0x04000578 RID: 1400
		private long compressedSize;

		// Token: 0x04000579 RID: 1401
		private long crc;
	}
}
