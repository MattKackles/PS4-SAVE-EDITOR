using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000E1 RID: 225
	public class RawTaggedData : ITaggedData
	{
		// Token: 0x06000965 RID: 2405 RVA: 0x000342D0 File Offset: 0x000324D0
		public RawTaggedData(short tag)
		{
			this._tag = tag;
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x000342DF File Offset: 0x000324DF
		// (set) Token: 0x06000967 RID: 2407 RVA: 0x000342E7 File Offset: 0x000324E7
		public short TagID
		{
			get
			{
				return this._tag;
			}
			set
			{
				this._tag = value;
			}
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x000342F0 File Offset: 0x000324F0
		public void SetData(byte[] data, int offset, int count)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this._data = new byte[count];
			Array.Copy(data, offset, this._data, 0, count);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0003431B File Offset: 0x0003251B
		public byte[] GetData()
		{
			return this._data;
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x00034323 File Offset: 0x00032523
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x0003432B File Offset: 0x0003252B
		public byte[] Data
		{
			get
			{
				return this._data;
			}
			set
			{
				this._data = value;
			}
		}

		// Token: 0x04000516 RID: 1302
		private short _tag;

		// Token: 0x04000517 RID: 1303
		private byte[] _data;
	}
}
