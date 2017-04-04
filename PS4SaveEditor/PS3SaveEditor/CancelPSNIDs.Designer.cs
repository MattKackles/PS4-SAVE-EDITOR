namespace PS3SaveEditor
{
	// Token: 0x02000009 RID: 9
	public partial class CancelPSNIDs : global::System.Windows.Forms.Form
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00007289 File Offset: 0x00005489
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000072A8 File Offset: 0x000054A8
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::PS3SaveEditor.CancelPSNIDs));
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.UserName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			base.SuspendLayout();
			this.panel1.Controls.Add(this.dataGridView1);
			this.panel1.Location = new global::System.Drawing.Point(10, 10);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(260, 179);
			this.panel1.TabIndex = 0;
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AutoSizeColumnsMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.UserName
			});
			this.dataGridView1.Location = new global::System.Drawing.Point(12, 15);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.Size = new global::System.Drawing.Size(237, 150);
			this.dataGridView1.TabIndex = 0;
			this.UserName.HeaderText = "UserName";
			this.UserName.Name = "UserName";
			this.UserName.ReadOnly = true;
			this.btnCancel.Location = new global::System.Drawing.Point(62, 195);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new global::System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			this.btnClose.Location = new global::System.Drawing.Point(143, 195);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new global::System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(284, 228);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CancelPSNIDs";
			base.ShowInTaskbar = false;
			this.Text = "CancelPSNIDs";
			base.Load += new global::System.EventHandler(this.CancelPSNIDs_Load);
			this.panel1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04000048 RID: 72
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000049 RID: 73
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x0400004A RID: 74
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x0400004B RID: 75
		private global::System.Windows.Forms.DataGridViewTextBoxColumn UserName;

		// Token: 0x0400004C RID: 76
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x0400004D RID: 77
		private global::System.Windows.Forms.Button btnClose;
	}
}
