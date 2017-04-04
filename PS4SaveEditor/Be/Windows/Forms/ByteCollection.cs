using System;
using System.Collections;

namespace Be.Windows.Forms
{
	// Token: 0x02000046 RID: 70
	public class ByteCollection : CollectionBase
	{
		// Token: 0x0600030C RID: 780 RVA: 0x0001191F File Offset: 0x0000FB1F
		public ByteCollection()
		{
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00011927 File Offset: 0x0000FB27
		public ByteCollection(byte[] bs)
		{
			this.AddRange(bs);
		}

		// Token: 0x17000186 RID: 390
		public byte this[int index]
		{
			get
			{
				return (byte)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0001195D File Offset: 0x0000FB5D
		public void Add(byte b)
		{
			base.List.Add(b);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00011971 File Offset: 0x0000FB71
		public void AddRange(byte[] bs)
		{
			base.InnerList.AddRange(bs);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0001197F File Offset: 0x0000FB7F
		public void Remove(byte b)
		{
			base.List.Remove(b);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00011992 File Offset: 0x0000FB92
		public void RemoveRange(int index, int count)
		{
			base.InnerList.RemoveRange(index, count);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x000119A1 File Offset: 0x0000FBA1
		public void InsertRange(int index, byte[] bs)
		{
			base.InnerList.InsertRange(index, bs);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000119B0 File Offset: 0x0000FBB0
		public byte[] GetBytes()
		{
			byte[] array = new byte[base.Count];
			base.InnerList.CopyTo(0, array, 0, array.Length);
			return array;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000119DB File Offset: 0x0000FBDB
		public void Insert(int index, byte b)
		{
			base.InnerList.Insert(index, b);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x000119EF File Offset: 0x0000FBEF
		public int IndexOf(byte b)
		{
			return base.InnerList.IndexOf(b);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00011A02 File Offset: 0x0000FC02
		public bool Contains(byte b)
		{
			return base.InnerList.Contains(b);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00011A15 File Offset: 0x0000FC15
		public void CopyTo(byte[] bs, int index)
		{
			base.InnerList.CopyTo(bs, index);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00011A24 File Offset: 0x0000FC24
		public byte[] ToArray()
		{
			byte[] array = new byte[base.Count];
			this.CopyTo(array, 0);
			return array;
		}
	}
}
