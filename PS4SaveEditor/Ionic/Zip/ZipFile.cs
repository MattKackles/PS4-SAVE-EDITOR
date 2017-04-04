using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Ionic.Zlib;
using Microsoft.CSharp;

namespace Ionic.Zip
{
	// Token: 0x02000155 RID: 341
	[ComVisible(true), Guid("ebc25cf6-9120-4283-b972-0e5520d00005"), ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ZipFile : IEnumerable<ZipEntry>, IEnumerable, IDisposable
	{
		// Token: 0x06000DD4 RID: 3540 RVA: 0x00051C53 File Offset: 0x0004FE53
		public ZipEntry AddItem(string fileOrDirectoryName)
		{
			return this.AddItem(fileOrDirectoryName, null);
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00051C5D File Offset: 0x0004FE5D
		public ZipEntry AddItem(string fileOrDirectoryName, string directoryPathInArchive)
		{
			if (File.Exists(fileOrDirectoryName))
			{
				return this.AddFile(fileOrDirectoryName, directoryPathInArchive);
			}
			if (Directory.Exists(fileOrDirectoryName))
			{
				return this.AddDirectory(fileOrDirectoryName, directoryPathInArchive);
			}
			throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", fileOrDirectoryName));
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00051C91 File Offset: 0x0004FE91
		public ZipEntry AddFile(string fileName)
		{
			return this.AddFile(fileName, null);
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00051C9C File Offset: 0x0004FE9C
		public ZipEntry AddFile(string fileName, string directoryPathInArchive)
		{
			string nameInArchive = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
			ZipEntry ze = ZipEntry.CreateFromFile(fileName, nameInArchive);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", fileName);
			}
			return this._InternalAddEntry(ze);
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00051CDC File Offset: 0x0004FEDC
		public void RemoveEntries(ICollection<ZipEntry> entriesToRemove)
		{
			if (entriesToRemove == null)
			{
				throw new ArgumentNullException("entriesToRemove");
			}
			foreach (ZipEntry current in entriesToRemove)
			{
				this.RemoveEntry(current);
			}
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00051D34 File Offset: 0x0004FF34
		public void RemoveEntries(ICollection<string> entriesToRemove)
		{
			if (entriesToRemove == null)
			{
				throw new ArgumentNullException("entriesToRemove");
			}
			foreach (string current in entriesToRemove)
			{
				this.RemoveEntry(current);
			}
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00051D8C File Offset: 0x0004FF8C
		public void AddFiles(IEnumerable<string> fileNames)
		{
			this.AddFiles(fileNames, null);
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00051D96 File Offset: 0x0004FF96
		public void UpdateFiles(IEnumerable<string> fileNames)
		{
			this.UpdateFiles(fileNames, null);
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00051DA0 File Offset: 0x0004FFA0
		public void AddFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
		{
			this.AddFiles(fileNames, false, directoryPathInArchive);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00051DAC File Offset: 0x0004FFAC
		public void AddFiles(IEnumerable<string> fileNames, bool preserveDirHierarchy, string directoryPathInArchive)
		{
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			this._addOperationCanceled = false;
			this.OnAddStarted();
			if (preserveDirHierarchy)
			{
				using (IEnumerator<string> enumerator = fileNames.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						if (this._addOperationCanceled)
						{
							break;
						}
						if (directoryPathInArchive != null)
						{
							string fullPath = Path.GetFullPath(Path.Combine(directoryPathInArchive, Path.GetDirectoryName(current)));
							this.AddFile(current, fullPath);
						}
						else
						{
							this.AddFile(current, null);
						}
					}
					goto IL_AD;
				}
			}
			foreach (string current2 in fileNames)
			{
				if (this._addOperationCanceled)
				{
					break;
				}
				this.AddFile(current2, directoryPathInArchive);
			}
			IL_AD:
			if (!this._addOperationCanceled)
			{
				this.OnAddCompleted();
			}
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00051E90 File Offset: 0x00050090
		public void UpdateFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
		{
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			this.OnAddStarted();
			foreach (string current in fileNames)
			{
				this.UpdateFile(current, directoryPathInArchive);
			}
			this.OnAddCompleted();
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x00051EF4 File Offset: 0x000500F4
		public ZipEntry UpdateFile(string fileName)
		{
			return this.UpdateFile(fileName, null);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00051F00 File Offset: 0x00050100
		public ZipEntry UpdateFile(string fileName, string directoryPathInArchive)
		{
			string fileName2 = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
			if (this[fileName2] != null)
			{
				this.RemoveEntry(fileName2);
			}
			return this.AddFile(fileName, directoryPathInArchive);
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00051F2D File Offset: 0x0005012D
		public ZipEntry UpdateDirectory(string directoryName)
		{
			return this.UpdateDirectory(directoryName, null);
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00051F37 File Offset: 0x00050137
		public ZipEntry UpdateDirectory(string directoryName, string directoryPathInArchive)
		{
			return this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOrUpdate);
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00051F42 File Offset: 0x00050142
		public void UpdateItem(string itemName)
		{
			this.UpdateItem(itemName, null);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00051F4C File Offset: 0x0005014C
		public void UpdateItem(string itemName, string directoryPathInArchive)
		{
			if (File.Exists(itemName))
			{
				this.UpdateFile(itemName, directoryPathInArchive);
				return;
			}
			if (Directory.Exists(itemName))
			{
				this.UpdateDirectory(itemName, directoryPathInArchive);
				return;
			}
			throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", itemName));
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00051F82 File Offset: 0x00050182
		public ZipEntry AddEntry(string entryName, string content)
		{
			return this.AddEntry(entryName, content, Encoding.Default);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00051F94 File Offset: 0x00050194
		public ZipEntry AddEntry(string entryName, string content, Encoding encoding)
		{
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream, encoding);
			streamWriter.Write(content);
			streamWriter.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return this.AddEntry(entryName, memoryStream);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00051FD0 File Offset: 0x000501D0
		public ZipEntry AddEntry(string entryName, Stream stream)
		{
			ZipEntry zipEntry = ZipEntry.CreateForStream(entryName, stream);
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(zipEntry);
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0005201C File Offset: 0x0005021C
		public ZipEntry AddEntry(string entryName, WriteDelegate writer)
		{
			ZipEntry ze = ZipEntry.CreateForWriter(entryName, writer);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(ze);
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00052054 File Offset: 0x00050254
		public ZipEntry AddEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
		{
			ZipEntry zipEntry = ZipEntry.CreateForJitStreamProvider(entryName, opener, closer);
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(zipEntry);
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x000520A0 File Offset: 0x000502A0
		private ZipEntry _InternalAddEntry(ZipEntry ze)
		{
			ze._container = new ZipContainer(this);
			ze.CompressionMethod = this.CompressionMethod;
			ze.CompressionLevel = this.CompressionLevel;
			ze.ExtractExistingFile = this.ExtractExistingFile;
			ze.ZipErrorAction = this.ZipErrorAction;
			ze.SetCompression = this.SetCompression;
			ze.AlternateEncoding = this.AlternateEncoding;
			ze.AlternateEncodingUsage = this.AlternateEncodingUsage;
			ze.Password = this._Password;
			ze.Encryption = this.Encryption;
			ze.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
			ze.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
			this.InternalAddEntry(ze.FileName, ze);
			this.AfterAddEntry(ze);
			return ze;
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00052152 File Offset: 0x00050352
		public ZipEntry UpdateEntry(string entryName, string content)
		{
			return this.UpdateEntry(entryName, content, Encoding.Default);
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00052161 File Offset: 0x00050361
		public ZipEntry UpdateEntry(string entryName, string content, Encoding encoding)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, content, encoding);
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00052173 File Offset: 0x00050373
		public ZipEntry UpdateEntry(string entryName, WriteDelegate writer)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, writer);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00052184 File Offset: 0x00050384
		public ZipEntry UpdateEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, opener, closer);
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00052196 File Offset: 0x00050396
		public ZipEntry UpdateEntry(string entryName, Stream stream)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, stream);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x000521A8 File Offset: 0x000503A8
		private void RemoveEntryForUpdate(string entryName)
		{
			if (string.IsNullOrEmpty(entryName))
			{
				throw new ArgumentNullException("entryName");
			}
			string directoryPathInArchive = null;
			if (entryName.IndexOf('\\') != -1)
			{
				directoryPathInArchive = Path.GetDirectoryName(entryName);
				entryName = Path.GetFileName(entryName);
			}
			string fileName = ZipEntry.NameInArchive(entryName, directoryPathInArchive);
			if (this[fileName] != null)
			{
				this.RemoveEntry(fileName);
			}
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x000521FC File Offset: 0x000503FC
		public ZipEntry AddEntry(string entryName, byte[] byteContent)
		{
			if (byteContent == null)
			{
				throw new ArgumentException("bad argument", "byteContent");
			}
			MemoryStream stream = new MemoryStream(byteContent);
			return this.AddEntry(entryName, stream);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0005222B File Offset: 0x0005042B
		public ZipEntry UpdateEntry(string entryName, byte[] byteContent)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, byteContent);
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0005223C File Offset: 0x0005043C
		public ZipEntry AddDirectory(string directoryName)
		{
			return this.AddDirectory(directoryName, null);
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00052246 File Offset: 0x00050446
		public ZipEntry AddDirectory(string directoryName, string directoryPathInArchive)
		{
			return this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOnly);
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00052254 File Offset: 0x00050454
		public ZipEntry AddDirectoryByName(string directoryNameInArchive)
		{
			ZipEntry zipEntry = ZipEntry.CreateFromNothing(directoryNameInArchive);
			zipEntry._container = new ZipContainer(this);
			zipEntry.MarkAsDirectory();
			zipEntry.AlternateEncoding = this.AlternateEncoding;
			zipEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			zipEntry.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
			zipEntry.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
			zipEntry._Source = ZipEntrySource.Stream;
			this.InternalAddEntry(zipEntry.FileName, zipEntry);
			this.AfterAddEntry(zipEntry);
			return zipEntry;
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x000522DB File Offset: 0x000504DB
		private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action)
		{
			if (rootDirectoryPathInArchive == null)
			{
				rootDirectoryPathInArchive = "";
			}
			return this.AddOrUpdateDirectoryImpl(directoryName, rootDirectoryPathInArchive, action, true, 0);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x000522F2 File Offset: 0x000504F2
		internal void InternalAddEntry(string name, ZipEntry entry)
		{
			this._entries.Add(name, entry);
			this._zipEntriesAsList = null;
			this._contentsChanged = true;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00052310 File Offset: 0x00050510
		private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action, bool recurse, int level)
		{
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("{0} {1}...", (action == AddOrUpdateAction.AddOnly) ? "adding" : "Adding or updating", directoryName);
			}
			if (level == 0)
			{
				this._addOperationCanceled = false;
				this.OnAddStarted();
			}
			if (this._addOperationCanceled)
			{
				return null;
			}
			string text = rootDirectoryPathInArchive;
			ZipEntry zipEntry = null;
			if (level > 0)
			{
				int num = directoryName.Length;
				for (int i = level; i > 0; i--)
				{
					num = directoryName.LastIndexOfAny("/\\".ToCharArray(), num - 1, num - 1);
				}
				text = directoryName.Substring(num + 1);
				text = Path.Combine(rootDirectoryPathInArchive, text);
			}
			if (level > 0 || rootDirectoryPathInArchive != "")
			{
				zipEntry = ZipEntry.CreateFromFile(directoryName, text);
				zipEntry._container = new ZipContainer(this);
				zipEntry.AlternateEncoding = this.AlternateEncoding;
				zipEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
				zipEntry.MarkAsDirectory();
				zipEntry.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
				zipEntry.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
				if (!this._entries.ContainsKey(zipEntry.FileName))
				{
					this.InternalAddEntry(zipEntry.FileName, zipEntry);
					this.AfterAddEntry(zipEntry);
				}
				text = zipEntry.FileName;
			}
			if (!this._addOperationCanceled)
			{
				string[] files = Directory.GetFiles(directoryName);
				if (recurse)
				{
					string[] array = files;
					for (int j = 0; j < array.Length; j++)
					{
						string fileName = array[j];
						if (this._addOperationCanceled)
						{
							break;
						}
						if (action == AddOrUpdateAction.AddOnly)
						{
							this.AddFile(fileName, text);
						}
						else
						{
							this.UpdateFile(fileName, text);
						}
					}
					if (!this._addOperationCanceled)
					{
						string[] directories = Directory.GetDirectories(directoryName);
						string[] array2 = directories;
						for (int k = 0; k < array2.Length; k++)
						{
							string text2 = array2[k];
							FileAttributes attributes = File.GetAttributes(text2);
							if (this.AddDirectoryWillTraverseReparsePoints || (attributes & FileAttributes.ReparsePoint) == (FileAttributes)0)
							{
								this.AddOrUpdateDirectoryImpl(text2, rootDirectoryPathInArchive, action, recurse, level + 1);
							}
						}
					}
				}
			}
			if (level == 0)
			{
				this.OnAddCompleted();
			}
			return zipEntry;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x000524EE File Offset: 0x000506EE
		public static bool CheckZip(string zipFileName)
		{
			return ZipFile.CheckZip(zipFileName, false, null);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x000524F8 File Offset: 0x000506F8
		public static bool CheckZip(string zipFileName, bool fixIfNecessary, TextWriter writer)
		{
			ZipFile zipFile = null;
			ZipFile zipFile2 = null;
			bool flag = true;
			try
			{
				zipFile = new ZipFile();
				zipFile.FullScan = true;
				zipFile.Initialize(zipFileName);
				zipFile2 = ZipFile.Read(zipFileName);
				foreach (ZipEntry current in zipFile)
				{
					foreach (ZipEntry current2 in zipFile2)
					{
						if (current.FileName == current2.FileName)
						{
							if (current._RelativeOffsetOfLocalHeader != current2._RelativeOffsetOfLocalHeader)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in RelativeOffsetOfLocalHeader  (0x{1:X16} != 0x{2:X16})", current.FileName, current._RelativeOffsetOfLocalHeader, current2._RelativeOffsetOfLocalHeader);
								}
							}
							if (current._CompressedSize != current2._CompressedSize)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in CompressedSize  (0x{1:X16} != 0x{2:X16})", current.FileName, current._CompressedSize, current2._CompressedSize);
								}
							}
							if (current._UncompressedSize != current2._UncompressedSize)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in UncompressedSize  (0x{1:X16} != 0x{2:X16})", current.FileName, current._UncompressedSize, current2._UncompressedSize);
								}
							}
							if (current.CompressionMethod != current2.CompressionMethod)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in CompressionMethod  (0x{1:X4} != 0x{2:X4})", current.FileName, current.CompressionMethod, current2.CompressionMethod);
								}
							}
							if (current.Crc == current2.Crc)
							{
								break;
							}
							flag = false;
							if (writer != null)
							{
								writer.WriteLine("{0}: mismatch in Crc32  (0x{1:X4} != 0x{2:X4})", current.FileName, current.Crc, current2.Crc);
								break;
							}
							break;
						}
					}
				}
				zipFile2.Dispose();
				zipFile2 = null;
				if (!flag && fixIfNecessary)
				{
					string text = Path.GetFileNameWithoutExtension(zipFileName);
					text = string.Format("{0}_fixed.zip", text);
					zipFile.Save(text);
				}
			}
			finally
			{
				if (zipFile != null)
				{
					zipFile.Dispose();
				}
				if (zipFile2 != null)
				{
					zipFile2.Dispose();
				}
			}
			return flag;
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00052758 File Offset: 0x00050958
		public static void FixZipDirectory(string zipFileName)
		{
			using (ZipFile zipFile = new ZipFile())
			{
				zipFile.FullScan = true;
				zipFile.Initialize(zipFileName);
				zipFile.Save(zipFileName);
			}
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0005279C File Offset: 0x0005099C
		public static bool CheckZipPassword(string zipFileName, string password)
		{
			bool result = false;
			try
			{
				using (ZipFile zipFile = ZipFile.Read(zipFileName))
				{
					foreach (ZipEntry current in zipFile)
					{
						if (!current.IsDirectory && current.UsesEncryption)
						{
							current.ExtractWithPassword(Stream.Null, password);
						}
					}
				}
				result = true;
			}
			catch (BadPasswordException)
			{
			}
			return result;
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000DFD RID: 3581 RVA: 0x00052830 File Offset: 0x00050A30
		public string Info
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Format("          ZipFile: {0}\n", this.Name));
				if (!string.IsNullOrEmpty(this._Comment))
				{
					stringBuilder.Append(string.Format("          Comment: {0}\n", this._Comment));
				}
				if (this._versionMadeBy != 0)
				{
					stringBuilder.Append(string.Format("  version made by: 0x{0:X4}\n", this._versionMadeBy));
				}
				if (this._versionNeededToExtract != 0)
				{
					stringBuilder.Append(string.Format("needed to extract: 0x{0:X4}\n", this._versionNeededToExtract));
				}
				stringBuilder.Append(string.Format("       uses ZIP64: {0}\n", this.InputUsesZip64));
				stringBuilder.Append(string.Format("     disk with CD: {0}\n", this._diskNumberWithCd));
				if (this._OffsetOfCentralDirectory == 4294967295u)
				{
					stringBuilder.Append(string.Format("      CD64 offset: 0x{0:X16}\n", this._OffsetOfCentralDirectory64));
				}
				else
				{
					stringBuilder.Append(string.Format("        CD offset: 0x{0:X8}\n", this._OffsetOfCentralDirectory));
				}
				stringBuilder.Append("\n");
				foreach (ZipEntry current in this._entries.Values)
				{
					stringBuilder.Append(current.Info);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x000529A4 File Offset: 0x00050BA4
		// (set) Token: 0x06000DFF RID: 3583 RVA: 0x000529AC File Offset: 0x00050BAC
		public bool FullScan
		{
			get;
			set;
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000E00 RID: 3584 RVA: 0x000529B5 File Offset: 0x00050BB5
		// (set) Token: 0x06000E01 RID: 3585 RVA: 0x000529BD File Offset: 0x00050BBD
		public bool SortEntriesBeforeSaving
		{
			get;
			set;
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000E02 RID: 3586 RVA: 0x000529C6 File Offset: 0x00050BC6
		// (set) Token: 0x06000E03 RID: 3587 RVA: 0x000529CE File Offset: 0x00050BCE
		public bool AddDirectoryWillTraverseReparsePoints
		{
			get;
			set;
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x000529D7 File Offset: 0x00050BD7
		// (set) Token: 0x06000E05 RID: 3589 RVA: 0x000529DF File Offset: 0x00050BDF
		public int BufferSize
		{
			get
			{
				return this._BufferSize;
			}
			set
			{
				this._BufferSize = value;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000E06 RID: 3590 RVA: 0x000529E8 File Offset: 0x00050BE8
		// (set) Token: 0x06000E07 RID: 3591 RVA: 0x000529F0 File Offset: 0x00050BF0
		public int CodecBufferSize
		{
			get;
			set;
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000E08 RID: 3592 RVA: 0x000529F9 File Offset: 0x00050BF9
		// (set) Token: 0x06000E09 RID: 3593 RVA: 0x00052A01 File Offset: 0x00050C01
		public bool FlattenFoldersOnExtract
		{
			get;
			set;
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x00052A0A File Offset: 0x00050C0A
		// (set) Token: 0x06000E0B RID: 3595 RVA: 0x00052A12 File Offset: 0x00050C12
		public CompressionStrategy Strategy
		{
			get
			{
				return this._Strategy;
			}
			set
			{
				this._Strategy = value;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x00052A1B File Offset: 0x00050C1B
		// (set) Token: 0x06000E0D RID: 3597 RVA: 0x00052A23 File Offset: 0x00050C23
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x00052A2C File Offset: 0x00050C2C
		// (set) Token: 0x06000E0F RID: 3599 RVA: 0x00052A34 File Offset: 0x00050C34
		public CompressionLevel CompressionLevel
		{
			get;
			set;
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x00052A3D File Offset: 0x00050C3D
		// (set) Token: 0x06000E11 RID: 3601 RVA: 0x00052A45 File Offset: 0x00050C45
		public CompressionMethod CompressionMethod
		{
			get
			{
				return this._compressionMethod;
			}
			set
			{
				this._compressionMethod = value;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x00052A4E File Offset: 0x00050C4E
		// (set) Token: 0x06000E13 RID: 3603 RVA: 0x00052A56 File Offset: 0x00050C56
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				this._Comment = value;
				this._contentsChanged = true;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000E14 RID: 3604 RVA: 0x00052A66 File Offset: 0x00050C66
		// (set) Token: 0x06000E15 RID: 3605 RVA: 0x00052A6E File Offset: 0x00050C6E
		public bool EmitTimesInWindowsFormatWhenSaving
		{
			get
			{
				return this._emitNtfsTimes;
			}
			set
			{
				this._emitNtfsTimes = value;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000E16 RID: 3606 RVA: 0x00052A77 File Offset: 0x00050C77
		// (set) Token: 0x06000E17 RID: 3607 RVA: 0x00052A7F File Offset: 0x00050C7F
		public bool EmitTimesInUnixFormatWhenSaving
		{
			get
			{
				return this._emitUnixTimes;
			}
			set
			{
				this._emitUnixTimes = value;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x00052A88 File Offset: 0x00050C88
		internal bool Verbose
		{
			get
			{
				return this._StatusMessageTextWriter != null;
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00052A96 File Offset: 0x00050C96
		public bool ContainsEntry(string name)
		{
			return this._entries.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x00052AA9 File Offset: 0x00050CA9
		// (set) Token: 0x06000E1B RID: 3611 RVA: 0x00052AB1 File Offset: 0x00050CB1
		public bool CaseSensitiveRetrieval
		{
			get
			{
				return this._CaseSensitiveRetrieval;
			}
			set
			{
				if (value != this._CaseSensitiveRetrieval)
				{
					this._CaseSensitiveRetrieval = value;
					this._initEntriesDictionary();
				}
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x00052AC9 File Offset: 0x00050CC9
		// (set) Token: 0x06000E1D RID: 3613 RVA: 0x00052AE8 File Offset: 0x00050CE8
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete.  It will be removed in a future version of the library. Your applications should  use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				return this._alternateEncoding == Encoding.GetEncoding("UTF-8") && this._alternateEncodingUsage == ZipOption.AsNecessary;
			}
			set
			{
				if (value)
				{
					this._alternateEncoding = Encoding.GetEncoding("UTF-8");
					this._alternateEncodingUsage = ZipOption.AsNecessary;
					return;
				}
				this._alternateEncoding = ZipFile.DefaultEncoding;
				this._alternateEncodingUsage = ZipOption.Default;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x00052B17 File Offset: 0x00050D17
		// (set) Token: 0x06000E1F RID: 3615 RVA: 0x00052B1F File Offset: 0x00050D1F
		public Zip64Option UseZip64WhenSaving
		{
			get
			{
				return this._zip64;
			}
			set
			{
				this._zip64 = value;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x00052B28 File Offset: 0x00050D28
		public bool? RequiresZip64
		{
			get
			{
				if (this._entries.Count > 65534)
				{
					return new bool?(true);
				}
				if (!this._hasBeenSaved || this._contentsChanged)
				{
					return null;
				}
				foreach (ZipEntry current in this._entries.Values)
				{
					if (current.RequiresZip64.Value)
					{
						return new bool?(true);
					}
				}
				return new bool?(false);
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000E21 RID: 3617 RVA: 0x00052BD0 File Offset: 0x00050DD0
		public bool? OutputUsedZip64
		{
			get
			{
				return this._OutputUsesZip64;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x00052BD8 File Offset: 0x00050DD8
		public bool? InputUsesZip64
		{
			get
			{
				if (this._entries.Count > 65534)
				{
					return new bool?(true);
				}
				foreach (ZipEntry current in this)
				{
					if (current.Source != ZipEntrySource.ZipFile)
					{
						bool? result = null;
						return result;
					}
					if (current._InputUsesZip64)
					{
						bool? result = new bool?(true);
						return result;
					}
				}
				return new bool?(false);
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000E23 RID: 3619 RVA: 0x00052C60 File Offset: 0x00050E60
		// (set) Token: 0x06000E24 RID: 3620 RVA: 0x00052C73 File Offset: 0x00050E73
		[Obsolete("use AlternateEncoding instead.")]
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				if (this._alternateEncodingUsage == ZipOption.AsNecessary)
				{
					return this._alternateEncoding;
				}
				return null;
			}
			set
			{
				this._alternateEncoding = value;
				this._alternateEncodingUsage = ZipOption.AsNecessary;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x00052C83 File Offset: 0x00050E83
		// (set) Token: 0x06000E26 RID: 3622 RVA: 0x00052C8B File Offset: 0x00050E8B
		public Encoding AlternateEncoding
		{
			get
			{
				return this._alternateEncoding;
			}
			set
			{
				this._alternateEncoding = value;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000E27 RID: 3623 RVA: 0x00052C94 File Offset: 0x00050E94
		// (set) Token: 0x06000E28 RID: 3624 RVA: 0x00052C9C File Offset: 0x00050E9C
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				return this._alternateEncodingUsage;
			}
			set
			{
				this._alternateEncodingUsage = value;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000E29 RID: 3625 RVA: 0x00052CA5 File Offset: 0x00050EA5
		public static Encoding DefaultEncoding
		{
			get
			{
				return ZipFile._defaultEncoding;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x00052CAC File Offset: 0x00050EAC
		// (set) Token: 0x06000E2B RID: 3627 RVA: 0x00052CB4 File Offset: 0x00050EB4
		public TextWriter StatusMessageTextWriter
		{
			get
			{
				return this._StatusMessageTextWriter;
			}
			set
			{
				this._StatusMessageTextWriter = value;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x00052CBD File Offset: 0x00050EBD
		// (set) Token: 0x06000E2D RID: 3629 RVA: 0x00052CC5 File Offset: 0x00050EC5
		public string TempFileFolder
		{
			get
			{
				return this._TempFileFolder;
			}
			set
			{
				this._TempFileFolder = value;
				if (value == null)
				{
					return;
				}
				if (!Directory.Exists(value))
				{
					throw new FileNotFoundException(string.Format("That directory ({0}) does not exist.", value));
				}
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000E2F RID: 3631 RVA: 0x00052D13 File Offset: 0x00050F13
		// (set) Token: 0x06000E2E RID: 3630 RVA: 0x00052CEB File Offset: 0x00050EEB
		public string Password
		{
			private get
			{
				return this._Password;
			}
			set
			{
				this._Password = value;
				if (this._Password == null)
				{
					this.Encryption = EncryptionAlgorithm.None;
					return;
				}
				if (this.Encryption == EncryptionAlgorithm.None)
				{
					this.Encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x00052D1B File Offset: 0x00050F1B
		// (set) Token: 0x06000E31 RID: 3633 RVA: 0x00052D23 File Offset: 0x00050F23
		public ExtractExistingFileAction ExtractExistingFile
		{
			get;
			set;
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x00052D2C File Offset: 0x00050F2C
		// (set) Token: 0x06000E33 RID: 3635 RVA: 0x00052D43 File Offset: 0x00050F43
		public ZipErrorAction ZipErrorAction
		{
			get
			{
				if (this.ZipError != null)
				{
					this._zipErrorAction = ZipErrorAction.InvokeErrorEvent;
				}
				return this._zipErrorAction;
			}
			set
			{
				this._zipErrorAction = value;
				if (this._zipErrorAction != ZipErrorAction.InvokeErrorEvent && this.ZipError != null)
				{
					this.ZipError = null;
				}
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x00052D64 File Offset: 0x00050F64
		// (set) Token: 0x06000E35 RID: 3637 RVA: 0x00052D6C File Offset: 0x00050F6C
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return this._Encryption;
			}
			set
			{
				if (value == EncryptionAlgorithm.Unsupported)
				{
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				this._Encryption = value;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x00052D84 File Offset: 0x00050F84
		// (set) Token: 0x06000E37 RID: 3639 RVA: 0x00052D8C File Offset: 0x00050F8C
		public SetCompressionCallback SetCompression
		{
			get;
			set;
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x00052D95 File Offset: 0x00050F95
		// (set) Token: 0x06000E39 RID: 3641 RVA: 0x00052D9D File Offset: 0x00050F9D
		public int MaxOutputSegmentSize
		{
			get
			{
				return this._maxOutputSegmentSize;
			}
			set
			{
				if (value < 65536 && value != 0)
				{
					throw new ZipException("The minimum acceptable segment size is 65536.");
				}
				this._maxOutputSegmentSize = value;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00052DBC File Offset: 0x00050FBC
		public int NumberOfSegmentsForMostRecentSave
		{
			get
			{
				return (int)(this._numberOfSegmentsForMostRecentSave + 1u);
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x00052DED File Offset: 0x00050FED
		// (set) Token: 0x06000E3B RID: 3643 RVA: 0x00052DC6 File Offset: 0x00050FC6
		public long ParallelDeflateThreshold
		{
			get
			{
				return this._ParallelDeflateThreshold;
			}
			set
			{
				if (value != 0L && value != -1L && value < 65536L)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateThreshold should be -1, 0, or > 65536");
				}
				this._ParallelDeflateThreshold = value;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000E3D RID: 3645 RVA: 0x00052DF5 File Offset: 0x00050FF5
		// (set) Token: 0x06000E3E RID: 3646 RVA: 0x00052DFD File Offset: 0x00050FFD
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateMaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00052E1A File Offset: 0x0005101A
		public override string ToString()
		{
			return string.Format("ZipFile::{0}", this.Name);
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x00052E2C File Offset: 0x0005102C
		public static Version LibraryVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version;
			}
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00052E3D File Offset: 0x0005103D
		internal void NotifyEntryChanged()
		{
			this._contentsChanged = true;
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00052E46 File Offset: 0x00051046
		internal Stream StreamForDiskNumber(uint diskNumber)
		{
			if (diskNumber + 1u == this._diskNumberWithCd || (diskNumber == 0u && this._diskNumberWithCd == 0u))
			{
				return this.ReadStream;
			}
			return ZipSegmentedStream.ForReading(this._readName ?? this._name, diskNumber, this._diskNumberWithCd);
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00052E84 File Offset: 0x00051084
		internal void Reset(bool whileSaving)
		{
			if (this._JustSaved)
			{
				using (ZipFile zipFile = new ZipFile())
				{
					zipFile._readName = (zipFile._name = (whileSaving ? (this._readName ?? this._name) : this._name));
					zipFile.AlternateEncoding = this.AlternateEncoding;
					zipFile.AlternateEncodingUsage = this.AlternateEncodingUsage;
					ZipFile.ReadIntoInstance(zipFile);
					foreach (ZipEntry current in zipFile)
					{
						foreach (ZipEntry current2 in this)
						{
							if (current.FileName == current2.FileName)
							{
								current2.CopyMetaData(current);
								break;
							}
						}
					}
				}
				this._JustSaved = false;
			}
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00052F94 File Offset: 0x00051194
		public ZipFile(string fileName)
		{
			try
			{
				this._InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("Could not read {0} as a zip file", fileName), innerException);
			}
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00053028 File Offset: 0x00051228
		public ZipFile(string fileName, Encoding encoding)
		{
			try
			{
				this.AlternateEncoding = encoding;
				this.AlternateEncodingUsage = ZipOption.Always;
				this._InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x000530CC File Offset: 0x000512CC
		public ZipFile()
		{
			this._InitInstance(null, null);
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0005313C File Offset: 0x0005133C
		public ZipFile(Encoding encoding)
		{
			this.AlternateEncoding = encoding;
			this.AlternateEncodingUsage = ZipOption.Always;
			this._InitInstance(null, null);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x000531BC File Offset: 0x000513BC
		public ZipFile(string fileName, TextWriter statusMessageWriter)
		{
			try
			{
				this._InitInstance(fileName, statusMessageWriter);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00053250 File Offset: 0x00051450
		public ZipFile(string fileName, TextWriter statusMessageWriter, Encoding encoding)
		{
			try
			{
				this.AlternateEncoding = encoding;
				this.AlternateEncodingUsage = ZipOption.Always;
				this._InitInstance(fileName, statusMessageWriter);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x000532F4 File Offset: 0x000514F4
		public void Initialize(string fileName)
		{
			try
			{
				this._InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x00053330 File Offset: 0x00051530
		private void _initEntriesDictionary()
		{
			StringComparer comparer = this.CaseSensitiveRetrieval ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
			this._entries = ((this._entries == null) ? new Dictionary<string, ZipEntry>(comparer) : new Dictionary<string, ZipEntry>(this._entries, comparer));
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00053374 File Offset: 0x00051574
		private void _InitInstance(string zipFileName, TextWriter statusMessageWriter)
		{
			this._name = zipFileName;
			this._StatusMessageTextWriter = statusMessageWriter;
			this._contentsChanged = true;
			this.AddDirectoryWillTraverseReparsePoints = true;
			this.CompressionLevel = CompressionLevel.Default;
			this.ParallelDeflateThreshold = 524288L;
			this._initEntriesDictionary();
			if (File.Exists(this._name))
			{
				if (this.FullScan)
				{
					ZipFile.ReadIntoInstance_Orig(this);
				}
				else
				{
					ZipFile.ReadIntoInstance(this);
				}
				this._fileAlreadyExists = true;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x000533E0 File Offset: 0x000515E0
		private List<ZipEntry> ZipEntriesAsList
		{
			get
			{
				if (this._zipEntriesAsList == null)
				{
					this._zipEntriesAsList = new List<ZipEntry>(this._entries.Values);
				}
				return this._zipEntriesAsList;
			}
		}

		// Token: 0x170003B4 RID: 948
		public ZipEntry this[int ix]
		{
			get
			{
				return this.ZipEntriesAsList[ix];
			}
		}

		// Token: 0x170003B5 RID: 949
		public ZipEntry this[string fileName]
		{
			get
			{
				string text = SharedUtilities.NormalizePathForUseInZipFile(fileName);
				if (this._entries.ContainsKey(text))
				{
					return this._entries[text];
				}
				text = text.Replace("/", "\\");
				if (this._entries.ContainsKey(text))
				{
					return this._entries[text];
				}
				return null;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x00053470 File Offset: 0x00051670
		public ICollection<string> EntryFileNames
		{
			get
			{
				return this._entries.Keys;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x0005347D File Offset: 0x0005167D
		public ICollection<ZipEntry> Entries
		{
			get
			{
				return this._entries.Values;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x000534AC File Offset: 0x000516AC
		public ICollection<ZipEntry> EntriesSorted
		{
			get
			{
				List<ZipEntry> list = new List<ZipEntry>();
				foreach (ZipEntry current in this.Entries)
				{
					list.Add(current);
				}
				StringComparison sc = this.CaseSensitiveRetrieval ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
				list.Sort((ZipEntry x, ZipEntry y) => string.Compare(x.FileName, y.FileName, sc));
				return list.AsReadOnly();
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x00053530 File Offset: 0x00051730
		public int Count
		{
			get
			{
				return this._entries.Count;
			}
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0005353D File Offset: 0x0005173D
		public void RemoveEntry(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this._entries.Remove(SharedUtilities.NormalizePathForUseInZipFile(entry.FileName));
			this._zipEntriesAsList = null;
			this._contentsChanged = true;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00053574 File Offset: 0x00051774
		public void RemoveEntry(string fileName)
		{
			string fileName2 = ZipEntry.NameInArchive(fileName, null);
			ZipEntry zipEntry = this[fileName2];
			if (zipEntry == null)
			{
				throw new ArgumentException("The entry you specified was not found in the zip archive.");
			}
			this.RemoveEntry(zipEntry);
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x000535A6 File Offset: 0x000517A6
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x000535B8 File Offset: 0x000517B8
		protected virtual void Dispose(bool disposeManagedResources)
		{
			if (!this._disposed)
			{
				if (disposeManagedResources)
				{
					if (this._ReadStreamIsOurs && this._readstream != null)
					{
						this._readstream.Dispose();
						this._readstream = null;
					}
					if (this._temporaryFileName != null && this._name != null && this._writestream != null)
					{
						this._writestream.Dispose();
						this._writestream = null;
					}
					if (this.ParallelDeflater != null)
					{
						this.ParallelDeflater.Dispose();
						this.ParallelDeflater = null;
					}
				}
				this._disposed = true;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000E58 RID: 3672 RVA: 0x00053640 File Offset: 0x00051840
		internal Stream ReadStream
		{
			get
			{
				if (this._readstream == null && (this._readName != null || this._name != null))
				{
					this._readstream = File.Open(this._readName ?? this._name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					this._ReadStreamIsOurs = true;
				}
				return this._readstream;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x00053690 File Offset: 0x00051890
		// (set) Token: 0x06000E5A RID: 3674 RVA: 0x0005370D File Offset: 0x0005190D
		private Stream WriteStream
		{
			get
			{
				if (this._writestream != null)
				{
					return this._writestream;
				}
				if (this._name == null)
				{
					return this._writestream;
				}
				if (this._maxOutputSegmentSize != 0)
				{
					this._writestream = ZipSegmentedStream.ForWriting(this._name, this._maxOutputSegmentSize);
					return this._writestream;
				}
				SharedUtilities.CreateAndOpenUniqueTempFile(this.TempFileFolder ?? Path.GetDirectoryName(this._name), out this._writestream, out this._temporaryFileName);
				return this._writestream;
			}
			set
			{
				if (value != null)
				{
					throw new ZipException("Cannot set the stream to a non-null value.");
				}
				this._writestream = null;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x00053724 File Offset: 0x00051924
		private string ArchiveNameForEvent
		{
			get
			{
				if (this._name == null)
				{
					return "(stream)";
				}
				return this._name;
			}
		}

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06000E5C RID: 3676 RVA: 0x0005373C File Offset: 0x0005193C
		// (remove) Token: 0x06000E5D RID: 3677 RVA: 0x00053774 File Offset: 0x00051974
		public event EventHandler<SaveProgressEventArgs> SaveProgress;

		// Token: 0x06000E5E RID: 3678 RVA: 0x000537AC File Offset: 0x000519AC
		internal bool OnSaveBlock(ZipEntry entry, long bytesXferred, long totalBytesToXfer)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesXferred, totalBytesToXfer);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
			return this._saveOperationCanceled;
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x000537F0 File Offset: 0x000519F0
		private void OnSaveEntry(int current, ZipEntry entry, bool before)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = new SaveProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, entry);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00053838 File Offset: 0x00051A38
		private void OnSaveEvent(ZipProgressEventType eventFlavor)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = new SaveProgressEventArgs(this.ArchiveNameForEvent, eventFlavor);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00053874 File Offset: 0x00051A74
		private void OnSaveStarted()
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.Started(this.ArchiveNameForEvent);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x000538B0 File Offset: 0x00051AB0
		private void OnSaveCompleted()
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs e = SaveProgressEventArgs.Completed(this.ArchiveNameForEvent);
				saveProgress(this, e);
			}
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06000E63 RID: 3683 RVA: 0x000538DC File Offset: 0x00051ADC
		// (remove) Token: 0x06000E64 RID: 3684 RVA: 0x00053914 File Offset: 0x00051B14
		public event EventHandler<ReadProgressEventArgs> ReadProgress;

		// Token: 0x06000E65 RID: 3685 RVA: 0x0005394C File Offset: 0x00051B4C
		private void OnReadStarted()
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.Started(this.ArchiveNameForEvent);
				readProgress(this, e);
			}
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00053978 File Offset: 0x00051B78
		private void OnReadCompleted()
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.Completed(this.ArchiveNameForEvent);
				readProgress(this, e);
			}
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x000539A4 File Offset: 0x00051BA4
		internal void OnReadBytes(ZipEntry entry)
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, this.ReadStream.Position, this.LengthOfReadStream);
				readProgress(this, e);
			}
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x000539E4 File Offset: 0x00051BE4
		internal void OnReadEntry(bool before, ZipEntry entry)
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = before ? ReadProgressEventArgs.Before(this.ArchiveNameForEvent, this._entries.Count) : ReadProgressEventArgs.After(this.ArchiveNameForEvent, entry, this._entries.Count);
				readProgress(this, e);
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x00053A36 File Offset: 0x00051C36
		private long LengthOfReadStream
		{
			get
			{
				if (this._lengthOfReadStream == -99L)
				{
					this._lengthOfReadStream = (this._ReadStreamIsOurs ? SharedUtilities.GetFileLength(this._name) : -1L);
				}
				return this._lengthOfReadStream;
			}
		}

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06000E6A RID: 3690 RVA: 0x00053A68 File Offset: 0x00051C68
		// (remove) Token: 0x06000E6B RID: 3691 RVA: 0x00053AA0 File Offset: 0x00051CA0
		public event EventHandler<ExtractProgressEventArgs> ExtractProgress;

		// Token: 0x06000E6C RID: 3692 RVA: 0x00053AD8 File Offset: 0x00051CD8
		private void OnExtractEntry(int current, bool before, ZipEntry currentEntry, string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = new ExtractProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, currentEntry, path);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00053B24 File Offset: 0x00051D24
		internal bool OnExtractBlock(ZipEntry entry, long bytesWritten, long totalBytesToWrite)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesWritten, totalBytesToWrite);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00053B68 File Offset: 0x00051D68
		internal bool OnSingleEntryExtract(ZipEntry entry, string path, bool before)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = before ? ExtractProgressEventArgs.BeforeExtractEntry(this.ArchiveNameForEvent, entry, path) : ExtractProgressEventArgs.AfterExtractEntry(this.ArchiveNameForEvent, entry, path);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00053BBC File Offset: 0x00051DBC
		internal bool OnExtractExisting(ZipEntry entry, string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ExtractExisting(this.ArchiveNameForEvent, entry, path);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00053C00 File Offset: 0x00051E00
		private void OnExtractAllCompleted(string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs e = ExtractProgressEventArgs.ExtractAllCompleted(this.ArchiveNameForEvent, path);
				extractProgress(this, e);
			}
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00053C2C File Offset: 0x00051E2C
		private void OnExtractAllStarted(string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs e = ExtractProgressEventArgs.ExtractAllStarted(this.ArchiveNameForEvent, path);
				extractProgress(this, e);
			}
		}

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06000E72 RID: 3698 RVA: 0x00053C58 File Offset: 0x00051E58
		// (remove) Token: 0x06000E73 RID: 3699 RVA: 0x00053C90 File Offset: 0x00051E90
		public event EventHandler<AddProgressEventArgs> AddProgress;

		// Token: 0x06000E74 RID: 3700 RVA: 0x00053CC8 File Offset: 0x00051EC8
		private void OnAddStarted()
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.Started(this.ArchiveNameForEvent);
				addProgress(this, addProgressEventArgs);
				if (addProgressEventArgs.Cancel)
				{
					this._addOperationCanceled = true;
				}
			}
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x00053D04 File Offset: 0x00051F04
		private void OnAddCompleted()
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs e = AddProgressEventArgs.Completed(this.ArchiveNameForEvent);
				addProgress(this, e);
			}
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00053D30 File Offset: 0x00051F30
		internal void AfterAddEntry(ZipEntry entry)
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.AfterEntry(this.ArchiveNameForEvent, entry, this._entries.Count);
				addProgress(this, addProgressEventArgs);
				if (addProgressEventArgs.Cancel)
				{
					this._addOperationCanceled = true;
				}
			}
		}

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06000E77 RID: 3703 RVA: 0x00053D78 File Offset: 0x00051F78
		// (remove) Token: 0x06000E78 RID: 3704 RVA: 0x00053DB0 File Offset: 0x00051FB0
		public event EventHandler<ZipErrorEventArgs> ZipError;

		// Token: 0x06000E79 RID: 3705 RVA: 0x00053DE8 File Offset: 0x00051FE8
		internal bool OnZipErrorSaving(ZipEntry entry, Exception exc)
		{
			if (this.ZipError != null)
			{
				lock (this.LOCK)
				{
					ZipErrorEventArgs zipErrorEventArgs = ZipErrorEventArgs.Saving(this.Name, entry, exc);
					this.ZipError(this, zipErrorEventArgs);
					if (zipErrorEventArgs.Cancel)
					{
						this._saveOperationCanceled = true;
					}
				}
			}
			return this._saveOperationCanceled;
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00053E5C File Offset: 0x0005205C
		public void ExtractAll(string path)
		{
			this._InternalExtractAll(path, true);
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00053E66 File Offset: 0x00052066
		public void ExtractAll(string path, ExtractExistingFileAction extractExistingFile)
		{
			this.ExtractExistingFile = extractExistingFile;
			this._InternalExtractAll(path, true);
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00053E78 File Offset: 0x00052078
		private void _InternalExtractAll(string path, bool overrideExtractExistingProperty)
		{
			bool flag = this.Verbose;
			this._inExtractAll = true;
			try
			{
				this.OnExtractAllStarted(path);
				int num = 0;
				foreach (ZipEntry current in this._entries.Values)
				{
					if (flag)
					{
						this.StatusMessageTextWriter.WriteLine("\n{1,-22} {2,-8} {3,4}   {4,-8}  {0}", new object[]
						{
							"Name",
							"Modified",
							"Size",
							"Ratio",
							"Packed"
						});
						this.StatusMessageTextWriter.WriteLine(new string('-', 72));
						flag = false;
					}
					if (this.Verbose)
					{
						this.StatusMessageTextWriter.WriteLine("{1,-22} {2,-8} {3,4:F0}%   {4,-8} {0}", new object[]
						{
							current.FileName,
							current.LastModified.ToString("yyyy-MM-dd HH:mm:ss"),
							current.UncompressedSize,
							current.CompressionRatio,
							current.CompressedSize
						});
						if (!string.IsNullOrEmpty(current.Comment))
						{
							this.StatusMessageTextWriter.WriteLine("  Comment: {0}", current.Comment);
						}
					}
					current.Password = this._Password;
					this.OnExtractEntry(num, true, current, path);
					if (overrideExtractExistingProperty)
					{
						current.ExtractExistingFile = this.ExtractExistingFile;
					}
					current.Extract(path);
					num++;
					this.OnExtractEntry(num, false, current, path);
					if (this._extractOperationCanceled)
					{
						break;
					}
				}
				if (!this._extractOperationCanceled)
				{
					foreach (ZipEntry current2 in this._entries.Values)
					{
						if (current2.IsDirectory || current2.FileName.EndsWith("/"))
						{
							string fileOrDirectory = current2.FileName.StartsWith("/") ? Path.Combine(path, current2.FileName.Substring(1)) : Path.Combine(path, current2.FileName);
							current2._SetTimes(fileOrDirectory, false);
						}
					}
					this.OnExtractAllCompleted(path);
				}
			}
			finally
			{
				this._inExtractAll = false;
			}
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00054104 File Offset: 0x00052304
		public static ZipFile Read(string fileName)
		{
			return ZipFile.Read(fileName, null, null, null);
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x0005410F File Offset: 0x0005230F
		public static ZipFile Read(string fileName, ReadOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return ZipFile.Read(fileName, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00054138 File Offset: 0x00052338
		private static ZipFile Read(string fileName, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
		{
			ZipFile zipFile = new ZipFile();
			zipFile.AlternateEncoding = (encoding ?? ZipFile.DefaultEncoding);
			zipFile.AlternateEncodingUsage = ZipOption.Always;
			zipFile._StatusMessageTextWriter = statusMessageWriter;
			zipFile._name = fileName;
			if (readProgress != null)
			{
				zipFile.ReadProgress = readProgress;
			}
			if (zipFile.Verbose)
			{
				zipFile._StatusMessageTextWriter.WriteLine("reading from {0}...", fileName);
			}
			ZipFile.ReadIntoInstance(zipFile);
			zipFile._fileAlreadyExists = true;
			return zipFile;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x000541A1 File Offset: 0x000523A1
		public static ZipFile Read(Stream zipStream)
		{
			return ZipFile.Read(zipStream, null, null, null);
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x000541AC File Offset: 0x000523AC
		public static ZipFile Read(Stream zipStream, ReadOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return ZipFile.Read(zipStream, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x000541D4 File Offset: 0x000523D4
		private static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
		{
			if (zipStream == null)
			{
				throw new ArgumentNullException("zipStream");
			}
			ZipFile zipFile = new ZipFile();
			zipFile._StatusMessageTextWriter = statusMessageWriter;
			zipFile._alternateEncoding = (encoding ?? ZipFile.DefaultEncoding);
			zipFile._alternateEncodingUsage = ZipOption.Always;
			if (readProgress != null)
			{
				zipFile.ReadProgress += readProgress;
			}
			zipFile._readstream = ((zipStream.Position == 0L) ? zipStream : new OffsetStream(zipStream));
			zipFile._ReadStreamIsOurs = false;
			if (zipFile.Verbose)
			{
				zipFile._StatusMessageTextWriter.WriteLine("reading from stream...");
			}
			ZipFile.ReadIntoInstance(zipFile);
			return zipFile;
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0005425C File Offset: 0x0005245C
		private static void ReadIntoInstance(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			try
			{
				zf._readName = zf._name;
				if (!readStream.CanSeek)
				{
					ZipFile.ReadIntoInstance_Orig(zf);
					return;
				}
				zf.OnReadStarted();
				uint num = ZipFile.ReadFirstFourBytes(readStream);
				if (num == 101010256u)
				{
					return;
				}
				int num2 = 0;
				bool flag = false;
				long num3 = readStream.Length - 64L;
				long num4 = Math.Max(readStream.Length - 16384L, 10L);
				do
				{
					if (num3 < 0L)
					{
						num3 = 0L;
					}
					readStream.Seek(num3, SeekOrigin.Begin);
					long num5 = SharedUtilities.FindSignature(readStream, 101010256);
					if (num5 != -1L)
					{
						flag = true;
					}
					else
					{
						if (num3 == 0L)
						{
							break;
						}
						num2++;
						num3 -= (long)(32 * (num2 + 1) * num2);
					}
				}
				while (!flag && num3 > num4);
				if (flag)
				{
					zf._locEndOfCDS = readStream.Position - 4L;
					byte[] array = new byte[16];
					readStream.Read(array, 0, array.Length);
					zf._diskNumberWithCd = (uint)BitConverter.ToUInt16(array, 2);
					if (zf._diskNumberWithCd == 65535u)
					{
						throw new ZipException("Spanned archives with more than 65534 segments are not supported at this time.");
					}
					zf._diskNumberWithCd += 1u;
					int startIndex = 12;
					uint num6 = BitConverter.ToUInt32(array, startIndex);
					if (num6 == 4294967295u)
					{
						ZipFile.Zip64SeekToCentralDirectory(zf);
					}
					else
					{
						zf._OffsetOfCentralDirectory = num6;
						readStream.Seek((long)((ulong)num6), SeekOrigin.Begin);
					}
					ZipFile.ReadCentralDirectory(zf);
				}
				else
				{
					readStream.Seek(0L, SeekOrigin.Begin);
					ZipFile.ReadIntoInstance_Orig(zf);
				}
			}
			catch (Exception innerException)
			{
				if (zf._ReadStreamIsOurs && zf._readstream != null)
				{
					zf._readstream.Dispose();
					zf._readstream = null;
				}
				throw new ZipException("Cannot read that as a ZipFile", innerException);
			}
			zf._contentsChanged = false;
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0005441C File Offset: 0x0005261C
		private static void Zip64SeekToCentralDirectory(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			byte[] array = new byte[16];
			readStream.Seek(-40L, SeekOrigin.Current);
			readStream.Read(array, 0, 16);
			long num = BitConverter.ToInt64(array, 8);
			zf._OffsetOfCentralDirectory = 4294967295u;
			zf._OffsetOfCentralDirectory64 = num;
			readStream.Seek(num, SeekOrigin.Begin);
			uint num2 = (uint)SharedUtilities.ReadInt(readStream);
			if (num2 != 101075792u)
			{
				throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) looking for ZIP64 EoCD Record at position 0x{1:X8}", num2, readStream.Position));
			}
			readStream.Read(array, 0, 8);
			long num3 = BitConverter.ToInt64(array, 0);
			array = new byte[num3];
			readStream.Read(array, 0, array.Length);
			num = BitConverter.ToInt64(array, 36);
			readStream.Seek(num, SeekOrigin.Begin);
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x000544D8 File Offset: 0x000526D8
		private static uint ReadFirstFourBytes(Stream s)
		{
			return (uint)SharedUtilities.ReadInt(s);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x000544F0 File Offset: 0x000526F0
		private static void ReadCentralDirectory(ZipFile zf)
		{
			bool flag = false;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			ZipEntry zipEntry;
			while ((zipEntry = ZipEntry.ReadDirEntry(zf, dictionary)) != null)
			{
				zipEntry.ResetDirEntry();
				zf.OnReadEntry(true, null);
				if (zf.Verbose)
				{
					zf.StatusMessageTextWriter.WriteLine("entry {0}", zipEntry.FileName);
				}
				zf._entries.Add(zipEntry.FileName, zipEntry);
				if (zipEntry._InputUsesZip64)
				{
					flag = true;
				}
				dictionary.Add(zipEntry.FileName, null);
			}
			if (flag)
			{
				zf.UseZip64WhenSaving = Zip64Option.Always;
			}
			if (zf._locEndOfCDS > 0L)
			{
				zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
			}
			ZipFile.ReadCentralDirectoryFooter(zf);
			if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
			{
				zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
			}
			if (zf.Verbose)
			{
				zf.StatusMessageTextWriter.WriteLine("read in {0} entries.", zf._entries.Count);
			}
			zf.OnReadCompleted();
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x000545F0 File Offset: 0x000527F0
		private static void ReadIntoInstance_Orig(ZipFile zf)
		{
			zf.OnReadStarted();
			zf._entries = new Dictionary<string, ZipEntry>();
			if (zf.Verbose)
			{
				if (zf.Name == null)
				{
					zf.StatusMessageTextWriter.WriteLine("Reading zip from stream...");
				}
				else
				{
					zf.StatusMessageTextWriter.WriteLine("Reading zip {0}...", zf.Name);
				}
			}
			bool first = true;
			ZipContainer zc = new ZipContainer(zf);
			ZipEntry zipEntry;
			while ((zipEntry = ZipEntry.ReadEntry(zc, first)) != null)
			{
				if (zf.Verbose)
				{
					zf.StatusMessageTextWriter.WriteLine("  {0}", zipEntry.FileName);
				}
				zf._entries.Add(zipEntry.FileName, zipEntry);
				first = false;
			}
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				ZipEntry zipEntry2;
				while ((zipEntry2 = ZipEntry.ReadDirEntry(zf, dictionary)) != null)
				{
					ZipEntry zipEntry3 = zf._entries[zipEntry2.FileName];
					if (zipEntry3 != null)
					{
						zipEntry3._Comment = zipEntry2.Comment;
						if (zipEntry2.IsDirectory)
						{
							zipEntry3.MarkAsDirectory();
						}
					}
					dictionary.Add(zipEntry2.FileName, null);
				}
				if (zf._locEndOfCDS > 0L)
				{
					zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
				}
				ZipFile.ReadCentralDirectoryFooter(zf);
				if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
				{
					zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
				}
			}
			catch (ZipException)
			{
			}
			catch (IOException)
			{
			}
			zf.OnReadCompleted();
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x0005475C File Offset: 0x0005295C
		private static void ReadCentralDirectoryFooter(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			int num = SharedUtilities.ReadSignature(readStream);
			int num2 = 0;
			byte[] array;
			if ((long)num == 101075792L)
			{
				array = new byte[52];
				readStream.Read(array, 0, array.Length);
				long num3 = BitConverter.ToInt64(array, 0);
				if (num3 < 44L)
				{
					throw new ZipException("Bad size in the ZIP64 Central Directory.");
				}
				zf._versionMadeBy = BitConverter.ToUInt16(array, num2);
				num2 += 2;
				zf._versionNeededToExtract = BitConverter.ToUInt16(array, num2);
				num2 += 2;
				zf._diskNumberWithCd = BitConverter.ToUInt32(array, num2);
				num2 += 2;
				array = new byte[num3 - 44L];
				readStream.Read(array, 0, array.Length);
				num = SharedUtilities.ReadSignature(readStream);
				if ((long)num != 117853008L)
				{
					throw new ZipException("Inconsistent metadata in the ZIP64 Central Directory.");
				}
				array = new byte[16];
				readStream.Read(array, 0, array.Length);
				num = SharedUtilities.ReadSignature(readStream);
			}
			if ((long)num != 101010256L)
			{
				readStream.Seek(-4L, SeekOrigin.Current);
				throw new BadReadException(string.Format("Bad signature ({0:X8}) at position 0x{1:X8}", num, readStream.Position));
			}
			array = new byte[16];
			zf.ReadStream.Read(array, 0, array.Length);
			if (zf._diskNumberWithCd == 0u)
			{
				zf._diskNumberWithCd = (uint)BitConverter.ToUInt16(array, 2);
			}
			ZipFile.ReadZipFileComment(zf);
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x000548A4 File Offset: 0x00052AA4
		private static void ReadZipFileComment(ZipFile zf)
		{
			byte[] array = new byte[2];
			zf.ReadStream.Read(array, 0, array.Length);
			short num = (short)((int)array[0] + (int)array[1] * 256);
			if (num > 0)
			{
				array = new byte[(int)num];
				zf.ReadStream.Read(array, 0, array.Length);
				string @string = zf.AlternateEncoding.GetString(array, 0, array.Length);
				zf.Comment = @string;
			}
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x0005490C File Offset: 0x00052B0C
		public static bool IsZipFile(string fileName)
		{
			return ZipFile.IsZipFile(fileName, false);
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00054918 File Offset: 0x00052B18
		public static bool IsZipFile(string fileName, bool testExtract)
		{
			bool result = false;
			try
			{
				if (!File.Exists(fileName))
				{
					return false;
				}
				using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					result = ZipFile.IsZipFile(fileStream, testExtract);
				}
			}
			catch (IOException)
			{
			}
			catch (ZipException)
			{
			}
			return result;
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00054984 File Offset: 0x00052B84
		public static bool IsZipFile(Stream stream, bool testExtract)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			bool result = false;
			try
			{
				if (!stream.CanRead)
				{
					return false;
				}
				Stream @null = Stream.Null;
				using (ZipFile zipFile = ZipFile.Read(stream, null, null, null))
				{
					if (testExtract)
					{
						foreach (ZipEntry current in zipFile)
						{
							if (!current.IsDirectory)
							{
								current.Extract(@null);
							}
						}
					}
				}
				result = true;
			}
			catch (IOException)
			{
			}
			catch (ZipException)
			{
			}
			return result;
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00054A48 File Offset: 0x00052C48
		private void DeleteFileWithRetry(string filename)
		{
			bool flag = false;
			int num = 3;
			int num2 = 0;
			while (num2 < num && !flag)
			{
				try
				{
					File.Delete(filename);
					flag = true;
				}
				catch (UnauthorizedAccessException)
				{
					Console.WriteLine("************************************************** Retry delete.");
					Thread.Sleep(200 + num2 * 200);
				}
				num2++;
			}
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00054AA4 File Offset: 0x00052CA4
		public void Save()
		{
			try
			{
				bool flag = false;
				this._saveOperationCanceled = false;
				this._numberOfSegmentsForMostRecentSave = 0u;
				this.OnSaveStarted();
				if (this.WriteStream == null)
				{
					throw new BadStateException("You haven't specified where to save the zip.");
				}
				if (this._name != null && this._name.EndsWith(".exe") && !this._SavingSfx)
				{
					throw new BadStateException("You specified an EXE for a plain zip file.");
				}
				if (!this._contentsChanged)
				{
					this.OnSaveCompleted();
					if (this.Verbose)
					{
						this.StatusMessageTextWriter.WriteLine("No save is necessary....");
					}
				}
				else
				{
					this.Reset(true);
					if (this.Verbose)
					{
						this.StatusMessageTextWriter.WriteLine("saving....");
					}
					if (this._entries.Count >= 65535 && this._zip64 == Zip64Option.Default)
					{
						throw new ZipException("The number of entries is 65535 or greater. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
					}
					int num = 0;
					ICollection<ZipEntry> collection = this.SortEntriesBeforeSaving ? this.EntriesSorted : this.Entries;
					foreach (ZipEntry current in collection)
					{
						this.OnSaveEntry(num, current, true);
						current.Write(this.WriteStream);
						if (this._saveOperationCanceled)
						{
							break;
						}
						num++;
						this.OnSaveEntry(num, current, false);
						if (this._saveOperationCanceled)
						{
							break;
						}
						if (current.IncludedInMostRecentSave)
						{
							flag |= current.OutputUsedZip64.Value;
						}
					}
					if (!this._saveOperationCanceled)
					{
						ZipSegmentedStream zipSegmentedStream = this.WriteStream as ZipSegmentedStream;
						this._numberOfSegmentsForMostRecentSave = ((zipSegmentedStream != null) ? zipSegmentedStream.CurrentSegment : 1u);
						bool flag2 = ZipOutput.WriteCentralDirectoryStructure(this.WriteStream, collection, this._numberOfSegmentsForMostRecentSave, this._zip64, this.Comment, new ZipContainer(this));
						this.OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
						this._hasBeenSaved = true;
						this._contentsChanged = false;
						flag |= flag2;
						this._OutputUsesZip64 = new bool?(flag);
						if (this._name != null && (this._temporaryFileName != null || zipSegmentedStream != null))
						{
							this.WriteStream.Dispose();
							if (this._saveOperationCanceled)
							{
								return;
							}
							if (this._fileAlreadyExists && this._readstream != null)
							{
								this._readstream.Close();
								this._readstream = null;
								foreach (ZipEntry current2 in collection)
								{
									ZipSegmentedStream zipSegmentedStream2 = current2._archiveStream as ZipSegmentedStream;
									if (zipSegmentedStream2 != null)
									{
										zipSegmentedStream2.Dispose();
									}
									current2._archiveStream = null;
								}
							}
							string text = null;
							if (File.Exists(this._name))
							{
								text = this._name + "." + Path.GetRandomFileName();
								if (File.Exists(text))
								{
									this.DeleteFileWithRetry(text);
								}
								File.Move(this._name, text);
							}
							this.OnSaveEvent(ZipProgressEventType.Saving_BeforeRenameTempArchive);
							File.Move((zipSegmentedStream != null) ? zipSegmentedStream.CurrentTempName : this._temporaryFileName, this._name);
							this.OnSaveEvent(ZipProgressEventType.Saving_AfterRenameTempArchive);
							if (text != null)
							{
								try
								{
									if (File.Exists(text))
									{
										File.Delete(text);
									}
								}
								catch
								{
								}
							}
							this._fileAlreadyExists = true;
						}
						ZipFile.NotifyEntriesSaveComplete(collection);
						this.OnSaveCompleted();
						this._JustSaved = true;
					}
				}
			}
			finally
			{
				this.CleanupAfterSaveOperation();
			}
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00054E34 File Offset: 0x00053034
		private static void NotifyEntriesSaveComplete(ICollection<ZipEntry> c)
		{
			foreach (ZipEntry current in c)
			{
				current.NotifySaveComplete();
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00054E7C File Offset: 0x0005307C
		private void RemoveTempFile()
		{
			try
			{
				if (File.Exists(this._temporaryFileName))
				{
					File.Delete(this._temporaryFileName);
				}
			}
			catch (IOException ex)
			{
				if (this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("ZipFile::Save: could not delete temp file: {0}.", ex.Message);
				}
			}
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00054ED4 File Offset: 0x000530D4
		private void CleanupAfterSaveOperation()
		{
			if (this._name != null)
			{
				if (this._writestream != null)
				{
					try
					{
						this._writestream.Dispose();
					}
					catch (IOException)
					{
					}
				}
				this._writestream = null;
				if (this._temporaryFileName != null)
				{
					this.RemoveTempFile();
					this._temporaryFileName = null;
				}
			}
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00054F30 File Offset: 0x00053130
		public void Save(string fileName)
		{
			if (this._name == null)
			{
				this._writestream = null;
			}
			else
			{
				this._readName = this._name;
			}
			this._name = fileName;
			if (Directory.Exists(this._name))
			{
				throw new ZipException("Bad Directory", new ArgumentException("That name specifies an existing directory. Please specify a filename.", "fileName"));
			}
			this._contentsChanged = true;
			this._fileAlreadyExists = File.Exists(this._name);
			this.Save();
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00054FA8 File Offset: 0x000531A8
		public void Save(Stream outputStream)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (!outputStream.CanWrite)
			{
				throw new ArgumentException("Must be a writable stream.", "outputStream");
			}
			this._name = null;
			this._writestream = new CountingStream(outputStream);
			this._contentsChanged = true;
			this._fileAlreadyExists = false;
			this.Save();
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00055004 File Offset: 0x00053204
		public void SaveSelfExtractor(string exeToGenerate, SelfExtractorFlavor flavor)
		{
			this.SaveSelfExtractor(exeToGenerate, new SelfExtractorSaveOptions
			{
				Flavor = flavor
			});
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00055028 File Offset: 0x00053228
		public void SaveSelfExtractor(string exeToGenerate, SelfExtractorSaveOptions options)
		{
			if (this._name == null)
			{
				this._writestream = null;
			}
			this._SavingSfx = true;
			this._name = exeToGenerate;
			if (Directory.Exists(this._name))
			{
				throw new ZipException("Bad Directory", new ArgumentException("That name specifies an existing directory. Please specify a filename.", "exeToGenerate"));
			}
			this._contentsChanged = true;
			this._fileAlreadyExists = File.Exists(this._name);
			this._SaveSfxStub(exeToGenerate, options);
			this.Save();
			this._SavingSfx = false;
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x000550A8 File Offset: 0x000532A8
		private static void ExtractResourceToFile(Assembly a, string resourceName, string filename)
		{
			byte[] array = new byte[1024];
			using (Stream manifestResourceStream = a.GetManifestResourceStream(resourceName))
			{
				if (manifestResourceStream == null)
				{
					throw new ZipException(string.Format("missing resource '{0}'", resourceName));
				}
				using (FileStream fileStream = File.OpenWrite(filename))
				{
					int num;
					do
					{
						num = manifestResourceStream.Read(array, 0, array.Length);
						fileStream.Write(array, 0, num);
					}
					while (num > 0);
				}
			}
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00055134 File Offset: 0x00053334
		private void _SaveSfxStub(string exeToGenerate, SelfExtractorSaveOptions options)
		{
			string text = null;
			string text2 = null;
			string dir = null;
			try
			{
				if (File.Exists(exeToGenerate) && this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("The existing file ({0}) will be overwritten.", exeToGenerate);
				}
				if (!exeToGenerate.EndsWith(".exe") && this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("Warning: The generated self-extracting file will not have an .exe extension.");
				}
				dir = (this.TempFileFolder ?? Path.GetDirectoryName(exeToGenerate));
				text = ZipFile.GenerateTempPathname(dir, "exe");
				Assembly assembly = typeof(ZipFile).Assembly;
				using (CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider())
				{
					ZipFile.ExtractorSettings extractorSettings = null;
					ZipFile.ExtractorSettings[] settingsList = ZipFile.SettingsList;
					for (int i = 0; i < settingsList.Length; i++)
					{
						ZipFile.ExtractorSettings extractorSettings2 = settingsList[i];
						if (extractorSettings2.Flavor == options.Flavor)
						{
							extractorSettings = extractorSettings2;
							break;
						}
					}
					if (extractorSettings == null)
					{
						throw new BadStateException(string.Format("While saving a Self-Extracting Zip, Cannot find that flavor ({0})?", options.Flavor));
					}
					CompilerParameters compilerParameters = new CompilerParameters();
					compilerParameters.ReferencedAssemblies.Add(assembly.Location);
					if (extractorSettings.ReferencedAssemblies != null)
					{
						foreach (string current in extractorSettings.ReferencedAssemblies)
						{
							compilerParameters.ReferencedAssemblies.Add(current);
						}
					}
					compilerParameters.GenerateInMemory = false;
					compilerParameters.GenerateExecutable = true;
					compilerParameters.IncludeDebugInformation = false;
					compilerParameters.CompilerOptions = "";
					Assembly executingAssembly = Assembly.GetExecutingAssembly();
					StringBuilder stringBuilder = new StringBuilder();
					string text3 = ZipFile.GenerateTempPathname(dir, "cs");
					using (ZipFile zipFile = ZipFile.Read(executingAssembly.GetManifestResourceStream("Ionic.Zip.Resources.ZippedResources.zip")))
					{
						text2 = ZipFile.GenerateTempPathname(dir, "tmp");
						if (string.IsNullOrEmpty(options.IconFile))
						{
							Directory.CreateDirectory(text2);
							ZipEntry zipEntry = zipFile["zippedFile.ico"];
							if ((zipEntry.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
							{
								zipEntry.Attributes ^= FileAttributes.ReadOnly;
							}
							zipEntry.Extract(text2);
							string arg = Path.Combine(text2, "zippedFile.ico");
							CompilerParameters expr_1F1 = compilerParameters;
							expr_1F1.CompilerOptions += string.Format("/win32icon:\"{0}\"", arg);
						}
						else
						{
							CompilerParameters expr_210 = compilerParameters;
							expr_210.CompilerOptions += string.Format("/win32icon:\"{0}\"", options.IconFile);
						}
						compilerParameters.OutputAssembly = text;
						if (options.Flavor == SelfExtractorFlavor.WinFormsApplication)
						{
							CompilerParameters expr_243 = compilerParameters;
							expr_243.CompilerOptions += " /target:winexe";
						}
						if (!string.IsNullOrEmpty(options.AdditionalCompilerSwitches))
						{
							CompilerParameters expr_267 = compilerParameters;
							expr_267.CompilerOptions = expr_267.CompilerOptions + " " + options.AdditionalCompilerSwitches;
						}
						if (string.IsNullOrEmpty(compilerParameters.CompilerOptions))
						{
							compilerParameters.CompilerOptions = null;
						}
						if (extractorSettings.CopyThroughResources != null && extractorSettings.CopyThroughResources.Count != 0)
						{
							if (!Directory.Exists(text2))
							{
								Directory.CreateDirectory(text2);
							}
							foreach (string current2 in extractorSettings.CopyThroughResources)
							{
								string text4 = Path.Combine(text2, current2);
								ZipFile.ExtractResourceToFile(executingAssembly, current2, text4);
								compilerParameters.EmbeddedResources.Add(text4);
							}
						}
						compilerParameters.EmbeddedResources.Add(assembly.Location);
						stringBuilder.Append("// " + Path.GetFileName(text3) + "\n").Append("// --------------------------------------------\n//\n").Append("// This SFX source file was generated by DotNetZip ").Append(ZipFile.LibraryVersion.ToString()).Append("\n//         at ").Append(DateTime.Now.ToString("yyyy MMMM dd  HH:mm:ss")).Append("\n//\n// --------------------------------------------\n\n\n");
						if (!string.IsNullOrEmpty(options.Description))
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyTitle(\"" + options.Description.Replace("\"", "") + "\")]\n");
						}
						else
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyTitle(\"DotNetZip SFX Archive\")]\n");
						}
						if (!string.IsNullOrEmpty(options.ProductVersion))
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyInformationalVersion(\"" + options.ProductVersion.Replace("\"", "") + "\")]\n");
						}
						string str = string.IsNullOrEmpty(options.Copyright) ? "Extractor: Copyright © Dino Chiesa 2008-2011" : options.Copyright.Replace("\"", "");
						if (!string.IsNullOrEmpty(options.ProductName))
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyProduct(\"").Append(options.ProductName.Replace("\"", "")).Append("\")]\n");
						}
						else
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyProduct(\"DotNetZip\")]\n");
						}
						stringBuilder.Append("[assembly: System.Reflection.AssemblyCopyright(\"" + str + "\")]\n").Append(string.Format("[assembly: System.Reflection.AssemblyVersion(\"{0}\")]\n", ZipFile.LibraryVersion.ToString()));
						if (options.FileVersion != null)
						{
							stringBuilder.Append(string.Format("[assembly: System.Reflection.AssemblyFileVersion(\"{0}\")]\n", options.FileVersion.ToString()));
						}
						stringBuilder.Append("\n\n\n");
						string text5 = options.DefaultExtractDirectory;
						if (text5 != null)
						{
							text5 = text5.Replace("\"", "").Replace("\\", "\\\\");
						}
						string text6 = options.PostExtractCommandLine;
						if (text6 != null)
						{
							text6 = text6.Replace("\\", "\\\\");
							text6 = text6.Replace("\"", "\\\"");
						}
						foreach (string current3 in extractorSettings.ResourcesToCompile)
						{
							using (Stream stream = zipFile[current3].OpenReader())
							{
								if (stream == null)
								{
									throw new ZipException(string.Format("missing resource '{0}'", current3));
								}
								using (StreamReader streamReader = new StreamReader(stream))
								{
									while (streamReader.Peek() >= 0)
									{
										string text7 = streamReader.ReadLine();
										if (text5 != null)
										{
											text7 = text7.Replace("@@EXTRACTLOCATION", text5);
										}
										text7 = text7.Replace("@@REMOVE_AFTER_EXECUTE", options.RemoveUnpackedFilesAfterExecute.ToString());
										text7 = text7.Replace("@@QUIET", options.Quiet.ToString());
										if (!string.IsNullOrEmpty(options.SfxExeWindowTitle))
										{
											text7 = text7.Replace("@@SFX_EXE_WINDOW_TITLE", options.SfxExeWindowTitle);
										}
										text7 = text7.Replace("@@EXTRACT_EXISTING_FILE", ((int)options.ExtractExistingFile).ToString());
										if (text6 != null)
										{
											text7 = text7.Replace("@@POST_UNPACK_CMD_LINE", text6);
										}
										stringBuilder.Append(text7).Append("\n");
									}
								}
								stringBuilder.Append("\n\n");
							}
						}
					}
					string text8 = stringBuilder.ToString();
					CompilerResults compilerResults = cSharpCodeProvider.CompileAssemblyFromSource(compilerParameters, new string[]
					{
						text8
					});
					if (compilerResults == null)
					{
						throw new SfxGenerationException("Cannot compile the extraction logic!");
					}
					if (this.Verbose)
					{
						foreach (string current4 in compilerResults.Output)
						{
							this.StatusMessageTextWriter.WriteLine(current4);
						}
					}
					if (compilerResults.Errors.Count != 0)
					{
						using (TextWriter textWriter = new StreamWriter(text3))
						{
							textWriter.Write(text8);
							textWriter.Write("\n\n\n// ------------------------------------------------------------------\n");
							textWriter.Write("// Errors during compilation: \n//\n");
							string fileName = Path.GetFileName(text3);
							foreach (CompilerError compilerError in compilerResults.Errors)
							{
								textWriter.Write(string.Format("//   {0}({1},{2}): {3} {4}: {5}\n//\n", new object[]
								{
									fileName,
									compilerError.Line,
									compilerError.Column,
									compilerError.IsWarning ? "Warning" : "error",
									compilerError.ErrorNumber,
									compilerError.ErrorText
								}));
							}
						}
						throw new SfxGenerationException(string.Format("Errors compiling the extraction logic!  {0}", text3));
					}
					this.OnSaveEvent(ZipProgressEventType.Saving_AfterCompileSelfExtractor);
					using (Stream stream2 = File.OpenRead(text))
					{
						byte[] array = new byte[4000];
						int num = 1;
						while (num != 0)
						{
							num = stream2.Read(array, 0, array.Length);
							if (num != 0)
							{
								this.WriteStream.Write(array, 0, num);
							}
						}
					}
				}
				this.OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
			}
			finally
			{
				try
				{
					if (Directory.Exists(text2))
					{
						try
						{
							Directory.Delete(text2, true);
						}
						catch (IOException arg2)
						{
							this.StatusMessageTextWriter.WriteLine("Warning: Exception: {0}", arg2);
						}
					}
					if (File.Exists(text))
					{
						try
						{
							File.Delete(text);
						}
						catch (IOException arg3)
						{
							this.StatusMessageTextWriter.WriteLine("Warning: Exception: {0}", arg3);
						}
					}
				}
				catch (IOException)
				{
				}
			}
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00055BCC File Offset: 0x00053DCC
		internal static string GenerateTempPathname(string dir, string extension)
		{
			string name = Assembly.GetExecutingAssembly().GetName().Name;
			string text2;
			do
			{
				string text = Guid.NewGuid().ToString();
				string path = string.Format("{0}-{1}-{2}.{3}", new object[]
				{
					name,
					DateTime.Now.ToString("yyyyMMMdd-HHmmss"),
					text,
					extension
				});
				text2 = Path.Combine(dir, path);
			}
			while (File.Exists(text2) || Directory.Exists(text2));
			return text2;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00055C54 File Offset: 0x00053E54
		public void AddSelectedFiles(string selectionCriteria)
		{
			this.AddSelectedFiles(selectionCriteria, ".", null, false);
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00055C64 File Offset: 0x00053E64
		public void AddSelectedFiles(string selectionCriteria, bool recurseDirectories)
		{
			this.AddSelectedFiles(selectionCriteria, ".", null, recurseDirectories);
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00055C74 File Offset: 0x00053E74
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, null, false);
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00055C80 File Offset: 0x00053E80
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, bool recurseDirectories)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, null, recurseDirectories);
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00055C8C File Offset: 0x00053E8C
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, false);
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00055C98 File Offset: 0x00053E98
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories)
		{
			this._AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, false);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00055CA6 File Offset: 0x00053EA6
		public void UpdateSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories)
		{
			this._AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, true);
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00055CB4 File Offset: 0x00053EB4
		private string EnsureendInSlash(string s)
		{
			if (s.EndsWith("\\"))
			{
				return s;
			}
			return s + "\\";
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00055CD0 File Offset: 0x00053ED0
		private void _AddOrUpdateSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories, bool wantUpdate)
		{
			if (directoryOnDisk == null && Directory.Exists(selectionCriteria))
			{
				directoryOnDisk = selectionCriteria;
				selectionCriteria = "*.*";
			}
			else if (string.IsNullOrEmpty(directoryOnDisk))
			{
				directoryOnDisk = ".";
			}
			while (directoryOnDisk.EndsWith("\\"))
			{
				directoryOnDisk = directoryOnDisk.Substring(0, directoryOnDisk.Length - 1);
			}
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding selection '{0}' from dir '{1}'...", selectionCriteria, directoryOnDisk);
			}
			FileSelector fileSelector = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints);
			ReadOnlyCollection<string> readOnlyCollection = fileSelector.SelectFiles(directoryOnDisk, recurseDirectories);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("found {0} files...", readOnlyCollection.Count);
			}
			this.OnAddStarted();
			AddOrUpdateAction action = wantUpdate ? AddOrUpdateAction.AddOrUpdate : AddOrUpdateAction.AddOnly;
			foreach (string current in readOnlyCollection)
			{
				string text = (directoryPathInArchive == null) ? null : ZipFile.ReplaceLeadingDirectory(Path.GetDirectoryName(current), directoryOnDisk, directoryPathInArchive);
				if (File.Exists(current))
				{
					if (wantUpdate)
					{
						this.UpdateFile(current, text);
					}
					else
					{
						this.AddFile(current, text);
					}
				}
				else
				{
					this.AddOrUpdateDirectoryImpl(current, text, action, false, 0);
				}
			}
			this.OnAddCompleted();
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x00055E0C File Offset: 0x0005400C
		private static string ReplaceLeadingDirectory(string original, string pattern, string replacement)
		{
			string text = original.ToUpper();
			string text2 = pattern.ToUpper();
			int num = text.IndexOf(text2);
			if (num != 0)
			{
				return original;
			}
			return replacement + original.Substring(text2.Length);
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x00055E48 File Offset: 0x00054048
		public ICollection<ZipEntry> SelectEntries(string selectionCriteria)
		{
			FileSelector fileSelector = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints);
			return fileSelector.SelectEntries(this);
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x00055E6C File Offset: 0x0005406C
		public ICollection<ZipEntry> SelectEntries(string selectionCriteria, string directoryPathInArchive)
		{
			FileSelector fileSelector = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints);
			return fileSelector.SelectEntries(this, directoryPathInArchive);
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00055E90 File Offset: 0x00054090
		public int RemoveSelectedEntries(string selectionCriteria)
		{
			ICollection<ZipEntry> collection = this.SelectEntries(selectionCriteria);
			this.RemoveEntries(collection);
			return collection.Count;
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00055EB4 File Offset: 0x000540B4
		public int RemoveSelectedEntries(string selectionCriteria, string directoryPathInArchive)
		{
			ICollection<ZipEntry> collection = this.SelectEntries(selectionCriteria, directoryPathInArchive);
			this.RemoveEntries(collection);
			return collection.Count;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00055ED8 File Offset: 0x000540D8
		public void ExtractSelectedEntries(string selectionCriteria)
		{
			foreach (ZipEntry current in this.SelectEntries(selectionCriteria))
			{
				current.Password = this._Password;
				current.Extract();
			}
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00055F34 File Offset: 0x00054134
		public void ExtractSelectedEntries(string selectionCriteria, ExtractExistingFileAction extractExistingFile)
		{
			foreach (ZipEntry current in this.SelectEntries(selectionCriteria))
			{
				current.Password = this._Password;
				current.Extract(extractExistingFile);
			}
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00055F90 File Offset: 0x00054190
		public void ExtractSelectedEntries(string selectionCriteria, string directoryPathInArchive)
		{
			foreach (ZipEntry current in this.SelectEntries(selectionCriteria, directoryPathInArchive))
			{
				current.Password = this._Password;
				current.Extract();
			}
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00055FEC File Offset: 0x000541EC
		public void ExtractSelectedEntries(string selectionCriteria, string directoryInArchive, string extractDirectory)
		{
			foreach (ZipEntry current in this.SelectEntries(selectionCriteria, directoryInArchive))
			{
				current.Password = this._Password;
				current.Extract(extractDirectory);
			}
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00056048 File Offset: 0x00054248
		public void ExtractSelectedEntries(string selectionCriteria, string directoryPathInArchive, string extractDirectory, ExtractExistingFileAction extractExistingFile)
		{
			foreach (ZipEntry current in this.SelectEntries(selectionCriteria, directoryPathInArchive))
			{
				current.Password = this._Password;
				current.Extract(extractDirectory, extractExistingFile);
			}
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x000561E0 File Offset: 0x000543E0
		public IEnumerator<ZipEntry> GetEnumerator()
		{
			foreach (ZipEntry current in this._entries.Values)
			{
				yield return current;
			}
			yield break;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x000561FC File Offset: 0x000543FC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00056204 File Offset: 0x00054404
		[DispId(-4)]
		public IEnumerator GetNewEnum()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040007CA RID: 1994
		private TextWriter _StatusMessageTextWriter;

		// Token: 0x040007CB RID: 1995
		private bool _CaseSensitiveRetrieval;

		// Token: 0x040007CC RID: 1996
		private Stream _readstream;

		// Token: 0x040007CD RID: 1997
		private Stream _writestream;

		// Token: 0x040007CE RID: 1998
		private ushort _versionMadeBy;

		// Token: 0x040007CF RID: 1999
		private ushort _versionNeededToExtract;

		// Token: 0x040007D0 RID: 2000
		private uint _diskNumberWithCd;

		// Token: 0x040007D1 RID: 2001
		private int _maxOutputSegmentSize;

		// Token: 0x040007D2 RID: 2002
		private uint _numberOfSegmentsForMostRecentSave;

		// Token: 0x040007D3 RID: 2003
		private ZipErrorAction _zipErrorAction;

		// Token: 0x040007D4 RID: 2004
		private bool _disposed;

		// Token: 0x040007D5 RID: 2005
		private Dictionary<string, ZipEntry> _entries;

		// Token: 0x040007D6 RID: 2006
		private List<ZipEntry> _zipEntriesAsList;

		// Token: 0x040007D7 RID: 2007
		private string _name;

		// Token: 0x040007D8 RID: 2008
		private string _readName;

		// Token: 0x040007D9 RID: 2009
		private string _Comment;

		// Token: 0x040007DA RID: 2010
		internal string _Password;

		// Token: 0x040007DB RID: 2011
		private bool _emitNtfsTimes = true;

		// Token: 0x040007DC RID: 2012
		private bool _emitUnixTimes;

		// Token: 0x040007DD RID: 2013
		private CompressionStrategy _Strategy;

		// Token: 0x040007DE RID: 2014
		private CompressionMethod _compressionMethod = CompressionMethod.Deflate;

		// Token: 0x040007DF RID: 2015
		private bool _fileAlreadyExists;

		// Token: 0x040007E0 RID: 2016
		private string _temporaryFileName;

		// Token: 0x040007E1 RID: 2017
		private bool _contentsChanged;

		// Token: 0x040007E2 RID: 2018
		private bool _hasBeenSaved;

		// Token: 0x040007E3 RID: 2019
		private string _TempFileFolder;

		// Token: 0x040007E4 RID: 2020
		private bool _ReadStreamIsOurs = true;

		// Token: 0x040007E5 RID: 2021
		private object LOCK = new object();

		// Token: 0x040007E6 RID: 2022
		private bool _saveOperationCanceled;

		// Token: 0x040007E7 RID: 2023
		private bool _extractOperationCanceled;

		// Token: 0x040007E8 RID: 2024
		private bool _addOperationCanceled;

		// Token: 0x040007E9 RID: 2025
		private EncryptionAlgorithm _Encryption;

		// Token: 0x040007EA RID: 2026
		private bool _JustSaved;

		// Token: 0x040007EB RID: 2027
		private long _locEndOfCDS = -1L;

		// Token: 0x040007EC RID: 2028
		private uint _OffsetOfCentralDirectory;

		// Token: 0x040007ED RID: 2029
		private long _OffsetOfCentralDirectory64;

		// Token: 0x040007EE RID: 2030
		private bool? _OutputUsesZip64;

		// Token: 0x040007EF RID: 2031
		internal bool _inExtractAll;

		// Token: 0x040007F0 RID: 2032
		private Encoding _alternateEncoding = Encoding.GetEncoding("IBM437");

		// Token: 0x040007F1 RID: 2033
		private ZipOption _alternateEncodingUsage;

		// Token: 0x040007F2 RID: 2034
		private static Encoding _defaultEncoding = Encoding.GetEncoding("IBM437");

		// Token: 0x040007F3 RID: 2035
		private int _BufferSize = ZipFile.BufferSizeDefault;

		// Token: 0x040007F4 RID: 2036
		internal ParallelDeflateOutputStream ParallelDeflater;

		// Token: 0x040007F5 RID: 2037
		private long _ParallelDeflateThreshold;

		// Token: 0x040007F6 RID: 2038
		private int _maxBufferPairs = 16;

		// Token: 0x040007F7 RID: 2039
		internal Zip64Option _zip64;

		// Token: 0x040007F8 RID: 2040
		private bool _SavingSfx;

		// Token: 0x040007F9 RID: 2041
		public static readonly int BufferSizeDefault = 32768;

		// Token: 0x040007FC RID: 2044
		private long _lengthOfReadStream = -99L;

		// Token: 0x04000800 RID: 2048
		private static ZipFile.ExtractorSettings[] SettingsList = new ZipFile.ExtractorSettings[]
		{
			new ZipFile.ExtractorSettings
			{
				Flavor = SelfExtractorFlavor.WinFormsApplication,
				ReferencedAssemblies = new List<string>
				{
					"System.dll",
					"System.Windows.Forms.dll",
					"System.Drawing.dll"
				},
				CopyThroughResources = new List<string>
				{
					"Ionic.Zip.WinFormsSelfExtractorStub.resources",
					"Ionic.Zip.Forms.PasswordDialog.resources",
					"Ionic.Zip.Forms.ZipContentsDialog.resources"
				},
				ResourcesToCompile = new List<string>
				{
					"WinFormsSelfExtractorStub.cs",
					"WinFormsSelfExtractorStub.Designer.cs",
					"PasswordDialog.cs",
					"PasswordDialog.Designer.cs",
					"ZipContentsDialog.cs",
					"ZipContentsDialog.Designer.cs",
					"FolderBrowserDialogEx.cs"
				}
			},
			new ZipFile.ExtractorSettings
			{
				Flavor = SelfExtractorFlavor.ConsoleApplication,
				ReferencedAssemblies = new List<string>
				{
					"System.dll"
				},
				CopyThroughResources = null,
				ResourcesToCompile = new List<string>
				{
					"CommandLineSelfExtractorStub.cs"
				}
			}
		};

		// Token: 0x02000156 RID: 342
		private class ExtractorSettings
		{
			// Token: 0x04000809 RID: 2057
			public SelfExtractorFlavor Flavor;

			// Token: 0x0400080A RID: 2058
			public List<string> ReferencedAssemblies;

			// Token: 0x0400080B RID: 2059
			public List<string> CopyThroughResources;

			// Token: 0x0400080C RID: 2060
			public List<string> ResourcesToCompile;
		}
	}
}
