using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x0200012B RID: 299
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00007")]
	[Serializable]
	public class BadStateException : ZipException
	{
		// Token: 0x06000C36 RID: 3126 RVA: 0x00042F71 File Offset: 0x00041171
		public BadStateException()
		{
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00042F79 File Offset: 0x00041179
		public BadStateException(string message) : base(message)
		{
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00042F82 File Offset: 0x00041182
		public BadStateException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00042F8C File Offset: 0x0004118C
		protected BadStateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
