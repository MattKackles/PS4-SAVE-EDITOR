namespace PS3SaveEditor
{
	// Token: 0x0200001C RID: 28
	public partial class DiffResults : global::System.Windows.Forms.Form
	{
		// Token: 0x06000128 RID: 296 RVA: 0x0000A764 File Offset: 0x00008964
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000A784 File Offset: 0x00008984
		private void InitializeComponent()
		{
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.StartAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bytes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StartAddress,
            this.EndAddress,
            this.Bytes});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(322, 213);
            this.dataGridView1.TabIndex = 0;
            // 
            // StartAddress
            // 
            this.StartAddress.HeaderText = "Start Address";
            this.StartAddress.Name = "StartAddress";
            this.StartAddress.ReadOnly = true;
            // 
            // EndAddress
            // 
            this.EndAddress.HeaderText = "End Address";
            this.EndAddress.Name = "EndAddress";
            this.EndAddress.ReadOnly = true;
            this.EndAddress.Width = 120;
            // 
            // Bytes
            // 
            this.Bytes.HeaderText = "Bytes";
            this.Bytes.Name = "Bytes";
            this.Bytes.ReadOnly = true;
            this.Bytes.Width = 90;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(26, 239);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DiffResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 274);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DiffResults";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "DiffResults";
            this.Load += new System.EventHandler(this.DiffResults_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

		}

		// Token: 0x04000098 RID: 152
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000099 RID: 153
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x0400009A RID: 154
		private global::System.Windows.Forms.DataGridViewTextBoxColumn StartAddress;

		// Token: 0x0400009B RID: 155
		private global::System.Windows.Forms.DataGridViewTextBoxColumn EndAddress;

		// Token: 0x0400009C RID: 156
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Bytes;

		// Token: 0x0400009D RID: 157
		private global::System.Windows.Forms.Button btnClose;
	}
}
