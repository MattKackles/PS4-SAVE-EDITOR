using System;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Encryption
{
	// Token: 0x020000B9 RID: 185
	internal class PkzipClassicEncryptCryptoTransform : PkzipClassicCryptoBase, ICryptoTransform, IDisposable
	{
		// Token: 0x060007CF RID: 1999 RVA: 0x0002D44F File Offset: 0x0002B64F
		internal PkzipClassicEncryptCryptoTransform(byte[] keyBlock)
		{
			base.SetKeys(keyBlock);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002D460 File Offset: 0x0002B660
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			byte[] array = new byte[inputCount];
			this.TransformBlock(inputBuffer, inputOffset, inputCount, array, 0);
			return array;
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0002D484 File Offset: 0x0002B684
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			for (int i = inputOffset; i < inputOffset + inputCount; i++)
			{
				byte ch = inputBuffer[i];
				outputBuffer[outputOffset++] = (inputBuffer[i] ^ base.TransformByte());
				base.UpdateKeys(ch);
			}
			return inputCount;
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x0002D4C0 File Offset: 0x0002B6C0
		public bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x0002D4C3 File Offset: 0x0002B6C3
		public int InputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0002D4C6 File Offset: 0x0002B6C6
		public int OutputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0002D4C9 File Offset: 0x0002B6C9
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0002D4CC File Offset: 0x0002B6CC
		public void Dispose()
		{
			base.Reset();
		}
	}
}
