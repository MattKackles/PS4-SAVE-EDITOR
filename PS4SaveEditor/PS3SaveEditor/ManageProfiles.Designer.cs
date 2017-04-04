namespace PS3SaveEditor
{
	// Token: 0x02000065 RID: 101
	public partial class ManageProfiles : global::System.Windows.Forms.Form
	{
		// Token: 0x0600054C RID: 1356 RVA: 0x00020B12 File Offset: 0x0001ED12
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00020B34 File Offset: 0x0001ED34
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.dgProfiles = new global::System.Windows.Forms.DataGridView();
			this._Name = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.deleteToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.renameToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.btnSave = new global::System.Windows.Forms.Button();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.panel1 = new global::System.Windows.Forms.Panel();
			((global::System.ComponentModel.ISupportInitialize)this.dgProfiles).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.dgProfiles.AllowUserToAddRows = false;
			this.dgProfiles.AllowUserToDeleteRows = false;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.Color.FromArgb(0, 175, 255);
			this.dgProfiles.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			this.dgProfiles.AutoSizeRowsMode = global::System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dgProfiles.BackgroundColor = global::System.Drawing.Color.FromArgb(175, 175, 175);
			this.dgProfiles.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgProfiles.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this._Name,
				this.ID
			});
			this.dgProfiles.ContextMenuStrip = this.contextMenuStrip1;
			this.dgProfiles.Location = new global::System.Drawing.Point(12, 12);
			this.dgProfiles.Name = "dgProfiles";
			this.dgProfiles.RowHeadersVisible = false;
			this.dgProfiles.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgProfiles.ShowEditingIcon = false;
			this.dgProfiles.Size = new global::System.Drawing.Size(218, 202);
			this.dgProfiles.TabIndex = 0;
			this._Name.HeaderText = "Name";
			this._Name.Name = "_Name";
			this._Name.Width = 114;
			this._Name.MaxInputLength = 32;
			this.ID.HeaderText = "PSN ID";
			this.ID.Name = "ID";
			this.ID.ReadOnly = true;
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.deleteToolStripMenuItem,
				this.renameToolStripMenuItem
			});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new global::System.Drawing.Size(118, 48);
			this.contextMenuStrip1.Opening += new global::System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new global::System.Drawing.Size(117, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new global::System.EventHandler(this.deleteToolStripMenuItem_Click);
			this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
			this.renameToolStripMenuItem.Size = new global::System.Drawing.Size(117, 22);
			this.renameToolStripMenuItem.Text = "Rename";
			this.renameToolStripMenuItem.Click += new global::System.EventHandler(this.renameToolStripMenuItem_Click);
			this.btnSave.ForeColor = global::System.Drawing.Color.White;
			this.btnSave.Location = new global::System.Drawing.Point(42, 217);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new global::System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Apply";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new global::System.EventHandler(this.btnSave_Click);
			this.btnClose.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Location = new global::System.Drawing.Point(121, 217);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new global::System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			this.panel1.BackColor = global::System.Drawing.Color.FromArgb(102, 102, 102);
			this.panel1.Controls.Add(this.dgProfiles);
			this.panel1.Controls.Add(this.btnClose);
			this.panel1.Controls.Add(this.btnSave);
			this.panel1.Location = new global::System.Drawing.Point(10, 10);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(242, 244);
			this.panel1.TabIndex = 4;
			base.AcceptButton = this.btnSave;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.Black;
			base.CancelButton = this.btnClose;
			base.ClientSize = new global::System.Drawing.Size(262, 264);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Fixed3D;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ManageProfiles";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Manage Profiles";
			base.Load += new global::System.EventHandler(this.ManageProfiles_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgProfiles).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x04000292 RID: 658
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000293 RID: 659
		private global::System.Windows.Forms.DataGridView dgProfiles;

		// Token: 0x04000294 RID: 660
		private global::System.Windows.Forms.Button btnSave;

		// Token: 0x04000295 RID: 661
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x04000296 RID: 662
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x04000297 RID: 663
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000298 RID: 664
		private global::System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;

		// Token: 0x04000299 RID: 665
		private global::System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;

		// Token: 0x0400029A RID: 666
		private global::System.Windows.Forms.DataGridViewTextBoxColumn _Name;

		// Token: 0x0400029B RID: 667
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ID;
	}
}
