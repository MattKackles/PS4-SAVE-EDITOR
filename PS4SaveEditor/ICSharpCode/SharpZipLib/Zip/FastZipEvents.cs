using System;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000CF RID: 207
	public class FastZipEvents
	{
		// Token: 0x060008C7 RID: 2247 RVA: 0x00032594 File Offset: 0x00030794
		public bool OnDirectoryFailure(string directory, Exception e)
		{
			bool result = false;
			DirectoryFailureHandler directoryFailure = this.DirectoryFailure;
			if (directoryFailure != null)
			{
				ScanFailureEventArgs scanFailureEventArgs = new ScanFailureEventArgs(directory, e);
				directoryFailure(this, scanFailureEventArgs);
				result = scanFailureEventArgs.ContinueRunning;
			}
			return result;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x000325C8 File Offset: 0x000307C8
		public bool OnFileFailure(string file, Exception e)
		{
			FileFailureHandler fileFailure = this.FileFailure;
			bool flag = fileFailure != null;
			if (flag)
			{
				ScanFailureEventArgs scanFailureEventArgs = new ScanFailureEventArgs(file, e);
				fileFailure(this, scanFailureEventArgs);
				flag = scanFailureEventArgs.ContinueRunning;
			}
			return flag;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00032600 File Offset: 0x00030800
		public bool OnProcessFile(string file)
		{
			bool result = true;
			ProcessFileHandler processFile = this.ProcessFile;
			if (processFile != null)
			{
				ScanEventArgs scanEventArgs = new ScanEventArgs(file);
				processFile(this, scanEventArgs);
				result = scanEventArgs.ContinueRunning;
			}
			return result;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00032630 File Offset: 0x00030830
		public bool OnCompletedFile(string file)
		{
			bool result = true;
			CompletedFileHandler completedFile = this.CompletedFile;
			if (completedFile != null)
			{
				ScanEventArgs scanEventArgs = new ScanEventArgs(file);
				completedFile(this, scanEventArgs);
				result = scanEventArgs.ContinueRunning;
			}
			return result;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00032660 File Offset: 0x00030860
		public bool OnProcessDirectory(string directory, bool hasMatchingFiles)
		{
			bool result = true;
			ProcessDirectoryHandler processDirectory = this.ProcessDirectory;
			if (processDirectory != null)
			{
				DirectoryEventArgs directoryEventArgs = new DirectoryEventArgs(directory, hasMatchingFiles);
				processDirectory(this, directoryEventArgs);
				result = directoryEventArgs.ContinueRunning;
			}
			return result;
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x00032691 File Offset: 0x00030891
		// (set) Token: 0x060008CD RID: 2253 RVA: 0x00032699 File Offset: 0x00030899
		public TimeSpan ProgressInterval
		{
			get
			{
				return this.progressInterval_;
			}
			set
			{
				this.progressInterval_ = value;
			}
		}

		// Token: 0x0400046A RID: 1130
		public ProcessDirectoryHandler ProcessDirectory;

		// Token: 0x0400046B RID: 1131
		public ProcessFileHandler ProcessFile;

		// Token: 0x0400046C RID: 1132
		public ProgressHandler Progress;

		// Token: 0x0400046D RID: 1133
		public CompletedFileHandler CompletedFile;

		// Token: 0x0400046E RID: 1134
		public DirectoryFailureHandler DirectoryFailure;

		// Token: 0x0400046F RID: 1135
		public FileFailureHandler FileFailure;

		// Token: 0x04000470 RID: 1136
		private TimeSpan progressInterval_ = TimeSpan.FromSeconds(3.0);
	}
}
