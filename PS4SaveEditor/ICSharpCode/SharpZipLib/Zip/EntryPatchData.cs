using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000100 RID: 256
	internal class EntryPatchData
	{
		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x00038A84 File Offset: 0x00036C84
		// (set) Token: 0x06000A88 RID: 2696 RVA: 0x00038A8C File Offset: 0x00036C8C
		public long SizePatchOffset
		{
			get
			{
				return this.sizePatchOffset_;
			}
			set
			{
				this.sizePatchOffset_ = value;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x00038A95 File Offset: 0x00036C95
		// (set) Token: 0x06000A8A RID: 2698 RVA: 0x00038A9D File Offset: 0x00036C9D
		public long CrcPatchOffset
		{
			get
			{
				return this.crcPatchOffset_;
			}
			set
			{
				this.crcPatchOffset_ = value;
			}
		}

		// Token: 0x0400057A RID: 1402
		private long sizePatchOffset_;

		// Token: 0x0400057B RID: 1403
		private long crcPatchOffset_;
	}
}
