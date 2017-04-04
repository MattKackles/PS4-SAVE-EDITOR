using System;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Encryption
{
	// Token: 0x020000BD RID: 189
	internal class ZipAESTransform : ICryptoTransform, IDisposable
	{
		// Token: 0x060007ED RID: 2029 RVA: 0x0002D844 File Offset: 0x0002BA44
		public ZipAESTransform(string key, byte[] saltBytes, int blockSize, bool writeMode)
		{
			if (blockSize != 16 && blockSize != 32)
			{
				throw new Exception("Invalid blocksize " + blockSize + ". Must be 16 or 32.");
			}
			if (saltBytes.Length != blockSize / 2)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Invalid salt len. Must be ",
					blockSize / 2,
					" for blocksize ",
					blockSize
				}));
			}
			this._blockSize = blockSize;
			this._encryptBuffer = new byte[this._blockSize];
			this._encrPos = 16;
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(key, saltBytes, 1000);
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.Mode = CipherMode.ECB;
			this._counterNonce = new byte[this._blockSize];
			byte[] bytes = rfc2898DeriveBytes.GetBytes(this._blockSize);
			byte[] bytes2 = rfc2898DeriveBytes.GetBytes(this._blockSize);
			this._encryptor = rijndaelManaged.CreateEncryptor(bytes, bytes2);
			this._pwdVerifier = rfc2898DeriveBytes.GetBytes(2);
			this._hmacsha1 = new HMACSHA1(bytes2);
			this._writeMode = writeMode;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0002D958 File Offset: 0x0002BB58
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			if (!this._writeMode)
			{
				this._hmacsha1.TransformBlock(inputBuffer, inputOffset, inputCount, inputBuffer, inputOffset);
			}
			for (int i = 0; i < inputCount; i++)
			{
				if (this._encrPos == 16)
				{
					int num = 0;
					while (true)
					{
						byte[] expr_3E_cp_0 = this._counterNonce;
						int expr_3E_cp_1 = num;
						if ((expr_3E_cp_0[expr_3E_cp_1] += 1) != 0)
						{
							break;
						}
						num++;
					}
					this._encryptor.TransformBlock(this._counterNonce, 0, this._blockSize, this._encryptBuffer, 0);
					this._encrPos = 0;
				}
				outputBuffer[i + outputOffset] = (inputBuffer[i + inputOffset] ^ this._encryptBuffer[this._encrPos++]);
			}
			if (this._writeMode)
			{
				this._hmacsha1.TransformBlock(outputBuffer, outputOffset, inputCount, outputBuffer, outputOffset);
			}
			return inputCount;
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0002DA2C File Offset: 0x0002BC2C
		public byte[] PwdVerifier
		{
			get
			{
				return this._pwdVerifier;
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0002DA34 File Offset: 0x0002BC34
		public byte[] GetAuthCode()
		{
			if (!this._finalised)
			{
				byte[] inputBuffer = new byte[0];
				this._hmacsha1.TransformFinalBlock(inputBuffer, 0, 0);
				this._finalised = true;
			}
			return this._hmacsha1.Hash;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0002DA71 File Offset: 0x0002BC71
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			throw new NotImplementedException("ZipAESTransform.TransformFinalBlock");
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x0002DA7D File Offset: 0x0002BC7D
		public int InputBlockSize
		{
			get
			{
				return this._blockSize;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0002DA85 File Offset: 0x0002BC85
		public int OutputBlockSize
		{
			get
			{
				return this._blockSize;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0002DA8D File Offset: 0x0002BC8D
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0002DA90 File Offset: 0x0002BC90
		public bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0002DA93 File Offset: 0x0002BC93
		public void Dispose()
		{
			this._encryptor.Dispose();
		}

		// Token: 0x04000395 RID: 917
		private const int PWD_VER_LENGTH = 2;

		// Token: 0x04000396 RID: 918
		private const int KEY_ROUNDS = 1000;

		// Token: 0x04000397 RID: 919
		private const int ENCRYPT_BLOCK = 16;

		// Token: 0x04000398 RID: 920
		private int _blockSize;

		// Token: 0x04000399 RID: 921
		private ICryptoTransform _encryptor;

		// Token: 0x0400039A RID: 922
		private readonly byte[] _counterNonce;

		// Token: 0x0400039B RID: 923
		private byte[] _encryptBuffer;

		// Token: 0x0400039C RID: 924
		private int _encrPos;

		// Token: 0x0400039D RID: 925
		private byte[] _pwdVerifier;

		// Token: 0x0400039E RID: 926
		private HMACSHA1 _hmacsha1;

		// Token: 0x0400039F RID: 927
		private bool _finalised;

		// Token: 0x040003A0 RID: 928
		private bool _writeMode;
	}
}
