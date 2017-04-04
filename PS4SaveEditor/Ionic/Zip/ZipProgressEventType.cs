using System;

namespace Ionic.Zip
{
	// Token: 0x0200011F RID: 287
	public enum ZipProgressEventType
	{
		// Token: 0x04000633 RID: 1587
		Adding_Started,
		// Token: 0x04000634 RID: 1588
		Adding_AfterAddEntry,
		// Token: 0x04000635 RID: 1589
		Adding_Completed,
		// Token: 0x04000636 RID: 1590
		Reading_Started,
		// Token: 0x04000637 RID: 1591
		Reading_BeforeReadEntry,
		// Token: 0x04000638 RID: 1592
		Reading_AfterReadEntry,
		// Token: 0x04000639 RID: 1593
		Reading_Completed,
		// Token: 0x0400063A RID: 1594
		Reading_ArchiveBytesRead,
		// Token: 0x0400063B RID: 1595
		Saving_Started,
		// Token: 0x0400063C RID: 1596
		Saving_BeforeWriteEntry,
		// Token: 0x0400063D RID: 1597
		Saving_AfterWriteEntry,
		// Token: 0x0400063E RID: 1598
		Saving_Completed,
		// Token: 0x0400063F RID: 1599
		Saving_AfterSaveTempArchive,
		// Token: 0x04000640 RID: 1600
		Saving_BeforeRenameTempArchive,
		// Token: 0x04000641 RID: 1601
		Saving_AfterRenameTempArchive,
		// Token: 0x04000642 RID: 1602
		Saving_AfterCompileSelfExtractor,
		// Token: 0x04000643 RID: 1603
		Saving_EntryBytesRead,
		// Token: 0x04000644 RID: 1604
		Extracting_BeforeExtractEntry,
		// Token: 0x04000645 RID: 1605
		Extracting_AfterExtractEntry,
		// Token: 0x04000646 RID: 1606
		Extracting_ExtractEntryWouldOverwrite,
		// Token: 0x04000647 RID: 1607
		Extracting_EntryBytesWritten,
		// Token: 0x04000648 RID: 1608
		Extracting_BeforeExtractAll,
		// Token: 0x04000649 RID: 1609
		Extracting_AfterExtractAll,
		// Token: 0x0400064A RID: 1610
		Error_Saving
	}
}
