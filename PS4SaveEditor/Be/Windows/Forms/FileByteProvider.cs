using System;
using System.Collections;
using System.IO;

namespace Be.Windows.Forms
{
	// Token: 0x0200004E RID: 78
	public class FileByteProvider : IByteProvider, IDisposable
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600037C RID: 892 RVA: 0x00012B1C File Offset: 0x00010D1C
		// (remove) Token: 0x0600037D RID: 893 RVA: 0x00012B54 File Offset: 0x00010D54
		public event EventHandler Changed;

		// Token: 0x0600037E RID: 894 RVA: 0x00012B8C File Offset: 0x00010D8C
		public FileByteProvider(string fileName)
		{
			this._fileName = fileName;
			try
			{
				this._fileStream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
			}
			catch
			{
				try
				{
					this._fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					this._readOnly = true;
				}
				catch
				{
					throw;
				}
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00012BFC File Offset: 0x00010DFC
		~FileByteProvider()
		{
			this.Dispose();
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00012C28 File Offset: 0x00010E28
		private void OnChanged(EventArgs e)
		{
			if (this.Changed != null)
			{
				this.Changed(this, e);
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00012C3F File Offset: 0x00010E3F
		public string FileName
		{
			get
			{
				return this._fileName;
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00012C47 File Offset: 0x00010E47
		public bool HasChanges()
		{
			return this._writes.Count > 0;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00012C58 File Offset: 0x00010E58
		public void ApplyChanges()
		{
			if (this._readOnly)
			{
				throw new Exception("File is in read-only mode.");
			}
			if (!this.HasChanges())
			{
				return;
			}
			IDictionaryEnumerator enumerator = this._writes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				long num = (long)enumerator.Key;
				byte b = (byte)enumerator.Value;
				if (this._fileStream.Position != num)
				{
					this._fileStream.Position = num;
				}
				this._fileStream.Write(new byte[]
				{
					b
				}, 0, 1);
			}
			this._writes.Clear();
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00012CED File Offset: 0x00010EED
		public void RejectChanges()
		{
			this._writes.Clear();
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000385 RID: 901 RVA: 0x00012CFC File Offset: 0x00010EFC
		// (remove) Token: 0x06000386 RID: 902 RVA: 0x00012D34 File Offset: 0x00010F34
		public event EventHandler LengthChanged;

		// Token: 0x06000387 RID: 903 RVA: 0x00012D6C File Offset: 0x00010F6C
		public byte ReadByte(long index)
		{
			if (this._writes.Contains(index))
			{
				return this._writes[index];
			}
			if (this._fileStream.Position != index)
			{
				this._fileStream.Position = index;
			}
			return (byte)this._fileStream.ReadByte();
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00012DBC File Offset: 0x00010FBC
		public long Length
		{
			get
			{
				return this._fileStream.Length;
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00012DC9 File Offset: 0x00010FC9
		public void WriteByte(long index, byte value)
		{
			if (this._writes.Contains(index))
			{
				this._writes[index] = value;
			}
			else
			{
				this._writes.Add(index, value);
			}
			this.OnChanged(EventArgs.Empty);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00012E00 File Offset: 0x00011000
		public void DeleteBytes(long index, long length)
		{
			throw new NotSupportedException("FileByteProvider.DeleteBytes");
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00012E0C File Offset: 0x0001100C
		public void InsertBytes(long index, byte[] bs)
		{
			throw new NotSupportedException("FileByteProvider.InsertBytes");
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00012E18 File Offset: 0x00011018
		public bool SupportsWriteByte()
		{
			return !this._readOnly;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00012E23 File Offset: 0x00011023
		public bool SupportsInsertBytes()
		{
			return false;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00012E26 File Offset: 0x00011026
		public bool SupportsDeleteBytes()
		{
			return false;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00012E29 File Offset: 0x00011029
		public void Dispose()
		{
			if (this._fileStream != null)
			{
				this._fileName = null;
				this._fileStream.Close();
				this._fileStream = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x040001B6 RID: 438
		private FileByteProvider.WriteCollection _writes = new FileByteProvider.WriteCollection();

		// Token: 0x040001B7 RID: 439
		private string _fileName;

		// Token: 0x040001B8 RID: 440
		private FileStream _fileStream;

		// Token: 0x040001B9 RID: 441
		private bool _readOnly;

		// Token: 0x0200004F RID: 79
		private class WriteCollection : DictionaryBase
		{
			// Token: 0x17000199 RID: 409
			public byte this[long index]
			{
				get
				{
					return (byte)base.Dictionary[index];
				}
				set
				{
					base.Dictionary[index] = value;
				}
			}

			// Token: 0x06000392 RID: 914 RVA: 0x00012E83 File Offset: 0x00011083
			public void Add(long index, byte value)
			{
				base.Dictionary.Add(index, value);
			}

			// Token: 0x06000393 RID: 915 RVA: 0x00012E9C File Offset: 0x0001109C
			public bool Contains(long index)
			{
				return base.Dictionary.Contains(index);
			}
		}
	}
}
