using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000FD RID: 253
	public class DiskArchiveStorage : BaseArchiveStorage
	{
		// Token: 0x06000A70 RID: 2672 RVA: 0x000386E0 File Offset: 0x000368E0
		public DiskArchiveStorage(ZipFile file, FileUpdateMode updateMode) : base(updateMode)
		{
			if (file.Name == null)
			{
				throw new ZipException("Cant handle non file archives");
			}
			this.fileName_ = file.Name;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00038708 File Offset: 0x00036908
		public DiskArchiveStorage(ZipFile file) : this(file, FileUpdateMode.Safe)
		{
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00038714 File Offset: 0x00036914
		public override Stream GetTemporaryOutput()
		{
			if (this.temporaryName_ != null)
			{
				this.temporaryName_ = DiskArchiveStorage.GetTempFileName(this.temporaryName_, true);
				this.temporaryStream_ = File.Open(this.temporaryName_, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
			}
			else
			{
				this.temporaryName_ = Path.GetTempFileName();
				this.temporaryStream_ = File.Open(this.temporaryName_, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
			}
			return this.temporaryStream_;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00038778 File Offset: 0x00036978
		public override Stream ConvertTemporaryToFinal()
		{
			if (this.temporaryStream_ == null)
			{
				throw new ZipException("No temporary stream has been created");
			}
			Stream result = null;
			string tempFileName = DiskArchiveStorage.GetTempFileName(this.fileName_, false);
			bool flag = false;
			try
			{
				this.temporaryStream_.Close();
				File.Move(this.fileName_, tempFileName);
				File.Move(this.temporaryName_, this.fileName_);
				flag = true;
				File.Delete(tempFileName);
				result = File.Open(this.fileName_, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			catch (Exception)
			{
				result = null;
				if (!flag)
				{
					File.Move(tempFileName, this.fileName_);
					File.Delete(this.temporaryName_);
				}
				throw;
			}
			return result;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0003881C File Offset: 0x00036A1C
		public override Stream MakeTemporaryCopy(Stream stream)
		{
			stream.Close();
			this.temporaryName_ = DiskArchiveStorage.GetTempFileName(this.fileName_, true);
			File.Copy(this.fileName_, this.temporaryName_, true);
			this.temporaryStream_ = new FileStream(this.temporaryName_, FileMode.Open, FileAccess.ReadWrite);
			return this.temporaryStream_;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0003886C File Offset: 0x00036A6C
		public override Stream OpenForDirectUpdate(Stream stream)
		{
			Stream result;
			if (stream == null || !stream.CanWrite)
			{
				if (stream != null)
				{
					stream.Close();
				}
				result = new FileStream(this.fileName_, FileMode.Open, FileAccess.ReadWrite);
			}
			else
			{
				result = stream;
			}
			return result;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x000388A0 File Offset: 0x00036AA0
		public override void Dispose()
		{
			if (this.temporaryStream_ != null)
			{
				this.temporaryStream_.Close();
			}
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x000388B8 File Offset: 0x00036AB8
		private static string GetTempFileName(string original, bool makeTempFile)
		{
			string text = null;
			if (original == null)
			{
				text = Path.GetTempFileName();
			}
			else
			{
				int num = 0;
				int second = DateTime.Now.Second;
				while (text == null)
				{
					num++;
					string text2 = string.Format("{0}.{1}{2}.tmp", original, second, num);
					if (!File.Exists(text2))
					{
						if (makeTempFile)
						{
							try
							{
								using (File.Create(text2))
								{
								}
								text = text2;
								continue;
							}
							catch
							{
								second = DateTime.Now.Second;
								continue;
							}
						}
						text = text2;
					}
				}
			}
			return text;
		}

		// Token: 0x04000572 RID: 1394
		private Stream temporaryStream_;

		// Token: 0x04000573 RID: 1395
		private string fileName_;

		// Token: 0x04000574 RID: 1396
		private string temporaryName_;
	}
}
