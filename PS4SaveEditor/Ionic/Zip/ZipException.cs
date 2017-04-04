using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x02000126 RID: 294
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00006")]
	[Serializable]
	public class ZipException : Exception
	{
		// Token: 0x06000C24 RID: 3108 RVA: 0x00042ECC File Offset: 0x000410CC
		public ZipException()
		{
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00042ED4 File Offset: 0x000410D4
		public ZipException(string message) : base(message)
		{
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00042EDD File Offset: 0x000410DD
		public ZipException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00042EE7 File Offset: 0x000410E7
		protected ZipException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
