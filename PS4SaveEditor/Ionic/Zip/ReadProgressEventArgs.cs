using System;

namespace Ionic.Zip
{
	// Token: 0x02000121 RID: 289
	public class ReadProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000C02 RID: 3074 RVA: 0x00042B58 File Offset: 0x00040D58
		internal ReadProgressEventArgs()
		{
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00042B60 File Offset: 0x00040D60
		private ReadProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00042B6C File Offset: 0x00040D6C
		internal static ReadProgressEventArgs Before(string archiveName, int entriesTotal)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_BeforeReadEntry)
			{
				EntriesTotal = entriesTotal
			};
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00042B8C File Offset: 0x00040D8C
		internal static ReadProgressEventArgs After(string archiveName, ZipEntry entry, int entriesTotal)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_AfterReadEntry)
			{
				EntriesTotal = entriesTotal,
				CurrentEntry = entry
			};
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00042BB0 File Offset: 0x00040DB0
		internal static ReadProgressEventArgs Started(string archiveName)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_Started);
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00042BC8 File Offset: 0x00040DC8
		internal static ReadProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesXferred, long totalBytes)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_ArchiveBytesRead)
			{
				CurrentEntry = entry,
				BytesTransferred = bytesXferred,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00042BF4 File Offset: 0x00040DF4
		internal static ReadProgressEventArgs Completed(string archiveName)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_Completed);
		}
	}
}
