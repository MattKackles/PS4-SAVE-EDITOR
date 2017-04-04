using System;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Encryption
{
	// Token: 0x020000BB RID: 187
	public sealed class PkzipClassicManaged : PkzipClassic
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x0002D556 File Offset: 0x0002B756
		// (set) Token: 0x060007E0 RID: 2016 RVA: 0x0002D559 File Offset: 0x0002B759
		public override int BlockSize
		{
			get
			{
				return 8;
			}
			set
			{
				if (value != 8)
				{
					throw new CryptographicException("Block size is invalid");
				}
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0002D56C File Offset: 0x0002B76C
		public override KeySizes[] LegalKeySizes
		{
			get
			{
				return new KeySizes[]
				{
					new KeySizes(96, 96, 0)
				};
			}
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0002D58E File Offset: 0x0002B78E
		public override void GenerateIV()
		{
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0002D590 File Offset: 0x0002B790
		public override KeySizes[] LegalBlockSizes
		{
			get
			{
				return new KeySizes[]
				{
					new KeySizes(8, 8, 0)
				};
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x0002D5B0 File Offset: 0x0002B7B0
		// (set) Token: 0x060007E5 RID: 2021 RVA: 0x0002D5D0 File Offset: 0x0002B7D0
		public override byte[] Key
		{
			get
			{
				if (this.key_ == null)
				{
					this.GenerateKey();
				}
				return (byte[])this.key_.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length != 12)
				{
					throw new CryptographicException("Key size is illegal");
				}
				this.key_ = (byte[])value.Clone();
			}
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0002D604 File Offset: 0x0002B804
		public override void GenerateKey()
		{
			this.key_ = new byte[12];
			Random random = new Random();
			random.NextBytes(this.key_);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0002D630 File Offset: 0x0002B830
		public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
		{
			this.key_ = rgbKey;
			return new PkzipClassicEncryptCryptoTransform(this.Key);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0002D644 File Offset: 0x0002B844
		public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
		{
			this.key_ = rgbKey;
			return new PkzipClassicDecryptCryptoTransform(this.Key);
		}

		// Token: 0x0400038C RID: 908
		private byte[] key_;
	}
}
