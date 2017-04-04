namespace PS3SaveEditor
{
	// Token: 0x02000041 RID: 65
	public partial class Goto : global::System.Windows.Forms.Form
	{
		// Token: 0x060002E4 RID: 740 RVA: 0x0001121F File Offset: 0x0000F41F
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00011240 File Offset: 0x0000F440
		private void InitializeComponent()
		{
			this.lblEnterLoc = new global::System.Windows.Forms.Label();
			this.txtLocation = new global::System.Windows.Forms.TextBox();
			this.btnOk = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			this.lblEnterLoc.AutoSize = true;
			this.lblEnterLoc.Location = new global::System.Drawing.Point(14, 11);
			this.lblEnterLoc.Name = "lblEnterLoc";
			this.lblEnterLoc.Size = new global::System.Drawing.Size(79, 13);
			this.lblEnterLoc.TabIndex = 0;
			this.lblEnterLoc.Text = "Enter Location:";
			this.txtLocation.Location = new global::System.Drawing.Point(99, 9);
			this.txtLocation.Name = "txtLocation";
			this.txtLocation.Size = new global::System.Drawing.Size(97, 20);
			this.txtLocation.TabIndex = 1;
			this.txtLocation.TextChanged += new global::System.EventHandler(this.txtLocation_TextChanged);
			this.txtLocation.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.txtLocation_KeyDown);
			this.txtLocation.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.txtLocation_KeyPress);
			this.btnOk.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new global::System.Drawing.Point(121, 35);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new global::System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new global::System.EventHandler(this.btnOk_Click);
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new global::System.Drawing.Point(202, 35);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new global::System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			base.AcceptButton = this.btnOk;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new global::System.Drawing.Size(284, 69);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.txtLocation);
			base.Controls.Add(this.lblEnterLoc);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "Goto";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			this.Text = "Go To Location";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000187 RID: 391
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000188 RID: 392
		private global::System.Windows.Forms.Label lblEnterLoc;

		// Token: 0x04000189 RID: 393
		private global::System.Windows.Forms.TextBox txtLocation;

		// Token: 0x0400018A RID: 394
		private global::System.Windows.Forms.Button btnOk;

		// Token: 0x0400018B RID: 395
		private global::System.Windows.Forms.Button btnCancel;
	}
}
