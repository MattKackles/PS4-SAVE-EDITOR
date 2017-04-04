using System;
using System.IO;
using System.Text;
using Ionic.Crc;

namespace Ionic.Zip
{
	// Token: 0x0200015E RID: 350
	public class ZipInputStream : Stream
	{
		// Token: 0x06000EDA RID: 3802 RVA: 0x00056B66 File Offset: 0x00054D66
		public ZipInputStream(Stream stream) : this(stream, false)
		{
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x00056B70 File Offset: 0x00054D70
		public ZipInputStream(string fileName)
		{
			Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this._Init(stream, false, fileName);
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00056B96 File Offset: 0x00054D96
		public ZipInputStream(Stream stream, bool leaveOpen)
		{
			this._Init(stream, leaveOpen, null);
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x00056BA8 File Offset: 0x00054DA8
		private void _Init(Stream stream, bool leaveOpen, string name)
		{
			this._inputStream = stream;
			if (!this._inputStream.CanRead)
			{
				throw new ZipException("The stream must be readable.");
			}
			this._container = new ZipContainer(this);
			this._provisionalAlternateEncoding = Encoding.GetEncoding("IBM437");
			this._leaveUnderlyingStreamOpen = leaveOpen;
			this._findRequired = true;
			this._name = (name ?? "(stream)");
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x00056C0E File Offset: 0x00054E0E
		public override string ToString()
		{
			return string.Format("ZipInputStream::{0}(leaveOpen({1})))", this._name, this._leaveUnderlyingStreamOpen);
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x00056C2B File Offset: 0x00054E2B
		// (set) Token: 0x06000EE0 RID: 3808 RVA: 0x00056C33 File Offset: 0x00054E33
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				return this._provisionalAlternateEncoding;
			}
			set
			{
				this._provisionalAlternateEncoding = value;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x00056C3C File Offset: 0x00054E3C
		// (set) Token: 0x06000EE2 RID: 3810 RVA: 0x00056C44 File Offset: 0x00054E44
		public int CodecBufferSize
		{
			get;
			set;
		}

		// Token: 0x170003D1 RID: 977
		// (set) Token: 0x06000EE3 RID: 3811 RVA: 0x00056C4D File Offset: 0x00054E4D
		public string Password
		{
			set
			{
				if (this._closed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._Password = value;
			}
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00056C70 File Offset: 0x00054E70
		private void SetupStream()
		{
			this._crcStream = this._currentEntry.InternalOpenReader(this._Password);
			this._LeftToRead = this._crcStream.Length;
			this._needSetup = false;
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x00056CA1 File Offset: 0x00054EA1
		internal Stream ReadStream
		{
			get
			{
				return this._inputStream;
			}
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00056CAC File Offset: 0x00054EAC
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._closed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			if (this._needSetup)
			{
				this.SetupStream();
			}
			if (this._LeftToRead == 0L)
			{
				return 0;
			}
			int count2 = (this._LeftToRead > (long)count) ? count : ((int)this._LeftToRead);
			int num = this._crcStream.Read(buffer, offset, count2);
			this._LeftToRead -= (long)num;
			if (this._LeftToRead == 0L)
			{
				int crc = this._crcStream.Crc;
				this._currentEntry.VerifyCrcAfterExtract(crc);
				this._inputStream.Seek(this._endOfEntry, SeekOrigin.Begin);
			}
			return num;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00056D58 File Offset: 0x00054F58
		public ZipEntry GetNextEntry()
		{
			if (this._findRequired)
			{
				long num = SharedUtilities.FindSignature(this._inputStream, 67324752);
				if (num == -1L)
				{
					return null;
				}
				this._inputStream.Seek(-4L, SeekOrigin.Current);
			}
			else if (this._firstEntry)
			{
				this._inputStream.Seek(this._endOfEntry, SeekOrigin.Begin);
			}
			this._currentEntry = ZipEntry.ReadEntry(this._container, !this._firstEntry);
			this._endOfEntry = this._inputStream.Position;
			this._firstEntry = true;
			this._needSetup = true;
			this._findRequired = false;
			return this._currentEntry;
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00056DF8 File Offset: 0x00054FF8
		protected override void Dispose(bool disposing)
		{
			if (this._closed)
			{
				return;
			}
			if (disposing)
			{
				if (this._exceptionPending)
				{
					return;
				}
				if (!this._leaveUnderlyingStreamOpen)
				{
					this._inputStream.Dispose();
				}
			}
			this._closed = true;
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x00056E29 File Offset: 0x00055029
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000EEA RID: 3818 RVA: 0x00056E2C File Offset: 0x0005502C
		public override bool CanSeek
		{
			get
			{
				return this._inputStream.CanSeek;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000EEB RID: 3819 RVA: 0x00056E39 File Offset: 0x00055039
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000EEC RID: 3820 RVA: 0x00056E3C File Offset: 0x0005503C
		public override long Length
		{
			get
			{
				return this._inputStream.Length;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000EED RID: 3821 RVA: 0x00056E49 File Offset: 0x00055049
		// (set) Token: 0x06000EEE RID: 3822 RVA: 0x00056E56 File Offset: 0x00055056
		public override long Position
		{
			get
			{
				return this._inputStream.Position;
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00056E61 File Offset: 0x00055061
		public override void Flush()
		{
			throw new NotSupportedException("Flush");
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00056E6D File Offset: 0x0005506D
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Write");
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00056E7C File Offset: 0x0005507C
		public override long Seek(long offset, SeekOrigin origin)
		{
			this._findRequired = true;
			return this._inputStream.Seek(offset, origin);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00056E9F File Offset: 0x0005509F
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400082E RID: 2094
		private Stream _inputStream;

		// Token: 0x0400082F RID: 2095
		private Encoding _provisionalAlternateEncoding;

		// Token: 0x04000830 RID: 2096
		private ZipEntry _currentEntry;

		// Token: 0x04000831 RID: 2097
		private bool _firstEntry;

		// Token: 0x04000832 RID: 2098
		private bool _needSetup;

		// Token: 0x04000833 RID: 2099
		private ZipContainer _container;

		// Token: 0x04000834 RID: 2100
		private CrcCalculatorStream _crcStream;

		// Token: 0x04000835 RID: 2101
		private long _LeftToRead;

		// Token: 0x04000836 RID: 2102
		internal string _Password;

		// Token: 0x04000837 RID: 2103
		private long _endOfEntry;

		// Token: 0x04000838 RID: 2104
		private string _name;

		// Token: 0x04000839 RID: 2105
		private bool _leaveUnderlyingStreamOpen;

		// Token: 0x0400083A RID: 2106
		private bool _closed;

		// Token: 0x0400083B RID: 2107
		private bool _findRequired;

		// Token: 0x0400083C RID: 2108
		private bool _exceptionPending;
	}
}
