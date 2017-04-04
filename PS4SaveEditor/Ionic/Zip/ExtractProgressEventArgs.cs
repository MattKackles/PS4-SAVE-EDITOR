using System;

namespace Ionic.Zip
{
	// Token: 0x02000124 RID: 292
	public class ExtractProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000C15 RID: 3093 RVA: 0x00042D17 File Offset: 0x00040F17
		internal ExtractProgressEventArgs(string archiveName, bool before, int entriesTotal, int entriesExtracted, ZipEntry entry, string extractLocation) : base(archiveName, before ? ZipProgressEventType.Extracting_BeforeExtractEntry : ZipProgressEventType.Extracting_AfterExtractEntry)
		{
			base.EntriesTotal = entriesTotal;
			base.CurrentEntry = entry;
			this._entriesExtracted = entriesExtracted;
			this._target = extractLocation;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00042D48 File Offset: 0x00040F48
		internal ExtractProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00042D52 File Offset: 0x00040F52
		internal ExtractProgressEventArgs()
		{
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00042D5C File Offset: 0x00040F5C
		internal static ExtractProgressEventArgs BeforeExtractEntry(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_BeforeExtractEntry,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00042D90 File Offset: 0x00040F90
		internal static ExtractProgressEventArgs ExtractExisting(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_ExtractEntryWouldOverwrite,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00042DC4 File Offset: 0x00040FC4
		internal static ExtractProgressEventArgs AfterExtractEntry(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_AfterExtractEntry,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00042DF8 File Offset: 0x00040FF8
		internal static ExtractProgressEventArgs ExtractAllStarted(string archiveName, string extractLocation)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_BeforeExtractAll)
			{
				_target = extractLocation
			};
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00042E18 File Offset: 0x00041018
		internal static ExtractProgressEventArgs ExtractAllCompleted(string archiveName, string extractLocation)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_AfterExtractAll)
			{
				_target = extractLocation
			};
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00042E38 File Offset: 0x00041038
		internal static ExtractProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesWritten, long totalBytes)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_EntryBytesWritten)
			{
				ArchiveName = archiveName,
				CurrentEntry = entry,
				BytesTransferred = bytesWritten,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x00042E6B File Offset: 0x0004106B
		public int EntriesExtracted
		{
			get
			{
				return this._entriesExtracted;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x00042E73 File Offset: 0x00041073
		public string ExtractLocation
		{
			get
			{
				return this._target;
			}
		}

		// Token: 0x04000653 RID: 1619
		private int _entriesExtracted;

		// Token: 0x04000654 RID: 1620
		private string _target;
	}
}
