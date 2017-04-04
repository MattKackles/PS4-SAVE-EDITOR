using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x020000B5 RID: 181
	public sealed class StreamUtils
	{
		// Token: 0x060007C0 RID: 1984 RVA: 0x0002CE44 File Offset: 0x0002B044
		public static void ReadFully(Stream stream, byte[] buffer)
		{
			StreamUtils.ReadFully(stream, buffer, 0, buffer.Length);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0002CE54 File Offset: 0x0002B054
		public static void ReadFully(Stream stream, byte[] buffer, int offset, int count)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			while (count > 0)
			{
				int num = stream.Read(buffer, offset, count);
				if (num <= 0)
				{
					throw new EndOfStreamException();
				}
				offset += num;
				count -= num;
			}
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0002CECC File Offset: 0x0002B0CC
		public static void Copy(Stream source, Stream destination, byte[] buffer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (buffer.Length < 128)
			{
				throw new ArgumentException("Buffer is too small", "buffer");
			}
			bool flag = true;
			while (flag)
			{
				int num = source.Read(buffer, 0, buffer.Length);
				if (num > 0)
				{
					destination.Write(buffer, 0, num);
				}
				else
				{
					destination.Flush();
					flag = false;
				}
			}
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0002CF47 File Offset: 0x0002B147
		public static void Copy(Stream source, Stream destination, byte[] buffer, ProgressHandler progressHandler, TimeSpan updateInterval, object sender, string name)
		{
			StreamUtils.Copy(source, destination, buffer, progressHandler, updateInterval, sender, name, -1L);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0002CF5C File Offset: 0x0002B15C
		public static void Copy(Stream source, Stream destination, byte[] buffer, ProgressHandler progressHandler, TimeSpan updateInterval, object sender, string name, long fixedTarget)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (buffer.Length < 128)
			{
				throw new ArgumentException("Buffer is too small", "buffer");
			}
			if (progressHandler == null)
			{
				throw new ArgumentNullException("progressHandler");
			}
			bool flag = true;
			DateTime now = DateTime.Now;
			long num = 0L;
			long target = 0L;
			if (fixedTarget >= 0L)
			{
				target = fixedTarget;
			}
			else if (source.CanSeek)
			{
				target = source.Length - source.Position;
			}
			ProgressEventArgs progressEventArgs = new ProgressEventArgs(name, num, target);
			progressHandler(sender, progressEventArgs);
			bool flag2 = true;
			while (flag)
			{
				int num2 = source.Read(buffer, 0, buffer.Length);
				if (num2 > 0)
				{
					num += (long)num2;
					flag2 = false;
					destination.Write(buffer, 0, num2);
				}
				else
				{
					destination.Flush();
					flag = false;
				}
				if (DateTime.Now - now > updateInterval)
				{
					flag2 = true;
					now = DateTime.Now;
					progressEventArgs = new ProgressEventArgs(name, num, target);
					progressHandler(sender, progressEventArgs);
					flag = progressEventArgs.ContinueRunning;
				}
			}
			if (!flag2)
			{
				progressEventArgs = new ProgressEventArgs(name, num, target);
				progressHandler(sender, progressEventArgs);
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0002D089 File Offset: 0x0002B289
		private StreamUtils()
		{
		}
	}
}
