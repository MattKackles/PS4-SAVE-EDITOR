using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000101 RID: 257
	internal class ZipHelperStream : Stream
	{
		// Token: 0x06000A8C RID: 2700 RVA: 0x00038AAE File Offset: 0x00036CAE
		public ZipHelperStream(string name)
		{
			this.stream_ = new FileStream(name, FileMode.Open, FileAccess.ReadWrite);
			this.isOwner_ = true;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00038ACB File Offset: 0x00036CCB
		public ZipHelperStream(Stream stream)
		{
			this.stream_ = stream;
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x00038ADA File Offset: 0x00036CDA
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x00038AE2 File Offset: 0x00036CE2
		public bool IsStreamOwner
		{
			get
			{
				return this.isOwner_;
			}
			set
			{
				this.isOwner_ = value;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00038AEB File Offset: 0x00036CEB
		public override bool CanRead
		{
			get
			{
				return this.stream_.CanRead;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x00038AF8 File Offset: 0x00036CF8
		public override bool CanSeek
		{
			get
			{
				return this.stream_.CanSeek;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x00038B05 File Offset: 0x00036D05
		public override bool CanTimeout
		{
			get
			{
				return this.stream_.CanTimeout;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000A93 RID: 2707 RVA: 0x00038B12 File Offset: 0x00036D12
		public override long Length
		{
			get
			{
				return this.stream_.Length;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x00038B1F File Offset: 0x00036D1F
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x00038B2C File Offset: 0x00036D2C
		public override long Position
		{
			get
			{
				return this.stream_.Position;
			}
			set
			{
				this.stream_.Position = value;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x00038B3A File Offset: 0x00036D3A
		public override bool CanWrite
		{
			get
			{
				return this.stream_.CanWrite;
			}
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00038B47 File Offset: 0x00036D47
		public override void Flush()
		{
			this.stream_.Flush();
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00038B54 File Offset: 0x00036D54
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.stream_.Seek(offset, origin);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00038B63 File Offset: 0x00036D63
		public override void SetLength(long value)
		{
			this.stream_.SetLength(value);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00038B71 File Offset: 0x00036D71
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.stream_.Read(buffer, offset, count);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00038B81 File Offset: 0x00036D81
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.stream_.Write(buffer, offset, count);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x00038B94 File Offset: 0x00036D94
		public override void Close()
		{
			Stream stream = this.stream_;
			this.stream_ = null;
			if (this.isOwner_ && stream != null)
			{
				this.isOwner_ = false;
				stream.Close();
			}
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00038BC8 File Offset: 0x00036DC8
		private void WriteLocalHeader(ZipEntry entry, EntryPatchData patchData)
		{
			CompressionMethod compressionMethod = entry.CompressionMethod;
			bool flag = true;
			bool flag2 = false;
			this.WriteLEInt(67324752);
			this.WriteLEShort(entry.Version);
			this.WriteLEShort(entry.Flags);
			this.WriteLEShort((int)((byte)compressionMethod));
			this.WriteLEInt((int)entry.DosTime);
			if (flag)
			{
				this.WriteLEInt((int)entry.Crc);
				if (entry.LocalHeaderRequiresZip64)
				{
					this.WriteLEInt(-1);
					this.WriteLEInt(-1);
				}
				else
				{
					this.WriteLEInt(entry.IsCrypted ? ((int)entry.CompressedSize + 12) : ((int)entry.CompressedSize));
					this.WriteLEInt((int)entry.Size);
				}
			}
			else
			{
				if (patchData != null)
				{
					patchData.CrcPatchOffset = this.stream_.Position;
				}
				this.WriteLEInt(0);
				if (patchData != null)
				{
					patchData.SizePatchOffset = this.stream_.Position;
				}
				if (entry.LocalHeaderRequiresZip64 && flag2)
				{
					this.WriteLEInt(-1);
					this.WriteLEInt(-1);
				}
				else
				{
					this.WriteLEInt(0);
					this.WriteLEInt(0);
				}
			}
			byte[] array = ZipConstants.ConvertToArray(entry.Flags, entry.Name);
			if (array.Length > 65535)
			{
				throw new ZipException("Entry name too long.");
			}
			ZipExtraData zipExtraData = new ZipExtraData(entry.ExtraData);
			if (entry.LocalHeaderRequiresZip64 && (flag || flag2))
			{
				zipExtraData.StartNewEntry();
				if (flag)
				{
					zipExtraData.AddLeLong(entry.Size);
					zipExtraData.AddLeLong(entry.CompressedSize);
				}
				else
				{
					zipExtraData.AddLeLong(-1L);
					zipExtraData.AddLeLong(-1L);
				}
				zipExtraData.AddNewEntry(1);
				if (!zipExtraData.Find(1))
				{
					throw new ZipException("Internal error cant find extra data");
				}
				if (patchData != null)
				{
					patchData.SizePatchOffset = (long)zipExtraData.CurrentReadIndex;
				}
			}
			else
			{
				zipExtraData.Delete(1);
			}
			byte[] entryData = zipExtraData.GetEntryData();
			this.WriteLEShort(array.Length);
			this.WriteLEShort(entryData.Length);
			if (array.Length > 0)
			{
				this.stream_.Write(array, 0, array.Length);
			}
			if (entry.LocalHeaderRequiresZip64 && flag2)
			{
				patchData.SizePatchOffset += this.stream_.Position;
			}
			if (entryData.Length > 0)
			{
				this.stream_.Write(entryData, 0, entryData.Length);
			}
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00038DEC File Offset: 0x00036FEC
		public long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize, int maximumVariableData)
		{
			long num = endLocation - (long)minimumBlockSize;
			if (num < 0L)
			{
				return -1L;
			}
			long num2 = Math.Max(num - (long)maximumVariableData, 0L);
			while (num >= num2)
			{
				long expr_23 = num;
				num = expr_23 - 1L;
				this.Seek(expr_23, SeekOrigin.Begin);
				if (this.ReadLEInt() == signature)
				{
					return this.Position;
				}
			}
			return -1L;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00038E38 File Offset: 0x00037038
		public void WriteZip64EndOfCentralDirectory(long noOfEntries, long sizeEntries, long centralDirOffset)
		{
			long position = this.stream_.Position;
			this.WriteLEInt(101075792);
			this.WriteLELong(44L);
			this.WriteLEShort(51);
			this.WriteLEShort(45);
			this.WriteLEInt(0);
			this.WriteLEInt(0);
			this.WriteLELong(noOfEntries);
			this.WriteLELong(noOfEntries);
			this.WriteLELong(sizeEntries);
			this.WriteLELong(centralDirOffset);
			this.WriteLEInt(117853008);
			this.WriteLEInt(0);
			this.WriteLELong(position);
			this.WriteLEInt(1);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00038EC0 File Offset: 0x000370C0
		public void WriteEndOfCentralDirectory(long noOfEntries, long sizeEntries, long startOfCentralDirectory, byte[] comment)
		{
			if (noOfEntries >= 65535L || startOfCentralDirectory >= (long)((ulong)-1) || sizeEntries >= (long)((ulong)-1))
			{
				this.WriteZip64EndOfCentralDirectory(noOfEntries, sizeEntries, startOfCentralDirectory);
			}
			this.WriteLEInt(101010256);
			this.WriteLEShort(0);
			this.WriteLEShort(0);
			if (noOfEntries >= 65535L)
			{
				this.WriteLEUshort(65535);
				this.WriteLEUshort(65535);
			}
			else
			{
				this.WriteLEShort((int)((short)noOfEntries));
				this.WriteLEShort((int)((short)noOfEntries));
			}
			if (sizeEntries >= (long)((ulong)-1))
			{
				this.WriteLEUint(4294967295u);
			}
			else
			{
				this.WriteLEInt((int)sizeEntries);
			}
			if (startOfCentralDirectory >= (long)((ulong)-1))
			{
				this.WriteLEUint(4294967295u);
			}
			else
			{
				this.WriteLEInt((int)startOfCentralDirectory);
			}
			int num = (comment != null) ? comment.Length : 0;
			if (num > 65535)
			{
				throw new ZipException(string.Format("Comment length({0}) is too long can only be 64K", num));
			}
			this.WriteLEShort(num);
			if (num > 0)
			{
				this.Write(comment, 0, comment.Length);
			}
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x00038FA4 File Offset: 0x000371A4
		public int ReadLEShort()
		{
			int num = this.stream_.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException();
			}
			int num2 = this.stream_.ReadByte();
			if (num2 < 0)
			{
				throw new EndOfStreamException();
			}
			return num | num2 << 8;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00038FE2 File Offset: 0x000371E2
		public int ReadLEInt()
		{
			return this.ReadLEShort() | this.ReadLEShort() << 16;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x00038FF4 File Offset: 0x000371F4
		public long ReadLELong()
		{
			return (long)((ulong)this.ReadLEInt() | (ulong)((ulong)((long)this.ReadLEInt()) << 32));
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00039008 File Offset: 0x00037208
		public void WriteLEShort(int value)
		{
			this.stream_.WriteByte((byte)(value & 255));
			this.stream_.WriteByte((byte)(value >> 8 & 255));
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00039032 File Offset: 0x00037232
		public void WriteLEUshort(ushort value)
		{
			this.stream_.WriteByte((byte)(value & 255));
			this.stream_.WriteByte((byte)(value >> 8));
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00039056 File Offset: 0x00037256
		public void WriteLEInt(int value)
		{
			this.WriteLEShort(value);
			this.WriteLEShort(value >> 16);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00039069 File Offset: 0x00037269
		public void WriteLEUint(uint value)
		{
			this.WriteLEUshort((ushort)(value & 65535u));
			this.WriteLEUshort((ushort)(value >> 16));
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00039084 File Offset: 0x00037284
		public void WriteLELong(long value)
		{
			this.WriteLEInt((int)value);
			this.WriteLEInt((int)(value >> 32));
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00039099 File Offset: 0x00037299
		public void WriteLEUlong(ulong value)
		{
			this.WriteLEUint((uint)(value & (ulong)-1));
			this.WriteLEUint((uint)(value >> 32));
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x000390B4 File Offset: 0x000372B4
		public int WriteDataDescriptor(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			int num = 0;
			if ((entry.Flags & 8) != 0)
			{
				this.WriteLEInt(134695760);
				this.WriteLEInt((int)entry.Crc);
				num += 8;
				if (entry.LocalHeaderRequiresZip64)
				{
					this.WriteLELong(entry.CompressedSize);
					this.WriteLELong(entry.Size);
					num += 16;
				}
				else
				{
					this.WriteLEInt((int)entry.CompressedSize);
					this.WriteLEInt((int)entry.Size);
					num += 8;
				}
			}
			return num;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00039140 File Offset: 0x00037340
		public void ReadDataDescriptor(bool zip64, DescriptorData data)
		{
			int num = this.ReadLEInt();
			if (num != 134695760)
			{
				throw new ZipException("Data descriptor signature not found");
			}
			data.Crc = (long)this.ReadLEInt();
			if (zip64)
			{
				data.CompressedSize = this.ReadLELong();
				data.Size = this.ReadLELong();
				return;
			}
			data.CompressedSize = (long)this.ReadLEInt();
			data.Size = (long)this.ReadLEInt();
		}

		// Token: 0x0400057C RID: 1404
		private bool isOwner_;

		// Token: 0x0400057D RID: 1405
		private Stream stream_;
	}
}
