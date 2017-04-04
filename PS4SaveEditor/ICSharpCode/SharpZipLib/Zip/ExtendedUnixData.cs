using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000E2 RID: 226
	public class ExtendedUnixData : ITaggedData
	{
		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x00034334 File Offset: 0x00032534
		public short TagID
		{
			get
			{
				return 21589;
			}
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0003433C File Offset: 0x0003253C
		public void SetData(byte[] data, int index, int count)
		{
			using (MemoryStream memoryStream = new MemoryStream(data, index, count, false))
			{
				using (ZipHelperStream zipHelperStream = new ZipHelperStream(memoryStream))
				{
					this._flags = (ExtendedUnixData.Flags)zipHelperStream.ReadByte();
					if ((byte)(this._flags & ExtendedUnixData.Flags.ModificationTime) != 0 && count >= 5)
					{
						int seconds = zipHelperStream.ReadLEInt();
						this._modificationTime = (new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime() + new TimeSpan(0, 0, 0, seconds, 0)).ToLocalTime();
					}
					if ((byte)(this._flags & ExtendedUnixData.Flags.AccessTime) != 0)
					{
						int seconds2 = zipHelperStream.ReadLEInt();
						this._lastAccessTime = (new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime() + new TimeSpan(0, 0, 0, seconds2, 0)).ToLocalTime();
					}
					if ((byte)(this._flags & ExtendedUnixData.Flags.CreateTime) != 0)
					{
						int seconds3 = zipHelperStream.ReadLEInt();
						this._createTime = (new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime() + new TimeSpan(0, 0, 0, seconds3, 0)).ToLocalTime();
					}
				}
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00034494 File Offset: 0x00032694
		public byte[] GetData()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (ZipHelperStream zipHelperStream = new ZipHelperStream(memoryStream))
				{
					zipHelperStream.IsStreamOwner = false;
					zipHelperStream.WriteByte((byte)this._flags);
					if ((byte)(this._flags & ExtendedUnixData.Flags.ModificationTime) != 0)
					{
						int value = (int)(this._modificationTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime()).TotalSeconds;
						zipHelperStream.WriteLEInt(value);
					}
					if ((byte)(this._flags & ExtendedUnixData.Flags.AccessTime) != 0)
					{
						int value2 = (int)(this._lastAccessTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime()).TotalSeconds;
						zipHelperStream.WriteLEInt(value2);
					}
					if ((byte)(this._flags & ExtendedUnixData.Flags.CreateTime) != 0)
					{
						int value3 = (int)(this._createTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime()).TotalSeconds;
						zipHelperStream.WriteLEInt(value3);
					}
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x000345E8 File Offset: 0x000327E8
		public static bool IsValidValue(DateTime value)
		{
			return value >= new DateTime(1901, 12, 13, 20, 45, 52) || value <= new DateTime(2038, 1, 19, 3, 14, 7);
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x0003461F File Offset: 0x0003281F
		// (set) Token: 0x06000971 RID: 2417 RVA: 0x00034627 File Offset: 0x00032827
		public DateTime ModificationTime
		{
			get
			{
				return this._modificationTime;
			}
			set
			{
				if (!ExtendedUnixData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._flags |= ExtendedUnixData.Flags.ModificationTime;
				this._modificationTime = value;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x00034652 File Offset: 0x00032852
		// (set) Token: 0x06000973 RID: 2419 RVA: 0x0003465A File Offset: 0x0003285A
		public DateTime AccessTime
		{
			get
			{
				return this._lastAccessTime;
			}
			set
			{
				if (!ExtendedUnixData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._flags |= ExtendedUnixData.Flags.AccessTime;
				this._lastAccessTime = value;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x00034685 File Offset: 0x00032885
		// (set) Token: 0x06000975 RID: 2421 RVA: 0x0003468D File Offset: 0x0003288D
		public DateTime CreateTime
		{
			get
			{
				return this._createTime;
			}
			set
			{
				if (!ExtendedUnixData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._flags |= ExtendedUnixData.Flags.CreateTime;
				this._createTime = value;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x000346B8 File Offset: 0x000328B8
		// (set) Token: 0x06000977 RID: 2423 RVA: 0x000346C0 File Offset: 0x000328C0
		private ExtendedUnixData.Flags Include
		{
			get
			{
				return this._flags;
			}
			set
			{
				this._flags = value;
			}
		}

		// Token: 0x04000518 RID: 1304
		private ExtendedUnixData.Flags _flags;

		// Token: 0x04000519 RID: 1305
		private DateTime _modificationTime = new DateTime(1970, 1, 1);

		// Token: 0x0400051A RID: 1306
		private DateTime _lastAccessTime = new DateTime(1970, 1, 1);

		// Token: 0x0400051B RID: 1307
		private DateTime _createTime = new DateTime(1970, 1, 1);

		// Token: 0x020000E3 RID: 227
		[Flags]
		public enum Flags : byte
		{
			// Token: 0x0400051D RID: 1309
			ModificationTime = 1,
			// Token: 0x0400051E RID: 1310
			AccessTime = 2,
			// Token: 0x0400051F RID: 1311
			CreateTime = 4
		}
	}
}
