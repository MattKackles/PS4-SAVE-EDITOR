using System;
using System.IO;
using System.Text;

namespace Ionic.Zip
{
	// Token: 0x0200015A RID: 346
	public class ReadOptions
	{
		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x00056366 File Offset: 0x00054566
		// (set) Token: 0x06000EB2 RID: 3762 RVA: 0x0005636E File Offset: 0x0005456E
		public EventHandler<ReadProgressEventArgs> ReadProgress
		{
			get;
			set;
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x00056377 File Offset: 0x00054577
		// (set) Token: 0x06000EB4 RID: 3764 RVA: 0x0005637F File Offset: 0x0005457F
		public TextWriter StatusMessageWriter
		{
			get;
			set;
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x00056388 File Offset: 0x00054588
		// (set) Token: 0x06000EB6 RID: 3766 RVA: 0x00056390 File Offset: 0x00054590
		public Encoding Encoding
		{
			get;
			set;
		}
	}
}
