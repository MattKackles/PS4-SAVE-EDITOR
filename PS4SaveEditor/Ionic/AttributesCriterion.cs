using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000135 RID: 309
	internal class AttributesCriterion : SelectionCriterion
	{
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x0004353C File Offset: 0x0004173C
		// (set) Token: 0x06000C57 RID: 3159 RVA: 0x000435E0 File Offset: 0x000417E0
		internal string AttributeString
		{
			get
			{
				string text = "";
				if ((this._Attributes & FileAttributes.Hidden) != (FileAttributes)0)
				{
					text += "H";
				}
				if ((this._Attributes & FileAttributes.System) != (FileAttributes)0)
				{
					text += "S";
				}
				if ((this._Attributes & FileAttributes.ReadOnly) != (FileAttributes)0)
				{
					text += "R";
				}
				if ((this._Attributes & FileAttributes.Archive) != (FileAttributes)0)
				{
					text += "A";
				}
				if ((this._Attributes & FileAttributes.ReparsePoint) != (FileAttributes)0)
				{
					text += "L";
				}
				if ((this._Attributes & FileAttributes.NotContentIndexed) != (FileAttributes)0)
				{
					text += "I";
				}
				return text;
			}
			set
			{
				this._Attributes = FileAttributes.Normal;
				string text = value.ToUpper();
				for (int i = 0; i < text.Length; i++)
				{
					char c = text[i];
					char c2 = c;
					if (c2 != 'A')
					{
						switch (c2)
						{
						case 'H':
							if ((this._Attributes & FileAttributes.Hidden) != (FileAttributes)0)
							{
								throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
							}
							this._Attributes |= FileAttributes.Hidden;
							goto IL_1C1;
						case 'I':
							if ((this._Attributes & FileAttributes.NotContentIndexed) != (FileAttributes)0)
							{
								throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
							}
							this._Attributes |= FileAttributes.NotContentIndexed;
							goto IL_1C1;
						case 'J':
						case 'K':
							break;
						case 'L':
							if ((this._Attributes & FileAttributes.ReparsePoint) != (FileAttributes)0)
							{
								throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
							}
							this._Attributes |= FileAttributes.ReparsePoint;
							goto IL_1C1;
						default:
							switch (c2)
							{
							case 'R':
								if ((this._Attributes & FileAttributes.ReadOnly) != (FileAttributes)0)
								{
									throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
								}
								this._Attributes |= FileAttributes.ReadOnly;
								goto IL_1C1;
							case 'S':
								if ((this._Attributes & FileAttributes.System) != (FileAttributes)0)
								{
									throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
								}
								this._Attributes |= FileAttributes.System;
								goto IL_1C1;
							}
							break;
						}
						throw new ArgumentException(value);
					}
					if ((this._Attributes & FileAttributes.Archive) != (FileAttributes)0)
					{
						throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
					}
					this._Attributes |= FileAttributes.Archive;
					IL_1C1:;
				}
			}
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x000437C0 File Offset: 0x000419C0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("attributes ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ").Append(this.AttributeString);
			return stringBuilder.ToString();
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x00043810 File Offset: 0x00041A10
		private bool _EvaluateOne(FileAttributes fileAttrs, FileAttributes criterionAttrs)
		{
			return (this._Attributes & criterionAttrs) != criterionAttrs || (fileAttrs & criterionAttrs) == criterionAttrs;
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00043838 File Offset: 0x00041A38
		internal override bool Evaluate(string filename)
		{
			if (Directory.Exists(filename))
			{
				return this.Operator != ComparisonOperator.EqualTo;
			}
			FileAttributes attributes = File.GetAttributes(filename);
			return this._Evaluate(attributes);
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00043868 File Offset: 0x00041A68
		private bool _Evaluate(FileAttributes fileAttrs)
		{
			bool flag = this._EvaluateOne(fileAttrs, FileAttributes.Hidden);
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.System);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.ReadOnly);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.Archive);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.NotContentIndexed);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.ReparsePoint);
			}
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x000438D4 File Offset: 0x00041AD4
		internal override bool Evaluate(ZipEntry entry)
		{
			FileAttributes attributes = entry.Attributes;
			return this._Evaluate(attributes);
		}

		// Token: 0x04000677 RID: 1655
		private FileAttributes _Attributes;

		// Token: 0x04000678 RID: 1656
		internal ComparisonOperator Operator;
	}
}
