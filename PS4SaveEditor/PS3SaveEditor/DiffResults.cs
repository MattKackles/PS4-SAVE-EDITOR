using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x0200001C RID: 28
	public partial class DiffResults : Form
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000121 RID: 289 RVA: 0x0000A4FC File Offset: 0x000086FC
		// (remove) Token: 0x06000122 RID: 290 RVA: 0x0000A534 File Offset: 0x00008734
		public event EventHandler OnDiffRowSelected;

		// Token: 0x1700004B RID: 75
		// (set) Token: 0x06000123 RID: 291 RVA: 0x0000A56C File Offset: 0x0000876C
		public Dictionary<long, byte> Differences
		{
			set
			{
				this.dataGridView1.Rows.Clear();
				foreach (long current in value.Keys)
				{
					int index = this.dataGridView1.Rows.Add();
					this.dataGridView1.Rows[index].Cells[0].Value = current.ToString("X8");
					if (value[current] != 1)
					{
						this.dataGridView1.Rows[index].Cells[1].Value = (current + (long)((ulong)value[current])).ToString("X8");
					}
					this.dataGridView1.Rows[index].Cells[2].Value = value[current].ToString("X2");
				}
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000A684 File Offset: 0x00008884
		public DiffResults()
		{
			this.InitializeComponent();
			this.Text = Resources.titleDiffResults;
			this.dataGridView1.Columns[0].HeaderText = Resources.colStartAddr;
			this.dataGridView1.Columns[1].HeaderText = Resources.colEndAddr;
			this.dataGridView1.Columns[2].HeaderText = Resources.colBytes;_AppDomain+
			base.CenterToScreen();
			this.dataGridView1.RowStateChanged += new DataGridViewRowStateChangedEventHandler(this.dataGridView1_RowStateChanged);
			base.FormClosing += new FormClosingEventHandler(this.DiffResults_FormClosing);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000A728 File Offset: 0x00008928
		private void DiffResults_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			base.Hide();
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000A737 File Offset: 0x00008937
		private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
		{
			if (e.StateChanged == DataGridViewElementStates.Selected && this.OnDiffRowSelected != null)
			{
				this.OnDiffRowSelected(sender, EventArgs.Empty);
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000A75C File Offset: 0x0000895C
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Hide();
		}

        private void DiffResults_Load(object sender, EventArgs e)
        {

        }
    }
}
