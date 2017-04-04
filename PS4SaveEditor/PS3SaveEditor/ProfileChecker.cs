using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x0200001D RID: 29
	public partial class ProfileChecker : Form
	{
		// Token: 0x0600012A RID: 298 RVA: 0x0000AA78 File Offset: 0x00008C78
		public ProfileChecker(int regMode = 0, string psn = null, string drive = null)
		{
			this.InitializeComponent();
			base.CenterToScreen();
			this.txtProfileName.MaxLength = 32;
			this.Text = Resources.titlePSNAdd;
			this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.lblTitle1.Text = Resources.lblPSNAddTitle;
			this.lblInstructions.Text = Resources.lblInstructionsPage1;
			this.lblInstruction1.Text = Resources.lblInstruction1;
			this.lblInstrucionRed.Text = Resources.lblInstruction1Red;
			this.lblInstruction2.Text = Resources.lblInstruction_2;
			this.lblInstruciton3.Text = Resources.lblInstruction3;
			this.lblInstructionPage1.Text = Resources.lblPage1;
			this.btnNext.Text = Resources.btnPage1;
			this.lblPageTitle.Text = Resources.lblPSNAddTitle;
			this.lblInstructionPage2.Text = Resources.lblPage2;
			this.lblUserName.Text = Resources.lblUserName;
			this.lblInstruction2Page2.Text = Resources.lblPage21;
			this.lblFooter2.Text = Resources.lblInstructionPage2;
			this.panelProfileName.Visible = false;
			this.panelFinish.Visible = false;
			this.lblTitleFinish.Text = Resources.titlePSNAdd;
			this.lblFinish.Text = Resources.lblInstuctionPage3;
			Control.CheckForIllegalCrossThreadCalls = false;
			this.btnNext.Click += new EventHandler(this.btnNext_Click);
			this.txtProfileName.TextChanged += new EventHandler(this.txtProfileName_TextChanged);
			base.DialogResult = DialogResult.Cancel;
			this.m_registerMode = regMode;
			if (regMode > 0)
			{
				if (regMode == 1)
				{
					this.panelInstructions.Visible = false;
					this.panelProfileName.Visible = true;
					this.btnNext.Enabled = false;
					this.btnNext.Text = Resources.btnPage2;
				}
				this.psnId = psn;
				this.lblDriveLetter.Text = string.Format(Resources.lblDrive, drive);
				return;
			}
			base.Load += new EventHandler(this.ProfileChecker_Load);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000AC88 File Offset: 0x00008E88
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000ACF0 File Offset: 0x00008EF0
		private void txtProfileName_TextChanged(object sender, EventArgs e)
		{
			this.txtProfileName.Text = this.txtProfileName.Text.Trim(new char[]
			{
				'.',
				'"',
				'/',
				'\\',
				'[',
				']',
				':',
				';',
				'|',
				'=',
				',',
				'?',
				'%',
				'<',
				'>',
				'&'
			});
			this.txtProfileName.SelectionStart = this.txtProfileName.Text.Length;
			this.btnNext.Enabled = (this.txtProfileName.Text.Length > 0);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000ADB4 File Offset: 0x00008FB4
		private void btnNext_Click(object sender, EventArgs e)
		{
			if (this.panelInstructions.Visible)
			{
				this.panelInstructions.Visible = false;
				this.panelProfileName.Visible = true;
				this.btnNext.Enabled = false;
				this.btnNext.Text = Resources.btnPage2;
				return;
			}
			if (!this.panelProfileName.Visible)
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
				return;
			}
			int num;
			if (this.RegisterPSNID(this.psnId, this.txtProfileName.Text, out num))
			{
				if (this.m_registerMode > 0)
				{
					base.DialogResult = DialogResult.OK;
					base.Close();
					return;
				}
				this.panelProfileName.Visible = false;
				this.panelFinish.Visible = true;
				this.btnNext.Text = Resources.btnOK;
				return;
			}
			else
			{
				if (num == 4121)
				{
					MessageBox.Show(Resources.errPSNNameUsed, Resources.msgError);
					return;
				}
				MessageBox.Show("Error occurred while registering PSN ID.");
				return;
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000AEA0 File Offset: 0x000090A0
		private bool RegisterPSNID(string psnId, string name, out int errorCode)
		{
			errorCode = 0;
			WebClientEx webClientEx = new WebClientEx();
			webClientEx.Credentials = Util.GetNetworkCredential();
			webClientEx.Encoding = Encoding.UTF8;
			webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
			byte[] bytes = webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.UTF8.GetBytes(string.Format("{{\"action\":\"REGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\",\"friendly_name\":\"{2}\"}}", Util.GetUserId(), psnId.Trim(), name.Trim())));
			string @string = Encoding.UTF8.GetString(bytes);
			Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(@string, typeof(Dictionary<string, object>)) as Dictionary<string, object>;
			if (dictionary.ContainsKey("status") && (string)dictionary["status"] == "OK")
			{
				return true;
			}
			if (dictionary.ContainsKey("code"))
			{
				errorCode = Convert.ToInt32(dictionary["code"]);
			}
			return false;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000AF8B File Offset: 0x0000918B
		private void ProfileChecker_Load(object sender, EventArgs e)
		{
			this.btnNext.Enabled = false;
			this.CheckDrives();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000AFA0 File Offset: 0x000091A0
		private void CheckDrives()
		{
			this.btnNext.Enabled = false;
			this.lblDriveLetter.Text = string.Format(Resources.lblDrive, "---");
			DriveInfo[] drives = DriveInfo.GetDrives();
			DriveInfo[] array = drives;
			for (int i = 0; i < array.Length; i++)
			{
				DriveInfo driveInfo = array[i];
				if (driveInfo.IsReady && driveInfo.DriveType == DriveType.Removable)
				{
					string path = Path.Combine(driveInfo.RootDirectory.Name, "PS4\\SAVEDATA");
					if (Directory.Exists(path) && Directory.GetDirectories(path).Length > 0)
					{
						this.psnId = Path.GetFileName(Directory.GetDirectories(path).First<string>());
						this.btnNext.Enabled = true;
						this.lblDriveLetter.Text = string.Format(Resources.lblDrive, driveInfo.RootDirectory.Name);
						return;
					}
				}
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000B078 File Offset: 0x00009278
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 537)
			{
				Thread thread = new Thread(new ThreadStart(this.CheckDrives));
				thread.Start();
			}
			base.WndProc(ref m);
		}

		// Token: 0x0400009E RID: 158
		private const string REGISTER_PSNID = "{{\"action\":\"REGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\",\"friendly_name\":\"{2}\"}}";

		// Token: 0x0400009F RID: 159
		private string psnId;

		// Token: 0x040000A0 RID: 160
		private int m_registerMode;
	}
}
