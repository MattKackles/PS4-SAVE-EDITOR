using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CSUST.Data
{
	// Token: 0x02000023 RID: 35
	public class CustomDataGridView : DataGridView
	{
		// Token: 0x0600022E RID: 558 RVA: 0x0000DB7C File Offset: 0x0000BD7C
		public CustomDataGridView()
		{
			this.brSelection = new SolidBrush(Color.FromArgb(0, 175, 255));
			this.borderPen = new Pen(Color.FromArgb(168, 173, 179), 1f);
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600022F RID: 559 RVA: 0x0000DBD0 File Offset: 0x0000BDD0
		// (remove) Token: 0x06000230 RID: 560 RVA: 0x0000DC08 File Offset: 0x0000BE08
		[Description("Set cell background color, Colindex -1 denotes any col.")]
		public event EventHandler<CellBackColorEventArgs> SetCellBackColor;

		// Token: 0x06000231 RID: 561 RVA: 0x0000DC40 File Offset: 0x0000BE40
		private void DrawCellBackColor(DataGridViewCellPaintingEventArgs e)
		{
			if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
			{
				base.OnCellPainting(e);
				return;
			}
			if (this.SetCellBackColor == null)
			{
				base.OnCellPainting(e);
				return;
			}
			CellBackColorEventArgs cellBackColorEventArgs = new CellBackColorEventArgs(e.RowIndex, e.ColumnIndex);
			this.SetCellBackColor(this, cellBackColorEventArgs);
			if (cellBackColorEventArgs.BackColor == Color.Empty)
			{
				base.OnCellPainting(e);
				return;
			}
			using (SolidBrush solidBrush = new SolidBrush(cellBackColorEventArgs.BackColor))
			{
				using (Pen pen = new Pen(base.GridColor))
				{
					Rectangle rect = new Rectangle(e.CellBounds.Location, e.CellBounds.Size);
					Rectangle rect2 = new Rectangle(e.CellBounds.Location, e.CellBounds.Size);
					rect.X--;
					rect.Y--;
					rect2.Width--;
					rect2.Height--;
					e.Graphics.DrawRectangle(pen, rect);
					e.Graphics.FillRectangle(solidBrush, rect2);
				}
			}
			e.PaintContent(e.CellBounds);
			e.Handled = true;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000DDAC File Offset: 0x0000BFAC
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawRectangle(this.borderPen, 0, 0, base.Width - 1, base.Height - 1);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000DDD8 File Offset: 0x0000BFD8
		protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex < 0)
			{
				base.OnCellPainting(e);
				return;
			}
			if (base.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag == null || (!(base.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString() == "GameFile") && !(base.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString() == "CheatGroup") && !(base.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString() == "NoCheats")))
			{
				if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
				{
					e.Graphics.FillRectangle(this.brSelection, e.CellBounds);
				}
				else
				{
					Brush brush = new SolidBrush(e.CellStyle.BackColor);
					e.Graphics.FillRectangle(brush, e.CellBounds);
					brush.Dispose();
				}
				e.Graphics.DrawLine(Pens.Gray, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Right, e.CellBounds.Top);
				e.Graphics.DrawLine(Pens.Gray, e.CellBounds.Left, e.CellBounds.Bottom, e.CellBounds.Right, e.CellBounds.Bottom);
				e.PaintContent(e.CellBounds);
				e.Handled = true;
				return;
			}
			if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
			{
				e.Graphics.FillRectangle(this.brSelection, e.CellBounds);
				e.Graphics.DrawLine(Pens.Gray, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Right, e.CellBounds.Top);
				e.Graphics.DrawLine(Pens.Gray, e.CellBounds.Left, e.CellBounds.Bottom, e.CellBounds.Right, e.CellBounds.Bottom);
				e.Handled = true;
				return;
			}
			if (base.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString() == "NoCheats")
			{
				e.Graphics.DrawRectangle(Pens.White, new Rectangle(e.CellBounds.Left, e.CellBounds.Top + 1, e.CellBounds.Width, e.CellBounds.Height - 2));
				e.Graphics.FillRectangle(Brushes.White, new Rectangle(e.CellBounds.Left, e.CellBounds.Top + 1, e.CellBounds.Width, e.CellBounds.Height - 2));
			}
			else if (base.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString() == "CheatGroup")
			{
				e.Graphics.FillRectangle(Brushes.White, e.CellBounds.Left, e.CellBounds.Top + 1, e.CellBounds.Width, e.CellBounds.Height - 1);
			}
			else
			{
				e.Graphics.DrawRectangle(Pens.Gray, e.CellBounds);
				e.Graphics.FillRectangle(Brushes.Gray, e.CellBounds);
			}
			e.Handled = true;
		}

		// Token: 0x040000C7 RID: 199
		private Pen borderPen;

		// Token: 0x040000C8 RID: 200
		private Brush brSelection;
	}
}
