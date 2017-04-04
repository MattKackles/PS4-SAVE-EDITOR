using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000132 RID: 306
	internal class TimeCriterion : SelectionCriterion
	{
		// Token: 0x06000C45 RID: 3141 RVA: 0x000430D4 File Offset: 0x000412D4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.Which.ToString()).Append(" ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ").Append(this.Time.ToString("yyyy-MM-dd-HH:mm:ss"));
			return stringBuilder.ToString();
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00043144 File Offset: 0x00041344
		internal override bool Evaluate(string filename)
		{
			DateTime x;
			switch (this.Which)
			{
			case WhichTime.atime:
				x = File.GetLastAccessTime(filename).ToUniversalTime();
				break;
			case WhichTime.mtime:
				x = File.GetLastWriteTime(filename).ToUniversalTime();
				break;
			case WhichTime.ctime:
				x = File.GetCreationTime(filename).ToUniversalTime();
				break;
			default:
				throw new ArgumentException("Operator");
			}
			return this._Evaluate(x);
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x000431B4 File Offset: 0x000413B4
		private bool _Evaluate(DateTime x)
		{
			bool result;
			switch (this.Operator)
			{
			case ComparisonOperator.GreaterThan:
				result = (x > this.Time);
				break;
			case ComparisonOperator.GreaterThanOrEqualTo:
				result = (x >= this.Time);
				break;
			case ComparisonOperator.LesserThan:
				result = (x < this.Time);
				break;
			case ComparisonOperator.LesserThanOrEqualTo:
				result = (x <= this.Time);
				break;
			case ComparisonOperator.EqualTo:
				result = (x == this.Time);
				break;
			case ComparisonOperator.NotEqualTo:
				result = (x != this.Time);
				break;
			default:
				throw new ArgumentException("Operator");
			}
			return result;
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00043250 File Offset: 0x00041450
		internal override bool Evaluate(ZipEntry entry)
		{
			DateTime x;
			switch (this.Which)
			{
			case WhichTime.atime:
				x = entry.AccessedTime;
				break;
			case WhichTime.mtime:
				x = entry.ModifiedTime;
				break;
			case WhichTime.ctime:
				x = entry.CreationTime;
				break;
			default:
				throw new ArgumentException("??time");
			}
			return this._Evaluate(x);
		}

		// Token: 0x0400066E RID: 1646
		internal ComparisonOperator Operator;

		// Token: 0x0400066F RID: 1647
		internal WhichTime Which;

		// Token: 0x04000670 RID: 1648
		internal DateTime Time;
	}
}
