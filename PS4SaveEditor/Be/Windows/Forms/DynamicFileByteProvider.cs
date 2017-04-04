using System;
using System.IO;

namespace Be.Windows.Forms
{
	// Token: 0x0200004D RID: 77
	public sealed class DynamicFileByteProvider : IByteProvider, IDisposable
	{
		// Token: 0x06000361 RID: 865 RVA: 0x00012117 File Offset: 0x00010317
		public DynamicFileByteProvider(string fileName) : this(fileName, false)
		{
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00012121 File Offset: 0x00010321
		public DynamicFileByteProvider(string fileName, bool readOnly)
		{
			this._fileName = fileName;
			if (!readOnly)
			{
				this._stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
			}
			else
			{
				this._stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			}
			this._readOnly = readOnly;
			this.ReInitialize();
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00012160 File Offset: 0x00010360
		public DynamicFileByteProvider(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanSeek)
			{
				throw new ArgumentException("stream must supported seek operations(CanSeek)");
			}
			this._stream = stream;
			this._readOnly = !stream.CanWrite;
			this.ReInitialize();
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000364 RID: 868 RVA: 0x000121B0 File Offset: 0x000103B0
		// (remove) Token: 0x06000365 RID: 869 RVA: 0x000121E8 File Offset: 0x000103E8
		public event EventHandler LengthChanged;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000366 RID: 870 RVA: 0x00012220 File Offset: 0x00010420
		// (remove) Token: 0x06000367 RID: 871 RVA: 0x00012258 File Offset: 0x00010458
		public event EventHandler Changed;

		// Token: 0x06000368 RID: 872 RVA: 0x00012290 File Offset: 0x00010490
		public byte ReadByte(long index)
		{
			long num;
			DataBlock dataBlock = this.GetDataBlock(index, out num);
			FileDataBlock fileDataBlock = dataBlock as FileDataBlock;
			if (fileDataBlock != null)
			{
				return this.ReadByteFromFile(fileDataBlock.FileOffset + index - num);
			}
			MemoryDataBlock memoryDataBlock = (MemoryDataBlock)dataBlock;
			return memoryDataBlock.Data[(int)(checked((IntPtr)(unchecked(index - num))))];
		}

		// Token: 0x06000369 RID: 873 RVA: 0x000122D4 File Offset: 0x000104D4
		public void WriteByte(long index, byte value)
		{
			try
			{
				long num;
				DataBlock dataBlock = this.GetDataBlock(index, out num);
				MemoryDataBlock memoryDataBlock = dataBlock as MemoryDataBlock;
				if (memoryDataBlock != null)
				{
					memoryDataBlock.Data[(int)(checked((IntPtr)(unchecked(index - num))))] = value;
				}
				else
				{
					FileDataBlock fileDataBlock = (FileDataBlock)dataBlock;
					if (num == index && dataBlock.PreviousBlock != null)
					{
						MemoryDataBlock memoryDataBlock2 = dataBlock.PreviousBlock as MemoryDataBlock;
						if (memoryDataBlock2 != null)
						{
							memoryDataBlock2.AddByteToEnd(value);
							fileDataBlock.RemoveBytesFromStart(1L);
							if (fileDataBlock.Length == 0L)
							{
								this._dataMap.Remove(fileDataBlock);
							}
							return;
						}
					}
					if (num + fileDataBlock.Length - 1L == index && dataBlock.NextBlock != null)
					{
						MemoryDataBlock memoryDataBlock3 = dataBlock.NextBlock as MemoryDataBlock;
						if (memoryDataBlock3 != null)
						{
							memoryDataBlock3.AddByteToStart(value);
							fileDataBlock.RemoveBytesFromEnd(1L);
							if (fileDataBlock.Length == 0L)
							{
								this._dataMap.Remove(fileDataBlock);
							}
							return;
						}
					}
					FileDataBlock fileDataBlock2 = null;
					if (index > num)
					{
						fileDataBlock2 = new FileDataBlock(fileDataBlock.FileOffset, index - num);
					}
					FileDataBlock fileDataBlock3 = null;
					if (index < num + fileDataBlock.Length - 1L)
					{
						fileDataBlock3 = new FileDataBlock(fileDataBlock.FileOffset + index - num + 1L, fileDataBlock.Length - (index - num + 1L));
					}
					dataBlock = this._dataMap.Replace(dataBlock, new MemoryDataBlock(value));
					if (fileDataBlock2 != null)
					{
						this._dataMap.AddBefore(dataBlock, fileDataBlock2);
					}
					if (fileDataBlock3 != null)
					{
						this._dataMap.AddAfter(dataBlock, fileDataBlock3);
					}
				}
			}
			finally
			{
				this.OnChanged(EventArgs.Empty);
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00012454 File Offset: 0x00010654
		public void InsertBytes(long index, byte[] bs)
		{
			try
			{
				long num;
				DataBlock dataBlock = this.GetDataBlock(index, out num);
				MemoryDataBlock memoryDataBlock = dataBlock as MemoryDataBlock;
				if (memoryDataBlock != null)
				{
					memoryDataBlock.InsertBytes(index - num, bs);
				}
				else
				{
					FileDataBlock fileDataBlock = (FileDataBlock)dataBlock;
					if (num == index && dataBlock.PreviousBlock != null)
					{
						MemoryDataBlock memoryDataBlock2 = dataBlock.PreviousBlock as MemoryDataBlock;
						if (memoryDataBlock2 != null)
						{
							memoryDataBlock2.InsertBytes(memoryDataBlock2.Length, bs);
							return;
						}
					}
					FileDataBlock fileDataBlock2 = null;
					if (index > num)
					{
						fileDataBlock2 = new FileDataBlock(fileDataBlock.FileOffset, index - num);
					}
					FileDataBlock fileDataBlock3 = null;
					if (index < num + fileDataBlock.Length)
					{
						fileDataBlock3 = new FileDataBlock(fileDataBlock.FileOffset + index - num, fileDataBlock.Length - (index - num));
					}
					dataBlock = this._dataMap.Replace(dataBlock, new MemoryDataBlock(bs));
					if (fileDataBlock2 != null)
					{
						this._dataMap.AddBefore(dataBlock, fileDataBlock2);
					}
					if (fileDataBlock3 != null)
					{
						this._dataMap.AddAfter(dataBlock, fileDataBlock3);
					}
				}
			}
			finally
			{
				this._totalLength += (long)bs.Length;
				this.OnLengthChanged(EventArgs.Empty);
				this.OnChanged(EventArgs.Empty);
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00012570 File Offset: 0x00010770
		public void DeleteBytes(long index, long length)
		{
			try
			{
				long num = length;
				long num2;
				DataBlock dataBlock = this.GetDataBlock(index, out num2);
				while (num > 0L && dataBlock != null)
				{
					long length2 = dataBlock.Length;
					DataBlock nextBlock = dataBlock.NextBlock;
					long num3 = Math.Min(num, length2 - (index - num2));
					dataBlock.RemoveBytes(index - num2, num3);
					if (dataBlock.Length == 0L)
					{
						this._dataMap.Remove(dataBlock);
						if (this._dataMap.FirstBlock == null)
						{
							this._dataMap.AddFirst(new MemoryDataBlock(new byte[0]));
						}
					}
					num -= num3;
					num2 += dataBlock.Length;
					dataBlock = ((num > 0L) ? nextBlock : null);
				}
			}
			finally
			{
				this._totalLength -= length;
				this.OnLengthChanged(EventArgs.Empty);
				this.OnChanged(EventArgs.Empty);
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00012648 File Offset: 0x00010848
		public long Length
		{
			get
			{
				return this._totalLength;
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00012650 File Offset: 0x00010850
		public bool HasChanges()
		{
			if (this._readOnly)
			{
				return false;
			}
			if (this._totalLength != this._stream.Length)
			{
				return true;
			}
			long num = 0L;
			for (DataBlock dataBlock = this._dataMap.FirstBlock; dataBlock != null; dataBlock = dataBlock.NextBlock)
			{
				FileDataBlock fileDataBlock = dataBlock as FileDataBlock;
				if (fileDataBlock == null)
				{
					return true;
				}
				if (fileDataBlock.FileOffset != num)
				{
					return true;
				}
				num += fileDataBlock.Length;
			}
			return num != this._stream.Length;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x000126C8 File Offset: 0x000108C8
		public void ApplyChanges()
		{
			if (this._readOnly)
			{
				throw new OperationCanceledException("File is in read-only mode");
			}
			if (this._totalLength > this._stream.Length)
			{
				this._stream.SetLength(this._totalLength);
			}
			long num = 0L;
			for (DataBlock dataBlock = this._dataMap.FirstBlock; dataBlock != null; dataBlock = dataBlock.NextBlock)
			{
				FileDataBlock fileDataBlock = dataBlock as FileDataBlock;
				if (fileDataBlock != null && fileDataBlock.FileOffset != num)
				{
					this.MoveFileBlock(fileDataBlock, num);
				}
				num += dataBlock.Length;
			}
			num = 0L;
			for (DataBlock dataBlock2 = this._dataMap.FirstBlock; dataBlock2 != null; dataBlock2 = dataBlock2.NextBlock)
			{
				MemoryDataBlock memoryDataBlock = dataBlock2 as MemoryDataBlock;
				if (memoryDataBlock != null)
				{
					this._stream.Position = num;
					int num2 = 0;
					while ((long)num2 < memoryDataBlock.Length)
					{
						this._stream.Write(memoryDataBlock.Data, num2, (int)Math.Min(4096L, memoryDataBlock.Length - (long)num2));
						num2 += 4096;
					}
				}
				num += dataBlock2.Length;
			}
			this._stream.SetLength(this._totalLength);
			this.ReInitialize();
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000127E4 File Offset: 0x000109E4
		public bool SupportsWriteByte()
		{
			return !this._readOnly;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x000127EF File Offset: 0x000109EF
		public bool SupportsInsertBytes()
		{
			return !this._readOnly;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x000127FA File Offset: 0x000109FA
		public bool SupportsDeleteBytes()
		{
			return !this._readOnly;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00012808 File Offset: 0x00010A08
		~DynamicFileByteProvider()
		{
			this.Dispose();
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00012834 File Offset: 0x00010A34
		public void Dispose()
		{
			if (this._stream != null)
			{
				this._stream.Close();
				this._stream = null;
			}
			this._fileName = null;
			this._dataMap = null;
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00012864 File Offset: 0x00010A64
		public bool ReadOnly
		{
			get
			{
				return this._readOnly;
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0001286C File Offset: 0x00010A6C
		private void OnLengthChanged(EventArgs e)
		{
			if (this.LengthChanged != null)
			{
				this.LengthChanged(this, e);
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00012883 File Offset: 0x00010A83
		private void OnChanged(EventArgs e)
		{
			if (this.Changed != null)
			{
				this.Changed(this, e);
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0001289C File Offset: 0x00010A9C
		private DataBlock GetDataBlock(long findOffset, out long blockOffset)
		{
			if (findOffset < 0L || findOffset > this._totalLength)
			{
				throw new ArgumentOutOfRangeException("findOffset");
			}
			blockOffset = 0L;
			for (DataBlock dataBlock = this._dataMap.FirstBlock; dataBlock != null; dataBlock = dataBlock.NextBlock)
			{
				if ((blockOffset <= findOffset && blockOffset + dataBlock.Length > findOffset) || dataBlock.NextBlock == null)
				{
					return dataBlock;
				}
				blockOffset += dataBlock.Length;
			}
			return null;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00012908 File Offset: 0x00010B08
		private FileDataBlock GetNextFileDataBlock(DataBlock block, long dataOffset, out long nextDataOffset)
		{
			nextDataOffset = dataOffset + block.Length;
			for (block = block.NextBlock; block != null; block = block.NextBlock)
			{
				FileDataBlock fileDataBlock = block as FileDataBlock;
				if (fileDataBlock != null)
				{
					return fileDataBlock;
				}
				nextDataOffset += block.Length;
			}
			return null;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0001294C File Offset: 0x00010B4C
		private byte ReadByteFromFile(long fileOffset)
		{
			if (this._stream.Position != fileOffset)
			{
				this._stream.Position = fileOffset;
			}
			return (byte)this._stream.ReadByte();
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00012974 File Offset: 0x00010B74
		private void MoveFileBlock(FileDataBlock fileBlock, long dataOffset)
		{
			long dataOffset2;
			FileDataBlock nextFileDataBlock = this.GetNextFileDataBlock(fileBlock, dataOffset, out dataOffset2);
			if (nextFileDataBlock != null && dataOffset + fileBlock.Length > nextFileDataBlock.FileOffset)
			{
				this.MoveFileBlock(nextFileDataBlock, dataOffset2);
			}
			if (fileBlock.FileOffset > dataOffset)
			{
				byte[] array = new byte[4096];
				for (long num = 0L; num < fileBlock.Length; num += (long)array.Length)
				{
					long position = fileBlock.FileOffset + num;
					int count = (int)Math.Min((long)array.Length, fileBlock.Length - num);
					this._stream.Position = position;
					this._stream.Read(array, 0, count);
					long position2 = dataOffset + num;
					this._stream.Position = position2;
					this._stream.Write(array, 0, count);
				}
			}
			else
			{
				byte[] array2 = new byte[4096];
				for (long num2 = 0L; num2 < fileBlock.Length; num2 += (long)array2.Length)
				{
					int num3 = (int)Math.Min((long)array2.Length, fileBlock.Length - num2);
					long position3 = fileBlock.FileOffset + fileBlock.Length - num2 - (long)num3;
					this._stream.Position = position3;
					this._stream.Read(array2, 0, num3);
					long position4 = dataOffset + fileBlock.Length - num2 - (long)num3;
					this._stream.Position = position4;
					this._stream.Write(array2, 0, num3);
				}
			}
			fileBlock.SetFileOffset(dataOffset);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00012ADF File Offset: 0x00010CDF
		private void ReInitialize()
		{
			this._dataMap = new DataMap();
			this._dataMap.AddFirst(new FileDataBlock(0L, this._stream.Length));
			this._totalLength = this._stream.Length;
		}

		// Token: 0x040001AD RID: 429
		private const int COPY_BLOCK_SIZE = 4096;

		// Token: 0x040001AE RID: 430
		private string _fileName;

		// Token: 0x040001AF RID: 431
		private Stream _stream;

		// Token: 0x040001B0 RID: 432
		private DataMap _dataMap;

		// Token: 0x040001B1 RID: 433
		private long _totalLength;

		// Token: 0x040001B2 RID: 434
		private bool _readOnly;
	}
}
