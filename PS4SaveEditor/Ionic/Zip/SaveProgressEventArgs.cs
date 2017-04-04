using System;

namespace Ionic.Zip
{
	// Token: 0x02000123 RID: 291
	public class SaveProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000C0E RID: 3086 RVA: 0x00042C6E File Offset: 0x00040E6E
		internal SaveProgressEventArgs(string archiveName, bool before, int entriesTotal, int entriesSaved, ZipEntry entry) : base(archiveName, before ? ZipProgressEventType.Saving_BeforeWriteEntry : ZipProgressEventType.Saving_AfterWriteEntry)
		{
			base.EntriesTotal = entriesTotal;
			base.CurrentEntry = entry;
			this._entriesSaved = entriesSaved;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00042C97 File Offset: 0x00040E97
		internal SaveProgressEventArgs()
		{
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00042C9F File Offset: 0x00040E9F
		internal SaveProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00042CAC File Offset: 0x00040EAC
		internal static SaveProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesXferred, long totalBytes)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_EntryBytesRead)
			{
				ArchiveName = archiveName,
				CurrentEntry = entry,
				BytesTransferred = bytesXferred,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00042CE0 File Offset: 0x00040EE0
		internal static SaveProgressEventArgs Started(string archiveName)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Started);
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00042CF8 File Offset: 0x00040EF8
		internal static SaveProgressEventArgs Completed(string archiveName)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Completed);
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x00042D0F File Offset: 0x00040F0F
		public int EntriesSaved
		{
			get
			{
				return this._entriesSaved;
			}
		}

		// Token: 0x04000652 RID: 1618
		private int _entriesSaved;
	}
}
