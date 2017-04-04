using System;

namespace Be.Windows.Forms
{
	// Token: 0x02000058 RID: 88
	internal sealed class MemoryDataBlock : DataBlock
	{
		// Token: 0x060004B6 RID: 1206 RVA: 0x0001876C File Offset: 0x0001696C
		public MemoryDataBlock(byte data)
		{
			this._data = new byte[]
			{
				data
			};
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00018791 File Offset: 0x00016991
		public MemoryDataBlock(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this._data = (byte[])data.Clone();
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x000187B8 File Offset: 0x000169B8
		public override long Length
		{
			get
			{
				return this._data.LongLength;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x000187C5 File Offset: 0x000169C5
		public byte[] Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000187D0 File Offset: 0x000169D0
		public void AddByteToEnd(byte value)
		{
			byte[] array = new byte[this._data.LongLength + 1L];
			this._data.CopyTo(array, 0);
			array[(int)(checked((IntPtr)(unchecked(array.LongLength - 1L))))] = value;
			this._data = array;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00018814 File Offset: 0x00016A14
		public void AddByteToStart(byte value)
		{
			byte[] array = new byte[this._data.LongLength + 1L];
			array[0] = value;
			this._data.CopyTo(array, 1);
			this._data = array;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00018850 File Offset: 0x00016A50
		public void InsertBytes(long position, byte[] data)
		{
			byte[] array = new byte[this._data.LongLength + data.LongLength];
			if (position > 0L)
			{
				Array.Copy(this._data, 0L, array, 0L, position);
			}
			Array.Copy(data, 0L, array, position, data.LongLength);
			if (position < this._data.LongLength)
			{
				Array.Copy(this._data, position, array, position + data.LongLength, this._data.LongLength - position);
			}
			this._data = array;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000188D4 File Offset: 0x00016AD4
		public override void RemoveBytes(long position, long count)
		{
			byte[] array = new byte[this._data.LongLength - count];
			if (position > 0L)
			{
				Array.Copy(this._data, 0L, array, 0L, position);
			}
			if (position + count < this._data.LongLength)
			{
				Array.Copy(this._data, position + count, array, position, array.LongLength - position);
			}
			this._data = array;
		}

		// Token: 0x0400021E RID: 542
		private byte[] _data;
	}
}
