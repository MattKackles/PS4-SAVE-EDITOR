using System;
using System.Runtime.Serialization;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000DF RID: 223
	[Serializable]
	public class ZipException : SharpZipBaseException
	{
		// Token: 0x0600095E RID: 2398 RVA: 0x000342AB File Offset: 0x000324AB
		protected ZipException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x000342B5 File Offset: 0x000324B5
		public ZipException()
		{
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x000342BD File Offset: 0x000324BD
		public ZipException(string message) : base(message)
		{
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x000342C6 File Offset: 0x000324C6
		public ZipException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
