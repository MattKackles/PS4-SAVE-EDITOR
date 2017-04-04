using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000DB RID: 219
	public class ZipEntry : ICloneable
	{
		// Token: 0x0600090D RID: 2317 RVA: 0x00033334 File Offset: 0x00031534
		public ZipEntry(string name) : this(name, 0, 51, CompressionMethod.Deflated)
		{
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00033341 File Offset: 0x00031541
		internal ZipEntry(string name, int versionRequiredToExtract) : this(name, versionRequiredToExtract, 51, CompressionMethod.Deflated)
		{
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00033350 File Offset: 0x00031550
		internal ZipEntry(string name, int versionRequiredToExtract, int madeByInfo, CompressionMethod method)
		{
			this.externalFileAttributes = -1;
			this.method = CompressionMethod.Deflated;
			this.zipFileIndex = -1L;
			base..ctor();
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length > 65535)
			{
				throw new ArgumentException("Name is too long", "name");
			}
			if (versionRequiredToExtract != 0 && versionRequiredToExtract < 10)
			{
				throw new ArgumentOutOfRangeException("versionRequiredToExtract");
			}
			this.DateTime = DateTime.Now;
			this.name = name;
			this.versionMadeBy = (ushort)madeByInfo;
			this.versionToExtract = (ushort)versionRequiredToExtract;
			this.method = method;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x000333E4 File Offset: 0x000315E4
		[Obsolete("Use Clone instead")]
		public ZipEntry(ZipEntry entry)
		{
			this.externalFileAttributes = -1;
			this.method = CompressionMethod.Deflated;
			this.zipFileIndex = -1L;
			base..ctor();
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.known = entry.known;
			this.name = entry.name;
			this.size = entry.size;
			this.compressedSize = entry.compressedSize;
			this.crc = entry.crc;
			this.dosTime = entry.dosTime;
			this.method = entry.method;
			this.comment = entry.comment;
			this.versionToExtract = entry.versionToExtract;
			this.versionMadeBy = entry.versionMadeBy;
			this.externalFileAttributes = entry.externalFileAttributes;
			this.flags = entry.flags;
			this.zipFileIndex = entry.zipFileIndex;
			this.offset = entry.offset;
			this.forceZip64_ = entry.forceZip64_;
			if (entry.extra != null)
			{
				this.extra = new byte[entry.extra.Length];
				Array.Copy(entry.extra, 0, this.extra, 0, entry.extra.Length);
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x00033505 File Offset: 0x00031705
		public bool HasCrc
		{
			get
			{
				return (byte)(this.known & ZipEntry.Known.Crc) != 0;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x00033516 File Offset: 0x00031716
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x00033526 File Offset: 0x00031726
		public bool IsCrypted
		{
			get
			{
				return (this.flags & 1) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 1;
					return;
				}
				this.flags &= -2;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00033549 File Offset: 0x00031749
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x0003355D File Offset: 0x0003175D
		public bool IsUnicodeText
		{
			get
			{
				return (this.flags & 2048) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 2048;
					return;
				}
				this.flags &= -2049;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x00033587 File Offset: 0x00031787
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x0003358F File Offset: 0x0003178F
		internal byte CryptoCheckValue
		{
			get
			{
				return this.cryptoCheckValue_;
			}
			set
			{
				this.cryptoCheckValue_ = value;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x00033598 File Offset: 0x00031798
		// (set) Token: 0x06000919 RID: 2329 RVA: 0x000335A0 File Offset: 0x000317A0
		public int Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x000335A9 File Offset: 0x000317A9
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x000335B1 File Offset: 0x000317B1
		public long ZipFileIndex
		{
			get
			{
				return this.zipFileIndex;
			}
			set
			{
				this.zipFileIndex = value;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x000335BA File Offset: 0x000317BA
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x000335C2 File Offset: 0x000317C2
		public long Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				this.offset = value;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x000335CB File Offset: 0x000317CB
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x000335E1 File Offset: 0x000317E1
		public int ExternalFileAttributes
		{
			get
			{
				if ((byte)(this.known & ZipEntry.Known.ExternalAttributes) == 0)
				{
					return -1;
				}
				return this.externalFileAttributes;
			}
			set
			{
				this.externalFileAttributes = value;
				this.known |= ZipEntry.Known.ExternalAttributes;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x000335FA File Offset: 0x000317FA
		public int VersionMadeBy
		{
			get
			{
				return (int)(this.versionMadeBy & 255);
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x00033608 File Offset: 0x00031808
		public bool IsDOSEntry
		{
			get
			{
				return this.HostSystem == 0 || this.HostSystem == 10;
			}
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00033620 File Offset: 0x00031820
		private bool HasDosAttributes(int attributes)
		{
			bool result = false;
			if ((byte)(this.known & ZipEntry.Known.ExternalAttributes) != 0 && (this.HostSystem == 0 || this.HostSystem == 10) && (this.ExternalFileAttributes & attributes) == attributes)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x0003365B File Offset: 0x0003185B
		// (set) Token: 0x06000924 RID: 2340 RVA: 0x0003366B File Offset: 0x0003186B
		public int HostSystem
		{
			get
			{
				return this.versionMadeBy >> 8 & 255;
			}
			set
			{
				this.versionMadeBy &= 255;
				this.versionMadeBy |= (ushort)((value & 255) << 8);
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x00033698 File Offset: 0x00031898
		public int Version
		{
			get
			{
				if (this.versionToExtract != 0)
				{
					return (int)this.versionToExtract;
				}
				int result = 10;
				if (this.AESKeySize > 0)
				{
					result = 51;
				}
				else if (this.CentralHeaderRequiresZip64)
				{
					result = 45;
				}
				else if (CompressionMethod.Deflated == this.method)
				{
					result = 20;
				}
				else if (this.IsDirectory)
				{
					result = 20;
				}
				else if (this.IsCrypted)
				{
					result = 20;
				}
				else if (this.HasDosAttributes(8))
				{
					result = 11;
				}
				return result;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x00033708 File Offset: 0x00031908
		public bool CanDecompress
		{
			get
			{
				return this.Version <= 51 && (this.Version == 10 || this.Version == 11 || this.Version == 20 || this.Version == 45 || this.Version == 51) && this.IsCompressionMethodSupported();
			}
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00033759 File Offset: 0x00031959
		public void ForceZip64()
		{
			this.forceZip64_ = true;
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00033762 File Offset: 0x00031962
		public bool IsZip64Forced()
		{
			return this.forceZip64_;
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x0003376C File Offset: 0x0003196C
		public bool LocalHeaderRequiresZip64
		{
			get
			{
				bool flag = this.forceZip64_;
				if (!flag)
				{
					ulong num = this.compressedSize;
					if (this.versionToExtract == 0 && this.IsCrypted)
					{
						num += 12uL;
					}
					flag = ((this.size >= (ulong)-1 || num >= (ulong)-1) && (this.versionToExtract == 0 || this.versionToExtract >= 45));
				}
				return flag;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x000337CC File Offset: 0x000319CC
		public bool CentralHeaderRequiresZip64
		{
			get
			{
				return this.LocalHeaderRequiresZip64 || this.offset >= (long)((ulong)-1);
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x000337E5 File Offset: 0x000319E5
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x000337FC File Offset: 0x000319FC
		public long DosTime
		{
			get
			{
				if ((byte)(this.known & ZipEntry.Known.Time) == 0)
				{
					return 0L;
				}
				return (long)((ulong)this.dosTime);
			}
			set
			{
				this.dosTime = (uint)value;
				this.known |= ZipEntry.Known.Time;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x00033818 File Offset: 0x00031A18
		// (set) Token: 0x0600092E RID: 2350 RVA: 0x000338BC File Offset: 0x00031ABC
		public DateTime DateTime
		{
			get
			{
				uint second = Math.Min(59u, 2u * (this.dosTime & 31u));
				uint minute = Math.Min(59u, this.dosTime >> 5 & 63u);
				uint hour = Math.Min(23u, this.dosTime >> 11 & 31u);
				uint month = Math.Max(1u, Math.Min(12u, this.dosTime >> 21 & 15u));
				uint year = (this.dosTime >> 25 & 127u) + 1980u;
				int day = Math.Max(1, Math.Min(DateTime.DaysInMonth((int)year, (int)month), (int)(this.dosTime >> 16 & 31u)));
				return new DateTime((int)year, (int)month, day, (int)hour, (int)minute, (int)second);
			}
			set
			{
				uint num = (uint)value.Year;
				uint num2 = (uint)value.Month;
				uint num3 = (uint)value.Day;
				uint num4 = (uint)value.Hour;
				uint num5 = (uint)value.Minute;
				uint num6 = (uint)value.Second;
				if (num < 1980u)
				{
					num = 1980u;
					num2 = 1u;
					num3 = 1u;
					num4 = 0u;
					num5 = 0u;
					num6 = 0u;
				}
				else if (num > 2107u)
				{
					num = 2107u;
					num2 = 12u;
					num3 = 31u;
					num4 = 23u;
					num5 = 59u;
					num6 = 59u;
				}
				this.DosTime = (long)((ulong)((num - 1980u & 127u) << 25 | num2 << 21 | num3 << 16 | num4 << 11 | num5 << 5 | num6 >> 1));
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x00033963 File Offset: 0x00031B63
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x0003396B File Offset: 0x00031B6B
		// (set) Token: 0x06000931 RID: 2353 RVA: 0x00033981 File Offset: 0x00031B81
		public long Size
		{
			get
			{
				if ((byte)(this.known & ZipEntry.Known.Size) == 0)
				{
					return -1L;
				}
				return (long)this.size;
			}
			set
			{
				this.size = (ulong)value;
				this.known |= ZipEntry.Known.Size;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x00033999 File Offset: 0x00031B99
		// (set) Token: 0x06000933 RID: 2355 RVA: 0x000339AF File Offset: 0x00031BAF
		public long CompressedSize
		{
			get
			{
				if ((byte)(this.known & ZipEntry.Known.CompressedSize) == 0)
				{
					return -1L;
				}
				return (long)this.compressedSize;
			}
			set
			{
				this.compressedSize = (ulong)value;
				this.known |= ZipEntry.Known.CompressedSize;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x000339C7 File Offset: 0x00031BC7
		// (set) Token: 0x06000935 RID: 2357 RVA: 0x000339E1 File Offset: 0x00031BE1
		public long Crc
		{
			get
			{
				if ((byte)(this.known & ZipEntry.Known.Crc) == 0)
				{
					return -1L;
				}
				return (long)((ulong)this.crc & (ulong)-1);
			}
			set
			{
				if (((ulong)this.crc & 18446744069414584320uL) != 0uL)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.crc = (uint)value;
				this.known |= ZipEntry.Known.Crc;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x00033A1A File Offset: 0x00031C1A
		// (set) Token: 0x06000937 RID: 2359 RVA: 0x00033A22 File Offset: 0x00031C22
		public CompressionMethod CompressionMethod
		{
			get
			{
				return this.method;
			}
			set
			{
				if (!ZipEntry.IsCompressionMethodSupported(value))
				{
					throw new NotSupportedException("Compression method not supported");
				}
				this.method = value;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x00033A3E File Offset: 0x00031C3E
		internal CompressionMethod CompressionMethodForHeader
		{
			get
			{
				if (this.AESKeySize <= 0)
				{
					return this.method;
				}
				return CompressionMethod.WinZipAES;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x00033A52 File Offset: 0x00031C52
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x00033A5C File Offset: 0x00031C5C
		public byte[] ExtraData
		{
			get
			{
				return this.extra;
			}
			set
			{
				if (value == null)
				{
					this.extra = null;
					return;
				}
				if (value.Length > 65535)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.extra = new byte[value.Length];
				Array.Copy(value, 0, this.extra, 0, value.Length);
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x00033AA8 File Offset: 0x00031CA8
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x00033B04 File Offset: 0x00031D04
		public int AESKeySize
		{
			get
			{
				switch (this._aesEncryptionStrength)
				{
				case 0:
					return 0;
				case 1:
					return 128;
				case 2:
					return 192;
				case 3:
					return 256;
				default:
					throw new ZipException("Invalid AESEncryptionStrength " + this._aesEncryptionStrength);
				}
			}
			set
			{
				if (value == 0)
				{
					this._aesEncryptionStrength = 0;
					return;
				}
				if (value == 128)
				{
					this._aesEncryptionStrength = 1;
					return;
				}
				if (value != 256)
				{
					throw new ZipException("AESKeySize must be 0, 128 or 256: " + value);
				}
				this._aesEncryptionStrength = 3;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x00033B56 File Offset: 0x00031D56
		internal byte AESEncryptionStrength
		{
			get
			{
				return (byte)this._aesEncryptionStrength;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x00033B5F File Offset: 0x00031D5F
		internal int AESSaltLen
		{
			get
			{
				return this.AESKeySize / 16;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00033B6A File Offset: 0x00031D6A
		internal int AESOverheadSize
		{
			get
			{
				return 12 + this.AESSaltLen;
			}
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00033B78 File Offset: 0x00031D78
		internal void ProcessExtraData(bool localHeader)
		{
			ZipExtraData zipExtraData = new ZipExtraData(this.extra);
			if (zipExtraData.Find(1))
			{
				this.forceZip64_ = true;
				if (zipExtraData.ValueLength < 4)
				{
					throw new ZipException("Extra data extended Zip64 information length is invalid");
				}
				if (localHeader || this.size == (ulong)-1)
				{
					this.size = (ulong)zipExtraData.ReadLong();
				}
				if (localHeader || this.compressedSize == (ulong)-1)
				{
					this.compressedSize = (ulong)zipExtraData.ReadLong();
				}
				if (!localHeader && this.offset == (long)((ulong)-1))
				{
					this.offset = zipExtraData.ReadLong();
				}
			}
			else if ((this.versionToExtract & 255) >= 45 && (this.size == (ulong)-1 || this.compressedSize == (ulong)-1))
			{
				throw new ZipException("Zip64 Extended information required but is missing.");
			}
			if (zipExtraData.Find(10))
			{
				if (zipExtraData.ValueLength < 4)
				{
					throw new ZipException("NTFS Extra data invalid");
				}
				zipExtraData.ReadInt();
				while (zipExtraData.UnreadCount >= 4)
				{
					int num = zipExtraData.ReadShort();
					int num2 = zipExtraData.ReadShort();
					if (num == 1)
					{
						if (num2 >= 24)
						{
							long fileTime = zipExtraData.ReadLong();
							zipExtraData.ReadLong();
							zipExtraData.ReadLong();
							this.DateTime = DateTime.FromFileTime(fileTime);
							break;
						}
						break;
					}
					else
					{
						zipExtraData.Skip(num2);
					}
				}
			}
			else if (zipExtraData.Find(21589))
			{
				int valueLength = zipExtraData.ValueLength;
				int num3 = zipExtraData.ReadByte();
				if ((num3 & 1) != 0 && valueLength >= 5)
				{
					int seconds = zipExtraData.ReadInt();
					this.DateTime = (new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime() + new TimeSpan(0, 0, 0, seconds, 0)).ToLocalTime();
				}
			}
			if (this.method == CompressionMethod.WinZipAES)
			{
				this.ProcessAESExtraData(zipExtraData);
			}
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00033D20 File Offset: 0x00031F20
		private void ProcessAESExtraData(ZipExtraData extraData)
		{
			if (!extraData.Find(39169))
			{
				throw new ZipException("AES Extra Data missing");
			}
			this.versionToExtract = 51;
			this.Flags |= 64;
			int valueLength = extraData.ValueLength;
			if (valueLength < 7)
			{
				throw new ZipException("AES Extra Data Length " + valueLength + " invalid.");
			}
			int aesVer = extraData.ReadShort();
			extraData.ReadShort();
			int aesEncryptionStrength = extraData.ReadByte();
			int num = extraData.ReadShort();
			this._aesVer = aesVer;
			this._aesEncryptionStrength = aesEncryptionStrength;
			this.method = (CompressionMethod)num;
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x00033DB3 File Offset: 0x00031FB3
		// (set) Token: 0x06000943 RID: 2371 RVA: 0x00033DBB File Offset: 0x00031FBB
		public string Comment
		{
			get
			{
				return this.comment;
			}
			set
			{
				if (value != null && value.Length > 65535)
				{
					throw new ArgumentOutOfRangeException("value", "cannot exceed 65535");
				}
				this.comment = value;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x00033DE4 File Offset: 0x00031FE4
		public bool IsDirectory
		{
			get
			{
				int length = this.name.Length;
				return (length > 0 && (this.name[length - 1] == '/' || this.name[length - 1] == '\\')) || this.HasDosAttributes(16);
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x00033E32 File Offset: 0x00032032
		public bool IsFile
		{
			get
			{
				return !this.IsDirectory && !this.HasDosAttributes(8);
			}
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00033E48 File Offset: 0x00032048
		public bool IsCompressionMethodSupported()
		{
			return ZipEntry.IsCompressionMethodSupported(this.CompressionMethod);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00033E58 File Offset: 0x00032058
		public object Clone()
		{
			ZipEntry zipEntry = (ZipEntry)base.MemberwiseClone();
			if (this.extra != null)
			{
				zipEntry.extra = new byte[this.extra.Length];
				Array.Copy(this.extra, 0, zipEntry.extra, 0, this.extra.Length);
			}
			return zipEntry;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00033EA8 File Offset: 0x000320A8
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00033EB0 File Offset: 0x000320B0
		public static bool IsCompressionMethodSupported(CompressionMethod method)
		{
			return method == CompressionMethod.Deflated || method == CompressionMethod.Stored;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00033EBC File Offset: 0x000320BC
		public static string CleanName(string name)
		{
			if (name == null)
			{
				return string.Empty;
			}
			if (Path.IsPathRooted(name))
			{
				name = name.Substring(Path.GetPathRoot(name).Length);
			}
			name = name.Replace("\\", "/");
			while (name.Length > 0 && name[0] == '/')
			{
				name = name.Remove(0, 1);
			}
			return name;
		}

		// Token: 0x040004EE RID: 1262
		private ZipEntry.Known known;

		// Token: 0x040004EF RID: 1263
		private int externalFileAttributes;

		// Token: 0x040004F0 RID: 1264
		private ushort versionMadeBy;

		// Token: 0x040004F1 RID: 1265
		private string name;

		// Token: 0x040004F2 RID: 1266
		private ulong size;

		// Token: 0x040004F3 RID: 1267
		private ulong compressedSize;

		// Token: 0x040004F4 RID: 1268
		private ushort versionToExtract;

		// Token: 0x040004F5 RID: 1269
		private uint crc;

		// Token: 0x040004F6 RID: 1270
		private uint dosTime;

		// Token: 0x040004F7 RID: 1271
		private CompressionMethod method;

		// Token: 0x040004F8 RID: 1272
		private byte[] extra;

		// Token: 0x040004F9 RID: 1273
		private string comment;

		// Token: 0x040004FA RID: 1274
		private int flags;

		// Token: 0x040004FB RID: 1275
		private long zipFileIndex;

		// Token: 0x040004FC RID: 1276
		private long offset;

		// Token: 0x040004FD RID: 1277
		private bool forceZip64_;

		// Token: 0x040004FE RID: 1278
		private byte cryptoCheckValue_;

		// Token: 0x040004FF RID: 1279
		private int _aesVer;

		// Token: 0x04000500 RID: 1280
		private int _aesEncryptionStrength;

		// Token: 0x020000DC RID: 220
		[Flags]
		private enum Known : byte
		{
			// Token: 0x04000502 RID: 1282
			None = 0,
			// Token: 0x04000503 RID: 1283
			Size = 1,
			// Token: 0x04000504 RID: 1284
			CompressedSize = 2,
			// Token: 0x04000505 RID: 1285
			Crc = 4,
			// Token: 0x04000506 RID: 1286
			Time = 8,
			// Token: 0x04000507 RID: 1287
			ExternalAttributes = 16
		}
	}
}
