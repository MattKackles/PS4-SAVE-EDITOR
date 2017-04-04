using System;

namespace Be.Windows.Forms
{
	// Token: 0x02000048 RID: 72
	internal abstract class DataBlock
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600031E RID: 798
		public abstract long Length
		{
			get;
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00011A66 File Offset: 0x0000FC66
		public DataMap Map
		{
			get
			{
				return this._map;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000320 RID: 800 RVA: 0x00011A6E File Offset: 0x0000FC6E
		public DataBlock NextBlock
		{
			get
			{
				return this._nextBlock;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00011A76 File Offset: 0x0000FC76
		public DataBlock PreviousBlock
		{
			get
			{
				return this._previousBlock;
			}
		}

		// Token: 0x06000322 RID: 802
		public abstract void RemoveBytes(long position, long count);

		// Token: 0x0400019E RID: 414
		internal DataMap _map;

		// Token: 0x0400019F RID: 415
		internal DataBlock _nextBlock;

		// Token: 0x040001A0 RID: 416
		internal DataBlock _previousBlock;
	}
}
