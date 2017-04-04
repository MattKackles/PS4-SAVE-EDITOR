namespace PS3SaveEditor
{
	// Token: 0x02000106 RID: 262
	public partial class SimpleEdit : global::System.Windows.Forms.Form
	{
		// Token: 0x06000B00 RID: 2816 RVA: 0x0003D146 File Offset: 0x0003B346
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0003D168 File Offset: 0x0003B368
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.lblGameName = new global::System.Windows.Forms.Label();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.addCodeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.editCodeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.deleteCodeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.lblProfile = new global::System.Windows.Forms.Label();
			this.cbProfile = new global::System.Windows.Forms.ComboBox();
			this.btnApplyCodes = new global::System.Windows.Forms.Button();
			this.dgCheatCodes = new global::CSUST.Data.CustomDataGridView();
			this.Location = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Value = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label1 = new global::System.Windows.Forms.Label();
			this.dgCheats = new global::CSUST.Data.CustomDataGridView();
			this.Select = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Description = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Comment = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.btnApply = new global::System.Windows.Forms.Button();
			this.contextMenuStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgCheatCodes).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgCheats).BeginInit();
			base.SuspendLayout();
			this.lblGameName.AutoSize = true;
			this.lblGameName.Location = new global::System.Drawing.Point(17, 9);
			this.lblGameName.Name = "lblGameName";
			this.lblGameName.Size = new global::System.Drawing.Size(0, 13);
			this.lblGameName.TabIndex = 0;
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.addCodeToolStripMenuItem,
				this.editCodeToolStripMenuItem,
				this.deleteCodeToolStripMenuItem
			});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new global::System.Drawing.Size(139, 70);
			this.contextMenuStrip1.Opening += new global::System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			this.addCodeToolStripMenuItem.Name = "addCodeToolStripMenuItem";
			this.addCodeToolStripMenuItem.Size = new global::System.Drawing.Size(138, 22);
			this.addCodeToolStripMenuItem.Text = "Add Code";
			this.addCodeToolStripMenuItem.Click += new global::System.EventHandler(this.addCodeToolStripMenuItem_Click);
			this.editCodeToolStripMenuItem.Name = "editCodeToolStripMenuItem";
			this.editCodeToolStripMenuItem.Size = new global::System.Drawing.Size(138, 22);
			this.editCodeToolStripMenuItem.Text = "Edit Code";
			this.editCodeToolStripMenuItem.Click += new global::System.EventHandler(this.editCodeToolStripMenuItem_Click);
			this.deleteCodeToolStripMenuItem.Name = "deleteCodeToolStripMenuItem";
			this.deleteCodeToolStripMenuItem.Size = new global::System.Drawing.Size(138, 22);
			this.deleteCodeToolStripMenuItem.Text = "Delete Code";
			this.deleteCodeToolStripMenuItem.Click += new global::System.EventHandler(this.deleteCodeToolStripMenuItem_Click);
			this.panel1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.panel1.BackColor = global::System.Drawing.Color.FromArgb(102, 102, 102);
			this.panel1.Controls.Add(this.lblProfile);
			this.panel1.Controls.Add(this.cbProfile);
			this.panel1.Controls.Add(this.btnApplyCodes);
			this.panel1.Controls.Add(this.dgCheatCodes);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.dgCheats);
			this.panel1.Controls.Add(this.btnClose);
			this.panel1.Controls.Add(this.btnApply);
			this.panel1.Location = new global::System.Drawing.Point(10, 11);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(634, 277);
			this.panel1.TabIndex = 1;
			this.lblProfile.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom;
			this.lblProfile.AutoSize = true;
			this.lblProfile.ForeColor = global::System.Drawing.Color.White;
			this.lblProfile.Location = new global::System.Drawing.Point(72, 250);
			this.lblProfile.Name = "lblProfile";
			this.lblProfile.Size = new global::System.Drawing.Size(39, 13);
			this.lblProfile.TabIndex = 17;
			this.lblProfile.Text = "Profile:";
			this.cbProfile.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom;
			this.cbProfile.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbProfile.FormattingEnabled = true;
			this.cbProfile.Location = new global::System.Drawing.Point(118, 246);
			this.cbProfile.Name = "cbProfile";
			this.cbProfile.Size = new global::System.Drawing.Size(112, 21);
			this.cbProfile.TabIndex = 16;
			this.btnApplyCodes.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.btnApplyCodes.Location = new global::System.Drawing.Point(551, 175);
			this.btnApplyCodes.Name = "btnApplyCodes";
			this.btnApplyCodes.Size = new global::System.Drawing.Size(75, 23);
			this.btnApplyCodes.TabIndex = 15;
			this.btnApplyCodes.Text = "Apply";
			this.btnApplyCodes.UseVisualStyleBackColor = true;
			this.btnApplyCodes.Visible = false;
			this.dgCheatCodes.AllowUserToAddRows = false;
			this.dgCheatCodes.AllowUserToDeleteRows = false;
			this.dgCheatCodes.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.dgCheatCodes.ClipboardCopyMode = global::System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgCheatCodes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgCheatCodes.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgCheatCodes.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.Location,
				this.Value
			});
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.Black;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgCheatCodes.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgCheatCodes.Location = new global::System.Drawing.Point(539, 37);
			this.dgCheatCodes.Name = "dgCheatCodes";
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgCheatCodes.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgCheatCodes.Size = new global::System.Drawing.Size(35, 48);
			this.dgCheatCodes.TabIndex = 14;
			this.dgCheatCodes.Visible = false;
			this.Location.HeaderText = "Location";
			this.Location.Name = "Location";
			this.Location.Width = 70;
			this.Value.HeaderText = "Value";
			this.Value.MaxInputLength = 13;
			this.Value.Name = "Value";
			this.Value.Width = 70;
			this.label1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(452, -1);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(71, 13);
			this.label1.TabIndex = 13;
			this.label1.Text = "Cheat Codes:";
			this.label1.Visible = false;
			this.dgCheats.AllowUserToAddRows = false;
			this.dgCheats.AllowUserToDeleteRows = false;
			this.dgCheats.AllowUserToResizeRows = false;
			this.dgCheats.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.dgCheats.BackgroundColor = global::System.Drawing.Color.FromArgb(175, 175, 175);
			this.dgCheats.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.dgCheats.ClipboardCopyMode = global::System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgCheats.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgCheats.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgCheats.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.Select,
				this.Description,
				this.Comment
			});
			this.dgCheats.ContextMenuStrip = this.contextMenuStrip1;
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			dataGridViewCellStyle5.ForeColor = global::System.Drawing.Color.Black;
			dataGridViewCellStyle5.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgCheats.DefaultCellStyle = dataGridViewCellStyle5;
			this.dgCheats.GridColor = global::System.Drawing.Color.FromArgb(175, 175, 175);
			this.dgCheats.Location = new global::System.Drawing.Point(12, 13);
			this.dgCheats.Name = "dgCheats";
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			dataGridViewCellStyle6.ForeColor = global::System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle6.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgCheats.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgCheats.RowHeadersVisible = false;
			this.dgCheats.RowHeadersWidth = 25;
			this.dgCheats.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgCheats.Size = new global::System.Drawing.Size(610, 222);
			this.dgCheats.TabIndex = 12;
			this.Select.HeaderText = global::PS3SaveEditor.Resources.Resources.btnSaves;
			this.Select.Name = "Select";
			this.Select.Width = 30;
			this.Description.HeaderText = "Description";
			this.Description.Name = "Description";
			this.Description.ReadOnly = true;
			this.Description.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Description.Width = 240;
			this.Comment.HeaderText = "Comment";
			this.Comment.Name = "Comment";
			this.Comment.ReadOnly = true;
			this.Comment.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Comment.Width = 340;
			this.btnClose.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.btnClose.BackColor = global::System.Drawing.Color.FromArgb(246, 128, 31);
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Location = new global::System.Drawing.Point(318, 246);
			this.btnClose.MaximumSize = new global::System.Drawing.Size(60, 23);
			this.btnClose.MinimumSize = new global::System.Drawing.Size(60, 23);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new global::System.Drawing.Size(60, 23);
			this.btnClose.TabIndex = 11;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnApply.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.btnApply.BackColor = global::System.Drawing.Color.FromArgb(246, 128, 31);
			this.btnApply.ForeColor = global::System.Drawing.Color.White;
			this.btnApply.Location = new global::System.Drawing.Point(254, 246);
			this.btnApply.MaximumSize = new global::System.Drawing.Size(60, 23);
			this.btnApply.MinimumSize = new global::System.Drawing.Size(60, 23);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new global::System.Drawing.Size(60, 23);
			this.btnApply.TabIndex = 10;
			this.btnApply.Text = "Patch && Download Save";
			this.btnApply.UseVisualStyleBackColor = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.Black;
			base.ClientSize = new global::System.Drawing.Size(654, 298);
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.lblGameName);
			this.ForeColor = global::System.Drawing.Color.Black;
			base.Icon = global::PS3SaveEditor.Resources.Resources.ps3se;
			this.MinimumSize = new global::System.Drawing.Size(550, 336);
			base.Name = "SimpleEdit";
			base.ShowInTaskbar = false;
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Simple Edit";
			this.contextMenuStrip1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgCheatCodes).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgCheats).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000598 RID: 1432
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000599 RID: 1433
		private global::System.Windows.Forms.Label lblGameName;

		// Token: 0x0400059A RID: 1434
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x0400059B RID: 1435
		private global::System.Windows.Forms.ToolStripMenuItem addCodeToolStripMenuItem;

		// Token: 0x0400059C RID: 1436
		private global::System.Windows.Forms.ToolStripMenuItem editCodeToolStripMenuItem;

		// Token: 0x0400059D RID: 1437
		private global::System.Windows.Forms.ToolStripMenuItem deleteCodeToolStripMenuItem;

		// Token: 0x0400059E RID: 1438
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x0400059F RID: 1439
		private global::System.Windows.Forms.Button btnApplyCodes;

		// Token: 0x040005A0 RID: 1440
		private new global::System.Windows.Forms.DataGridViewTextBoxColumn Location;

		// Token: 0x040005A1 RID: 1441
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Value;

		// Token: 0x040005A2 RID: 1442
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040005A3 RID: 1443
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x040005A4 RID: 1444
		private global::System.Windows.Forms.Button btnApply;

		// Token: 0x040005A5 RID: 1445
		private global::CSUST.Data.CustomDataGridView dgCheatCodes;

		// Token: 0x040005A6 RID: 1446
		private global::CSUST.Data.CustomDataGridView dgCheats;

		// Token: 0x040005A7 RID: 1447
		private global::System.Windows.Forms.ComboBox cbProfile;

		// Token: 0x040005A8 RID: 1448
		private global::System.Windows.Forms.Label lblProfile;

		// Token: 0x040005A9 RID: 1449
		private new global::System.Windows.Forms.DataGridViewCheckBoxColumn Select;

		// Token: 0x040005AA RID: 1450
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Description;

		// Token: 0x040005AB RID: 1451
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Comment;
	}
}
