using System;
using System.Text;
using System.Threading;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000D9 RID: 217
	public sealed class ZipConstants
	{
		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x00033238 File Offset: 0x00031438
		// (set) Token: 0x06000904 RID: 2308 RVA: 0x0003323F File Offset: 0x0003143F
		public static int DefaultCodePage
		{
			get
			{
				return ZipConstants.defaultCodePage;
			}
			set
			{
				ZipConstants.defaultCodePage = value;
			}
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00033247 File Offset: 0x00031447
		public static string ConvertToString(byte[] data, int count)
		{
			if (data == null)
			{
				return string.Empty;
			}
			return Encoding.GetEncoding(ZipConstants.DefaultCodePage).GetString(data, 0, count);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00033264 File Offset: 0x00031464
		public static string ConvertToString(byte[] data)
		{
			if (data == null)
			{
				return string.Empty;
			}
			return ZipConstants.ConvertToString(data, data.Length);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00033278 File Offset: 0x00031478
		public static string ConvertToStringExt(int flags, byte[] data, int count)
		{
			if (data == null)
			{
				return string.Empty;
			}
			if ((flags & 2048) != 0)
			{
				return Encoding.UTF8.GetString(data, 0, count);
			}
			return ZipConstants.ConvertToString(data, count);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x000332A1 File Offset: 0x000314A1
		public static string ConvertToStringExt(int flags, byte[] data)
		{
			if (data == null)
			{
				return string.Empty;
			}
			if ((flags & 2048) != 0)
			{
				return Encoding.UTF8.GetString(data, 0, data.Length);
			}
			return ZipConstants.ConvertToString(data, data.Length);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x000332CE File Offset: 0x000314CE
		public static byte[] ConvertToArray(string str)
		{
			if (str == null)
			{
				return new byte[0];
			}
			return Encoding.GetEncoding(ZipConstants.DefaultCodePage).GetBytes(str);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x000332EA File Offset: 0x000314EA
		public static byte[] ConvertToArray(int flags, string str)
		{
			if (str == null)
			{
				return new byte[0];
			}
			if ((flags & 2048) != 0)
			{
				return Encoding.UTF8.GetBytes(str);
			}
			return ZipConstants.ConvertToArray(str);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00033311 File Offset: 0x00031511
		private ZipConstants()
		{
		}

		// Token: 0x040004B4 RID: 1204
		public const int VersionMadeBy = 51;

		// Token: 0x040004B5 RID: 1205
		[Obsolete("Use VersionMadeBy instead")]
		public const int VERSION_MADE_BY = 51;

		// Token: 0x040004B6 RID: 1206
		public const int VersionStrongEncryption = 50;

		// Token: 0x040004B7 RID: 1207
		[Obsolete("Use VersionStrongEncryption instead")]
		public const int VERSION_STRONG_ENCRYPTION = 50;

		// Token: 0x040004B8 RID: 1208
		public const int VERSION_AES = 51;

		// Token: 0x040004B9 RID: 1209
		public const int VersionZip64 = 45;

		// Token: 0x040004BA RID: 1210
		public const int LocalHeaderBaseSize = 30;

		// Token: 0x040004BB RID: 1211
		[Obsolete("Use LocalHeaderBaseSize instead")]
		public const int LOCHDR = 30;

		// Token: 0x040004BC RID: 1212
		public const int Zip64DataDescriptorSize = 24;

		// Token: 0x040004BD RID: 1213
		public const int DataDescriptorSize = 16;

		// Token: 0x040004BE RID: 1214
		[Obsolete("Use DataDescriptorSize instead")]
		public const int EXTHDR = 16;

		// Token: 0x040004BF RID: 1215
		public const int CentralHeaderBaseSize = 46;

		// Token: 0x040004C0 RID: 1216
		[Obsolete("Use CentralHeaderBaseSize instead")]
		public const int CENHDR = 46;

		// Token: 0x040004C1 RID: 1217
		public const int EndOfCentralRecordBaseSize = 22;

		// Token: 0x040004C2 RID: 1218
		[Obsolete("Use EndOfCentralRecordBaseSize instead")]
		public const int ENDHDR = 22;

		// Token: 0x040004C3 RID: 1219
		public const int CryptoHeaderSize = 12;

		// Token: 0x040004C4 RID: 1220
		[Obsolete("Use CryptoHeaderSize instead")]
		public const int CRYPTO_HEADER_SIZE = 12;

		// Token: 0x040004C5 RID: 1221
		public const int LocalHeaderSignature = 67324752;

		// Token: 0x040004C6 RID: 1222
		[Obsolete("Use LocalHeaderSignature instead")]
		public const int LOCSIG = 67324752;

		// Token: 0x040004C7 RID: 1223
		public const int SpanningSignature = 134695760;

		// Token: 0x040004C8 RID: 1224
		[Obsolete("Use SpanningSignature instead")]
		public const int SPANNINGSIG = 134695760;

		// Token: 0x040004C9 RID: 1225
		public const int SpanningTempSignature = 808471376;

		// Token: 0x040004CA RID: 1226
		[Obsolete("Use SpanningTempSignature instead")]
		public const int SPANTEMPSIG = 808471376;

		// Token: 0x040004CB RID: 1227
		public const int DataDescriptorSignature = 134695760;

		// Token: 0x040004CC RID: 1228
		[Obsolete("Use DataDescriptorSignature instead")]
		public const int EXTSIG = 134695760;

		// Token: 0x040004CD RID: 1229
		[Obsolete("Use CentralHeaderSignature instead")]
		public const int CENSIG = 33639248;

		// Token: 0x040004CE RID: 1230
		public const int CentralHeaderSignature = 33639248;

		// Token: 0x040004CF RID: 1231
		public const int Zip64CentralFileHeaderSignature = 101075792;

		// Token: 0x040004D0 RID: 1232
		[Obsolete("Use Zip64CentralFileHeaderSignature instead")]
		public const int CENSIG64 = 101075792;

		// Token: 0x040004D1 RID: 1233
		public const int Zip64CentralDirLocatorSignature = 117853008;

		// Token: 0x040004D2 RID: 1234
		public const int ArchiveExtraDataSignature = 117853008;

		// Token: 0x040004D3 RID: 1235
		public const int CentralHeaderDigitalSignature = 84233040;

		// Token: 0x040004D4 RID: 1236
		[Obsolete("Use CentralHeaderDigitalSignaure instead")]
		public const int CENDIGITALSIG = 84233040;

		// Token: 0x040004D5 RID: 1237
		public const int EndOfCentralDirectorySignature = 101010256;

		// Token: 0x040004D6 RID: 1238
		[Obsolete("Use EndOfCentralDirectorySignature instead")]
		public const int ENDSIG = 101010256;

		// Token: 0x040004D7 RID: 1239
		private static int defaultCodePage = Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage;
	}
}
