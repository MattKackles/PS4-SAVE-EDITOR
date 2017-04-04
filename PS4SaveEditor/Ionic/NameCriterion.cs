using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000133 RID: 307
	internal class NameCriterion : SelectionCriterion
	{
		// Token: 0x1700032F RID: 815
		// (set) Token: 0x06000C4A RID: 3146 RVA: 0x000432B0 File Offset: 0x000414B0
		internal virtual string MatchingFileSpec
		{
			set
			{
				if (Directory.Exists(value))
				{
					this._MatchingFileSpec = ".\\" + value + "\\*.*";
				}
				else
				{
					this._MatchingFileSpec = value;
				}
				this._regexString = "^" + Regex.Escape(this._MatchingFileSpec).Replace("\\\\\\*\\.\\*", "\\\\([^\\.]+|.*\\.[^\\\\\\.]*)").Replace("\\.\\*", "\\.[^\\\\\\.]*").Replace("\\*", ".*").Replace("\\?", "[^\\\\\\.]") + "$";
				this._re = new Regex(this._regexString, RegexOptions.IgnoreCase);
			}
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00043354 File Offset: 0x00041554
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("name ").Append(EnumUtil.GetDescription(this.Operator)).Append(" '").Append(this._MatchingFileSpec).Append("'");
			return stringBuilder.ToString();
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x000433AD File Offset: 0x000415AD
		internal override bool Evaluate(string filename)
		{
			return this._Evaluate(filename);
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x000433B8 File Offset: 0x000415B8
		private bool _Evaluate(string fullpath)
		{
			string input = (this._MatchingFileSpec.IndexOf('\\') == -1) ? Path.GetFileName(fullpath) : fullpath;
			bool flag = this._re.IsMatch(input);
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x000433FC File Offset: 0x000415FC
		internal override bool Evaluate(ZipEntry entry)
		{
			string fullpath = entry.FileName.Replace("/", "\\");
			return this._Evaluate(fullpath);
		}

		// Token: 0x04000671 RID: 1649
		private Regex _re;

		// Token: 0x04000672 RID: 1650
		private string _regexString;

		// Token: 0x04000673 RID: 1651
		internal ComparisonOperator Operator;

		// Token: 0x04000674 RID: 1652
		private string _MatchingFileSpec;
	}
}
