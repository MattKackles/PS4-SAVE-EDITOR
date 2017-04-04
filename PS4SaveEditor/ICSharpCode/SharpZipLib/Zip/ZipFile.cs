using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Encryption;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000ED RID: 237
	public class ZipFile : IEnumerable, IDisposable
	{
		// Token: 0x060009B8 RID: 2488 RVA: 0x0003505C File Offset: 0x0003325C
		private void OnKeysRequired(string fileName)
		{
			if (this.KeysRequired != null)
			{
				KeysRequiredEventArgs keysRequiredEventArgs = new KeysRequiredEventArgs(fileName, this.key);
				this.KeysRequired(this, keysRequiredEventArgs);
				this.key = keysRequiredEventArgs.Key;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x00035097 File Offset: 0x00033297
		// (set) Token: 0x060009BA RID: 2490 RVA: 0x0003509F File Offset: 0x0003329F
		private byte[] Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		// Token: 0x170002CB RID: 715
		// (set) Token: 0x060009BB RID: 2491 RVA: 0x000350A8 File Offset: 0x000332A8
		public string Password
		{
			set
			{
				if (value == null || value.Length == 0)
				{
					this.key = null;
					return;
				}
				this.rawPassword_ = value;
				this.key = PkzipClassic.GenerateKeys(ZipConstants.ConvertToArray(value));
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x000350D5 File Offset: 0x000332D5
		private bool HaveKeys
		{
			get
			{
				return this.key != null;
			}
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x000350E4 File Offset: 0x000332E4
		public ZipFile(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.name_ = name;
			this.baseStream_ = File.Open(name, FileMode.Open, FileAccess.Read, FileShare.Read);
			this.isStreamOwner = true;
			try
			{
				this.ReadEntries();
			}
			catch
			{
				this.DisposeInternal(true);
				throw;
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x00035164 File Offset: 0x00033364
		public ZipFile(FileStream file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			if (!file.CanSeek)
			{
				throw new ArgumentException("Stream is not seekable", "file");
			}
			this.baseStream_ = file;
			this.name_ = file.Name;
			this.isStreamOwner = true;
			try
			{
				this.ReadEntries();
			}
			catch
			{
				this.DisposeInternal(true);
				throw;
			}
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x000351F8 File Offset: 0x000333F8
		public ZipFile(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanSeek)
			{
				throw new ArgumentException("Stream is not seekable", "stream");
			}
			this.baseStream_ = stream;
			this.isStreamOwner = true;
			if (this.baseStream_.Length > 0L)
			{
				try
				{
					this.ReadEntries();
					return;
				}
				catch
				{
					this.DisposeInternal(true);
					throw;
				}
			}
			this.entries_ = new ZipEntry[0];
			this.isNewArchive_ = true;
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000352A0 File Offset: 0x000334A0
		internal ZipFile()
		{
			this.entries_ = new ZipEntry[0];
			this.isNewArchive_ = true;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x000352D8 File Offset: 0x000334D8
		~ZipFile()
		{
			this.Dispose(false);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00035308 File Offset: 0x00033508
		public void Close()
		{
			this.DisposeInternal(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00035318 File Offset: 0x00033518
		public static ZipFile Create(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			FileStream fileStream = File.Create(fileName);
			return new ZipFile
			{
				name_ = fileName,
				baseStream_ = fileStream,
				isStreamOwner = true
			};
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x00035358 File Offset: 0x00033558
		public static ZipFile Create(Stream outStream)
		{
			if (outStream == null)
			{
				throw new ArgumentNullException("outStream");
			}
			if (!outStream.CanWrite)
			{
				throw new ArgumentException("Stream is not writeable", "outStream");
			}
			if (!outStream.CanSeek)
			{
				throw new ArgumentException("Stream is not seekable", "outStream");
			}
			return new ZipFile
			{
				baseStream_ = outStream
			};
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x000353B1 File Offset: 0x000335B1
		// (set) Token: 0x060009C6 RID: 2502 RVA: 0x000353B9 File Offset: 0x000335B9
		public bool IsStreamOwner
		{
			get
			{
				return this.isStreamOwner;
			}
			set
			{
				this.isStreamOwner = value;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x000353C2 File Offset: 0x000335C2
		public bool IsEmbeddedArchive
		{
			get
			{
				return this.offsetOfFirstEntry > 0L;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x000353CE File Offset: 0x000335CE
		public bool IsNewArchive
		{
			get
			{
				return this.isNewArchive_;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x000353D6 File Offset: 0x000335D6
		public string ZipFileComment
		{
			get
			{
				return this.comment_;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x000353DE File Offset: 0x000335DE
		public string Name
		{
			get
			{
				return this.name_;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x000353E6 File Offset: 0x000335E6
		[Obsolete("Use the Count property instead")]
		public int Size
		{
			get
			{
				return this.entries_.Length;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x000353F0 File Offset: 0x000335F0
		public long Count
		{
			get
			{
				return (long)this.entries_.Length;
			}
		}

		// Token: 0x170002D4 RID: 724
		public ZipEntry this[int index]
		{
			get
			{
				return (ZipEntry)this.entries_[index].Clone();
			}
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0003540F File Offset: 0x0003360F
		public IEnumerator GetEnumerator()
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			return new ZipFile.ZipEntryEnumerator(this.entries_);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00035430 File Offset: 0x00033630
		public int FindEntry(string name, bool ignoreCase)
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			for (int i = 0; i < this.entries_.Length; i++)
			{
				if (string.Compare(name, this.entries_[i].Name, ignoreCase, CultureInfo.InvariantCulture) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00035484 File Offset: 0x00033684
		public ZipEntry GetEntry(string name)
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			int num = this.FindEntry(name, true);
			if (num < 0)
			{
				return null;
			}
			return (ZipEntry)this.entries_[num].Clone();
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x000354C8 File Offset: 0x000336C8
		public Stream GetInputStream(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			long num = entry.ZipFileIndex;
			if (num < 0L || num >= (long)this.entries_.Length || this.entries_[(int)(checked((IntPtr)num))].Name != entry.Name)
			{
				num = (long)this.FindEntry(entry.Name, true);
				if (num < 0L)
				{
					throw new ZipException("Entry cannot be found");
				}
			}
			return this.GetInputStream(num);
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x00035550 File Offset: 0x00033750
		public Stream GetInputStream(long entryIndex)
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			checked
			{
				long start = this.LocateEntry(this.entries_[(int)((IntPtr)entryIndex)]);
				CompressionMethod compressionMethod = this.entries_[(int)((IntPtr)entryIndex)].CompressionMethod;
				Stream stream = new ZipFile.PartialInputStream(this, start, this.entries_[(int)((IntPtr)entryIndex)].CompressedSize);
				if (this.entries_[(int)((IntPtr)entryIndex)].IsCrypted)
				{
					stream = this.CreateAndInitDecryptionStream(stream, this.entries_[(int)((IntPtr)entryIndex)]);
					if (stream == null)
					{
						throw new ZipException("Unable to decrypt this entry");
					}
				}
				CompressionMethod compressionMethod2 = compressionMethod;
				if (compressionMethod2 != CompressionMethod.Stored)
				{
					if (compressionMethod2 != CompressionMethod.Deflated)
					{
						throw new ZipException("Unsupported compression method " + compressionMethod);
					}
					stream = new InflaterInputStream(stream, new Inflater(true));
				}
				return stream;
			}
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x00035604 File Offset: 0x00033804
		public bool TestArchive(bool testData)
		{
			return this.TestArchive(testData, TestStrategy.FindFirstError, null);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00035610 File Offset: 0x00033810
		public bool TestArchive(bool testData, TestStrategy strategy, ZipTestResultHandler resultHandler)
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			TestStatus testStatus = new TestStatus(this);
			if (resultHandler != null)
			{
				resultHandler(testStatus, null);
			}
			ZipFile.HeaderTest tests = testData ? (ZipFile.HeaderTest.Extract | ZipFile.HeaderTest.Header) : ZipFile.HeaderTest.Header;
			bool flag = true;
			try
			{
				int num = 0;
				while (flag && (long)num < this.Count)
				{
					if (resultHandler != null)
					{
						testStatus.SetEntry(this[num]);
						testStatus.SetOperation(TestOperation.EntryHeader);
						resultHandler(testStatus, null);
					}
					try
					{
						this.TestLocalHeader(this[num], tests);
					}
					catch (ZipException ex)
					{
						testStatus.AddError();
						if (resultHandler != null)
						{
							resultHandler(testStatus, string.Format("Exception during test - '{0}'", ex.Message));
						}
						if (strategy == TestStrategy.FindFirstError)
						{
							flag = false;
						}
					}
					if (flag && testData && this[num].IsFile)
					{
						if (resultHandler != null)
						{
							testStatus.SetOperation(TestOperation.EntryData);
							resultHandler(testStatus, null);
						}
						Crc32 crc = new Crc32();
						using (Stream inputStream = this.GetInputStream(this[num]))
						{
							byte[] array = new byte[4096];
							long num2 = 0L;
							int num3;
							while ((num3 = inputStream.Read(array, 0, array.Length)) > 0)
							{
								crc.Update(array, 0, num3);
								if (resultHandler != null)
								{
									num2 += (long)num3;
									testStatus.SetBytesTested(num2);
									resultHandler(testStatus, null);
								}
							}
						}
						if (this[num].Crc != crc.Value)
						{
							testStatus.AddError();
							if (resultHandler != null)
							{
								resultHandler(testStatus, "CRC mismatch");
							}
							if (strategy == TestStrategy.FindFirstError)
							{
								flag = false;
							}
						}
						if ((this[num].Flags & 8) != 0)
						{
							ZipHelperStream zipHelperStream = new ZipHelperStream(this.baseStream_);
							DescriptorData descriptorData = new DescriptorData();
							zipHelperStream.ReadDataDescriptor(this[num].LocalHeaderRequiresZip64, descriptorData);
							if (this[num].Crc != descriptorData.Crc)
							{
								testStatus.AddError();
							}
							if (this[num].CompressedSize != descriptorData.CompressedSize)
							{
								testStatus.AddError();
							}
							if (this[num].Size != descriptorData.Size)
							{
								testStatus.AddError();
							}
						}
					}
					if (resultHandler != null)
					{
						testStatus.SetOperation(TestOperation.EntryComplete);
						resultHandler(testStatus, null);
					}
					num++;
				}
				if (resultHandler != null)
				{
					testStatus.SetOperation(TestOperation.MiscellaneousTests);
					resultHandler(testStatus, null);
				}
			}
			catch (Exception ex2)
			{
				testStatus.AddError();
				if (resultHandler != null)
				{
					resultHandler(testStatus, string.Format("Exception during test - '{0}'", ex2.Message));
				}
			}
			if (resultHandler != null)
			{
				testStatus.SetOperation(TestOperation.Complete);
				testStatus.SetEntry(null);
				resultHandler(testStatus, null);
			}
			return testStatus.ErrorCount == 0;
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x000358D4 File Offset: 0x00033AD4
		private long TestLocalHeader(ZipEntry entry, ZipFile.HeaderTest tests)
		{
			long result;
			lock (this.baseStream_)
			{
				bool flag2 = (tests & ZipFile.HeaderTest.Header) != (ZipFile.HeaderTest)0;
				bool flag3 = (tests & ZipFile.HeaderTest.Extract) != (ZipFile.HeaderTest)0;
				this.baseStream_.Seek(this.offsetOfFirstEntry + entry.Offset, SeekOrigin.Begin);
				if (this.ReadLEUint() != 67324752u)
				{
					throw new ZipException(string.Format("Wrong local header signature @{0:X}", this.offsetOfFirstEntry + entry.Offset));
				}
				short num = (short)this.ReadLEUshort();
				short num2 = (short)this.ReadLEUshort();
				short num3 = (short)this.ReadLEUshort();
				short num4 = (short)this.ReadLEUshort();
				short num5 = (short)this.ReadLEUshort();
				uint num6 = this.ReadLEUint();
				long num7 = (long)((ulong)this.ReadLEUint());
				long num8 = (long)((ulong)this.ReadLEUint());
				int num9 = (int)this.ReadLEUshort();
				int num10 = (int)this.ReadLEUshort();
				byte[] array = new byte[num9];
				StreamUtils.ReadFully(this.baseStream_, array);
				byte[] array2 = new byte[num10];
				StreamUtils.ReadFully(this.baseStream_, array2);
				ZipExtraData zipExtraData = new ZipExtraData(array2);
				if (zipExtraData.Find(1))
				{
					num8 = zipExtraData.ReadLong();
					num7 = zipExtraData.ReadLong();
					if ((num2 & 8) != 0)
					{
						if (num8 != -1L && num8 != entry.Size)
						{
							throw new ZipException("Size invalid for descriptor");
						}
						if (num7 != -1L && num7 != entry.CompressedSize)
						{
							throw new ZipException("Compressed size invalid for descriptor");
						}
					}
				}
				else if (num >= 45 && ((uint)num8 == 4294967295u || (uint)num7 == 4294967295u))
				{
					throw new ZipException("Required Zip64 extended information missing");
				}
				if (flag3 && entry.IsFile)
				{
					if (!entry.IsCompressionMethodSupported())
					{
						throw new ZipException("Compression method not supported");
					}
					if (num > 51 || (num > 20 && num < 45))
					{
						throw new ZipException(string.Format("Version required to extract this entry not supported ({0})", num));
					}
					if ((num2 & 12384) != 0)
					{
						throw new ZipException("The library does not support the zip version required to extract this entry");
					}
				}
				if (flag2)
				{
					if (num <= 63 && num != 10 && num != 11 && num != 20 && num != 21 && num != 25 && num != 27 && num != 45 && num != 46 && num != 50 && num != 51 && num != 52 && num != 61 && num != 62 && num != 63)
					{
						throw new ZipException(string.Format("Version required to extract this entry is invalid ({0})", num));
					}
					if (((int)num2 & 49168) != 0)
					{
						throw new ZipException("Reserved bit flags cannot be set.");
					}
					if ((num2 & 1) != 0 && num < 20)
					{
						throw new ZipException(string.Format("Version required to extract this entry is too low for encryption ({0})", num));
					}
					if ((num2 & 64) != 0)
					{
						if ((num2 & 1) == 0)
						{
							throw new ZipException("Strong encryption flag set but encryption flag is not set");
						}
						if (num < 50)
						{
							throw new ZipException(string.Format("Version required to extract this entry is too low for encryption ({0})", num));
						}
					}
					if ((num2 & 32) != 0 && num < 27)
					{
						throw new ZipException(string.Format("Patched data requires higher version than ({0})", num));
					}
					if ((int)num2 != entry.Flags)
					{
						throw new ZipException("Central header/local header flags mismatch");
					}
					if (entry.CompressionMethod != (CompressionMethod)num3)
					{
						throw new ZipException("Central header/local header compression method mismatch");
					}
					if (entry.Version != (int)num)
					{
						throw new ZipException("Extract version mismatch");
					}
					if ((num2 & 64) != 0 && num < 62)
					{
						throw new ZipException("Strong encryption flag set but version not high enough");
					}
					if ((num2 & 8192) != 0 && (num4 != 0 || num5 != 0))
					{
						throw new ZipException("Header masked set but date/time values non-zero");
					}
					if ((num2 & 8) == 0 && num6 != (uint)entry.Crc)
					{
						throw new ZipException("Central header/local header crc mismatch");
					}
					if (num8 == 0L && num7 == 0L && num6 != 0u)
					{
						throw new ZipException("Invalid CRC for empty entry");
					}
					if (entry.Name.Length > num9)
					{
						throw new ZipException("File name length mismatch");
					}
					string text = ZipConstants.ConvertToStringExt((int)num2, array);
					if (text != entry.Name)
					{
						throw new ZipException("Central header and local header file name mismatch");
					}
					if (entry.IsDirectory)
					{
						if (num8 > 0L)
						{
							throw new ZipException("Directory cannot have size");
						}
						if (entry.IsCrypted)
						{
							if (num7 > 14L)
							{
								throw new ZipException("Directory compressed size invalid");
							}
						}
						else if (num7 > 2L)
						{
							throw new ZipException("Directory compressed size invalid");
						}
					}
					if (!ZipNameTransform.IsValidName(text, true))
					{
						throw new ZipException("Name is invalid");
					}
				}
				if ((num2 & 8) == 0 || num8 > 0L || num7 > 0L)
				{
					if (num8 != entry.Size)
					{
						throw new ZipException(string.Format("Size mismatch between central header({0}) and local header({1})", entry.Size, num8));
					}
					if (num7 != entry.CompressedSize && num7 != (long)((ulong)-1) && num7 != -1L)
					{
						throw new ZipException(string.Format("Compressed size mismatch between central header({0}) and local header({1})", entry.CompressedSize, num7));
					}
				}
				int num11 = num9 + num10;
				result = this.offsetOfFirstEntry + entry.Offset + 30L + (long)num11;
			}
			return result;
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x00035D90 File Offset: 0x00033F90
		// (set) Token: 0x060009D7 RID: 2519 RVA: 0x00035D9D File Offset: 0x00033F9D
		public INameTransform NameTransform
		{
			get
			{
				return this.updateEntryFactory_.NameTransform;
			}
			set
			{
				this.updateEntryFactory_.NameTransform = value;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x00035DAB File Offset: 0x00033FAB
		// (set) Token: 0x060009D9 RID: 2521 RVA: 0x00035DB3 File Offset: 0x00033FB3
		public IEntryFactory EntryFactory
		{
			get
			{
				return this.updateEntryFactory_;
			}
			set
			{
				if (value == null)
				{
					this.updateEntryFactory_ = new ZipEntryFactory();
					return;
				}
				this.updateEntryFactory_ = value;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x00035DCB File Offset: 0x00033FCB
		// (set) Token: 0x060009DB RID: 2523 RVA: 0x00035DD3 File Offset: 0x00033FD3
		public int BufferSize
		{
			get
			{
				return this.bufferSize_;
			}
			set
			{
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("value", "cannot be below 1024");
				}
				if (this.bufferSize_ != value)
				{
					this.bufferSize_ = value;
					this.copyBuffer_ = null;
				}
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x00035E04 File Offset: 0x00034004
		public bool IsUpdating
		{
			get
			{
				return this.updates_ != null;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x00035E12 File Offset: 0x00034012
		// (set) Token: 0x060009DE RID: 2526 RVA: 0x00035E1A File Offset: 0x0003401A
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

		// Token: 0x060009DF RID: 2527 RVA: 0x00035E24 File Offset: 0x00034024
		public void BeginUpdate(IArchiveStorage archiveStorage, IDynamicDataSource dataSource)
		{
			if (archiveStorage == null)
			{
				throw new ArgumentNullException("archiveStorage");
			}
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			if (this.IsEmbeddedArchive)
			{
				throw new ZipException("Cannot update embedded/SFX archives");
			}
			this.archiveStorage_ = archiveStorage;
			this.updateDataSource_ = dataSource;
			this.updateIndex_ = new Hashtable();
			this.updates_ = new ArrayList(this.entries_.Length);
			ZipEntry[] array = this.entries_;
			for (int i = 0; i < array.Length; i++)
			{
				ZipEntry zipEntry = array[i];
				int num = this.updates_.Add(new ZipFile.ZipUpdate(zipEntry));
				this.updateIndex_.Add(zipEntry.Name, num);
			}
			this.updates_.Sort(new ZipFile.UpdateComparer());
			int num2 = 0;
			foreach (ZipFile.ZipUpdate zipUpdate in this.updates_)
			{
				if (num2 == this.updates_.Count - 1)
				{
					break;
				}
				zipUpdate.OffsetBasedSize = ((ZipFile.ZipUpdate)this.updates_[num2 + 1]).Entry.Offset - zipUpdate.Entry.Offset;
				num2++;
			}
			this.updateCount_ = (long)this.updates_.Count;
			this.contentsEdited_ = false;
			this.commentEdited_ = false;
			this.newComment_ = null;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00035FB4 File Offset: 0x000341B4
		public void BeginUpdate(IArchiveStorage archiveStorage)
		{
			this.BeginUpdate(archiveStorage, new DynamicDiskDataSource());
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00035FC2 File Offset: 0x000341C2
		public void BeginUpdate()
		{
			if (this.Name == null)
			{
				this.BeginUpdate(new MemoryArchiveStorage(), new DynamicDiskDataSource());
				return;
			}
			this.BeginUpdate(new DiskArchiveStorage(this), new DynamicDiskDataSource());
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00035FF0 File Offset: 0x000341F0
		public void CommitUpdate()
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			this.CheckUpdating();
			try
			{
				this.updateIndex_.Clear();
				this.updateIndex_ = null;
				if (this.contentsEdited_)
				{
					this.RunUpdates();
				}
				else if (this.commentEdited_)
				{
					this.UpdateCommentOnly();
				}
				else if (this.entries_.Length == 0)
				{
					byte[] comment = (this.newComment_ != null) ? this.newComment_.RawComment : ZipConstants.ConvertToArray(this.comment_);
					using (ZipHelperStream zipHelperStream = new ZipHelperStream(this.baseStream_))
					{
						zipHelperStream.WriteEndOfCentralDirectory(0L, 0L, 0L, comment);
					}
				}
			}
			finally
			{
				this.PostUpdateCleanup();
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x000360C0 File Offset: 0x000342C0
		public void AbortUpdate()
		{
			this.PostUpdateCleanup();
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x000360C8 File Offset: 0x000342C8
		public void SetComment(string comment)
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			this.CheckUpdating();
			this.newComment_ = new ZipFile.ZipString(comment);
			if (this.newComment_.RawLength > 65535)
			{
				this.newComment_ = null;
				throw new ZipException("Comment length exceeds maximum - 65535");
			}
			this.commentEdited_ = true;
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00036128 File Offset: 0x00034328
		private void AddUpdate(ZipFile.ZipUpdate update)
		{
			this.contentsEdited_ = true;
			int num = this.FindExistingUpdate(update.Entry.Name);
			if (num >= 0)
			{
				if (this.updates_[num] == null)
				{
					this.updateCount_ += 1L;
				}
				this.updates_[num] = update;
				return;
			}
			num = this.updates_.Add(update);
			this.updateCount_ += 1L;
			this.updateIndex_.Add(update.Entry.Name, num);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x000361B8 File Offset: 0x000343B8
		public void Add(string fileName, CompressionMethod compressionMethod, bool useUnicodeText)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			if (!ZipEntry.IsCompressionMethodSupported(compressionMethod))
			{
				throw new ArgumentOutOfRangeException("compressionMethod");
			}
			this.CheckUpdating();
			this.contentsEdited_ = true;
			ZipEntry zipEntry = this.EntryFactory.MakeFileEntry(fileName);
			zipEntry.IsUnicodeText = useUnicodeText;
			zipEntry.CompressionMethod = compressionMethod;
			this.AddUpdate(new ZipFile.ZipUpdate(fileName, zipEntry));
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00036230 File Offset: 0x00034430
		public void Add(string fileName, CompressionMethod compressionMethod)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (!ZipEntry.IsCompressionMethodSupported(compressionMethod))
			{
				throw new ArgumentOutOfRangeException("compressionMethod");
			}
			this.CheckUpdating();
			this.contentsEdited_ = true;
			ZipEntry zipEntry = this.EntryFactory.MakeFileEntry(fileName);
			zipEntry.CompressionMethod = compressionMethod;
			this.AddUpdate(new ZipFile.ZipUpdate(fileName, zipEntry));
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0003628C File Offset: 0x0003448C
		public void Add(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			this.CheckUpdating();
			this.AddUpdate(new ZipFile.ZipUpdate(fileName, this.EntryFactory.MakeFileEntry(fileName)));
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x000362BA File Offset: 0x000344BA
		public void Add(string fileName, string entryName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (entryName == null)
			{
				throw new ArgumentNullException("entryName");
			}
			this.CheckUpdating();
			this.AddUpdate(new ZipFile.ZipUpdate(fileName, this.EntryFactory.MakeFileEntry(entryName)));
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x000362F6 File Offset: 0x000344F6
		public void Add(IStaticDataSource dataSource, string entryName)
		{
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			if (entryName == null)
			{
				throw new ArgumentNullException("entryName");
			}
			this.CheckUpdating();
			this.AddUpdate(new ZipFile.ZipUpdate(dataSource, this.EntryFactory.MakeFileEntry(entryName, false)));
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00036334 File Offset: 0x00034534
		public void Add(IStaticDataSource dataSource, string entryName, CompressionMethod compressionMethod)
		{
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			if (entryName == null)
			{
				throw new ArgumentNullException("entryName");
			}
			this.CheckUpdating();
			ZipEntry zipEntry = this.EntryFactory.MakeFileEntry(entryName, false);
			zipEntry.CompressionMethod = compressionMethod;
			this.AddUpdate(new ZipFile.ZipUpdate(dataSource, zipEntry));
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00036388 File Offset: 0x00034588
		public void Add(IStaticDataSource dataSource, string entryName, CompressionMethod compressionMethod, bool useUnicodeText)
		{
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			if (entryName == null)
			{
				throw new ArgumentNullException("entryName");
			}
			this.CheckUpdating();
			ZipEntry zipEntry = this.EntryFactory.MakeFileEntry(entryName, false);
			zipEntry.IsUnicodeText = useUnicodeText;
			zipEntry.CompressionMethod = compressionMethod;
			this.AddUpdate(new ZipFile.ZipUpdate(dataSource, zipEntry));
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x000363E4 File Offset: 0x000345E4
		public void Add(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.CheckUpdating();
			if (entry.Size != 0L || entry.CompressedSize != 0L)
			{
				throw new ZipException("Entry cannot have any data");
			}
			this.AddUpdate(new ZipFile.ZipUpdate(ZipFile.UpdateCommand.Add, entry));
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00036434 File Offset: 0x00034634
		public void AddDirectory(string directoryName)
		{
			if (directoryName == null)
			{
				throw new ArgumentNullException("directoryName");
			}
			this.CheckUpdating();
			ZipEntry entry = this.EntryFactory.MakeDirectoryEntry(directoryName);
			this.AddUpdate(new ZipFile.ZipUpdate(ZipFile.UpdateCommand.Add, entry));
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00036470 File Offset: 0x00034670
		public bool Delete(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			this.CheckUpdating();
			int num = this.FindExistingUpdate(fileName);
			if (num >= 0 && this.updates_[num] != null)
			{
				bool result = true;
				this.contentsEdited_ = true;
				this.updates_[num] = null;
				this.updateCount_ -= 1L;
				return result;
			}
			throw new ZipException("Cannot find entry to delete");
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x000364E0 File Offset: 0x000346E0
		public void Delete(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.CheckUpdating();
			int num = this.FindExistingUpdate(entry);
			if (num >= 0)
			{
				this.contentsEdited_ = true;
				this.updates_[num] = null;
				this.updateCount_ -= 1L;
				return;
			}
			throw new ZipException("Cannot find entry to delete");
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0003653B File Offset: 0x0003473B
		private void WriteLEShort(int value)
		{
			this.baseStream_.WriteByte((byte)(value & 255));
			this.baseStream_.WriteByte((byte)(value >> 8 & 255));
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00036565 File Offset: 0x00034765
		private void WriteLEUshort(ushort value)
		{
			this.baseStream_.WriteByte((byte)(value & 255));
			this.baseStream_.WriteByte((byte)(value >> 8));
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00036589 File Offset: 0x00034789
		private void WriteLEInt(int value)
		{
			this.WriteLEShort(value & 65535);
			this.WriteLEShort(value >> 16);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x000365A2 File Offset: 0x000347A2
		private void WriteLEUint(uint value)
		{
			this.WriteLEUshort((ushort)(value & 65535u));
			this.WriteLEUshort((ushort)(value >> 16));
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x000365BD File Offset: 0x000347BD
		private void WriteLeLong(long value)
		{
			this.WriteLEInt((int)(value & (long)((ulong)-1)));
			this.WriteLEInt((int)(value >> 32));
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x000365D5 File Offset: 0x000347D5
		private void WriteLEUlong(ulong value)
		{
			this.WriteLEUint((uint)(value & (ulong)-1));
			this.WriteLEUint((uint)(value >> 32));
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x000365F0 File Offset: 0x000347F0
		private void WriteLocalEntryHeader(ZipFile.ZipUpdate update)
		{
			ZipEntry outEntry = update.OutEntry;
			outEntry.Offset = this.baseStream_.Position;
			if (update.Command != ZipFile.UpdateCommand.Copy)
			{
				if (outEntry.CompressionMethod == CompressionMethod.Deflated)
				{
					if (outEntry.Size == 0L)
					{
						outEntry.CompressedSize = outEntry.Size;
						outEntry.Crc = 0L;
						outEntry.CompressionMethod = CompressionMethod.Stored;
					}
				}
				else if (outEntry.CompressionMethod == CompressionMethod.Stored)
				{
					outEntry.Flags &= -9;
				}
				if (this.HaveKeys)
				{
					outEntry.IsCrypted = true;
					if (outEntry.Crc < 0L)
					{
						outEntry.Flags |= 8;
					}
				}
				else
				{
					outEntry.IsCrypted = false;
				}
				switch (this.useZip64_)
				{
				case UseZip64.On:
					outEntry.ForceZip64();
					break;
				case UseZip64.Dynamic:
					if (outEntry.Size < 0L)
					{
						outEntry.ForceZip64();
					}
					break;
				}
			}
			this.WriteLEInt(67324752);
			this.WriteLEShort(outEntry.Version);
			this.WriteLEShort(outEntry.Flags);
			this.WriteLEShort((int)((byte)outEntry.CompressionMethod));
			this.WriteLEInt((int)outEntry.DosTime);
			if (!outEntry.HasCrc)
			{
				update.CrcPatchOffset = this.baseStream_.Position;
				this.WriteLEInt(0);
			}
			else
			{
				this.WriteLEInt((int)outEntry.Crc);
			}
			if (outEntry.LocalHeaderRequiresZip64)
			{
				this.WriteLEInt(-1);
				this.WriteLEInt(-1);
			}
			else
			{
				if (outEntry.CompressedSize < 0L || outEntry.Size < 0L)
				{
					update.SizePatchOffset = this.baseStream_.Position;
				}
				this.WriteLEInt((int)outEntry.CompressedSize);
				this.WriteLEInt((int)outEntry.Size);
			}
			byte[] array = ZipConstants.ConvertToArray(outEntry.Flags, outEntry.Name);
			if (array.Length > 65535)
			{
				throw new ZipException("Entry name too long.");
			}
			ZipExtraData zipExtraData = new ZipExtraData(outEntry.ExtraData);
			if (outEntry.LocalHeaderRequiresZip64)
			{
				zipExtraData.StartNewEntry();
				zipExtraData.AddLeLong(outEntry.Size);
				zipExtraData.AddLeLong(outEntry.CompressedSize);
				zipExtraData.AddNewEntry(1);
			}
			else
			{
				zipExtraData.Delete(1);
			}
			outEntry.ExtraData = zipExtraData.GetEntryData();
			this.WriteLEShort(array.Length);
			this.WriteLEShort(outEntry.ExtraData.Length);
			if (array.Length > 0)
			{
				this.baseStream_.Write(array, 0, array.Length);
			}
			if (outEntry.LocalHeaderRequiresZip64)
			{
				if (!zipExtraData.Find(1))
				{
					throw new ZipException("Internal error cannot find extra data");
				}
				update.SizePatchOffset = this.baseStream_.Position + (long)zipExtraData.CurrentReadIndex;
			}
			if (outEntry.ExtraData.Length > 0)
			{
				this.baseStream_.Write(outEntry.ExtraData, 0, outEntry.ExtraData.Length);
			}
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0003688C File Offset: 0x00034A8C
		private int WriteCentralDirectoryHeader(ZipEntry entry)
		{
			if (entry.CompressedSize < 0L)
			{
				throw new ZipException("Attempt to write central directory entry with unknown csize");
			}
			if (entry.Size < 0L)
			{
				throw new ZipException("Attempt to write central directory entry with unknown size");
			}
			if (entry.Crc < 0L)
			{
				throw new ZipException("Attempt to write central directory entry with unknown crc");
			}
			this.WriteLEInt(33639248);
			this.WriteLEShort(51);
			this.WriteLEShort(entry.Version);
			this.WriteLEShort(entry.Flags);
			this.WriteLEShort((int)((byte)entry.CompressionMethod));
			this.WriteLEInt((int)entry.DosTime);
			this.WriteLEInt((int)entry.Crc);
			if (entry.IsZip64Forced() || entry.CompressedSize >= (long)((ulong)-1))
			{
				this.WriteLEInt(-1);
			}
			else
			{
				this.WriteLEInt((int)(entry.CompressedSize & (long)((ulong)-1)));
			}
			if (entry.IsZip64Forced() || entry.Size >= (long)((ulong)-1))
			{
				this.WriteLEInt(-1);
			}
			else
			{
				this.WriteLEInt((int)entry.Size);
			}
			byte[] array = ZipConstants.ConvertToArray(entry.Flags, entry.Name);
			if (array.Length > 65535)
			{
				throw new ZipException("Entry name is too long.");
			}
			this.WriteLEShort(array.Length);
			ZipExtraData zipExtraData = new ZipExtraData(entry.ExtraData);
			if (entry.CentralHeaderRequiresZip64)
			{
				zipExtraData.StartNewEntry();
				if (entry.Size >= (long)((ulong)-1) || this.useZip64_ == UseZip64.On)
				{
					zipExtraData.AddLeLong(entry.Size);
				}
				if (entry.CompressedSize >= (long)((ulong)-1) || this.useZip64_ == UseZip64.On)
				{
					zipExtraData.AddLeLong(entry.CompressedSize);
				}
				if (entry.Offset >= (long)((ulong)-1))
				{
					zipExtraData.AddLeLong(entry.Offset);
				}
				zipExtraData.AddNewEntry(1);
			}
			else
			{
				zipExtraData.Delete(1);
			}
			byte[] entryData = zipExtraData.GetEntryData();
			this.WriteLEShort(entryData.Length);
			this.WriteLEShort((entry.Comment != null) ? entry.Comment.Length : 0);
			this.WriteLEShort(0);
			this.WriteLEShort(0);
			if (entry.ExternalFileAttributes != -1)
			{
				this.WriteLEInt(entry.ExternalFileAttributes);
			}
			else if (entry.IsDirectory)
			{
				this.WriteLEUint(16u);
			}
			else
			{
				this.WriteLEUint(0u);
			}
			if (entry.Offset >= (long)((ulong)-1))
			{
				this.WriteLEUint(4294967295u);
			}
			else
			{
				this.WriteLEUint((uint)((int)entry.Offset));
			}
			if (array.Length > 0)
			{
				this.baseStream_.Write(array, 0, array.Length);
			}
			if (entryData.Length > 0)
			{
				this.baseStream_.Write(entryData, 0, entryData.Length);
			}
			byte[] array2 = (entry.Comment != null) ? Encoding.ASCII.GetBytes(entry.Comment) : new byte[0];
			if (array2.Length > 0)
			{
				this.baseStream_.Write(array2, 0, array2.Length);
			}
			return 46 + array.Length + entryData.Length + array2.Length;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00036B27 File Offset: 0x00034D27
		private void PostUpdateCleanup()
		{
			this.updateDataSource_ = null;
			this.updates_ = null;
			this.updateIndex_ = null;
			if (this.archiveStorage_ != null)
			{
				this.archiveStorage_.Dispose();
				this.archiveStorage_ = null;
			}
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00036B58 File Offset: 0x00034D58
		private string GetTransformedFileName(string name)
		{
			INameTransform nameTransform = this.NameTransform;
			if (nameTransform == null)
			{
				return name;
			}
			return nameTransform.TransformFile(name);
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00036B78 File Offset: 0x00034D78
		private string GetTransformedDirectoryName(string name)
		{
			INameTransform nameTransform = this.NameTransform;
			if (nameTransform == null)
			{
				return name;
			}
			return nameTransform.TransformDirectory(name);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00036B98 File Offset: 0x00034D98
		private byte[] GetBuffer()
		{
			if (this.copyBuffer_ == null)
			{
				this.copyBuffer_ = new byte[this.bufferSize_];
			}
			return this.copyBuffer_;
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00036BBC File Offset: 0x00034DBC
		private void CopyDescriptorBytes(ZipFile.ZipUpdate update, Stream dest, Stream source)
		{
			int i = this.GetDescriptorSize(update);
			if (i > 0)
			{
				byte[] buffer = this.GetBuffer();
				while (i > 0)
				{
					int count = Math.Min(buffer.Length, i);
					int num = source.Read(buffer, 0, count);
					if (num <= 0)
					{
						throw new ZipException("Unxpected end of stream");
					}
					dest.Write(buffer, 0, num);
					i -= num;
				}
			}
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00036C14 File Offset: 0x00034E14
		private void CopyBytes(ZipFile.ZipUpdate update, Stream destination, Stream source, long bytesToCopy, bool updateCrc)
		{
			if (destination == source)
			{
				throw new InvalidOperationException("Destination and source are the same");
			}
			Crc32 crc = new Crc32();
			byte[] buffer = this.GetBuffer();
			long num = bytesToCopy;
			long num2 = 0L;
			int num4;
			do
			{
				int num3 = buffer.Length;
				if (bytesToCopy < (long)num3)
				{
					num3 = (int)bytesToCopy;
				}
				num4 = source.Read(buffer, 0, num3);
				if (num4 > 0)
				{
					if (updateCrc)
					{
						crc.Update(buffer, 0, num4);
					}
					destination.Write(buffer, 0, num4);
					bytesToCopy -= (long)num4;
					num2 += (long)num4;
				}
			}
			while (num4 > 0 && bytesToCopy > 0L);
			if (num2 != num)
			{
				throw new ZipException(string.Format("Failed to copy bytes expected {0} read {1}", num, num2));
			}
			if (updateCrc)
			{
				update.OutEntry.Crc = crc.Value;
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00036CCC File Offset: 0x00034ECC
		private int GetDescriptorSize(ZipFile.ZipUpdate update)
		{
			int result = 0;
			if ((update.Entry.Flags & 8) != 0)
			{
				result = 12;
				if (update.Entry.LocalHeaderRequiresZip64)
				{
					result = 20;
				}
			}
			return result;
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00036D00 File Offset: 0x00034F00
		private void CopyDescriptorBytesDirect(ZipFile.ZipUpdate update, Stream stream, ref long destinationPosition, long sourcePosition)
		{
			int i = this.GetDescriptorSize(update);
			while (i > 0)
			{
				int count = i;
				byte[] buffer = this.GetBuffer();
				stream.Position = sourcePosition;
				int num = stream.Read(buffer, 0, count);
				if (num <= 0)
				{
					throw new ZipException("Unxpected end of stream");
				}
				stream.Position = destinationPosition;
				stream.Write(buffer, 0, num);
				i -= num;
				destinationPosition += (long)num;
				sourcePosition += (long)num;
			}
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00036D6C File Offset: 0x00034F6C
		private void CopyEntryDataDirect(ZipFile.ZipUpdate update, Stream stream, bool updateCrc, ref long destinationPosition, ref long sourcePosition)
		{
			long num = update.Entry.CompressedSize;
			Crc32 crc = new Crc32();
			byte[] buffer = this.GetBuffer();
			long num2 = num;
			long num3 = 0L;
			int num5;
			do
			{
				int num4 = buffer.Length;
				if (num < (long)num4)
				{
					num4 = (int)num;
				}
				stream.Position = sourcePosition;
				num5 = stream.Read(buffer, 0, num4);
				if (num5 > 0)
				{
					if (updateCrc)
					{
						crc.Update(buffer, 0, num5);
					}
					stream.Position = destinationPosition;
					stream.Write(buffer, 0, num5);
					destinationPosition += (long)num5;
					sourcePosition += (long)num5;
					num -= (long)num5;
					num3 += (long)num5;
				}
			}
			while (num5 > 0 && num > 0L);
			if (num3 != num2)
			{
				throw new ZipException(string.Format("Failed to copy bytes expected {0} read {1}", num2, num3));
			}
			if (updateCrc)
			{
				update.OutEntry.Crc = crc.Value;
			}
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00036E44 File Offset: 0x00035044
		private int FindExistingUpdate(ZipEntry entry)
		{
			int result = -1;
			string transformedFileName = this.GetTransformedFileName(entry.Name);
			if (this.updateIndex_.ContainsKey(transformedFileName))
			{
				result = (int)this.updateIndex_[transformedFileName];
			}
			return result;
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00036E84 File Offset: 0x00035084
		private int FindExistingUpdate(string fileName)
		{
			int result = -1;
			string transformedFileName = this.GetTransformedFileName(fileName);
			if (this.updateIndex_.ContainsKey(transformedFileName))
			{
				result = (int)this.updateIndex_[transformedFileName];
			}
			return result;
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00036EBC File Offset: 0x000350BC
		private Stream GetOutputStream(ZipEntry entry)
		{
			Stream stream = this.baseStream_;
			if (entry.IsCrypted)
			{
				stream = this.CreateAndInitEncryptionStream(stream, entry);
			}
			CompressionMethod compressionMethod = entry.CompressionMethod;
			if (compressionMethod != CompressionMethod.Stored)
			{
				if (compressionMethod != CompressionMethod.Deflated)
				{
					throw new ZipException("Unknown compression method " + entry.CompressionMethod);
				}
				stream = new DeflaterOutputStream(stream, new Deflater(9, true))
				{
					IsStreamOwner = false
				};
			}
			else
			{
				stream = new ZipFile.UncompressedStream(stream);
			}
			return stream;
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x00036F34 File Offset: 0x00035134
		private void AddEntry(ZipFile workFile, ZipFile.ZipUpdate update)
		{
			Stream stream = null;
			if (update.Entry.IsFile)
			{
				stream = update.GetSource();
				if (stream == null)
				{
					stream = this.updateDataSource_.GetSource(update.Entry, update.Filename);
				}
			}
			if (stream != null)
			{
				using (stream)
				{
					long length = stream.Length;
					if (update.OutEntry.Size < 0L)
					{
						update.OutEntry.Size = length;
					}
					else if (update.OutEntry.Size != length)
					{
						throw new ZipException("Entry size/stream size mismatch");
					}
					workFile.WriteLocalEntryHeader(update);
					long position = workFile.baseStream_.Position;
					using (Stream outputStream = workFile.GetOutputStream(update.OutEntry))
					{
						this.CopyBytes(update, outputStream, stream, length, true);
					}
					long position2 = workFile.baseStream_.Position;
					update.OutEntry.CompressedSize = position2 - position;
					if ((update.OutEntry.Flags & 8) == 8)
					{
						ZipHelperStream zipHelperStream = new ZipHelperStream(workFile.baseStream_);
						zipHelperStream.WriteDataDescriptor(update.OutEntry);
					}
					return;
				}
			}
			workFile.WriteLocalEntryHeader(update);
			update.OutEntry.CompressedSize = 0L;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00037074 File Offset: 0x00035274
		private void ModifyEntry(ZipFile workFile, ZipFile.ZipUpdate update)
		{
			workFile.WriteLocalEntryHeader(update);
			long position = workFile.baseStream_.Position;
			if (update.Entry.IsFile && update.Filename != null)
			{
				using (Stream outputStream = workFile.GetOutputStream(update.OutEntry))
				{
					using (Stream inputStream = this.GetInputStream(update.Entry))
					{
						this.CopyBytes(update, outputStream, inputStream, inputStream.Length, true);
					}
				}
			}
			long position2 = workFile.baseStream_.Position;
			update.Entry.CompressedSize = position2 - position;
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00037124 File Offset: 0x00035324
		private void CopyEntryDirect(ZipFile workFile, ZipFile.ZipUpdate update, ref long destinationPosition)
		{
			bool flag = false;
			if (update.Entry.Offset == destinationPosition)
			{
				flag = true;
			}
			if (!flag)
			{
				this.baseStream_.Position = destinationPosition;
				workFile.WriteLocalEntryHeader(update);
				destinationPosition = this.baseStream_.Position;
			}
			long num = 0L;
			long num2 = update.Entry.Offset + 26L;
			this.baseStream_.Seek(num2, SeekOrigin.Begin);
			uint num3 = (uint)this.ReadLEUshort();
			uint num4 = (uint)this.ReadLEUshort();
			num = this.baseStream_.Position + (long)((ulong)num3) + (long)((ulong)num4);
			if (!flag)
			{
				if (update.Entry.CompressedSize > 0L)
				{
					this.CopyEntryDataDirect(update, this.baseStream_, false, ref destinationPosition, ref num);
				}
				this.CopyDescriptorBytesDirect(update, this.baseStream_, ref destinationPosition, num);
				return;
			}
			if (update.OffsetBasedSize != -1L)
			{
				destinationPosition += update.OffsetBasedSize;
				return;
			}
			destinationPosition += num - num2 + 26L + update.Entry.CompressedSize + (long)this.GetDescriptorSize(update);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00037218 File Offset: 0x00035418
		private void CopyEntry(ZipFile workFile, ZipFile.ZipUpdate update)
		{
			workFile.WriteLocalEntryHeader(update);
			if (update.Entry.CompressedSize > 0L)
			{
				long offset = update.Entry.Offset + 26L;
				this.baseStream_.Seek(offset, SeekOrigin.Begin);
				uint num = (uint)this.ReadLEUshort();
				uint num2 = (uint)this.ReadLEUshort();
				this.baseStream_.Seek((long)((ulong)(num + num2)), SeekOrigin.Current);
				this.CopyBytes(update, workFile.baseStream_, this.baseStream_, update.Entry.CompressedSize, false);
			}
			this.CopyDescriptorBytes(update, workFile.baseStream_, this.baseStream_);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x000372AA File Offset: 0x000354AA
		private void Reopen(Stream source)
		{
			if (source == null)
			{
				throw new ZipException("Failed to reopen archive - no source");
			}
			this.isNewArchive_ = false;
			this.baseStream_ = source;
			this.ReadEntries();
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x000372CE File Offset: 0x000354CE
		private void Reopen()
		{
			if (this.Name == null)
			{
				throw new InvalidOperationException("Name is not known cannot Reopen");
			}
			this.Reopen(File.Open(this.Name, FileMode.Open, FileAccess.Read, FileShare.Read));
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x000372F8 File Offset: 0x000354F8
		private void UpdateCommentOnly()
		{
			long length = this.baseStream_.Length;
			ZipHelperStream zipHelperStream;
			if (this.archiveStorage_.UpdateMode == FileUpdateMode.Safe)
			{
				Stream stream = this.archiveStorage_.MakeTemporaryCopy(this.baseStream_);
				zipHelperStream = new ZipHelperStream(stream);
				zipHelperStream.IsStreamOwner = true;
				this.baseStream_.Close();
				this.baseStream_ = null;
			}
			else if (this.archiveStorage_.UpdateMode == FileUpdateMode.Direct)
			{
				this.baseStream_ = this.archiveStorage_.OpenForDirectUpdate(this.baseStream_);
				zipHelperStream = new ZipHelperStream(this.baseStream_);
			}
			else
			{
				this.baseStream_.Close();
				this.baseStream_ = null;
				zipHelperStream = new ZipHelperStream(this.Name);
			}
			using (zipHelperStream)
			{
				long num = zipHelperStream.LocateBlockWithSignature(101010256, length, 22, 65535);
				if (num < 0L)
				{
					throw new ZipException("Cannot find central directory");
				}
				zipHelperStream.Position += 16L;
				byte[] rawComment = this.newComment_.RawComment;
				zipHelperStream.WriteLEShort(rawComment.Length);
				zipHelperStream.Write(rawComment, 0, rawComment.Length);
				zipHelperStream.SetLength(zipHelperStream.Position);
			}
			if (this.archiveStorage_.UpdateMode == FileUpdateMode.Safe)
			{
				this.Reopen(this.archiveStorage_.ConvertTemporaryToFinal());
				return;
			}
			this.ReadEntries();
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00037450 File Offset: 0x00035650
		private void RunUpdates()
		{
			long num = 0L;
			long length = 0L;
			bool flag = false;
			long position = 0L;
			ZipFile zipFile;
			if (this.IsNewArchive)
			{
				zipFile = this;
				zipFile.baseStream_.Position = 0L;
				flag = true;
			}
			else if (this.archiveStorage_.UpdateMode == FileUpdateMode.Direct)
			{
				zipFile = this;
				zipFile.baseStream_.Position = 0L;
				flag = true;
				this.updates_.Sort(new ZipFile.UpdateComparer());
			}
			else
			{
				zipFile = ZipFile.Create(this.archiveStorage_.GetTemporaryOutput());
				zipFile.UseZip64 = this.UseZip64;
				if (this.key != null)
				{
					zipFile.key = (byte[])this.key.Clone();
				}
			}
			try
			{
				foreach (ZipFile.ZipUpdate zipUpdate in this.updates_)
				{
					if (zipUpdate != null)
					{
						switch (zipUpdate.Command)
						{
						case ZipFile.UpdateCommand.Copy:
							if (flag)
							{
								this.CopyEntryDirect(zipFile, zipUpdate, ref position);
							}
							else
							{
								this.CopyEntry(zipFile, zipUpdate);
							}
							break;
						case ZipFile.UpdateCommand.Modify:
							this.ModifyEntry(zipFile, zipUpdate);
							break;
						case ZipFile.UpdateCommand.Add:
							if (!this.IsNewArchive && flag)
							{
								zipFile.baseStream_.Position = position;
							}
							this.AddEntry(zipFile, zipUpdate);
							if (flag)
							{
								position = zipFile.baseStream_.Position;
							}
							break;
						}
					}
				}
				if (!this.IsNewArchive && flag)
				{
					zipFile.baseStream_.Position = position;
				}
				long position2 = zipFile.baseStream_.Position;
				foreach (ZipFile.ZipUpdate zipUpdate2 in this.updates_)
				{
					if (zipUpdate2 != null)
					{
						num += (long)zipFile.WriteCentralDirectoryHeader(zipUpdate2.OutEntry);
					}
				}
				byte[] comment = (this.newComment_ != null) ? this.newComment_.RawComment : ZipConstants.ConvertToArray(this.comment_);
				using (ZipHelperStream zipHelperStream = new ZipHelperStream(zipFile.baseStream_))
				{
					zipHelperStream.WriteEndOfCentralDirectory(this.updateCount_, num, position2, comment);
				}
				length = zipFile.baseStream_.Position;
				foreach (ZipFile.ZipUpdate zipUpdate3 in this.updates_)
				{
					if (zipUpdate3 != null)
					{
						if (zipUpdate3.CrcPatchOffset > 0L && zipUpdate3.OutEntry.CompressedSize > 0L)
						{
							zipFile.baseStream_.Position = zipUpdate3.CrcPatchOffset;
							zipFile.WriteLEInt((int)zipUpdate3.OutEntry.Crc);
						}
						if (zipUpdate3.SizePatchOffset > 0L)
						{
							zipFile.baseStream_.Position = zipUpdate3.SizePatchOffset;
							if (zipUpdate3.OutEntry.LocalHeaderRequiresZip64)
							{
								zipFile.WriteLeLong(zipUpdate3.OutEntry.Size);
								zipFile.WriteLeLong(zipUpdate3.OutEntry.CompressedSize);
							}
							else
							{
								zipFile.WriteLEInt((int)zipUpdate3.OutEntry.CompressedSize);
								zipFile.WriteLEInt((int)zipUpdate3.OutEntry.Size);
							}
						}
					}
				}
			}
			catch
			{
				zipFile.Close();
				if (!flag && zipFile.Name != null)
				{
					File.Delete(zipFile.Name);
				}
				throw;
			}
			if (flag)
			{
				zipFile.baseStream_.SetLength(length);
				zipFile.baseStream_.Flush();
				this.isNewArchive_ = false;
				this.ReadEntries();
				return;
			}
			this.baseStream_.Close();
			this.Reopen(this.archiveStorage_.ConvertTemporaryToFinal());
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0003787C File Offset: 0x00035A7C
		private void CheckUpdating()
		{
			if (this.updates_ == null)
			{
				throw new InvalidOperationException("BeginUpdate has not been called");
			}
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00037891 File Offset: 0x00035A91
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0003789C File Offset: 0x00035A9C
		private void DisposeInternal(bool disposing)
		{
			if (!this.isDisposed_)
			{
				this.isDisposed_ = true;
				this.entries_ = new ZipEntry[0];
				if (this.IsStreamOwner && this.baseStream_ != null)
				{
					lock (this.baseStream_)
					{
						this.baseStream_.Close();
					}
				}
				this.PostUpdateCleanup();
			}
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00037914 File Offset: 0x00035B14
		protected virtual void Dispose(bool disposing)
		{
			this.DisposeInternal(disposing);
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x00037920 File Offset: 0x00035B20
		private ushort ReadLEUshort()
		{
			int num = this.baseStream_.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException("End of stream");
			}
			int num2 = this.baseStream_.ReadByte();
			if (num2 < 0)
			{
				throw new EndOfStreamException("End of stream");
			}
			return (ushort)num | (ushort)(num2 << 8);
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0003796B File Offset: 0x00035B6B
		private uint ReadLEUint()
		{
			return (uint)((int)this.ReadLEUshort() | (int)this.ReadLEUshort() << 16);
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0003797D File Offset: 0x00035B7D
		private ulong ReadLEUlong()
		{
			return (ulong)this.ReadLEUint() | (ulong)this.ReadLEUint() << 32;
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x00037994 File Offset: 0x00035B94
		private long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize, int maximumVariableData)
		{
			long result;
			using (ZipHelperStream zipHelperStream = new ZipHelperStream(this.baseStream_))
			{
				result = zipHelperStream.LocateBlockWithSignature(signature, endLocation, minimumBlockSize, maximumVariableData);
			}
			return result;
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x000379D8 File Offset: 0x00035BD8
		private void ReadEntries()
		{
			if (!this.baseStream_.CanSeek)
			{
				throw new ZipException("ZipFile stream must be seekable");
			}
			long num = this.LocateBlockWithSignature(101010256, this.baseStream_.Length, 22, 65535);
			if (num < 0L)
			{
				throw new ZipException("Cannot find central directory");
			}
			ushort num2 = this.ReadLEUshort();
			ushort num3 = this.ReadLEUshort();
			ulong num4 = (ulong)this.ReadLEUshort();
			ulong num5 = (ulong)this.ReadLEUshort();
			ulong num6 = (ulong)this.ReadLEUint();
			long num7 = (long)((ulong)this.ReadLEUint());
			uint num8 = (uint)this.ReadLEUshort();
			if (num8 > 0u)
			{
				byte[] array = new byte[num8];
				StreamUtils.ReadFully(this.baseStream_, array);
				this.comment_ = ZipConstants.ConvertToString(array);
			}
			else
			{
				this.comment_ = string.Empty;
			}
			bool flag = false;
			if (num2 == 65535 || num3 == 65535 || num4 == 65535uL || num5 == 65535uL || num6 == (ulong)-1 || num7 == (long)((ulong)-1))
			{
				flag = true;
				long num9 = this.LocateBlockWithSignature(117853008, num, 0, 4096);
				if (num9 < 0L)
				{
					throw new ZipException("Cannot find Zip64 locator");
				}
				this.ReadLEUint();
				ulong num10 = this.ReadLEUlong();
				this.ReadLEUint();
				this.baseStream_.Position = (long)num10;
				long num11 = (long)((ulong)this.ReadLEUint());
				if (num11 != 101075792L)
				{
					throw new ZipException(string.Format("Invalid Zip64 Central directory signature at {0:X}", num10));
				}
				this.ReadLEUlong();
				this.ReadLEUshort();
				this.ReadLEUshort();
				this.ReadLEUint();
				this.ReadLEUint();
				num4 = this.ReadLEUlong();
				num5 = this.ReadLEUlong();
				num6 = this.ReadLEUlong();
				num7 = (long)this.ReadLEUlong();
			}
			this.entries_ = new ZipEntry[num4];
			if (!flag && num7 < num - (long)(4uL + num6))
			{
				this.offsetOfFirstEntry = num - (long)(4uL + num6 + (ulong)num7);
				if (this.offsetOfFirstEntry <= 0L)
				{
					throw new ZipException("Invalid embedded zip archive");
				}
			}
			this.baseStream_.Seek(this.offsetOfFirstEntry + num7, SeekOrigin.Begin);
			for (ulong num12 = 0uL; num12 < num4; num12 += 1uL)
			{
				if (this.ReadLEUint() != 33639248u)
				{
					throw new ZipException("Wrong Central Directory signature");
				}
				int madeByInfo = (int)this.ReadLEUshort();
				int versionRequiredToExtract = (int)this.ReadLEUshort();
				int num13 = (int)this.ReadLEUshort();
				int method = (int)this.ReadLEUshort();
				uint num14 = this.ReadLEUint();
				uint num15 = this.ReadLEUint();
				long num16 = (long)((ulong)this.ReadLEUint());
				long num17 = (long)((ulong)this.ReadLEUint());
				int num18 = (int)this.ReadLEUshort();
				int num19 = (int)this.ReadLEUshort();
				int num20 = (int)this.ReadLEUshort();
				this.ReadLEUshort();
				this.ReadLEUshort();
				uint externalFileAttributes = this.ReadLEUint();
				long offset = (long)((ulong)this.ReadLEUint());
				byte[] array2 = new byte[Math.Max(num18, num20)];
				StreamUtils.ReadFully(this.baseStream_, array2, 0, num18);
				string name = ZipConstants.ConvertToStringExt(num13, array2, num18);
				ZipEntry zipEntry = new ZipEntry(name, versionRequiredToExtract, madeByInfo, (CompressionMethod)method);
				zipEntry.Crc = (long)((ulong)num15 & (ulong)-1);
				zipEntry.Size = (num17 & (long)((ulong)-1));
				zipEntry.CompressedSize = (num16 & (long)((ulong)-1));
				zipEntry.Flags = num13;
				zipEntry.DosTime = (long)((ulong)num14);
				zipEntry.ZipFileIndex = (long)num12;
				zipEntry.Offset = offset;
				zipEntry.ExternalFileAttributes = (int)externalFileAttributes;
				if ((num13 & 8) == 0)
				{
					zipEntry.CryptoCheckValue = (byte)(num15 >> 24);
				}
				else
				{
					zipEntry.CryptoCheckValue = (byte)(num14 >> 8 & 255u);
				}
				if (num19 > 0)
				{
					byte[] array3 = new byte[num19];
					StreamUtils.ReadFully(this.baseStream_, array3);
					zipEntry.ExtraData = array3;
				}
				zipEntry.ProcessExtraData(false);
				if (num20 > 0)
				{
					StreamUtils.ReadFully(this.baseStream_, array2, 0, num20);
					zipEntry.Comment = ZipConstants.ConvertToStringExt(num13, array2, num20);
				}
				this.entries_[(int)(checked((IntPtr)num12))] = zipEntry;
			}
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x00037D9B File Offset: 0x00035F9B
		private long LocateEntry(ZipEntry entry)
		{
			return this.TestLocalHeader(entry, ZipFile.HeaderTest.Extract);
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x00037DA8 File Offset: 0x00035FA8
		private Stream CreateAndInitDecryptionStream(Stream baseStream, ZipEntry entry)
		{
			CryptoStream cryptoStream;
			if (entry.Version < 50 || (entry.Flags & 64) == 0)
			{
				PkzipClassicManaged pkzipClassicManaged = new PkzipClassicManaged();
				this.OnKeysRequired(entry.Name);
				if (!this.HaveKeys)
				{
					throw new ZipException("No password available for encrypted stream");
				}
				cryptoStream = new CryptoStream(baseStream, pkzipClassicManaged.CreateDecryptor(this.key, null), CryptoStreamMode.Read);
				ZipFile.CheckClassicPassword(cryptoStream, entry);
			}
			else
			{
				if (entry.Version != 51)
				{
					throw new ZipException("Decryption method not supported");
				}
				this.OnKeysRequired(entry.Name);
				if (!this.HaveKeys)
				{
					throw new ZipException("No password available for AES encrypted stream");
				}
				int aESSaltLen = entry.AESSaltLen;
				byte[] array = new byte[aESSaltLen];
				int num = baseStream.Read(array, 0, aESSaltLen);
				if (num != aESSaltLen)
				{
					throw new ZipException(string.Concat(new object[]
					{
						"AES Salt expected ",
						aESSaltLen,
						" got ",
						num
					}));
				}
				byte[] array2 = new byte[2];
				baseStream.Read(array2, 0, 2);
				int blockSize = entry.AESKeySize / 8;
				ZipAESTransform zipAESTransform = new ZipAESTransform(this.rawPassword_, array, blockSize, false);
				byte[] pwdVerifier = zipAESTransform.PwdVerifier;
				if (pwdVerifier[0] != array2[0] || pwdVerifier[1] != array2[1])
				{
					throw new Exception("Invalid password for AES");
				}
				cryptoStream = new ZipAESStream(baseStream, zipAESTransform, CryptoStreamMode.Read);
			}
			return cryptoStream;
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00037F08 File Offset: 0x00036108
		private Stream CreateAndInitEncryptionStream(Stream baseStream, ZipEntry entry)
		{
			CryptoStream cryptoStream = null;
			if (entry.Version < 50 || (entry.Flags & 64) == 0)
			{
				PkzipClassicManaged pkzipClassicManaged = new PkzipClassicManaged();
				this.OnKeysRequired(entry.Name);
				if (!this.HaveKeys)
				{
					throw new ZipException("No password available for encrypted stream");
				}
				cryptoStream = new CryptoStream(new ZipFile.UncompressedStream(baseStream), pkzipClassicManaged.CreateEncryptor(this.key, null), CryptoStreamMode.Write);
				if (entry.Crc < 0L || (entry.Flags & 8) != 0)
				{
					ZipFile.WriteEncryptionHeader(cryptoStream, entry.DosTime << 16);
				}
				else
				{
					ZipFile.WriteEncryptionHeader(cryptoStream, entry.Crc);
				}
			}
			return cryptoStream;
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x00037FA0 File Offset: 0x000361A0
		private static void CheckClassicPassword(CryptoStream classicCryptoStream, ZipEntry entry)
		{
			byte[] array = new byte[12];
			StreamUtils.ReadFully(classicCryptoStream, array);
			if (array[11] != entry.CryptoCheckValue)
			{
				throw new ZipException("Invalid password");
			}
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x00037FD4 File Offset: 0x000361D4
		private static void WriteEncryptionHeader(Stream stream, long crcValue)
		{
			byte[] array = new byte[12];
			Random random = new Random();
			random.NextBytes(array);
			array[11] = (byte)(crcValue >> 24);
			stream.Write(array, 0, array.Length);
		}

		// Token: 0x0400053D RID: 1341
		private const int DefaultBufferSize = 4096;

		// Token: 0x0400053E RID: 1342
		public ZipFile.KeysRequiredEventHandler KeysRequired;

		// Token: 0x0400053F RID: 1343
		private bool isDisposed_;

		// Token: 0x04000540 RID: 1344
		private string name_;

		// Token: 0x04000541 RID: 1345
		private string comment_;

		// Token: 0x04000542 RID: 1346
		private string rawPassword_;

		// Token: 0x04000543 RID: 1347
		private Stream baseStream_;

		// Token: 0x04000544 RID: 1348
		private bool isStreamOwner;

		// Token: 0x04000545 RID: 1349
		private long offsetOfFirstEntry;

		// Token: 0x04000546 RID: 1350
		private ZipEntry[] entries_;

		// Token: 0x04000547 RID: 1351
		private byte[] key;

		// Token: 0x04000548 RID: 1352
		private bool isNewArchive_;

		// Token: 0x04000549 RID: 1353
		private UseZip64 useZip64_ = UseZip64.Dynamic;

		// Token: 0x0400054A RID: 1354
		private ArrayList updates_;

		// Token: 0x0400054B RID: 1355
		private long updateCount_;

		// Token: 0x0400054C RID: 1356
		private Hashtable updateIndex_;

		// Token: 0x0400054D RID: 1357
		private IArchiveStorage archiveStorage_;

		// Token: 0x0400054E RID: 1358
		private IDynamicDataSource updateDataSource_;

		// Token: 0x0400054F RID: 1359
		private bool contentsEdited_;

		// Token: 0x04000550 RID: 1360
		private int bufferSize_ = 4096;

		// Token: 0x04000551 RID: 1361
		private byte[] copyBuffer_;

		// Token: 0x04000552 RID: 1362
		private ZipFile.ZipString newComment_;

		// Token: 0x04000553 RID: 1363
		private bool commentEdited_;

		// Token: 0x04000554 RID: 1364
		private IEntryFactory updateEntryFactory_ = new ZipEntryFactory();

		// Token: 0x020000EE RID: 238
		// (Invoke) Token: 0x06000A1C RID: 2588
		public delegate void KeysRequiredEventHandler(object sender, KeysRequiredEventArgs e);

		// Token: 0x020000EF RID: 239
		[Flags]
		private enum HeaderTest
		{
			// Token: 0x04000556 RID: 1366
			Extract = 1,
			// Token: 0x04000557 RID: 1367
			Header = 2
		}

		// Token: 0x020000F0 RID: 240
		private enum UpdateCommand
		{
			// Token: 0x04000559 RID: 1369
			Copy,
			// Token: 0x0400055A RID: 1370
			Modify,
			// Token: 0x0400055B RID: 1371
			Add
		}

		// Token: 0x020000F1 RID: 241
		private class UpdateComparer : IComparer
		{
			// Token: 0x06000A1F RID: 2591 RVA: 0x0003800C File Offset: 0x0003620C
			public int Compare(object x, object y)
			{
				ZipFile.ZipUpdate zipUpdate = x as ZipFile.ZipUpdate;
				ZipFile.ZipUpdate zipUpdate2 = y as ZipFile.ZipUpdate;
				int num;
				if (zipUpdate == null)
				{
					if (zipUpdate2 == null)
					{
						num = 0;
					}
					else
					{
						num = -1;
					}
				}
				else if (zipUpdate2 == null)
				{
					num = 1;
				}
				else
				{
					int num2 = (zipUpdate.Command == ZipFile.UpdateCommand.Copy || zipUpdate.Command == ZipFile.UpdateCommand.Modify) ? 0 : 1;
					int num3 = (zipUpdate2.Command == ZipFile.UpdateCommand.Copy || zipUpdate2.Command == ZipFile.UpdateCommand.Modify) ? 0 : 1;
					num = num2 - num3;
					if (num == 0)
					{
						long num4 = zipUpdate.Entry.Offset - zipUpdate2.Entry.Offset;
						if (num4 < 0L)
						{
							num = -1;
						}
						else if (num4 == 0L)
						{
							num = 0;
						}
						else
						{
							num = 1;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x020000F2 RID: 242
		private class ZipUpdate
		{
			// Token: 0x06000A21 RID: 2593 RVA: 0x000380A9 File Offset: 0x000362A9
			public ZipUpdate(string fileName, ZipEntry entry)
			{
				this.command_ = ZipFile.UpdateCommand.Add;
				this.entry_ = entry;
				this.filename_ = fileName;
			}

			// Token: 0x06000A22 RID: 2594 RVA: 0x000380E0 File Offset: 0x000362E0
			[Obsolete]
			public ZipUpdate(string fileName, string entryName, CompressionMethod compressionMethod)
			{
				this.command_ = ZipFile.UpdateCommand.Add;
				this.entry_ = new ZipEntry(entryName);
				this.entry_.CompressionMethod = compressionMethod;
				this.filename_ = fileName;
			}

			// Token: 0x06000A23 RID: 2595 RVA: 0x00038131 File Offset: 0x00036331
			[Obsolete]
			public ZipUpdate(string fileName, string entryName) : this(fileName, entryName, CompressionMethod.Deflated)
			{
			}

			// Token: 0x06000A24 RID: 2596 RVA: 0x0003813C File Offset: 0x0003633C
			[Obsolete]
			public ZipUpdate(IStaticDataSource dataSource, string entryName, CompressionMethod compressionMethod)
			{
				this.command_ = ZipFile.UpdateCommand.Add;
				this.entry_ = new ZipEntry(entryName);
				this.entry_.CompressionMethod = compressionMethod;
				this.dataSource_ = dataSource;
			}

			// Token: 0x06000A25 RID: 2597 RVA: 0x0003818D File Offset: 0x0003638D
			public ZipUpdate(IStaticDataSource dataSource, ZipEntry entry)
			{
				this.command_ = ZipFile.UpdateCommand.Add;
				this.entry_ = entry;
				this.dataSource_ = dataSource;
			}

			// Token: 0x06000A26 RID: 2598 RVA: 0x000381C2 File Offset: 0x000363C2
			public ZipUpdate(ZipEntry original, ZipEntry updated)
			{
				throw new ZipException("Modify not currently supported");
			}

			// Token: 0x06000A27 RID: 2599 RVA: 0x000381EC File Offset: 0x000363EC
			public ZipUpdate(ZipFile.UpdateCommand command, ZipEntry entry)
			{
				this.command_ = command;
				this.entry_ = (ZipEntry)entry.Clone();
			}

			// Token: 0x06000A28 RID: 2600 RVA: 0x00038224 File Offset: 0x00036424
			public ZipUpdate(ZipEntry entry) : this(ZipFile.UpdateCommand.Copy, entry)
			{
			}

			// Token: 0x170002DA RID: 730
			// (get) Token: 0x06000A29 RID: 2601 RVA: 0x0003822E File Offset: 0x0003642E
			public ZipEntry Entry
			{
				get
				{
					return this.entry_;
				}
			}

			// Token: 0x170002DB RID: 731
			// (get) Token: 0x06000A2A RID: 2602 RVA: 0x00038236 File Offset: 0x00036436
			public ZipEntry OutEntry
			{
				get
				{
					if (this.outEntry_ == null)
					{
						this.outEntry_ = (ZipEntry)this.entry_.Clone();
					}
					return this.outEntry_;
				}
			}

			// Token: 0x170002DC RID: 732
			// (get) Token: 0x06000A2B RID: 2603 RVA: 0x0003825C File Offset: 0x0003645C
			public ZipFile.UpdateCommand Command
			{
				get
				{
					return this.command_;
				}
			}

			// Token: 0x170002DD RID: 733
			// (get) Token: 0x06000A2C RID: 2604 RVA: 0x00038264 File Offset: 0x00036464
			public string Filename
			{
				get
				{
					return this.filename_;
				}
			}

			// Token: 0x170002DE RID: 734
			// (get) Token: 0x06000A2D RID: 2605 RVA: 0x0003826C File Offset: 0x0003646C
			// (set) Token: 0x06000A2E RID: 2606 RVA: 0x00038274 File Offset: 0x00036474
			public long SizePatchOffset
			{
				get
				{
					return this.sizePatchOffset_;
				}
				set
				{
					this.sizePatchOffset_ = value;
				}
			}

			// Token: 0x170002DF RID: 735
			// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0003827D File Offset: 0x0003647D
			// (set) Token: 0x06000A30 RID: 2608 RVA: 0x00038285 File Offset: 0x00036485
			public long CrcPatchOffset
			{
				get
				{
					return this.crcPatchOffset_;
				}
				set
				{
					this.crcPatchOffset_ = value;
				}
			}

			// Token: 0x170002E0 RID: 736
			// (get) Token: 0x06000A31 RID: 2609 RVA: 0x0003828E File Offset: 0x0003648E
			// (set) Token: 0x06000A32 RID: 2610 RVA: 0x00038296 File Offset: 0x00036496
			public long OffsetBasedSize
			{
				get
				{
					return this._offsetBasedSize;
				}
				set
				{
					this._offsetBasedSize = value;
				}
			}

			// Token: 0x06000A33 RID: 2611 RVA: 0x000382A0 File Offset: 0x000364A0
			public Stream GetSource()
			{
				Stream result = null;
				if (this.dataSource_ != null)
				{
					result = this.dataSource_.GetSource();
				}
				return result;
			}

			// Token: 0x0400055C RID: 1372
			private ZipEntry entry_;

			// Token: 0x0400055D RID: 1373
			private ZipEntry outEntry_;

			// Token: 0x0400055E RID: 1374
			private ZipFile.UpdateCommand command_;

			// Token: 0x0400055F RID: 1375
			private IStaticDataSource dataSource_;

			// Token: 0x04000560 RID: 1376
			private string filename_;

			// Token: 0x04000561 RID: 1377
			private long sizePatchOffset_ = -1L;

			// Token: 0x04000562 RID: 1378
			private long crcPatchOffset_ = -1L;

			// Token: 0x04000563 RID: 1379
			private long _offsetBasedSize = -1L;
		}

		// Token: 0x020000F3 RID: 243
		private class ZipString
		{
			// Token: 0x06000A34 RID: 2612 RVA: 0x000382C4 File Offset: 0x000364C4
			public ZipString(string comment)
			{
				this.comment_ = comment;
				this.isSourceString_ = true;
			}

			// Token: 0x06000A35 RID: 2613 RVA: 0x000382DA File Offset: 0x000364DA
			public ZipString(byte[] rawString)
			{
				this.rawComment_ = rawString;
			}

			// Token: 0x170002E1 RID: 737
			// (get) Token: 0x06000A36 RID: 2614 RVA: 0x000382E9 File Offset: 0x000364E9
			public bool IsSourceString
			{
				get
				{
					return this.isSourceString_;
				}
			}

			// Token: 0x170002E2 RID: 738
			// (get) Token: 0x06000A37 RID: 2615 RVA: 0x000382F1 File Offset: 0x000364F1
			public int RawLength
			{
				get
				{
					this.MakeBytesAvailable();
					return this.rawComment_.Length;
				}
			}

			// Token: 0x170002E3 RID: 739
			// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00038301 File Offset: 0x00036501
			public byte[] RawComment
			{
				get
				{
					this.MakeBytesAvailable();
					return (byte[])this.rawComment_.Clone();
				}
			}

			// Token: 0x06000A39 RID: 2617 RVA: 0x00038319 File Offset: 0x00036519
			public void Reset()
			{
				if (this.isSourceString_)
				{
					this.rawComment_ = null;
					return;
				}
				this.comment_ = null;
			}

			// Token: 0x06000A3A RID: 2618 RVA: 0x00038332 File Offset: 0x00036532
			private void MakeTextAvailable()
			{
				if (this.comment_ == null)
				{
					this.comment_ = ZipConstants.ConvertToString(this.rawComment_);
				}
			}

			// Token: 0x06000A3B RID: 2619 RVA: 0x0003834D File Offset: 0x0003654D
			private void MakeBytesAvailable()
			{
				if (this.rawComment_ == null)
				{
					this.rawComment_ = ZipConstants.ConvertToArray(this.comment_);
				}
			}

			// Token: 0x06000A3C RID: 2620 RVA: 0x00038368 File Offset: 0x00036568
			public static implicit operator string(ZipFile.ZipString zipString)
			{
				zipString.MakeTextAvailable();
				return zipString.comment_;
			}

			// Token: 0x04000564 RID: 1380
			private string comment_;

			// Token: 0x04000565 RID: 1381
			private byte[] rawComment_;

			// Token: 0x04000566 RID: 1382
			private bool isSourceString_;
		}

		// Token: 0x020000F4 RID: 244
		private class ZipEntryEnumerator : IEnumerator
		{
			// Token: 0x06000A3D RID: 2621 RVA: 0x00038376 File Offset: 0x00036576
			public ZipEntryEnumerator(ZipEntry[] entries)
			{
				this.array = entries;
			}

			// Token: 0x170002E4 RID: 740
			// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0003838C File Offset: 0x0003658C
			public object Current
			{
				get
				{
					return this.array[this.index];
				}
			}

			// Token: 0x06000A3F RID: 2623 RVA: 0x0003839B File Offset: 0x0003659B
			public void Reset()
			{
				this.index = -1;
			}

			// Token: 0x06000A40 RID: 2624 RVA: 0x000383A4 File Offset: 0x000365A4
			public bool MoveNext()
			{
				return ++this.index < this.array.Length;
			}

			// Token: 0x04000567 RID: 1383
			private ZipEntry[] array;

			// Token: 0x04000568 RID: 1384
			private int index = -1;
		}

		// Token: 0x020000F5 RID: 245
		private class UncompressedStream : Stream
		{
			// Token: 0x06000A41 RID: 2625 RVA: 0x000383CC File Offset: 0x000365CC
			public UncompressedStream(Stream baseStream)
			{
				this.baseStream_ = baseStream;
			}

			// Token: 0x06000A42 RID: 2626 RVA: 0x000383DB File Offset: 0x000365DB
			public override void Close()
			{
			}

			// Token: 0x170002E5 RID: 741
			// (get) Token: 0x06000A43 RID: 2627 RVA: 0x000383DD File Offset: 0x000365DD
			public override bool CanRead
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000A44 RID: 2628 RVA: 0x000383E0 File Offset: 0x000365E0
			public override void Flush()
			{
				this.baseStream_.Flush();
			}

			// Token: 0x170002E6 RID: 742
			// (get) Token: 0x06000A45 RID: 2629 RVA: 0x000383ED File Offset: 0x000365ED
			public override bool CanWrite
			{
				get
				{
					return this.baseStream_.CanWrite;
				}
			}

			// Token: 0x170002E7 RID: 743
			// (get) Token: 0x06000A46 RID: 2630 RVA: 0x000383FA File Offset: 0x000365FA
			public override bool CanSeek
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170002E8 RID: 744
			// (get) Token: 0x06000A47 RID: 2631 RVA: 0x000383FD File Offset: 0x000365FD
			public override long Length
			{
				get
				{
					return 0L;
				}
			}

			// Token: 0x170002E9 RID: 745
			// (get) Token: 0x06000A48 RID: 2632 RVA: 0x00038401 File Offset: 0x00036601
			// (set) Token: 0x06000A49 RID: 2633 RVA: 0x0003840E File Offset: 0x0003660E
			public override long Position
			{
				get
				{
					return this.baseStream_.Position;
				}
				set
				{
				}
			}

			// Token: 0x06000A4A RID: 2634 RVA: 0x00038410 File Offset: 0x00036610
			public override int Read(byte[] buffer, int offset, int count)
			{
				return 0;
			}

			// Token: 0x06000A4B RID: 2635 RVA: 0x00038413 File Offset: 0x00036613
			public override long Seek(long offset, SeekOrigin origin)
			{
				return 0L;
			}

			// Token: 0x06000A4C RID: 2636 RVA: 0x00038417 File Offset: 0x00036617
			public override void SetLength(long value)
			{
			}

			// Token: 0x06000A4D RID: 2637 RVA: 0x00038419 File Offset: 0x00036619
			public override void Write(byte[] buffer, int offset, int count)
			{
				this.baseStream_.Write(buffer, offset, count);
			}

			// Token: 0x04000569 RID: 1385
			private Stream baseStream_;
		}

		// Token: 0x020000F6 RID: 246
		private class PartialInputStream : Stream
		{
			// Token: 0x06000A4E RID: 2638 RVA: 0x00038429 File Offset: 0x00036629
			public PartialInputStream(ZipFile zipFile, long start, long length)
			{
				this.start_ = start;
				this.length_ = length;
				this.zipFile_ = zipFile;
				this.baseStream_ = this.zipFile_.baseStream_;
				this.readPos_ = start;
				this.end_ = start + length;
			}

			// Token: 0x06000A4F RID: 2639 RVA: 0x00038468 File Offset: 0x00036668
			public override int ReadByte()
			{
				if (this.readPos_ >= this.end_)
				{
					return -1;
				}
				int result;
				lock (this.baseStream_)
				{
					Stream arg_3A_0 = this.baseStream_;
					long offset;
					this.readPos_ = (offset = this.readPos_) + 1L;
					arg_3A_0.Seek(offset, SeekOrigin.Begin);
					result = this.baseStream_.ReadByte();
				}
				return result;
			}

			// Token: 0x06000A50 RID: 2640 RVA: 0x000384E0 File Offset: 0x000366E0
			public override void Close()
			{
			}

			// Token: 0x06000A51 RID: 2641 RVA: 0x000384E4 File Offset: 0x000366E4
			public override int Read(byte[] buffer, int offset, int count)
			{
				int result;
				lock (this.baseStream_)
				{
					if ((long)count > this.end_ - this.readPos_)
					{
						count = (int)(this.end_ - this.readPos_);
						if (count == 0)
						{
							result = 0;
							return result;
						}
					}
					this.baseStream_.Seek(this.readPos_, SeekOrigin.Begin);
					int num = this.baseStream_.Read(buffer, offset, count);
					if (num > 0)
					{
						this.readPos_ += (long)num;
					}
					result = num;
				}
				return result;
			}

			// Token: 0x06000A52 RID: 2642 RVA: 0x00038580 File Offset: 0x00036780
			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000A53 RID: 2643 RVA: 0x00038587 File Offset: 0x00036787
			public override void SetLength(long value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000A54 RID: 2644 RVA: 0x00038590 File Offset: 0x00036790
			public override long Seek(long offset, SeekOrigin origin)
			{
				long num = this.readPos_;
				switch (origin)
				{
				case SeekOrigin.Begin:
					num = this.start_ + offset;
					break;
				case SeekOrigin.Current:
					num = this.readPos_ + offset;
					break;
				case SeekOrigin.End:
					num = this.end_ + offset;
					break;
				}
				if (num < this.start_)
				{
					throw new ArgumentException("Negative position is invalid");
				}
				if (num >= this.end_)
				{
					throw new IOException("Cannot seek past end");
				}
				this.readPos_ = num;
				return this.readPos_;
			}

			// Token: 0x06000A55 RID: 2645 RVA: 0x0003860E File Offset: 0x0003680E
			public override void Flush()
			{
			}

			// Token: 0x170002EA RID: 746
			// (get) Token: 0x06000A56 RID: 2646 RVA: 0x00038610 File Offset: 0x00036810
			// (set) Token: 0x06000A57 RID: 2647 RVA: 0x00038620 File Offset: 0x00036820
			public override long Position
			{
				get
				{
					return this.readPos_ - this.start_;
				}
				set
				{
					long num = this.start_ + value;
					if (num < this.start_)
					{
						throw new ArgumentException("Negative position is invalid");
					}
					if (num >= this.end_)
					{
						throw new InvalidOperationException("Cannot seek past end");
					}
					this.readPos_ = num;
				}
			}

			// Token: 0x170002EB RID: 747
			// (get) Token: 0x06000A58 RID: 2648 RVA: 0x00038665 File Offset: 0x00036865
			public override long Length
			{
				get
				{
					return this.length_;
				}
			}

			// Token: 0x170002EC RID: 748
			// (get) Token: 0x06000A59 RID: 2649 RVA: 0x0003866D File Offset: 0x0003686D
			public override bool CanWrite
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170002ED RID: 749
			// (get) Token: 0x06000A5A RID: 2650 RVA: 0x00038670 File Offset: 0x00036870
			public override bool CanSeek
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170002EE RID: 750
			// (get) Token: 0x06000A5B RID: 2651 RVA: 0x00038673 File Offset: 0x00036873
			public override bool CanRead
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170002EF RID: 751
			// (get) Token: 0x06000A5C RID: 2652 RVA: 0x00038676 File Offset: 0x00036876
			public override bool CanTimeout
			{
				get
				{
					return this.baseStream_.CanTimeout;
				}
			}

			// Token: 0x0400056A RID: 1386
			private ZipFile zipFile_;

			// Token: 0x0400056B RID: 1387
			private Stream baseStream_;

			// Token: 0x0400056C RID: 1388
			private long start_;

			// Token: 0x0400056D RID: 1389
			private long length_;

			// Token: 0x0400056E RID: 1390
			private long readPos_;

			// Token: 0x0400056F RID: 1391
			private long end_;
		}
	}
}
