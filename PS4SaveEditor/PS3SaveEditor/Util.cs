using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Ionic.Zip;
using Microsoft.Win32;

namespace PS3SaveEditor
{
	// Token: 0x0200010D RID: 269
	internal class Util
	{
		// Token: 0x06000B39 RID: 2873
		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		// Token: 0x06000B3A RID: 2874 RVA: 0x0003EDA0 File Offset: 0x0003CFA0
		public static string GetBackupLocation()
		{
			string registryValue = Util.GetRegistryValue("Location");
			if (!string.IsNullOrEmpty(registryValue))
			{
				Directory.CreateDirectory(registryValue);
				return registryValue;
			}
			string text = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar + "PS4Saves_Backup";
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0003EDEC File Offset: 0x0003CFEC
		internal static string GetUserId()
		{
			return Util.GetRegistryValue("User");
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0003EDF8 File Offset: 0x0003CFF8
		internal static string GetRegistryValue(string key)
		{
			RegistryKey currentUser = Registry.CurrentUser;
			RegistryKey registryKey = currentUser.OpenSubKey(Util.GetRegistryBase());
			if (registryKey == null)
			{
				return null;
			}
			string result;
			try
			{
				string text = registryKey.GetValue(key) as string;
				registryKey.Close();
				currentUser.Close();
				result = text;
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0003EE50 File Offset: 0x0003D050
		internal static void DeleteRegistryValue(string key)
		{
			RegistryKey currentUser = Registry.CurrentUser;
			RegistryKey registryKey = currentUser.OpenSubKey(Util.GetRegistryBase(), true);
			if (registryKey == null)
			{
				return;
			}
			try
			{
				registryKey.DeleteValue(key);
			}
			catch
			{
			}
			registryKey.Close();
			currentUser.Close();
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0003EE9C File Offset: 0x0003D09C
		internal static bool IsMatch(string a, string pattern)
		{
			return Regex.IsMatch(a, "^" + pattern + "$");
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0003EEB4 File Offset: 0x0003D0B4
		internal static void SetRegistryValue(string key, string value)
		{
			RegistryKey currentUser = Registry.CurrentUser;
			RegistryKey registryKey = currentUser.CreateSubKey(Util.GetRegistryBase());
			if (registryKey == null)
			{
				return;
			}
			registryKey.SetValue(key, value);
			registryKey.Close();
			currentUser.Close();
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0003EEEB File Offset: 0x0003D0EB
		internal static string GetAdapterName(bool blackListed = false)
		{
			if (blackListed)
			{
				return null;
			}
			return Util.GetRegistryValue("Adapter");
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0003EEFC File Offset: 0x0003D0FC
		internal static string GetUID(bool blackListed = false, bool register = false)
		{
			string sysDrive = Environment.ExpandEnvironmentVariables("%SYSTEMDRIVE%");
			string uIDFromWMI = Util.GetUIDFromWMI(sysDrive);
			return uIDFromWMI.Substring(uIDFromWMI.IndexOf('{') + 1, uIDFromWMI.IndexOf('}') - uIDFromWMI.IndexOf('{') - 1);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0003EF46 File Offset: 0x0003D146
		internal static string GetSerial()
		{
			return Util.GetRegistryValue("Serial");
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0003EF52 File Offset: 0x0003D152
		private static void zipFile_ExtractProgress(object sender, ExtractProgressEventArgs e)
		{
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0003EF54 File Offset: 0x0003D154
		internal static void UpdatePSNId(string sfoFile, string profile)
		{
			if (File.Exists(sfoFile))
			{
				if (profile == null)
				{
					return;
				}
				byte[] array = File.ReadAllBytes(sfoFile);
				byte[] array2 = new byte[16];
				byte[] sourceArray = array2;
				int value;
				byte[] array3;
				byte[] profileKey = Util.GetProfileKey(profile, out value, out array3);
				if (profileKey == null || profileKey.Length != 16)
				{
					return;
				}
				if (array3 != null)
				{
					sourceArray = array3;
				}
				int num = BitConverter.ToInt32(array, 8);
				int num2 = BitConverter.ToInt32(array, 12);
				int num3 = BitConverter.ToInt32(array, 16);
				int num4 = 16;
				for (int i = 0; i < num3; i++)
				{
					short num5 = BitConverter.ToInt16(array, i * num4 + 20);
					int num6 = BitConverter.ToInt32(array, i * num4 + 12 + 20);
					if (Encoding.UTF8.GetString(array, num + (int)num5, 10) == "ACCOUNT_ID")
					{
						Array.Copy(sourceArray, 0, array, num2 + num6, 16);
					}
					if (Encoding.UTF8.GetString(array, num + (int)num5, 5) == "PARAM")
					{
						Array.Copy(profileKey, 0, array, num2 + num6 + 28, 16);
						byte[] bytes = BitConverter.GetBytes(value);
						Array.Copy(bytes, 0, array, num2 + num6 + 28 - 4, 4);
						Array.Copy(bytes, 0, array, num2 + num6 + 44, 4);
						Array.Copy(sourceArray, 0, array, num2 + num6 + 48, 16);
						File.SetAttributes(sfoFile, FileAttributes.Normal);
						File.WriteAllBytes(sfoFile, array);
						return;
					}
				}
			}
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0003F0AC File Offset: 0x0003D2AC
		internal static void UpdateProfileKey(string sfoFile, string profile)
		{
			if (File.Exists(sfoFile))
			{
				if (profile == null)
				{
					return;
				}
				byte[] array = File.ReadAllBytes(sfoFile);
				byte[] array2 = new byte[16];
				byte[] sourceArray = array2;
				int value;
				byte[] array3;
				byte[] profileKey = Util.GetProfileKey(profile, out value, out array3);
				if (profileKey == null || profileKey.Length != 16)
				{
					return;
				}
				int num = BitConverter.ToInt32(array, 8);
				int num2 = BitConverter.ToInt32(array, 12);
				int num3 = BitConverter.ToInt32(array, 16);
				int num4 = 16;
				for (int i = 0; i < num3; i++)
				{
					short num5 = BitConverter.ToInt16(array, i * num4 + 20);
					int num6 = BitConverter.ToInt32(array, i * num4 + 12 + 20);
					if (Encoding.UTF8.GetString(array, num + (int)num5, 10) == "ACCOUNT_ID")
					{
						Array.Copy(sourceArray, 0, array, num2 + num6, 16);
					}
					if (Encoding.UTF8.GetString(array, num + (int)num5, 5) == "PARAM")
					{
						Array.Copy(profileKey, 0, array, num2 + num6 + 28, 16);
						byte[] bytes = BitConverter.GetBytes(value);
						Array.Copy(bytes, 0, array, num2 + num6 + 28 - 4, 4);
						Array.Copy(bytes, 0, array, num2 + num6 + 44, 4);
						Array.Copy(sourceArray, 0, array, num2 + num6 + 48, 16);
						File.SetAttributes(sfoFile, FileAttributes.Normal);
						File.WriteAllBytes(sfoFile, array);
						return;
					}
				}
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0003F200 File Offset: 0x0003D400
		private static byte[] GetProfileKey(string profile, out int profileId, out byte[] psnId)
		{
			profileId = 0;
			psnId = null;
			byte[] result;
			using (RegistryKey currentUser = Registry.CurrentUser)
			{
				using (RegistryKey registryKey = currentUser.CreateSubKey(Util.GetRegistryBase() + "\\Profiles"))
				{
					string text = (string)registryKey.GetValue(profile);
					if (text == null)
					{
						result = null;
					}
					else
					{
						string[] array = text.Split(new char[]
						{
							':'
						});
						if (array.Length < 2)
						{
							result = null;
						}
						else
						{
							profileId = int.Parse(array[0]);
							if (array.Length == 3 && array[2].Length > 0)
							{
								psnId = Convert.FromBase64String(array[2]);
							}
							result = Convert.FromBase64String(array[1]);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0003F2CC File Offset: 0x0003D4CC
		internal static string GetHtaccessUser()
		{
			return Program.HTACCESS_USER;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0003F2D3 File Offset: 0x0003D4D3
		internal static string GetHtaccessPwd()
		{
			return Program.HTACCESS_PWD;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0003F2DA File Offset: 0x0003D4DA
		internal static NetworkCredential GetNetworkCredential()
		{
			return new NetworkCredential(Util.GetHtaccessUser(), Util.GetHtaccessPwd());
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0003F2EB File Offset: 0x0003D4EB
		internal static string GetBaseUrl()
		{
			return "http://seps4.cybergadget.net:8082";
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0003F2F2 File Offset: 0x0003D4F2
		internal static void SetAuthToken(string Token)
		{
			Util.SessionToken = Token;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0003F2FA File Offset: 0x0003D4FA
		internal static string GetAuthToken()
		{
			return Util.SessionToken;
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0003F304 File Offset: 0x0003D504
		internal static string GetUserAgent()
		{
			return "PS4SaveEditor " + AboutBox1.AssemblyVersion;
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0003F324 File Offset: 0x0003D524
		private static string GetUIDFromWMI(string sysDrive)
		{
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * \r\n                                     FROM   Win32_Volume\r\n                                     WHERE  DriveLetter = '" + sysDrive + "'");
			ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
			string text = null;
			using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectCollection.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)enumerator.Current;
					if (text != null)
					{
						break;
					}
					object propertyValue = managementObject.GetPropertyValue("DeviceID");
					if (propertyValue != null)
					{
						text = propertyValue.ToString();
					}
					text.Substring(text.IndexOf('{') + 1).TrimEnd(new char[]
					{
						'\\'
					}).TrimEnd(new char[]
					{
						'}'
					});
				}
			}
			return text;
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0003F3E8 File Offset: 0x0003D5E8
		internal static void ClearTemp()
		{
			try
			{
				string tempFolder = Util.GetTempFolder();
				string[] files = Directory.GetFiles(tempFolder);
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (text.IndexOf("Log.txt") <= 0)
					{
						File.Delete(text);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0003F444 File Offset: 0x0003D644
		internal static string GetTempFolder()
		{
			string text = Path.GetTempPath();
			string path = "SEPS4";
			text = Path.Combine(text, path);
			Directory.CreateDirectory(text);
			return text + "/";
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0003F478 File Offset: 0x0003D678
		internal static string GetRegistryBase()
		{
			return "SOFTWARE\\CYBER Gadget\\PS4SaveEditor";
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0003F48C File Offset: 0x0003D68C
		internal static bool IsTrialMode()
		{
			return false;
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0003F490 File Offset: 0x0003D690
		internal static string GetRegion(Dictionary<int, string> regionMap, int p, string exregions)
		{
			string text = "";
			foreach (int current in regionMap.Keys)
			{
				if ((p & current) > 0 && exregions.IndexOf(regionMap[current]) < 0)
				{
					text = text + "[" + regionMap[current] + "]";
				}
			}
			return text;
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0003F514 File Offset: 0x0003D714
		internal static byte[] GetPSNId(string saveFolder)
		{
			string sfoFile = Path.Combine(saveFolder, "PARAM.SFO");
			return Encoding.UTF8.GetBytes(MainForm.GetParamInfo(sfoFile, "ACCOUNT_ID"));
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0003F544 File Offset: 0x0003D744
		internal static bool GetCache(string hash)
		{
			try
			{
				WebClient webClient = new WebClient();
				webClient.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
				webClient.Credentials = Util.GetNetworkCredential();
				byte[] bytes = webClient.UploadData(Util.GetBaseUrl() + "/request_cache?token=" + Util.GetAuthToken(), Encoding.ASCII.GetBytes("{\"pfs\":\"" + hash + "\"}"));
				string @string = Encoding.ASCII.GetString(bytes);
				return @string.IndexOf("true") > 0;
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0003F5DC File Offset: 0x0003D7DC
		internal static string GetHash(byte[] buf)
		{
			MD5 mD = MD5.Create();
			byte[] value = mD.ComputeHash(buf);
			return BitConverter.ToString(value).Replace("-", "").ToLower();
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0003F614 File Offset: 0x0003D814
		internal static string GetHash(string file)
		{
			MD5 mD = MD5.Create();
			string result;
			using (FileStream fileStream = File.OpenRead(file))
			{
				byte[] value = mD.ComputeHash(fileStream);
				result = BitConverter.ToString(value).Replace("-", "").ToLower();
			}
			return result;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0003F670 File Offset: 0x0003D870
		internal static string GetCacheFolder(game game, string curCacheFolder)
		{
			string path = Path.Combine(Util.GetBackupLocation(), "cache");
			string text = Path.Combine(path, game.id);
			if (string.IsNullOrEmpty(curCacheFolder))
			{
				return text;
			}
			return Path.Combine(text, curCacheFolder);
		}

		// Token: 0x040005C6 RID: 1478
		private static string SessionToken;
	}
}
