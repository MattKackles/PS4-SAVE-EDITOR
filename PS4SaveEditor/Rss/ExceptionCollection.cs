using System;
using System.Collections;

namespace Rss
{
	// Token: 0x0200006F RID: 111
	[Serializable]
	public class ExceptionCollection : CollectionBase
	{
		// Token: 0x170001CA RID: 458
		public Exception this[int index]
		{
			get
			{
				return (Exception)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x000223D8 File Offset: 0x000205D8
		public int Add(Exception exception)
		{
			foreach (Exception ex in base.List)
			{
				if (ex.Message == exception.Message)
				{
					return -1;
				}
			}
			this.lastException = exception;
			return base.List.Add(exception);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00022458 File Offset: 0x00020658
		public bool Contains(Exception exception)
		{
			return base.List.Contains(exception);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00022466 File Offset: 0x00020666
		public void CopyTo(Exception[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00022475 File Offset: 0x00020675
		public int IndexOf(Exception exception)
		{
			return base.List.IndexOf(exception);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00022483 File Offset: 0x00020683
		public void Insert(int index, Exception exception)
		{
			base.List.Insert(index, exception);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00022492 File Offset: 0x00020692
		public void Remove(Exception exception)
		{
			base.List.Remove(exception);
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x000224A0 File Offset: 0x000206A0
		public Exception LastException
		{
			get
			{
				return this.lastException;
			}
		}

		// Token: 0x040002B8 RID: 696
		private Exception lastException;
	}
}
