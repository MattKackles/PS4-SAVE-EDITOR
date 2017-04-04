using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x02000129 RID: 297
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00009")]
	[Serializable]
	public class BadCrcException : ZipException
	{
		// Token: 0x06000C30 RID: 3120 RVA: 0x00042F3B File Offset: 0x0004113B
		public BadCrcException()
		{
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x00042F43 File Offset: 0x00041143
		public BadCrcException(string message) : base(message)
		{
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00042F4C File Offset: 0x0004114C
		protected BadCrcException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
