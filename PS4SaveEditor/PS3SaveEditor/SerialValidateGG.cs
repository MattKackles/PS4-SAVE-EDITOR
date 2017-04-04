using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Microsoft.Win32;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x0200009C RID: 156
	public partial class SerialValidateGG : Form
	{
		// Token: 0x0600072D RID: 1837 RVA: 0x00029BF4 File Offset: 0x00027DF4
		public SerialValidateGG()
		{
			this.InitializeComponent();
			this.BackColor = Color.FromArgb(0, 0, 0);
			this.panel1.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.btnOk.BackColor = SystemColors.ButtonFace;
			this.lblInstruction.BackColor = Color.Transparent;
			this.lblInstruction2.BackColor = Color.Transparent;
			this.label1.BackColor = (this.label2.BackColor = (this.label3.BackColor = (this.label4.BackColor = Color.Transparent)));
			this.UpdateStatus = new SerialValidateGG.UpdateStatusDelegate(this.UpdateStatusSafe);
			this.CloseForm = new SerialValidateGG.CloseDelegate(this.CloseFormSafe);
			this.EnableOk = new SerialValidateGG.EnableOkDelegate(this.EnableOkSafe);
			Util.SetForegroundWindow(base.Handle);
			base.CenterToScreen();
			this.txtSerial1.TextChanged += new EventHandler(this.txtSerial_TextChanged);
			this.txtSerial1.KeyDown += new KeyEventHandler(this.txtSerial_KeyDown);
			this.txtSerial1.KeyPress += new KeyPressEventHandler(this.txtSerial_KeyPress);
			this.txtSerial2.TextChanged += new EventHandler(this.txtSerial_TextChanged);
			this.txtSerial2.KeyDown += new KeyEventHandler(this.txtSerial_KeyDown);
			this.txtSerial2.KeyPress += new KeyPressEventHandler(this.txtSerial_KeyPress);
			this.txtSerial3.TextChanged += new EventHandler(this.txtSerial_TextChanged);
			this.txtSerial3.KeyDown += new KeyEventHandler(this.txtSerial_KeyDown);
			this.txtSerial3.KeyPress += new KeyPressEventHandler(this.txtSerial_KeyPress);
			this.txtSerial4.TextChanged += new EventHandler(this.txtSerial_TextChanged);
			this.txtSerial4.KeyDown += new KeyEventHandler(this.txtSerial_KeyDown);
			this.txtSerial4.KeyPress += new KeyPressEventHandler(this.txtSerial_KeyPress);
			this.Text = Resources.titleSerialEntry;
			this.lblInstruction2.Text = Resources.lblInstruction2;
			this.lblInstruction.Text = "";
			base.Load += new EventHandler(this.SerialValidateGG_Load);
			this.btnOk.Enabled = false;
			string arg_250_0 = this.m_serial;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00029E54 File Offset: 0x00028054
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(base.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
			{
				e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			}
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00029EBC File Offset: 0x000280BC
		private void CheckForDevice()
		{
			Thread.Sleep(10000);
			this.m_serial = null;
			this.FindGGUSB();
			if (this.m_serial != null)
			{
				if (this.label1.IsHandleCreated)
				{
					this.label1.Invoke(this.UpdateStatus, new object[]
					{
						"Please wait. Registering Game Genie Save Editor for PS3."
					});
				}
				this.RegisterSerial();
			}
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00029F1D File Offset: 0x0002811D
		private void UpdateStatusSafe(string status)
		{
			this.label1.Text = status;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00029F2C File Offset: 0x0002812C
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 537 && m.WParam.ToInt32() == 32768)
			{
				m.LParam != IntPtr.Zero;
			}
			base.WndProc(ref m);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00029F73 File Offset: 0x00028173
		private void SerialValidateGG_Load(object sender, EventArgs e)
		{
			this.txtSerial1.Select();
			string arg_11_0 = this.m_serial;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00029F88 File Offset: 0x00028188
		private void RegisterSerial()
		{
			try
			{
				WebClientEx webClientEx = new WebClientEx();
				webClientEx.Credentials = Util.GetNetworkCredential();
				string serial = this.m_serial;
				this.m_hash = SerialValidateGG.ComputeHash(serial);
				string uID = Util.GetUID(false, true);
				if (string.IsNullOrEmpty(uID))
				{
					MessageBox.Show("There appears to have been an issue activating. Please contact support.");
				}
				else
				{
					string uriString = string.Format("{0}/ps4auth", Util.GetBaseUrl(), this.m_hash);
					webClientEx.DownloadStringAsync(new Uri(uriString, UriKind.Absolute));
					webClientEx.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.client_DownloadStringCompleted);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.StackTrace);
				MessageBox.Show(Resources.errSerial, Resources.msgError);
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0002A058 File Offset: 0x00028258
		public static string ComputeHash(string serial)
		{
			string text = "";
			byte[] array = new byte[32];
			byte[] array2 = new byte[16];
			byte[] array3 = new byte[]
			{
				59,
				67,
				235,
				54,
				183,
				124,
				22,
				65,
				172,
				154,
				31,
				14,
				188,
				91,
				48,
				41
			};
			long value = long.Parse(serial, NumberStyles.HexNumber);
			byte[] array4 = null;
			if (serial.Length == 16)
			{
				byte[] bytes = BitConverter.GetBytes(value);
				Array.Reverse(bytes, 0, bytes.Length);
				Array.Copy(bytes, array2, bytes.Length);
				for (int i = 0; i < 8; i++)
				{
					array[i] = (array2[i] ^ array3[i]);
				}
				Array.Copy(Encoding.ASCII.GetBytes("GameGenie"), 0, array, 8, "GameGenie".Length);
				array4 = SHA1.Create().ComputeHash(array, 0, 8 + "GameGenie".Length);
			}
			else if (serial.Length == 20)
			{
				byte[] bytes2 = BitConverter.GetBytes(value);
				Array.Reverse(bytes2, 0, bytes2.Length);
				Array.Copy(bytes2, 0, array2, 4, bytes2.Length);
				for (int j = 0; j < 12; j++)
				{
					array[j] = (array2[j] ^ array3[j]);
				}
				Array.Copy(Encoding.ASCII.GetBytes("GameGenie"), 0, array, 12, "GameGenie".Length);
				array4 = SHA1.Create().ComputeHash(array, 0, 12 + "GameGenie".Length);
			}
			if (array4 != null)
			{
				for (int k = 0; k < array4.Length; k++)
				{
					text += array4[k].ToString("X2");
				}
			}
			return text;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0002A1E4 File Offset: 0x000283E4
		private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				MessageBox.Show(Resources.errSerial, Resources.msgError);
				base.Invoke(this.CloseForm, new object[]
				{
					false
				});
				return;
			}
			string result = e.Result;
			if (result == null)
			{
				MessageBox.Show(Resources.errInvalidSerial, Resources.msgError);
				base.Invoke(this.CloseForm, new object[]
				{
					false
				});
				return;
			}
			if (result.IndexOf('#') > 0)
			{
				string[] array = result.Split(new char[]
				{
					'#'
				});
				if (array.Length > 1)
				{
					if (array[0] == "4")
					{
						MessageBox.Show(Resources.errInvalidSerial, Resources.msgError);
						base.Invoke(this.CloseForm, new object[]
						{
							false
						});
						return;
					}
					if (array[0] == "5")
					{
						MessageBox.Show(Resources.errTooManyTimes, Resources.msgError);
						base.Invoke(this.CloseForm, new object[]
						{
							false
						});
						return;
					}
				}
			}
			else
			{
				if (result.ToLower() == "toomanytimes" || result.ToLower().Contains("too many"))
				{
					MessageBox.Show(Resources.errTooManyTimes, Resources.msgError);
					base.Invoke(this.CloseForm, new object[]
					{
						false
					});
					return;
				}
				if (result == null || result.ToLower().Contains("error") || result.ToLower().Contains("not found"))
				{
					string text = result.Replace("ERROR", "");
					if (!text.Contains("1002"))
					{
						if (text.Contains("1014"))
						{
							MessageBox.Show(Resources.errOffline, Resources.msgInfo);
							base.Invoke(this.CloseForm, new object[]
							{
								false
							});
							return;
						}
						if (text.Contains("1005"))
						{
							MessageBox.Show(Resources.errTooManyTimes + text, Resources.msgError);
							base.Invoke(this.CloseForm, new object[]
							{
								false
							});
							return;
						}
						if (text.Contains("1007"))
						{
							Util.GetUID(true, true);
							this.RegisterSerial();
						}
						else
						{
							if (this.m_serial == null)
							{
								MessageBox.Show(Resources.errInvalidSerial + text, Resources.msgError);
							}
							else
							{
								MessageBox.Show(Resources.errInvalidUSB + text, Resources.msgError);
							}
							this.m_retryCount++;
							if (this.m_retryCount >= 3)
							{
								base.Invoke(this.CloseForm, new object[]
								{
									false
								});
								return;
							}
							if (this.m_serial == null)
							{
								this.btnOk.Invoke(this.EnableOk, new object[]
								{
									true
								});
							}
							else
							{
								this.btnOk.Invoke(this.EnableOk, new object[]
								{
									false
								});
							}
							this.label1.Invoke(this.UpdateStatus, new object[]
							{
								""
							});
							return;
						}
					}
				}
			}
			RegistryKey currentUser = Registry.CurrentUser;
			RegistryKey registryKey = currentUser.CreateSubKey(Util.GetRegistryBase());
			if (this.m_serial == null)
			{
				string s = string.Format("{0}-{1}-{2}-{3}", new object[]
				{
					this.txtSerial1.Text,
					this.txtSerial2.Text,
					this.txtSerial3.Text,
					this.txtSerial4.Text
				});
				this.m_hash = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(s)));
				this.m_hash = this.m_hash.Replace("-", "");
			}
			else
			{
				this.m_hash = SerialValidateGG.ComputeHash(this.m_serial);
			}
			registryKey.SetValue("Hash", this.m_hash.ToUpper());
			registryKey.SetValue("BackupSaves", "true");
			string value = string.Format("{0}-{1}-{2}-{3}", new object[]
			{
				this.txtSerial1.Text,
				this.txtSerial2.Text,
				this.txtSerial3.Text,
				this.txtSerial4.Text
			});
			registryKey.SetValue("Serial", value);
			registryKey.Close();
			currentUser.Close();
			try
			{
				if (base.IsHandleCreated)
				{
					base.Invoke(this.CloseForm, new object[]
					{
						true
					});
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0002A6DC File Offset: 0x000288DC
		private void EnableOkSafe(bool bEnable)
		{
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0002A6DE File Offset: 0x000288DE
		private void CloseFormSafe(bool bSuccess)
		{
			if (!bSuccess)
			{
				base.DialogResult = DialogResult.Abort;
			}
			else
			{
				base.DialogResult = DialogResult.OK;
			}
			base.Close();
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0002A6FC File Offset: 0x000288FC
		private void FindGGUSB()
		{
			foreach (USB.USBController current in USB.GetHostControllers())
			{
				USB.USBHub rootHub = current.GetRootHub();
				this.ProcessHub(rootHub);
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0002A750 File Offset: 0x00028950
		private void ProcessHub(USB.USBHub hub)
		{
			foreach (USB.USBPort current in hub.GetPorts())
			{
				if (current.IsHub)
				{
					this.ProcessHub(current.GetHub());
				}
				USB.USBDevice device = current.GetDevice();
				if (device != null && device.DeviceManufacturer != null && device.DeviceManufacturer.ToLower() == "dpdev" && device.DeviceProduct != null && device.DeviceProduct.ToLower() == "gamegenie")
				{
					this.m_serial = device.SerialNumber;
				}
			}
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0002A800 File Offset: 0x00028A00
		private bool ValidateSerial()
		{
			for (int i = 1; i <= 4; i++)
			{
				TextBox textBox = base.Controls.Find("txtSerial" + i, true)[0] as TextBox;
				if (textBox.Text.Length < 4)
				{
					textBox.Focus();
					MessageBox.Show(Resources.errInvalidSerial, this.Text);
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0002A868 File Offset: 0x00028A68
		private void btnOk_Click(object sender, EventArgs e)
		{
			this.m_serial = null;
			try
			{
				if (this.ValidateSerial())
				{
					this.btnOk.Enabled = false;
					this.btnCancel.Enabled = false;
					WebClientEx webClientEx = new WebClientEx();
					webClientEx.Credentials = Util.GetNetworkCredential();
					this.label1.Text = Resources.msgWaitSerial;
					webClientEx.Headers[HttpRequestHeader.ContentType] = "application/json";
					webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
					webClientEx.UploadDataCompleted += new UploadDataCompletedEventHandler(this.client_UploadDataCompleted);
					webClientEx.UploadDataAsync(new Uri(string.Format("{0}/ps4auth", Util.GetBaseUrl()), UriKind.Absolute), "POST", Encoding.ASCII.GetBytes(string.Format("{{\"action\":\"ACTIVATE_LICENSE\",\"license\":\"{0}\"}}", string.Format("{0}-{1}-{2}-{3}", new object[]
					{
						this.txtSerial1.Text,
						this.txtSerial2.Text,
						this.txtSerial3.Text,
						this.txtSerial4.Text
					}))));
				}
			}
			catch (Exception)
			{
				MessageBox.Show(Resources.errSerial);
				if (this.m_serial == null)
				{
					this.btnOk.Enabled = true;
				}
				this.btnCancel.Enabled = true;
			}
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0002A9B8 File Offset: 0x00028BB8
		private void client_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				this.btnOk.Invoke(this.EnableOk, new object[]
				{
					true
				});
				MessageBox.Show(Resources.errSerial);
				return;
			}
			string @string = Encoding.ASCII.GetString(e.Result);
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			Dictionary<string, object> dictionary = javaScriptSerializer.Deserialize(@string, typeof(Dictionary<string, object>)) as Dictionary<string, object>;
			if ((string)dictionary["status"] == "ERROR" && dictionary["code"].ToString() != "4020")
			{
				this.btnOk.Invoke(this.EnableOk, new object[]
				{
					true
				});
				this.label1.Invoke(this.UpdateStatus, new object[]
				{
					""
				});
				MessageBox.Show(string.Concat(new object[]
				{
					Resources.errSerial,
					" (",
					dictionary["code"],
					")"
				}));
				return;
			}
			Util.SetRegistryValue("User", (string)dictionary["userid"]);
			this.RegisterUID();
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0002AB14 File Offset: 0x00028D14
		private void RegisterUID()
		{
			string uID = Util.GetUID(false, false);
			WebClient webClient = new WebClient();
			webClient.Credentials = Util.GetNetworkCredential();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
			webClient.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
			webClient.UploadDataCompleted += new UploadDataCompletedEventHandler(this.client2_UploadDataCompleted);
			webClient.UploadDataAsync(new Uri(string.Format("{0}/ps4auth", Util.GetBaseUrl()), UriKind.Absolute), "POST", Encoding.ASCII.GetBytes(string.Format("{{\"action\":\"REGISTER_UUID\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}", Util.GetRegistryValue("User"), uID)));
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0002ABB0 File Offset: 0x00028DB0
		private void client2_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				this.btnOk.Invoke(this.EnableOk, new object[]
				{
					true
				});
				MessageBox.Show(Resources.errSerial);
				return;
			}
			string @string = Encoding.ASCII.GetString(e.Result);
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			Dictionary<string, object> dictionary = javaScriptSerializer.Deserialize(@string, typeof(Dictionary<string, object>)) as Dictionary<string, object>;
			if ((string)dictionary["status"] == "ERROR" && dictionary["code"].ToString() != "4021")
			{
				this.btnOk.Invoke(this.EnableOk, new object[]
				{
					true
				});
				this.label1.Invoke(this.UpdateStatus, new object[]
				{
					""
				});
				MessageBox.Show(Resources.errSerial + dictionary["code"]);
				return;
			}
			base.Invoke(this.CloseForm, new object[]
			{
				true
			});
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0002ACE8 File Offset: 0x00028EE8
		private void txtSerial_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
			{
				return;
			}
			TextBox textBox = sender as TextBox;
			if (textBox.Text.Length == 4)
			{
				e.SuppressKeyPress = true;
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0002AD28 File Offset: 0x00028F28
		private void txtSerial_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			if (textBox.Name == "txtSerial1")
			{
				return;
			}
			if (textBox.Text.Length == 0 && e.KeyChar == '\b')
			{
				Control[] array = textBox.Parent.Controls.Find("txtSerial" + (textBox.Name[9] - '\u0001'), true);
				if (array.Length == 1)
				{
					TextBox textBox2 = array[0] as TextBox;
					if (textBox2.Text.Length > 0)
					{
						textBox2.SelectionStart = textBox2.Text.Length;
					}
					array[0].Focus();
				}
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0002ADD0 File Offset: 0x00028FD0
		private void txtSerial_TextChanged(object sender, EventArgs e)
		{
			TextBox textBox = sender as TextBox;
			int selectionStart = textBox.SelectionStart;
			textBox.Text = Regex.Replace(textBox.Text, "[^0-9a-zA-Z ]", "");
			textBox.SelectionStart = selectionStart;
			if (textBox.Name == "txtSerial1")
			{
				string text = Clipboard.GetText();
				string[] array = text.Split(new char[]
				{
					'-'
				});
				if (array.Length == 4)
				{
					array[0] = array[0].Trim();
					array[1] = array[1].Trim();
					array[2] = array[2].Trim();
					array[3] = array[3].Trim();
					if (array[0].Length != 4 || array[1].Length != 4 || array[2].Length != 4 || array[3].Length != 4)
					{
						return;
					}
					Clipboard.Clear();
					this.txtSerial1.Text = array[0];
					this.txtSerial2.Text = array[1];
					this.txtSerial3.Text = array[2];
					this.txtSerial4.Text = array[3];
				}
			}
			if (textBox.Text.Length == 4)
			{
				Control[] array2 = textBox.Parent.Controls.Find("txtSerial" + (textBox.Name[9] + '\u0001'), true);
				if (array2.Length == 1)
				{
					array2[0].Focus();
				}
			}
			if (this.txtSerial1.Text.Length == 4 && this.txtSerial2.Text.Length == 4 && this.txtSerial3.Text.Length == 4 && this.txtSerial4.Text.Length == 4)
			{
				this.btnOk.Enabled = true;
				this.btnOk.Focus();
				return;
			}
			this.btnOk.Enabled = false;
		}

		// Token: 0x04000351 RID: 849
		public const string SERIAL_VALIDATE_URL = "{0}/ps4auth";

		// Token: 0x04000352 RID: 850
		public const string LICNESE_INFO = "{{\"action\":\"ACTIVATE_LICENSE\",\"license\":\"{0}\"}}";

		// Token: 0x04000353 RID: 851
		private const string REGISTER_UID = "{{\"action\":\"REGISTER_UUID\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}";

		// Token: 0x04000354 RID: 852
		private string m_serial;

		// Token: 0x04000355 RID: 853
		private string m_hash;

		// Token: 0x04000356 RID: 854
		private SerialValidateGG.CloseDelegate CloseForm;

		// Token: 0x04000357 RID: 855
		private SerialValidateGG.UpdateStatusDelegate UpdateStatus;

		// Token: 0x04000358 RID: 856
		private SerialValidateGG.EnableOkDelegate EnableOk;

		// Token: 0x04000359 RID: 857
		private int m_retryCount;

		// Token: 0x0200009D RID: 157
		// (Invoke) Token: 0x06000745 RID: 1861
		private delegate void CloseDelegate(bool bSuccess);

		// Token: 0x0200009E RID: 158
		// (Invoke) Token: 0x06000749 RID: 1865
		private delegate void UpdateStatusDelegate(string status);

		// Token: 0x0200009F RID: 159
		// (Invoke) Token: 0x0600074D RID: 1869
		private delegate void EnableOkDelegate(bool bEnable);

        private void SerialValidateGG_Load_1(object sender, EventArgs e)
        {

        }
    }
}
