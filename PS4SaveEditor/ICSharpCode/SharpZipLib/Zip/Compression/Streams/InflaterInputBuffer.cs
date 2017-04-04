using System;
using System.IO;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
	// Token: 0x020000CB RID: 203
	public class InflaterInputBuffer
	{
		// Token: 0x06000889 RID: 2185 RVA: 0x000318D7 File Offset: 0x0002FAD7
		public InflaterInputBuffer(Stream stream) : this(stream, 4096)
		{
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x000318E5 File Offset: 0x0002FAE5
		public InflaterInputBuffer(Stream stream, int bufferSize)
		{
			this.inputStream = stream;
			if (bufferSize < 1024)
			{
				bufferSize = 1024;
			}
			this.rawData = new byte[bufferSize];
			this.clearText = this.rawData;
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x0003191B File Offset: 0x0002FB1B
		public int RawLength
		{
			get
			{
				return this.rawLength;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00031923 File Offset: 0x0002FB23
		public byte[] RawData
		{
			get
			{
				return this.rawData;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x0003192B File Offset: 0x0002FB2B
		public int ClearTextLength
		{
			get
			{
				return this.clearTextLength;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00031933 File Offset: 0x0002FB33
		public byte[] ClearText
		{
			get
			{
				return this.clearText;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x0003193B File Offset: 0x0002FB3B
		// (set) Token: 0x06000890 RID: 2192 RVA: 0x00031943 File Offset: 0x0002FB43
		public int Available
		{
			get
			{
				return this.available;
			}
			set
			{
				this.available = value;
			}
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0003194C File Offset: 0x0002FB4C
		public void SetInflaterInput(Inflater inflater)
		{
			if (this.available > 0)
			{
				inflater.SetInput(this.clearText, this.clearTextLength - this.available, this.available);
				this.available = 0;
			}
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00031980 File Offset: 0x0002FB80
		public void Fill()
		{
			this.rawLength = 0;
			int num;
			for (int i = this.rawData.Length; i > 0; i -= num)
			{
				num = this.inputStream.Read(this.rawData, this.rawLength, i);
				if (num <= 0)
				{
					break;
				}
				this.rawLength += num;
			}
			if (this.cryptoTransform != null)
			{
				this.clearTextLength = this.cryptoTransform.TransformBlock(this.rawData, 0, this.rawLength, this.clearText, 0);
			}
			else
			{
				this.clearTextLength = this.rawLength;
			}
			this.available = this.clearTextLength;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00031A19 File Offset: 0x0002FC19
		public int ReadRawBuffer(byte[] buffer)
		{
			return this.ReadRawBuffer(buffer, 0, buffer.Length);
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00031A28 File Offset: 0x0002FC28
		public int ReadRawBuffer(byte[] outBuffer, int offset, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			int num = offset;
			int i = length;
			while (i > 0)
			{
				if (this.available <= 0)
				{
					this.Fill();
					if (this.available <= 0)
					{
						return 0;
					}
				}
				int num2 = Math.Min(i, this.available);
				Array.Copy(this.rawData, this.rawLength - this.available, outBuffer, num, num2);
				num += num2;
				i -= num2;
				this.available -= num2;
			}
			return length;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00031AA8 File Offset: 0x0002FCA8
		public int ReadClearTextBuffer(byte[] outBuffer, int offset, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			int num = offset;
			int i = length;
			while (i > 0)
			{
				if (this.available <= 0)
				{
					this.Fill();
					if (this.available <= 0)
					{
						return 0;
					}
				}
				int num2 = Math.Min(i, this.available);
				Array.Copy(this.clearText, this.clearTextLength - this.available, outBuffer, num, num2);
				num += num2;
				i -= num2;
				this.available -= num2;
			}
			return length;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00031B28 File Offset: 0x0002FD28
		public int ReadLeByte()
		{
			if (this.available <= 0)
			{
				this.Fill();
				if (this.available <= 0)
				{
					throw new ZipException("EOF in header");
				}
			}
			byte result = this.rawData[this.rawLength - this.available];
			this.available--;
			return (int)result;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00031B7C File Offset: 0x0002FD7C
		public int ReadLeShort()
		{
			return this.ReadLeByte() | this.ReadLeByte() << 8;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00031B8D File Offset: 0x0002FD8D
		public int ReadLeInt()
		{
			return this.ReadLeShort() | this.ReadLeShort() << 16;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00031B9F File Offset: 0x0002FD9F
		public long ReadLeLong()
		{
			return (long)((ulong)this.ReadLeInt() | (ulong)((ulong)((long)this.ReadLeInt()) << 32));
		}

		// Token: 0x17000275 RID: 629
		// (set) Token: 0x0600089A RID: 2202 RVA: 0x00031BB4 File Offset: 0x0002FDB4
		public ICryptoTransform CryptoTransform
		{
			set
			{
				this.cryptoTransform = value;
				if (this.cryptoTransform != null)
				{
					if (this.rawData == this.clearText)
					{
						if (this.internalClearText == null)
						{
							this.internalClearText = new byte[this.rawData.Length];
						}
						this.clearText = this.internalClearText;
					}
					this.clearTextLength = this.rawLength;
					if (this.available > 0)
					{
						this.cryptoTransform.TransformBlock(this.rawData, this.rawLength - this.available, this.available, this.clearText, this.rawLength - this.available);
						return;
					}
				}
				else
				{
					this.clearText = this.rawData;
					this.clearTextLength = this.rawLength;
				}
			}
		}

		// Token: 0x04000452 RID: 1106
		private int rawLength;

		// Token: 0x04000453 RID: 1107
		private byte[] rawData;

		// Token: 0x04000454 RID: 1108
		private int clearTextLength;

		// Token: 0x04000455 RID: 1109
		private byte[] clearText;

		// Token: 0x04000456 RID: 1110
		private byte[] internalClearText;

		// Token: 0x04000457 RID: 1111
		private int available;

		// Token: 0x04000458 RID: 1112
		private ICryptoTransform cryptoTransform;

		// Token: 0x04000459 RID: 1113
		private Stream inputStream;
	}
}
