using System;
using System.ComponentModel;

namespace Ionic
{
	// Token: 0x0200012F RID: 303
	internal enum ComparisonOperator
	{
		// Token: 0x04000665 RID: 1637
		[Description(">")]
		GreaterThan,
		// Token: 0x04000666 RID: 1638
		[Description(">=")]
		GreaterThanOrEqualTo,
		// Token: 0x04000667 RID: 1639
		[Description("<")]
		LesserThan,
		// Token: 0x04000668 RID: 1640
		[Description("<=")]
		LesserThanOrEqualTo,
		// Token: 0x04000669 RID: 1641
		[Description("=")]
		EqualTo,
		// Token: 0x0400066A RID: 1642
		[Description("!=")]
		NotEqualTo
	}
}
