namespace PS3SaveEditor
{
	// Token: 0x0200009C RID: 156
	public partial class SerialValidateGG : global::System.Windows.Forms.Form
	{
		// Token: 0x06000742 RID: 1858 RVA: 0x0002AFA0 File Offset: 0x000291A0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0002AFC0 File Offset: 0x000291C0
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtSerial4 = new System.Windows.Forms.TextBox();
            this.txtSerial3 = new System.Windows.Forms.TextBox();
            this.txtSerial2 = new System.Windows.Forms.TextBox();
            this.txtSerial1 = new System.Windows.Forms.TextBox();
            this.lblInstruction2 = new System.Windows.Forms.Label();
            this.lblInstruction = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(65, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(299, 15);
            this.label1.TabIndex = 0;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.txtSerial4);
            this.panel1.Controls.Add(this.txtSerial3);
            this.panel1.Controls.Add(this.txtSerial2);
            this.panel1.Controls.Add(this.txtSerial1);
            this.panel1.Controls.Add(this.lblInstruction2);
            this.panel1.Controls.Add(this.lblInstruction);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(10, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(439, 120);
            this.panel1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(218, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(163, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(107, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "-";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(3, 64);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            // 
            // btnOk
            // 
            this.btnOk.ForeColor = System.Drawing.Color.Black;
            this.btnOk.Location = new System.Drawing.Point(293, 65);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtSerial4
            // 
            this.txtSerial4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSerial4.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerial4.Location = new System.Drawing.Point(232, 66);
            this.txtSerial4.MaxLength = 4;
            this.txtSerial4.Name = "txtSerial4";
            this.txtSerial4.Size = new System.Drawing.Size(40, 21);
            this.txtSerial4.TabIndex = 7;
            // 
            // txtSerial3
            // 
            this.txtSerial3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSerial3.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerial3.Location = new System.Drawing.Point(176, 66);
            this.txtSerial3.MaxLength = 4;
            this.txtSerial3.Name = "txtSerial3";
            this.txtSerial3.Size = new System.Drawing.Size(40, 21);
            this.txtSerial3.TabIndex = 6;
            // 
            // txtSerial2
            // 
            this.txtSerial2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSerial2.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerial2.Location = new System.Drawing.Point(120, 66);
            this.txtSerial2.MaxLength = 4;
            this.txtSerial2.Name = "txtSerial2";
            this.txtSerial2.Size = new System.Drawing.Size(40, 21);
            this.txtSerial2.TabIndex = 5;
            // 
            // txtSerial1
            // 
            this.txtSerial1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSerial1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerial1.Location = new System.Drawing.Point(64, 66);
            this.txtSerial1.MaxLength = 4;
            this.txtSerial1.Name = "txtSerial1";
            this.txtSerial1.Size = new System.Drawing.Size(40, 21);
            this.txtSerial1.TabIndex = 4;
            // 
            // lblInstruction2
            // 
            this.lblInstruction2.Location = new System.Drawing.Point(5, 34);
            this.lblInstruction2.Name = "lblInstruction2";
            this.lblInstruction2.Size = new System.Drawing.Size(430, 13);
            this.lblInstruction2.TabIndex = 2;
            this.lblInstruction2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInstruction
            // 
            this.lblInstruction.AutoSize = true;
            this.lblInstruction.Location = new System.Drawing.Point(13, 8);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(0, 13);
            this.lblInstruction.TabIndex = 1;
            this.lblInstruction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SerialValidateGG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 142);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SerialValidateGG";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Registering Game Genie";
            this.Load += new System.EventHandler(this.SerialValidateGG_Load_1);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}

		// Token: 0x0400035A RID: 858
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400035B RID: 859
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400035C RID: 860
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x0400035D RID: 861
		private global::System.Windows.Forms.Label lblInstruction;

		// Token: 0x0400035E RID: 862
		private global::System.Windows.Forms.Label lblInstruction2;

		// Token: 0x0400035F RID: 863
		private global::System.Windows.Forms.TextBox txtSerial4;

		// Token: 0x04000360 RID: 864
		private global::System.Windows.Forms.TextBox txtSerial3;

		// Token: 0x04000361 RID: 865
		private global::System.Windows.Forms.TextBox txtSerial2;

		// Token: 0x04000362 RID: 866
		private global::System.Windows.Forms.TextBox txtSerial1;

		// Token: 0x04000363 RID: 867
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04000364 RID: 868
		private global::System.Windows.Forms.Button btnOk;

		// Token: 0x04000365 RID: 869
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04000366 RID: 870
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04000367 RID: 871
		private global::System.Windows.Forms.Label label2;
	}
}
