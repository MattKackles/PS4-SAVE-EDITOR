using System;
using System.Runtime.Serialization;

namespace ICSharpCode.SharpZipLib
{
	// Token: 0x020000BE RID: 190
	[Serializable]
	public class SharpZipBaseException : ApplicationException
	{
		// Token: 0x060007F7 RID: 2039 RVA: 0x0002DAA0 File Offset: 0x0002BCA0
		protected SharpZipBaseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0002DAAA File Offset: 0x0002BCAA
		public SharpZipBaseException()
		{
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0002DAB2 File Offset: 0x0002BCB2
		public SharpZipBaseException(string message) : base(message)
		{
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0002DABB File Offset: 0x0002BCBB
		public SharpZipBaseException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
