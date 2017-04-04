using System;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000136 RID: 310
	internal class CompoundCriterion : SelectionCriterion
	{
		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x000438F7 File Offset: 0x00041AF7
		// (set) Token: 0x06000C5F RID: 3167 RVA: 0x000438FF File Offset: 0x00041AFF
		internal SelectionCriterion Right
		{
			get
			{
				return this._Right;
			}
			set
			{
				this._Right = value;
				if (value == null)
				{
					this.Conjunction = LogicalConjunction.NONE;
					return;
				}
				if (this.Conjunction == LogicalConjunction.NONE)
				{
					this.Conjunction = LogicalConjunction.AND;
				}
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00043924 File Offset: 0x00041B24
		internal override bool Evaluate(string filename)
		{
			bool flag = this.Left.Evaluate(filename);
			switch (this.Conjunction)
			{
			case LogicalConjunction.AND:
				if (flag)
				{
					flag = this.Right.Evaluate(filename);
				}
				break;
			case LogicalConjunction.OR:
				if (!flag)
				{
					flag = this.Right.Evaluate(filename);
				}
				break;
			case LogicalConjunction.XOR:
				flag ^= this.Right.Evaluate(filename);
				break;
			default:
				throw new ArgumentException("Conjunction");
			}
			return flag;
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0004399C File Offset: 0x00041B9C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(").Append((this.Left != null) ? this.Left.ToString() : "null").Append(" ").Append(this.Conjunction.ToString()).Append(" ").Append((this.Right != null) ? this.Right.ToString() : "null").Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00043A34 File Offset: 0x00041C34
		internal override bool Evaluate(ZipEntry entry)
		{
			bool flag = this.Left.Evaluate(entry);
			switch (this.Conjunction)
			{
			case LogicalConjunction.AND:
				if (flag)
				{
					flag = this.Right.Evaluate(entry);
				}
				break;
			case LogicalConjunction.OR:
				if (!flag)
				{
					flag = this.Right.Evaluate(entry);
				}
				break;
			case LogicalConjunction.XOR:
				flag ^= this.Right.Evaluate(entry);
				break;
			}
			return flag;
		}

		// Token: 0x04000679 RID: 1657
		internal LogicalConjunction Conjunction;

		// Token: 0x0400067A RID: 1658
		internal SelectionCriterion Left;

		// Token: 0x0400067B RID: 1659
		private SelectionCriterion _Right;
	}
}
