using System;

namespace Be.Windows.Forms
{
	// Token: 0x02000050 RID: 80
	internal sealed class FileDataBlock : DataBlock
	{
		// Token: 0x06000395 RID: 917 RVA: 0x00012EB7 File Offset: 0x000110B7
		public FileDataBlock(long fileOffset, long length)
		{
			this._fileOffset = fileOffset;
			this._length = length;
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00012ECD File Offset: 0x000110CD
		public long FileOffset
		{
			get
			{
				return this._fileOffset;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00012ED5 File Offset: 0x000110D5
		public override long Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00012EDD File Offset: 0x000110DD
		public void SetFileOffset(long value)
		{
			this._fileOffset = value;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00012EE6 File Offset: 0x000110E6
		public void RemoveBytesFromEnd(long count)
		{
			if (count > this._length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this._length -= count;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00012F0A File Offset: 0x0001110A
		public void RemoveBytesFromStart(long count)
		{
			if (count > this._length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this._fileOffset += count;
			this._length -= count;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00012F3C File Offset: 0x0001113C
		public override void RemoveBytes(long position, long count)
		{
			if (position > this._length)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			if (position + count > this._length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			long fileOffset = this._fileOffset;
			long num = this._length - count - position;
			long fileOffset2 = this._fileOffset + position + count;
			if (position > 0L && num > 0L)
			{
				this._fileOffset = fileOffset;
				this._length = position;
				this._map.AddAfter(this, new FileDataBlock(fileOffset2, num));
				return;
			}
			if (position > 0L)
			{
				this._fileOffset = fileOffset;
				this._length = position;
				return;
			}
			this._fileOffset = fileOffset2;
			this._length = num;
		}

		// Token: 0x040001BB RID: 443
		private long _length;

		// Token: 0x040001BC RID: 444
		private long _fileOffset;
	}
}
