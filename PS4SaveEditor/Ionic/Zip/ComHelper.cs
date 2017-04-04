using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x02000111 RID: 273
	[ClassInterface(ClassInterfaceType.AutoDispatch), Guid("ebc25cf6-9120-4283-b972-0e5520d0000F"), ComVisible(true)]
	public class ComHelper
	{
		// Token: 0x06000B64 RID: 2916 RVA: 0x0003FA43 File Offset: 0x0003DC43
		public bool IsZipFile(string filename)
		{
			return ZipFile.IsZipFile(filename);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0003FA4B File Offset: 0x0003DC4B
		public bool IsZipFileWithExtract(string filename)
		{
			return ZipFile.IsZipFile(filename, true);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0003FA54 File Offset: 0x0003DC54
		public bool CheckZip(string filename)
		{
			return ZipFile.CheckZip(filename);
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0003FA5C File Offset: 0x0003DC5C
		public bool CheckZipPassword(string filename, string password)
		{
			return ZipFile.CheckZipPassword(filename, password);
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0003FA65 File Offset: 0x0003DC65
		public void FixZipDirectory(string filename)
		{
			ZipFile.FixZipDirectory(filename);
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0003FA6D File Offset: 0x0003DC6D
		public string GetZipLibraryVersion()
		{
			return ZipFile.LibraryVersion.ToString();
		}
	}
}
