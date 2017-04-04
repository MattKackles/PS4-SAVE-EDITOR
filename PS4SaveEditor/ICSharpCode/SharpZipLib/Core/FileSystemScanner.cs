using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x020000AE RID: 174
	public class FileSystemScanner
	{
		// Token: 0x06000795 RID: 1941 RVA: 0x0002C4AC File Offset: 0x0002A6AC
		public FileSystemScanner(string filter)
		{
			this.fileFilter_ = new PathFilter(filter);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0002C4C0 File Offset: 0x0002A6C0
		public FileSystemScanner(string fileFilter, string directoryFilter)
		{
			this.fileFilter_ = new PathFilter(fileFilter);
			this.directoryFilter_ = new PathFilter(directoryFilter);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0002C4E0 File Offset: 0x0002A6E0
		public FileSystemScanner(IScanFilter fileFilter)
		{
			this.fileFilter_ = fileFilter;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0002C4EF File Offset: 0x0002A6EF
		public FileSystemScanner(IScanFilter fileFilter, IScanFilter directoryFilter)
		{
			this.fileFilter_ = fileFilter;
			this.directoryFilter_ = directoryFilter;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0002C508 File Offset: 0x0002A708
		private bool OnDirectoryFailure(string directory, Exception e)
		{
			DirectoryFailureHandler directoryFailure = this.DirectoryFailure;
			bool flag = directoryFailure != null;
			if (flag)
			{
				ScanFailureEventArgs scanFailureEventArgs = new ScanFailureEventArgs(directory, e);
				directoryFailure(this, scanFailureEventArgs);
				this.alive_ = scanFailureEventArgs.ContinueRunning;
			}
			return flag;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0002C544 File Offset: 0x0002A744
		private bool OnFileFailure(string file, Exception e)
		{
			FileFailureHandler fileFailure = this.FileFailure;
			bool flag = fileFailure != null;
			if (flag)
			{
				ScanFailureEventArgs scanFailureEventArgs = new ScanFailureEventArgs(file, e);
				this.FileFailure(this, scanFailureEventArgs);
				this.alive_ = scanFailureEventArgs.ContinueRunning;
			}
			return flag;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0002C588 File Offset: 0x0002A788
		private void OnProcessFile(string file)
		{
			ProcessFileHandler processFile = this.ProcessFile;
			if (processFile != null)
			{
				ScanEventArgs scanEventArgs = new ScanEventArgs(file);
				processFile(this, scanEventArgs);
				this.alive_ = scanEventArgs.ContinueRunning;
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0002C5BC File Offset: 0x0002A7BC
		private void OnCompleteFile(string file)
		{
			CompletedFileHandler completedFile = this.CompletedFile;
			if (completedFile != null)
			{
				ScanEventArgs scanEventArgs = new ScanEventArgs(file);
				completedFile(this, scanEventArgs);
				this.alive_ = scanEventArgs.ContinueRunning;
			}
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0002C5F0 File Offset: 0x0002A7F0
		private void OnProcessDirectory(string directory, bool hasMatchingFiles)
		{
			ProcessDirectoryHandler processDirectory = this.ProcessDirectory;
			if (processDirectory != null)
			{
				DirectoryEventArgs directoryEventArgs = new DirectoryEventArgs(directory, hasMatchingFiles);
				processDirectory(this, directoryEventArgs);
				this.alive_ = directoryEventArgs.ContinueRunning;
			}
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0002C623 File Offset: 0x0002A823
		public void Scan(string directory, bool recurse)
		{
			this.alive_ = true;
			this.ScanDir(directory, recurse);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0002C634 File Offset: 0x0002A834
		private void ScanDir(string directory, bool recurse)
		{
			try
			{
				string[] files = Directory.GetFiles(directory);
				bool flag = false;
				for (int i = 0; i < files.Length; i++)
				{
					if (!this.fileFilter_.IsMatch(files[i]))
					{
						files[i] = null;
					}
					else
					{
						flag = true;
					}
				}
				this.OnProcessDirectory(directory, flag);
				if (this.alive_ && flag)
				{
					string[] array = files;
					for (int j = 0; j < array.Length; j++)
					{
						string text = array[j];
						try
						{
							if (text != null)
							{
								this.OnProcessFile(text);
								if (!this.alive_)
								{
									break;
								}
							}
						}
						catch (Exception e)
						{
							if (!this.OnFileFailure(text, e))
							{
								throw;
							}
						}
					}
				}
			}
			catch (Exception e2)
			{
				if (!this.OnDirectoryFailure(directory, e2))
				{
					throw;
				}
			}
			if (this.alive_ && recurse)
			{
				try
				{
					string[] directories = Directory.GetDirectories(directory);
					string[] array2 = directories;
					for (int k = 0; k < array2.Length; k++)
					{
						string text2 = array2[k];
						if (this.directoryFilter_ == null || this.directoryFilter_.IsMatch(text2))
						{
							this.ScanDir(text2, true);
							if (!this.alive_)
							{
								break;
							}
						}
					}
				}
				catch (Exception e3)
				{
					if (!this.OnDirectoryFailure(directory, e3))
					{
						throw;
					}
				}
			}
		}

		// Token: 0x04000379 RID: 889
		public ProcessDirectoryHandler ProcessDirectory;

		// Token: 0x0400037A RID: 890
		public ProcessFileHandler ProcessFile;

		// Token: 0x0400037B RID: 891
		public CompletedFileHandler CompletedFile;

		// Token: 0x0400037C RID: 892
		public DirectoryFailureHandler DirectoryFailure;

		// Token: 0x0400037D RID: 893
		public FileFailureHandler FileFailure;

		// Token: 0x0400037E RID: 894
		private IScanFilter fileFilter_;

		// Token: 0x0400037F RID: 895
		private IScanFilter directoryFilter_;

		// Token: 0x04000380 RID: 896
		private bool alive_;
	}
}
