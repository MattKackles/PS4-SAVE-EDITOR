using System;

namespace Ionic.Zip
{
	// Token: 0x02000120 RID: 288
	public class ZipProgressEventArgs : EventArgs
	{
		// Token: 0x06000BF2 RID: 3058 RVA: 0x00042AB8 File Offset: 0x00040CB8
		internal ZipProgressEventArgs()
		{
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00042AC0 File Offset: 0x00040CC0
		internal ZipProgressEventArgs(string archiveName, ZipProgressEventType flavor)
		{
			this._archiveName = archiveName;
			this._flavor = flavor;
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x00042AD6 File Offset: 0x00040CD6
		// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x00042ADE File Offset: 0x00040CDE
		public int EntriesTotal
		{
			get
			{
				return this._entriesTotal;
			}
			set
			{
				this._entriesTotal = value;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x00042AE7 File Offset: 0x00040CE7
		// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x00042AEF File Offset: 0x00040CEF
		public ZipEntry CurrentEntry
		{
			get
			{
				return this._latestEntry;
			}
			set
			{
				this._latestEntry = value;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x00042AF8 File Offset: 0x00040CF8
		// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x00042B00 File Offset: 0x00040D00
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = (this._cancel || value);
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x00042B14 File Offset: 0x00040D14
		// (set) Token: 0x06000BFB RID: 3067 RVA: 0x00042B1C File Offset: 0x00040D1C
		public ZipProgressEventType EventType
		{
			get
			{
				return this._flavor;
			}
			set
			{
				this._flavor = value;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x00042B25 File Offset: 0x00040D25
		// (set) Token: 0x06000BFD RID: 3069 RVA: 0x00042B2D File Offset: 0x00040D2D
		public string ArchiveName
		{
			get
			{
				return this._archiveName;
			}
			set
			{
				this._archiveName = value;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x00042B36 File Offset: 0x00040D36
		// (set) Token: 0x06000BFF RID: 3071 RVA: 0x00042B3E File Offset: 0x00040D3E
		public long BytesTransferred
		{
			get
			{
				return this._bytesTransferred;
			}
			set
			{
				this._bytesTransferred = value;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x00042B47 File Offset: 0x00040D47
		// (set) Token: 0x06000C01 RID: 3073 RVA: 0x00042B4F File Offset: 0x00040D4F
		public long TotalBytesToTransfer
		{
			get
			{
				return this._totalBytesToTransfer;
			}
			set
			{
				this._totalBytesToTransfer = value;
			}
		}

		// Token: 0x0400064B RID: 1611
		private int _entriesTotal;

		// Token: 0x0400064C RID: 1612
		private bool _cancel;

		// Token: 0x0400064D RID: 1613
		private ZipEntry _latestEntry;

		// Token: 0x0400064E RID: 1614
		private ZipProgressEventType _flavor;

		// Token: 0x0400064F RID: 1615
		private string _archiveName;

		// Token: 0x04000650 RID: 1616
		private long _bytesTransferred;

		// Token: 0x04000651 RID: 1617
		private long _totalBytesToTransfer;
	}
}
