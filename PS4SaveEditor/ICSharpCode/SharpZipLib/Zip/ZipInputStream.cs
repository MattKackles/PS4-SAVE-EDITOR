using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Encryption;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000102 RID: 258
	public class ZipInputStream : InflaterInputStream
	{
		// Token: 0x06000AAC RID: 2732 RVA: 0x000391AA File Offset: 0x000373AA
		public ZipInputStream(Stream baseInputStream) : base(baseInputStream, new Inflater(true))
		{
			this.internalReader = new ZipInputStream.ReadDataHandler(this.ReadingNotAvailable);
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x000391D6 File Offset: 0x000373D6
		public ZipInputStream(Stream baseInputStream, int bufferSize) : base(baseInputStream, new Inflater(true), bufferSize)
		{
			this.internalReader = new ZipInputStream.ReadDataHandler(this.ReadingNotAvailable);
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x00039203 File Offset: 0x00037403
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x0003920B File Offset: 0x0003740B
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x00039214 File Offset: 0x00037414
		public bool CanDecompressEntry
		{
			get
			{
				return this.entry != null && this.entry.CanDecompress;
			}
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0003922C File Offset: 0x0003742C
		public ZipEntry GetNextEntry()
		{
			if (this.crc == null)
			{
				throw new InvalidOperationException("Closed.");
			}
			if (this.entry != null)
			{
				this.CloseEntry();
			}
			int num = this.inputBuffer.ReadLeInt();
			if (num == 33639248 || num == 101010256 || num == 84233040 || num == 117853008 || num == 101075792)
			{
				this.Close();
				return null;
			}
			if (num == 808471376 || num == 134695760)
			{
				num = this.inputBuffer.ReadLeInt();
			}
			if (num != 67324752)
			{
				throw new ZipException("Wrong Local header signature: 0x" + string.Format("{0:X}", num));
			}
			short versionRequiredToExtract = (short)this.inputBuffer.ReadLeShort();
			this.flags = this.inputBuffer.ReadLeShort();
			this.method = this.inputBuffer.ReadLeShort();
			uint num2 = (uint)this.inputBuffer.ReadLeInt();
			int num3 = this.inputBuffer.ReadLeInt();
			this.csize = (long)this.inputBuffer.ReadLeInt();
			this.size = (long)this.inputBuffer.ReadLeInt();
			int num4 = this.inputBuffer.ReadLeShort();
			int num5 = this.inputBuffer.ReadLeShort();
			bool flag = (this.flags & 1) == 1;
			byte[] array = new byte[num4];
			this.inputBuffer.ReadRawBuffer(array);
			string name = ZipConstants.ConvertToStringExt(this.flags, array);
			this.entry = new ZipEntry(name, (int)versionRequiredToExtract);
			this.entry.Flags = this.flags;
			this.entry.CompressionMethod = (CompressionMethod)this.method;
			if ((this.flags & 8) == 0)
			{
				this.entry.Crc = ((long)num3 & (long)((ulong)-1));
				this.entry.Size = (this.size & (long)((ulong)-1));
				this.entry.CompressedSize = (this.csize & (long)((ulong)-1));
				this.entry.CryptoCheckValue = (byte)(num3 >> 24 & 255);
			}
			else
			{
				if (num3 != 0)
				{
					this.entry.Crc = ((long)num3 & (long)((ulong)-1));
				}
				if (this.size != 0L)
				{
					this.entry.Size = (this.size & (long)((ulong)-1));
				}
				if (this.csize != 0L)
				{
					this.entry.CompressedSize = (this.csize & (long)((ulong)-1));
				}
				this.entry.CryptoCheckValue = (byte)(num2 >> 8 & 255u);
			}
			this.entry.DosTime = (long)((ulong)num2);
			if (num5 > 0)
			{
				byte[] array2 = new byte[num5];
				this.inputBuffer.ReadRawBuffer(array2);
				this.entry.ExtraData = array2;
			}
			this.entry.ProcessExtraData(true);
			if (this.entry.CompressedSize >= 0L)
			{
				this.csize = this.entry.CompressedSize;
			}
			if (this.entry.Size >= 0L)
			{
				this.size = this.entry.Size;
			}
			if (this.method == 0 && ((!flag && this.csize != this.size) || (flag && this.csize - 12L != this.size)))
			{
				throw new ZipException("Stored, but compressed != uncompressed");
			}
			if (this.entry.IsCompressionMethodSupported())
			{
				this.internalReader = new ZipInputStream.ReadDataHandler(this.InitialRead);
			}
			else
			{
				this.internalReader = new ZipInputStream.ReadDataHandler(this.ReadingNotSupported);
			}
			return this.entry;
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00039578 File Offset: 0x00037778
		private void ReadDataDescriptor()
		{
			if (this.inputBuffer.ReadLeInt() != 134695760)
			{
				throw new ZipException("Data descriptor signature not found");
			}
			this.entry.Crc = ((long)this.inputBuffer.ReadLeInt() & (long)((ulong)-1));
			if (this.entry.LocalHeaderRequiresZip64)
			{
				this.csize = this.inputBuffer.ReadLeLong();
				this.size = this.inputBuffer.ReadLeLong();
			}
			else
			{
				this.csize = (long)this.inputBuffer.ReadLeInt();
				this.size = (long)this.inputBuffer.ReadLeInt();
			}
			this.entry.CompressedSize = this.csize;
			this.entry.Size = this.size;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00039634 File Offset: 0x00037834
		private void CompleteCloseEntry(bool testCrc)
		{
			base.StopDecrypting();
			if ((this.flags & 8) != 0)
			{
				this.ReadDataDescriptor();
			}
			this.size = 0L;
			if (testCrc && (this.crc.Value & (long)((ulong)-1)) != this.entry.Crc && this.entry.Crc != -1L)
			{
				throw new ZipException("CRC mismatch");
			}
			this.crc.Reset();
			if (this.method == 8)
			{
				this.inf.Reset();
			}
			this.entry = null;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x000396C0 File Offset: 0x000378C0
		public void CloseEntry()
		{
			if (this.crc == null)
			{
				throw new InvalidOperationException("Closed");
			}
			if (this.entry == null)
			{
				return;
			}
			if (this.method == 8)
			{
				if ((this.flags & 8) != 0)
				{
					byte[] array = new byte[4096];
					while (this.Read(array, 0, array.Length) > 0)
					{
					}
					return;
				}
				this.csize -= this.inf.TotalIn;
				this.inputBuffer.Available += this.inf.RemainingInput;
			}
			if ((long)this.inputBuffer.Available > this.csize && this.csize >= 0L)
			{
				this.inputBuffer.Available = (int)((long)this.inputBuffer.Available - this.csize);
			}
			else
			{
				this.csize -= (long)this.inputBuffer.Available;
				this.inputBuffer.Available = 0;
				while (this.csize != 0L)
				{
					long num = base.Skip(this.csize);
					if (num <= 0L)
					{
						throw new ZipException("Zip archive ends early.");
					}
					this.csize -= num;
				}
			}
			this.CompleteCloseEntry(false);
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x000397ED File Offset: 0x000379ED
		public override int Available
		{
			get
			{
				if (this.entry == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x000397FA File Offset: 0x000379FA
		public override long Length
		{
			get
			{
				if (this.entry == null)
				{
					throw new InvalidOperationException("No current entry");
				}
				if (this.entry.Size >= 0L)
				{
					return this.entry.Size;
				}
				throw new ZipException("Length not available for the current entry");
			}
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x00039834 File Offset: 0x00037A34
		public override int ReadByte()
		{
			byte[] array = new byte[1];
			if (this.Read(array, 0, 1) <= 0)
			{
				return -1;
			}
			return (int)(array[0] & 255);
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0003985F File Offset: 0x00037A5F
		private int ReadingNotAvailable(byte[] destination, int offset, int count)
		{
			throw new InvalidOperationException("Unable to read from this stream");
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0003986B File Offset: 0x00037A6B
		private int ReadingNotSupported(byte[] destination, int offset, int count)
		{
			throw new ZipException("The compression method for this entry is not supported");
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00039878 File Offset: 0x00037A78
		private int InitialRead(byte[] destination, int offset, int count)
		{
			if (!this.CanDecompressEntry)
			{
				throw new ZipException("Library cannot extract this entry. Version required is (" + this.entry.Version.ToString() + ")");
			}
			if (this.entry.IsCrypted)
			{
				if (this.password == null)
				{
					throw new ZipException("No password set.");
				}
				PkzipClassicManaged pkzipClassicManaged = new PkzipClassicManaged();
				byte[] rgbKey = PkzipClassic.GenerateKeys(ZipConstants.ConvertToArray(this.password));
				this.inputBuffer.CryptoTransform = pkzipClassicManaged.CreateDecryptor(rgbKey, null);
				byte[] array = new byte[12];
				this.inputBuffer.ReadClearTextBuffer(array, 0, 12);
				if (array[11] != this.entry.CryptoCheckValue)
				{
					throw new ZipException("Invalid password");
				}
				if (this.csize >= 12L)
				{
					this.csize -= 12L;
				}
				else if ((this.entry.Flags & 8) == 0)
				{
					throw new ZipException(string.Format("Entry compressed size {0} too small for encryption", this.csize));
				}
			}
			else
			{
				this.inputBuffer.CryptoTransform = null;
			}
			if (this.csize > 0L || (this.flags & 8) != 0)
			{
				if (this.method == 8 && this.inputBuffer.Available > 0)
				{
					this.inputBuffer.SetInflaterInput(this.inf);
				}
				this.internalReader = new ZipInputStream.ReadDataHandler(this.BodyRead);
				return this.BodyRead(destination, offset, count);
			}
			this.internalReader = new ZipInputStream.ReadDataHandler(this.ReadingNotAvailable);
			return 0;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x000399F4 File Offset: 0x00037BF4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Cannot be negative");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Cannot be negative");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Invalid offset/count combination");
			}
			return this.internalReader(buffer, offset, count);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00039A58 File Offset: 0x00037C58
		private int BodyRead(byte[] buffer, int offset, int count)
		{
			if (this.crc == null)
			{
				throw new InvalidOperationException("Closed");
			}
			if (this.entry == null || count <= 0)
			{
				return 0;
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentException("Offset + count exceeds buffer size");
			}
			bool flag = false;
			int num = this.method;
			if (num != 0)
			{
				if (num == 8)
				{
					count = base.Read(buffer, offset, count);
					if (count <= 0)
					{
						if (!this.inf.IsFinished)
						{
							throw new ZipException("Inflater not finished!");
						}
						this.inputBuffer.Available = this.inf.RemainingInput;
						if ((this.flags & 8) == 0 && ((this.inf.TotalIn != this.csize && this.csize != (long)((ulong)-1) && this.csize != -1L) || this.inf.TotalOut != this.size))
						{
							throw new ZipException(string.Concat(new object[]
							{
								"Size mismatch: ",
								this.csize,
								";",
								this.size,
								" <-> ",
								this.inf.TotalIn,
								";",
								this.inf.TotalOut
							}));
						}
						this.inf.Reset();
						flag = true;
					}
				}
			}
			else
			{
				if ((long)count > this.csize && this.csize >= 0L)
				{
					count = (int)this.csize;
				}
				if (count > 0)
				{
					count = this.inputBuffer.ReadClearTextBuffer(buffer, offset, count);
					if (count > 0)
					{
						this.csize -= (long)count;
						this.size -= (long)count;
					}
				}
				if (this.csize == 0L)
				{
					flag = true;
				}
				else if (count < 0)
				{
					throw new ZipException("EOF in stored block");
				}
			}
			if (count > 0)
			{
				this.crc.Update(buffer, offset, count);
			}
			if (flag)
			{
				this.CompleteCloseEntry(true);
			}
			return count;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00039C48 File Offset: 0x00037E48
		public override void Close()
		{
			this.internalReader = new ZipInputStream.ReadDataHandler(this.ReadingNotAvailable);
			this.crc = null;
			this.entry = null;
			base.Close();
		}

		// Token: 0x0400057E RID: 1406
		private ZipInputStream.ReadDataHandler internalReader;

		// Token: 0x0400057F RID: 1407
		private Crc32 crc = new Crc32();

		// Token: 0x04000580 RID: 1408
		private ZipEntry entry;

		// Token: 0x04000581 RID: 1409
		private long size;

		// Token: 0x04000582 RID: 1410
		private int method;

		// Token: 0x04000583 RID: 1411
		private int flags;

		// Token: 0x04000584 RID: 1412
		private string password;

		// Token: 0x02000103 RID: 259
		// (Invoke) Token: 0x06000ABF RID: 2751
		private delegate int ReadDataHandler(byte[] b, int offset, int length);
	}
}
