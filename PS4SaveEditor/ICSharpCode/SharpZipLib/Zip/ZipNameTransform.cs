using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000104 RID: 260
	public class ZipNameTransform : INameTransform
	{
		// Token: 0x06000AC2 RID: 2754 RVA: 0x00039C70 File Offset: 0x00037E70
		public ZipNameTransform()
		{
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00039C78 File Offset: 0x00037E78
		public ZipNameTransform(string trimPrefix)
		{
			this.TrimPrefix = trimPrefix;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00039C88 File Offset: 0x00037E88
		static ZipNameTransform()
		{
			char[] invalidPathChars = Path.GetInvalidPathChars();
			int num = invalidPathChars.Length + 2;
			ZipNameTransform.InvalidEntryCharsRelaxed = new char[num];
			Array.Copy(invalidPathChars, 0, ZipNameTransform.InvalidEntryCharsRelaxed, 0, invalidPathChars.Length);
			ZipNameTransform.InvalidEntryCharsRelaxed[num - 1] = '*';
			ZipNameTransform.InvalidEntryCharsRelaxed[num - 2] = '?';
			num = invalidPathChars.Length + 4;
			ZipNameTransform.InvalidEntryChars = new char[num];
			Array.Copy(invalidPathChars, 0, ZipNameTransform.InvalidEntryChars, 0, invalidPathChars.Length);
			ZipNameTransform.InvalidEntryChars[num - 1] = ':';
			ZipNameTransform.InvalidEntryChars[num - 2] = '\\';
			ZipNameTransform.InvalidEntryChars[num - 3] = '*';
			ZipNameTransform.InvalidEntryChars[num - 4] = '?';
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00039D1F File Offset: 0x00037F1F
		public string TransformDirectory(string name)
		{
			name = this.TransformFile(name);
			if (name.Length > 0)
			{
				if (!name.EndsWith("/"))
				{
					name += "/";
				}
				return name;
			}
			throw new ZipException("Cannot have an empty directory name");
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00039D5C File Offset: 0x00037F5C
		public string TransformFile(string name)
		{
			if (name != null)
			{
				string text = name.ToLower();
				if (this.trimPrefix_ != null && text.IndexOf(this.trimPrefix_) == 0)
				{
					name = name.Substring(this.trimPrefix_.Length);
				}
				name = name.Replace("\\", "/");
				name = WindowsPathUtils.DropPathRoot(name);
				while (name.Length > 0)
				{
					if (name[0] != '/')
					{
						break;
					}
					name = name.Remove(0, 1);
				}
				while (name.Length > 0 && name[name.Length - 1] == '/')
				{
					name = name.Remove(name.Length - 1, 1);
				}
				for (int i = name.IndexOf("//"); i >= 0; i = name.IndexOf("//"))
				{
					name = name.Remove(i, 1);
				}
				name = ZipNameTransform.MakeValidName(name, '_');
			}
			else
			{
				name = string.Empty;
			}
			return name;
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x00039E43 File Offset: 0x00038043
		// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x00039E4B File Offset: 0x0003804B
		public string TrimPrefix
		{
			get
			{
				return this.trimPrefix_;
			}
			set
			{
				this.trimPrefix_ = value;
				if (this.trimPrefix_ != null)
				{
					this.trimPrefix_ = this.trimPrefix_.ToLower();
				}
			}
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00039E70 File Offset: 0x00038070
		private static string MakeValidName(string name, char replacement)
		{
			int i = name.IndexOfAny(ZipNameTransform.InvalidEntryChars);
			if (i >= 0)
			{
				StringBuilder stringBuilder = new StringBuilder(name);
				while (i >= 0)
				{
					stringBuilder[i] = replacement;
					if (i >= name.Length)
					{
						i = -1;
					}
					else
					{
						i = name.IndexOfAny(ZipNameTransform.InvalidEntryChars, i + 1);
					}
				}
				name = stringBuilder.ToString();
			}
			if (name.Length > 65535)
			{
				throw new PathTooLongException();
			}
			return name;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00039EDC File Offset: 0x000380DC
		public static bool IsValidName(string name, bool relaxed)
		{
			bool flag = name != null;
			if (flag)
			{
				if (relaxed)
				{
					flag = (name.IndexOfAny(ZipNameTransform.InvalidEntryCharsRelaxed) < 0);
				}
				else
				{
					flag = (name.IndexOfAny(ZipNameTransform.InvalidEntryChars) < 0 && name.IndexOf('/') != 0);
				}
			}
			return flag;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x00039F2C File Offset: 0x0003812C
		public static bool IsValidName(string name)
		{
			return name != null && name.IndexOfAny(ZipNameTransform.InvalidEntryChars) < 0 && name.IndexOf('/') != 0;
		}

		// Token: 0x04000585 RID: 1413
		private string trimPrefix_;

		// Token: 0x04000586 RID: 1414
		private static readonly char[] InvalidEntryChars;

		// Token: 0x04000587 RID: 1415
		private static readonly char[] InvalidEntryCharsRelaxed;
	}
}
