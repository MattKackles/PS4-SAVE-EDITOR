using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000D4 RID: 212
	public class WindowsNameTransform : INameTransform
	{
		// Token: 0x060008F6 RID: 2294 RVA: 0x00032F3A File Offset: 0x0003113A
		public WindowsNameTransform(string baseDirectory)
		{
			if (baseDirectory == null)
			{
				throw new ArgumentNullException("baseDirectory", "Directory name is invalid");
			}
			this.BaseDirectory = baseDirectory;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00032F64 File Offset: 0x00031164
		public WindowsNameTransform()
		{
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x00032F74 File Offset: 0x00031174
		// (set) Token: 0x060008F9 RID: 2297 RVA: 0x00032F7C File Offset: 0x0003117C
		public string BaseDirectory
		{
			get
			{
				return this._baseDirectory;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._baseDirectory = Path.GetFullPath(value);
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x00032F98 File Offset: 0x00031198
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x00032FA0 File Offset: 0x000311A0
		public bool TrimIncomingPaths
		{
			get
			{
				return this._trimIncomingPaths;
			}
			set
			{
				this._trimIncomingPaths = value;
			}
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00032FAC File Offset: 0x000311AC
		public string TransformDirectory(string name)
		{
			name = this.TransformFile(name);
			if (name.Length > 0)
			{
				while (name.EndsWith("\\"))
				{
					name = name.Remove(name.Length - 1, 1);
				}
				return name;
			}
			throw new ZipException("Cannot have an empty directory name");
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00032FFC File Offset: 0x000311FC
		public string TransformFile(string name)
		{
			if (name != null)
			{
				name = WindowsNameTransform.MakeValidName(name, this._replacementChar);
				if (this._trimIncomingPaths)
				{
					name = Path.GetFileName(name);
				}
				if (this._baseDirectory != null)
				{
					name = Path.Combine(this._baseDirectory, name);
				}
			}
			else
			{
				name = string.Empty;
			}
			return name;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0003304C File Offset: 0x0003124C
		public static bool IsValidName(string name)
		{
			return name != null && name.Length <= 260 && string.Compare(name, WindowsNameTransform.MakeValidName(name, '_')) == 0;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00033080 File Offset: 0x00031280
		static WindowsNameTransform()
		{
			char[] invalidPathChars = Path.GetInvalidPathChars();
			int num = invalidPathChars.Length + 3;
			WindowsNameTransform.InvalidEntryChars = new char[num];
			Array.Copy(invalidPathChars, 0, WindowsNameTransform.InvalidEntryChars, 0, invalidPathChars.Length);
			WindowsNameTransform.InvalidEntryChars[num - 1] = '*';
			WindowsNameTransform.InvalidEntryChars[num - 2] = '?';
			WindowsNameTransform.InvalidEntryChars[num - 3] = ':';
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x000330D8 File Offset: 0x000312D8
		public static string MakeValidName(string name, char replacement)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			name = WindowsPathUtils.DropPathRoot(name.Replace("/", "\\"));
			while (name.Length > 0)
			{
				if (name[0] != '\\')
				{
					break;
				}
				name = name.Remove(0, 1);
			}
			while (name.Length > 0 && name[name.Length - 1] == '\\')
			{
				name = name.Remove(name.Length - 1, 1);
			}
			int i;
			for (i = name.IndexOf("\\\\"); i >= 0; i = name.IndexOf("\\\\"))
			{
				name = name.Remove(i, 1);
			}
			i = name.IndexOfAny(WindowsNameTransform.InvalidEntryChars);
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
						i = name.IndexOfAny(WindowsNameTransform.InvalidEntryChars, i + 1);
					}
				}
				name = stringBuilder.ToString();
			}
			if (name.Length > 260)
			{
				throw new PathTooLongException();
			}
			return name;
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x000331DD File Offset: 0x000313DD
		// (set) Token: 0x06000902 RID: 2306 RVA: 0x000331E8 File Offset: 0x000313E8
		public char Replacement
		{
			get
			{
				return this._replacementChar;
			}
			set
			{
				for (int i = 0; i < WindowsNameTransform.InvalidEntryChars.Length; i++)
				{
					if (WindowsNameTransform.InvalidEntryChars[i] == value)
					{
						throw new ArgumentException("invalid path character");
					}
				}
				if (value == '\\' || value == '/')
				{
					throw new ArgumentException("invalid replacement character");
				}
				this._replacementChar = value;
			}
		}

		// Token: 0x04000486 RID: 1158
		private const int MaxPath = 260;

		// Token: 0x04000487 RID: 1159
		private string _baseDirectory;

		// Token: 0x04000488 RID: 1160
		private bool _trimIncomingPaths;

		// Token: 0x04000489 RID: 1161
		private char _replacementChar = '_';

		// Token: 0x0400048A RID: 1162
		private static readonly char[] InvalidEntryChars;
	}
}
