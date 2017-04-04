using System;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000D0 RID: 208
	public class FastZip
	{
		// Token: 0x060008CF RID: 2255 RVA: 0x000326BE File Offset: 0x000308BE
		public FastZip()
		{
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x000326D8 File Offset: 0x000308D8
		public FastZip(FastZipEvents events)
		{
			this.events_ = events;
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x000326F9 File Offset: 0x000308F9
		// (set) Token: 0x060008D2 RID: 2258 RVA: 0x00032701 File Offset: 0x00030901
		public bool CreateEmptyDirectories
		{
			get
			{
				return this.createEmptyDirectories_;
			}
			set
			{
				this.createEmptyDirectories_ = value;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0003270A File Offset: 0x0003090A
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x00032712 File Offset: 0x00030912
		public string Password
		{
			get
			{
				return this.password_;
			}
			set
			{
				this.password_ = value;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0003271B File Offset: 0x0003091B
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x00032728 File Offset: 0x00030928
		public INameTransform NameTransform
		{
			get
			{
				return this.entryFactory_.NameTransform;
			}
			set
			{
				this.entryFactory_.NameTransform = value;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x00032736 File Offset: 0x00030936
		// (set) Token: 0x060008D8 RID: 2264 RVA: 0x0003273E File Offset: 0x0003093E
		public IEntryFactory EntryFactory
		{
			get
			{
				return this.entryFactory_;
			}
			set
			{
				if (value == null)
				{
					this.entryFactory_ = new ZipEntryFactory();
					return;
				}
				this.entryFactory_ = value;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x00032756 File Offset: 0x00030956
		// (set) Token: 0x060008DA RID: 2266 RVA: 0x0003275E File Offset: 0x0003095E
		public UseZip64 UseZip64
		{
			get
			{
				return this.useZip64_;
			}
			set
			{
				this.useZip64_ = value;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x00032767 File Offset: 0x00030967
		// (set) Token: 0x060008DC RID: 2268 RVA: 0x0003276F File Offset: 0x0003096F
		public bool RestoreDateTimeOnExtract
		{
			get
			{
				return this.restoreDateTimeOnExtract_;
			}
			set
			{
				this.restoreDateTimeOnExtract_ = value;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x00032778 File Offset: 0x00030978
		// (set) Token: 0x060008DE RID: 2270 RVA: 0x00032780 File Offset: 0x00030980
		public bool RestoreAttributesOnExtract
		{
			get
			{
				return this.restoreAttributesOnExtract_;
			}
			set
			{
				this.restoreAttributesOnExtract_ = value;
			}
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00032789 File Offset: 0x00030989
		public void CreateZip(string zipFileName, string sourceDirectory, bool recurse, string fileFilter, string directoryFilter)
		{
			this.CreateZip(File.Create(zipFileName), sourceDirectory, recurse, fileFilter, directoryFilter);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0003279D File Offset: 0x0003099D
		public void CreateZip(string zipFileName, string sourceDirectory, bool recurse, string fileFilter)
		{
			this.CreateZip(File.Create(zipFileName), sourceDirectory, recurse, fileFilter, null);
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x000327B0 File Offset: 0x000309B0
		public void CreateZip(Stream outputStream, string sourceDirectory, bool recurse, string fileFilter, string directoryFilter)
		{
			this.NameTransform = new ZipNameTransform(sourceDirectory);
			this.sourceDirectory_ = sourceDirectory;
			using (this.outputStream_ = new ZipOutputStream(outputStream))
			{
				if (this.password_ != null)
				{
					this.outputStream_.Password = this.password_;
				}
				this.outputStream_.UseZip64 = this.UseZip64;
				FileSystemScanner fileSystemScanner = new FileSystemScanner(fileFilter, directoryFilter);
				FileSystemScanner expr_58 = fileSystemScanner;
				expr_58.ProcessFile = (ProcessFileHandler)Delegate.Combine(expr_58.ProcessFile, new ProcessFileHandler(this.ProcessFile));
				if (this.CreateEmptyDirectories)
				{
					FileSystemScanner expr_82 = fileSystemScanner;
					expr_82.ProcessDirectory = (ProcessDirectoryHandler)Delegate.Combine(expr_82.ProcessDirectory, new ProcessDirectoryHandler(this.ProcessDirectory));
				}
				if (this.events_ != null)
				{
					if (this.events_.FileFailure != null)
					{
						FileSystemScanner expr_B9 = fileSystemScanner;
						expr_B9.FileFailure = (FileFailureHandler)Delegate.Combine(expr_B9.FileFailure, this.events_.FileFailure);
					}
					if (this.events_.DirectoryFailure != null)
					{
						FileSystemScanner expr_E7 = fileSystemScanner;
						expr_E7.DirectoryFailure = (DirectoryFailureHandler)Delegate.Combine(expr_E7.DirectoryFailure, this.events_.DirectoryFailure);
					}
				}
				fileSystemScanner.Scan(sourceDirectory, recurse);
			}
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x000328E8 File Offset: 0x00030AE8
		public void ExtractZip(string zipFileName, string targetDirectory, string fileFilter)
		{
			this.ExtractZip(zipFileName, targetDirectory, FastZip.Overwrite.Always, null, fileFilter, null, this.restoreDateTimeOnExtract_);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x000328FC File Offset: 0x00030AFC
		public void ExtractZip(string zipFileName, string targetDirectory, FastZip.Overwrite overwrite, FastZip.ConfirmOverwriteDelegate confirmDelegate, string fileFilter, string directoryFilter, bool restoreDateTime)
		{
			Stream inputStream = File.Open(zipFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this.ExtractZip(inputStream, targetDirectory, overwrite, confirmDelegate, fileFilter, directoryFilter, restoreDateTime, true);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00032928 File Offset: 0x00030B28
		public void ExtractZip(Stream inputStream, string targetDirectory, FastZip.Overwrite overwrite, FastZip.ConfirmOverwriteDelegate confirmDelegate, string fileFilter, string directoryFilter, bool restoreDateTime, bool isStreamOwner)
		{
			if (overwrite == FastZip.Overwrite.Prompt && confirmDelegate == null)
			{
				throw new ArgumentNullException("confirmDelegate");
			}
			this.continueRunning_ = true;
			this.overwrite_ = overwrite;
			this.confirmDelegate_ = confirmDelegate;
			this.extractNameTransform_ = new WindowsNameTransform(targetDirectory);
			this.fileFilter_ = new NameFilter(fileFilter);
			this.directoryFilter_ = new NameFilter(directoryFilter);
			this.restoreDateTimeOnExtract_ = restoreDateTime;
			using (this.zipFile_ = new ZipFile(inputStream))
			{
				if (this.password_ != null)
				{
					this.zipFile_.Password = this.password_;
				}
				this.zipFile_.IsStreamOwner = isStreamOwner;
				IEnumerator enumerator = this.zipFile_.GetEnumerator();
				while (this.continueRunning_ && enumerator.MoveNext())
				{
					ZipEntry zipEntry = (ZipEntry)enumerator.Current;
					if (zipEntry.IsFile)
					{
						if (this.directoryFilter_.IsMatch(Path.GetDirectoryName(zipEntry.Name)) && this.fileFilter_.IsMatch(zipEntry.Name))
						{
							this.ExtractEntry(zipEntry);
						}
					}
					else if (zipEntry.IsDirectory && this.directoryFilter_.IsMatch(zipEntry.Name) && this.CreateEmptyDirectories)
					{
						this.ExtractEntry(zipEntry);
					}
				}
			}
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00032A70 File Offset: 0x00030C70
		private void ProcessDirectory(object sender, DirectoryEventArgs e)
		{
			if (!e.HasMatchingFiles && this.CreateEmptyDirectories)
			{
				if (this.events_ != null)
				{
					this.events_.OnProcessDirectory(e.Name, e.HasMatchingFiles);
				}
				if (e.ContinueRunning && e.Name != this.sourceDirectory_)
				{
					ZipEntry entry = this.entryFactory_.MakeDirectoryEntry(e.Name);
					this.outputStream_.PutNextEntry(entry);
				}
			}
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00032AE8 File Offset: 0x00030CE8
		private void ProcessFile(object sender, ScanEventArgs e)
		{
			if (this.events_ != null && this.events_.ProcessFile != null)
			{
				this.events_.ProcessFile(sender, e);
			}
			if (e.ContinueRunning)
			{
				try
				{
					using (FileStream fileStream = File.Open(e.Name, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						ZipEntry entry = this.entryFactory_.MakeFileEntry(e.Name);
						this.outputStream_.PutNextEntry(entry);
						this.AddFileContents(e.Name, fileStream);
					}
				}
				catch (Exception e2)
				{
					if (this.events_ == null)
					{
						this.continueRunning_ = false;
						throw;
					}
					this.continueRunning_ = this.events_.OnFileFailure(e.Name, e2);
				}
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00032BB8 File Offset: 0x00030DB8
		private void AddFileContents(string name, Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this.buffer_ == null)
			{
				this.buffer_ = new byte[4096];
			}
			if (this.events_ != null && this.events_.Progress != null)
			{
				StreamUtils.Copy(stream, this.outputStream_, this.buffer_, this.events_.Progress, this.events_.ProgressInterval, this, name);
			}
			else
			{
				StreamUtils.Copy(stream, this.outputStream_, this.buffer_);
			}
			if (this.events_ != null)
			{
				this.continueRunning_ = this.events_.OnCompletedFile(name);
			}
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00032C58 File Offset: 0x00030E58
		private void ExtractFileEntry(ZipEntry entry, string targetName)
		{
			bool flag = true;
			if (this.overwrite_ != FastZip.Overwrite.Always && File.Exists(targetName))
			{
				flag = (this.overwrite_ == FastZip.Overwrite.Prompt && this.confirmDelegate_ != null && this.confirmDelegate_(targetName));
			}
			if (flag)
			{
				if (this.events_ != null)
				{
					this.continueRunning_ = this.events_.OnProcessFile(entry.Name);
				}
				if (this.continueRunning_)
				{
					try
					{
						using (FileStream fileStream = File.Create(targetName))
						{
							if (this.buffer_ == null)
							{
								this.buffer_ = new byte[4096];
							}
							if (this.events_ != null && this.events_.Progress != null)
							{
								StreamUtils.Copy(this.zipFile_.GetInputStream(entry), fileStream, this.buffer_, this.events_.Progress, this.events_.ProgressInterval, this, entry.Name, entry.Size);
							}
							else
							{
								StreamUtils.Copy(this.zipFile_.GetInputStream(entry), fileStream, this.buffer_);
							}
							if (this.events_ != null)
							{
								this.continueRunning_ = this.events_.OnCompletedFile(entry.Name);
							}
						}
						if (this.restoreDateTimeOnExtract_)
						{
							File.SetLastWriteTime(targetName, entry.DateTime);
						}
						if (this.RestoreAttributesOnExtract && entry.IsDOSEntry && entry.ExternalFileAttributes != -1)
						{
							FileAttributes fileAttributes = (FileAttributes)entry.ExternalFileAttributes;
							fileAttributes &= (FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.Archive | FileAttributes.Normal);
							File.SetAttributes(targetName, fileAttributes);
						}
					}
					catch (Exception e)
					{
						if (this.events_ == null)
						{
							this.continueRunning_ = false;
							throw;
						}
						this.continueRunning_ = this.events_.OnFileFailure(targetName, e);
					}
				}
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00032E08 File Offset: 0x00031008
		private void ExtractEntry(ZipEntry entry)
		{
			bool flag = entry.IsCompressionMethodSupported();
			string text = entry.Name;
			if (flag)
			{
				if (entry.IsFile)
				{
					text = this.extractNameTransform_.TransformFile(text);
				}
				else if (entry.IsDirectory)
				{
					text = this.extractNameTransform_.TransformDirectory(text);
				}
				flag = (text != null && text.Length != 0);
			}
			string path = null;
			if (flag)
			{
				if (entry.IsDirectory)
				{
					path = text;
				}
				else
				{
					path = Path.GetDirectoryName(Path.GetFullPath(text));
				}
			}
			if (flag && !Directory.Exists(path))
			{
				if (entry.IsDirectory)
				{
					if (!this.CreateEmptyDirectories)
					{
						goto IL_D9;
					}
				}
				try
				{
					Directory.CreateDirectory(path);
				}
				catch (Exception e)
				{
					flag = false;
					if (this.events_ == null)
					{
						this.continueRunning_ = false;
						throw;
					}
					if (entry.IsDirectory)
					{
						this.continueRunning_ = this.events_.OnDirectoryFailure(text, e);
					}
					else
					{
						this.continueRunning_ = this.events_.OnFileFailure(text, e);
					}
				}
			}
			IL_D9:
			if (flag && entry.IsFile)
			{
				this.ExtractFileEntry(entry, text);
			}
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00032F14 File Offset: 0x00031114
		private static int MakeExternalAttributes(FileInfo info)
		{
			return (int)info.Attributes;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00032F1C File Offset: 0x0003111C
		private static bool NameIsValid(string name)
		{
			return name != null && name.Length > 0 && name.IndexOfAny(Path.GetInvalidPathChars()) < 0;
		}

		// Token: 0x04000471 RID: 1137
		private bool continueRunning_;

		// Token: 0x04000472 RID: 1138
		private byte[] buffer_;

		// Token: 0x04000473 RID: 1139
		private ZipOutputStream outputStream_;

		// Token: 0x04000474 RID: 1140
		private ZipFile zipFile_;

		// Token: 0x04000475 RID: 1141
		private string sourceDirectory_;

		// Token: 0x04000476 RID: 1142
		private NameFilter fileFilter_;

		// Token: 0x04000477 RID: 1143
		private NameFilter directoryFilter_;

		// Token: 0x04000478 RID: 1144
		private FastZip.Overwrite overwrite_;

		// Token: 0x04000479 RID: 1145
		private FastZip.ConfirmOverwriteDelegate confirmDelegate_;

		// Token: 0x0400047A RID: 1146
		private bool restoreDateTimeOnExtract_;

		// Token: 0x0400047B RID: 1147
		private bool restoreAttributesOnExtract_;

		// Token: 0x0400047C RID: 1148
		private bool createEmptyDirectories_;

		// Token: 0x0400047D RID: 1149
		private FastZipEvents events_;

		// Token: 0x0400047E RID: 1150
		private IEntryFactory entryFactory_ = new ZipEntryFactory();

		// Token: 0x0400047F RID: 1151
		private INameTransform extractNameTransform_;

		// Token: 0x04000480 RID: 1152
		private UseZip64 useZip64_ = UseZip64.Dynamic;

		// Token: 0x04000481 RID: 1153
		private string password_;

		// Token: 0x020000D1 RID: 209
		public enum Overwrite
		{
			// Token: 0x04000483 RID: 1155
			Prompt,
			// Token: 0x04000484 RID: 1156
			Never,
			// Token: 0x04000485 RID: 1157
			Always
		}

		// Token: 0x020000D2 RID: 210
		// (Invoke) Token: 0x060008ED RID: 2285
		public delegate bool ConfirmOverwriteDelegate(string fileName);
	}
}
