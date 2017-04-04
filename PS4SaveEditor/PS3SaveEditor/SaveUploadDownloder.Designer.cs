using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Core;
using Ionic.Zip;
using PS3SaveEditor.Resources;

namespace PS3SaveEditor
{
	// Token: 0x02000095 RID: 149
	public class SaveUploadDownloder : UserControl
	{
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x00026A27 File Offset: 0x00024C27
		// (set) Token: 0x060006D9 RID: 1753 RVA: 0x00026A2F File Offset: 0x00024C2F
		public ProgressBar ProgressBar
		{
			get;
			set;
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x00026A38 File Offset: 0x00024C38
		// (set) Token: 0x060006DB RID: 1755 RVA: 0x00026A40 File Offset: 0x00024C40
		public Label StatusLabel
		{
			get;
			set;
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x00026A49 File Offset: 0x00024C49
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x00026A51 File Offset: 0x00024C51
		public string Action
		{
			get;
			set;
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x00026A5A File Offset: 0x00024C5A
		// (set) Token: 0x060006DF RID: 1759 RVA: 0x00026A62 File Offset: 0x00024C62
		public game Game
		{
			get;
			set;
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x00026A6B File Offset: 0x00024C6B
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x00026A73 File Offset: 0x00024C73
		public bool IsUpload
		{
			get;
			set;
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00026A7C File Offset: 0x00024C7C
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x00026A84 File Offset: 0x00024C84
		public string FilePath
		{
			get;
			set;
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00026A8D File Offset: 0x00024C8D
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x00026A95 File Offset: 0x00024C95
		public string[] Files
		{
			get;
			set;
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x00026A9E File Offset: 0x00024C9E
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x00026AA6 File Offset: 0x00024CA6
		public string SaveId
		{
			get;
			set;
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x00026AAF File Offset: 0x00024CAF
		// (set) Token: 0x060006E9 RID: 1769 RVA: 0x00026AB7 File Offset: 0x00024CB7
		public string OutputFolder
		{
			get;
			set;
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x00026AC0 File Offset: 0x00024CC0
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x00026AC8 File Offset: 0x00024CC8
		public string ListResult
		{
			get;
			set;
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00026AD1 File Offset: 0x00024CD1
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x00026AD9 File Offset: 0x00024CD9
		public List<string> OrderedEntries
		{
			get;
			set;
		}

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x060006EE RID: 1774 RVA: 0x00026AE4 File Offset: 0x00024CE4
		// (remove) Token: 0x060006EF RID: 1775 RVA: 0x00026B1C File Offset: 0x00024D1C
		public event SaveUploadDownloder.DownloadStartEventHandler DownloadStart;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x060006F0 RID: 1776 RVA: 0x00026B54 File Offset: 0x00024D54
		// (remove) Token: 0x060006F1 RID: 1777 RVA: 0x00026B8C File Offset: 0x00024D8C
		public event SaveUploadDownloder.UploadStartEventHandler UploadStart;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060006F2 RID: 1778 RVA: 0x00026BC4 File Offset: 0x00024DC4
		// (remove) Token: 0x060006F3 RID: 1779 RVA: 0x00026BFC File Offset: 0x00024DFC
		public event SaveUploadDownloder.DownloadFinishEventHandler DownloadFinish;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x060006F4 RID: 1780 RVA: 0x00026C34 File Offset: 0x00024E34
		// (remove) Token: 0x060006F5 RID: 1781 RVA: 0x00026C6C File Offset: 0x00024E6C
		public event SaveUploadDownloder.UploadFinishEventHandler UploadFinish;

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00026CA1 File Offset: 0x00024EA1
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x00026CA9 File Offset: 0x00024EA9
		public string Profile
		{
			get;
			set;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00026CB4 File Offset: 0x00024EB4
		public SaveUploadDownloder()
		{
			this.InitializeComponent();
			this.BackColor = Color.FromArgb(0, 102, 102, 102);
			this.BackColor = Color.FromArgb(127, 204, 204, 204);
			this.lblStatus.BackColor = Color.Transparent;
			this.lblCurrentProgress.BackColor = Color.Transparent;
			this.lblTotalProgress.BackColor = Color.Transparent;
			this.lblStatus.ForeColor = Color.White;
			this.lblCurrentProgress.ForeColor = Color.White;
			this.lblTotalProgress.ForeColor = Color.White;
			this.UpdateProgress = new SaveUploadDownloder.UpdateProgressDelegate(this.UpdateProgressSafe);
			this.UpdateStatus = new SaveUploadDownloder.UpdateStatusDelegate(this.UpdateStatusSafe);
			this.ProgressBar = this.pbProgress;
			this.StatusLabel = this.lblStatus;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00026D98 File Offset: 0x00024F98
		private bool CheckCompressability()
		{
			string path = this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4);
			bool result;
			using (FileStream fileStream = File.OpenRead(path))
			{
				if (fileStream.Length < 2097152L)
				{
					result = false;
				}
				else
				{
					fileStream.Seek(1048576L, SeekOrigin.Begin);
					byte[] array = new byte[1048576];
					byte[] buffer = new byte[1048576];
					fileStream.Read(array, 0, 1048576);
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (MemoryStream memoryStream2 = new MemoryStream(array))
						{
							using (ZipOutputStream zipOutputStream = new ZipOutputStream(memoryStream))
							{
								zipOutputStream.PutNextEntry("temp");
								StreamUtils.Copy(memoryStream2, zipOutputStream, buffer);
								result = ((double)memoryStream.Length / (double)array.Length < 0.7);
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00026EC8 File Offset: 0x000250C8
		private bool BackupSaveData()
		{
			if (Util.GetRegistryValue("BackupSaves") == "false")
			{
				return this.CheckCompressability();
			}
			string path = null;
			if (this.Action == "resign")
			{
				path = this.OutputFolder;
			}
			else if (this.Game != null)
			{
				path = Path.GetDirectoryName(this.Game.LocalSaveFolder);
			}
			if (File.Exists(this.FilePath))
			{
				this.SetStatus("Backing up the save...");
				string text = string.Concat(new object[]
				{
					Util.GetBackupLocation(),
					Path.DirectorySeparatorChar,
					this.Game.PSN_ID,
					"_",
					Path.GetFileName(path),
					"_",
					Path.GetFileNameWithoutExtension(this.Game.LocalSaveFolder),
					"_",
					DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"),
					".bak"
				});
				this.SetProgress(0, new int?(30));
				string asZipFile = ZipUtil.GetAsZipFile(new string[]
				{
					this.Game.LocalSaveFolder,
					this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4)
				}, new ZipUtil.OnZipProgress(this.OnProgress));
				File.Copy(asZipFile, text, true);
				File.Delete(asZipFile);
				return (double)(new FileInfo(text).Length / new FileInfo(this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4)).Length) < 0.7;
			}
			return false;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0002707D File Offset: 0x0002527D
		protected virtual void RaiseDownloadFinishEvent(bool bSuccess, string error)
		{
			this.DownloadFinish(this, new DownloadFinishEventArgs(bSuccess, error));
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00027092 File Offset: 0x00025292
		protected virtual void RaiseUploadFinishEvent()
		{
			this.UploadFinish(this, new EventArgs());
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000270A5 File Offset: 0x000252A5
		protected virtual void RaiseUploadStartEvent()
		{
			this.UploadStart(this, new EventArgs());
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x000270B8 File Offset: 0x000252B8
		protected virtual void RaiseDownloadStartEvent()
		{
			this.DownloadStart(this, new EventArgs());
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x000270CB File Offset: 0x000252CB
		public void Start()
		{
			this.t = new Thread(new ThreadStart(this.UploadFile));
			this.t.Start();
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000270F0 File Offset: 0x000252F0
		public void SetStatus(string status)
		{
			if (base.IsHandleCreated)
			{
				this.lblStatus.Invoke(this.UpdateStatus, new object[]
				{
					status
				});
			}
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00027123 File Offset: 0x00025323
		private void UpdateStatusSafe(string status)
		{
			this.lblStatus.Text = status;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00027134 File Offset: 0x00025334
		private void SetProgress(int val, int? overall)
		{
			long arg_09_0 = this.start;
			if (base.IsHandleCreated)
			{
				this.pbProgress.Invoke(this.UpdateProgress, new object[]
				{
					val,
					overall
				});
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0002717F File Offset: 0x0002537F
		private void UpdateProgressSafe(int val, int? overall)
		{
			this.pbProgress.Value = val;
			if (overall.HasValue)
			{
				this.pbOverallProgress.Value = overall.Value;
			}
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x000271A8 File Offset: 0x000253A8
		public void OnProgress(int progress)
		{
			this.SetProgress(progress, null);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x000271FC File Offset: 0x000253FC
		public static void ErrorMessage(Form Parent, string errorMessage, string title = null)
		{
			if (Parent != null && Parent.InvokeRequired)
			{
				if (title != null)
				{
					Parent.Invoke(new Action(delegate
					{
						MessageBox.Show(Parent, errorMessage, title);
					}));
					return;
				}
				Parent.Invoke(new Action(delegate
				{
					MessageBox.Show(Parent, errorMessage);
				}));
				return;
			}
			else
			{
				if (title != null)
				{
					MessageBox.Show(Parent, errorMessage, title);
					return;
				}
				MessageBox.Show(Parent, errorMessage);
				return;
			}
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x000272B4 File Offset: 0x000254B4
		private void UploadFile()
		{
			this.SetStatus("Checking Session validity");
			this.SetStatus("Preparing data for upload...");
			if (this.Files != null)
			{
				this.SetProgress(0, new int?(0));
				if (this.Action == "decrypt" || this.Action == "patch")
				{
					long num = 0L;
					using (FileStream fileStream = File.OpenRead(this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4)))
					{
						using (HashAlgorithm hashAlgorithm = MD5.Create())
						{
							long length = fileStream.Length;
							byte[] array = new byte[4096];
							int num2 = fileStream.Read(array, 0, array.Length);
							num += (long)num2;
							do
							{
								int inputCount = num2;
								byte[] array2 = array;
								array = new byte[4096];
								num2 = fileStream.Read(array, 0, array.Length);
								num += (long)num2;
								if (num2 == 0)
								{
									hashAlgorithm.TransformFinalBlock(array2, 0, inputCount);
								}
								else
								{
									hashAlgorithm.TransformBlock(array2, 0, inputCount, array2, 0);
								}
								this.SetProgress((int)((double)num * 100.0 / (double)length), new int?(0));
							}
							while (num2 != 0);
							string text = BitConverter.ToString(hashAlgorithm.Hash).Replace("-", "").ToLower();
							List<string> list = new List<string>(this.Files);
							string text2;
							if (this.Action == "decrypt" || this.Action == "patch")
							{
								if (this.Action == "decrypt")
								{
									text2 = this.Game.ToString(new List<string>(), "decrypt");
								}
								else
								{
									text2 = this.Game.ToString(true, list);
								}
								text2 = text2.Replace("<name>" + Path.GetFileNameWithoutExtension(this.Game.LocalSaveFolder) + "</name>", string.Concat(new string[]
								{
									"<name>",
									Path.GetFileNameWithoutExtension(this.Game.LocalSaveFolder),
									"</name><md5>",
									text,
									"</md5>"
								}));
								if (list.IndexOf(this.Game.LocalSaveFolder) < 0)
								{
									list.Add(this.Game.LocalSaveFolder);
								}
							}
							else
							{
								text2 = this.Game.ToString(true, list);
								text2 = text2.Replace("<name>" + Path.GetFileNameWithoutExtension(this.Game.LocalSaveFolder) + "</name>", string.Concat(new string[]
								{
									"<name>",
									Path.GetFileNameWithoutExtension(this.Game.LocalSaveFolder),
									"</name><md5>",
									text,
									"</md5>"
								}));
								list = this.Game.GetContainerFiles();
							}
							string tempFolder = Util.GetTempFolder();
							string text3 = Path.Combine(tempFolder, "ps4_list.xml");
							File.WriteAllText(text3, text2);
							list.Add(text3);
							this.Files = list.ToArray();
						}
					}
					this.SetProgress(0, new int?(20));
				}
				this.FilePath = ZipUtil.GetAsZipFile(this.Files, this.Profile, new ZipUtil.OnZipProgress(this.OnProgress));
			}
			bool flag;
			if (this.Action == "patch" || this.Action == "encrypt")
			{
				flag = this.BackupSaveData();
			}
			flag = false;
			this.SetProgress(0, new int?(40));
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("form_id", "request_form");
			if (this.Game != null)
			{
				nameValueCollection.Add("gamecode", this.Game.id);
				if (!string.IsNullOrEmpty(this.Game.diskcode))
				{
					nameValueCollection.Add("diskcode", this.Game.diskcode);
				}
			}
			else if (!string.IsNullOrEmpty(this.OutputFolder) && this.Action != "download")
			{
				nameValueCollection.Add("gameid", Path.GetFileName(this.OutputFolder).Substring(0, 9));
			}
			if (this.SaveId != null)
			{
				nameValueCollection.Add("saveid", this.SaveId);
			}
			nameValueCollection.Add("action", this.Action);
			string text4 = this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4);
			if (this.Action == "decrypt" || this.Action == "patch")
			{
				if (flag)
				{
					this.SetStatus("Compressing the files...");
					this.SetProgress(0, new int?(50));
					text4 = ZipUtil.GetAsZipFile(new string[]
					{
						text4
					}, new ZipUtil.OnZipProgress(this.OnProgress));
				}
				this.RaiseUploadStartEvent();
				if (!this.UploadChunks(text4))
				{
					SaveUploadDownloder.ErrorMessage(base.ParentForm, Resources.errServer, null);
					return;
				}
			}
			this.HttpUploadFile(string.Format("{0}{1}", Util.GetBaseUrl(), string.Format(SaveUploadDownloder.UPLOAD_URL, Util.GetAuthToken())), this.FilePath, "files[input_zip_file]", "application/x-zip-compressed", nameValueCollection);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00027838 File Offset: 0x00025A38
		private bool CheckSession()
		{
			byte[] bytes = new WebClientEx
			{
				Credentials = Util.GetNetworkCredential()
			}.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.ASCII.GetBytes(string.Format("{{\"action\":\"SESSION_REFRESH\",\"userid\":\"{0}\",\"token\":\"{1}\"}}", Util.GetUserId(), Util.GetAuthToken())));
			string @string = Encoding.ASCII.GetString(bytes);
			return !@string.Contains("ERROR");
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x000278A4 File Offset: 0x00025AA4
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (this.t != null)
			{
				this.t.Abort();
			}
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x000278C0 File Offset: 0x00025AC0
		public bool UploadChunks_(string file)
		{
			int num = 8;
			int num2 = 1048576;
			string fileName = Path.GetFileName(file);
			string hash = Util.GetHash(file);
			string text = "---------------------------" + DateTime.Now.Ticks.ToString("x");
			byte[] bytes = Encoding.ASCII.GetBytes("\r\n--" + text + "\r\n");
			long length = new FileInfo(file).Length;
			int num3 = (int)Math.Ceiling((double)length / 1024.0 * 1024.0);
			List<int> remainingChunks = this.GetRemainingChunks(fileName, hash, text, length);
			if (remainingChunks != null && remainingChunks.Count == 0)
			{
				return true;
			}
			if (remainingChunks == null)
			{
				return false;
			}
			long num4 = (long)(remainingChunks.Count * num2);
			long num5 = 0L;
			using (FileStream fileStream = File.OpenRead(file))
			{
				for (int i = 0; i < remainingChunks.Count; i += num)
				{
					HttpWebRequest webRequest = this.GetWebRequest(text);
					string format = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
					Stream requestStream = webRequest.GetRequestStream();
					int j = i;
					int num6 = 1;
					string s;
					byte[] bytes2;
					while (j < Math.Min(remainingChunks.Count, i + num))
					{
						fileStream.Seek((long)(j * 1024 * 1024), SeekOrigin.Begin);
						requestStream.Write(bytes, 0, bytes.Length);
						int num7 = (int)Math.Min((long)num2, length - (long)(j * 1024 * 1024));
						byte[] array = new byte[num7];
						int num8 = fileStream.Read(array, 0, num2);
						string hash2 = Util.GetHash(array);
						s = string.Format(format, "chunk" + num6 + "_md5", hash2);
						bytes2 = Encoding.UTF8.GetBytes(s);
						requestStream.Write(bytes2, 0, bytes2.Length);
						requestStream.Write(bytes, 0, bytes.Length);
						s = string.Format(format, "chunk" + num6 + "id", string.Concat(remainingChunks[j]));
						bytes2 = Encoding.UTF8.GetBytes(s);
						requestStream.Write(bytes2, 0, bytes2.Length);
						string format2 = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
						string s2 = string.Format(format2, "files[chunk" + num6 + "]", "chunk" + remainingChunks[j], "application/octet-stream");
						requestStream.Write(bytes, 0, bytes.Length);
						byte[] bytes3 = Encoding.UTF8.GetBytes(s2);
						requestStream.Write(bytes3, 0, bytes3.Length);
						requestStream.Write(array, 0, num8);
						num5 += (long)num8;
						int val = (int)(num5 * 100L / num4);
						this.SetProgress(val, new int?(40));
						j++;
						num6++;
					}
					requestStream.Write(bytes, 0, bytes.Length);
					s = string.Format(format, "op", "Submit");
					bytes2 = Encoding.UTF8.GetBytes(s);
					requestStream.Write(bytes2, 0, bytes2.Length);
					requestStream.Write(bytes, 0, bytes.Length);
					s = string.Format(format, "form_id", "chunk_upload_form");
					bytes2 = Encoding.UTF8.GetBytes(s);
					requestStream.Write(bytes2, 0, bytes2.Length);
					requestStream.Write(bytes, 0, bytes.Length);
					s = string.Format(format, "pfs_md5", hash);
					bytes2 = Encoding.UTF8.GetBytes(s);
					requestStream.Write(bytes2, 0, bytes2.Length);
					requestStream.Write(bytes, 0, bytes.Length);
					s = string.Format(format, "total_chunks", num3);
					bytes2 = Encoding.UTF8.GetBytes(s);
					requestStream.Write(bytes2, 0, bytes2.Length);
					requestStream.Write(bytes, 0, bytes.Length);
					s = string.Format(format, "gamecode", this.Game.id);
					bytes2 = Encoding.UTF8.GetBytes(s);
					requestStream.Write(bytes2, 0, bytes2.Length);
					if (!string.IsNullOrEmpty(this.Game.diskcode))
					{
						requestStream.Write(bytes, 0, bytes.Length);
						s = string.Format(format, "diskcode", this.Game.diskcode);
						bytes2 = Encoding.UTF8.GetBytes(s);
						requestStream.Write(bytes2, 0, bytes2.Length);
					}
					requestStream.Write(bytes, 0, bytes.Length);
					s = string.Format(format, "pfs", fileName);
					bytes2 = Encoding.UTF8.GetBytes(s);
					requestStream.Write(bytes2, 0, bytes2.Length);
					requestStream.Write(bytes, 0, bytes.Length);
					s = string.Format(format, "pfs_size", length);
					bytes2 = Encoding.UTF8.GetBytes(s);
					requestStream.Write(bytes2, 0, bytes2.Length);
					byte[] bytes4 = Encoding.ASCII.GetBytes("\r\n--" + text + "--\r\n");
					requestStream.Write(bytes4, 0, bytes4.Length);
					requestStream.Close();
					HttpWebResponse httpWebResponse = webRequest.GetResponse() as HttpWebResponse;
					if (httpWebResponse.StatusCode == HttpStatusCode.OK)
					{
						using (Stream responseStream = httpWebResponse.GetResponseStream())
						{
							using (StreamReader streamReader = new StreamReader(responseStream))
							{
								long arg_51C_0 = httpWebResponse.ContentLength;
								string text2 = streamReader.ReadToEnd();
								if (text2.IndexOf("true") > 0)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00027E98 File Offset: 0x00026098
		private List<int> GetRemainingChunks(string pfsFileName, string pfsHash, string boundary, long fileSize)
		{
			try
			{
				List<int> list = new List<int>();
				int num = (int)Math.Ceiling((double)fileSize / 1024.0 * 1024.0);
				HttpWebRequest webRequest = this.GetWebRequest(boundary);
				Stream requestStream = webRequest.GetRequestStream();
				string format = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
				byte[] bytes = Encoding.ASCII.GetBytes(boundary);
				requestStream.Write(bytes, 0, bytes.Length);
				string s = string.Format(format, "op", "Submit");
				byte[] bytes2 = Encoding.UTF8.GetBytes(s);
				requestStream.Write(bytes2, 0, bytes2.Length);
				requestStream.Write(bytes, 0, bytes.Length);
				s = string.Format(format, "form_id", "chunk_upload_form");
				bytes2 = Encoding.UTF8.GetBytes(s);
				requestStream.Write(bytes2, 0, bytes2.Length);
				requestStream.Write(bytes, 0, bytes.Length);
				s = string.Format(format, "pfs_md5", pfsHash);
				bytes2 = Encoding.UTF8.GetBytes(s);
				requestStream.Write(bytes2, 0, bytes2.Length);
				requestStream.Write(bytes, 0, bytes.Length);
				s = string.Format(format, "total_chunks", num);
				bytes2 = Encoding.UTF8.GetBytes(s);
				requestStream.Write(bytes2, 0, bytes2.Length);
				requestStream.Write(bytes, 0, bytes.Length);
				s = string.Format(format, "gamecode", this.Game.id);
				bytes2 = Encoding.UTF8.GetBytes(s);
				requestStream.Write(bytes2, 0, bytes2.Length);
				if (!string.IsNullOrEmpty(this.Game.diskcode))
				{
					requestStream.Write(bytes, 0, bytes.Length);
					s = string.Format(format, "diskcode", this.Game.diskcode);
					bytes2 = Encoding.UTF8.GetBytes(s);
					requestStream.Write(bytes2, 0, bytes2.Length);
				}
				requestStream.Write(bytes, 0, bytes.Length);
				s = string.Format(format, "pfs", pfsFileName);
				bytes2 = Encoding.UTF8.GetBytes(s);
				requestStream.Write(bytes2, 0, bytes2.Length);
				requestStream.Write(bytes, 0, bytes.Length);
				s = string.Format(format, "pfs_size", fileSize);
				bytes2 = Encoding.UTF8.GetBytes(s);
				requestStream.Write(bytes2, 0, bytes2.Length);
				byte[] bytes3 = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
				requestStream.Write(bytes3, 0, bytes3.Length);
				requestStream.Close();
				HttpWebResponse httpWebResponse = webRequest.GetResponse() as HttpWebResponse;
				if (httpWebResponse.StatusCode == HttpStatusCode.OK)
				{
					using (Stream responseStream = httpWebResponse.GetResponseStream())
					{
						using (StreamReader streamReader = new StreamReader(responseStream))
						{
							long arg_29E_0 = httpWebResponse.ContentLength;
							string text = streamReader.ReadToEnd();
							if (text.IndexOf("true") > 0)
							{
								List<int> result = list;
								return result;
							}
							Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(text, typeof(Dictionary<string, object>)) as Dictionary<string, object>;
							if (dictionary.ContainsKey("remaining_chunks"))
							{
								Dictionary<string, object> dictionary2 = dictionary["remaining_chunks"] as Dictionary<string, object>;
								list = new List<int>();
								foreach (string current in dictionary2.Keys)
								{
									if (!(bool)dictionary2[current])
									{
										list.Add(int.Parse(current));
									}
								}
								List<int> result = list;
								return result;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				List<int> result = null;
				return result;
			}
			return null;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00028288 File Offset: 0x00026488
		private HttpWebRequest GetWebRequest(string boundary)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Util.GetBaseUrl() + "/chunk_upload?token=" + Util.GetAuthToken());
			httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
			httpWebRequest.AllowWriteStreamBuffering = true;
			httpWebRequest.PreAuthenticate = true;
			httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
			httpWebRequest.Method = "POST";
			httpWebRequest.UserAgent = Util.GetUserAgent();
			httpWebRequest.ProtocolVersion = new Version(1, 1);
			httpWebRequest.KeepAlive = true;
			ServicePointManager.Expect100Continue = false;
			httpWebRequest.Credentials = Util.GetNetworkCredential();
			httpWebRequest.Timeout = 600000;
			httpWebRequest.ReadWriteTimeout = 600000;
			httpWebRequest.SendChunked = true;
			string value = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
			httpWebRequest.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
			httpWebRequest.Headers.Add("Authorization", value);
			return httpWebRequest;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00028380 File Offset: 0x00026580
		public bool UploadChunks(string file)
		{
			int val = 8;
			int num = 1048576;
			string fileName = Path.GetFileName(file);
			string hash = Util.GetHash(file);
			List<int> list = new List<int>();
			string str = "---------------------------" + DateTime.Now.Ticks.ToString("x");
			byte[] bytes = Encoding.ASCII.GetBytes("\r\n--" + str + "\r\n");
			int num2 = 0;
			bool result;
			using (FileStream fileStream = File.Open(file, FileMode.Open))
			{
				int num3 = (int)Math.Ceiling((double)fileStream.Length / (double)num);
				long num4 = 0L;
				long length = fileStream.Length;
				long num5 = length;
				bool flag = true;
				this.SetProgress(0, new int?(60));
				while (true)
				{
					try
					{
						HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Util.GetBaseUrl() + "/chunk_upload?token=" + Util.GetAuthToken());
						httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
						httpWebRequest.AllowWriteStreamBuffering = true;
						httpWebRequest.PreAuthenticate = true;
						httpWebRequest.ContentType = "multipart/form-data; boundary=" + str;
						httpWebRequest.Method = "POST";
						httpWebRequest.UserAgent = Util.GetUserAgent();
						httpWebRequest.ProtocolVersion = new Version(1, 1);
						httpWebRequest.KeepAlive = true;
						ServicePointManager.Expect100Continue = false;
						httpWebRequest.Credentials = Util.GetNetworkCredential();
						httpWebRequest.Timeout = 600000;
						httpWebRequest.ReadWriteTimeout = 600000;
						httpWebRequest.SendChunked = true;
						string value = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
						httpWebRequest.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
						httpWebRequest.Headers.Add("Authorization", value);
						string format = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
						long num6 = 0L;
						num6 += (long)bytes.Length;
						string s = string.Format(format, "form_id", "chunk_upload_form");
						byte[] bytes2 = Encoding.UTF8.GetBytes(s);
						num6 += (long)bytes2.Length;
						num6 += (long)bytes.Length;
						s = string.Format(format, "op", "Submit");
						bytes2 = Encoding.UTF8.GetBytes(s);
						num6 += (long)bytes2.Length;
						num6 += (long)bytes.Length;
						s = string.Format(format, "pfs_md5", hash);
						bytes2 = Encoding.UTF8.GetBytes(s);
						num6 += (long)bytes2.Length;
						num6 += (long)bytes.Length;
						s = string.Format(format, "total_chunks", num3);
						bytes2 = Encoding.UTF8.GetBytes(s);
						num6 += (long)bytes2.Length;
						num6 += (long)bytes.Length;
						s = string.Format(format, "gamecode", this.Game.id);
						bytes2 = Encoding.UTF8.GetBytes(s);
						num6 += (long)bytes2.Length;
						if (!string.IsNullOrEmpty(this.Game.diskcode))
						{
							num6 += (long)bytes.Length;
							s = string.Format(format, "diskcode", this.Game.diskcode);
							bytes2 = Encoding.UTF8.GetBytes(s);
							num6 += (long)bytes2.Length;
						}
						num6 += (long)bytes.Length;
						s = string.Format(format, "pfs", fileName);
						bytes2 = Encoding.UTF8.GetBytes(s);
						num6 += (long)bytes2.Length;
						num6 += (long)bytes.Length;
						s = string.Format(format, "pfs_size", length);
						bytes2 = Encoding.UTF8.GetBytes(s);
						num6 += (long)bytes2.Length;
						Dictionary<int, byte[]> dictionary = new Dictionary<int, byte[]>();
						if (!flag)
						{
							int i = 0;
							int num7 = 1;
							while (i < Math.Min(list.Count, val))
							{
								byte[] array = new byte[num];
								fileStream.Seek((long)((list[i] - 1) * num), SeekOrigin.Begin);
								int num8 = fileStream.Read(array, 0, num);
								if (num8 < num)
								{
									byte[] array2 = new byte[num8];
									Array.Copy(array, array2, num8);
									array = array2;
								}
								dictionary.Add(list[i], array);
								string hash2 = Util.GetHash(array);
								num6 += (long)bytes.Length;
								s = string.Format(format, "chunk" + num7 + "_md5", hash2);
								bytes2 = Encoding.UTF8.GetBytes(s);
								num6 += (long)bytes2.Length;
								num6 += (long)bytes.Length;
								s = string.Format(format, "chunk" + num7 + "id", string.Concat(list[i]));
								bytes2 = Encoding.UTF8.GetBytes(s);
								num6 += (long)bytes2.Length;
								num6 += (long)bytes.Length;
								string format2 = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
								string s2 = string.Format(format2, "files[chunk" + num7 + "]", "chunk" + list[i], "application/octet-stream");
								byte[] bytes3 = Encoding.UTF8.GetBytes(s2);
								num6 += (long)bytes3.Length;
								num6 += (long)num8;
								i++;
								num7++;
							}
						}
						if (!flag && dictionary.Count == 0)
						{
							result = false;
							break;
						}
						byte[] bytes4 = Encoding.ASCII.GetBytes("\r\n--" + str + "--\r\n");
						num6 += (long)bytes4.Length;
						httpWebRequest.ContentLength = num6;
						Stream requestStream = httpWebRequest.GetRequestStream();
						format = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
						if (!flag)
						{
							int j = 0;
							int num9 = 1;
							while (j < Math.Min(list.Count, val))
							{
								requestStream.Write(bytes, 0, bytes.Length);
								byte[] array3 = dictionary[list[j]];
								int num10 = array3.Length;
								string hash3 = Util.GetHash(array3);
								s = string.Format(format, "chunk" + num9 + "_md5", hash3);
								bytes2 = Encoding.UTF8.GetBytes(s);
								requestStream.Write(bytes2, 0, bytes2.Length);
								requestStream.Write(bytes, 0, bytes.Length);
								s = string.Format(format, "chunk" + num9 + "id", string.Concat(list[j]));
								bytes2 = Encoding.UTF8.GetBytes(s);
								requestStream.Write(bytes2, 0, bytes2.Length);
								string format3 = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
								string s3 = string.Format(format3, "files[chunk" + num9 + "]", "chunk" + list[j], "application/octet-stream");
								requestStream.Write(bytes, 0, bytes.Length);
								byte[] bytes5 = Encoding.UTF8.GetBytes(s3);
								requestStream.Write(bytes5, 0, bytes5.Length);
								requestStream.Write(array3, 0, num10);
								num4 += (long)num10;
								int val2 = Math.Min(100, (int)(num4 * 100L / num5));
								this.SetProgress(val2, null);
								j++;
								num9++;
							}
						}
						requestStream.Write(bytes, 0, bytes.Length);
						s = string.Format(format, "op", "Submit");
						bytes2 = Encoding.UTF8.GetBytes(s);
						requestStream.Write(bytes2, 0, bytes2.Length);
						requestStream.Write(bytes, 0, bytes.Length);
						s = string.Format(format, "form_id", "chunk_upload_form");
						bytes2 = Encoding.UTF8.GetBytes(s);
						requestStream.Write(bytes2, 0, bytes2.Length);
						requestStream.Write(bytes, 0, bytes.Length);
						s = string.Format(format, "pfs_md5", hash);
						bytes2 = Encoding.UTF8.GetBytes(s);
						requestStream.Write(bytes2, 0, bytes2.Length);
						requestStream.Write(bytes, 0, bytes.Length);
						s = string.Format(format, "total_chunks", num3);
						bytes2 = Encoding.UTF8.GetBytes(s);
						requestStream.Write(bytes2, 0, bytes2.Length);
						requestStream.Write(bytes, 0, bytes.Length);
						s = string.Format(format, "gamecode", this.Game.id);
						bytes2 = Encoding.UTF8.GetBytes(s);
						requestStream.Write(bytes2, 0, bytes2.Length);
						if (!string.IsNullOrEmpty(this.Game.diskcode))
						{
							requestStream.Write(bytes, 0, bytes.Length);
							s = string.Format(format, "diskcode", this.Game.diskcode);
							bytes2 = Encoding.UTF8.GetBytes(s);
							requestStream.Write(bytes2, 0, bytes2.Length);
						}
						requestStream.Write(bytes, 0, bytes.Length);
						s = string.Format(format, "pfs", fileName);
						bytes2 = Encoding.UTF8.GetBytes(s);
						requestStream.Write(bytes2, 0, bytes2.Length);
						requestStream.Write(bytes, 0, bytes.Length);
						s = string.Format(format, "pfs_size", length);
						bytes2 = Encoding.UTF8.GetBytes(s);
						requestStream.Write(bytes2, 0, bytes2.Length);
						bytes4 = Encoding.ASCII.GetBytes("\r\n--" + str + "--\r\n");
						requestStream.Write(bytes4, 0, bytes4.Length);
						requestStream.Close();
						HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
						if (httpWebResponse.StatusCode == HttpStatusCode.OK)
						{
							using (Stream responseStream = httpWebResponse.GetResponseStream())
							{
								using (StreamReader streamReader = new StreamReader(responseStream))
								{
									long arg_96D_0 = httpWebResponse.ContentLength;
									string text = streamReader.ReadToEnd();
									if (text.IndexOf("true") > 0)
									{
										httpWebResponse.Close();
										requestStream.Dispose();
										result = true;
										break;
									}
									Dictionary<string, object> dictionary2 = new JavaScriptSerializer().Deserialize(text, typeof(Dictionary<string, object>)) as Dictionary<string, object>;
									if (dictionary2.ContainsKey("remaining_chunks"))
									{
										Dictionary<string, object> dictionary3 = dictionary2["remaining_chunks"] as Dictionary<string, object>;
										list = new List<int>();
										foreach (string current in dictionary3.Keys)
										{
											if (!(bool)dictionary3[current])
											{
												list.Add(int.Parse(current));
											}
										}
										if (list.Count == 0)
										{
											result = false;
											break;
										}
									}
								}
							}
							httpWebResponse.Close();
							requestStream.Dispose();
							if (flag)
							{
								num5 = (long)(list.Count * num);
								flag = false;
							}
						}
						else
						{
							num2++;
							if (num2 > 3)
							{
								result = false;
								break;
							}
						}
					}
					catch (Exception)
					{
						num2++;
						if (num2 > 3)
						{
							result = false;
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00028EC0 File Offset: 0x000270C0
		public void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
		{
			string error = "";
			string str = "---------------------------" + DateTime.Now.Ticks.ToString("x");
			byte[] bytes = Encoding.ASCII.GetBytes("\r\n--" + str + "\r\n");
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
			httpWebRequest.AllowWriteStreamBuffering = true;
			httpWebRequest.PreAuthenticate = true;
			httpWebRequest.ContentType = "multipart/form-data; boundary=" + str;
			httpWebRequest.Method = "POST";
			httpWebRequest.UserAgent = Util.GetUserAgent();
			httpWebRequest.ProtocolVersion = new Version(1, 1);
			httpWebRequest.KeepAlive = true;
			ServicePointManager.Expect100Continue = false;
			httpWebRequest.Credentials = Util.GetNetworkCredential();
			httpWebRequest.Timeout = 600000;
			httpWebRequest.ReadWriteTimeout = 600000;
			httpWebRequest.SendChunked = true;
			string value = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
			httpWebRequest.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
			httpWebRequest.Headers.Add("Authorization", value);
			string format = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
			long num = 0L;
			foreach (string text in nvc.Keys)
			{
				num += (long)bytes.Length;
				string s = string.Format(format, text, nvc[text]);
				byte[] bytes2 = Encoding.UTF8.GetBytes(s);
				num += (long)bytes2.Length;
			}
			num += (long)bytes.Length;
			string format2 = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
			string s2 = string.Format(format2, paramName, file, contentType);
			byte[] bytes3 = Encoding.UTF8.GetBytes(s2);
			num += (long)bytes3.Length;
			bool bSuccess = true;
			if (file != null)
			{
				num += new FileInfo(file).Length;
				byte[] bytes4 = Encoding.ASCII.GetBytes("\r\n--" + str + "--\r\n");
				num += (long)bytes4.Length;
				httpWebRequest.ContentLength = num;
				this.start = DateTime.Now.Ticks;
				try
				{
					Stream requestStream = httpWebRequest.GetRequestStream();
					format = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
					foreach (string text2 in nvc.Keys)
					{
						requestStream.Write(bytes, 0, bytes.Length);
						string s3 = string.Format(format, text2, nvc[text2]);
						byte[] bytes5 = Encoding.UTF8.GetBytes(s3);
						requestStream.Write(bytes5, 0, bytes5.Length);
					}
					requestStream.Write(bytes, 0, bytes.Length);
					format2 = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
					s2 = string.Format(format2, paramName, Path.GetFileName(file), contentType);
					bytes3 = Encoding.UTF8.GetBytes(s2);
					requestStream.Write(bytes3, 0, bytes3.Length);
					FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
					byte[] array = new byte[4096];
					long num2 = 0L;
					long length = fileStream.Length;
					int num3;
					while ((num3 = fileStream.Read(array, 0, array.Length)) != 0)
					{
						num2 += (long)num3;
						this.SetProgress((int)(num2 * 100L / length), new int?(80));
						requestStream.Write(array, 0, num3);
					}
					fileStream.Close();
					bytes4 = Encoding.ASCII.GetBytes("\r\n--" + str + "--\r\n");
					requestStream.Write(bytes4, 0, bytes4.Length);
					requestStream.Close();
					File.Delete(file);
					goto IL_452;
				}
				catch (Exception)
				{
					bSuccess = false;
					this.RaiseDownloadFinishEvent(bSuccess, Resources.errConnection);
					return;
				}
			}
			Stream requestStream2 = httpWebRequest.GetRequestStream();
			format = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
			foreach (string text3 in nvc.Keys)
			{
				requestStream2.Write(bytes, 0, bytes.Length);
				string s4 = string.Format(format, text3, nvc[text3]);
				byte[] bytes6 = Encoding.UTF8.GetBytes(s4);
				requestStream2.Write(bytes6, 0, bytes6.Length);
			}
			requestStream2.Write(bytes, 0, bytes.Length);
			requestStream2.Close();
			IL_452:
			this.RaiseUploadFinishEvent();
			this.SetProgress(0, new int?(80));
			WebResponse webResponse = null;
			try
			{
				webResponse = httpWebRequest.GetResponse();
				long arg_478_0 = webResponse.ContentLength;
				this.RaiseDownloadStartEvent();
				using (Stream responseStream = webResponse.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(responseStream))
					{
						string text4 = streamReader.ReadToEnd();
						if (this.Action == "list" && text4.IndexOf("[") == 0)
						{
							this.ListResult = text4;
						}
						else
						{
							try
							{
								Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(text4, typeof(Dictionary<string, object>)) as Dictionary<string, object>;
								if (dictionary != null && dictionary.ContainsKey("status") && (string)dictionary["status"] == "OK")
								{
									string zipFile = (string)dictionary["zip"];
									this.SetProgress(0, new int?(80));
									bSuccess = this.DownloadZip(zipFile, 0L, 0);
								}
								else
								{
									error = Resources.errServer + text4;
									bSuccess = false;
								}
							}
							catch (Exception)
							{
								error = Resources.errServer;
								bSuccess = false;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				if (webResponse != null)
				{
					webResponse.Close();
					webResponse = null;
				}
				bSuccess = false;
			}
			finally
			{
				httpWebRequest = null;
			}
			this.RaiseDownloadFinishEvent(bSuccess, error);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0002954C File Offset: 0x0002774C
		private bool DownloadZip(string zipFile, long start, int retry = 0)
		{
			long num = 0L;
			long num2 = start;
			bool result;
			try
			{
				HttpWebRequest httpWebRequest = WebRequest.Create(Util.GetBaseUrl() + zipFile) as HttpWebRequest;
				httpWebRequest.Method = "GET";
				httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
				httpWebRequest.PreAuthenticate = true;
				httpWebRequest.UserAgent = Util.GetUserAgent();
				httpWebRequest.Credentials = Util.GetNetworkCredential();
				httpWebRequest.Timeout = 300000;
				httpWebRequest.ReadWriteTimeout = 300000;
				string value = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
				httpWebRequest.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
				httpWebRequest.Headers.Add("Authorization", value);
				httpWebRequest.AddRange(start);
				WebResponse response = httpWebRequest.GetResponse();
				num = response.ContentLength;
				using (Stream responseStream = response.GetResponseStream())
				{
					byte[] array = new byte[4096];
					string tempFileName = Path.GetTempFileName();
					using (FileStream fileStream = File.OpenWrite(tempFileName))
					{
						while (true)
						{
							int num3 = responseStream.Read(array, 0, array.Length);
							if (num3 == 0)
							{
								break;
							}
							fileStream.Write(array, 0, num3);
							num2 += (long)num3;
							this.SetProgress((int)(num2 * 100L / num), null);
						}
					}
					this.SetProgress(0, new int?(90));
					string text = this.EnsureSpace();
					try
					{
						this.OrderedEntries = this.ExtractZip(tempFileName);
					}
					catch (Exception)
					{
						if (text != null)
						{
							File.Copy(text, this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4));
						}
					}
					if (text != null)
					{
						File.Delete(text);
					}
					result = true;
				}
			}
			catch (Exception)
			{
				if ((num > 0L && num2 == num) || retry > 3)
				{
					result = false;
				}
				else
				{
					result = this.DownloadZip(zipFile, num2, retry++);
				}
			}
			return result;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002978C File Offset: 0x0002798C
		private string EnsureSpace()
		{
			if (this.Action == "decrypt")
			{
				return null;
			}
			DriveInfo driveInfo = new DriveInfo(Path.GetPathRoot(this.Game.LocalSaveFolder));
			string text = this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4);
			if (driveInfo.AvailableFreeSpace > new FileInfo(text).Length)
			{
				return null;
			}
			string text2 = Path.GetTempFileName();
			if (Util.GetRegistryValue("BackupSaves") == "false")
			{
				File.Copy(text, text2, true);
			}
			else
			{
				text2 = null;
			}
			File.Delete(text);
			return text2;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0002982C File Offset: 0x00027A2C
		private List<string> ExtractZip(string tempFile)
		{
			List<string> list = new List<string>();
			ZipFile zipFile = new ZipFile(tempFile);
			foreach (ZipEntry current in zipFile.Entries)
			{
				list.Add(current.FileName);
			}
			zipFile.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(this.zipFile_ExtractProgress);
			zipFile.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
			zipFile.ExtractAll(this.OutputFolder);
			list.Reverse();
			return list;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x000298B8 File Offset: 0x00027AB8
		private void zipFile_ExtractProgress(object sender, ExtractProgressEventArgs e)
		{
			if (e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
			{
				this.SetProgress((int)(e.BytesTransferred * 100L / e.TotalBytesToTransfer), null);
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x000298F0 File Offset: 0x00027AF0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00029910 File Offset: 0x00027B10
		private void InitializeComponent()
		{
			this.lblStatus = new Label();
			this.lblCurrentProgress = new Label();
			this.lblTotalProgress = new Label();
			this.pbOverallProgress = new PS4ProgressBar();
			this.pbProgress = new PS4ProgressBar();
			base.SuspendLayout();
			this.lblStatus.AutoSize = true;
			this.lblStatus.ForeColor = Color.White;
			this.lblStatus.Location = new Point(11, 10);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new Size(28, 13);
			this.lblStatus.TabIndex = 0;
			this.lblStatus.Text = "Text";
			this.lblCurrentProgress.AutoSize = true;
			this.lblCurrentProgress.ForeColor = Color.White;
			this.lblCurrentProgress.Location = new Point(11, 30);
			this.lblCurrentProgress.Name = "lblCurrentProgress";
			this.lblCurrentProgress.Size = new Size(85, 13);
			this.lblCurrentProgress.TabIndex = 3;
			this.lblCurrentProgress.Text = "Current Progress";
			this.lblTotalProgress.AutoSize = true;
			this.lblTotalProgress.ForeColor = Color.White;
			this.lblTotalProgress.Location = new Point(11, 91);
			this.lblTotalProgress.Name = "lblTotalProgress";
			this.lblTotalProgress.Size = new Size(75, 13);
			this.lblTotalProgress.TabIndex = 4;
			this.lblTotalProgress.Text = "Total Progress";
			this.pbOverallProgress.Location = new Point(11, 113);
			this.pbOverallProgress.Name = "pbOverallProgress";
			this.pbOverallProgress.Size = new Size(424, 23);
			this.pbOverallProgress.TabIndex = 2;
			this.pbProgress.Location = new Point(11, 52);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new Size(424, 23);
			this.pbProgress.TabIndex = 1;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.FromArgb(102, 102, 102);
			base.Controls.Add(this.lblTotalProgress);
			base.Controls.Add(this.lblCurrentProgress);
			base.Controls.Add(this.pbOverallProgress);
			base.Controls.Add(this.pbProgress);
			base.Controls.Add(this.lblStatus);
			base.Name = "SaveUploadDownloder";
			base.Size = new Size(446, 151);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000335 RID: 821
		private const string SESSION_CHECK_URL = "{{\"action\":\"SESSION_REFRESH\",\"userid\":\"{0}\",\"token\":\"{1}\"}}";

		// Token: 0x04000336 RID: 822
		private SaveUploadDownloder.UpdateProgressDelegate UpdateProgress;

		// Token: 0x04000337 RID: 823
		private SaveUploadDownloder.UpdateStatusDelegate UpdateStatus;

		// Token: 0x04000338 RID: 824
		private static string UPLOAD_URL = "/request?token={0}";

		// Token: 0x0400033D RID: 829
		private Thread t;

		// Token: 0x0400033E RID: 830
		private long start;

		// Token: 0x0400033F RID: 831
		private IContainer components;

		// Token: 0x04000340 RID: 832
		private Label lblStatus;

		// Token: 0x04000341 RID: 833
		private PS4ProgressBar pbProgress;

		// Token: 0x04000342 RID: 834
		private PS4ProgressBar pbOverallProgress;

		// Token: 0x04000343 RID: 835
		private Label lblCurrentProgress;

		// Token: 0x04000344 RID: 836
		private Label lblTotalProgress;

		// Token: 0x02000096 RID: 150
		// (Invoke) Token: 0x06000716 RID: 1814
		private delegate void UpdateProgressDelegate(int value, int? overall);

		// Token: 0x02000097 RID: 151
		// (Invoke) Token: 0x0600071A RID: 1818
		private delegate void UpdateStatusDelegate(string status);

		// Token: 0x02000098 RID: 152
		// (Invoke) Token: 0x0600071E RID: 1822
		public delegate void DownloadStartEventHandler(object sender, EventArgs e);

		// Token: 0x02000099 RID: 153
		// (Invoke) Token: 0x06000722 RID: 1826
		public delegate void UploadStartEventHandler(object sender, EventArgs e);

		// Token: 0x0200009A RID: 154
		// (Invoke) Token: 0x06000726 RID: 1830
		public delegate void DownloadFinishEventHandler(object sender, DownloadFinishEventArgs e);

		// Token: 0x0200009B RID: 155
		// (Invoke) Token: 0x0600072A RID: 1834
		public delegate void UploadFinishEventHandler(object sender, EventArgs e);
	}
}
