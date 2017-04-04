using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000134 RID: 308
	internal class TypeCriterion : SelectionCriterion
	{
		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x0004342E File Offset: 0x0004162E
		// (set) Token: 0x06000C51 RID: 3153 RVA: 0x0004343B File Offset: 0x0004163B
		internal string AttributeString
		{
			get
			{
				return this.ObjectType.ToString();
			}
			set
			{
				if (value.Length != 1 || (value[0] != 'D' && value[0] != 'F'))
				{
					throw new ArgumentException("Specify a single character: either D or F");
				}
				this.ObjectType = value[0];
			}
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00043474 File Offset: 0x00041674
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("type ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ").Append(this.AttributeString);
			return stringBuilder.ToString();
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x000434C4 File Offset: 0x000416C4
		internal override bool Evaluate(string filename)
		{
			bool flag = (this.ObjectType == 'D') ? Directory.Exists(filename) : File.Exists(filename);
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x000434FC File Offset: 0x000416FC
		internal override bool Evaluate(ZipEntry entry)
		{
			bool flag = (this.ObjectType == 'D') ? entry.IsDirectory : (!entry.IsDirectory);
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x04000675 RID: 1653
		private char ObjectType;

		// Token: 0x04000676 RID: 1654
		internal ComparisonOperator Operator;
	}
}
