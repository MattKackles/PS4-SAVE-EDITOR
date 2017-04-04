using System;

namespace Ionic.Zip
{
	// Token: 0x0200015D RID: 349
	public class SelfExtractorSaveOptions
	{
		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x00056A70 File Offset: 0x00054C70
		// (set) Token: 0x06000EBE RID: 3774 RVA: 0x00056A78 File Offset: 0x00054C78
		public SelfExtractorFlavor Flavor
		{
			get;
			set;
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x00056A81 File Offset: 0x00054C81
		// (set) Token: 0x06000EC0 RID: 3776 RVA: 0x00056A89 File Offset: 0x00054C89
		public string PostExtractCommandLine
		{
			get;
			set;
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x00056A92 File Offset: 0x00054C92
		// (set) Token: 0x06000EC2 RID: 3778 RVA: 0x00056A9A File Offset: 0x00054C9A
		public string DefaultExtractDirectory
		{
			get;
			set;
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x00056AA3 File Offset: 0x00054CA3
		// (set) Token: 0x06000EC4 RID: 3780 RVA: 0x00056AAB File Offset: 0x00054CAB
		public string IconFile
		{
			get;
			set;
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x00056AB4 File Offset: 0x00054CB4
		// (set) Token: 0x06000EC6 RID: 3782 RVA: 0x00056ABC File Offset: 0x00054CBC
		public bool Quiet
		{
			get;
			set;
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x00056AC5 File Offset: 0x00054CC5
		// (set) Token: 0x06000EC8 RID: 3784 RVA: 0x00056ACD File Offset: 0x00054CCD
		public ExtractExistingFileAction ExtractExistingFile
		{
			get;
			set;
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x00056AD6 File Offset: 0x00054CD6
		// (set) Token: 0x06000ECA RID: 3786 RVA: 0x00056ADE File Offset: 0x00054CDE
		public bool RemoveUnpackedFilesAfterExecute
		{
			get;
			set;
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x00056AE7 File Offset: 0x00054CE7
		// (set) Token: 0x06000ECC RID: 3788 RVA: 0x00056AEF File Offset: 0x00054CEF
		public Version FileVersion
		{
			get;
			set;
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x00056AF8 File Offset: 0x00054CF8
		// (set) Token: 0x06000ECE RID: 3790 RVA: 0x00056B00 File Offset: 0x00054D00
		public string ProductVersion
		{
			get;
			set;
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x00056B09 File Offset: 0x00054D09
		// (set) Token: 0x06000ED0 RID: 3792 RVA: 0x00056B11 File Offset: 0x00054D11
		public string Copyright
		{
			get;
			set;
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x00056B1A File Offset: 0x00054D1A
		// (set) Token: 0x06000ED2 RID: 3794 RVA: 0x00056B22 File Offset: 0x00054D22
		public string Description
		{
			get;
			set;
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x00056B2B File Offset: 0x00054D2B
		// (set) Token: 0x06000ED4 RID: 3796 RVA: 0x00056B33 File Offset: 0x00054D33
		public string ProductName
		{
			get;
			set;
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x00056B3C File Offset: 0x00054D3C
		// (set) Token: 0x06000ED6 RID: 3798 RVA: 0x00056B44 File Offset: 0x00054D44
		public string SfxExeWindowTitle
		{
			get;
			set;
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x00056B4D File Offset: 0x00054D4D
		// (set) Token: 0x06000ED8 RID: 3800 RVA: 0x00056B55 File Offset: 0x00054D55
		public string AdditionalCompilerSwitches
		{
			get;
			set;
		}
	}
}
