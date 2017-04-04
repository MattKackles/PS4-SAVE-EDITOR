using System;

namespace Ionic.Zip
{
	// Token: 0x02000122 RID: 290
	public class AddProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000C09 RID: 3081 RVA: 0x00042C0A File Offset: 0x00040E0A
		internal AddProgressEventArgs()
		{
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00042C12 File Offset: 0x00040E12
		private AddProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00042C1C File Offset: 0x00040E1C
		internal static AddProgressEventArgs AfterEntry(string archiveName, ZipEntry entry, int entriesTotal)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_AfterAddEntry)
			{
				EntriesTotal = entriesTotal,
				CurrentEntry = entry
			};
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00042C40 File Offset: 0x00040E40
		internal static AddProgressEventArgs Started(string archiveName)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Started);
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00042C58 File Offset: 0x00040E58
		internal static AddProgressEventArgs Completed(string archiveName)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Completed);
		}
	}
}
