using System;

namespace PS3SaveEditor
{
	// Token: 0x02000028 RID: 40
	public class DownloadFinishEventArgs : EventArgs
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000FBEC File Offset: 0x0000DDEC
		public bool Status
		{
			get
			{
				return this.m_status;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000FBF4 File Offset: 0x0000DDF4
		public string Error
		{
			get
			{
				return this.m_error;
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000FBFC File Offset: 0x0000DDFC
		public DownloadFinishEventArgs(bool status, string error)
		{
			this.m_status = status;
			this.m_error = error;
		}

		// Token: 0x040000F9 RID: 249
		private bool m_status;

		// Token: 0x040000FA RID: 250
		private string m_error;
	}
}
