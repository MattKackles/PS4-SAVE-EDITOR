using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Ionic.Crc;
using Ionic.Zlib;

namespace Ionic.Zip
{
	// Token: 0x0200014E RID: 334
	[ClassInterface(ClassInterfaceType.AutoDispatch), ComVisible(true), Guid("ebc25cf6-9120-4283-b972-0e5520d00004")]
	public class ZipEntry
	{
		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000D2E RID: 3374 RVA: 0x0004C4FF File Offset: 0x0004A6FF
		internal bool AttributesIndicateDirectory
		{
			get
			{
				return this._InternalFileAttrs == 0 && (this._ExternalFileAttrs & 16) == 16;
			}
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0004C518 File Offset: 0x0004A718
		internal void ResetDirEntry()
		{
			this.__FileDataPosition = -1L;
			this._LengthOfHeader = 0;
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000D30 RID: 3376 RVA: 0x0004C52C File Offset: 0x0004A72C
		public string Info
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Format("          ZipEntry: {0}\n", this.FileName)).Append(string.Format("   Version Made By: {0}\n", this._VersionMadeBy)).Append(string.Format(" Needed to extract: {0}\n", this.VersionNeeded));
				if (this._IsDirectory)
				{
					stringBuilder.Append("        Entry type: directory\n");
				}
				else
				{
					stringBuilder.Append(string.Format("         File type: {0}\n", this._IsText ? "text" : "binary")).Append(string.Format("       Compression: {0}\n", this.CompressionMethod)).Append(string.Format("        Compressed: 0x{0:X}\n", this.CompressedSize)).Append(string.Format("      Uncompressed: 0x{0:X}\n", this.UncompressedSize)).Append(string.Format("             CRC32: 0x{0:X8}\n", this._Crc32));
				}
				stringBuilder.Append(string.Format("       Disk Number: {0}\n", this._diskNumber));
				if (this._RelativeOffsetOfLocalHeader > (long)((ulong)-1))
				{
					stringBuilder.Append(string.Format("   Relative Offset: 0x{0:X16}\n", this._RelativeOffsetOfLocalHeader));
				}
				else
				{
					stringBuilder.Append(string.Format("   Relative Offset: 0x{0:X8}\n", this._RelativeOffsetOfLocalHeader));
				}
				stringBuilder.Append(string.Format("         Bit Field: 0x{0:X4}\n", this._BitField)).Append(string.Format("        Encrypted?: {0}\n", this._sourceIsEncrypted)).Append(string.Format("          Timeblob: 0x{0:X8}\n", this._TimeBlob)).Append(string.Format("              Time: {0}\n", SharedUtilities.PackedToDateTime(this._TimeBlob)));
				stringBuilder.Append(string.Format("         Is Zip64?: {0}\n", this._InputUsesZip64));
				if (!string.IsNullOrEmpty(this._Comment))
				{
					stringBuilder.Append(string.Format("           Comment: {0}\n", this._Comment));
				}
				stringBuilder.Append("\n");
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0004C754 File Offset: 0x0004A954
		internal static ZipEntry ReadDirEntry(ZipFile zf, Dictionary<string, object> previouslySeen)
		{
			Stream readStream = zf.ReadStream;
			Encoding encoding = (zf.AlternateEncodingUsage == ZipOption.Always) ? zf.AlternateEncoding : ZipFile.DefaultEncoding;
			int num = SharedUtilities.ReadSignature(readStream);
			if (ZipEntry.IsNotValidZipDirEntrySig(num))
			{
				readStream.Seek(-4L, SeekOrigin.Current);
				if ((long)num != 101010256L && (long)num != 101075792L && num != 67324752)
				{
					throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) at position 0x{1:X8}", num, readStream.Position));
				}
				return null;
			}
			else
			{
				int num2 = 46;
				byte[] array = new byte[42];
				int num3 = readStream.Read(array, 0, array.Length);
				if (num3 != array.Length)
				{
					return null;
				}
				int num4 = 0;
				ZipEntry zipEntry = new ZipEntry();
				zipEntry.AlternateEncoding = encoding;
				zipEntry._Source = ZipEntrySource.ZipFile;
				zipEntry._container = new ZipContainer(zf);
				zipEntry._VersionMadeBy = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._VersionNeeded = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._BitField = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._CompressionMethod = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._TimeBlob = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
				zipEntry._LastModified = SharedUtilities.PackedToDateTime(zipEntry._TimeBlob);
				zipEntry._timestamp |= ZipEntryTimestamp.DOS;
				zipEntry._Crc32 = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
				zipEntry._CompressedSize = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
				zipEntry._UncompressedSize = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
				zipEntry._CompressionMethod_FromZipFile = zipEntry._CompressionMethod;
				zipEntry._filenameLength = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._extraFieldLength = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._commentLength = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._diskNumber = (uint)array[num4++] + (uint)array[num4++] * 256u;
				zipEntry._InternalFileAttrs = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._ExternalFileAttrs = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
				zipEntry._RelativeOffsetOfLocalHeader = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
				zipEntry.IsText = ((zipEntry._InternalFileAttrs & 1) == 1);
				array = new byte[(int)zipEntry._filenameLength];
				num3 = readStream.Read(array, 0, array.Length);
				num2 += num3;
				if ((zipEntry._BitField & 2048) == 2048)
				{
					zipEntry._FileNameInArchive = SharedUtilities.Utf8StringFromBuffer(array);
				}
				else
				{
					zipEntry._FileNameInArchive = SharedUtilities.StringFromBuffer(array, encoding);
				}
				while (previouslySeen.ContainsKey(zipEntry._FileNameInArchive))
				{
					zipEntry._FileNameInArchive = ZipEntry.CopyHelper.AppendCopyToFileName(zipEntry._FileNameInArchive);
					zipEntry._metadataChanged = true;
				}
				if (zipEntry.AttributesIndicateDirectory)
				{
					zipEntry.MarkAsDirectory();
				}
				else if (zipEntry._FileNameInArchive.EndsWith("/"))
				{
					zipEntry.MarkAsDirectory();
				}
				zipEntry._CompressedFileDataSize = zipEntry._CompressedSize;
				if ((zipEntry._BitField & 1) == 1)
				{
					zipEntry._Encryption_FromZipFile = (zipEntry._Encryption = EncryptionAlgorithm.PkzipWeak);
					zipEntry._sourceIsEncrypted = true;
				}
				if (zipEntry._extraFieldLength > 0)
				{
					zipEntry._InputUsesZip64 = (zipEntry._CompressedSize == (long)((ulong)-1) || zipEntry._UncompressedSize == (long)((ulong)-1) || zipEntry._RelativeOffsetOfLocalHeader == (long)((ulong)-1));
					num2 += zipEntry.ProcessExtraField(readStream, zipEntry._extraFieldLength);
					zipEntry._CompressedFileDataSize = zipEntry._CompressedSize;
				}
				if (zipEntry._Encryption == EncryptionAlgorithm.PkzipWeak)
				{
					zipEntry._CompressedFileDataSize -= 12L;
				}
				if ((zipEntry._BitField & 8) == 8)
				{
					if (zipEntry._InputUsesZip64)
					{
						zipEntry._LengthOfTrailer += 24;
					}
					else
					{
						zipEntry._LengthOfTrailer += 16;
					}
				}
				zipEntry.AlternateEncoding = (((zipEntry._BitField & 2048) == 2048) ? Encoding.UTF8 : encoding);
				zipEntry.AlternateEncodingUsage = ZipOption.Always;
				if (zipEntry._commentLength > 0)
				{
					array = new byte[(int)zipEntry._commentLength];
					num3 = readStream.Read(array, 0, array.Length);
					num2 += num3;
					if ((zipEntry._BitField & 2048) == 2048)
					{
						zipEntry._Comment = SharedUtilities.Utf8StringFromBuffer(array);
					}
					else
					{
						zipEntry._Comment = SharedUtilities.StringFromBuffer(array, encoding);
					}
				}
				return zipEntry;
			}
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0004CDBE File Offset: 0x0004AFBE
		internal static bool IsNotValidZipDirEntrySig(int signature)
		{
			return signature != 33639248;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0004CDCC File Offset: 0x0004AFCC
		public ZipEntry()
		{
			this._CompressionMethod = 8;
			this._CompressionLevel = CompressionLevel.Default;
			this._Encryption = EncryptionAlgorithm.None;
			this._Source = ZipEntrySource.None;
			this.AlternateEncoding = Encoding.GetEncoding("IBM437");
			this.AlternateEncodingUsage = ZipOption.Default;
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x0004CE33 File Offset: 0x0004B033
		// (set) Token: 0x06000D35 RID: 3381 RVA: 0x0004CE40 File Offset: 0x0004B040
		public DateTime LastModified
		{
			get
			{
				return this._LastModified.ToLocalTime();
			}
			set
			{
				this._LastModified = ((value.Kind == DateTimeKind.Unspecified) ? DateTime.SpecifyKind(value, DateTimeKind.Local) : value.ToLocalTime());
				this._Mtime = SharedUtilities.AdjustTime_Reverse(this._LastModified).ToUniversalTime();
				this._metadataChanged = true;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000D36 RID: 3382 RVA: 0x0004CE8C File Offset: 0x0004B08C
		private int BufferSize
		{
			get
			{
				return this._container.BufferSize;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x0004CE99 File Offset: 0x0004B099
		// (set) Token: 0x06000D38 RID: 3384 RVA: 0x0004CEA1 File Offset: 0x0004B0A1
		public DateTime ModifiedTime
		{
			get
			{
				return this._Mtime;
			}
			set
			{
				this.SetEntryTimes(this._Ctime, this._Atime, value);
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x0004CEB6 File Offset: 0x0004B0B6
		// (set) Token: 0x06000D3A RID: 3386 RVA: 0x0004CEBE File Offset: 0x0004B0BE
		public DateTime AccessedTime
		{
			get
			{
				return this._Atime;
			}
			set
			{
				this.SetEntryTimes(this._Ctime, value, this._Mtime);
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x0004CED3 File Offset: 0x0004B0D3
		// (set) Token: 0x06000D3C RID: 3388 RVA: 0x0004CEDB File Offset: 0x0004B0DB
		public DateTime CreationTime
		{
			get
			{
				return this._Ctime;
			}
			set
			{
				this.SetEntryTimes(value, this._Atime, this._Mtime);
			}
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0004CEF0 File Offset: 0x0004B0F0
		public void SetEntryTimes(DateTime created, DateTime accessed, DateTime modified)
		{
			this._ntfsTimesAreSet = true;
			if (created == ZipEntry._zeroHour && created.Kind == ZipEntry._zeroHour.Kind)
			{
				created = ZipEntry._win32Epoch;
			}
			if (accessed == ZipEntry._zeroHour && accessed.Kind == ZipEntry._zeroHour.Kind)
			{
				accessed = ZipEntry._win32Epoch;
			}
			if (modified == ZipEntry._zeroHour && modified.Kind == ZipEntry._zeroHour.Kind)
			{
				modified = ZipEntry._win32Epoch;
			}
			this._Ctime = created.ToUniversalTime();
			this._Atime = accessed.ToUniversalTime();
			this._Mtime = modified.ToUniversalTime();
			this._LastModified = this._Mtime;
			if (!this._emitUnixTimes && !this._emitNtfsTimes)
			{
				this._emitNtfsTimes = true;
			}
			this._metadataChanged = true;
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x0004CFCA File Offset: 0x0004B1CA
		// (set) Token: 0x06000D3F RID: 3391 RVA: 0x0004CFD2 File Offset: 0x0004B1D2
		public bool EmitTimesInWindowsFormatWhenSaving
		{
			get
			{
				return this._emitNtfsTimes;
			}
			set
			{
				this._emitNtfsTimes = value;
				this._metadataChanged = true;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x0004CFE2 File Offset: 0x0004B1E2
		// (set) Token: 0x06000D41 RID: 3393 RVA: 0x0004CFEA File Offset: 0x0004B1EA
		public bool EmitTimesInUnixFormatWhenSaving
		{
			get
			{
				return this._emitUnixTimes;
			}
			set
			{
				this._emitUnixTimes = value;
				this._metadataChanged = true;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000D42 RID: 3394 RVA: 0x0004CFFA File Offset: 0x0004B1FA
		public ZipEntryTimestamp Timestamp
		{
			get
			{
				return this._timestamp;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x0004D002 File Offset: 0x0004B202
		// (set) Token: 0x06000D44 RID: 3396 RVA: 0x0004D00A File Offset: 0x0004B20A
		public FileAttributes Attributes
		{
			get
			{
				return (FileAttributes)this._ExternalFileAttrs;
			}
			set
			{
				this._ExternalFileAttrs = (int)value;
				this._VersionMadeBy = 45;
				this._metadataChanged = true;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x0004D022 File Offset: 0x0004B222
		internal string LocalFileName
		{
			get
			{
				return this._LocalFileName;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x0004D02A File Offset: 0x0004B22A
		// (set) Token: 0x06000D47 RID: 3399 RVA: 0x0004D034 File Offset: 0x0004B234
		public string FileName
		{
			get
			{
				return this._FileNameInArchive;
			}
			set
			{
				if (this._container.ZipFile == null)
				{
					throw new ZipException("Cannot rename; this is not supported in ZipOutputStream/ZipInputStream.");
				}
				if (string.IsNullOrEmpty(value))
				{
					throw new ZipException("The FileName must be non empty and non-null.");
				}
				string text = ZipEntry.NameInArchive(value, null);
				if (this._FileNameInArchive == text)
				{
					return;
				}
				this._container.ZipFile.RemoveEntry(this);
				this._container.ZipFile.InternalAddEntry(text, this);
				this._FileNameInArchive = text;
				this._container.ZipFile.NotifyEntryChanged();
				this._metadataChanged = true;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x0004D0C4 File Offset: 0x0004B2C4
		// (set) Token: 0x06000D49 RID: 3401 RVA: 0x0004D0CC File Offset: 0x0004B2CC
		public Stream InputStream
		{
			get
			{
				return this._sourceStream;
			}
			set
			{
				if (this._Source != ZipEntrySource.Stream)
				{
					throw new ZipException("You must not set the input stream for this entry.");
				}
				this._sourceWasJitProvided = true;
				this._sourceStream = value;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x0004D0F0 File Offset: 0x0004B2F0
		public bool InputStreamWasJitProvided
		{
			get
			{
				return this._sourceWasJitProvided;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x0004D0F8 File Offset: 0x0004B2F8
		public ZipEntrySource Source
		{
			get
			{
				return this._Source;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000D4C RID: 3404 RVA: 0x0004D100 File Offset: 0x0004B300
		public short VersionNeeded
		{
			get
			{
				return this._VersionNeeded;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x0004D108 File Offset: 0x0004B308
		// (set) Token: 0x06000D4E RID: 3406 RVA: 0x0004D110 File Offset: 0x0004B310
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				this._Comment = value;
				this._metadataChanged = true;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000D4F RID: 3407 RVA: 0x0004D120 File Offset: 0x0004B320
		public bool? RequiresZip64
		{
			get
			{
				return this._entryRequiresZip64;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000D50 RID: 3408 RVA: 0x0004D128 File Offset: 0x0004B328
		public bool? OutputUsedZip64
		{
			get
			{
				return this._OutputUsesZip64;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000D51 RID: 3409 RVA: 0x0004D130 File Offset: 0x0004B330
		public short BitField
		{
			get
			{
				return this._BitField;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x0004D138 File Offset: 0x0004B338
		// (set) Token: 0x06000D53 RID: 3411 RVA: 0x0004D140 File Offset: 0x0004B340
		public CompressionMethod CompressionMethod
		{
			get
			{
				return (CompressionMethod)this._CompressionMethod;
			}
			set
			{
				if (value == (CompressionMethod)this._CompressionMethod)
				{
					return;
				}
				if (value != CompressionMethod.None && value != CompressionMethod.Deflate)
				{
					throw new InvalidOperationException("Unsupported compression method.");
				}
				this._CompressionMethod = (short)value;
				if (this._CompressionMethod == 0)
				{
					this._CompressionLevel = CompressionLevel.None;
				}
				else if (this.CompressionLevel == CompressionLevel.None)
				{
					this._CompressionLevel = CompressionLevel.Default;
				}
				if (this._container.ZipFile != null)
				{
					this._container.ZipFile.NotifyEntryChanged();
				}
				this._restreamRequiredOnSave = true;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x0004D1B5 File Offset: 0x0004B3B5
		// (set) Token: 0x06000D55 RID: 3413 RVA: 0x0004D1C0 File Offset: 0x0004B3C0
		public CompressionLevel CompressionLevel
		{
			get
			{
				return this._CompressionLevel;
			}
			set
			{
				if (this._CompressionMethod != 8 && this._CompressionMethod != 0)
				{
					return;
				}
				if (value == CompressionLevel.Default && this._CompressionMethod == 8)
				{
					return;
				}
				this._CompressionLevel = value;
				if (value == CompressionLevel.None && this._CompressionMethod == 0)
				{
					return;
				}
				if (this._CompressionLevel == CompressionLevel.None)
				{
					this._CompressionMethod = 0;
				}
				else
				{
					this._CompressionMethod = 8;
				}
				if (this._container.ZipFile != null)
				{
					this._container.ZipFile.NotifyEntryChanged();
				}
				this._restreamRequiredOnSave = true;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x0004D23C File Offset: 0x0004B43C
		public long CompressedSize
		{
			get
			{
				return this._CompressedSize;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x0004D244 File Offset: 0x0004B444
		public long UncompressedSize
		{
			get
			{
				return this._UncompressedSize;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x0004D24C File Offset: 0x0004B44C
		public double CompressionRatio
		{
			get
			{
				if (this.UncompressedSize == 0L)
				{
					return 0.0;
				}
				return 100.0 * (1.0 - 1.0 * (double)this.CompressedSize / (1.0 * (double)this.UncompressedSize));
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x0004D2A4 File Offset: 0x0004B4A4
		public int Crc
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x0004D2AC File Offset: 0x0004B4AC
		public bool IsDirectory
		{
			get
			{
				return this._IsDirectory;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000D5B RID: 3419 RVA: 0x0004D2B4 File Offset: 0x0004B4B4
		public bool UsesEncryption
		{
			get
			{
				return this._Encryption_FromZipFile != EncryptionAlgorithm.None;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x0004D2C2 File Offset: 0x0004B4C2
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x0004D2CC File Offset: 0x0004B4CC
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return this._Encryption;
			}
			set
			{
				if (value == this._Encryption)
				{
					return;
				}
				if (value == EncryptionAlgorithm.Unsupported)
				{
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				this._Encryption = value;
				this._restreamRequiredOnSave = true;
				if (this._container.ZipFile != null)
				{
					this._container.ZipFile.NotifyEntryChanged();
				}
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000D5F RID: 3423 RVA: 0x0004D35D File Offset: 0x0004B55D
		// (set) Token: 0x06000D5E RID: 3422 RVA: 0x0004D31D File Offset: 0x0004B51D
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
					this._Encryption = EncryptionAlgorithm.None;
					return;
				}
				if (this._Source == ZipEntrySource.ZipFile && !this._sourceIsEncrypted)
				{
					this._restreamRequiredOnSave = true;
				}
				if (this.Encryption == EncryptionAlgorithm.None)
				{
					this._Encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x0004D365 File Offset: 0x0004B565
		internal bool IsChanged
		{
			get
			{
				return this._restreamRequiredOnSave | this._metadataChanged;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000D61 RID: 3425 RVA: 0x0004D374 File Offset: 0x0004B574
		// (set) Token: 0x06000D62 RID: 3426 RVA: 0x0004D37C File Offset: 0x0004B57C
		public ExtractExistingFileAction ExtractExistingFile
		{
			get;
			set;
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000D63 RID: 3427 RVA: 0x0004D385 File Offset: 0x0004B585
		// (set) Token: 0x06000D64 RID: 3428 RVA: 0x0004D38D File Offset: 0x0004B58D
		public ZipErrorAction ZipErrorAction
		{
			get;
			set;
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000D65 RID: 3429 RVA: 0x0004D396 File Offset: 0x0004B596
		public bool IncludedInMostRecentSave
		{
			get
			{
				return !this._skippedDuringSave;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x0004D3A1 File Offset: 0x0004B5A1
		// (set) Token: 0x06000D67 RID: 3431 RVA: 0x0004D3A9 File Offset: 0x0004B5A9
		public SetCompressionCallback SetCompression
		{
			get;
			set;
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x0004D3B2 File Offset: 0x0004B5B2
		// (set) Token: 0x06000D69 RID: 3433 RVA: 0x0004D3D1 File Offset: 0x0004B5D1
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete.  It will be removed in a future version of the library. Your applications should  use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				return this.AlternateEncoding == Encoding.GetEncoding("UTF-8") && this.AlternateEncodingUsage == ZipOption.AsNecessary;
			}
			set
			{
				if (value)
				{
					this.AlternateEncoding = Encoding.GetEncoding("UTF-8");
					this.AlternateEncodingUsage = ZipOption.AsNecessary;
					return;
				}
				this.AlternateEncoding = ZipFile.DefaultEncoding;
				this.AlternateEncodingUsage = ZipOption.Default;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x0004D400 File Offset: 0x0004B600
		// (set) Token: 0x06000D6B RID: 3435 RVA: 0x0004D408 File Offset: 0x0004B608
		[Obsolete("This property is obsolete since v1.9.1.6. Use AlternateEncoding and AlternateEncodingUsage instead.", true)]
		public Encoding ProvisionalAlternateEncoding
		{
			get;
			set;
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x0004D411 File Offset: 0x0004B611
		// (set) Token: 0x06000D6D RID: 3437 RVA: 0x0004D419 File Offset: 0x0004B619
		public Encoding AlternateEncoding
		{
			get;
			set;
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x0004D422 File Offset: 0x0004B622
		// (set) Token: 0x06000D6F RID: 3439 RVA: 0x0004D42A File Offset: 0x0004B62A
		public ZipOption AlternateEncodingUsage
		{
			get;
			set;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0004D434 File Offset: 0x0004B634
		internal static string NameInArchive(string filename, string directoryPathInArchive)
		{
			string pathName;
			if (directoryPathInArchive == null)
			{
				pathName = filename;
			}
			else if (string.IsNullOrEmpty(directoryPathInArchive))
			{
				pathName = Path.GetFileName(filename);
			}
			else
			{
				pathName = Path.Combine(directoryPathInArchive, Path.GetFileName(filename));
			}
			return SharedUtilities.NormalizePathForUseInZipFile(pathName);
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0004D470 File Offset: 0x0004B670
		internal static ZipEntry CreateFromNothing(string nameInArchive)
		{
			return ZipEntry.Create(nameInArchive, ZipEntrySource.None, null, null);
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0004D47B File Offset: 0x0004B67B
		internal static ZipEntry CreateFromFile(string filename, string nameInArchive)
		{
			return ZipEntry.Create(nameInArchive, ZipEntrySource.FileSystem, filename, null);
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0004D486 File Offset: 0x0004B686
		internal static ZipEntry CreateForStream(string entryName, Stream s)
		{
			return ZipEntry.Create(entryName, ZipEntrySource.Stream, s, null);
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0004D491 File Offset: 0x0004B691
		internal static ZipEntry CreateForWriter(string entryName, WriteDelegate d)
		{
			return ZipEntry.Create(entryName, ZipEntrySource.WriteDelegate, d, null);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0004D49C File Offset: 0x0004B69C
		internal static ZipEntry CreateForJitStreamProvider(string nameInArchive, OpenDelegate opener, CloseDelegate closer)
		{
			return ZipEntry.Create(nameInArchive, ZipEntrySource.JitStream, opener, closer);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0004D4A7 File Offset: 0x0004B6A7
		internal static ZipEntry CreateForZipOutputStream(string nameInArchive)
		{
			return ZipEntry.Create(nameInArchive, ZipEntrySource.ZipOutputStream, null, null);
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0004D4B4 File Offset: 0x0004B6B4
		private static ZipEntry Create(string nameInArchive, ZipEntrySource source, object arg1, object arg2)
		{
			if (string.IsNullOrEmpty(nameInArchive))
			{
				throw new ZipException("The entry name must be non-null and non-empty.");
			}
			ZipEntry zipEntry = new ZipEntry();
			zipEntry._VersionMadeBy = 45;
			zipEntry._Source = source;
			zipEntry._Mtime = (zipEntry._Atime = (zipEntry._Ctime = DateTime.UtcNow));
			if (source == ZipEntrySource.Stream)
			{
				zipEntry._sourceStream = (arg1 as Stream);
			}
			else if (source == ZipEntrySource.WriteDelegate)
			{
				zipEntry._WriteDelegate = (arg1 as WriteDelegate);
			}
			else if (source == ZipEntrySource.JitStream)
			{
				zipEntry._OpenDelegate = (arg1 as OpenDelegate);
				zipEntry._CloseDelegate = (arg2 as CloseDelegate);
			}
			else if (source != ZipEntrySource.ZipOutputStream)
			{
				if (source == ZipEntrySource.None)
				{
					zipEntry._Source = ZipEntrySource.FileSystem;
				}
				else
				{
					string text = arg1 as string;
					if (string.IsNullOrEmpty(text))
					{
						throw new ZipException("The filename must be non-null and non-empty.");
					}
					try
					{
						zipEntry._Mtime = File.GetLastWriteTime(text).ToUniversalTime();
						zipEntry._Ctime = File.GetCreationTime(text).ToUniversalTime();
						zipEntry._Atime = File.GetLastAccessTime(text).ToUniversalTime();
						if (File.Exists(text) || Directory.Exists(text))
						{
							zipEntry._ExternalFileAttrs = (int)File.GetAttributes(text);
						}
						zipEntry._ntfsTimesAreSet = true;
						zipEntry._LocalFileName = Path.GetFullPath(text);
					}
					catch (PathTooLongException innerException)
					{
						string message = string.Format("The path is too long, filename={0}", text);
						throw new ZipException(message, innerException);
					}
				}
			}
			zipEntry._LastModified = zipEntry._Mtime;
			zipEntry._FileNameInArchive = SharedUtilities.NormalizePathForUseInZipFile(nameInArchive);
			return zipEntry;
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0004D634 File Offset: 0x0004B834
		internal void MarkAsDirectory()
		{
			this._IsDirectory = true;
			if (!this._FileNameInArchive.EndsWith("/"))
			{
				this._FileNameInArchive += "/";
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x0004D665 File Offset: 0x0004B865
		// (set) Token: 0x06000D7A RID: 3450 RVA: 0x0004D66D File Offset: 0x0004B86D
		public bool IsText
		{
			get
			{
				return this._IsText;
			}
			set
			{
				this._IsText = value;
			}
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0004D676 File Offset: 0x0004B876
		public override string ToString()
		{
			return string.Format("ZipEntry::{0}", this.FileName);
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x0004D688 File Offset: 0x0004B888
		internal Stream ArchiveStream
		{
			get
			{
				if (this._archiveStream == null)
				{
					if (this._container.ZipFile != null)
					{
						ZipFile zipFile = this._container.ZipFile;
						zipFile.Reset(false);
						this._archiveStream = zipFile.StreamForDiskNumber(this._diskNumber);
					}
					else
					{
						this._archiveStream = this._container.ZipOutputStream.OutputStream;
					}
				}
				return this._archiveStream;
			}
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0004D6F0 File Offset: 0x0004B8F0
		private void SetFdpLoh()
		{
			long position = this.ArchiveStream.Position;
			try
			{
				this.ArchiveStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
			}
			catch (IOException innerException)
			{
				string message = string.Format("Exception seeking  entry({0}) offset(0x{1:X8}) len(0x{2:X8})", this.FileName, this._RelativeOffsetOfLocalHeader, this.ArchiveStream.Length);
				throw new BadStateException(message, innerException);
			}
			byte[] array = new byte[30];
			this.ArchiveStream.Read(array, 0, array.Length);
			short num = (short)((int)array[26] + (int)array[27] * 256);
			short num2 = (short)((int)array[28] + (int)array[29] * 256);
			this.ArchiveStream.Seek((long)(num + num2), SeekOrigin.Current);
			this._LengthOfHeader = (int)(30 + num2 + num) + ZipEntry.GetLengthOfCryptoHeaderBytes(this._Encryption_FromZipFile);
			this.__FileDataPosition = this._RelativeOffsetOfLocalHeader + (long)this._LengthOfHeader;
			this.ArchiveStream.Seek(position, SeekOrigin.Begin);
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0004D7F0 File Offset: 0x0004B9F0
		internal static int GetLengthOfCryptoHeaderBytes(EncryptionAlgorithm a)
		{
			if (a == EncryptionAlgorithm.None)
			{
				return 0;
			}
			if (a == EncryptionAlgorithm.PkzipWeak)
			{
				return 12;
			}
			throw new ZipException("internal error");
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x0004D808 File Offset: 0x0004BA08
		internal long FileDataPosition
		{
			get
			{
				if (this.__FileDataPosition == -1L)
				{
					this.SetFdpLoh();
				}
				return this.__FileDataPosition;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000D80 RID: 3456 RVA: 0x0004D820 File Offset: 0x0004BA20
		private int LengthOfHeader
		{
			get
			{
				if (this._LengthOfHeader == 0)
				{
					this.SetFdpLoh();
				}
				return this._LengthOfHeader;
			}
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0004D836 File Offset: 0x0004BA36
		public void Extract()
		{
			this.InternalExtract(".", null, null);
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0004D845 File Offset: 0x0004BA45
		public void Extract(ExtractExistingFileAction extractExistingFile)
		{
			this.ExtractExistingFile = extractExistingFile;
			this.InternalExtract(".", null, null);
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0004D85B File Offset: 0x0004BA5B
		public void Extract(Stream stream)
		{
			this.InternalExtract(null, stream, null);
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0004D866 File Offset: 0x0004BA66
		public void Extract(string baseDirectory)
		{
			this.InternalExtract(baseDirectory, null, null);
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0004D871 File Offset: 0x0004BA71
		public void Extract(string baseDirectory, ExtractExistingFileAction extractExistingFile)
		{
			this.ExtractExistingFile = extractExistingFile;
			this.InternalExtract(baseDirectory, null, null);
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0004D883 File Offset: 0x0004BA83
		public void ExtractWithPassword(string password)
		{
			this.InternalExtract(".", null, password);
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0004D892 File Offset: 0x0004BA92
		public void ExtractWithPassword(string baseDirectory, string password)
		{
			this.InternalExtract(baseDirectory, null, password);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0004D89D File Offset: 0x0004BA9D
		public void ExtractWithPassword(ExtractExistingFileAction extractExistingFile, string password)
		{
			this.ExtractExistingFile = extractExistingFile;
			this.InternalExtract(".", null, password);
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0004D8B3 File Offset: 0x0004BAB3
		public void ExtractWithPassword(string baseDirectory, ExtractExistingFileAction extractExistingFile, string password)
		{
			this.ExtractExistingFile = extractExistingFile;
			this.InternalExtract(baseDirectory, null, password);
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0004D8C5 File Offset: 0x0004BAC5
		public void ExtractWithPassword(Stream stream, string password)
		{
			this.InternalExtract(null, stream, password);
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0004D8D0 File Offset: 0x0004BAD0
		public CrcCalculatorStream OpenReader()
		{
			if (this._container.ZipFile == null)
			{
				throw new InvalidOperationException("Use OpenReader() only with ZipFile.");
			}
			return this.InternalOpenReader(this._Password ?? this._container.Password);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0004D905 File Offset: 0x0004BB05
		public CrcCalculatorStream OpenReader(string password)
		{
			if (this._container.ZipFile == null)
			{
				throw new InvalidOperationException("Use OpenReader() only with ZipFile.");
			}
			return this.InternalOpenReader(password);
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0004D928 File Offset: 0x0004BB28
		internal CrcCalculatorStream InternalOpenReader(string password)
		{
			this.ValidateCompression();
			this.ValidateEncryption();
			this.SetupCryptoForExtract(password);
			if (this._Source != ZipEntrySource.ZipFile)
			{
				throw new BadStateException("You must call ZipFile.Save before calling OpenReader");
			}
			long length = (this._CompressionMethod_FromZipFile == 0) ? this._CompressedFileDataSize : this.UncompressedSize;
			Stream archiveStream = this.ArchiveStream;
			this.ArchiveStream.Seek(this.FileDataPosition, SeekOrigin.Begin);
			this._inputDecryptorStream = this.GetExtractDecryptor(archiveStream);
			Stream extractDecompressor = this.GetExtractDecompressor(this._inputDecryptorStream);
			return new CrcCalculatorStream(extractDecompressor, length);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0004D9AE File Offset: 0x0004BBAE
		private void OnExtractProgress(long bytesWritten, long totalBytesToWrite)
		{
			if (this._container.ZipFile != null)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnExtractBlock(this, bytesWritten, totalBytesToWrite);
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0004D9D6 File Offset: 0x0004BBD6
		private void OnBeforeExtract(string path)
		{
			if (this._container.ZipFile != null && !this._container.ZipFile._inExtractAll)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnSingleEntryExtract(this, path, true);
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0004DA10 File Offset: 0x0004BC10
		private void OnAfterExtract(string path)
		{
			if (this._container.ZipFile != null && !this._container.ZipFile._inExtractAll)
			{
				this._container.ZipFile.OnSingleEntryExtract(this, path, false);
			}
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0004DA45 File Offset: 0x0004BC45
		private void OnExtractExisting(string path)
		{
			if (this._container.ZipFile != null)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnExtractExisting(this, path);
			}
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0004DA6C File Offset: 0x0004BC6C
		private static void ReallyDelete(string fileName)
		{
			if ((File.GetAttributes(fileName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				File.SetAttributes(fileName, FileAttributes.Normal);
			}
			File.Delete(fileName);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0004DA8A File Offset: 0x0004BC8A
		private void WriteStatus(string format, params object[] args)
		{
			if (this._container.ZipFile != null && this._container.ZipFile.Verbose)
			{
				this._container.ZipFile.StatusMessageTextWriter.WriteLine(format, args);
			}
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0004DAC4 File Offset: 0x0004BCC4
		private void InternalExtract(string baseDir, Stream outstream, string password)
		{
			if (this._container == null)
			{
				throw new BadStateException("This entry is an orphan");
			}
			if (this._container.ZipFile == null)
			{
				throw new InvalidOperationException("Use Extract() only with ZipFile.");
			}
			this._container.ZipFile.Reset(false);
			if (this._Source != ZipEntrySource.ZipFile)
			{
				throw new BadStateException("You must call ZipFile.Save before calling any Extract method");
			}
			this.OnBeforeExtract(baseDir);
			this._ioOperationCanceled = false;
			string text = null;
			Stream stream = null;
			bool flag = false;
			bool flag2 = false;
			try
			{
				this.ValidateCompression();
				this.ValidateEncryption();
				if (this.ValidateOutput(baseDir, outstream, out text))
				{
					this.WriteStatus("extract dir {0}...", new object[]
					{
						text
					});
					this.OnAfterExtract(baseDir);
				}
				else
				{
					if (text != null && File.Exists(text))
					{
						flag = true;
						int num = this.CheckExtractExistingFile(baseDir, text);
						if (num == 2)
						{
							goto IL_28F;
						}
						if (num == 1)
						{
							return;
						}
					}
					string text2 = password ?? (this._Password ?? this._container.Password);
					if (this._Encryption_FromZipFile != EncryptionAlgorithm.None)
					{
						if (text2 == null)
						{
							throw new BadPasswordException();
						}
						this.SetupCryptoForExtract(text2);
					}
					if (text != null)
					{
						this.WriteStatus("extract file {0}...", new object[]
						{
							text
						});
						text += ".tmp";
						string directoryName = Path.GetDirectoryName(text);
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
						else if (this._container.ZipFile != null)
						{
							flag2 = this._container.ZipFile._inExtractAll;
						}
						stream = new FileStream(text, FileMode.Create, FileAccess.ReadWrite);
					}
					else
					{
						this.WriteStatus("extract entry {0} to stream...", new object[]
						{
							this.FileName
						});
						stream = outstream;
					}
					if (!this._ioOperationCanceled)
					{
						int actualCrc = this.ExtractOne(stream);
						if (!this._ioOperationCanceled)
						{
							this.VerifyCrcAfterExtract(actualCrc);
							if (text != null)
							{
								stream.Close();
								stream = null;
								string text3 = text;
								string text4 = null;
								text = text3.Substring(0, text3.Length - 4);
								if (flag)
								{
									text4 = text + ".PendingOverwrite";
									File.Move(text, text4);
								}
								File.Move(text3, text);
								this._SetTimes(text, true);
								if (text4 != null && File.Exists(text4))
								{
									ZipEntry.ReallyDelete(text4);
								}
								if (flag2 && this.FileName.IndexOf('/') != -1)
								{
									string directoryName2 = Path.GetDirectoryName(this.FileName);
									if (this._container.ZipFile[directoryName2] == null)
									{
										this._SetTimes(Path.GetDirectoryName(text), false);
									}
								}
								if (((int)this._VersionMadeBy & 65280) == 2560 || ((int)this._VersionMadeBy & 65280) == 0)
								{
									File.SetAttributes(text, (FileAttributes)this._ExternalFileAttrs);
								}
							}
							this.OnAfterExtract(baseDir);
						}
					}
					IL_28F:;
				}
			}
			catch (Exception)
			{
				this._ioOperationCanceled = true;
				throw;
			}
			finally
			{
				if (this._ioOperationCanceled && text != null)
				{
					if (stream != null)
					{
						stream.Close();
					}
					if (File.Exists(text) && !flag)
					{
						File.Delete(text);
					}
				}
			}
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0004DDC8 File Offset: 0x0004BFC8
		internal void VerifyCrcAfterExtract(int actualCrc32)
		{
			if (actualCrc32 != this._Crc32)
			{
				throw new BadCrcException("CRC error: the file being extracted appears to be corrupted. " + string.Format("Expected 0x{0:X8}, Actual 0x{1:X8}", this._Crc32, actualCrc32));
			}
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0004DE00 File Offset: 0x0004C000
		private int CheckExtractExistingFile(string baseDir, string targetFileName)
		{
			int num = 0;
			while (true)
			{
				switch (this.ExtractExistingFile)
				{
				case ExtractExistingFileAction.OverwriteSilently:
					goto IL_21;
				case ExtractExistingFileAction.DoNotOverwrite:
					goto IL_3A;
				case ExtractExistingFileAction.InvokeExtractProgressEvent:
					if (num > 0)
					{
						goto Block_2;
					}
					this.OnExtractExisting(baseDir);
					if (this._ioOperationCanceled)
					{
						return 2;
					}
					num++;
					continue;
				}
				break;
			}
			goto IL_85;
			IL_21:
			this.WriteStatus("the file {0} exists; will overwrite it...", new object[]
			{
				targetFileName
			});
			return 0;
			IL_3A:
			this.WriteStatus("the file {0} exists; not extracting entry...", new object[]
			{
				this.FileName
			});
			this.OnAfterExtract(baseDir);
			return 1;
			Block_2:
			throw new ZipException(string.Format("The file {0} already exists.", targetFileName));
			IL_85:
			throw new ZipException(string.Format("The file {0} already exists.", targetFileName));
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0004DEAB File Offset: 0x0004C0AB
		private void _CheckRead(int nbytes)
		{
			if (nbytes == 0)
			{
				throw new BadReadException(string.Format("bad read of entry {0} from compressed archive.", this.FileName));
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0004DEC8 File Offset: 0x0004C0C8
		private int ExtractOne(Stream output)
		{
			int result = 0;
			Stream archiveStream = this.ArchiveStream;
			try
			{
				archiveStream.Seek(this.FileDataPosition, SeekOrigin.Begin);
				byte[] array = new byte[this.BufferSize];
				long num = (this._CompressionMethod_FromZipFile != 0) ? this.UncompressedSize : this._CompressedFileDataSize;
				this._inputDecryptorStream = this.GetExtractDecryptor(archiveStream);
				Stream extractDecompressor = this.GetExtractDecompressor(this._inputDecryptorStream);
				long num2 = 0L;
				using (CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(extractDecompressor))
				{
					while (num > 0L)
					{
						int count = (num > (long)array.Length) ? array.Length : ((int)num);
						int num3 = crcCalculatorStream.Read(array, 0, count);
						this._CheckRead(num3);
						output.Write(array, 0, num3);
						num -= (long)num3;
						num2 += (long)num3;
						this.OnExtractProgress(num2, this.UncompressedSize);
						if (this._ioOperationCanceled)
						{
							break;
						}
					}
					result = crcCalculatorStream.Crc;
				}
			}
			finally
			{
				ZipSegmentedStream zipSegmentedStream = archiveStream as ZipSegmentedStream;
				if (zipSegmentedStream != null)
				{
					zipSegmentedStream.Dispose();
					this._archiveStream = null;
				}
			}
			return result;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0004DFE4 File Offset: 0x0004C1E4
		internal Stream GetExtractDecompressor(Stream input2)
		{
			short compressionMethod_FromZipFile = this._CompressionMethod_FromZipFile;
			if (compressionMethod_FromZipFile == 0)
			{
				return input2;
			}
			if (compressionMethod_FromZipFile != 8)
			{
				return null;
			}
			return new DeflateStream(input2, CompressionMode.Decompress, true);
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0004E010 File Offset: 0x0004C210
		internal Stream GetExtractDecryptor(Stream input)
		{
			Stream result;
			if (this._Encryption_FromZipFile == EncryptionAlgorithm.PkzipWeak)
			{
				result = new ZipCipherStream(input, this._zipCrypto_forExtract, CryptoMode.Decrypt);
			}
			else
			{
				result = input;
			}
			return result;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0004E03C File Offset: 0x0004C23C
		internal void _SetTimes(string fileOrDirectory, bool isFile)
		{
			try
			{
				if (this._ntfsTimesAreSet)
				{
					if (isFile)
					{
						if (File.Exists(fileOrDirectory))
						{
							File.SetCreationTimeUtc(fileOrDirectory, this._Ctime);
							File.SetLastAccessTimeUtc(fileOrDirectory, this._Atime);
							File.SetLastWriteTimeUtc(fileOrDirectory, this._Mtime);
						}
					}
					else if (Directory.Exists(fileOrDirectory))
					{
						Directory.SetCreationTimeUtc(fileOrDirectory, this._Ctime);
						Directory.SetLastAccessTimeUtc(fileOrDirectory, this._Atime);
						Directory.SetLastWriteTimeUtc(fileOrDirectory, this._Mtime);
					}
				}
				else
				{
					DateTime lastWriteTime = SharedUtilities.AdjustTime_Reverse(this.LastModified);
					if (isFile)
					{
						File.SetLastWriteTime(fileOrDirectory, lastWriteTime);
					}
					else
					{
						Directory.SetLastWriteTime(fileOrDirectory, lastWriteTime);
					}
				}
			}
			catch (IOException ex)
			{
				this.WriteStatus("failed to set time on {0}: {1}", new object[]
				{
					fileOrDirectory,
					ex.Message
				});
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x0004E104 File Offset: 0x0004C304
		private string UnsupportedAlgorithm
		{
			get
			{
				string result = string.Empty;
				uint unsupportedAlgorithmId = this._UnsupportedAlgorithmId;
				if (unsupportedAlgorithmId <= 26128u)
				{
					if (unsupportedAlgorithmId <= 26115u)
					{
						if (unsupportedAlgorithmId == 0u)
						{
							result = "--";
							return result;
						}
						switch (unsupportedAlgorithmId)
						{
						case 26113u:
							result = "DES";
							return result;
						case 26114u:
							result = "RC2";
							return result;
						case 26115u:
							result = "3DES-168";
							return result;
						}
					}
					else
					{
						if (unsupportedAlgorithmId == 26121u)
						{
							result = "3DES-112";
							return result;
						}
						switch (unsupportedAlgorithmId)
						{
						case 26126u:
							result = "PKWare AES128";
							return result;
						case 26127u:
							result = "PKWare AES192";
							return result;
						case 26128u:
							result = "PKWare AES256";
							return result;
						}
					}
				}
				else if (unsupportedAlgorithmId <= 26401u)
				{
					if (unsupportedAlgorithmId == 26370u)
					{
						result = "RC2";
						return result;
					}
					switch (unsupportedAlgorithmId)
					{
					case 26400u:
						result = "Blowfish";
						return result;
					case 26401u:
						result = "Twofish";
						return result;
					}
				}
				else
				{
					if (unsupportedAlgorithmId == 26625u)
					{
						result = "RC4";
						return result;
					}
					if (unsupportedAlgorithmId != 65535u)
					{
					}
				}
				result = string.Format("Unknown (0x{0:X4})", this._UnsupportedAlgorithmId);
				return result;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x0004E224 File Offset: 0x0004C424
		private string UnsupportedCompressionMethod
		{
			get
			{
				string result = string.Empty;
				int compressionMethod = (int)this._CompressionMethod;
				if (compressionMethod <= 14)
				{
					switch (compressionMethod)
					{
					case 0:
						result = "Store";
						return result;
					case 1:
						result = "Shrink";
						return result;
					default:
						switch (compressionMethod)
						{
						case 8:
							result = "DEFLATE";
							return result;
						case 9:
							result = "Deflate64";
							return result;
						case 12:
							result = "BZIP2";
							return result;
						case 14:
							result = "LZMA";
							return result;
						}
						break;
					}
				}
				else
				{
					if (compressionMethod == 19)
					{
						result = "LZ77";
						return result;
					}
					if (compressionMethod == 98)
					{
						result = "PPMd";
						return result;
					}
				}
				result = string.Format("Unknown (0x{0:X4})", this._CompressionMethod);
				return result;
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0004E2DC File Offset: 0x0004C4DC
		internal void ValidateEncryption()
		{
			if (this.Encryption == EncryptionAlgorithm.PkzipWeak || this.Encryption == EncryptionAlgorithm.None)
			{
				return;
			}
			if (this._UnsupportedAlgorithmId != 0u)
			{
				throw new ZipException(string.Format("Cannot extract: Entry {0} is encrypted with an algorithm not supported by DotNetZip: {1}", this.FileName, this.UnsupportedAlgorithm));
			}
			throw new ZipException(string.Format("Cannot extract: Entry {0} uses an unsupported encryption algorithm ({1:X2})", this.FileName, (int)this.Encryption));
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0004E33F File Offset: 0x0004C53F
		private void ValidateCompression()
		{
			if (this._CompressionMethod_FromZipFile != 0 && this._CompressionMethod_FromZipFile != 8)
			{
				throw new ZipException(string.Format("Entry {0} uses an unsupported compression method (0x{1:X2}, {2})", this.FileName, this._CompressionMethod_FromZipFile, this.UnsupportedCompressionMethod));
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0004E37C File Offset: 0x0004C57C
		private void SetupCryptoForExtract(string password)
		{
			if (this._Encryption_FromZipFile == EncryptionAlgorithm.None)
			{
				return;
			}
			if (this._Encryption_FromZipFile == EncryptionAlgorithm.PkzipWeak)
			{
				if (password == null)
				{
					throw new ZipException("Missing password.");
				}
				this.ArchiveStream.Seek(this.FileDataPosition - 12L, SeekOrigin.Begin);
				this._zipCrypto_forExtract = ZipCrypto.ForRead(password, this);
			}
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0004E3D0 File Offset: 0x0004C5D0
		private bool ValidateOutput(string basedir, Stream outstream, out string outFileName)
		{
			if (basedir != null)
			{
				string text = this.FileName.Replace("\\", "/");
				if (text.IndexOf(':') == 1)
				{
					text = text.Substring(2);
				}
				if (text.StartsWith("/"))
				{
					text = text.Substring(1);
				}
				if (this._container.ZipFile.FlattenFoldersOnExtract)
				{
					outFileName = Path.Combine(basedir, (text.IndexOf('/') != -1) ? Path.GetFileName(text) : text);
				}
				else
				{
					outFileName = Path.Combine(basedir, text);
				}
				outFileName = outFileName.Replace("/", "\\");
				if (this.IsDirectory || this.FileName.EndsWith("/"))
				{
					if (!Directory.Exists(outFileName))
					{
						Directory.CreateDirectory(outFileName);
						this._SetTimes(outFileName, false);
					}
					else if (this.ExtractExistingFile == ExtractExistingFileAction.OverwriteSilently)
					{
						this._SetTimes(outFileName, false);
					}
					return true;
				}
				return false;
			}
			else
			{
				if (outstream != null)
				{
					outFileName = null;
					return this.IsDirectory || this.FileName.EndsWith("/");
				}
				throw new ArgumentNullException("outstream");
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0004E4E8 File Offset: 0x0004C6E8
		private void ReadExtraField()
		{
			this._readExtraDepth++;
			long position = this.ArchiveStream.Position;
			this.ArchiveStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
			byte[] array = new byte[30];
			this.ArchiveStream.Read(array, 0, array.Length);
			int num = 26;
			short num2 = (short)((int)array[num++] + (int)array[num++] * 256);
			short extraFieldLength = (short)((int)array[num++] + (int)array[num++] * 256);
			this.ArchiveStream.Seek((long)num2, SeekOrigin.Current);
			this.ProcessExtraField(this.ArchiveStream, extraFieldLength);
			this.ArchiveStream.Seek(position, SeekOrigin.Begin);
			this._readExtraDepth--;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0004E5A8 File Offset: 0x0004C7A8
		private static bool ReadHeader(ZipEntry ze, Encoding defaultEncoding)
		{
			int num = 0;
			ze._RelativeOffsetOfLocalHeader = ze.ArchiveStream.Position;
			int num2 = SharedUtilities.ReadEntrySignature(ze.ArchiveStream);
			num += 4;
			if (ZipEntry.IsNotValidSig(num2))
			{
				ze.ArchiveStream.Seek(-4L, SeekOrigin.Current);
				if (ZipEntry.IsNotValidZipDirEntrySig(num2) && (long)num2 != 101010256L)
				{
					throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) at position  0x{1:X8}", num2, ze.ArchiveStream.Position));
				}
				return false;
			}
			else
			{
				byte[] array = new byte[26];
				int num3 = ze.ArchiveStream.Read(array, 0, array.Length);
				if (num3 != array.Length)
				{
					return false;
				}
				num += num3;
				int num4 = 0;
				ze._VersionNeeded = (short)((int)array[num4++] + (int)array[num4++] * 256);
				ze._BitField = (short)((int)array[num4++] + (int)array[num4++] * 256);
				ze._CompressionMethod_FromZipFile = (ze._CompressionMethod = (short)((int)array[num4++] + (int)array[num4++] * 256));
				ze._TimeBlob = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
				ze._LastModified = SharedUtilities.PackedToDateTime(ze._TimeBlob);
				ze._timestamp |= ZipEntryTimestamp.DOS;
				if ((ze._BitField & 1) == 1)
				{
					ze._Encryption_FromZipFile = (ze._Encryption = EncryptionAlgorithm.PkzipWeak);
					ze._sourceIsEncrypted = true;
				}
				ze._Crc32 = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
				ze._CompressedSize = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
				ze._UncompressedSize = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
				if ((uint)ze._CompressedSize == 4294967295u || (uint)ze._UncompressedSize == 4294967295u)
				{
					ze._InputUsesZip64 = true;
				}
				short num5 = (short)((int)array[num4++] + (int)array[num4++] * 256);
				short extraFieldLength = (short)((int)array[num4++] + (int)array[num4++] * 256);
				array = new byte[(int)num5];
				num3 = ze.ArchiveStream.Read(array, 0, array.Length);
				num += num3;
				if ((ze._BitField & 2048) == 2048)
				{
					ze.AlternateEncoding = Encoding.UTF8;
					ze.AlternateEncodingUsage = ZipOption.Always;
				}
				ze._FileNameInArchive = ze.AlternateEncoding.GetString(array, 0, array.Length);
				if (ze._FileNameInArchive.EndsWith("/"))
				{
					ze.MarkAsDirectory();
				}
				num += ze.ProcessExtraField(ze.ArchiveStream, extraFieldLength);
				ze._LengthOfTrailer = 0;
				if (!ze._FileNameInArchive.EndsWith("/") && (ze._BitField & 8) == 8)
				{
					long position = ze.ArchiveStream.Position;
					bool flag = true;
					long num6 = 0L;
					int num7 = 0;
					while (flag)
					{
						num7++;
						if (ze._container.ZipFile != null)
						{
							ze._container.ZipFile.OnReadBytes(ze);
						}
						long num8 = SharedUtilities.FindSignature(ze.ArchiveStream, 134695760);
						if (num8 == -1L)
						{
							return false;
						}
						num6 += num8;
						if (ze._InputUsesZip64)
						{
							array = new byte[20];
							num3 = ze.ArchiveStream.Read(array, 0, array.Length);
							if (num3 != 20)
							{
								return false;
							}
							num4 = 0;
							ze._Crc32 = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
							ze._CompressedSize = BitConverter.ToInt64(array, num4);
							num4 += 8;
							ze._UncompressedSize = BitConverter.ToInt64(array, num4);
							num4 += 8;
							ze._LengthOfTrailer += 24;
						}
						else
						{
							array = new byte[12];
							num3 = ze.ArchiveStream.Read(array, 0, array.Length);
							if (num3 != 12)
							{
								return false;
							}
							num4 = 0;
							ze._Crc32 = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
							ze._CompressedSize = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
							ze._UncompressedSize = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
							ze._LengthOfTrailer += 16;
						}
						flag = (num6 != ze._CompressedSize);
						if (flag)
						{
							ze.ArchiveStream.Seek(-12L, SeekOrigin.Current);
							num6 += 4L;
						}
					}
					ze.ArchiveStream.Seek(position, SeekOrigin.Begin);
				}
				ze._CompressedFileDataSize = ze._CompressedSize;
				if ((ze._BitField & 1) == 1)
				{
					ze._WeakEncryptionHeader = new byte[12];
					num += ZipEntry.ReadWeakEncryptionHeader(ze._archiveStream, ze._WeakEncryptionHeader);
					ze._CompressedFileDataSize -= 12L;
				}
				ze._LengthOfHeader = num;
				ze._TotalEntrySize = (long)ze._LengthOfHeader + ze._CompressedFileDataSize + (long)ze._LengthOfTrailer;
				return true;
			}
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0004EC34 File Offset: 0x0004CE34
		internal static int ReadWeakEncryptionHeader(Stream s, byte[] buffer)
		{
			int num = s.Read(buffer, 0, 12);
			if (num != 12)
			{
				throw new ZipException(string.Format("Unexpected end of data at position 0x{0:X8}", s.Position));
			}
			return num;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0004EC6D File Offset: 0x0004CE6D
		private static bool IsNotValidSig(int signature)
		{
			return signature != 67324752;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0004EC7C File Offset: 0x0004CE7C
		internal static ZipEntry ReadEntry(ZipContainer zc, bool first)
		{
			ZipFile zipFile = zc.ZipFile;
			Stream readStream = zc.ReadStream;
			Encoding alternateEncoding = zc.AlternateEncoding;
			ZipEntry zipEntry = new ZipEntry();
			zipEntry._Source = ZipEntrySource.ZipFile;
			zipEntry._container = zc;
			zipEntry._archiveStream = readStream;
			if (zipFile != null)
			{
				zipFile.OnReadEntry(true, null);
			}
			if (first)
			{
				ZipEntry.HandlePK00Prefix(readStream);
			}
			if (!ZipEntry.ReadHeader(zipEntry, alternateEncoding))
			{
				return null;
			}
			zipEntry.__FileDataPosition = zipEntry.ArchiveStream.Position;
			readStream.Seek(zipEntry._CompressedFileDataSize + (long)zipEntry._LengthOfTrailer, SeekOrigin.Current);
			ZipEntry.HandleUnexpectedDataDescriptor(zipEntry);
			if (zipFile != null)
			{
				zipFile.OnReadBytes(zipEntry);
				zipFile.OnReadEntry(false, zipEntry);
			}
			return zipEntry;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0004ED18 File Offset: 0x0004CF18
		internal static void HandlePK00Prefix(Stream s)
		{
			uint num = (uint)SharedUtilities.ReadInt(s);
			if (num != 808471376u)
			{
				s.Seek(-4L, SeekOrigin.Current);
			}
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0004ED40 File Offset: 0x0004CF40
		private static void HandleUnexpectedDataDescriptor(ZipEntry entry)
		{
			Stream archiveStream = entry.ArchiveStream;
			uint num = (uint)SharedUtilities.ReadInt(archiveStream);
			if ((ulong)num != (ulong)((long)entry._Crc32))
			{
				archiveStream.Seek(-4L, SeekOrigin.Current);
				return;
			}
			int num2 = SharedUtilities.ReadInt(archiveStream);
			if ((long)num2 != entry._CompressedSize)
			{
				archiveStream.Seek(-8L, SeekOrigin.Current);
				return;
			}
			num2 = SharedUtilities.ReadInt(archiveStream);
			if ((long)num2 == entry._UncompressedSize)
			{
				return;
			}
			archiveStream.Seek(-12L, SeekOrigin.Current);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0004EDAC File Offset: 0x0004CFAC
		internal static int FindExtraFieldSegment(byte[] extra, int offx, ushort targetHeaderId)
		{
			int num = offx;
			while (num + 3 < extra.Length)
			{
				ushort num2 = (ushort)((int)extra[num++] + (int)extra[num++] * 256);
				if (num2 == targetHeaderId)
				{
					return num - 2;
				}
				short num3 = (short)((int)extra[num++] + (int)extra[num++] * 256);
				num += (int)num3;
			}
			return -1;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0004EE00 File Offset: 0x0004D000
		internal int ProcessExtraField(Stream s, short extraFieldLength)
		{
			int num = 0;
			if (extraFieldLength > 0)
			{
				byte[] array = this._Extra = new byte[(int)extraFieldLength];
				num = s.Read(array, 0, array.Length);
				long posn = s.Position - (long)num;
				int num2 = 0;
				while (num2 + 3 < array.Length)
				{
					int num3 = num2;
					ushort num4 = (ushort)((int)array[num2++] + (int)array[num2++] * 256);
					short num5 = (short)((int)array[num2++] + (int)array[num2++] * 256);
					ushort num6 = num4;
					if (num6 <= 23)
					{
						if (num6 != 1)
						{
							if (num6 != 10)
							{
								if (num6 == 23)
								{
									num2 = this.ProcessExtraFieldPkwareStrongEncryption(array, num2);
								}
							}
							else
							{
								num2 = this.ProcessExtraFieldWindowsTimes(array, num2, num5, posn);
							}
						}
						else
						{
							num2 = this.ProcessExtraFieldZip64(array, num2, num5, posn);
						}
					}
					else if (num6 <= 22613)
					{
						if (num6 != 21589)
						{
							if (num6 == 22613)
							{
								num2 = this.ProcessExtraFieldInfoZipTimes(array, num2, num5, posn);
							}
						}
						else
						{
							num2 = this.ProcessExtraFieldUnixTimes(array, num2, num5, posn);
						}
					}
					else if (num6 != 30805 && num6 != 30837)
					{
					}
					num2 = num3 + (int)num5 + 4;
				}
			}
			return num;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0004EF1C File Offset: 0x0004D11C
		private int ProcessExtraFieldPkwareStrongEncryption(byte[] Buffer, int j)
		{
			j += 2;
			this._UnsupportedAlgorithmId = (uint)((ushort)((int)Buffer[j++] + (int)Buffer[j++] * 256));
			this._Encryption_FromZipFile = (this._Encryption = EncryptionAlgorithm.Unsupported);
			return j;
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0004EFC8 File Offset: 0x0004D1C8
		private int ProcessExtraFieldZip64(byte[] buffer, int j, short dataSize, long posn)
		{
			this._InputUsesZip64 = true;
			if (dataSize > 28)
			{
				throw new BadReadException(string.Format("  Inconsistent size (0x{0:X4}) for ZIP64 extra field at position 0x{1:X16}", dataSize, posn));
			}
			int remainingData = (int)dataSize;
			ZipEntry.Func<long> func = delegate
			{
				if (remainingData < 8)
				{
					throw new BadReadException(string.Format("  Missing data for ZIP64 extra field, position 0x{0:X16}", posn));
				}
				long result = BitConverter.ToInt64(buffer, j);
				j += 8;
				remainingData -= 8;
				return result;
			};
			if (this._UncompressedSize == (long)((ulong)-1))
			{
				this._UncompressedSize = func();
			}
			if (this._CompressedSize == (long)((ulong)-1))
			{
				this._CompressedSize = func();
			}
			if (this._RelativeOffsetOfLocalHeader == (long)((ulong)-1))
			{
				this._RelativeOffsetOfLocalHeader = func();
			}
			return j;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0004F07C File Offset: 0x0004D27C
		private int ProcessExtraFieldInfoZipTimes(byte[] buffer, int j, short dataSize, long posn)
		{
			if (dataSize != 12 && dataSize != 8)
			{
				throw new BadReadException(string.Format("  Unexpected size (0x{0:X4}) for InfoZip v1 extra field at position 0x{1:X16}", dataSize, posn));
			}
			int num = BitConverter.ToInt32(buffer, j);
			this._Mtime = ZipEntry._unixEpoch.AddSeconds((double)num);
			j += 4;
			num = BitConverter.ToInt32(buffer, j);
			this._Atime = ZipEntry._unixEpoch.AddSeconds((double)num);
			j += 4;
			this._Ctime = DateTime.UtcNow;
			this._ntfsTimesAreSet = true;
			this._timestamp |= ZipEntryTimestamp.InfoZip1;
			return j;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0004F160 File Offset: 0x0004D360
		private int ProcessExtraFieldUnixTimes(byte[] buffer, int j, short dataSize, long posn)
		{
			if (dataSize != 13 && dataSize != 9 && dataSize != 5)
			{
				throw new BadReadException(string.Format("  Unexpected size (0x{0:X4}) for Extended Timestamp extra field at position 0x{1:X16}", dataSize, posn));
			}
			int remainingData = (int)dataSize;
			ZipEntry.Func<DateTime> func = delegate
			{
				int num = BitConverter.ToInt32(buffer, j);
				j += 4;
				remainingData -= 4;
				return ZipEntry._unixEpoch.AddSeconds((double)num);
			};
			if (dataSize == 13 || this._readExtraDepth > 0)
			{
				byte b = buffer[j++];
				remainingData--;
				if ((b & 1) != 0 && remainingData >= 4)
				{
					this._Mtime = func();
				}
				this._Atime = (((b & 2) != 0 && remainingData >= 4) ? func() : DateTime.UtcNow);
				this._Ctime = (((b & 4) != 0 && remainingData >= 4) ? func() : DateTime.UtcNow);
				this._timestamp |= ZipEntryTimestamp.Unix;
				this._ntfsTimesAreSet = true;
				this._emitUnixTimes = true;
			}
			else
			{
				this.ReadExtraField();
			}
			return j;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0004F280 File Offset: 0x0004D480
		private int ProcessExtraFieldWindowsTimes(byte[] buffer, int j, short dataSize, long posn)
		{
			if (dataSize != 32)
			{
				throw new BadReadException(string.Format("  Unexpected size (0x{0:X4}) for NTFS times extra field at position 0x{1:X16}", dataSize, posn));
			}
			j += 4;
			short num = (short)((int)buffer[j] + (int)buffer[j + 1] * 256);
			short num2 = (short)((int)buffer[j + 2] + (int)buffer[j + 3] * 256);
			j += 4;
			if (num == 1 && num2 == 24)
			{
				long fileTime = BitConverter.ToInt64(buffer, j);
				this._Mtime = DateTime.FromFileTimeUtc(fileTime);
				j += 8;
				fileTime = BitConverter.ToInt64(buffer, j);
				this._Atime = DateTime.FromFileTimeUtc(fileTime);
				j += 8;
				fileTime = BitConverter.ToInt64(buffer, j);
				this._Ctime = DateTime.FromFileTimeUtc(fileTime);
				j += 8;
				this._ntfsTimesAreSet = true;
				this._timestamp |= ZipEntryTimestamp.Windows;
				this._emitNtfsTimes = true;
			}
			return j;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0004F350 File Offset: 0x0004D550
		internal void WriteCentralDirectoryEntry(Stream s)
		{
			byte[] array = new byte[4096];
			int num = 0;
			array[num++] = 80;
			array[num++] = 75;
			array[num++] = 1;
			array[num++] = 2;
			array[num++] = (byte)(this._VersionMadeBy & 255);
			array[num++] = (byte)(((int)this._VersionMadeBy & 65280) >> 8);
			short num2 = (this.VersionNeeded != 0) ? this.VersionNeeded : 20;
			if (!this._OutputUsesZip64.HasValue)
			{
				this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always);
			}
			short num3 = this._OutputUsesZip64.Value ? 45 : num2;
			array[num++] = (byte)(num3 & 255);
			array[num++] = (byte)(((int)num3 & 65280) >> 8);
			array[num++] = (byte)(this._BitField & 255);
			array[num++] = (byte)(((int)this._BitField & 65280) >> 8);
			array[num++] = (byte)(this._CompressionMethod & 255);
			array[num++] = (byte)(((int)this._CompressionMethod & 65280) >> 8);
			array[num++] = (byte)(this._TimeBlob & 255);
			array[num++] = (byte)((this._TimeBlob & 65280) >> 8);
			array[num++] = (byte)((this._TimeBlob & 16711680) >> 16);
			array[num++] = (byte)(((long)this._TimeBlob & (long)((ulong)-16777216)) >> 24);
			array[num++] = (byte)(this._Crc32 & 255);
			array[num++] = (byte)((this._Crc32 & 65280) >> 8);
			array[num++] = (byte)((this._Crc32 & 16711680) >> 16);
			array[num++] = (byte)(((long)this._Crc32 & (long)((ulong)-16777216)) >> 24);
			if (this._OutputUsesZip64.Value)
			{
				for (int i = 0; i < 8; i++)
				{
					array[num++] = 255;
				}
			}
			else
			{
				array[num++] = (byte)(this._CompressedSize & 255L);
				array[num++] = (byte)((this._CompressedSize & 65280L) >> 8);
				array[num++] = (byte)((this._CompressedSize & 16711680L) >> 16);
				array[num++] = (byte)((this._CompressedSize & (long)((ulong)-16777216)) >> 24);
				array[num++] = (byte)(this._UncompressedSize & 255L);
				array[num++] = (byte)((this._UncompressedSize & 65280L) >> 8);
				array[num++] = (byte)((this._UncompressedSize & 16711680L) >> 16);
				array[num++] = (byte)((this._UncompressedSize & (long)((ulong)-16777216)) >> 24);
			}
			byte[] encodedFileNameBytes = this.GetEncodedFileNameBytes();
			short num4 = (short)encodedFileNameBytes.Length;
			array[num++] = (byte)(num4 & 255);
			array[num++] = (byte)(((int)num4 & 65280) >> 8);
			this._presumeZip64 = this._OutputUsesZip64.Value;
			this._Extra = this.ConstructExtraField(true);
			short num5 = (short)((this._Extra == null) ? 0 : this._Extra.Length);
			array[num++] = (byte)(num5 & 255);
			array[num++] = (byte)(((int)num5 & 65280) >> 8);
			int num6 = (this._CommentBytes == null) ? 0 : this._CommentBytes.Length;
			if (num6 + num > array.Length)
			{
				num6 = array.Length - num;
			}
			array[num++] = (byte)(num6 & 255);
			array[num++] = (byte)((num6 & 65280) >> 8);
			bool flag = this._container.ZipFile != null && this._container.ZipFile.MaxOutputSegmentSize != 0;
			if (flag)
			{
				array[num++] = (byte)(this._diskNumber & 255u);
				array[num++] = (byte)((this._diskNumber & 65280u) >> 8);
			}
			else
			{
				array[num++] = 0;
				array[num++] = 0;
			}
			array[num++] = (this._IsText ? 1 : 0);
			array[num++] = 0;
			array[num++] = (byte)(this._ExternalFileAttrs & 255);
			array[num++] = (byte)((this._ExternalFileAttrs & 65280) >> 8);
			array[num++] = (byte)((this._ExternalFileAttrs & 16711680) >> 16);
			array[num++] = (byte)(((long)this._ExternalFileAttrs & (long)((ulong)-16777216)) >> 24);
			if (this._RelativeOffsetOfLocalHeader > (long)((ulong)-1))
			{
				array[num++] = 255;
				array[num++] = 255;
				array[num++] = 255;
				array[num++] = 255;
			}
			else
			{
				array[num++] = (byte)(this._RelativeOffsetOfLocalHeader & 255L);
				array[num++] = (byte)((this._RelativeOffsetOfLocalHeader & 65280L) >> 8);
				array[num++] = (byte)((this._RelativeOffsetOfLocalHeader & 16711680L) >> 16);
				array[num++] = (byte)((this._RelativeOffsetOfLocalHeader & (long)((ulong)-16777216)) >> 24);
			}
			Buffer.BlockCopy(encodedFileNameBytes, 0, array, num, (int)num4);
			num += (int)num4;
			if (this._Extra != null)
			{
				byte[] extra = this._Extra;
				int srcOffset = 0;
				Buffer.BlockCopy(extra, srcOffset, array, num, (int)num5);
				num += (int)num5;
			}
			if (num6 != 0)
			{
				Buffer.BlockCopy(this._CommentBytes, 0, array, num, num6);
				num += num6;
			}
			s.Write(array, 0, num);
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0004F8B4 File Offset: 0x0004DAB4
		private byte[] ConstructExtraField(bool forCentralDirectory)
		{
			List<byte[]> list = new List<byte[]>();
			if (this._container.Zip64 == Zip64Option.Always || (this._container.Zip64 == Zip64Option.AsNecessary && (!forCentralDirectory || this._entryRequiresZip64.Value)))
			{
				int num = 4 + (forCentralDirectory ? 28 : 16);
				byte[] array = new byte[num];
				int num2 = 0;
				if (this._presumeZip64 || forCentralDirectory)
				{
					array[num2++] = 1;
					array[num2++] = 0;
				}
				else
				{
					array[num2++] = 153;
					array[num2++] = 153;
				}
				array[num2++] = (byte)(num - 4);
				array[num2++] = 0;
				Array.Copy(BitConverter.GetBytes(this._UncompressedSize), 0, array, num2, 8);
				num2 += 8;
				Array.Copy(BitConverter.GetBytes(this._CompressedSize), 0, array, num2, 8);
				num2 += 8;
				if (forCentralDirectory)
				{
					Array.Copy(BitConverter.GetBytes(this._RelativeOffsetOfLocalHeader), 0, array, num2, 8);
					num2 += 8;
					Array.Copy(BitConverter.GetBytes(0), 0, array, num2, 4);
				}
				list.Add(array);
			}
			if (this._ntfsTimesAreSet && this._emitNtfsTimes)
			{
				byte[] array = new byte[36];
				int num3 = 0;
				array[num3++] = 10;
				array[num3++] = 0;
				array[num3++] = 32;
				array[num3++] = 0;
				num3 += 4;
				array[num3++] = 1;
				array[num3++] = 0;
				array[num3++] = 24;
				array[num3++] = 0;
				long value = this._Mtime.ToFileTime();
				Array.Copy(BitConverter.GetBytes(value), 0, array, num3, 8);
				num3 += 8;
				value = this._Atime.ToFileTime();
				Array.Copy(BitConverter.GetBytes(value), 0, array, num3, 8);
				num3 += 8;
				value = this._Ctime.ToFileTime();
				Array.Copy(BitConverter.GetBytes(value), 0, array, num3, 8);
				num3 += 8;
				list.Add(array);
			}
			if (this._ntfsTimesAreSet && this._emitUnixTimes)
			{
				int num4 = 9;
				if (!forCentralDirectory)
				{
					num4 += 8;
				}
				byte[] array = new byte[num4];
				int num5 = 0;
				array[num5++] = 85;
				array[num5++] = 84;
				array[num5++] = (byte)(num4 - 4);
				array[num5++] = 0;
				array[num5++] = 7;
				int value2 = (int)(this._Mtime - ZipEntry._unixEpoch).TotalSeconds;
				Array.Copy(BitConverter.GetBytes(value2), 0, array, num5, 4);
				num5 += 4;
				if (!forCentralDirectory)
				{
					value2 = (int)(this._Atime - ZipEntry._unixEpoch).TotalSeconds;
					Array.Copy(BitConverter.GetBytes(value2), 0, array, num5, 4);
					num5 += 4;
					value2 = (int)(this._Ctime - ZipEntry._unixEpoch).TotalSeconds;
					Array.Copy(BitConverter.GetBytes(value2), 0, array, num5, 4);
					num5 += 4;
				}
				list.Add(array);
			}
			byte[] array2 = null;
			if (list.Count > 0)
			{
				int num6 = 0;
				int num7 = 0;
				for (int i = 0; i < list.Count; i++)
				{
					num6 += list[i].Length;
				}
				array2 = new byte[num6];
				for (int i = 0; i < list.Count; i++)
				{
					Array.Copy(list[i], 0, array2, num7, list[i].Length);
					num7 += list[i].Length;
				}
			}
			return array2;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0004FC30 File Offset: 0x0004DE30
		private string NormalizeFileName()
		{
			string text = this.FileName.Replace("\\", "/");
			string result;
			if (this._TrimVolumeFromFullyQualifiedPaths && this.FileName.Length >= 3 && this.FileName[1] == ':' && text[2] == '/')
			{
				result = text.Substring(3);
			}
			else if (this.FileName.Length >= 4 && text[0] == '/' && text[1] == '/')
			{
				int num = text.IndexOf('/', 2);
				if (num == -1)
				{
					throw new ArgumentException("The path for that entry appears to be badly formatted");
				}
				result = text.Substring(num + 1);
			}
			else if (this.FileName.Length >= 3 && text[0] == '.' && text[1] == '/')
			{
				result = text.Substring(2);
			}
			else
			{
				result = text;
			}
			return result;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0004FD0C File Offset: 0x0004DF0C
		private byte[] GetEncodedFileNameBytes()
		{
			string text = this.NormalizeFileName();
			switch (this.AlternateEncodingUsage)
			{
			case ZipOption.Default:
				if (this._Comment != null && this._Comment.Length != 0)
				{
					this._CommentBytes = ZipEntry.ibm437.GetBytes(this._Comment);
				}
				this._actualEncoding = ZipEntry.ibm437;
				return ZipEntry.ibm437.GetBytes(text);
			case ZipOption.Always:
				if (this._Comment != null && this._Comment.Length != 0)
				{
					this._CommentBytes = this.AlternateEncoding.GetBytes(this._Comment);
				}
				this._actualEncoding = this.AlternateEncoding;
				return this.AlternateEncoding.GetBytes(text);
			}
			byte[] bytes = ZipEntry.ibm437.GetBytes(text);
			string @string = ZipEntry.ibm437.GetString(bytes, 0, bytes.Length);
			this._CommentBytes = null;
			if (@string != text)
			{
				bytes = this.AlternateEncoding.GetBytes(text);
				if (this._Comment != null && this._Comment.Length != 0)
				{
					this._CommentBytes = this.AlternateEncoding.GetBytes(this._Comment);
				}
				this._actualEncoding = this.AlternateEncoding;
				return bytes;
			}
			this._actualEncoding = ZipEntry.ibm437;
			if (this._Comment == null || this._Comment.Length == 0)
			{
				return bytes;
			}
			byte[] bytes2 = ZipEntry.ibm437.GetBytes(this._Comment);
			string string2 = ZipEntry.ibm437.GetString(bytes2, 0, bytes2.Length);
			if (string2 != this.Comment)
			{
				bytes = this.AlternateEncoding.GetBytes(text);
				this._CommentBytes = this.AlternateEncoding.GetBytes(this._Comment);
				this._actualEncoding = this.AlternateEncoding;
				return bytes;
			}
			this._CommentBytes = bytes2;
			return bytes;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0004FEC8 File Offset: 0x0004E0C8
		private bool WantReadAgain()
		{
			return this._UncompressedSize >= 16L && this._CompressionMethod != 0 && this.CompressionLevel != CompressionLevel.None && this._CompressedSize >= this._UncompressedSize && (this._Source != ZipEntrySource.Stream || this._sourceStream.CanSeek) && (this._zipCrypto_forWrite == null || this.CompressedSize - 12L > this.UncompressedSize);
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0004FF3C File Offset: 0x0004E13C
		private void MaybeUnsetCompressionMethodForWriting(int cycle)
		{
			if (cycle > 1)
			{
				this._CompressionMethod = 0;
				return;
			}
			if (this.IsDirectory)
			{
				this._CompressionMethod = 0;
				return;
			}
			if (this._Source == ZipEntrySource.ZipFile)
			{
				return;
			}
			if (this._Source == ZipEntrySource.Stream)
			{
				if (this._sourceStream != null && this._sourceStream.CanSeek)
				{
					long length = this._sourceStream.Length;
					if (length == 0L)
					{
						this._CompressionMethod = 0;
						return;
					}
				}
			}
			else if (this._Source == ZipEntrySource.FileSystem && SharedUtilities.GetFileLength(this.LocalFileName) == 0L)
			{
				this._CompressionMethod = 0;
				return;
			}
			if (this.SetCompression != null)
			{
				this.CompressionLevel = this.SetCompression(this.LocalFileName, this._FileNameInArchive);
			}
			if (this.CompressionLevel == CompressionLevel.None && this.CompressionMethod == CompressionMethod.Deflate)
			{
				this._CompressionMethod = 0;
			}
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00050004 File Offset: 0x0004E204
		internal void WriteHeader(Stream s, int cycle)
		{
			CountingStream countingStream = s as CountingStream;
			this._future_ROLH = ((countingStream != null) ? countingStream.ComputedPosition : s.Position);
			int num = 0;
			byte[] array = new byte[30];
			array[num++] = 80;
			array[num++] = 75;
			array[num++] = 3;
			array[num++] = 4;
			this._presumeZip64 = (this._container.Zip64 == Zip64Option.Always || (this._container.Zip64 == Zip64Option.AsNecessary && !s.CanSeek));
			short num2 = this._presumeZip64 ? 45 : 20;
			array[num++] = (byte)(num2 & 255);
			array[num++] = (byte)(((int)num2 & 65280) >> 8);
			byte[] encodedFileNameBytes = this.GetEncodedFileNameBytes();
			short num3 = (short)encodedFileNameBytes.Length;
			if (this._Encryption == EncryptionAlgorithm.None)
			{
				this._BitField &= -2;
			}
			else
			{
				this._BitField |= 1;
			}
			if (this._actualEncoding.CodePage == Encoding.UTF8.CodePage)
			{
				this._BitField |= 2048;
			}
			if (this.IsDirectory || cycle == 99)
			{
				this._BitField &= -9;
				this._BitField &= -2;
				this.Encryption = EncryptionAlgorithm.None;
				this.Password = null;
			}
			else if (!s.CanSeek)
			{
				this._BitField |= 8;
			}
			array[num++] = (byte)(this._BitField & 255);
			array[num++] = (byte)(((int)this._BitField & 65280) >> 8);
			if (this.__FileDataPosition == -1L)
			{
				this._CompressedSize = 0L;
				this._crcCalculated = false;
			}
			this.MaybeUnsetCompressionMethodForWriting(cycle);
			array[num++] = (byte)(this._CompressionMethod & 255);
			array[num++] = (byte)(((int)this._CompressionMethod & 65280) >> 8);
			if (cycle == 99)
			{
				this.SetZip64Flags();
			}
			this._TimeBlob = SharedUtilities.DateTimeToPacked(this.LastModified);
			array[num++] = (byte)(this._TimeBlob & 255);
			array[num++] = (byte)((this._TimeBlob & 65280) >> 8);
			array[num++] = (byte)((this._TimeBlob & 16711680) >> 16);
			array[num++] = (byte)(((long)this._TimeBlob & (long)((ulong)-16777216)) >> 24);
			array[num++] = (byte)(this._Crc32 & 255);
			array[num++] = (byte)((this._Crc32 & 65280) >> 8);
			array[num++] = (byte)((this._Crc32 & 16711680) >> 16);
			array[num++] = (byte)(((long)this._Crc32 & (long)((ulong)-16777216)) >> 24);
			if (this._presumeZip64)
			{
				for (int i = 0; i < 8; i++)
				{
					array[num++] = 255;
				}
			}
			else
			{
				array[num++] = (byte)(this._CompressedSize & 255L);
				array[num++] = (byte)((this._CompressedSize & 65280L) >> 8);
				array[num++] = (byte)((this._CompressedSize & 16711680L) >> 16);
				array[num++] = (byte)((this._CompressedSize & (long)((ulong)-16777216)) >> 24);
				array[num++] = (byte)(this._UncompressedSize & 255L);
				array[num++] = (byte)((this._UncompressedSize & 65280L) >> 8);
				array[num++] = (byte)((this._UncompressedSize & 16711680L) >> 16);
				array[num++] = (byte)((this._UncompressedSize & (long)((ulong)-16777216)) >> 24);
			}
			array[num++] = (byte)(num3 & 255);
			array[num++] = (byte)(((int)num3 & 65280) >> 8);
			this._Extra = this.ConstructExtraField(false);
			short num4 = (short)((this._Extra == null) ? 0 : this._Extra.Length);
			array[num++] = (byte)(num4 & 255);
			array[num++] = (byte)(((int)num4 & 65280) >> 8);
			byte[] array2 = new byte[num + (int)num3 + (int)num4];
			Buffer.BlockCopy(array, 0, array2, 0, num);
			Buffer.BlockCopy(encodedFileNameBytes, 0, array2, num, encodedFileNameBytes.Length);
			num += encodedFileNameBytes.Length;
			if (this._Extra != null)
			{
				Buffer.BlockCopy(this._Extra, 0, array2, num, this._Extra.Length);
				num += this._Extra.Length;
			}
			this._LengthOfHeader = num;
			ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = true;
				uint num5 = zipSegmentedStream.ComputeSegment(num);
				if (num5 != zipSegmentedStream.CurrentSegment)
				{
					this._future_ROLH = 0L;
				}
				else
				{
					this._future_ROLH = zipSegmentedStream.Position;
				}
				this._diskNumber = num5;
			}
			if (this._container.Zip64 == Zip64Option.Default && (uint)this._RelativeOffsetOfLocalHeader >= 4294967295u)
			{
				throw new ZipException("Offset within the zip archive exceeds 0xFFFFFFFF. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
			}
			s.Write(array2, 0, num);
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = false;
			}
			this._EntryHeader = array2;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x000504E8 File Offset: 0x0004E6E8
		private int FigureCrc32()
		{
			if (!this._crcCalculated)
			{
				Stream stream = null;
				if (this._Source == ZipEntrySource.WriteDelegate)
				{
					CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(Stream.Null);
					this._WriteDelegate(this.FileName, crcCalculatorStream);
					this._Crc32 = crcCalculatorStream.Crc;
				}
				else if (this._Source != ZipEntrySource.ZipFile)
				{
					if (this._Source == ZipEntrySource.Stream)
					{
						this.PrepSourceStream();
						stream = this._sourceStream;
					}
					else if (this._Source == ZipEntrySource.JitStream)
					{
						if (this._sourceStream == null)
						{
							this._sourceStream = this._OpenDelegate(this.FileName);
						}
						this.PrepSourceStream();
						stream = this._sourceStream;
					}
					else if (this._Source != ZipEntrySource.ZipOutputStream)
					{
						stream = File.Open(this.LocalFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					}
					CRC32 cRC = new CRC32();
					this._Crc32 = cRC.GetCrc32(stream);
					if (this._sourceStream == null)
					{
						stream.Dispose();
					}
				}
				this._crcCalculated = true;
			}
			return this._Crc32;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x000505DC File Offset: 0x0004E7DC
		private void PrepSourceStream()
		{
			if (this._sourceStream == null)
			{
				throw new ZipException(string.Format("The input stream is null for entry '{0}'.", this.FileName));
			}
			if (this._sourceStreamOriginalPosition.HasValue)
			{
				this._sourceStream.Position = this._sourceStreamOriginalPosition.Value;
				return;
			}
			if (this._sourceStream.CanSeek)
			{
				this._sourceStreamOriginalPosition = new long?(this._sourceStream.Position);
				return;
			}
			if (this.Encryption == EncryptionAlgorithm.PkzipWeak && this._Source != ZipEntrySource.ZipFile && (this._BitField & 8) != 8)
			{
				throw new ZipException("It is not possible to use PKZIP encryption on a non-seekable input stream");
			}
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00050678 File Offset: 0x0004E878
		internal void CopyMetaData(ZipEntry source)
		{
			this.__FileDataPosition = source.__FileDataPosition;
			this.CompressionMethod = source.CompressionMethod;
			this._CompressionMethod_FromZipFile = source._CompressionMethod_FromZipFile;
			this._CompressedFileDataSize = source._CompressedFileDataSize;
			this._UncompressedSize = source._UncompressedSize;
			this._BitField = source._BitField;
			this._Source = source._Source;
			this._LastModified = source._LastModified;
			this._Mtime = source._Mtime;
			this._Atime = source._Atime;
			this._Ctime = source._Ctime;
			this._ntfsTimesAreSet = source._ntfsTimesAreSet;
			this._emitUnixTimes = source._emitUnixTimes;
			this._emitNtfsTimes = source._emitNtfsTimes;
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0005072D File Offset: 0x0004E92D
		private void OnWriteBlock(long bytesXferred, long totalBytesToXfer)
		{
			if (this._container.ZipFile != null)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnSaveBlock(this, bytesXferred, totalBytesToXfer);
			}
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00050758 File Offset: 0x0004E958
		private void _WriteEntryData(Stream s)
		{
			Stream stream = null;
			long _FileDataPosition = -1L;
			try
			{
				_FileDataPosition = s.Position;
			}
			catch (Exception)
			{
			}
			try
			{
				long num = this.SetInputAndFigureFileLength(ref stream);
				CountingStream countingStream = new CountingStream(s);
				Stream stream2;
				Stream stream3;
				if (num != 0L)
				{
					stream2 = this.MaybeApplyEncryption(countingStream);
					stream3 = this.MaybeApplyCompression(stream2, num);
				}
				else
				{
					stream3 = (stream2 = countingStream);
				}
				CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(stream3, true);
				if (this._Source == ZipEntrySource.WriteDelegate)
				{
					this._WriteDelegate(this.FileName, crcCalculatorStream);
				}
				else
				{
					byte[] array = new byte[this.BufferSize];
					int count;
					while ((count = SharedUtilities.ReadWithRetry(stream, array, 0, array.Length, this.FileName)) != 0)
					{
						crcCalculatorStream.Write(array, 0, count);
						this.OnWriteBlock(crcCalculatorStream.TotalBytesSlurped, num);
						if (this._ioOperationCanceled)
						{
							break;
						}
					}
				}
				this.FinishOutputStream(s, countingStream, stream2, stream3, crcCalculatorStream);
			}
			finally
			{
				if (this._Source == ZipEntrySource.JitStream)
				{
					if (this._CloseDelegate != null)
					{
						this._CloseDelegate(this.FileName, stream);
					}
				}
				else if (stream is FileStream)
				{
					stream.Dispose();
				}
			}
			if (this._ioOperationCanceled)
			{
				return;
			}
			this.__FileDataPosition = _FileDataPosition;
			this.PostProcessOutput(s);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00050890 File Offset: 0x0004EA90
		private long SetInputAndFigureFileLength(ref Stream input)
		{
			long result = -1L;
			if (this._Source == ZipEntrySource.Stream)
			{
				this.PrepSourceStream();
				input = this._sourceStream;
				try
				{
					result = this._sourceStream.Length;
					return result;
				}
				catch (NotSupportedException)
				{
					return result;
				}
			}
			if (this._Source == ZipEntrySource.ZipFile)
			{
				string password = (this._Encryption_FromZipFile == EncryptionAlgorithm.None) ? null : (this._Password ?? this._container.Password);
				this._sourceStream = this.InternalOpenReader(password);
				this.PrepSourceStream();
				input = this._sourceStream;
				result = this._sourceStream.Length;
			}
			else
			{
				if (this._Source == ZipEntrySource.JitStream)
				{
					if (this._sourceStream == null)
					{
						this._sourceStream = this._OpenDelegate(this.FileName);
					}
					this.PrepSourceStream();
					input = this._sourceStream;
					try
					{
						result = this._sourceStream.Length;
						return result;
					}
					catch (NotSupportedException)
					{
						return result;
					}
				}
				if (this._Source == ZipEntrySource.FileSystem)
				{
					FileShare fileShare = FileShare.ReadWrite;
					fileShare |= FileShare.Delete;
					input = File.Open(this.LocalFileName, FileMode.Open, FileAccess.Read, fileShare);
					result = input.Length;
				}
			}
			return result;
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x000509AC File Offset: 0x0004EBAC
		internal void FinishOutputStream(Stream s, CountingStream entryCounter, Stream encryptor, Stream compressor, CrcCalculatorStream output)
		{
			if (output == null)
			{
				return;
			}
			output.Close();
			if (compressor is DeflateStream)
			{
				compressor.Close();
			}
			else if (compressor is ParallelDeflateOutputStream)
			{
				compressor.Close();
			}
			encryptor.Flush();
			encryptor.Close();
			this._LengthOfTrailer = 0;
			this._UncompressedSize = output.TotalBytesSlurped;
			this._CompressedFileDataSize = entryCounter.BytesWritten;
			this._CompressedSize = this._CompressedFileDataSize;
			this._Crc32 = output.Crc;
			this.StoreRelativeOffset();
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00050A34 File Offset: 0x0004EC34
		internal void PostProcessOutput(Stream s)
		{
			CountingStream countingStream = s as CountingStream;
			if (this._UncompressedSize == 0L && this._CompressedSize == 0L)
			{
				if (this._Source == ZipEntrySource.ZipOutputStream)
				{
					return;
				}
				if (this._Password != null)
				{
					int num = 0;
					if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
					{
						num = 12;
					}
					if (this._Source == ZipEntrySource.ZipOutputStream && !s.CanSeek)
					{
						throw new ZipException("Zero bytes written, encryption in use, and non-seekable output.");
					}
					if (this.Encryption != EncryptionAlgorithm.None)
					{
						s.Seek((long)(-1 * num), SeekOrigin.Current);
						s.SetLength(s.Position);
						if (countingStream != null)
						{
							countingStream.Adjust((long)num);
						}
						this._LengthOfHeader -= num;
						this.__FileDataPosition -= (long)num;
					}
					this._Password = null;
					this._BitField &= -2;
					int num2 = 6;
					this._EntryHeader[num2++] = (byte)(this._BitField & 255);
					this._EntryHeader[num2++] = (byte)(((int)this._BitField & 65280) >> 8);
				}
				this.CompressionMethod = CompressionMethod.None;
				this.Encryption = EncryptionAlgorithm.None;
			}
			else if (this._zipCrypto_forWrite != null && this.Encryption == EncryptionAlgorithm.PkzipWeak)
			{
				this._CompressedSize += 12L;
			}
			int num3 = 8;
			this._EntryHeader[num3++] = (byte)(this._CompressionMethod & 255);
			this._EntryHeader[num3++] = (byte)(((int)this._CompressionMethod & 65280) >> 8);
			num3 = 14;
			this._EntryHeader[num3++] = (byte)(this._Crc32 & 255);
			this._EntryHeader[num3++] = (byte)((this._Crc32 & 65280) >> 8);
			this._EntryHeader[num3++] = (byte)((this._Crc32 & 16711680) >> 16);
			this._EntryHeader[num3++] = (byte)(((long)this._Crc32 & (long)((ulong)-16777216)) >> 24);
			this.SetZip64Flags();
			short num4 = (short)((int)this._EntryHeader[26] + (int)this._EntryHeader[27] * 256);
			short num5 = (short)((int)this._EntryHeader[28] + (int)this._EntryHeader[29] * 256);
			if (this._OutputUsesZip64.Value)
			{
				this._EntryHeader[4] = 45;
				this._EntryHeader[5] = 0;
				for (int i = 0; i < 8; i++)
				{
					this._EntryHeader[num3++] = 255;
				}
				num3 = (int)(30 + num4);
				this._EntryHeader[num3++] = 1;
				this._EntryHeader[num3++] = 0;
				num3 += 2;
				Array.Copy(BitConverter.GetBytes(this._UncompressedSize), 0, this._EntryHeader, num3, 8);
				num3 += 8;
				Array.Copy(BitConverter.GetBytes(this._CompressedSize), 0, this._EntryHeader, num3, 8);
			}
			else
			{
				this._EntryHeader[4] = 20;
				this._EntryHeader[5] = 0;
				num3 = 18;
				this._EntryHeader[num3++] = (byte)(this._CompressedSize & 255L);
				this._EntryHeader[num3++] = (byte)((this._CompressedSize & 65280L) >> 8);
				this._EntryHeader[num3++] = (byte)((this._CompressedSize & 16711680L) >> 16);
				this._EntryHeader[num3++] = (byte)((this._CompressedSize & (long)((ulong)-16777216)) >> 24);
				this._EntryHeader[num3++] = (byte)(this._UncompressedSize & 255L);
				this._EntryHeader[num3++] = (byte)((this._UncompressedSize & 65280L) >> 8);
				this._EntryHeader[num3++] = (byte)((this._UncompressedSize & 16711680L) >> 16);
				this._EntryHeader[num3++] = (byte)((this._UncompressedSize & (long)((ulong)-16777216)) >> 24);
				if (num5 != 0)
				{
					num3 = (int)(30 + num4);
					short num6 = (short)((int)this._EntryHeader[num3 + 2] + (int)this._EntryHeader[num3 + 3] * 256);
					if (num6 == 16)
					{
						this._EntryHeader[num3++] = 153;
						this._EntryHeader[num3++] = 153;
					}
				}
			}
			if ((this._BitField & 8) != 8 || (this._Source == ZipEntrySource.ZipOutputStream && s.CanSeek))
			{
				ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
				if (zipSegmentedStream != null && this._diskNumber != zipSegmentedStream.CurrentSegment)
				{
					using (Stream stream = ZipSegmentedStream.ForUpdate(this._container.ZipFile.Name, this._diskNumber))
					{
						stream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
						stream.Write(this._EntryHeader, 0, this._EntryHeader.Length);
						goto IL_4C4;
					}
				}
				s.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
				s.Write(this._EntryHeader, 0, this._EntryHeader.Length);
				if (countingStream != null)
				{
					countingStream.Adjust((long)this._EntryHeader.Length);
				}
				s.Seek(this._CompressedSize, SeekOrigin.Current);
			}
			IL_4C4:
			if ((this._BitField & 8) == 8 && !this.IsDirectory)
			{
				byte[] array = new byte[16 + (this._OutputUsesZip64.Value ? 8 : 0)];
				num3 = 0;
				Array.Copy(BitConverter.GetBytes(134695760), 0, array, num3, 4);
				num3 += 4;
				Array.Copy(BitConverter.GetBytes(this._Crc32), 0, array, num3, 4);
				num3 += 4;
				if (this._OutputUsesZip64.Value)
				{
					Array.Copy(BitConverter.GetBytes(this._CompressedSize), 0, array, num3, 8);
					num3 += 8;
					Array.Copy(BitConverter.GetBytes(this._UncompressedSize), 0, array, num3, 8);
					num3 += 8;
				}
				else
				{
					array[num3++] = (byte)(this._CompressedSize & 255L);
					array[num3++] = (byte)((this._CompressedSize & 65280L) >> 8);
					array[num3++] = (byte)((this._CompressedSize & 16711680L) >> 16);
					array[num3++] = (byte)((this._CompressedSize & (long)((ulong)-16777216)) >> 24);
					array[num3++] = (byte)(this._UncompressedSize & 255L);
					array[num3++] = (byte)((this._UncompressedSize & 65280L) >> 8);
					array[num3++] = (byte)((this._UncompressedSize & 16711680L) >> 16);
					array[num3++] = (byte)((this._UncompressedSize & (long)((ulong)-16777216)) >> 24);
				}
				s.Write(array, 0, array.Length);
				this._LengthOfTrailer += array.Length;
			}
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x000510A0 File Offset: 0x0004F2A0
		private void SetZip64Flags()
		{
			this._entryRequiresZip64 = new bool?(this._CompressedSize >= (long)((ulong)-1) || this._UncompressedSize >= (long)((ulong)-1) || this._RelativeOffsetOfLocalHeader >= (long)((ulong)-1));
			if (this._container.Zip64 == Zip64Option.Default && this._entryRequiresZip64.Value)
			{
				throw new ZipException("Compressed or Uncompressed size, or offset exceeds the maximum value. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
			}
			this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always || this._entryRequiresZip64.Value);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00051128 File Offset: 0x0004F328
		internal void PrepOutputStream(Stream s, long streamLength, out CountingStream outputCounter, out Stream encryptor, out Stream compressor, out CrcCalculatorStream output)
		{
			outputCounter = new CountingStream(s);
			if (streamLength != 0L)
			{
				encryptor = this.MaybeApplyEncryption(outputCounter);
				compressor = this.MaybeApplyCompression(encryptor, streamLength);
			}
			else
			{
				Stream stream;
				compressor = (stream = outputCounter);
				encryptor = stream;
			}
			output = new CrcCalculatorStream(compressor, true);
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00051174 File Offset: 0x0004F374
		private Stream MaybeApplyCompression(Stream s, long streamLength)
		{
			if (this._CompressionMethod != 8 || this.CompressionLevel == CompressionLevel.None)
			{
				return s;
			}
			if (this._container.ParallelDeflateThreshold == 0L || (streamLength > this._container.ParallelDeflateThreshold && this._container.ParallelDeflateThreshold > 0L))
			{
				if (this._container.ParallelDeflater == null)
				{
					this._container.ParallelDeflater = new ParallelDeflateOutputStream(s, this.CompressionLevel, this._container.Strategy, true);
					if (this._container.CodecBufferSize > 0)
					{
						this._container.ParallelDeflater.BufferSize = this._container.CodecBufferSize;
					}
					if (this._container.ParallelDeflateMaxBufferPairs > 0)
					{
						this._container.ParallelDeflater.MaxBufferPairs = this._container.ParallelDeflateMaxBufferPairs;
					}
				}
				ParallelDeflateOutputStream parallelDeflater = this._container.ParallelDeflater;
				parallelDeflater.Reset(s);
				return parallelDeflater;
			}
			DeflateStream deflateStream = new DeflateStream(s, CompressionMode.Compress, this.CompressionLevel, true);
			if (this._container.CodecBufferSize > 0)
			{
				deflateStream.BufferSize = this._container.CodecBufferSize;
			}
			deflateStream.Strategy = this._container.Strategy;
			return deflateStream;
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x000512A3 File Offset: 0x0004F4A3
		private Stream MaybeApplyEncryption(Stream s)
		{
			if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
			{
				return new ZipCipherStream(s, this._zipCrypto_forWrite, CryptoMode.Encrypt);
			}
			return s;
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x000512BD File Offset: 0x0004F4BD
		private void OnZipErrorWhileSaving(Exception e)
		{
			if (this._container.ZipFile != null)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnZipErrorSaving(this, e);
			}
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x000512E4 File Offset: 0x0004F4E4
		internal void Write(Stream s)
		{
			CountingStream countingStream = s as CountingStream;
			ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
			bool flag = false;
			do
			{
				try
				{
					if (this._Source == ZipEntrySource.ZipFile && !this._restreamRequiredOnSave)
					{
						this.CopyThroughOneEntry(s);
						break;
					}
					if (this.IsDirectory)
					{
						this.WriteHeader(s, 1);
						this.StoreRelativeOffset();
						this._entryRequiresZip64 = new bool?(this._RelativeOffsetOfLocalHeader >= (long)((ulong)-1));
						this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always || this._entryRequiresZip64.Value);
						if (zipSegmentedStream != null)
						{
							this._diskNumber = zipSegmentedStream.CurrentSegment;
						}
						break;
					}
					int num = 0;
					bool flag2;
					do
					{
						num++;
						this.WriteHeader(s, num);
						this.WriteSecurityMetadata(s);
						this._WriteEntryData(s);
						this._TotalEntrySize = (long)this._LengthOfHeader + this._CompressedFileDataSize + (long)this._LengthOfTrailer;
						flag2 = (num <= 1 && s.CanSeek && this.WantReadAgain());
						if (flag2)
						{
							if (zipSegmentedStream != null)
							{
								zipSegmentedStream.TruncateBackward(this._diskNumber, this._RelativeOffsetOfLocalHeader);
							}
							else
							{
								s.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
							}
							s.SetLength(s.Position);
							if (countingStream != null)
							{
								countingStream.Adjust(this._TotalEntrySize);
							}
						}
					}
					while (flag2);
					this._skippedDuringSave = false;
					flag = true;
				}
				catch (Exception ex)
				{
					ZipErrorAction zipErrorAction = this.ZipErrorAction;
					int num2 = 0;
					while (this.ZipErrorAction != ZipErrorAction.Throw)
					{
						if (this.ZipErrorAction == ZipErrorAction.Skip || this.ZipErrorAction == ZipErrorAction.Retry)
						{
							long num3 = (countingStream != null) ? countingStream.ComputedPosition : s.Position;
							long num4 = num3 - this._future_ROLH;
							if (num4 > 0L)
							{
								s.Seek(num4, SeekOrigin.Current);
								long position = s.Position;
								s.SetLength(s.Position);
								if (countingStream != null)
								{
									countingStream.Adjust(num3 - position);
								}
							}
							if (this.ZipErrorAction == ZipErrorAction.Skip)
							{
								this.WriteStatus("Skipping file {0} (exception: {1})", new object[]
								{
									this.LocalFileName,
									ex.ToString()
								});
								this._skippedDuringSave = true;
								flag = true;
							}
							else
							{
								this.ZipErrorAction = zipErrorAction;
							}
						}
						else
						{
							if (num2 > 0)
							{
								throw;
							}
							if (this.ZipErrorAction == ZipErrorAction.InvokeErrorEvent)
							{
								this.OnZipErrorWhileSaving(ex);
								if (this._ioOperationCanceled)
								{
									flag = true;
									goto IL_23B;
								}
							}
							num2++;
							continue;
						}
						IL_23B:
						goto IL_23D;
					}
					throw;
				}
				IL_23D:;
			}
			while (!flag);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00051550 File Offset: 0x0004F750
		internal void StoreRelativeOffset()
		{
			this._RelativeOffsetOfLocalHeader = this._future_ROLH;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0005155E File Offset: 0x0004F75E
		internal void NotifySaveComplete()
		{
			this._Encryption_FromZipFile = this._Encryption;
			this._CompressionMethod_FromZipFile = this._CompressionMethod;
			this._restreamRequiredOnSave = false;
			this._metadataChanged = false;
			this._Source = ZipEntrySource.ZipFile;
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x00051590 File Offset: 0x0004F790
		internal void WriteSecurityMetadata(Stream outstream)
		{
			if (this.Encryption == EncryptionAlgorithm.None)
			{
				return;
			}
			string password = this._Password;
			if (this._Source == ZipEntrySource.ZipFile && password == null)
			{
				password = this._container.Password;
			}
			if (password == null)
			{
				this._zipCrypto_forWrite = null;
				return;
			}
			if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
			{
				this._zipCrypto_forWrite = ZipCrypto.ForWrite(password);
				Random random = new Random();
				byte[] array = new byte[12];
				random.NextBytes(array);
				if ((this._BitField & 8) == 8)
				{
					this._TimeBlob = SharedUtilities.DateTimeToPacked(this.LastModified);
					array[11] = (byte)(this._TimeBlob >> 8 & 255);
				}
				else
				{
					this.FigureCrc32();
					array[11] = (byte)(this._Crc32 >> 24 & 255);
				}
				byte[] array2 = this._zipCrypto_forWrite.EncryptMessage(array, array.Length);
				outstream.Write(array2, 0, array2.Length);
				this._LengthOfHeader += array2.Length;
			}
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00051674 File Offset: 0x0004F874
		private void CopyThroughOneEntry(Stream outStream)
		{
			if (this.LengthOfHeader == 0)
			{
				throw new BadStateException("Bad header length.");
			}
			bool flag = this._metadataChanged || this.ArchiveStream is ZipSegmentedStream || outStream is ZipSegmentedStream || (this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Default) || (!this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Always);
			if (flag)
			{
				this.CopyThroughWithRecompute(outStream);
			}
			else
			{
				this.CopyThroughWithNoChange(outStream);
			}
			this._entryRequiresZip64 = new bool?(this._CompressedSize >= (long)((ulong)-1) || this._UncompressedSize >= (long)((ulong)-1) || this._RelativeOffsetOfLocalHeader >= (long)((ulong)-1));
			this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always || this._entryRequiresZip64.Value);
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0005174C File Offset: 0x0004F94C
		private void CopyThroughWithRecompute(Stream outstream)
		{
			byte[] array = new byte[this.BufferSize];
			CountingStream countingStream = new CountingStream(this.ArchiveStream);
			long relativeOffsetOfLocalHeader = this._RelativeOffsetOfLocalHeader;
			int lengthOfHeader = this.LengthOfHeader;
			this.WriteHeader(outstream, 0);
			this.StoreRelativeOffset();
			if (!this.FileName.EndsWith("/"))
			{
				long num = relativeOffsetOfLocalHeader + (long)lengthOfHeader;
				int num2 = ZipEntry.GetLengthOfCryptoHeaderBytes(this._Encryption_FromZipFile);
				num -= (long)num2;
				this._LengthOfHeader += num2;
				countingStream.Seek(num, SeekOrigin.Begin);
				long num3 = this._CompressedSize;
				while (num3 > 0L)
				{
					num2 = ((num3 > (long)array.Length) ? array.Length : ((int)num3));
					int num4 = countingStream.Read(array, 0, num2);
					outstream.Write(array, 0, num4);
					num3 -= (long)num4;
					this.OnWriteBlock(countingStream.BytesRead, this._CompressedSize);
					if (this._ioOperationCanceled)
					{
						break;
					}
				}
				if ((this._BitField & 8) == 8)
				{
					int num5 = 16;
					if (this._InputUsesZip64)
					{
						num5 += 8;
					}
					byte[] buffer = new byte[num5];
					countingStream.Read(buffer, 0, num5);
					if (this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Default)
					{
						outstream.Write(buffer, 0, 8);
						if (this._CompressedSize > (long)((ulong)-1))
						{
							throw new InvalidOperationException("ZIP64 is required");
						}
						outstream.Write(buffer, 8, 4);
						if (this._UncompressedSize > (long)((ulong)-1))
						{
							throw new InvalidOperationException("ZIP64 is required");
						}
						outstream.Write(buffer, 16, 4);
						this._LengthOfTrailer -= 8;
					}
					else if (!this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Always)
					{
						byte[] buffer2 = new byte[4];
						outstream.Write(buffer, 0, 8);
						outstream.Write(buffer, 8, 4);
						outstream.Write(buffer2, 0, 4);
						outstream.Write(buffer, 12, 4);
						outstream.Write(buffer2, 0, 4);
						this._LengthOfTrailer += 8;
					}
					else
					{
						outstream.Write(buffer, 0, num5);
					}
				}
			}
			this._TotalEntrySize = (long)this._LengthOfHeader + this._CompressedFileDataSize + (long)this._LengthOfTrailer;
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0005195C File Offset: 0x0004FB5C
		private void CopyThroughWithNoChange(Stream outstream)
		{
			byte[] array = new byte[this.BufferSize];
			CountingStream countingStream = new CountingStream(this.ArchiveStream);
			countingStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
			if (this._TotalEntrySize == 0L)
			{
				this._TotalEntrySize = (long)this._LengthOfHeader + this._CompressedFileDataSize + (long)this._LengthOfTrailer;
			}
			CountingStream countingStream2 = outstream as CountingStream;
			this._RelativeOffsetOfLocalHeader = ((countingStream2 != null) ? countingStream2.ComputedPosition : outstream.Position);
			long num = this._TotalEntrySize;
			while (num > 0L)
			{
				int count = (num > (long)array.Length) ? array.Length : ((int)num);
				int num2 = countingStream.Read(array, 0, count);
				outstream.Write(array, 0, num2);
				num -= (long)num2;
				this.OnWriteBlock(countingStream.BytesRead, this._TotalEntrySize);
				if (this._ioOperationCanceled)
				{
					return;
				}
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x00051A2C File Offset: 0x0004FC2C
		[Conditional("Trace")]
		private void TraceWriteLine(string format, params object[] varParams)
		{
			lock (this._outputLock)
			{
				int hashCode = Thread.CurrentThread.GetHashCode();
				Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
				Console.Write("{0:000} ZipEntry.Write ", hashCode);
				Console.WriteLine(format, varParams);
				Console.ResetColor();
			}
		}

		// Token: 0x04000762 RID: 1890
		private short _VersionMadeBy;

		// Token: 0x04000763 RID: 1891
		private short _InternalFileAttrs;

		// Token: 0x04000764 RID: 1892
		private int _ExternalFileAttrs;

		// Token: 0x04000765 RID: 1893
		private short _filenameLength;

		// Token: 0x04000766 RID: 1894
		private short _extraFieldLength;

		// Token: 0x04000767 RID: 1895
		private short _commentLength;

		// Token: 0x04000768 RID: 1896
		private ZipCrypto _zipCrypto_forExtract;

		// Token: 0x04000769 RID: 1897
		private ZipCrypto _zipCrypto_forWrite;

		// Token: 0x0400076A RID: 1898
		internal DateTime _LastModified;

		// Token: 0x0400076B RID: 1899
		private DateTime _Mtime;

		// Token: 0x0400076C RID: 1900
		private DateTime _Atime;

		// Token: 0x0400076D RID: 1901
		private DateTime _Ctime;

		// Token: 0x0400076E RID: 1902
		private bool _ntfsTimesAreSet;

		// Token: 0x0400076F RID: 1903
		private bool _emitNtfsTimes = true;

		// Token: 0x04000770 RID: 1904
		private bool _emitUnixTimes;

		// Token: 0x04000771 RID: 1905
		private bool _TrimVolumeFromFullyQualifiedPaths = true;

		// Token: 0x04000772 RID: 1906
		internal string _LocalFileName;

		// Token: 0x04000773 RID: 1907
		private string _FileNameInArchive;

		// Token: 0x04000774 RID: 1908
		internal short _VersionNeeded;

		// Token: 0x04000775 RID: 1909
		internal short _BitField;

		// Token: 0x04000776 RID: 1910
		internal short _CompressionMethod;

		// Token: 0x04000777 RID: 1911
		private short _CompressionMethod_FromZipFile;

		// Token: 0x04000778 RID: 1912
		private CompressionLevel _CompressionLevel;

		// Token: 0x04000779 RID: 1913
		internal string _Comment;

		// Token: 0x0400077A RID: 1914
		private bool _IsDirectory;

		// Token: 0x0400077B RID: 1915
		private byte[] _CommentBytes;

		// Token: 0x0400077C RID: 1916
		internal long _CompressedSize;

		// Token: 0x0400077D RID: 1917
		internal long _CompressedFileDataSize;

		// Token: 0x0400077E RID: 1918
		internal long _UncompressedSize;

		// Token: 0x0400077F RID: 1919
		internal int _TimeBlob;

		// Token: 0x04000780 RID: 1920
		private bool _crcCalculated;

		// Token: 0x04000781 RID: 1921
		internal int _Crc32;

		// Token: 0x04000782 RID: 1922
		internal byte[] _Extra;

		// Token: 0x04000783 RID: 1923
		private bool _metadataChanged;

		// Token: 0x04000784 RID: 1924
		private bool _restreamRequiredOnSave;

		// Token: 0x04000785 RID: 1925
		private bool _sourceIsEncrypted;

		// Token: 0x04000786 RID: 1926
		private bool _skippedDuringSave;

		// Token: 0x04000787 RID: 1927
		private uint _diskNumber;

		// Token: 0x04000788 RID: 1928
		private static Encoding ibm437 = Encoding.GetEncoding("IBM437");

		// Token: 0x04000789 RID: 1929
		private Encoding _actualEncoding;

		// Token: 0x0400078A RID: 1930
		internal ZipContainer _container;

		// Token: 0x0400078B RID: 1931
		private long __FileDataPosition = -1L;

		// Token: 0x0400078C RID: 1932
		private byte[] _EntryHeader;

		// Token: 0x0400078D RID: 1933
		internal long _RelativeOffsetOfLocalHeader;

		// Token: 0x0400078E RID: 1934
		private long _future_ROLH;

		// Token: 0x0400078F RID: 1935
		private long _TotalEntrySize;

		// Token: 0x04000790 RID: 1936
		private int _LengthOfHeader;

		// Token: 0x04000791 RID: 1937
		private int _LengthOfTrailer;

		// Token: 0x04000792 RID: 1938
		internal bool _InputUsesZip64;

		// Token: 0x04000793 RID: 1939
		private uint _UnsupportedAlgorithmId;

		// Token: 0x04000794 RID: 1940
		internal string _Password;

		// Token: 0x04000795 RID: 1941
		internal ZipEntrySource _Source;

		// Token: 0x04000796 RID: 1942
		internal EncryptionAlgorithm _Encryption;

		// Token: 0x04000797 RID: 1943
		internal EncryptionAlgorithm _Encryption_FromZipFile;

		// Token: 0x04000798 RID: 1944
		internal byte[] _WeakEncryptionHeader;

		// Token: 0x04000799 RID: 1945
		internal Stream _archiveStream;

		// Token: 0x0400079A RID: 1946
		private Stream _sourceStream;

		// Token: 0x0400079B RID: 1947
		private long? _sourceStreamOriginalPosition;

		// Token: 0x0400079C RID: 1948
		private bool _sourceWasJitProvided;

		// Token: 0x0400079D RID: 1949
		private bool _ioOperationCanceled;

		// Token: 0x0400079E RID: 1950
		private bool _presumeZip64;

		// Token: 0x0400079F RID: 1951
		private bool? _entryRequiresZip64;

		// Token: 0x040007A0 RID: 1952
		private bool? _OutputUsesZip64;

		// Token: 0x040007A1 RID: 1953
		private bool _IsText;

		// Token: 0x040007A2 RID: 1954
		private ZipEntryTimestamp _timestamp;

		// Token: 0x040007A3 RID: 1955
		private static DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x040007A4 RID: 1956
		private static DateTime _win32Epoch = DateTime.FromFileTimeUtc(0L);

		// Token: 0x040007A5 RID: 1957
		private static DateTime _zeroHour = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x040007A6 RID: 1958
		private WriteDelegate _WriteDelegate;

		// Token: 0x040007A7 RID: 1959
		private OpenDelegate _OpenDelegate;

		// Token: 0x040007A8 RID: 1960
		private CloseDelegate _CloseDelegate;

		// Token: 0x040007A9 RID: 1961
		private Stream _inputDecryptorStream;

		// Token: 0x040007AA RID: 1962
		private int _readExtraDepth;

		// Token: 0x040007AB RID: 1963
		private object _outputLock = new object();

		// Token: 0x0200014F RID: 335
		private class CopyHelper
		{
			// Token: 0x06000DCD RID: 3533 RVA: 0x00051AE8 File Offset: 0x0004FCE8
			internal static string AppendCopyToFileName(string f)
			{
				ZipEntry.CopyHelper.callCount++;
				if (ZipEntry.CopyHelper.callCount > 25)
				{
					throw new OverflowException("overflow while creating filename");
				}
				int num = 1;
				int num2 = f.LastIndexOf(".");
				if (num2 == -1)
				{
					Match match = ZipEntry.CopyHelper.re.Match(f);
					if (match.Success)
					{
						num = int.Parse(match.Groups[1].Value) + 1;
						string str = string.Format(" (copy {0})", num);
						f = f.Substring(0, match.Index) + str;
					}
					else
					{
						string str2 = string.Format(" (copy {0})", num);
						f += str2;
					}
				}
				else
				{
					Match match2 = ZipEntry.CopyHelper.re.Match(f.Substring(0, num2));
					if (match2.Success)
					{
						num = int.Parse(match2.Groups[1].Value) + 1;
						string str3 = string.Format(" (copy {0})", num);
						f = f.Substring(0, match2.Index) + str3 + f.Substring(num2);
					}
					else
					{
						string str4 = string.Format(" (copy {0})", num);
						f = f.Substring(0, num2) + str4 + f.Substring(num2);
					}
				}
				return f;
			}

			// Token: 0x040007B2 RID: 1970
			private static Regex re = new Regex(" \\(copy (\\d+)\\)$");

			// Token: 0x040007B3 RID: 1971
			private static int callCount = 0;
		}

		// Token: 0x02000150 RID: 336
		// (Invoke) Token: 0x06000DD1 RID: 3537
		private delegate T Func<T>();
	}
}
