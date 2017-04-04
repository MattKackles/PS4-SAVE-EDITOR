namespace PS3SaveEditor
{
	// Token: 0x0200001D RID: 29
	public partial class ProfileChecker : global::System.Windows.Forms.Form
	{
		// Token: 0x06000132 RID: 306 RVA: 0x0000B0B1 File Offset: 0x000092B1
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000B0D0 File Offset: 0x000092D0
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::PS3SaveEditor.ProfileChecker));
			this.panelInstructions = new global::System.Windows.Forms.Panel();
			this.lblInstructionPage1 = new global::System.Windows.Forms.Label();
			this.lblInstruciton3 = new global::System.Windows.Forms.Label();
			this.lblInstruction2 = new global::System.Windows.Forms.Label();
			this.lblInstrucionRed = new global::System.Windows.Forms.Label();
			this.lblInstruction1 = new global::System.Windows.Forms.Label();
			this.lblInstructions = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.lblTitle1 = new global::System.Windows.Forms.Label();
			this.btnNext = new global::System.Windows.Forms.Button();
			this.panelProfileName = new global::System.Windows.Forms.Panel();
			this.lblFooter2 = new global::System.Windows.Forms.Label();
			this.lblUserName = new global::System.Windows.Forms.Label();
			this.lblInstruction2Page2 = new global::System.Windows.Forms.Label();
			this.lblDriveLetter = new global::System.Windows.Forms.Label();
			this.txtProfileName = new global::System.Windows.Forms.TextBox();
			this.lblInstructionPage2 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.lblPageTitle = new global::System.Windows.Forms.Label();
			this.panelFinish = new global::System.Windows.Forms.Panel();
			this.lblFinish = new global::System.Windows.Forms.Label();
			this.label8 = new global::System.Windows.Forms.Label();
			this.lblTitleFinish = new global::System.Windows.Forms.Label();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.panelInstructions.SuspendLayout();
			this.panelProfileName.SuspendLayout();
			this.panelFinish.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.panelInstructions.BackColor = global::System.Drawing.Color.White;
			this.panelInstructions.Controls.Add(this.lblInstructionPage1);
			this.panelInstructions.Controls.Add(this.lblInstruciton3);
			this.panelInstructions.Controls.Add(this.lblInstruction2);
			this.panelInstructions.Controls.Add(this.lblInstrucionRed);
			this.panelInstructions.Controls.Add(this.lblInstruction1);
			this.panelInstructions.Controls.Add(this.lblInstructions);
			this.panelInstructions.Controls.Add(this.label1);
			this.panelInstructions.Controls.Add(this.lblTitle1);
			this.panelInstructions.Location = new global::System.Drawing.Point(12, 12);
			this.panelInstructions.Name = "panelInstructions";
			this.panelInstructions.Size = new global::System.Drawing.Size(570, 340);
			this.panelInstructions.TabIndex = 0;
			this.lblInstructionPage1.ForeColor = global::System.Drawing.Color.Black;
			this.lblInstructionPage1.Location = new global::System.Drawing.Point(16, 290);
			this.lblInstructionPage1.Name = "lblInstructionPage1";
			this.lblInstructionPage1.Size = new global::System.Drawing.Size(541, 23);
			this.lblInstructionPage1.TabIndex = 7;
			this.lblInstruciton3.ForeColor = global::System.Drawing.Color.Black;
			this.lblInstruciton3.Location = new global::System.Drawing.Point(16, 243);
			this.lblInstruciton3.Name = "lblInstruciton3";
			this.lblInstruciton3.Size = new global::System.Drawing.Size(541, 28);
			this.lblInstruciton3.TabIndex = 6;
			this.lblInstruction2.Location = new global::System.Drawing.Point(16, 170);
			this.lblInstruction2.Name = "lblInstruction2";
			this.lblInstruction2.Size = new global::System.Drawing.Size(541, 61);
			this.lblInstruction2.TabIndex = 5;
			this.lblInstrucionRed.ForeColor = global::System.Drawing.Color.Red;
			this.lblInstrucionRed.Location = new global::System.Drawing.Point(16, 110);
			this.lblInstrucionRed.Name = "lblInstrucionRed";
			this.lblInstrucionRed.Size = new global::System.Drawing.Size(541, 35);
			this.lblInstrucionRed.TabIndex = 4;
			this.lblInstruction1.Location = new global::System.Drawing.Point(16, 93);
			this.lblInstruction1.Name = "lblInstruction1";
			this.lblInstruction1.Size = new global::System.Drawing.Size(541, 26);
			this.lblInstruction1.TabIndex = 3;
			this.lblInstructions.Location = new global::System.Drawing.Point(16, 63);
			this.lblInstructions.Name = "lblInstructions";
			this.lblInstructions.Size = new global::System.Drawing.Size(541, 26);
			this.lblInstructions.TabIndex = 2;
			this.label1.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Location = new global::System.Drawing.Point(14, 48);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(537, 1);
			this.label1.TabIndex = 1;
			this.lblTitle1.AutoSize = true;
			this.lblTitle1.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 16f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.lblTitle1.Location = new global::System.Drawing.Point(16, 13);
			this.lblTitle1.Name = "lblTitle1";
			this.lblTitle1.Size = new global::System.Drawing.Size(0, 26);
			this.lblTitle1.TabIndex = 0;
			this.btnNext.Location = new global::System.Drawing.Point(259, 379);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new global::System.Drawing.Size(75, 23);
			this.btnNext.TabIndex = 1;
			this.btnNext.Text = "Next";
			this.btnNext.UseVisualStyleBackColor = true;
			this.panelProfileName.BackColor = global::System.Drawing.Color.White;
			this.panelProfileName.Controls.Add(this.lblFooter2);
			this.panelProfileName.Controls.Add(this.lblUserName);
			this.panelProfileName.Controls.Add(this.lblInstruction2Page2);
			this.panelProfileName.Controls.Add(this.lblDriveLetter);
			this.panelProfileName.Controls.Add(this.txtProfileName);
			this.panelProfileName.Controls.Add(this.lblInstructionPage2);
			this.panelProfileName.Controls.Add(this.label4);
			this.panelProfileName.Controls.Add(this.lblPageTitle);
			this.panelProfileName.Location = new global::System.Drawing.Point(12, 12);
			this.panelProfileName.Name = "panelProfileName";
			this.panelProfileName.Size = new global::System.Drawing.Size(570, 340);
			this.panelProfileName.TabIndex = 2;
			this.lblFooter2.Location = new global::System.Drawing.Point(20, 283);
			this.lblFooter2.Name = "lblFooter2";
			this.lblFooter2.Size = new global::System.Drawing.Size(532, 20);
			this.lblFooter2.TabIndex = 7;
			this.lblUserName.Location = new global::System.Drawing.Point(63, 190);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.Size = new global::System.Drawing.Size(193, 20);
			this.lblUserName.TabIndex = 6;
			this.lblInstruction2Page2.Location = new global::System.Drawing.Point(20, 155);
			this.lblInstruction2Page2.Name = "lblInstruction2Page2";
			this.lblInstruction2Page2.Size = new global::System.Drawing.Size(532, 20);
			this.lblInstruction2Page2.TabIndex = 5;
			this.lblDriveLetter.Location = new global::System.Drawing.Point(63, 86);
			this.lblDriveLetter.Name = "lblDriveLetter";
			this.lblDriveLetter.Size = new global::System.Drawing.Size(300, 13);
			this.lblDriveLetter.TabIndex = 4;
			this.txtProfileName.Location = new global::System.Drawing.Point(63, 213);
			this.txtProfileName.Name = "txtProfileName";
			this.txtProfileName.Size = new global::System.Drawing.Size(485, 20);
			this.txtProfileName.TabIndex = 3;
			this.lblInstructionPage2.Location = new global::System.Drawing.Point(20, 61);
			this.lblInstructionPage2.Name = "lblInstructionPage2";
			this.lblInstructionPage2.Size = new global::System.Drawing.Size(532, 20);
			this.lblInstructionPage2.TabIndex = 2;
			this.label4.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.label4.Location = new global::System.Drawing.Point(14, 48);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(538, 1);
			this.label4.TabIndex = 1;
			this.lblPageTitle.AutoSize = true;
			this.lblPageTitle.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 16f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.lblPageTitle.Location = new global::System.Drawing.Point(16, 13);
			this.lblPageTitle.Name = "lblPageTitle";
			this.lblPageTitle.Size = new global::System.Drawing.Size(0, 26);
			this.lblPageTitle.TabIndex = 0;
			this.panelFinish.BackColor = global::System.Drawing.Color.White;
			this.panelFinish.Controls.Add(this.lblFinish);
			this.panelFinish.Controls.Add(this.label8);
			this.panelFinish.Controls.Add(this.lblTitleFinish);
			this.panelFinish.Location = new global::System.Drawing.Point(12, 12);
			this.panelFinish.Name = "panelFinish";
			this.panelFinish.Size = new global::System.Drawing.Size(570, 340);
			this.panelFinish.TabIndex = 3;
			this.lblFinish.Location = new global::System.Drawing.Point(18, 61);
			this.lblFinish.Name = "lblFinish";
			this.lblFinish.Size = new global::System.Drawing.Size(532, 25);
			this.lblFinish.TabIndex = 2;
			this.label8.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.label8.Location = new global::System.Drawing.Point(14, 48);
			this.label8.Name = "label8";
			this.label8.Size = new global::System.Drawing.Size(537, 1);
			this.label8.TabIndex = 1;
			this.lblTitleFinish.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 16f, global::System.Drawing.FontStyle.Bold);
			this.lblTitleFinish.Location = new global::System.Drawing.Point(14, 13);
			this.lblTitleFinish.Name = "lblTitleFinish";
			this.lblTitleFinish.Size = new global::System.Drawing.Size(537, 26);
			this.lblTitleFinish.TabIndex = 0;
			this.panel1.Controls.Add(this.panelProfileName);
			this.panel1.Controls.Add(this.panelFinish);
			this.panel1.Controls.Add(this.panelInstructions);
			this.panel1.Location = new global::System.Drawing.Point(10, 10);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(594, 363);
			this.panel1.TabIndex = 4;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(614, 410);
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.btnNext);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ProfileChecker";
			base.ShowInTaskbar = false;
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "ProfileChecker";
			this.panelInstructions.ResumeLayout(false);
			this.panelInstructions.PerformLayout();
			this.panelProfileName.ResumeLayout(false);
			this.panelProfileName.PerformLayout();
			this.panelFinish.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x040000A1 RID: 161
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040000A2 RID: 162
		private global::System.Windows.Forms.Panel panelInstructions;

		// Token: 0x040000A3 RID: 163
		private global::System.Windows.Forms.Button btnNext;

		// Token: 0x040000A4 RID: 164
		private global::System.Windows.Forms.Label lblInstructions;

		// Token: 0x040000A5 RID: 165
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040000A6 RID: 166
		private global::System.Windows.Forms.Label lblTitle1;

		// Token: 0x040000A7 RID: 167
		private global::System.Windows.Forms.Panel panelProfileName;

		// Token: 0x040000A8 RID: 168
		private global::System.Windows.Forms.Label lblDriveLetter;

		// Token: 0x040000A9 RID: 169
		private global::System.Windows.Forms.TextBox txtProfileName;

		// Token: 0x040000AA RID: 170
		private global::System.Windows.Forms.Label lblInstructionPage2;

		// Token: 0x040000AB RID: 171
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040000AC RID: 172
		private global::System.Windows.Forms.Label lblPageTitle;

		// Token: 0x040000AD RID: 173
		private global::System.Windows.Forms.Panel panelFinish;

		// Token: 0x040000AE RID: 174
		private global::System.Windows.Forms.Label lblFinish;

		// Token: 0x040000AF RID: 175
		private global::System.Windows.Forms.Label label8;

		// Token: 0x040000B0 RID: 176
		private global::System.Windows.Forms.Label lblTitleFinish;

		// Token: 0x040000B1 RID: 177
		private global::System.Windows.Forms.Label lblInstructionPage1;

		// Token: 0x040000B2 RID: 178
		private global::System.Windows.Forms.Label lblInstruciton3;

		// Token: 0x040000B3 RID: 179
		private global::System.Windows.Forms.Label lblInstruction2;

		// Token: 0x040000B4 RID: 180
		private global::System.Windows.Forms.Label lblInstrucionRed;

		// Token: 0x040000B5 RID: 181
		private global::System.Windows.Forms.Label lblInstruction1;

		// Token: 0x040000B6 RID: 182
		private global::System.Windows.Forms.Label lblFooter2;

		// Token: 0x040000B7 RID: 183
		private global::System.Windows.Forms.Label lblUserName;

		// Token: 0x040000B8 RID: 184
		private global::System.Windows.Forms.Label lblInstruction2Page2;

		// Token: 0x040000B9 RID: 185
		private global::System.Windows.Forms.Panel panel1;
	}
}
