namespace PS3SaveEditor
{
	// Token: 0x0200005B RID: 91
	public partial class MainForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000508 RID: 1288 RVA: 0x0001D7FC File Offset: 0x0001B9FC
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001D81C File Offset: 0x0001BA1C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::PS3SaveEditor.MainForm));
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.simpleToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.advancedToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.resignToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.registerPSNIDToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new global::System.Windows.Forms.ToolStripSeparator();
			this.restoreFromBackupToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.deleteSaveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.btnHome = new global::System.Windows.Forms.Button();
			this.btnHelp = new global::System.Windows.Forms.Button();
			this.btnOptions = new global::System.Windows.Forms.Button();
			this.pnlHome = new global::System.Windows.Forms.Panel();
			this.chkShowAll = new global::System.Windows.Forms.CheckBox();
			this.dgServerGames = new global::CSUST.Data.CustomDataGridView();
			this.Choose = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.dataGridViewTextBoxColumn5 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pnlNoSaves = new global::System.Windows.Forms.Panel();
			this.lblNoSaves = new global::System.Windows.Forms.Label();
			this.btnGamesInServer = new global::System.Windows.Forms.Button();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.cbDrives = new global::System.Windows.Forms.ComboBox();
			this.pnlBackup = new global::System.Windows.Forms.Panel();
			this.gbManageProfile = new global::PS3SaveEditor.CustomGroupBox();
			this.gbProfiles = new global::PS3SaveEditor.CustomGroupBox();
			this.lblManageProfiles = new global::System.Windows.Forms.Label();
			this.btnManageProfiles = new global::System.Windows.Forms.Button();
			this.groupBox2 = new global::PS3SaveEditor.CustomGroupBox();
			this.lblDeactivate = new global::System.Windows.Forms.Label();
			this.btnDeactivate = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::PS3SaveEditor.CustomGroupBox();
			this.lblRSSSection = new global::System.Windows.Forms.Label();
			this.btnRss = new global::System.Windows.Forms.Button();
			this.gbBackupLocation = new global::PS3SaveEditor.CustomGroupBox();
			this.btnOpenFolder = new global::System.Windows.Forms.Button();
			this.lblBackup = new global::System.Windows.Forms.Label();
			this.btnBrowse = new global::System.Windows.Forms.Button();
			this.txtBackupLocation = new global::System.Windows.Forms.TextBox();
			this.chkBackup = new global::System.Windows.Forms.CheckBox();
			this.btnApply = new global::System.Windows.Forms.Button();
			this.Multi = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.panel2 = new global::System.Windows.Forms.Panel();
			this.panel3 = new global::System.Windows.Forms.Panel();
			this.picVersion = new global::System.Windows.Forms.PictureBox();
			this.pictureBox2 = new global::System.Windows.Forms.PictureBox();
			this.picTraffic = new global::System.Windows.Forms.PictureBox();
			this.contextMenuStrip1.SuspendLayout();
			this.pnlHome.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgServerGames).BeginInit();
			this.pnlNoSaves.SuspendLayout();
			this.panel1.SuspendLayout();
			this.pnlBackup.SuspendLayout();
			this.gbManageProfile.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.gbBackupLocation.SuspendLayout();
			this.panel3.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.picVersion).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.picTraffic).BeginInit();
			base.SuspendLayout();
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.simpleToolStripMenuItem,
				this.advancedToolStripMenuItem,
				this.toolStripSeparator1,
				this.resignToolStripMenuItem,
				this.registerPSNIDToolStripMenuItem,
				this.toolStripSeparator2,
				this.restoreFromBackupToolStripMenuItem,
				this.deleteSaveToolStripMenuItem
			});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new global::System.Drawing.Size(185, 148);
			this.contextMenuStrip1.Opening += new global::System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			this.simpleToolStripMenuItem.Name = "simpleToolStripMenuItem";
			this.simpleToolStripMenuItem.Size = new global::System.Drawing.Size(184, 22);
			this.simpleToolStripMenuItem.Text = "Simple";
			this.simpleToolStripMenuItem.Click += new global::System.EventHandler(this.simpleToolStripMenuItem_Click);
			this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
			this.advancedToolStripMenuItem.Size = new global::System.Drawing.Size(184, 22);
			this.advancedToolStripMenuItem.Text = "Advanced";
			this.advancedToolStripMenuItem.Click += new global::System.EventHandler(this.advancedToolStripMenuItem_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new global::System.Drawing.Size(181, 6);
			this.resignToolStripMenuItem.Name = "resignToolStripMenuItem";
			this.resignToolStripMenuItem.Size = new global::System.Drawing.Size(184, 22);
			this.resignToolStripMenuItem.Text = "Re-sign...";
			this.resignToolStripMenuItem.Click += new global::System.EventHandler(this.resignToolStripMenuItem_Click);
			this.registerPSNIDToolStripMenuItem.Name = "registerPSNIDToolStripMenuItem";
			this.registerPSNIDToolStripMenuItem.Size = new global::System.Drawing.Size(184, 22);
			this.registerPSNIDToolStripMenuItem.Text = "Register PSN ID...";
			this.registerPSNIDToolStripMenuItem.Click += new global::System.EventHandler(this.registerPSNIDToolStripMenuItem_Click);
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new global::System.Drawing.Size(181, 6);
			this.restoreFromBackupToolStripMenuItem.Name = "restoreFromBackupToolStripMenuItem";
			this.restoreFromBackupToolStripMenuItem.Size = new global::System.Drawing.Size(184, 22);
			this.restoreFromBackupToolStripMenuItem.Text = "Restore from Backup";
			this.restoreFromBackupToolStripMenuItem.Click += new global::System.EventHandler(this.restoreFromBackupToolStripMenuItem_Click);
			this.deleteSaveToolStripMenuItem.Name = "deleteSaveToolStripMenuItem";
			this.deleteSaveToolStripMenuItem.Size = new global::System.Drawing.Size(184, 22);
			this.deleteSaveToolStripMenuItem.Text = "Delete Save";
			this.deleteSaveToolStripMenuItem.Click += new global::System.EventHandler(this.deleteSaveToolStripMenuItem_Click);
			this.btnHome.BackColor = global::System.Drawing.Color.Transparent;
			this.btnHome.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.btnHome.FlatAppearance.BorderColor = global::System.Drawing.Color.FromArgb(127, 215, 255);
			this.btnHome.FlatAppearance.BorderSize = 0;
			this.btnHome.FlatAppearance.MouseDownBackColor = global::System.Drawing.Color.Transparent;
			this.btnHome.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.btnHome.Location = new global::System.Drawing.Point(15, 15);
			this.btnHome.Name = "btnHome";
			this.btnHome.Size = new global::System.Drawing.Size(237, 61);
			this.btnHome.TabIndex = 3;
			this.btnHome.UseVisualStyleBackColor = false;
			this.btnHome.Click += new global::System.EventHandler(this.btnHome_Click);
			this.btnHelp.BackColor = global::System.Drawing.Color.Transparent;
			this.btnHelp.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.btnHelp.FlatAppearance.BorderColor = global::System.Drawing.Color.FromArgb(127, 215, 255);
			this.btnHelp.FlatAppearance.BorderSize = 0;
			this.btnHelp.FlatAppearance.MouseDownBackColor = global::System.Drawing.Color.Transparent;
			this.btnHelp.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.btnHelp.Location = new global::System.Drawing.Point(15, 143);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new global::System.Drawing.Size(237, 61);
			this.btnHelp.TabIndex = 6;
			this.btnHelp.UseVisualStyleBackColor = false;
			this.btnHelp.Click += new global::System.EventHandler(this.btnHelp_Click);
			this.btnOptions.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOptions.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.btnOptions.FlatAppearance.BorderColor = global::System.Drawing.Color.FromArgb(127, 215, 255);
			this.btnOptions.FlatAppearance.BorderSize = 0;
			this.btnOptions.FlatAppearance.MouseDownBackColor = global::System.Drawing.Color.Transparent;
			this.btnOptions.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.btnOptions.Location = new global::System.Drawing.Point(15, 79);
			this.btnOptions.Name = "btnOptions";
			this.btnOptions.Size = new global::System.Drawing.Size(237, 61);
			this.btnOptions.TabIndex = 5;
			this.btnOptions.UseVisualStyleBackColor = false;
			this.btnOptions.Click += new global::System.EventHandler(this.btnBackup_Click);
			this.pnlHome.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.pnlHome.BackColor = global::System.Drawing.Color.FromArgb(102, 102, 102);
			this.pnlHome.Controls.Add(this.chkShowAll);
			this.pnlHome.Controls.Add(this.dgServerGames);
			this.pnlHome.Controls.Add(this.pnlNoSaves);
			this.pnlHome.Location = new global::System.Drawing.Point(257, 15);
			this.pnlHome.Name = "pnlHome";
			this.pnlHome.Size = new global::System.Drawing.Size(508, 347);
			this.pnlHome.TabIndex = 8;
			this.chkShowAll.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.chkShowAll.Location = new global::System.Drawing.Point(11, 324);
			this.chkShowAll.Name = "chkShowAll";
			this.chkShowAll.Size = new global::System.Drawing.Size(97, 16);
			this.chkShowAll.TabIndex = 11;
			this.chkShowAll.Text = "Show All";
			this.chkShowAll.UseVisualStyleBackColor = true;
			this.dgServerGames.AllowUserToAddRows = false;
			this.dgServerGames.AllowUserToDeleteRows = false;
			this.dgServerGames.AllowUserToResizeRows = false;
			this.dgServerGames.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.dgServerGames.ClipboardCopyMode = global::System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgServerGames.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgServerGames.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgServerGames.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.Choose,
				this.dataGridViewTextBoxColumn1,
				this.dataGridViewTextBoxColumn2,
				this.dataGridViewTextBoxColumn3,
				this.dataGridViewTextBoxColumn4,
				this.dataGridViewCheckBoxColumn1,
				this.dataGridViewTextBoxColumn5
			});
			this.dgServerGames.ContextMenuStrip = this.contextMenuStrip1;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgServerGames.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgServerGames.Location = new global::System.Drawing.Point(12, 12);
			this.dgServerGames.Name = "dgServerGames";
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgServerGames.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgServerGames.RowHeadersVisible = false;
			this.dgServerGames.RowHeadersWidth = 25;
			this.dgServerGames.RowTemplate.Height = 24;
			this.dgServerGames.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgServerGames.Size = new global::System.Drawing.Size(484, 304);
			this.dgServerGames.TabIndex = 1;
			this.Choose.Frozen = true;
			this.Choose.HeaderText = "Choose";
			this.Choose.Name = "Choose";
			this.Choose.ReadOnly = true;
			this.Choose.Width = 20;
			this.dataGridViewTextBoxColumn1.FillWeight = 20f;
			this.dataGridViewTextBoxColumn1.Frozen = true;
			this.dataGridViewTextBoxColumn1.HeaderText = "Game Name";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Width = 240;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle4;
			this.dataGridViewTextBoxColumn2.Frozen = true;
			this.dataGridViewTextBoxColumn2.HeaderText = "Cheats";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.Width = 60;
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle5;
			this.dataGridViewTextBoxColumn3.HeaderText = "GameCode";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Width = 80;
			this.dataGridViewTextBoxColumn4.HeaderText = "Client";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.Width = 80;
			this.dataGridViewCheckBoxColumn1.HeaderText = "Local Save Exist";
			this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
			this.dataGridViewCheckBoxColumn1.ReadOnly = true;
			this.dataGridViewCheckBoxColumn1.Visible = false;
			this.dataGridViewTextBoxColumn5.HeaderText = "Client";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.Visible = false;
			this.pnlNoSaves.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.pnlNoSaves.Controls.Add(this.lblNoSaves);
			this.pnlNoSaves.Location = new global::System.Drawing.Point(12, 12);
			this.pnlNoSaves.Name = "pnlNoSaves";
			this.pnlNoSaves.Size = new global::System.Drawing.Size(485, 311);
			this.pnlNoSaves.TabIndex = 7;
			this.pnlNoSaves.Visible = false;
			this.lblNoSaves.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.lblNoSaves.BackColor = global::System.Drawing.Color.Transparent;
			this.lblNoSaves.ForeColor = global::System.Drawing.Color.White;
			this.lblNoSaves.Location = new global::System.Drawing.Point(0, 100);
			this.lblNoSaves.Name = "lblNoSaves";
			this.lblNoSaves.Size = new global::System.Drawing.Size(480, 13);
			this.lblNoSaves.TabIndex = 10;
			this.lblNoSaves.Text = "No PS4 saves were found on this USB drive.";
			this.lblNoSaves.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.btnGamesInServer.Location = new global::System.Drawing.Point(0, 0);
			this.btnGamesInServer.Name = "btnGamesInServer";
			this.btnGamesInServer.Size = new global::System.Drawing.Size(75, 23);
			this.btnGamesInServer.TabIndex = 0;
			this.panel1.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.panel1.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.panel1.Controls.Add(this.cbDrives);
			this.panel1.Location = new global::System.Drawing.Point(15, 332);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(237, 30);
			this.panel1.TabIndex = 11;
			this.cbDrives.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDrives.FormattingEnabled = true;
			this.cbDrives.Location = new global::System.Drawing.Point(190, 6);
			this.cbDrives.Name = "cbDrives";
			this.cbDrives.Size = new global::System.Drawing.Size(40, 21);
			this.cbDrives.TabIndex = 3;
			this.pnlBackup.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.pnlBackup.BackColor = global::System.Drawing.Color.FromArgb(102, 102, 102);
			this.pnlBackup.Controls.Add(this.gbManageProfile);
			this.pnlBackup.Controls.Add(this.groupBox2);
			this.pnlBackup.Controls.Add(this.groupBox1);
			this.pnlBackup.Controls.Add(this.gbBackupLocation);
			this.pnlBackup.Location = new global::System.Drawing.Point(257, 15);
			this.pnlBackup.Name = "pnlBackup";
			this.pnlBackup.Size = new global::System.Drawing.Size(508, 347);
			this.pnlBackup.TabIndex = 9;
			this.gbManageProfile.Controls.Add(this.gbProfiles);
			this.gbManageProfile.Controls.Add(this.lblManageProfiles);
			this.gbManageProfile.Controls.Add(this.btnManageProfiles);
			this.gbManageProfile.Location = new global::System.Drawing.Point(12, 270);
			this.gbManageProfile.Name = "gbManageProfile";
			this.gbManageProfile.Size = new global::System.Drawing.Size(483, 65);
			this.gbManageProfile.TabIndex = 10;
			this.gbManageProfile.TabStop = false;
			this.gbProfiles.Location = new global::System.Drawing.Point(134, 30);
			this.gbProfiles.Name = "gbProfiles";
			this.gbProfiles.Size = new global::System.Drawing.Size(80, 27);
			this.gbProfiles.TabIndex = 9;
			this.gbProfiles.TabStop = false;
			this.lblManageProfiles.AutoSize = true;
			this.lblManageProfiles.ForeColor = global::System.Drawing.Color.White;
			this.lblManageProfiles.Location = new global::System.Drawing.Point(10, 15);
			this.lblManageProfiles.Name = "lblManageProfiles";
			this.lblManageProfiles.Size = new global::System.Drawing.Size(106, 13);
			this.lblManageProfiles.TabIndex = 8;
			this.lblManageProfiles.Text = "Manage PS4 Profiles";
			this.btnManageProfiles.AutoSize = true;
			this.btnManageProfiles.ForeColor = global::System.Drawing.Color.White;
			this.btnManageProfiles.Location = new global::System.Drawing.Point(10, 33);
			this.btnManageProfiles.Name = "btnManageProfiles";
			this.btnManageProfiles.Size = new global::System.Drawing.Size(115, 23);
			this.btnManageProfiles.TabIndex = 0;
			this.btnManageProfiles.Text = "Manage Profiles";
			this.btnManageProfiles.UseVisualStyleBackColor = false;
			this.btnManageProfiles.Click += new global::System.EventHandler(this.btnManageProfiles_Click);
			this.groupBox2.Controls.Add(this.lblDeactivate);
			this.groupBox2.Controls.Add(this.btnDeactivate);
			this.groupBox2.Location = new global::System.Drawing.Point(12, 200);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new global::System.Drawing.Size(483, 65);
			this.groupBox2.TabIndex = 9;
			this.groupBox2.TabStop = false;
			this.lblDeactivate.AutoSize = true;
			this.lblDeactivate.ForeColor = global::System.Drawing.Color.White;
			this.lblDeactivate.Location = new global::System.Drawing.Point(10, 15);
			this.lblDeactivate.Name = "lblDeactivate";
			this.lblDeactivate.Size = new global::System.Drawing.Size(42, 13);
			this.lblDeactivate.TabIndex = 8;
			this.lblDeactivate.Text = "Testing";
			this.btnDeactivate.AutoSize = true;
			this.btnDeactivate.ForeColor = global::System.Drawing.Color.White;
			this.btnDeactivate.Location = new global::System.Drawing.Point(10, 35);
			this.btnDeactivate.Name = "btnDeactivate";
			this.btnDeactivate.Size = new global::System.Drawing.Size(115, 23);
			this.btnDeactivate.TabIndex = 0;
			this.btnDeactivate.Text = "Deactivate";
			this.btnDeactivate.UseVisualStyleBackColor = false;
			this.btnDeactivate.Click += new global::System.EventHandler(this.btnDeactivate_Click);
			this.groupBox1.Controls.Add(this.lblRSSSection);
			this.groupBox1.Controls.Add(this.btnRss);
			this.groupBox1.Location = new global::System.Drawing.Point(12, 128);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(483, 67);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.lblRSSSection.AutoSize = true;
			this.lblRSSSection.ForeColor = global::System.Drawing.Color.White;
			this.lblRSSSection.Location = new global::System.Drawing.Point(10, 15);
			this.lblRSSSection.Name = "lblRSSSection";
			this.lblRSSSection.Size = new global::System.Drawing.Size(295, 13);
			this.lblRSSSection.TabIndex = 6;
			this.lblRSSSection.Text = "Select below button to check the availability of latest version.";
			this.btnRss.ForeColor = global::System.Drawing.Color.White;
			this.btnRss.Location = new global::System.Drawing.Point(10, 37);
			this.btnRss.Name = "btnRss";
			this.btnRss.Size = new global::System.Drawing.Size(115, 23);
			this.btnRss.TabIndex = 0;
			this.btnRss.Text = "Update";
			this.btnRss.UseVisualStyleBackColor = false;
			this.btnRss.Click += new global::System.EventHandler(this.btnRss_Click);
			this.gbBackupLocation.Controls.Add(this.btnOpenFolder);
			this.gbBackupLocation.Controls.Add(this.lblBackup);
			this.gbBackupLocation.Controls.Add(this.btnBrowse);
			this.gbBackupLocation.Controls.Add(this.txtBackupLocation);
			this.gbBackupLocation.Controls.Add(this.chkBackup);
			this.gbBackupLocation.Controls.Add(this.btnApply);
			this.gbBackupLocation.ForeColor = global::System.Drawing.Color.White;
			this.gbBackupLocation.Location = new global::System.Drawing.Point(12, 8);
			this.gbBackupLocation.Margin = new global::System.Windows.Forms.Padding(0);
			this.gbBackupLocation.Name = "gbBackupLocation";
			this.gbBackupLocation.Padding = new global::System.Windows.Forms.Padding(0);
			this.gbBackupLocation.Size = new global::System.Drawing.Size(483, 115);
			this.gbBackupLocation.TabIndex = 3;
			this.gbBackupLocation.TabStop = false;
			this.btnOpenFolder.ForeColor = global::System.Drawing.Color.White;
			this.btnOpenFolder.Location = new global::System.Drawing.Point(11, 85);
			this.btnOpenFolder.Name = "btnOpenFolder";
			this.btnOpenFolder.Size = new global::System.Drawing.Size(123, 23);
			this.btnOpenFolder.TabIndex = 3;
			this.btnOpenFolder.Text = "Open Folder";
			this.btnOpenFolder.UseVisualStyleBackColor = false;
			this.btnOpenFolder.Click += new global::System.EventHandler(this.btnOpenFolder_Click);
			this.lblBackup.AutoSize = true;
			this.lblBackup.Location = new global::System.Drawing.Point(10, 40);
			this.lblBackup.Name = "lblBackup";
			this.lblBackup.Size = new global::System.Drawing.Size(0, 13);
			this.lblBackup.TabIndex = 5;
			this.btnBrowse.ForeColor = global::System.Drawing.Color.White;
			this.btnBrowse.Location = new global::System.Drawing.Point(281, 60);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new global::System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.UseVisualStyleBackColor = false;
			this.btnBrowse.Click += new global::System.EventHandler(this.btnBrowse_Click);
			this.txtBackupLocation.Location = new global::System.Drawing.Point(11, 61);
			this.txtBackupLocation.Name = "txtBackupLocation";
			this.txtBackupLocation.Size = new global::System.Drawing.Size(264, 20);
			this.txtBackupLocation.TabIndex = 0;
			this.chkBackup.AutoSize = true;
			this.chkBackup.ForeColor = global::System.Drawing.Color.White;
			this.chkBackup.Location = new global::System.Drawing.Point(10, 15);
			this.chkBackup.Name = "chkBackup";
			this.chkBackup.Size = new global::System.Drawing.Size(96, 17);
			this.chkBackup.TabIndex = 0;
			this.chkBackup.Text = "Backup Saves";
			this.chkBackup.UseVisualStyleBackColor = true;
			this.chkBackup.CheckedChanged += new global::System.EventHandler(this.chkBackup_CheckedChanged);
			this.chkBackup.Click += new global::System.EventHandler(this.chkBackup_Click);
			this.btnApply.BackColor = global::System.Drawing.Color.FromArgb(102, 102, 102);
			this.btnApply.ForeColor = global::System.Drawing.Color.White;
			this.btnApply.Location = new global::System.Drawing.Point(11, 85);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new global::System.Drawing.Size(75, 23);
			this.btnApply.TabIndex = 2;
			this.btnApply.Text = "Apply";
			this.btnApply.UseVisualStyleBackColor = false;
			this.btnApply.Visible = false;
			this.btnApply.Click += new global::System.EventHandler(this.btnApply_Click);
			this.Multi.FillWeight = 20f;
			this.Multi.Frozen = true;
			this.Multi.Name = "Multi";
			this.Multi.ReadOnly = true;
			this.Multi.Width = 20;
			this.panel2.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.panel2.BackColor = global::System.Drawing.Color.FromArgb(51, 51, 51);
			this.panel2.Location = new global::System.Drawing.Point(15, 215);
			this.panel2.Name = "panel2";
			this.panel2.Size = new global::System.Drawing.Size(237, 3);
			this.panel2.TabIndex = 12;
			this.panel3.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.panel3.BackgroundImage = (global::System.Drawing.Image)componentResourceManager.GetObject("panel3.BackgroundImage");
			this.panel3.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.panel3.Controls.Add(this.picVersion);
			this.panel3.Controls.Add(this.pictureBox2);
			this.panel3.Controls.Add(this.picTraffic);
			this.panel3.Location = new global::System.Drawing.Point(16, 207);
			this.panel3.Name = "panel3";
			this.panel3.Size = new global::System.Drawing.Size(237, 122);
			this.panel3.TabIndex = 13;
			this.picVersion.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.None;
			this.picVersion.Location = new global::System.Drawing.Point(0, 24);
			this.picVersion.Name = "picVersion";
			this.picVersion.Size = new global::System.Drawing.Size(237, 26);
			this.picVersion.TabIndex = 13;
			this.picVersion.TabStop = false;
			this.pictureBox2.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.pictureBox2.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.None;
			this.pictureBox2.Location = new global::System.Drawing.Point(0, 78);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new global::System.Drawing.Size(237, 45);
			this.pictureBox2.TabIndex = 12;
			this.pictureBox2.TabStop = false;
			this.picTraffic.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.None;
			this.picTraffic.Location = new global::System.Drawing.Point(0, 0);
			this.picTraffic.Name = "picTraffic";
			this.picTraffic.Size = new global::System.Drawing.Size(237, 26);
			this.picTraffic.TabIndex = 11;
			this.picTraffic.TabStop = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = global::System.Drawing.Color.FromArgb(0, 44, 101);
			base.ClientSize = new global::System.Drawing.Size(780, 377);
			base.Controls.Add(this.panel3);
			base.Controls.Add(this.pnlBackup);
			base.Controls.Add(this.pnlHome);
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.btnOptions);
			base.Controls.Add(this.btnHome);
			base.Controls.Add(this.btnHelp);
			this.DoubleBuffered = true;
			this.MinimumSize = new global::System.Drawing.Size(780, 377);
			base.Name = "MainForm";
			this.Text = "PS4 Save Editor";
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.contextMenuStrip1.ResumeLayout(false);
			this.pnlHome.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgServerGames).EndInit();
			this.pnlNoSaves.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.pnlBackup.ResumeLayout(false);
			this.gbManageProfile.ResumeLayout(false);
			this.gbManageProfile.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.gbBackupLocation.ResumeLayout(false);
			this.gbBackupLocation.PerformLayout();
			this.panel3.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.picVersion).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.picTraffic).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04000244 RID: 580
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000246 RID: 582
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000247 RID: 583
		private global::System.Windows.Forms.ToolStripMenuItem simpleToolStripMenuItem;

		// Token: 0x04000248 RID: 584
		private global::System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;

		// Token: 0x04000249 RID: 585
		private global::System.Windows.Forms.Button btnHome;

		// Token: 0x0400024A RID: 586
		private global::System.Windows.Forms.Button btnHelp;

		// Token: 0x0400024B RID: 587
		private global::System.Windows.Forms.Button btnOptions;

		// Token: 0x0400024C RID: 588
		private global::System.Windows.Forms.Panel pnlHome;

		// Token: 0x0400024D RID: 589
		private global::System.Windows.Forms.ToolStripMenuItem restoreFromBackupToolStripMenuItem;

		// Token: 0x0400024E RID: 590
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x0400024F RID: 591
		private global::System.Windows.Forms.ComboBox cbDrives;

		// Token: 0x04000250 RID: 592
		private global::CSUST.Data.CustomDataGridView dgServerGames;

		// Token: 0x04000251 RID: 593
		private global::System.Windows.Forms.ToolStripMenuItem deleteSaveToolStripMenuItem;

		// Token: 0x04000252 RID: 594
		private global::System.Windows.Forms.Button btnGamesInServer;

		// Token: 0x04000253 RID: 595
		private global::System.Windows.Forms.CheckBox chkShowAll;

		// Token: 0x04000254 RID: 596
		private global::System.Windows.Forms.Panel pnlNoSaves;

		// Token: 0x04000255 RID: 597
		private global::System.Windows.Forms.Label lblNoSaves;

		// Token: 0x04000256 RID: 598
		private global::System.Windows.Forms.Button btnOpenFolder;

		// Token: 0x04000257 RID: 599
		private global::System.Windows.Forms.Label lblBackup;

		// Token: 0x04000258 RID: 600
		private global::System.Windows.Forms.Button btnBrowse;

		// Token: 0x04000259 RID: 601
		private global::System.Windows.Forms.TextBox txtBackupLocation;

		// Token: 0x0400025A RID: 602
		private global::System.Windows.Forms.CheckBox chkBackup;

		// Token: 0x0400025B RID: 603
		private global::System.Windows.Forms.Button btnApply;

		// Token: 0x0400025C RID: 604
		private global::System.Windows.Forms.Label lblRSSSection;

		// Token: 0x0400025D RID: 605
		private global::System.Windows.Forms.Button btnRss;

		// Token: 0x0400025E RID: 606
		private global::System.Windows.Forms.Label lblDeactivate;

		// Token: 0x0400025F RID: 607
		private global::System.Windows.Forms.Button btnDeactivate;

		// Token: 0x04000260 RID: 608
		private global::System.Windows.Forms.Panel pnlBackup;

		// Token: 0x04000261 RID: 609
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Multi;

		// Token: 0x04000262 RID: 610
		private global::System.Windows.Forms.Label lblManageProfiles;

		// Token: 0x04000263 RID: 611
		private global::System.Windows.Forms.Button btnManageProfiles;

		// Token: 0x04000264 RID: 612
		private global::System.Windows.Forms.ToolStripMenuItem registerPSNIDToolStripMenuItem;

		// Token: 0x04000265 RID: 613
		private global::System.Windows.Forms.ToolStripMenuItem resignToolStripMenuItem;

		// Token: 0x04000266 RID: 614
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

		// Token: 0x04000267 RID: 615
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

		// Token: 0x04000268 RID: 616
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Choose;

		// Token: 0x04000269 RID: 617
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x0400026A RID: 618
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x0400026B RID: 619
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x0400026C RID: 620
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x0400026D RID: 621
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

		// Token: 0x0400026E RID: 622
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

		// Token: 0x0400026F RID: 623
		private global::System.Windows.Forms.Panel panel2;

		// Token: 0x04000270 RID: 624
		private global::PS3SaveEditor.CustomGroupBox gbBackupLocation;

		// Token: 0x04000271 RID: 625
		private global::PS3SaveEditor.CustomGroupBox groupBox1;

		// Token: 0x04000272 RID: 626
		private global::PS3SaveEditor.CustomGroupBox groupBox2;

		// Token: 0x04000273 RID: 627
		private global::PS3SaveEditor.CustomGroupBox gbManageProfile;

		// Token: 0x04000274 RID: 628
		private global::PS3SaveEditor.CustomGroupBox gbProfiles;

		// Token: 0x04000275 RID: 629
		private global::System.Windows.Forms.Panel panel3;

		// Token: 0x04000276 RID: 630
		private global::System.Windows.Forms.PictureBox picTraffic;

		// Token: 0x04000277 RID: 631
		private global::System.Windows.Forms.PictureBox pictureBox2;

		// Token: 0x04000278 RID: 632
		private global::System.Windows.Forms.PictureBox picVersion;
	}
}
