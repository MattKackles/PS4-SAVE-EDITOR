using System;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace PS3SaveEditor
{
	// Token: 0x0200010F RID: 271
	internal class ZipUtil
	{
		// Token: 0x06000B5C RID: 2908 RVA: 0x0003F74C File Offset: 0x0003D94C
		public static string GetAsZipFile(string[] filePaths, ZipUtil.OnZipProgress onProgress)
		{
			string tempFileName = Path.GetTempFileName();
			File.Delete(tempFileName);
			ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(tempFileName));
			zipOutputStream.UseZip64 = UseZip64.Off;
			byte[] buffer = new byte[4096];
			for (int i = 0; i < filePaths.Length; i++)
			{
				string path = filePaths[i];
				FileStream fileStream = File.OpenRead(path);
				try
				{
					ZipEntry entry = new ZipEntry(Path.GetFileName(path));
					zipOutputStream.PutNextEntry(entry);
					if (onProgress != null)
					{
						StreamUtils.Copy(fileStream, zipOutputStream, buffer, delegate(object snder, ProgressEventArgs e)
						{
							onProgress((int)e.PercentComplete);
						}, TimeSpan.FromSeconds(1.0), null, "");
					}
					else
					{
						StreamUtils.Copy(fileStream, zipOutputStream, buffer);
					}
				}
				finally
				{
					fileStream.Close();
				}
			}
			zipOutputStream.Finish();
			zipOutputStream.Close();
			return tempFileName;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0003F85C File Offset: 0x0003DA5C
		public static string GetAsZipFile(string[] filePaths, string profile, ZipUtil.OnZipProgress onProgress)
		{
			string tempFileName = Path.GetTempFileName();
			File.Delete(tempFileName);
			ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(tempFileName));
			zipOutputStream.UseZip64 = UseZip64.Off;
			byte[] buffer = new byte[4096];
			int num = 0;
			for (int i = 0; i < filePaths.Length; i++)
			{
				string text = filePaths[i];
				if (File.Exists(text))
				{
					FileStream fileStream = File.OpenRead(text);
					string fileName = Path.GetFileName(text);
					try
					{
						if (fileName.ToUpper() == "PARAM.SFO" && profile != "None")
						{
							string tempFileName2 = Path.GetTempFileName();
							File.Delete(tempFileName2);
							File.Copy(text, tempFileName2);
							Util.UpdateProfileKey(tempFileName2, profile);
							Util.UpdatePSNId(tempFileName2, profile);
							fileStream.Close();
							fileStream = File.OpenRead(tempFileName2);
						}
						ZipEntry entry = new ZipEntry(fileName);
						zipOutputStream.PutNextEntry(entry);
						if (fileStream.Length > 1000000L)
						{
							StreamUtils.Copy(fileStream, zipOutputStream, buffer, delegate(object snder, ProgressEventArgs e)
							{
								onProgress((int)e.PercentComplete);
							}, TimeSpan.FromSeconds(1.0), null, "");
						}
						else
						{
							StreamUtils.Copy(fileStream, zipOutputStream, buffer);
						}
						onProgress(num * 100 / filePaths.Length);
					}
					finally
					{
						fileStream.Close();
						if (fileName.ToUpper() == "PARAM.SFO" && profile != "None")
						{
							File.SetAttributes(fileStream.Name, FileAttributes.Normal);
							File.Delete(fileStream.Name);
						}
					}
					num++;
				}
			}
			zipOutputStream.Finish();
			zipOutputStream.Close();
			return tempFileName;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0003FA18 File Offset: 0x0003DC18
		public static string GetPs3SeTempFolder()
		{
			string tempFolder = Util.GetTempFolder();
			if (!Directory.Exists(tempFolder))
			{
				Directory.CreateDirectory(tempFolder);
			}
			return tempFolder;
		}

		// Token: 0x02000110 RID: 272
		// (Invoke) Token: 0x06000B61 RID: 2913
		public delegate void OnZipProgress(int progress);
	}
}
