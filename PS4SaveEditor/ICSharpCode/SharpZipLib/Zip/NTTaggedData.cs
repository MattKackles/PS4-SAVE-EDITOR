using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000E4 RID: 228
	public class NTTaggedData : ITaggedData
	{
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x00034707 File Offset: 0x00032907
		public short TagID
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0003470C File Offset: 0x0003290C
		public void SetData(byte[] data, int index, int count)
		{
			using (MemoryStream memoryStream = new MemoryStream(data, index, count, false))
			{
				using (ZipHelperStream zipHelperStream = new ZipHelperStream(memoryStream))
				{
					zipHelperStream.ReadLEInt();
					while (zipHelperStream.Position < zipHelperStream.Length)
					{
						int num = zipHelperStream.ReadLEShort();
						int num2 = zipHelperStream.ReadLEShort();
						if (num == 1)
						{
							if (num2 >= 24)
							{
								long fileTime = zipHelperStream.ReadLELong();
								this._lastModificationTime = DateTime.FromFileTime(fileTime);
								long fileTime2 = zipHelperStream.ReadLELong();
								this._lastAccessTime = DateTime.FromFileTime(fileTime2);
								long fileTime3 = zipHelperStream.ReadLELong();
								this._createTime = DateTime.FromFileTime(fileTime3);
								break;
							}
							break;
						}
						else
						{
							zipHelperStream.Seek((long)num2, SeekOrigin.Current);
						}
					}
				}
			}
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x000347D8 File Offset: 0x000329D8
		public byte[] GetData()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (ZipHelperStream zipHelperStream = new ZipHelperStream(memoryStream))
				{
					zipHelperStream.IsStreamOwner = false;
					zipHelperStream.WriteLEInt(0);
					zipHelperStream.WriteLEShort(1);
					zipHelperStream.WriteLEShort(24);
					zipHelperStream.WriteLELong(this._lastModificationTime.ToFileTime());
					zipHelperStream.WriteLELong(this._lastAccessTime.ToFileTime());
					zipHelperStream.WriteLELong(this._createTime.ToFileTime());
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0003487C File Offset: 0x00032A7C
		public static bool IsValidValue(DateTime value)
		{
			bool result = true;
			try
			{
				value.ToFileTimeUtc();
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x000348AC File Offset: 0x00032AAC
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x000348B4 File Offset: 0x00032AB4
		public DateTime LastModificationTime
		{
			get
			{
				return this._lastModificationTime;
			}
			set
			{
				if (!NTTaggedData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._lastModificationTime = value;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x000348D0 File Offset: 0x00032AD0
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x000348D8 File Offset: 0x00032AD8
		public DateTime CreateTime
		{
			get
			{
				return this._createTime;
			}
			set
			{
				if (!NTTaggedData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._createTime = value;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x000348F4 File Offset: 0x00032AF4
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x000348FC File Offset: 0x00032AFC
		public DateTime LastAccessTime
		{
			get
			{
				return this._lastAccessTime;
			}
			set
			{
				if (!NTTaggedData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._lastAccessTime = value;
			}
		}

		// Token: 0x04000520 RID: 1312
		private DateTime _lastAccessTime = DateTime.FromFileTime(0L);

		// Token: 0x04000521 RID: 1313
		private DateTime _lastModificationTime = DateTime.FromFileTime(0L);

		// Token: 0x04000522 RID: 1314
		private DateTime _createTime = DateTime.FromFileTime(0L);
	}
}
