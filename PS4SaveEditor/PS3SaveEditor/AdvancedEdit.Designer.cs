namespace PS3SaveEditor
{
	// Token: 0x02000005 RID: 5
	public partial class AdvancedEdit : global::System.Windows.Forms.Form
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000059BC File Offset: 0x00003BBC
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000059DC File Offset: 0x00003BDC
		private void InitializeComponent()
		{
			this.lblCheatCodes = new global::System.Windows.Forms.Label();
			this.lblCheats = new global::System.Windows.Forms.Label();
			this.btnApply = new global::System.Windows.Forms.Button();
			this.lblOffset = new global::System.Windows.Forms.Label();
			this.lblOffsetValue = new global::System.Windows.Forms.Label();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.cbSaveFiles = new global::System.Windows.Forms.ComboBox();
			this.txtSaveData = new global::System.Windows.Forms.RichTextBox();
			this.lblProfile = new global::System.Windows.Forms.Label();
			this.cbProfile = new global::System.Windows.Forms.ComboBox();
			this.btnFindPrev = new global::System.Windows.Forms.Button();
			this.btnFind = new global::System.Windows.Forms.Button();
			this.lblAddress = new global::System.Windows.Forms.Label();
			this.txtSearchValue = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.lstCheats = new global::System.Windows.Forms.ListBox();
			this.lstValues = new global::System.Windows.Forms.ListBox();
			this.lblGameName = new global::System.Windows.Forms.Label();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.hexBox1 = new global::Be.Windows.Forms.HexBox();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.lblCheatCodes.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.lblCheatCodes.AutoSize = true;
			this.lblCheatCodes.ForeColor = global::System.Drawing.Color.White;
			this.lblCheatCodes.Location = new global::System.Drawing.Point(684, 146);
			this.lblCheatCodes.Name = "lblCheatCodes";
			this.lblCheatCodes.Size = new global::System.Drawing.Size(71, 13);
			this.lblCheatCodes.TabIndex = 4;
			this.lblCheatCodes.Text = "Cheat Codes:";
			this.lblCheats.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.lblCheats.AutoSize = true;
			this.lblCheats.ForeColor = global::System.Drawing.Color.White;
			this.lblCheats.Location = new global::System.Drawing.Point(684, 24);
			this.lblCheats.Name = "lblCheats";
			this.lblCheats.Size = new global::System.Drawing.Size(43, 13);
			this.lblCheats.TabIndex = 5;
			this.lblCheats.Text = "Cheats:";
			this.btnApply.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.btnApply.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnApply.BackColor = global::System.Drawing.Color.FromArgb(246, 128, 31);
			this.btnApply.ForeColor = global::System.Drawing.Color.White;
			this.btnApply.Location = new global::System.Drawing.Point(725, 317);
			this.btnApply.MinimumSize = new global::System.Drawing.Size(57, 23);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new global::System.Drawing.Size(57, 23);
			this.btnApply.TabIndex = 6;
			this.btnApply.Text = "Apply && Download";
			this.btnApply.UseVisualStyleBackColor = false;
			this.lblOffset.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.lblOffset.AutoSize = true;
			this.lblOffset.ForeColor = global::System.Drawing.Color.White;
			this.lblOffset.Location = new global::System.Drawing.Point(571, 319);
			this.lblOffset.Name = "lblOffset";
			this.lblOffset.Size = new global::System.Drawing.Size(38, 13);
			this.lblOffset.TabIndex = 8;
			this.lblOffset.Text = "Offset:";
			this.lblOffsetValue.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.lblOffsetValue.AutoSize = true;
			this.lblOffsetValue.ForeColor = global::System.Drawing.Color.White;
			this.lblOffsetValue.Location = new global::System.Drawing.Point(612, 319);
			this.lblOffsetValue.Name = "lblOffsetValue";
			this.lblOffsetValue.Size = new global::System.Drawing.Size(0, 13);
			this.lblOffsetValue.TabIndex = 9;
			this.panel1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.panel1.BackColor = global::System.Drawing.Color.FromArgb(102, 102, 102);
			this.panel1.Controls.Add(this.cbSaveFiles);
			this.panel1.Controls.Add(this.txtSaveData);
			this.panel1.Controls.Add(this.lblProfile);
			this.panel1.Controls.Add(this.cbProfile);
			this.panel1.Controls.Add(this.btnFindPrev);
			this.panel1.Controls.Add(this.btnFind);
			this.panel1.Controls.Add(this.lblAddress);
			this.panel1.Controls.Add(this.txtSearchValue);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.lstCheats);
			this.panel1.Controls.Add(this.lstValues);
			this.panel1.Controls.Add(this.lblGameName);
			this.panel1.Controls.Add(this.btnClose);
			this.panel1.Controls.Add(this.lblOffsetValue);
			this.panel1.Controls.Add(this.lblOffset);
			this.panel1.Controls.Add(this.btnApply);
			this.panel1.Controls.Add(this.lblCheats);
			this.panel1.Controls.Add(this.lblCheatCodes);
			this.panel1.Controls.Add(this.hexBox1);
			this.panel1.Location = new global::System.Drawing.Point(10, 11);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(856, 348);
			this.panel1.TabIndex = 10;
			this.cbSaveFiles.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSaveFiles.FormattingEnabled = true;
			this.cbSaveFiles.Location = new global::System.Drawing.Point(235, 7);
			this.cbSaveFiles.Name = "cbSaveFiles";
			this.cbSaveFiles.Size = new global::System.Drawing.Size(121, 21);
			this.cbSaveFiles.Sorted = true;
			this.cbSaveFiles.TabIndex = 25;
			this.txtSaveData.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.txtSaveData.Location = new global::System.Drawing.Point(10, 42);
			this.txtSaveData.Name = "txtSaveData";
			this.txtSaveData.ScrollBars = global::System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.txtSaveData.Size = new global::System.Drawing.Size(666, 265);
			this.txtSaveData.TabIndex = 24;
			this.txtSaveData.Text = "";
			this.txtSaveData.Visible = false;
			this.lblProfile.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.lblProfile.AutoSize = true;
			this.lblProfile.ForeColor = global::System.Drawing.Color.White;
			this.lblProfile.Location = new global::System.Drawing.Point(380, 321);
			this.lblProfile.Name = "lblProfile";
			this.lblProfile.Size = new global::System.Drawing.Size(39, 13);
			this.lblProfile.TabIndex = 23;
			this.lblProfile.Text = "Profile:";
			this.cbProfile.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.cbProfile.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbProfile.FormattingEnabled = true;
			this.cbProfile.Location = new global::System.Drawing.Point(421, 317);
			this.cbProfile.Name = "cbProfile";
			this.cbProfile.Size = new global::System.Drawing.Size(112, 21);
			this.cbProfile.TabIndex = 22;
			this.btnFindPrev.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.btnFindPrev.BackColor = global::System.Drawing.Color.FromArgb(246, 128, 31);
			this.btnFindPrev.ForeColor = global::System.Drawing.Color.White;
			this.btnFindPrev.Location = new global::System.Drawing.Point(221, 316);
			this.btnFindPrev.Name = "btnFindPrev";
			this.btnFindPrev.Size = new global::System.Drawing.Size(81, 23);
			this.btnFindPrev.TabIndex = 21;
			this.btnFindPrev.Text = "Find Previous";
			this.btnFindPrev.UseVisualStyleBackColor = false;
			this.btnFind.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.btnFind.BackColor = global::System.Drawing.Color.FromArgb(246, 128, 31);
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Location = new global::System.Drawing.Point(152, 316);
			this.btnFind.Name = "btnFind";
			this.btnFind.Size = new global::System.Drawing.Size(63, 23);
			this.btnFind.TabIndex = 20;
			this.btnFind.Text = "Find";
			this.btnFind.UseVisualStyleBackColor = false;
			this.lblAddress.Font = new global::System.Drawing.Font("Courier New", 7.8f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.lblAddress.ForeColor = global::System.Drawing.Color.White;
			this.lblAddress.Location = new global::System.Drawing.Point(14, 24);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new global::System.Drawing.Size(641, 15);
			this.lblAddress.TabIndex = 17;
			this.lblAddress.Text = "Address      Data (Hex)                                                    Data (ASCII)";
			this.txtSearchValue.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.txtSearchValue.Location = new global::System.Drawing.Point(56, 317);
			this.txtSearchValue.Name = "txtSearchValue";
			this.txtSearchValue.Size = new global::System.Drawing.Size(90, 20);
			this.txtSearchValue.TabIndex = 16;
			this.label1.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.label1.AutoSize = true;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Location = new global::System.Drawing.Point(10, 321);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(41, 13);
			this.label1.TabIndex = 15;
			this.label1.Text = "Search";
			this.lstCheats.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.lstCheats.FormattingEnabled = true;
			this.lstCheats.Location = new global::System.Drawing.Point(684, 42);
			this.lstCheats.Name = "lstCheats";
			this.lstCheats.Size = new global::System.Drawing.Size(160, 95);
			this.lstCheats.TabIndex = 14;
			this.lstValues.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.lstValues.FormattingEnabled = true;
			this.lstValues.Location = new global::System.Drawing.Point(684, 162);
			this.lstValues.MultiColumn = true;
			this.lstValues.Name = "lstValues";
			this.lstValues.Size = new global::System.Drawing.Size(160, 147);
			this.lstValues.TabIndex = 13;
			this.lblGameName.AutoSize = true;
			this.lblGameName.ForeColor = global::System.Drawing.Color.White;
			this.lblGameName.Location = new global::System.Drawing.Point(12, 8);
			this.lblGameName.Name = "lblGameName";
			this.lblGameName.Size = new global::System.Drawing.Size(28, 13);
			this.lblGameName.TabIndex = 12;
			this.lblGameName.Text = "Test";
			this.btnClose.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.btnClose.BackColor = global::System.Drawing.Color.FromArgb(246, 128, 31);
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Location = new global::System.Drawing.Point(787, 317);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new global::System.Drawing.Size(57, 23);
			this.btnClose.TabIndex = 10;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = false;
			this.hexBox1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.hexBox1.Font = new global::System.Drawing.Font("Courier New", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.hexBox1.HScrollBarVisible = false;
			this.hexBox1.LineInfoForeColor = global::System.Drawing.Color.Empty;
			this.hexBox1.Location = new global::System.Drawing.Point(13, 42);
			this.hexBox1.Name = "hexBox1";
			this.hexBox1.ShadowSelectionColor = global::System.Drawing.Color.FromArgb(100, 60, 188, 255);
			this.hexBox1.Size = new global::System.Drawing.Size(666, 265);
			this.hexBox1.TabIndex = 0;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.Black;
			base.ClientSize = new global::System.Drawing.Size(876, 369);
			base.Controls.Add(this.panel1);
			base.Icon = global::PS3SaveEditor.Resources.Resources.ps3se;
			base.KeyPreview = true;
			this.MinimumSize = new global::System.Drawing.Size(856, 362);
			base.Name = "AdvancedEdit";
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Advanced Edit";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04000027 RID: 39
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000028 RID: 40
		private global::Be.Windows.Forms.HexBox hexBox1;

		// Token: 0x04000029 RID: 41
		private global::System.Windows.Forms.Label lblCheatCodes;

		// Token: 0x0400002A RID: 42
		private global::System.Windows.Forms.Label lblCheats;

		// Token: 0x0400002B RID: 43
		private global::System.Windows.Forms.Button btnApply;

		// Token: 0x0400002C RID: 44
		private global::System.Windows.Forms.Label lblOffset;

		// Token: 0x0400002D RID: 45
		private global::System.Windows.Forms.Label lblOffsetValue;

		// Token: 0x0400002E RID: 46
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x0400002F RID: 47
		private global::System.Windows.Forms.ListBox lstCheats;

		// Token: 0x04000030 RID: 48
		private global::System.Windows.Forms.ListBox lstValues;

		// Token: 0x04000031 RID: 49
		private global::System.Windows.Forms.Label lblGameName;

		// Token: 0x04000032 RID: 50
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x04000033 RID: 51
		private global::System.Windows.Forms.TextBox txtSearchValue;

		// Token: 0x04000034 RID: 52
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000035 RID: 53
		private global::System.Windows.Forms.Button btnFindPrev;

		// Token: 0x04000036 RID: 54
		private global::System.Windows.Forms.Button btnFind;

		// Token: 0x04000037 RID: 55
		private global::System.Windows.Forms.Label lblAddress;

		// Token: 0x04000038 RID: 56
		private global::System.Windows.Forms.ComboBox cbProfile;

		// Token: 0x04000039 RID: 57
		private global::System.Windows.Forms.Label lblProfile;

		// Token: 0x0400003A RID: 58
		private global::System.Windows.Forms.RichTextBox txtSaveData;

		// Token: 0x0400003B RID: 59
		private global::System.Windows.Forms.ComboBox cbSaveFiles;
	}
}
