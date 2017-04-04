using System;

namespace Be.Windows.Forms
{
	// Token: 0x02000047 RID: 71
	internal struct BytePositionInfo
	{
		// Token: 0x0600031B RID: 795 RVA: 0x00011A46 File Offset: 0x0000FC46
		public BytePositionInfo(long index, int characterPosition)
		{
			this._index = index;
			this._characterPosition = characterPosition;
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600031C RID: 796 RVA: 0x00011A56 File Offset: 0x0000FC56
		public int CharacterPosition
		{
			get
			{
				return this._characterPosition;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00011A5E File Offset: 0x0000FC5E
		public long Index
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x0400019C RID: 412
		private int _characterPosition;

		// Token: 0x0400019D RID: 413
		private long _index;
	}
}
