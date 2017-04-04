using System;

namespace Be.Windows.Forms
{
	// Token: 0x0200004C RID: 76
	public class DynamicByteProvider : IByteProvider
	{
		// Token: 0x0600034E RID: 846 RVA: 0x00011F1A File Offset: 0x0001011A
		public DynamicByteProvider(byte[] data) : this(new ByteCollection(data))
		{
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00011F28 File Offset: 0x00010128
		public DynamicByteProvider(ByteCollection bytes)
		{
			this._bytes = bytes;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00011F37 File Offset: 0x00010137
		private void OnChanged(EventArgs e)
		{
			this._hasChanges = true;
			if (this.Changed != null)
			{
				this.Changed(this, e);
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00011F55 File Offset: 0x00010155
		private void OnLengthChanged(EventArgs e)
		{
			if (this.LengthChanged != null)
			{
				this.LengthChanged(this, e);
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00011F6C File Offset: 0x0001016C
		public ByteCollection Bytes
		{
			get
			{
				return this._bytes;
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00011F74 File Offset: 0x00010174
		public bool HasChanges()
		{
			return this._hasChanges;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00011F7C File Offset: 0x0001017C
		public void ApplyChanges()
		{
			this._hasChanges = false;
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000355 RID: 853 RVA: 0x00011F88 File Offset: 0x00010188
		// (remove) Token: 0x06000356 RID: 854 RVA: 0x00011FC0 File Offset: 0x000101C0
		public event EventHandler Changed;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000357 RID: 855 RVA: 0x00011FF8 File Offset: 0x000101F8
		// (remove) Token: 0x06000358 RID: 856 RVA: 0x00012030 File Offset: 0x00010230
		public event EventHandler LengthChanged;

		// Token: 0x06000359 RID: 857 RVA: 0x00012065 File Offset: 0x00010265
		public byte ReadByte(long index)
		{
			return this._bytes[(int)index];
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00012074 File Offset: 0x00010274
		public void WriteByte(long index, byte value)
		{
			this._bytes[(int)index] = value;
			this.OnChanged(EventArgs.Empty);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00012090 File Offset: 0x00010290
		public void DeleteBytes(long index, long length)
		{
			int index2 = (int)Math.Max(0L, index);
			int count = (int)Math.Min((long)((int)this.Length), length);
			this._bytes.RemoveRange(index2, count);
			this.OnLengthChanged(EventArgs.Empty);
			this.OnChanged(EventArgs.Empty);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x000120DA File Offset: 0x000102DA
		public void InsertBytes(long index, byte[] bs)
		{
			this._bytes.InsertRange((int)index, bs);
			this.OnLengthChanged(EventArgs.Empty);
			this.OnChanged(EventArgs.Empty);
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00012100 File Offset: 0x00010300
		public long Length
		{
			get
			{
				return (long)this._bytes.Count;
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0001210E File Offset: 0x0001030E
		public bool SupportsWriteByte()
		{
			return true;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00012111 File Offset: 0x00010311
		public bool SupportsInsertBytes()
		{
			return false;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00012114 File Offset: 0x00010314
		public bool SupportsDeleteBytes()
		{
			return false;
		}

		// Token: 0x040001A9 RID: 425
		private bool _hasChanges;

		// Token: 0x040001AA RID: 426
		private ByteCollection _bytes;
	}
}
