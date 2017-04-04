using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x020000B1 RID: 177
	public class NameFilter : IScanFilter
	{
		// Token: 0x060007A3 RID: 1955 RVA: 0x0002C770 File Offset: 0x0002A970
		public NameFilter(string filter)
		{
			this.filter_ = filter;
			this.inclusions_ = new ArrayList();
			this.exclusions_ = new ArrayList();
			this.Compile();
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0002C79C File Offset: 0x0002A99C
		public static bool IsValidExpression(string expression)
		{
			bool result = true;
			try
			{
				new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			}
			catch (ArgumentException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0002C7CC File Offset: 0x0002A9CC
		public static bool IsValidFilterExpression(string toTest)
		{
			if (toTest == null)
			{
				throw new ArgumentNullException("toTest");
			}
			bool result = true;
			try
			{
				string[] array = NameFilter.SplitQuoted(toTest);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null && array[i].Length > 0)
					{
						string pattern;
						if (array[i][0] == '+')
						{
							pattern = array[i].Substring(1, array[i].Length - 1);
						}
						else if (array[i][0] == '-')
						{
							pattern = array[i].Substring(1, array[i].Length - 1);
						}
						else
						{
							pattern = array[i];
						}
						new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
					}
				}
			}
			catch (ArgumentException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0002C87C File Offset: 0x0002AA7C
		public static string[] SplitQuoted(string original)
		{
			char c = '\\';
			char[] array = new char[]
			{
				';'
			};
			ArrayList arrayList = new ArrayList();
			if (original != null && original.Length > 0)
			{
				int i = -1;
				StringBuilder stringBuilder = new StringBuilder();
				while (i < original.Length)
				{
					i++;
					if (i >= original.Length)
					{
						arrayList.Add(stringBuilder.ToString());
					}
					else if (original[i] == c)
					{
						i++;
						if (i >= original.Length)
						{
							throw new ArgumentException("Missing terminating escape character", "original");
						}
						if (Array.IndexOf<char>(array, original[i]) < 0)
						{
							stringBuilder.Append(c);
						}
						stringBuilder.Append(original[i]);
					}
					else if (Array.IndexOf<char>(array, original[i]) >= 0)
					{
						arrayList.Add(stringBuilder.ToString());
						stringBuilder.Length = 0;
					}
					else
					{
						stringBuilder.Append(original[i]);
					}
				}
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0002C98C File Offset: 0x0002AB8C
		public override string ToString()
		{
			return this.filter_;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0002C994 File Offset: 0x0002AB94
		public bool IsIncluded(string name)
		{
			bool result = false;
			if (this.inclusions_.Count == 0)
			{
				result = true;
			}
			else
			{
				foreach (Regex regex in this.inclusions_)
				{
					if (regex.IsMatch(name))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0002CA08 File Offset: 0x0002AC08
		public bool IsExcluded(string name)
		{
			bool result = false;
			foreach (Regex regex in this.exclusions_)
			{
				if (regex.IsMatch(name))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0002CA6C File Offset: 0x0002AC6C
		public bool IsMatch(string name)
		{
			return this.IsIncluded(name) && !this.IsExcluded(name);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0002CA84 File Offset: 0x0002AC84
		private void Compile()
		{
			if (this.filter_ == null)
			{
				return;
			}
			string[] array = NameFilter.SplitQuoted(this.filter_);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].Length > 0)
				{
					bool flag = array[i][0] != '-';
					string pattern;
					if (array[i][0] == '+')
					{
						pattern = array[i].Substring(1, array[i].Length - 1);
					}
					else if (array[i][0] == '-')
					{
						pattern = array[i].Substring(1, array[i].Length - 1);
					}
					else
					{
						pattern = array[i];
					}
					if (flag)
					{
						this.inclusions_.Add(new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline));
					}
					else
					{
						this.exclusions_.Add(new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline));
					}
				}
			}
		}

		// Token: 0x04000381 RID: 897
		private string filter_;

		// Token: 0x04000382 RID: 898
		private ArrayList inclusions_;

		// Token: 0x04000383 RID: 899
		private ArrayList exclusions_;
	}
}
