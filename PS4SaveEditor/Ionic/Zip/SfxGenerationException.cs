using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x0200012A RID: 298
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00008")]
	[Serializable]
	public class SfxGenerationException : ZipException
	{
		// Token: 0x06000C33 RID: 3123 RVA: 0x00042F56 File Offset: 0x00041156
		public SfxGenerationException()
		{
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00042F5E File Offset: 0x0004115E
		public SfxGenerationException(string message) : base(message)
		{
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00042F67 File Offset: 0x00041167
		protected SfxGenerationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
