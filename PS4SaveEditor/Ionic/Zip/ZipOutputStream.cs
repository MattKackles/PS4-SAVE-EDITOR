using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ionic.Crc;
using Ionic.Zlib;

namespace Ionic.Zip
{
	// Token: 0x0200015F RID: 351
	public class ZipOutputStream : Stream
	{
		// Token: 0x06000EF3 RID: 3827 RVA: 0x00056EA6 File Offset: 0x000550A6
		public ZipOutputStream(Stream stream) : this(stream, false)
		{
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00056EB0 File Offset: 0x000550B0
		public ZipOutputStream(string fileName)
		{
			this._alternateEncoding = Encoding.GetEncoding("IBM437");
			this._maxBufferPairs = 16;
			base..ctor();
			Stream stream = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			this._Init(stream, false, fileName);
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00056EEE File Offset: 0x000550EE
		public ZipOutputStream(Stream stream, bool leaveOpen)
		{
			this._alternateEncoding = Encoding.GetEncoding("IBM437");
			this._maxBufferPairs = 16;
			base..ctor();
			this._Init(stream, leaveOpen, null);
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00056F18 File Offset: 0x00055118
		private void _Init(Stream stream, bool leaveOpen, string name)
		{
			this._outputStream = (stream.CanRead ? stream : new CountingStream(stream));
			this.CompressionLevel = CompressionLevel.Default;
			this.CompressionMethod = CompressionMethod.Deflate;
			this._encryption = EncryptionAlgorithm.None;
			this._entriesWritten = new Dictionary<string, ZipEntry>(StringComparer.Ordinal);
			this._zip64 = Zip64Option.Default;
			this._leaveUnderlyingStreamOpen = leaveOpen;
			this.Strategy = CompressionStrategy.Default;
			this._name = (name ?? "(stream)");
			this.ParallelDeflateThreshold = -1L;
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00056F8E File Offset: 0x0005518E
		public override string ToString()
		{
			return string.Format("ZipOutputStream::{0}(leaveOpen({1})))", this._name, this._leaveUnderlyingStreamOpen);
		}

		// Token: 0x170003D8 RID: 984
		// (set) Token: 0x06000EF8 RID: 3832 RVA: 0x00056FAC File Offset: 0x000551AC
		public string Password
		{
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._password = value;
				if (this._password == null)
				{
					this._encryption = EncryptionAlgorithm.None;
					return;
				}
				if (this._encryption == EncryptionAlgorithm.None)
				{
					this._encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x00056FF9 File Offset: 0x000551F9
		// (set) Token: 0x06000EFA RID: 3834 RVA: 0x00057001 File Offset: 0x00055201
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return this._encryption;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				if (value == EncryptionAlgorithm.Unsupported)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				this._encryption = value;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x0005703A File Offset: 0x0005523A
		// (set) Token: 0x06000EFC RID: 3836 RVA: 0x00057042 File Offset: 0x00055242
		public int CodecBufferSize
		{
			get;
			set;
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x0005704B File Offset: 0x0005524B
		// (set) Token: 0x06000EFE RID: 3838 RVA: 0x00057053 File Offset: 0x00055253
		public CompressionStrategy Strategy
		{
			get;
			set;
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x0005705C File Offset: 0x0005525C
		// (set) Token: 0x06000F00 RID: 3840 RVA: 0x00057064 File Offset: 0x00055264
		public ZipEntryTimestamp Timestamp
		{
			get
			{
				return this._timestamp;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._timestamp = value;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x00057087 File Offset: 0x00055287
		// (set) Token: 0x06000F02 RID: 3842 RVA: 0x0005708F File Offset: 0x0005528F
		public CompressionLevel CompressionLevel
		{
			get;
			set;
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x00057098 File Offset: 0x00055298
		// (set) Token: 0x06000F04 RID: 3844 RVA: 0x000570A0 File Offset: 0x000552A0
		public CompressionMethod CompressionMethod
		{
			get;
			set;
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x000570A9 File Offset: 0x000552A9
		// (set) Token: 0x06000F06 RID: 3846 RVA: 0x000570B1 File Offset: 0x000552B1
		public string Comment
		{
			get
			{
				return this._comment;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._comment = value;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x000570D4 File Offset: 0x000552D4
		// (set) Token: 0x06000F08 RID: 3848 RVA: 0x000570DC File Offset: 0x000552DC
		public Zip64Option EnableZip64
		{
			get
			{
				return this._zip64;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._zip64 = value;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000F09 RID: 3849 RVA: 0x000570FF File Offset: 0x000552FF
		public bool OutputUsedZip64
		{
			get
			{
				return this._anyEntriesUsedZip64 || this._directoryNeededZip64;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x00057111 File Offset: 0x00055311
		// (set) Token: 0x06000F0B RID: 3851 RVA: 0x0005711C File Offset: 0x0005531C
		public bool IgnoreCase
		{
			get
			{
				return !this._DontIgnoreCase;
			}
			set
			{
				this._DontIgnoreCase = !value;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000F0C RID: 3852 RVA: 0x00057128 File Offset: 0x00055328
		// (set) Token: 0x06000F0D RID: 3853 RVA: 0x00057142 File Offset: 0x00055342
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete. It will be removed in a future version of the library. Use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				return this._alternateEncoding == Encoding.UTF8 && this.AlternateEncodingUsage == ZipOption.AsNecessary;
			}
			set
			{
				if (value)
				{
					this._alternateEncoding = Encoding.UTF8;
					this._alternateEncodingUsage = ZipOption.AsNecessary;
					return;
				}
				this._alternateEncoding = ZipOutputStream.DefaultEncoding;
				this._alternateEncodingUsage = ZipOption.Default;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0005716C File Offset: 0x0005536C
		// (set) Token: 0x06000F0F RID: 3855 RVA: 0x0005717F File Offset: 0x0005537F
		[Obsolete("use AlternateEncoding and AlternateEncodingUsage instead.")]
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

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x0005718F File Offset: 0x0005538F
		// (set) Token: 0x06000F11 RID: 3857 RVA: 0x00057197 File Offset: 0x00055397
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

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x000571A0 File Offset: 0x000553A0
		// (set) Token: 0x06000F13 RID: 3859 RVA: 0x000571A8 File Offset: 0x000553A8
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

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000F14 RID: 3860 RVA: 0x000571B1 File Offset: 0x000553B1
		public static Encoding DefaultEncoding
		{
			get
			{
				return Encoding.GetEncoding("IBM437");
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x000571E4 File Offset: 0x000553E4
		// (set) Token: 0x06000F15 RID: 3861 RVA: 0x000571BD File Offset: 0x000553BD
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
					throw new ArgumentOutOfRangeException("value must be greater than 64k, or 0, or -1");
				}
				this._ParallelDeflateThreshold = value;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x000571EC File Offset: 0x000553EC
		// (set) Token: 0x06000F18 RID: 3864 RVA: 0x000571F4 File Offset: 0x000553F4
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

		// Token: 0x06000F19 RID: 3865 RVA: 0x00057211 File Offset: 0x00055411
		private void InsureUniqueEntry(ZipEntry ze1)
		{
			if (this._entriesWritten.ContainsKey(ze1.FileName))
			{
				this._exceptionPending = true;
				throw new ArgumentException(string.Format("The entry '{0}' already exists in the zip archive.", ze1.FileName));
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x00057243 File Offset: 0x00055443
		internal Stream OutputStream
		{
			get
			{
				return this._outputStream;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x0005724B File Offset: 0x0005544B
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x00057253 File Offset: 0x00055453
		public bool ContainsEntry(string name)
		{
			return this._entriesWritten.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x00057268 File Offset: 0x00055468
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			if (buffer == null)
			{
				this._exceptionPending = true;
				throw new ArgumentNullException("buffer");
			}
			if (this._currentEntry == null)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("You must call PutNextEntry() before calling Write().");
			}
			if (this._currentEntry.IsDirectory)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("You cannot Write() data for an entry that is a directory.");
			}
			if (this._needToWriteEntryHeader)
			{
				this._InitiateCurrentEntry(false);
			}
			if (count != 0)
			{
				this._entryOutputStream.Write(buffer, offset, count);
			}
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00057300 File Offset: 0x00055500
		public ZipEntry PutNextEntry(string entryName)
		{
			if (string.IsNullOrEmpty(entryName))
			{
				throw new ArgumentNullException("entryName");
			}
			if (this._disposed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			this._FinishCurrentEntry();
			this._currentEntry = ZipEntry.CreateForZipOutputStream(entryName);
			this._currentEntry._container = new ZipContainer(this);
			ZipEntry expr_56 = this._currentEntry;
			expr_56._BitField |= 8;
			this._currentEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			this._currentEntry.CompressionLevel = this.CompressionLevel;
			this._currentEntry.CompressionMethod = this.CompressionMethod;
			this._currentEntry.Password = this._password;
			this._currentEntry.Encryption = this.Encryption;
			this._currentEntry.AlternateEncoding = this.AlternateEncoding;
			this._currentEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
			if (entryName.EndsWith("/"))
			{
				this._currentEntry.MarkAsDirectory();
			}
			this._currentEntry.EmitTimesInWindowsFormatWhenSaving = ((this._timestamp & ZipEntryTimestamp.Windows) != ZipEntryTimestamp.None);
			this._currentEntry.EmitTimesInUnixFormatWhenSaving = ((this._timestamp & ZipEntryTimestamp.Unix) != ZipEntryTimestamp.None);
			this.InsureUniqueEntry(this._currentEntry);
			this._needToWriteEntryHeader = true;
			return this._currentEntry;
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00057454 File Offset: 0x00055654
		private void _InitiateCurrentEntry(bool finishing)
		{
			this._entriesWritten.Add(this._currentEntry.FileName, this._currentEntry);
			this._entryCount++;
			if (this._entryCount > 65534 && this._zip64 == Zip64Option.Default)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Too many entries. Consider setting ZipOutputStream.EnableZip64.");
			}
			this._currentEntry.WriteHeader(this._outputStream, finishing ? 99 : 0);
			this._currentEntry.StoreRelativeOffset();
			if (!this._currentEntry.IsDirectory)
			{
				this._currentEntry.WriteSecurityMetadata(this._outputStream);
				this._currentEntry.PrepOutputStream(this._outputStream, finishing ? 0L : -1L, out this._outputCounter, out this._encryptor, out this._deflater, out this._entryOutputStream);
			}
			this._needToWriteEntryHeader = false;
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0005752C File Offset: 0x0005572C
		private void _FinishCurrentEntry()
		{
			if (this._currentEntry != null)
			{
				if (this._needToWriteEntryHeader)
				{
					this._InitiateCurrentEntry(true);
				}
				this._currentEntry.FinishOutputStream(this._outputStream, this._outputCounter, this._encryptor, this._deflater, this._entryOutputStream);
				this._currentEntry.PostProcessOutput(this._outputStream);
				if (this._currentEntry.OutputUsedZip64.HasValue)
				{
					this._anyEntriesUsedZip64 |= this._currentEntry.OutputUsedZip64.Value;
				}
				this._outputCounter = null;
				this._encryptor = (this._deflater = null);
				this._entryOutputStream = null;
			}
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x000575E0 File Offset: 0x000557E0
		protected override void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing && !this._exceptionPending)
			{
				this._FinishCurrentEntry();
				this._directoryNeededZip64 = ZipOutput.WriteCentralDirectoryStructure(this._outputStream, this._entriesWritten.Values, 1u, this._zip64, this.Comment, new ZipContainer(this));
				CountingStream countingStream = this._outputStream as CountingStream;
				Stream stream;
				if (countingStream != null)
				{
					stream = countingStream.WrappedStream;
					countingStream.Dispose();
				}
				else
				{
					stream = this._outputStream;
				}
				if (!this._leaveUnderlyingStreamOpen)
				{
					stream.Dispose();
				}
				this._outputStream = null;
			}
			this._disposed = true;
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x00057679 File Offset: 0x00055879
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x0005767C File Offset: 0x0005587C
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0005767F File Offset: 0x0005587F
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000F25 RID: 3877 RVA: 0x00057682 File Offset: 0x00055882
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x00057689 File Offset: 0x00055889
		// (set) Token: 0x06000F27 RID: 3879 RVA: 0x00057696 File Offset: 0x00055896
		public override long Position
		{
			get
			{
				return this._outputStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0005769D File Offset: 0x0005589D
		public override void Flush()
		{
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0005769F File Offset: 0x0005589F
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Read");
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x000576AB File Offset: 0x000558AB
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("Seek");
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x000576B7 File Offset: 0x000558B7
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400083E RID: 2110
		private EncryptionAlgorithm _encryption;

		// Token: 0x0400083F RID: 2111
		private ZipEntryTimestamp _timestamp;

		// Token: 0x04000840 RID: 2112
		internal string _password;

		// Token: 0x04000841 RID: 2113
		private string _comment;

		// Token: 0x04000842 RID: 2114
		private Stream _outputStream;

		// Token: 0x04000843 RID: 2115
		private ZipEntry _currentEntry;

		// Token: 0x04000844 RID: 2116
		internal Zip64Option _zip64;

		// Token: 0x04000845 RID: 2117
		private Dictionary<string, ZipEntry> _entriesWritten;

		// Token: 0x04000846 RID: 2118
		private int _entryCount;

		// Token: 0x04000847 RID: 2119
		private ZipOption _alternateEncodingUsage;

		// Token: 0x04000848 RID: 2120
		private Encoding _alternateEncoding;

		// Token: 0x04000849 RID: 2121
		private bool _leaveUnderlyingStreamOpen;

		// Token: 0x0400084A RID: 2122
		private bool _disposed;

		// Token: 0x0400084B RID: 2123
		private bool _exceptionPending;

		// Token: 0x0400084C RID: 2124
		private bool _anyEntriesUsedZip64;

		// Token: 0x0400084D RID: 2125
		private bool _directoryNeededZip64;

		// Token: 0x0400084E RID: 2126
		private CountingStream _outputCounter;

		// Token: 0x0400084F RID: 2127
		private Stream _encryptor;

		// Token: 0x04000850 RID: 2128
		private Stream _deflater;

		// Token: 0x04000851 RID: 2129
		private CrcCalculatorStream _entryOutputStream;

		// Token: 0x04000852 RID: 2130
		private bool _needToWriteEntryHeader;

		// Token: 0x04000853 RID: 2131
		private string _name;

		// Token: 0x04000854 RID: 2132
		private bool _DontIgnoreCase;

		// Token: 0x04000855 RID: 2133
		internal ParallelDeflateOutputStream ParallelDeflater;

		// Token: 0x04000856 RID: 2134
		private long _ParallelDeflateThreshold;

		// Token: 0x04000857 RID: 2135
		private int _maxBufferPairs;
	}
}
