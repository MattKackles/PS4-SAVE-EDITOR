using System;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x020000B6 RID: 182
	public abstract class WindowsPathUtils
	{
		// Token: 0x060007C6 RID: 1990 RVA: 0x0002D091 File Offset: 0x0002B291
		internal WindowsPathUtils()
		{
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0002D09C File Offset: 0x0002B29C
		public static string DropPathRoot(string path)
		{
			string text = path;
			if (path != null && path.Length > 0)
			{
				if (path[0] == '\\' || path[0] == '/')
				{
					if (path.Length > 1 && (path[1] == '\\' || path[1] == '/'))
					{
						int num = 2;
						int num2 = 2;
						while (num <= path.Length && ((path[num] != '\\' && path[num] != '/') || --num2 > 0))
						{
							num++;
						}
						num++;
						if (num < path.Length)
						{
							text = path.Substring(num);
						}
						else
						{
							text = "";
						}
					}
				}
				else if (path.Length > 1 && path[1] == ':')
				{
					int count = 2;
					if (path.Length > 2 && (path[2] == '\\' || path[2] == '/'))
					{
						count = 3;
					}
					text = text.Remove(0, count);
				}
			}
			return text;
		}
	}
}
