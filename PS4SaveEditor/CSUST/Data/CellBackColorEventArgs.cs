using System;
using System.Drawing;

namespace CSUST.Data
{
	// Token: 0x02000024 RID: 36
	public class CellBackColorEventArgs : EventArgs
	{
		// Token: 0x06000234 RID: 564 RVA: 0x0000E23C File Offset: 0x0000C43C
		public CellBackColorEventArgs(int row, int col)
		{
			this.m_RowIndex = row;
			this.m_ColIndex = col;
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000E25D File Offset: 0x0000C45D
		public int RowIndex
		{
			get
			{
				return this.m_RowIndex;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000E265 File Offset: 0x0000C465
		public int ColIndex
		{
			get
			{
				return this.m_ColIndex;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000E26D File Offset: 0x0000C46D
		// (set) Token: 0x06000238 RID: 568 RVA: 0x0000E275 File Offset: 0x0000C475
		public Color BackColor
		{
			get
			{
				return this.m_BackColor;
			}
			set
			{
				this.m_BackColor = value;
			}
		}

		// Token: 0x040000CA RID: 202
		private int m_RowIndex;

		// Token: 0x040000CB RID: 203
		private int m_ColIndex;

		// Token: 0x040000CC RID: 204
		private Color m_BackColor = Color.Empty;
	}
}
