using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x02000161 RID: 353
	internal class ZipSegmentedStream : Stream
	{
		// Token: 0x06000F3E RID: 3902 RVA: 0x00057957 File Offset: 0x00055B57
		private ZipSegmentedStream()
		{
			this._exceptionPending = false;
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00057968 File Offset: 0x00055B68
		public static ZipSegmentedStream ForReading(string name, uint initialDiskNumber, uint maxDiskNumber)
		{
			ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream
			{
				rwMode = ZipSegmentedStream.RwMode.ReadOnly,
				CurrentSegment = initialDiskNumber,
				_maxDiskNumber = maxDiskNumber,
				_baseName = name
			};
			zipSegmentedStream._SetReadStream();
			return zipSegmentedStream;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x000579A0 File Offset: 0x00055BA0
		public static ZipSegmentedStream ForWriting(string name, int maxSegmentSize)
		{
			ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream
			{
				rwMode = ZipSegmentedStream.RwMode.Write,
				CurrentSegment = 0u,
				_baseName = name,
				_maxSegmentSize = maxSegmentSize,
				_baseDir = Path.GetDirectoryName(name)
			};
			if (zipSegmentedStream._baseDir == "")
			{
				zipSegmentedStream._baseDir = ".";
			}
			zipSegmentedStream._SetWriteStream(0u);
			return zipSegmentedStream;
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00057A04 File Offset: 0x00055C04
		public static Stream ForUpdate(string name, uint diskNumber)
		{
			if (diskNumber >= 99u)
			{
				throw new ArgumentOutOfRangeException("diskNumber");
			}
			string path = string.Format("{0}.z{1:D2}", Path.Combine(Path.GetDirectoryName(name), Path.GetFileNameWithoutExtension(name)), diskNumber + 1u);
			return File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x00057A4E File Offset: 0x00055C4E
		// (set) Token: 0x06000F43 RID: 3907 RVA: 0x00057A56 File Offset: 0x00055C56
		public bool ContiguousWrite
		{
			get;
			set;
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000F44 RID: 3908 RVA: 0x00057A5F File Offset: 0x00055C5F
		// (set) Token: 0x06000F45 RID: 3909 RVA: 0x00057A67 File Offset: 0x00055C67
		public uint CurrentSegment
		{
			get
			{
				return this._currentDiskNumber;
			}
			private set
			{
				this._currentDiskNumber = value;
				this._currentName = null;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000F46 RID: 3910 RVA: 0x00057A77 File Offset: 0x00055C77
		public string CurrentName
		{
			get
			{
				if (this._currentName == null)
				{
					this._currentName = this._NameForSegment(this.CurrentSegment);
				}
				return this._currentName;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000F47 RID: 3911 RVA: 0x00057A99 File Offset: 0x00055C99
		public string CurrentTempName
		{
			get
			{
				return this._currentTempName;
			}
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x00057AA4 File Offset: 0x00055CA4
		private string _NameForSegment(uint diskNumber)
		{
			if (diskNumber >= 99u)
			{
				this._exceptionPending = true;
				throw new OverflowException("The number of zip segments would exceed 99.");
			}
			return string.Format("{0}.z{1:D2}", Path.Combine(Path.GetDirectoryName(this._baseName), Path.GetFileNameWithoutExtension(this._baseName)), diskNumber + 1u);
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00057AF5 File Offset: 0x00055CF5
		public uint ComputeSegment(int length)
		{
			if (this._innerStream.Position + (long)length > (long)this._maxSegmentSize)
			{
				return this.CurrentSegment + 1u;
			}
			return this.CurrentSegment;
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x00057B20 File Offset: 0x00055D20
		public override string ToString()
		{
			return string.Format("{0}[{1}][{2}], pos=0x{3:X})", new object[]
			{
				"ZipSegmentedStream",
				this.CurrentName,
				this.rwMode.ToString(),
				this.Position
			});
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x00057B74 File Offset: 0x00055D74
		private void _SetReadStream()
		{
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
			}
			if (this.CurrentSegment + 1u == this._maxDiskNumber)
			{
				this._currentName = this._baseName;
			}
			this._innerStream = File.OpenRead(this.CurrentName);
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x00057BC4 File Offset: 0x00055DC4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.ReadOnly)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Stream Error: Cannot Read.");
			}
			int num = this._innerStream.Read(buffer, offset, count);
			int num2 = num;
			while (num2 != count)
			{
				if (this._innerStream.Position != this._innerStream.Length)
				{
					this._exceptionPending = true;
					throw new ZipException(string.Format("Read error in file {0}", this.CurrentName));
				}
				if (this.CurrentSegment + 1u == this._maxDiskNumber)
				{
					return num;
				}
				this.CurrentSegment += 1u;
				this._SetReadStream();
				offset += num2;
				count -= num2;
				num2 = this._innerStream.Read(buffer, offset, count);
				num += num2;
			}
			return num;
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00057C7C File Offset: 0x00055E7C
		private void _SetWriteStream(uint increment)
		{
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
				if (File.Exists(this.CurrentName))
				{
					File.Delete(this.CurrentName);
				}
				File.Move(this._currentTempName, this.CurrentName);
			}
			if (increment > 0u)
			{
				this.CurrentSegment += increment;
			}
			SharedUtilities.CreateAndOpenUniqueTempFile(this._baseDir, out this._innerStream, out this._currentTempName);
			if (this.CurrentSegment == 0u)
			{
				this._innerStream.Write(BitConverter.GetBytes(134695760), 0, 4);
			}
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00057D10 File Offset: 0x00055F10
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Stream Error: Cannot Write.");
			}
			if (this.ContiguousWrite)
			{
				if (this._innerStream.Position + (long)count > (long)this._maxSegmentSize)
				{
					this._SetWriteStream(1u);
				}
			}
			else
			{
				while (this._innerStream.Position + (long)count > (long)this._maxSegmentSize)
				{
					int num = this._maxSegmentSize - (int)this._innerStream.Position;
					this._innerStream.Write(buffer, offset, num);
					this._SetWriteStream(1u);
					count -= num;
					offset += num;
				}
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00057DB8 File Offset: 0x00055FB8
		public long TruncateBackward(uint diskNumber, long offset)
		{
			if (diskNumber >= 99u)
			{
				throw new ArgumentOutOfRangeException("diskNumber");
			}
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new ZipException("bad state.");
			}
			if (diskNumber == this.CurrentSegment)
			{
				return this._innerStream.Seek(offset, SeekOrigin.Begin);
			}
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
				if (File.Exists(this._currentTempName))
				{
					File.Delete(this._currentTempName);
				}
			}
			for (uint num = this.CurrentSegment - 1u; num > diskNumber; num -= 1u)
			{
				string path = this._NameForSegment(num);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			this.CurrentSegment = diskNumber;
			for (int i = 0; i < 3; i++)
			{
				try
				{
					this._currentTempName = SharedUtilities.InternalGetTempFileName();
					File.Move(this.CurrentName, this._currentTempName);
					break;
				}
				catch (IOException)
				{
					if (i == 2)
					{
						throw;
					}
				}
			}
			this._innerStream = new FileStream(this._currentTempName, FileMode.Open);
			return this._innerStream.Seek(offset, SeekOrigin.Begin);
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000F50 RID: 3920 RVA: 0x00057ECC File Offset: 0x000560CC
		public override bool CanRead
		{
			get
			{
				return this.rwMode == ZipSegmentedStream.RwMode.ReadOnly && this._innerStream != null && this._innerStream.CanRead;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000F51 RID: 3921 RVA: 0x00057EEC File Offset: 0x000560EC
		public override bool CanSeek
		{
			get
			{
				return this._innerStream != null && this._innerStream.CanSeek;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000F52 RID: 3922 RVA: 0x00057F03 File Offset: 0x00056103
		public override bool CanWrite
		{
			get
			{
				return this.rwMode == ZipSegmentedStream.RwMode.Write && this._innerStream != null && this._innerStream.CanWrite;
			}
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00057F23 File Offset: 0x00056123
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000F54 RID: 3924 RVA: 0x00057F30 File Offset: 0x00056130
		public override long Length
		{
			get
			{
				return this._innerStream.Length;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x00057F3D File Offset: 0x0005613D
		// (set) Token: 0x06000F56 RID: 3926 RVA: 0x00057F4A File Offset: 0x0005614A
		public override long Position
		{
			get
			{
				return this._innerStream.Position;
			}
			set
			{
				this._innerStream.Position = value;
			}
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00057F58 File Offset: 0x00056158
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._innerStream.Seek(offset, origin);
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x00057F74 File Offset: 0x00056174
		public override void SetLength(long value)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException();
			}
			this._innerStream.SetLength(value);
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x00057F98 File Offset: 0x00056198
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (this._innerStream != null)
				{
					this._innerStream.Dispose();
					if (this.rwMode == ZipSegmentedStream.RwMode.Write)
					{
						bool arg_22_0 = this._exceptionPending;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x0400085F RID: 2143
		private ZipSegmentedStream.RwMode rwMode;

		// Token: 0x04000860 RID: 2144
		private bool _exceptionPending;

		// Token: 0x04000861 RID: 2145
		private string _baseName;

		// Token: 0x04000862 RID: 2146
		private string _baseDir;

		// Token: 0x04000863 RID: 2147
		private string _currentName;

		// Token: 0x04000864 RID: 2148
		private string _currentTempName;

		// Token: 0x04000865 RID: 2149
		private uint _currentDiskNumber;

		// Token: 0x04000866 RID: 2150
		private uint _maxDiskNumber;

		// Token: 0x04000867 RID: 2151
		private int _maxSegmentSize;

		// Token: 0x04000868 RID: 2152
		private Stream _innerStream;

		// Token: 0x02000162 RID: 354
		private enum RwMode
		{
			// Token: 0x0400086B RID: 2155
			None,
			// Token: 0x0400086C RID: 2156
			ReadOnly,
			// Token: 0x0400086D RID: 2157
			Write
		}
	}
}
