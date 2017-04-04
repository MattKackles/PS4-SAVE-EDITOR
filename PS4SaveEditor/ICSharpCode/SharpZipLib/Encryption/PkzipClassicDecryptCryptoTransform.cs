using System;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Encryption
{
	// Token: 0x020000BA RID: 186
	internal class PkzipClassicDecryptCryptoTransform : PkzipClassicCryptoBase, ICryptoTransform, IDisposable
	{
		// Token: 0x060007D7 RID: 2007 RVA: 0x0002D4D4 File Offset: 0x0002B6D4
		internal PkzipClassicDecryptCryptoTransform(byte[] keyBlock)
		{
			base.SetKeys(keyBlock);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0002D4E4 File Offset: 0x0002B6E4
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			byte[] array = new byte[inputCount];
			this.TransformBlock(inputBuffer, inputOffset, inputCount, array, 0);
			return array;
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0002D508 File Offset: 0x0002B708
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			for (int i = inputOffset; i < inputOffset + inputCount; i++)
			{
				byte b = inputBuffer[i] ^ base.TransformByte();
				outputBuffer[outputOffset++] = b;
				base.UpdateKeys(b);
			}
			return inputCount;
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x0002D542 File Offset: 0x0002B742
		public bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0002D545 File Offset: 0x0002B745
		public int InputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x0002D548 File Offset: 0x0002B748
		public int OutputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0002D54B File Offset: 0x0002B74B
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0002D54E File Offset: 0x0002B74E
		public void Dispose()
		{
			base.Reset();
		}
	}
}
