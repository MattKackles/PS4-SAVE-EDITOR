using System;
using System.IO;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000FE RID: 254
	public class MemoryArchiveStorage : BaseArchiveStorage
	{
		// Token: 0x06000A78 RID: 2680 RVA: 0x0003895C File Offset: 0x00036B5C
		public MemoryArchiveStorage() : base(FileUpdateMode.Direct)
		{
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00038965 File Offset: 0x00036B65
		public MemoryArchiveStorage(FileUpdateMode updateMode) : base(updateMode)
		{
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x0003896E File Offset: 0x00036B6E
		public MemoryStream FinalStream
		{
			get
			{
				return this.finalStream_;
			}
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00038976 File Offset: 0x00036B76
		public override Stream GetTemporaryOutput()
		{
			this.temporaryStream_ = new MemoryStream();
			return this.temporaryStream_;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00038989 File Offset: 0x00036B89
		public override Stream ConvertTemporaryToFinal()
		{
			if (this.temporaryStream_ == null)
			{
				throw new ZipException("No temporary stream has been created");
			}
			this.finalStream_ = new MemoryStream(this.temporaryStream_.ToArray());
			return this.finalStream_;
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x000389BA File Offset: 0x00036BBA
		public override Stream MakeTemporaryCopy(Stream stream)
		{
			this.temporaryStream_ = new MemoryStream();
			stream.Position = 0L;
			StreamUtils.Copy(stream, this.temporaryStream_, new byte[4096]);
			return this.temporaryStream_;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x000389EC File Offset: 0x00036BEC
		public override Stream OpenForDirectUpdate(Stream stream)
		{
			Stream stream2;
			if (stream == null || !stream.CanWrite)
			{
				stream2 = new MemoryStream();
				if (stream != null)
				{
					stream.Position = 0L;
					StreamUtils.Copy(stream, stream2, new byte[4096]);
					stream.Close();
				}
			}
			else
			{
				stream2 = stream;
			}
			return stream2;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00038A31 File Offset: 0x00036C31
		public override void Dispose()
		{
			if (this.temporaryStream_ != null)
			{
				this.temporaryStream_.Close();
			}
		}

		// Token: 0x04000575 RID: 1397
		private MemoryStream temporaryStream_;

		// Token: 0x04000576 RID: 1398
		private MemoryStream finalStream_;
	}
}
