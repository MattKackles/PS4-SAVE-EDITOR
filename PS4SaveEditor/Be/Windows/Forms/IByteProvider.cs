using System;

namespace Be.Windows.Forms
{
	// Token: 0x0200004B RID: 75
	public interface IByteProvider
	{
		// Token: 0x06000340 RID: 832
		byte ReadByte(long index);

		// Token: 0x06000341 RID: 833
		void WriteByte(long index, byte value);

		// Token: 0x06000342 RID: 834
		void InsertBytes(long index, byte[] bs);

		// Token: 0x06000343 RID: 835
		void DeleteBytes(long index, long length);

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000344 RID: 836
		long Length
		{
			get;
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000345 RID: 837
		// (remove) Token: 0x06000346 RID: 838
		event EventHandler LengthChanged;

		// Token: 0x06000347 RID: 839
		bool HasChanges();

		// Token: 0x06000348 RID: 840
		void ApplyChanges();

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000349 RID: 841
		// (remove) Token: 0x0600034A RID: 842
		event EventHandler Changed;

		// Token: 0x0600034B RID: 843
		bool SupportsWriteByte();

		// Token: 0x0600034C RID: 844
		bool SupportsInsertBytes();

		// Token: 0x0600034D RID: 845
		bool SupportsDeleteBytes();
	}
}
