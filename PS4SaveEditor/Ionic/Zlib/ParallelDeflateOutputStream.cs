using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Crc;

namespace Ionic.Zlib
{
	// Token: 0x02000145 RID: 325
	public class ParallelDeflateOutputStream : Stream
	{
		// Token: 0x06000CC9 RID: 3273 RVA: 0x0004A236 File Offset: 0x00048436
		public ParallelDeflateOutputStream(Stream stream) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0004A242 File Offset: 0x00048442
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level) : this(stream, level, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0004A24E File Offset: 0x0004844E
		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0004A25A File Offset: 0x0004845A
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0004A268 File Offset: 0x00048468
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			this._outStream = stream;
			this._compressLevel = level;
			this.Strategy = strategy;
			this._leaveOpen = leaveOpen;
			this.MaxBufferPairs = 16;
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0004A2D7 File Offset: 0x000484D7
		// (set) Token: 0x06000CCF RID: 3279 RVA: 0x0004A2DF File Offset: 0x000484DF
		public CompressionStrategy Strategy
		{
			get;
			private set;
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0004A2E8 File Offset: 0x000484E8
		// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x0004A2F0 File Offset: 0x000484F0
		public int MaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x0004A30D File Offset: 0x0004850D
		// (set) Token: 0x06000CD3 RID: 3283 RVA: 0x0004A315 File Offset: 0x00048515
		public int BufferSize
		{
			get
			{
				return this._bufferSize;
			}
			set
			{
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("BufferSize", "BufferSize must be greater than 1024 bytes");
				}
				this._bufferSize = value;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x0004A336 File Offset: 0x00048536
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x0004A33E File Offset: 0x0004853E
		public long BytesProcessed
		{
			get
			{
				return this._totalBytesProcessed;
			}
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0004A348 File Offset: 0x00048548
		private void _InitializePoolOfWorkItems()
		{
			this._toWrite = new Queue<int>();
			this._toFill = new Queue<int>();
			this._pool = new List<WorkItem>();
			int num = ParallelDeflateOutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			num = Math.Min(num, this._maxBufferPairs);
			for (int i = 0; i < num; i++)
			{
				this._pool.Add(new WorkItem(this._bufferSize, this._compressLevel, this.Strategy, i));
				this._toFill.Enqueue(i);
			}
			this._newlyCompressedBlob = new AutoResetEvent(false);
			this._runningCrc = new CRC32();
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0004A400 File Offset: 0x00048600
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool mustWait = false;
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (count == 0)
			{
				return;
			}
			if (!this._firstWriteDone)
			{
				this._InitializePoolOfWorkItems();
				this._firstWriteDone = true;
			}
			while (true)
			{
				this.EmitPendingBuffers(false, mustWait);
				mustWait = false;
				int num;
				if (this._currentlyFilling >= 0)
				{
					num = this._currentlyFilling;
					goto IL_9A;
				}
				if (this._toFill.Count != 0)
				{
					num = this._toFill.Dequeue();
					this._lastFilled++;
					goto IL_9A;
				}
				mustWait = true;
				IL_14C:
				if (count <= 0)
				{
					return;
				}
				continue;
				IL_9A:
				WorkItem workItem = this._pool[num];
				int num2 = (workItem.buffer.Length - workItem.inputBytesAvailable > count) ? count : (workItem.buffer.Length - workItem.inputBytesAvailable);
				workItem.ordinal = this._lastFilled;
				Buffer.BlockCopy(buffer, offset, workItem.buffer, workItem.inputBytesAvailable, num2);
				count -= num2;
				offset += num2;
				workItem.inputBytesAvailable += num2;
				if (workItem.inputBytesAvailable == workItem.buffer.Length)
				{
					if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this._DeflateOne), workItem))
					{
						break;
					}
					this._currentlyFilling = -1;
				}
				else
				{
					this._currentlyFilling = num;
				}
				goto IL_14C;
			}
			throw new Exception("Cannot enqueue workitem");
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0004A560 File Offset: 0x00048760
		private void _FlushFinish()
		{
			byte[] array = new byte[128];
			ZlibCodec zlibCodec = new ZlibCodec();
			int num = zlibCodec.InitializeDeflate(this._compressLevel, false);
			zlibCodec.InputBuffer = null;
			zlibCodec.NextIn = 0;
			zlibCodec.AvailableBytesIn = 0;
			zlibCodec.OutputBuffer = array;
			zlibCodec.NextOut = 0;
			zlibCodec.AvailableBytesOut = array.Length;
			num = zlibCodec.Deflate(FlushType.Finish);
			if (num != 1 && num != 0)
			{
				throw new Exception("deflating: " + zlibCodec.Message);
			}
			if (array.Length - zlibCodec.AvailableBytesOut > 0)
			{
				this._outStream.Write(array, 0, array.Length - zlibCodec.AvailableBytesOut);
			}
			zlibCodec.EndDeflate();
			this._Crc32 = this._runningCrc.Crc32Result;
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0004A61C File Offset: 0x0004881C
		private void _Flush(bool lastInput)
		{
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this.emitting)
			{
				return;
			}
			if (this._currentlyFilling >= 0)
			{
				WorkItem wi = this._pool[this._currentlyFilling];
				this._DeflateOne(wi);
				this._currentlyFilling = -1;
			}
			if (lastInput)
			{
				this.EmitPendingBuffers(true, false);
				this._FlushFinish();
				return;
			}
			this.EmitPendingBuffers(false, false);
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0004A684 File Offset: 0x00048884
		public override void Flush()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			this._Flush(false);
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0004A6C8 File Offset: 0x000488C8
		public override void Close()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			if (this._isClosed)
			{
				return;
			}
			this._Flush(true);
			if (!this._leaveOpen)
			{
				this._outStream.Close();
			}
			this._isClosed = true;
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0004A72D File Offset: 0x0004892D
		public new void Dispose()
		{
			this.Close();
			this._pool = null;
			this.Dispose(true);
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0004A743 File Offset: 0x00048943
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0004A74C File Offset: 0x0004894C
		public void Reset(Stream stream)
		{
			if (!this._firstWriteDone)
			{
				return;
			}
			this._toWrite.Clear();
			this._toFill.Clear();
			foreach (WorkItem current in this._pool)
			{
				this._toFill.Enqueue(current.index);
				current.ordinal = -1;
			}
			this._firstWriteDone = false;
			this._totalBytesProcessed = 0L;
			this._runningCrc = new CRC32();
			this._isClosed = false;
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
			this._outStream = stream;
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0004A814 File Offset: 0x00048A14
		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (this.emitting)
			{
				return;
			}
			this.emitting = true;
			if (doAll || mustWait)
			{
				this._newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = doAll ? 200 : (mustWait ? -1 : 0);
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(this._toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (this._toWrite.Count > 0)
							{
								num3 = this._toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(this._toWrite);
						}
						if (num3 >= 0)
						{
							WorkItem workItem = this._pool[num3];
							if (workItem.ordinal != this._lastWritten + 1)
							{
								lock (this._toWrite)
								{
									this._toWrite.Enqueue(num3);
								}
								if (num == num3)
								{
									this._newlyCompressedBlob.WaitOne();
									num = -1;
								}
								else if (num == -1)
								{
									num = num3;
								}
							}
							else
							{
								num = -1;
								this._outStream.Write(workItem.compressed, 0, workItem.compressedBytesAvailable);
								this._runningCrc.Combine(workItem.crc, workItem.inputBytesAvailable);
								this._totalBytesProcessed += (long)workItem.inputBytesAvailable;
								workItem.inputBytesAvailable = 0;
								this._lastWritten = workItem.ordinal;
								this._toFill.Enqueue(workItem.index);
								if (num2 == -1)
								{
									num2 = 0;
								}
							}
						}
					}
					else
					{
						num3 = -1;
					}
				}
				while (num3 >= 0);
			}
			while (doAll && this._lastWritten != this._latestCompressed);
			this.emitting = false;
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0004A9B4 File Offset: 0x00048BB4
		private void _DeflateOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				int arg_0D_0 = workItem.index;
				CRC32 cRC = new CRC32();
				cRC.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
				this.DeflateOneSegment(workItem);
				workItem.crc = cRC.Crc32Result;
				lock (this._latestLock)
				{
					if (workItem.ordinal > this._latestCompressed)
					{
						this._latestCompressed = workItem.ordinal;
					}
				}
				lock (this._toWrite)
				{
					this._toWrite.Enqueue(workItem.index);
				}
				this._newlyCompressedBlob.Set();
			}
			catch (Exception pendingException)
			{
				lock (this._eLock)
				{
					if (this._pendingException != null)
					{
						this._pendingException = pendingException;
					}
				}
			}
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0004AAE0 File Offset: 0x00048CE0
		private bool DeflateOneSegment(WorkItem workitem)
		{
			ZlibCodec compressor = workitem.compressor;
			compressor.ResetDeflate();
			compressor.NextIn = 0;
			compressor.AvailableBytesIn = workitem.inputBytesAvailable;
			compressor.NextOut = 0;
			compressor.AvailableBytesOut = workitem.compressed.Length;
			do
			{
				compressor.Deflate(FlushType.None);
			}
			while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);
			compressor.Deflate(FlushType.Sync);
			workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
			return true;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0004AB54 File Offset: 0x00048D54
		[Conditional("Trace")]
		private void TraceOutput(ParallelDeflateOutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this._DesiredTrace) != ParallelDeflateOutputStream.TraceBits.None)
			{
				lock (this._outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
					Console.Write("{0:000} PDOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x0004ABCC File Offset: 0x00048DCC
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0004ABCF File Offset: 0x00048DCF
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x0004ABD2 File Offset: 0x00048DD2
		public override bool CanWrite
		{
			get
			{
				return this._outStream.CanWrite;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x0004ABDF File Offset: 0x00048DDF
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x0004ABE6 File Offset: 0x00048DE6
		// (set) Token: 0x06000CE8 RID: 3304 RVA: 0x0004ABF3 File Offset: 0x00048DF3
		public override long Position
		{
			get
			{
				return this._outStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0004ABFA File Offset: 0x00048DFA
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0004AC01 File Offset: 0x00048E01
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0004AC08 File Offset: 0x00048E08
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400070A RID: 1802
		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		// Token: 0x0400070B RID: 1803
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x0400070C RID: 1804
		private List<WorkItem> _pool;

		// Token: 0x0400070D RID: 1805
		private bool _leaveOpen;

		// Token: 0x0400070E RID: 1806
		private bool emitting;

		// Token: 0x0400070F RID: 1807
		private Stream _outStream;

		// Token: 0x04000710 RID: 1808
		private int _maxBufferPairs;

		// Token: 0x04000711 RID: 1809
		private int _bufferSize = ParallelDeflateOutputStream.IO_BUFFER_SIZE_DEFAULT;

		// Token: 0x04000712 RID: 1810
		private AutoResetEvent _newlyCompressedBlob;

		// Token: 0x04000713 RID: 1811
		private object _outputLock = new object();

		// Token: 0x04000714 RID: 1812
		private bool _isClosed;

		// Token: 0x04000715 RID: 1813
		private bool _firstWriteDone;

		// Token: 0x04000716 RID: 1814
		private int _currentlyFilling;

		// Token: 0x04000717 RID: 1815
		private int _lastFilled;

		// Token: 0x04000718 RID: 1816
		private int _lastWritten;

		// Token: 0x04000719 RID: 1817
		private int _latestCompressed;

		// Token: 0x0400071A RID: 1818
		private int _Crc32;

		// Token: 0x0400071B RID: 1819
		private CRC32 _runningCrc;

		// Token: 0x0400071C RID: 1820
		private object _latestLock = new object();

		// Token: 0x0400071D RID: 1821
		private Queue<int> _toWrite;

		// Token: 0x0400071E RID: 1822
		private Queue<int> _toFill;

		// Token: 0x0400071F RID: 1823
		private long _totalBytesProcessed;

		// Token: 0x04000720 RID: 1824
		private CompressionLevel _compressLevel;

		// Token: 0x04000721 RID: 1825
		private volatile Exception _pendingException;

		// Token: 0x04000722 RID: 1826
		private bool _handlingException;

		// Token: 0x04000723 RID: 1827
		private object _eLock = new object();

		// Token: 0x04000724 RID: 1828
		private ParallelDeflateOutputStream.TraceBits _DesiredTrace = ParallelDeflateOutputStream.TraceBits.EmitLock | ParallelDeflateOutputStream.TraceBits.EmitEnter | ParallelDeflateOutputStream.TraceBits.EmitBegin | ParallelDeflateOutputStream.TraceBits.EmitDone | ParallelDeflateOutputStream.TraceBits.EmitSkip | ParallelDeflateOutputStream.TraceBits.Session | ParallelDeflateOutputStream.TraceBits.Compress | ParallelDeflateOutputStream.TraceBits.WriteEnter | ParallelDeflateOutputStream.TraceBits.WriteTake;

		// Token: 0x02000146 RID: 326
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x04000727 RID: 1831
			None = 0u,
			// Token: 0x04000728 RID: 1832
			NotUsed1 = 1u,
			// Token: 0x04000729 RID: 1833
			EmitLock = 2u,
			// Token: 0x0400072A RID: 1834
			EmitEnter = 4u,
			// Token: 0x0400072B RID: 1835
			EmitBegin = 8u,
			// Token: 0x0400072C RID: 1836
			EmitDone = 16u,
			// Token: 0x0400072D RID: 1837
			EmitSkip = 32u,
			// Token: 0x0400072E RID: 1838
			EmitAll = 58u,
			// Token: 0x0400072F RID: 1839
			Flush = 64u,
			// Token: 0x04000730 RID: 1840
			Lifecycle = 128u,
			// Token: 0x04000731 RID: 1841
			Session = 256u,
			// Token: 0x04000732 RID: 1842
			Synch = 512u,
			// Token: 0x04000733 RID: 1843
			Instance = 1024u,
			// Token: 0x04000734 RID: 1844
			Compress = 2048u,
			// Token: 0x04000735 RID: 1845
			Write = 4096u,
			// Token: 0x04000736 RID: 1846
			WriteEnter = 8192u,
			// Token: 0x04000737 RID: 1847
			WriteTake = 16384u,
			// Token: 0x04000738 RID: 1848
			All = 4294967295u
		}
	}
}
