using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000E6 RID: 230
	public sealed class ZipExtraData : IDisposable
	{
		// Token: 0x06000985 RID: 2437 RVA: 0x00034947 File Offset: 0x00032B47
		public ZipExtraData()
		{
			this.Clear();
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00034955 File Offset: 0x00032B55
		public ZipExtraData(byte[] data)
		{
			if (data == null)
			{
				this._data = new byte[0];
				return;
			}
			this._data = data;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00034974 File Offset: 0x00032B74
		public byte[] GetEntryData()
		{
			if (this.Length > 65535)
			{
				throw new ZipException("Data exceeds maximum length");
			}
			return (byte[])this._data.Clone();
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0003499E File Offset: 0x00032B9E
		public void Clear()
		{
			if (this._data == null || this._data.Length != 0)
			{
				this._data = new byte[0];
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x000349BE File Offset: 0x00032BBE
		public int Length
		{
			get
			{
				return this._data.Length;
			}
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x000349C8 File Offset: 0x00032BC8
		public Stream GetStreamForTag(int tag)
		{
			Stream result = null;
			if (this.Find(tag))
			{
				result = new MemoryStream(this._data, this._index, this._readValueLength, false);
			}
			return result;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x000349FC File Offset: 0x00032BFC
		private ITaggedData GetData(short tag)
		{
			ITaggedData result = null;
			if (this.Find((int)tag))
			{
				result = ZipExtraData.Create(tag, this._data, this._readValueStart, this._readValueLength);
			}
			return result;
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00034A30 File Offset: 0x00032C30
		private static ITaggedData Create(short tag, byte[] data, int offset, int count)
		{
			ITaggedData taggedData;
			if (tag != 10)
			{
				if (tag != 21589)
				{
					taggedData = new RawTaggedData(tag);
				}
				else
				{
					taggedData = new ExtendedUnixData();
				}
			}
			else
			{
				taggedData = new NTTaggedData();
			}
			taggedData.SetData(data, offset, count);
			return taggedData;
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x00034A71 File Offset: 0x00032C71
		public int ValueLength
		{
			get
			{
				return this._readValueLength;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x00034A79 File Offset: 0x00032C79
		public int CurrentReadIndex
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x00034A81 File Offset: 0x00032C81
		public int UnreadCount
		{
			get
			{
				if (this._readValueStart > this._data.Length || this._readValueStart < 4)
				{
					throw new ZipException("Find must be called before calling a Read method");
				}
				return this._readValueStart + this._readValueLength - this._index;
			}
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00034ABC File Offset: 0x00032CBC
		public bool Find(int headerID)
		{
			this._readValueStart = this._data.Length;
			this._readValueLength = 0;
			this._index = 0;
			int num = this._readValueStart;
			int num2 = headerID - 1;
			while (num2 != headerID && this._index < this._data.Length - 3)
			{
				num2 = this.ReadShortInternal();
				num = this.ReadShortInternal();
				if (num2 != headerID)
				{
					this._index += num;
				}
			}
			bool flag = num2 == headerID && this._index + num <= this._data.Length;
			if (flag)
			{
				this._readValueStart = this._index;
				this._readValueLength = num;
			}
			return flag;
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00034B5C File Offset: 0x00032D5C
		public void AddEntry(ITaggedData taggedData)
		{
			if (taggedData == null)
			{
				throw new ArgumentNullException("taggedData");
			}
			this.AddEntry((int)taggedData.TagID, taggedData.GetData());
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00034B80 File Offset: 0x00032D80
		public void AddEntry(int headerID, byte[] fieldData)
		{
			if (headerID > 65535 || headerID < 0)
			{
				throw new ArgumentOutOfRangeException("headerID");
			}
			int num = (fieldData == null) ? 0 : fieldData.Length;
			if (num > 65535)
			{
				throw new ArgumentOutOfRangeException("fieldData", "exceeds maximum length");
			}
			int num2 = this._data.Length + num + 4;
			if (this.Find(headerID))
			{
				num2 -= this.ValueLength + 4;
			}
			if (num2 > 65535)
			{
				throw new ZipException("Data exceeds maximum length");
			}
			this.Delete(headerID);
			byte[] array = new byte[num2];
			this._data.CopyTo(array, 0);
			int index = this._data.Length;
			this._data = array;
			this.SetShort(ref index, headerID);
			this.SetShort(ref index, num);
			if (fieldData != null)
			{
				fieldData.CopyTo(array, index);
			}
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00034C43 File Offset: 0x00032E43
		public void StartNewEntry()
		{
			this._newEntry = new MemoryStream();
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00034C50 File Offset: 0x00032E50
		public void AddNewEntry(int headerID)
		{
			byte[] fieldData = this._newEntry.ToArray();
			this._newEntry = null;
			this.AddEntry(headerID, fieldData);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00034C78 File Offset: 0x00032E78
		public void AddData(byte data)
		{
			this._newEntry.WriteByte(data);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00034C86 File Offset: 0x00032E86
		public void AddData(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this._newEntry.Write(data, 0, data.Length);
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00034CA6 File Offset: 0x00032EA6
		public void AddLeShort(int toAdd)
		{
			this._newEntry.WriteByte((byte)toAdd);
			this._newEntry.WriteByte((byte)(toAdd >> 8));
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00034CC4 File Offset: 0x00032EC4
		public void AddLeInt(int toAdd)
		{
			this.AddLeShort((int)((short)toAdd));
			this.AddLeShort((int)((short)(toAdd >> 16)));
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00034CD9 File Offset: 0x00032ED9
		public void AddLeLong(long toAdd)
		{
			this.AddLeInt((int)(toAdd & (long)((ulong)-1)));
			this.AddLeInt((int)(toAdd >> 32));
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00034CF4 File Offset: 0x00032EF4
		public bool Delete(int headerID)
		{
			bool result = false;
			if (this.Find(headerID))
			{
				result = true;
				int num = this._readValueStart - 4;
				byte[] array = new byte[this._data.Length - (this.ValueLength + 4)];
				Array.Copy(this._data, 0, array, 0, num);
				int num2 = num + this.ValueLength + 4;
				Array.Copy(this._data, num2, array, num, this._data.Length - num2);
				this._data = array;
			}
			return result;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00034D68 File Offset: 0x00032F68
		public long ReadLong()
		{
			this.ReadCheck(8);
			return ((long)this.ReadInt() & (long)((ulong)-1)) | (long)this.ReadInt() << 32;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00034D88 File Offset: 0x00032F88
		public int ReadInt()
		{
			this.ReadCheck(4);
			int result = (int)this._data[this._index] + ((int)this._data[this._index + 1] << 8) + ((int)this._data[this._index + 2] << 16) + ((int)this._data[this._index + 3] << 24);
			this._index += 4;
			return result;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00034DF4 File Offset: 0x00032FF4
		public int ReadShort()
		{
			this.ReadCheck(2);
			int result = (int)this._data[this._index] + ((int)this._data[this._index + 1] << 8);
			this._index += 2;
			return result;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00034E38 File Offset: 0x00033038
		public int ReadByte()
		{
			int result = -1;
			if (this._index < this._data.Length && this._readValueStart + this._readValueLength > this._index)
			{
				result = (int)this._data[this._index];
				this._index++;
			}
			return result;
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00034E89 File Offset: 0x00033089
		public void Skip(int amount)
		{
			this.ReadCheck(amount);
			this._index += amount;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00034EA0 File Offset: 0x000330A0
		private void ReadCheck(int length)
		{
			if (this._readValueStart > this._data.Length || this._readValueStart < 4)
			{
				throw new ZipException("Find must be called before calling a Read method");
			}
			if (this._index > this._readValueStart + this._readValueLength - length)
			{
				throw new ZipException("End of extra data");
			}
			if (this._index + length < 4)
			{
				throw new ZipException("Cannot read before start of tag");
			}
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x00034F0C File Offset: 0x0003310C
		private int ReadShortInternal()
		{
			if (this._index > this._data.Length - 2)
			{
				throw new ZipException("End of extra data");
			}
			int result = (int)this._data[this._index] + ((int)this._data[this._index + 1] << 8);
			this._index += 2;
			return result;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00034F65 File Offset: 0x00033165
		private void SetShort(ref int index, int source)
		{
			this._data[index] = (byte)source;
			this._data[index + 1] = (byte)(source >> 8);
			index += 2;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00034F87 File Offset: 0x00033187
		public void Dispose()
		{
			if (this._newEntry != null)
			{
				this._newEntry.Close();
			}
		}

		// Token: 0x04000523 RID: 1315
		private int _index;

		// Token: 0x04000524 RID: 1316
		private int _readValueStart;

		// Token: 0x04000525 RID: 1317
		private int _readValueLength;

		// Token: 0x04000526 RID: 1318
		private MemoryStream _newEntry;

		// Token: 0x04000527 RID: 1319
		private byte[] _data;
	}
}
