using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x02000127 RID: 295
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000B")]
	[Serializable]
	public class BadPasswordException : ZipException
	{
		// Token: 0x06000C28 RID: 3112 RVA: 0x00042EF1 File Offset: 0x000410F1
		public BadPasswordException()
		{
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00042EF9 File Offset: 0x000410F9
		public BadPasswordException(string message) : base(message)
		{
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00042F02 File Offset: 0x00041102
		public BadPasswordException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x00042F0C File Offset: 0x0004110C
		protected BadPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
