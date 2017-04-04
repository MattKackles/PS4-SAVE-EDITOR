using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000FC RID: 252
	public abstract class BaseArchiveStorage : IArchiveStorage
	{
		// Token: 0x06000A69 RID: 2665 RVA: 0x000386C9 File Offset: 0x000368C9
		protected BaseArchiveStorage(FileUpdateMode updateMode)
		{
			this.updateMode_ = updateMode;
		}

		// Token: 0x06000A6A RID: 2666
		public abstract Stream GetTemporaryOutput();

		// Token: 0x06000A6B RID: 2667
		public abstract Stream ConvertTemporaryToFinal();

		// Token: 0x06000A6C RID: 2668
		public abstract Stream MakeTemporaryCopy(Stream stream);

		// Token: 0x06000A6D RID: 2669
		public abstract Stream OpenForDirectUpdate(Stream stream);

		// Token: 0x06000A6E RID: 2670
		public abstract void Dispose();

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x000386D8 File Offset: 0x000368D8
		public FileUpdateMode UpdateMode
		{
			get
			{
				return this.updateMode_;
			}
		}

		// Token: 0x04000571 RID: 1393
		private FileUpdateMode updateMode_;
	}
}
