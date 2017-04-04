using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x02000128 RID: 296
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000A")]
	[Serializable]
	public class BadReadException : ZipException
	{
		// Token: 0x06000C2C RID: 3116 RVA: 0x00042F16 File Offset: 0x00041116
		public BadReadException()
		{
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00042F1E File Offset: 0x0004111E
		public BadReadException(string message) : base(message)
		{
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00042F27 File Offset: 0x00041127
		public BadReadException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00042F31 File Offset: 0x00041131
		protected BadReadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
