using System;

namespace Ionic.Zip
{
	// Token: 0x02000125 RID: 293
	public class ZipErrorEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000C20 RID: 3104 RVA: 0x00042E7B File Offset: 0x0004107B
		private ZipErrorEventArgs()
		{
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00042E84 File Offset: 0x00041084
		internal static ZipErrorEventArgs Saving(string archiveName, ZipEntry entry, Exception exception)
		{
			return new ZipErrorEventArgs
			{
				EventType = ZipProgressEventType.Error_Saving,
				ArchiveName = archiveName,
				CurrentEntry = entry,
				_exc = exception
			};
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x00042EB7 File Offset: 0x000410B7
		public Exception Exception
		{
			get
			{
				return this._exc;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x00042EBF File Offset: 0x000410BF
		public string FileName
		{
			get
			{
				return base.CurrentEntry.LocalFileName;
			}
		}

		// Token: 0x04000655 RID: 1621
		private Exception _exc;
	}
}
