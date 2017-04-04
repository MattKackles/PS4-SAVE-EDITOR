using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x020000EA RID: 234
	public class TestStatus
	{
		// Token: 0x060009A9 RID: 2473 RVA: 0x00034FDA File Offset: 0x000331DA
		public TestStatus(ZipFile file)
		{
			this.file_ = file;
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x00034FE9 File Offset: 0x000331E9
		public TestOperation Operation
		{
			get
			{
				return this.operation_;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x00034FF1 File Offset: 0x000331F1
		public ZipFile File
		{
			get
			{
				return this.file_;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x00034FF9 File Offset: 0x000331F9
		public ZipEntry Entry
		{
			get
			{
				return this.entry_;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x00035001 File Offset: 0x00033201
		public int ErrorCount
		{
			get
			{
				return this.errorCount_;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x00035009 File Offset: 0x00033209
		public long BytesTested
		{
			get
			{
				return this.bytesTested_;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x00035011 File Offset: 0x00033211
		public bool EntryValid
		{
			get
			{
				return this.entryValid_;
			}
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00035019 File Offset: 0x00033219
		internal void AddError()
		{
			this.errorCount_++;
			this.entryValid_ = false;
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00035030 File Offset: 0x00033230
		internal void SetOperation(TestOperation operation)
		{
			this.operation_ = operation;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x00035039 File Offset: 0x00033239
		internal void SetEntry(ZipEntry entry)
		{
			this.entry_ = entry;
			this.entryValid_ = true;
			this.bytesTested_ = 0L;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00035051 File Offset: 0x00033251
		internal void SetBytesTested(long value)
		{
			this.bytesTested_ = value;
		}

		// Token: 0x04000534 RID: 1332
		private ZipFile file_;

		// Token: 0x04000535 RID: 1333
		private ZipEntry entry_;

		// Token: 0x04000536 RID: 1334
		private bool entryValid_;

		// Token: 0x04000537 RID: 1335
		private int errorCount_;

		// Token: 0x04000538 RID: 1336
		private long bytesTested_;

		// Token: 0x04000539 RID: 1337
		private TestOperation operation_;
	}
}
