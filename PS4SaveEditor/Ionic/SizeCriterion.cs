using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000131 RID: 305
	internal class SizeCriterion : SelectionCriterion
	{
		// Token: 0x06000C40 RID: 3136 RVA: 0x00042FB4 File Offset: 0x000411B4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("size ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ").Append(this.Size.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00043008 File Offset: 0x00041208
		internal override bool Evaluate(string filename)
		{
			FileInfo fileInfo = new FileInfo(filename);
			return this._Evaluate(fileInfo.Length);
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00043028 File Offset: 0x00041228
		private bool _Evaluate(long Length)
		{
			bool result;
			switch (this.Operator)
			{
			case ComparisonOperator.GreaterThan:
				result = (Length > this.Size);
				break;
			case ComparisonOperator.GreaterThanOrEqualTo:
				result = (Length >= this.Size);
				break;
			case ComparisonOperator.LesserThan:
				result = (Length < this.Size);
				break;
			case ComparisonOperator.LesserThanOrEqualTo:
				result = (Length <= this.Size);
				break;
			case ComparisonOperator.EqualTo:
				result = (Length == this.Size);
				break;
			case ComparisonOperator.NotEqualTo:
				result = (Length != this.Size);
				break;
			default:
				throw new ArgumentException("Operator");
			}
			return result;
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x000430BB File Offset: 0x000412BB
		internal override bool Evaluate(ZipEntry entry)
		{
			return this._Evaluate(entry.UncompressedSize);
		}

		// Token: 0x0400066C RID: 1644
		internal ComparisonOperator Operator;

		// Token: 0x0400066D RID: 1645
		internal long Size;
	}
}
