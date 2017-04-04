using System;
using System.IO;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000DD RID: 221
	public class ZipEntryFactory : IEntryFactory
	{
		// Token: 0x0600094B RID: 2379 RVA: 0x00033F20 File Offset: 0x00032120
		public ZipEntryFactory()
		{
			this.nameTransform_ = new ZipNameTransform();
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00033F45 File Offset: 0x00032145
		public ZipEntryFactory(ZipEntryFactory.TimeSetting timeSetting)
		{
			this.timeSetting_ = timeSetting;
			this.nameTransform_ = new ZipNameTransform();
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00033F71 File Offset: 0x00032171
		public ZipEntryFactory(DateTime time)
		{
			this.timeSetting_ = ZipEntryFactory.TimeSetting.Fixed;
			this.FixedDateTime = time;
			this.nameTransform_ = new ZipNameTransform();
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x00033FA4 File Offset: 0x000321A4
		// (set) Token: 0x0600094F RID: 2383 RVA: 0x00033FAC File Offset: 0x000321AC
		public INameTransform NameTransform
		{
			get
			{
				return this.nameTransform_;
			}
			set
			{
				if (value == null)
				{
					this.nameTransform_ = new ZipNameTransform();
					return;
				}
				this.nameTransform_ = value;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x00033FC4 File Offset: 0x000321C4
		// (set) Token: 0x06000951 RID: 2385 RVA: 0x00033FCC File Offset: 0x000321CC
		public ZipEntryFactory.TimeSetting Setting
		{
			get
			{
				return this.timeSetting_;
			}
			set
			{
				this.timeSetting_ = value;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x00033FD5 File Offset: 0x000321D5
		// (set) Token: 0x06000953 RID: 2387 RVA: 0x00033FDD File Offset: 0x000321DD
		public DateTime FixedDateTime
		{
			get
			{
				return this.fixedDateTime_;
			}
			set
			{
				if (value.Year < 1970)
				{
					throw new ArgumentException("Value is too old to be valid", "value");
				}
				this.fixedDateTime_ = value;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x00034004 File Offset: 0x00032204
		// (set) Token: 0x06000955 RID: 2389 RVA: 0x0003400C File Offset: 0x0003220C
		public int GetAttributes
		{
			get
			{
				return this.getAttributes_;
			}
			set
			{
				this.getAttributes_ = value;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x00034015 File Offset: 0x00032215
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x0003401D File Offset: 0x0003221D
		public int SetAttributes
		{
			get
			{
				return this.setAttributes_;
			}
			set
			{
				this.setAttributes_ = value;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x00034026 File Offset: 0x00032226
		// (set) Token: 0x06000959 RID: 2393 RVA: 0x0003402E File Offset: 0x0003222E
		public bool IsUnicodeText
		{
			get
			{
				return this.isUnicodeText_;
			}
			set
			{
				this.isUnicodeText_ = value;
			}
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x00034037 File Offset: 0x00032237
		public ZipEntry MakeFileEntry(string fileName)
		{
			return this.MakeFileEntry(fileName, true);
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x00034044 File Offset: 0x00032244
		public ZipEntry MakeFileEntry(string fileName, bool useFileSystem)
		{
			ZipEntry zipEntry = new ZipEntry(this.nameTransform_.TransformFile(fileName));
			zipEntry.IsUnicodeText = this.isUnicodeText_;
			int num = 0;
			bool flag = this.setAttributes_ != 0;
			FileInfo fileInfo = null;
			if (useFileSystem)
			{
				fileInfo = new FileInfo(fileName);
			}
			if (fileInfo != null && fileInfo.Exists)
			{
				switch (this.timeSetting_)
				{
				case ZipEntryFactory.TimeSetting.LastWriteTime:
					zipEntry.DateTime = fileInfo.LastWriteTime;
					break;
				case ZipEntryFactory.TimeSetting.LastWriteTimeUtc:
					zipEntry.DateTime = fileInfo.LastWriteTimeUtc;
					break;
				case ZipEntryFactory.TimeSetting.CreateTime:
					zipEntry.DateTime = fileInfo.CreationTime;
					break;
				case ZipEntryFactory.TimeSetting.CreateTimeUtc:
					zipEntry.DateTime = fileInfo.CreationTimeUtc;
					break;
				case ZipEntryFactory.TimeSetting.LastAccessTime:
					zipEntry.DateTime = fileInfo.LastAccessTime;
					break;
				case ZipEntryFactory.TimeSetting.LastAccessTimeUtc:
					zipEntry.DateTime = fileInfo.LastAccessTimeUtc;
					break;
				case ZipEntryFactory.TimeSetting.Fixed:
					zipEntry.DateTime = this.fixedDateTime_;
					break;
				default:
					throw new ZipException("Unhandled time setting in MakeFileEntry");
				}
				zipEntry.Size = fileInfo.Length;
				flag = true;
				num = (int)(fileInfo.Attributes & (FileAttributes)this.getAttributes_);
			}
			else if (this.timeSetting_ == ZipEntryFactory.TimeSetting.Fixed)
			{
				zipEntry.DateTime = this.fixedDateTime_;
			}
			if (flag)
			{
				num |= this.setAttributes_;
				zipEntry.ExternalFileAttributes = num;
			}
			return zipEntry;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0003417C File Offset: 0x0003237C
		public ZipEntry MakeDirectoryEntry(string directoryName)
		{
			return this.MakeDirectoryEntry(directoryName, true);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00034188 File Offset: 0x00032388
		public ZipEntry MakeDirectoryEntry(string directoryName, bool useFileSystem)
		{
			ZipEntry zipEntry = new ZipEntry(this.nameTransform_.TransformDirectory(directoryName));
			zipEntry.IsUnicodeText = this.isUnicodeText_;
			zipEntry.Size = 0L;
			int num = 0;
			DirectoryInfo directoryInfo = null;
			if (useFileSystem)
			{
				directoryInfo = new DirectoryInfo(directoryName);
			}
			if (directoryInfo != null && directoryInfo.Exists)
			{
				switch (this.timeSetting_)
				{
				case ZipEntryFactory.TimeSetting.LastWriteTime:
					zipEntry.DateTime = directoryInfo.LastWriteTime;
					break;
				case ZipEntryFactory.TimeSetting.LastWriteTimeUtc:
					zipEntry.DateTime = directoryInfo.LastWriteTimeUtc;
					break;
				case ZipEntryFactory.TimeSetting.CreateTime:
					zipEntry.DateTime = directoryInfo.CreationTime;
					break;
				case ZipEntryFactory.TimeSetting.CreateTimeUtc:
					zipEntry.DateTime = directoryInfo.CreationTimeUtc;
					break;
				case ZipEntryFactory.TimeSetting.LastAccessTime:
					zipEntry.DateTime = directoryInfo.LastAccessTime;
					break;
				case ZipEntryFactory.TimeSetting.LastAccessTimeUtc:
					zipEntry.DateTime = directoryInfo.LastAccessTimeUtc;
					break;
				case ZipEntryFactory.TimeSetting.Fixed:
					zipEntry.DateTime = this.fixedDateTime_;
					break;
				default:
					throw new ZipException("Unhandled time setting in MakeDirectoryEntry");
				}
				num = (int)(directoryInfo.Attributes & (FileAttributes)this.getAttributes_);
			}
			else if (this.timeSetting_ == ZipEntryFactory.TimeSetting.Fixed)
			{
				zipEntry.DateTime = this.fixedDateTime_;
			}
			num |= (this.setAttributes_ | 16);
			zipEntry.ExternalFileAttributes = num;
			return zipEntry;
		}

		// Token: 0x04000508 RID: 1288
		private INameTransform nameTransform_;

		// Token: 0x04000509 RID: 1289
		private DateTime fixedDateTime_ = DateTime.Now;

		// Token: 0x0400050A RID: 1290
		private ZipEntryFactory.TimeSetting timeSetting_;

		// Token: 0x0400050B RID: 1291
		private bool isUnicodeText_;

		// Token: 0x0400050C RID: 1292
		private int getAttributes_ = -1;

		// Token: 0x0400050D RID: 1293
		private int setAttributes_;

		// Token: 0x020000DE RID: 222
		public enum TimeSetting
		{
			// Token: 0x0400050F RID: 1295
			LastWriteTime,
			// Token: 0x04000510 RID: 1296
			LastWriteTimeUtc,
			// Token: 0x04000511 RID: 1297
			CreateTime,
			// Token: 0x04000512 RID: 1298
			CreateTimeUtc,
			// Token: 0x04000513 RID: 1299
			LastAccessTime,
			// Token: 0x04000514 RID: 1300
			LastAccessTimeUtc,
			// Token: 0x04000515 RID: 1301
			Fixed
		}
	}
}
