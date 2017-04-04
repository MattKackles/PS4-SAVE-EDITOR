using System;
using System.Collections;

namespace Be.Windows.Forms
{
	// Token: 0x02000049 RID: 73
	internal class DataMap : ICollection, IEnumerable
	{
		// Token: 0x06000324 RID: 804 RVA: 0x00011A86 File Offset: 0x0000FC86
		public DataMap()
		{
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00011A9C File Offset: 0x0000FC9C
		public DataMap(IEnumerable collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			foreach (DataBlock block in collection)
			{
				this.AddLast(block);
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000326 RID: 806 RVA: 0x00011B10 File Offset: 0x0000FD10
		public DataBlock FirstBlock
		{
			get
			{
				return this._firstBlock;
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00011B18 File Offset: 0x0000FD18
		public void AddAfter(DataBlock block, DataBlock newBlock)
		{
			this.AddAfterInternal(block, newBlock);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00011B22 File Offset: 0x0000FD22
		public void AddBefore(DataBlock block, DataBlock newBlock)
		{
			this.AddBeforeInternal(block, newBlock);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00011B2C File Offset: 0x0000FD2C
		public void AddFirst(DataBlock block)
		{
			if (this._firstBlock == null)
			{
				this.AddBlockToEmptyMap(block);
				return;
			}
			this.AddBeforeInternal(this._firstBlock, block);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00011B4B File Offset: 0x0000FD4B
		public void AddLast(DataBlock block)
		{
			if (this._firstBlock == null)
			{
				this.AddBlockToEmptyMap(block);
				return;
			}
			this.AddAfterInternal(this.GetLastBlock(), block);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00011B6A File Offset: 0x0000FD6A
		public void Remove(DataBlock block)
		{
			this.RemoveInternal(block);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00011B73 File Offset: 0x0000FD73
		public void RemoveFirst()
		{
			if (this._firstBlock == null)
			{
				throw new InvalidOperationException("The collection is empty.");
			}
			this.RemoveInternal(this._firstBlock);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00011B94 File Offset: 0x0000FD94
		public void RemoveLast()
		{
			if (this._firstBlock == null)
			{
				throw new InvalidOperationException("The collection is empty.");
			}
			this.RemoveInternal(this.GetLastBlock());
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00011BB5 File Offset: 0x0000FDB5
		public DataBlock Replace(DataBlock block, DataBlock newBlock)
		{
			this.AddAfterInternal(block, newBlock);
			this.RemoveInternal(block);
			return newBlock;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00011BC8 File Offset: 0x0000FDC8
		public void Clear()
		{
			DataBlock nextBlock;
			for (DataBlock dataBlock = this.FirstBlock; dataBlock != null; dataBlock = nextBlock)
			{
				nextBlock = dataBlock.NextBlock;
				this.InvalidateBlock(dataBlock);
			}
			this._firstBlock = null;
			this._count = 0;
			this._version++;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00011C10 File Offset: 0x0000FE10
		private void AddAfterInternal(DataBlock block, DataBlock newBlock)
		{
			newBlock._previousBlock = block;
			newBlock._nextBlock = block._nextBlock;
			newBlock._map = this;
			if (block._nextBlock != null)
			{
				block._nextBlock._previousBlock = newBlock;
			}
			block._nextBlock = newBlock;
			this._version++;
			this._count++;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00011C70 File Offset: 0x0000FE70
		private void AddBeforeInternal(DataBlock block, DataBlock newBlock)
		{
			newBlock._nextBlock = block;
			newBlock._previousBlock = block._previousBlock;
			newBlock._map = this;
			if (block._previousBlock != null)
			{
				block._previousBlock._nextBlock = newBlock;
			}
			block._previousBlock = newBlock;
			if (this._firstBlock == block)
			{
				this._firstBlock = newBlock;
			}
			this._version++;
			this._count++;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00011CE0 File Offset: 0x0000FEE0
		private void RemoveInternal(DataBlock block)
		{
			DataBlock previousBlock = block._previousBlock;
			DataBlock nextBlock = block._nextBlock;
			if (previousBlock != null)
			{
				previousBlock._nextBlock = nextBlock;
			}
			if (nextBlock != null)
			{
				nextBlock._previousBlock = previousBlock;
			}
			if (this._firstBlock == block)
			{
				this._firstBlock = nextBlock;
			}
			this.InvalidateBlock(block);
			this._count--;
			this._version++;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00011D44 File Offset: 0x0000FF44
		private DataBlock GetLastBlock()
		{
			DataBlock result = null;
			for (DataBlock dataBlock = this.FirstBlock; dataBlock != null; dataBlock = dataBlock.NextBlock)
			{
				result = dataBlock;
			}
			return result;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00011D69 File Offset: 0x0000FF69
		private void InvalidateBlock(DataBlock block)
		{
			block._map = null;
			block._nextBlock = null;
			block._previousBlock = null;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00011D80 File Offset: 0x0000FF80
		private void AddBlockToEmptyMap(DataBlock block)
		{
			block._map = this;
			block._nextBlock = null;
			block._previousBlock = null;
			this._firstBlock = block;
			this._version++;
			this._count++;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00011DBC File Offset: 0x0000FFBC
		public void CopyTo(Array array, int index)
		{
			DataBlock[] array2 = array as DataBlock[];
			for (DataBlock dataBlock = this.FirstBlock; dataBlock != null; dataBlock = dataBlock.NextBlock)
			{
				array2[index++] = dataBlock;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00011DEC File Offset: 0x0000FFEC
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00011DF4 File Offset: 0x0000FFF4
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00011DF7 File Offset: 0x0000FFF7
		public object SyncRoot
		{
			get
			{
				return this._syncRoot;
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00011DFF File Offset: 0x0000FFFF
		public IEnumerator GetEnumerator()
		{
			return new DataMap.Enumerator(this);
		}

		// Token: 0x040001A1 RID: 417
		private readonly object _syncRoot = new object();

		// Token: 0x040001A2 RID: 418
		internal int _count;

		// Token: 0x040001A3 RID: 419
		internal DataBlock _firstBlock;

		// Token: 0x040001A4 RID: 420
		internal int _version;

		// Token: 0x0200004A RID: 74
		internal class Enumerator : IEnumerator, IDisposable
		{
			// Token: 0x0600033B RID: 827 RVA: 0x00011E07 File Offset: 0x00010007
			internal Enumerator(DataMap map)
			{
				this._map = map;
				this._version = map._version;
				this._current = null;
				this._index = -1;
			}

			// Token: 0x17000191 RID: 401
			// (get) Token: 0x0600033C RID: 828 RVA: 0x00011E30 File Offset: 0x00010030
			object IEnumerator.Current
			{
				get
				{
					if (this._index < 0 || this._index > this._map.Count)
					{
						throw new InvalidOperationException("Enumerator is positioned before the first element or after the last element of the collection.");
					}
					return this._current;
				}
			}

			// Token: 0x0600033D RID: 829 RVA: 0x00011E60 File Offset: 0x00010060
			public bool MoveNext()
			{
				if (this._version != this._map._version)
				{
					throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
				}
				if (this._index >= this._map.Count)
				{
					return false;
				}
				if (++this._index == 0)
				{
					this._current = this._map.FirstBlock;
				}
				else
				{
					this._current = this._current.NextBlock;
				}
				return this._index < this._map.Count;
			}

			// Token: 0x0600033E RID: 830 RVA: 0x00011EEA File Offset: 0x000100EA
			void IEnumerator.Reset()
			{
				if (this._version != this._map._version)
				{
					throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
				}
				this._index = -1;
				this._current = null;
			}

			// Token: 0x0600033F RID: 831 RVA: 0x00011F18 File Offset: 0x00010118
			public void Dispose()
			{
			}

			// Token: 0x040001A5 RID: 421
			private DataMap _map;

			// Token: 0x040001A6 RID: 422
			private DataBlock _current;

			// Token: 0x040001A7 RID: 423
			private int _index;

			// Token: 0x040001A8 RID: 424
			private int _version;
		}
	}
}
