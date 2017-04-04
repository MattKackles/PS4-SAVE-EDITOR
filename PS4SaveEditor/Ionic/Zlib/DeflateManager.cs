using System;

namespace Ionic.Zlib
{
	// Token: 0x02000116 RID: 278
	internal sealed class DeflateManager
	{
		// Token: 0x06000B96 RID: 2966 RVA: 0x00040140 File Offset: 0x0003E340
		internal DeflateManager()
		{
			this.dyn_ltree = new short[DeflateManager.HEAP_SIZE * 2];
			this.dyn_dtree = new short[(2 * InternalConstants.D_CODES + 1) * 2];
			this.bl_tree = new short[(2 * InternalConstants.BL_CODES + 1) * 2];
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x000401F4 File Offset: 0x0003E3F4
		private void _InitializeLazyMatch()
		{
			this.window_size = 2 * this.w_size;
			Array.Clear(this.head, 0, this.hash_size);
			this.config = DeflateManager.Config.Lookup(this.compressionLevel);
			this.SetDeflater();
			this.strstart = 0;
			this.block_start = 0;
			this.lookahead = 0;
			this.match_length = (this.prev_length = DeflateManager.MIN_MATCH - 1);
			this.match_available = 0;
			this.ins_h = 0;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00040274 File Offset: 0x0003E474
		private void _InitializeTreeData()
		{
			this.treeLiterals.dyn_tree = this.dyn_ltree;
			this.treeLiterals.staticTree = StaticTree.Literals;
			this.treeDistances.dyn_tree = this.dyn_dtree;
			this.treeDistances.staticTree = StaticTree.Distances;
			this.treeBitLengths.dyn_tree = this.bl_tree;
			this.treeBitLengths.staticTree = StaticTree.BitLengths;
			this.bi_buf = 0;
			this.bi_valid = 0;
			this.last_eob_len = 8;
			this._InitializeBlocks();
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00040300 File Offset: 0x0003E500
		internal void _InitializeBlocks()
		{
			for (int i = 0; i < InternalConstants.L_CODES; i++)
			{
				this.dyn_ltree[i * 2] = 0;
			}
			for (int j = 0; j < InternalConstants.D_CODES; j++)
			{
				this.dyn_dtree[j * 2] = 0;
			}
			for (int k = 0; k < InternalConstants.BL_CODES; k++)
			{
				this.bl_tree[k * 2] = 0;
			}
			this.dyn_ltree[DeflateManager.END_BLOCK * 2] = 1;
			this.opt_len = (this.static_len = 0);
			this.last_lit = (this.matches = 0);
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00040390 File Offset: 0x0003E590
		internal void pqdownheap(short[] tree, int k)
		{
			int num = this.heap[k];
			for (int i = k << 1; i <= this.heap_len; i <<= 1)
			{
				if (i < this.heap_len && DeflateManager._IsSmaller(tree, this.heap[i + 1], this.heap[i], this.depth))
				{
					i++;
				}
				if (DeflateManager._IsSmaller(tree, num, this.heap[i], this.depth))
				{
					break;
				}
				this.heap[k] = this.heap[i];
				k = i;
			}
			this.heap[k] = num;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0004041C File Offset: 0x0003E61C
		internal static bool _IsSmaller(short[] tree, int n, int m, sbyte[] depth)
		{
			short num = tree[n * 2];
			short num2 = tree[m * 2];
			return num < num2 || (num == num2 && depth[n] <= depth[m]);
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0004044C File Offset: 0x0003E64C
		internal void scan_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			tree[(max_code + 1) * 2 + 1] = 32767;
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						this.bl_tree[num6 * 2] = (short)((int)this.bl_tree[num6 * 2] + num3);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							short[] expr_87_cp_0 = this.bl_tree;
							int expr_87_cp_1 = num6 * 2;
							expr_87_cp_0[expr_87_cp_1] += 1;
						}
						short[] expr_A7_cp_0 = this.bl_tree;
						int expr_A7_cp_1 = InternalConstants.REP_3_6 * 2;
						expr_A7_cp_0[expr_A7_cp_1] += 1;
					}
					else if (num3 <= 10)
					{
						short[] expr_CF_cp_0 = this.bl_tree;
						int expr_CF_cp_1 = InternalConstants.REPZ_3_10 * 2;
						expr_CF_cp_0[expr_CF_cp_1] += 1;
					}
					else
					{
						short[] expr_F1_cp_0 = this.bl_tree;
						int expr_F1_cp_1 = InternalConstants.REPZ_11_138 * 2;
						expr_F1_cp_0[expr_F1_cp_1] += 1;
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0004058C File Offset: 0x0003E78C
		internal int build_bl_tree()
		{
			this.scan_tree(this.dyn_ltree, this.treeLiterals.max_code);
			this.scan_tree(this.dyn_dtree, this.treeDistances.max_code);
			this.treeBitLengths.build_tree(this);
			int num = InternalConstants.BL_CODES - 1;
			while (num >= 3 && this.bl_tree[(int)(Tree.bl_order[num] * 2 + 1)] == 0)
			{
				num--;
			}
			this.opt_len += 3 * (num + 1) + 5 + 5 + 4;
			return num;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00040614 File Offset: 0x0003E814
		internal void send_all_trees(int lcodes, int dcodes, int blcodes)
		{
			this.send_bits(lcodes - 257, 5);
			this.send_bits(dcodes - 1, 5);
			this.send_bits(blcodes - 4, 4);
			for (int i = 0; i < blcodes; i++)
			{
				this.send_bits((int)this.bl_tree[(int)(Tree.bl_order[i] * 2 + 1)], 3);
			}
			this.send_tree(this.dyn_ltree, lcodes - 1);
			this.send_tree(this.dyn_dtree, dcodes - 1);
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x00040688 File Offset: 0x0003E888
		internal void send_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						do
						{
							this.send_code(num6, this.bl_tree);
						}
						while (--num3 != 0);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							this.send_code(num6, this.bl_tree);
							num3--;
						}
						this.send_code(InternalConstants.REP_3_6, this.bl_tree);
						this.send_bits(num3 - 3, 2);
					}
					else if (num3 <= 10)
					{
						this.send_code(InternalConstants.REPZ_3_10, this.bl_tree);
						this.send_bits(num3 - 3, 3);
					}
					else
					{
						this.send_code(InternalConstants.REPZ_11_138, this.bl_tree);
						this.send_bits(num3 - 11, 7);
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0004079E File Offset: 0x0003E99E
		private void put_bytes(byte[] p, int start, int len)
		{
			Array.Copy(p, start, this.pending, this.pendingCount, len);
			this.pendingCount += len;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x000407C4 File Offset: 0x0003E9C4
		internal void send_code(int c, short[] tree)
		{
			int num = c * 2;
			this.send_bits((int)tree[num] & 65535, (int)tree[num + 1] & 65535);
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x000407F0 File Offset: 0x0003E9F0
		internal void send_bits(int value, int length)
		{
			if (this.bi_valid > DeflateManager.Buf_size - length)
			{
				this.bi_buf |= (short)(value << this.bi_valid & 65535);
				this.pending[this.pendingCount++] = (byte)this.bi_buf;
				this.pending[this.pendingCount++] = (byte)(this.bi_buf >> 8);
				this.bi_buf = (short)((uint)value >> DeflateManager.Buf_size - this.bi_valid);
				this.bi_valid += length - DeflateManager.Buf_size;
				return;
			}
			this.bi_buf |= (short)(value << this.bi_valid & 65535);
			this.bi_valid += length;
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x000408CC File Offset: 0x0003EACC
		internal void _tr_align()
		{
			this.send_bits(DeflateManager.STATIC_TREES << 1, 3);
			this.send_code(DeflateManager.END_BLOCK, StaticTree.lengthAndLiteralsTreeCodes);
			this.bi_flush();
			if (1 + this.last_eob_len + 10 - this.bi_valid < 9)
			{
				this.send_bits(DeflateManager.STATIC_TREES << 1, 3);
				this.send_code(DeflateManager.END_BLOCK, StaticTree.lengthAndLiteralsTreeCodes);
				this.bi_flush();
			}
			this.last_eob_len = 7;
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00040940 File Offset: 0x0003EB40
		internal bool _tr_tally(int dist, int lc)
		{
			this.pending[this._distanceOffset + this.last_lit * 2] = (byte)((uint)dist >> 8);
			this.pending[this._distanceOffset + this.last_lit * 2 + 1] = (byte)dist;
			this.pending[this._lengthOffset + this.last_lit] = (byte)lc;
			this.last_lit++;
			if (dist == 0)
			{
				short[] expr_69_cp_0 = this.dyn_ltree;
				int expr_69_cp_1 = lc * 2;
				expr_69_cp_0[expr_69_cp_1] += 1;
			}
			else
			{
				this.matches++;
				dist--;
				short[] expr_A8_cp_0 = this.dyn_ltree;
				int expr_A8_cp_1 = ((int)Tree.LengthCode[lc] + InternalConstants.LITERALS + 1) * 2;
				expr_A8_cp_0[expr_A8_cp_1] += 1;
				short[] expr_C9_cp_0 = this.dyn_dtree;
				int expr_C9_cp_1 = Tree.DistanceCode(dist) * 2;
				expr_C9_cp_0[expr_C9_cp_1] += 1;
			}
			if ((this.last_lit & 8191) == 0 && this.compressionLevel > CompressionLevel.Level2)
			{
				int num = this.last_lit << 3;
				int num2 = this.strstart - this.block_start;
				for (int i = 0; i < InternalConstants.D_CODES; i++)
				{
					num = (int)((long)num + (long)this.dyn_dtree[i * 2] * (5L + (long)Tree.ExtraDistanceBits[i]));
				}
				num >>= 3;
				if (this.matches < this.last_lit / 2 && num < num2 / 2)
				{
					return true;
				}
			}
			return this.last_lit == this.lit_bufsize - 1 || this.last_lit == this.lit_bufsize;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x00040ABC File Offset: 0x0003ECBC
		internal void send_compressed_block(short[] ltree, short[] dtree)
		{
			int num = 0;
			if (this.last_lit != 0)
			{
				do
				{
					int num2 = this._distanceOffset + num * 2;
					int num3 = ((int)this.pending[num2] << 8 & 65280) | (int)(this.pending[num2 + 1] & 255);
					int num4 = (int)(this.pending[this._lengthOffset + num] & 255);
					num++;
					if (num3 == 0)
					{
						this.send_code(num4, ltree);
					}
					else
					{
						int num5 = (int)Tree.LengthCode[num4];
						this.send_code(num5 + InternalConstants.LITERALS + 1, ltree);
						int num6 = Tree.ExtraLengthBits[num5];
						if (num6 != 0)
						{
							num4 -= Tree.LengthBase[num5];
							this.send_bits(num4, num6);
						}
						num3--;
						num5 = Tree.DistanceCode(num3);
						this.send_code(num5, dtree);
						num6 = Tree.ExtraDistanceBits[num5];
						if (num6 != 0)
						{
							num3 -= Tree.DistanceBase[num5];
							this.send_bits(num3, num6);
						}
					}
				}
				while (num < this.last_lit);
			}
			this.send_code(DeflateManager.END_BLOCK, ltree);
			this.last_eob_len = (int)ltree[DeflateManager.END_BLOCK * 2 + 1];
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00040BC4 File Offset: 0x0003EDC4
		internal void set_data_type()
		{
			int i = 0;
			int num = 0;
			int num2 = 0;
			while (i < 7)
			{
				num2 += (int)this.dyn_ltree[i * 2];
				i++;
			}
			while (i < 128)
			{
				num += (int)this.dyn_ltree[i * 2];
				i++;
			}
			while (i < InternalConstants.LITERALS)
			{
				num2 += (int)this.dyn_ltree[i * 2];
				i++;
			}
			this.data_type = (sbyte)((num2 > num >> 2) ? DeflateManager.Z_BINARY : DeflateManager.Z_ASCII);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00040C40 File Offset: 0x0003EE40
		internal void bi_flush()
		{
			if (this.bi_valid == 16)
			{
				this.pending[this.pendingCount++] = (byte)this.bi_buf;
				this.pending[this.pendingCount++] = (byte)(this.bi_buf >> 8);
				this.bi_buf = 0;
				this.bi_valid = 0;
				return;
			}
			if (this.bi_valid >= 8)
			{
				this.pending[this.pendingCount++] = (byte)this.bi_buf;
				this.bi_buf = (short)(this.bi_buf >> 8);
				this.bi_valid -= 8;
			}
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00040CEC File Offset: 0x0003EEEC
		internal void bi_windup()
		{
			if (this.bi_valid > 8)
			{
				this.pending[this.pendingCount++] = (byte)this.bi_buf;
				this.pending[this.pendingCount++] = (byte)(this.bi_buf >> 8);
			}
			else if (this.bi_valid > 0)
			{
				this.pending[this.pendingCount++] = (byte)this.bi_buf;
			}
			this.bi_buf = 0;
			this.bi_valid = 0;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00040D7C File Offset: 0x0003EF7C
		internal void copy_block(int buf, int len, bool header)
		{
			this.bi_windup();
			this.last_eob_len = 8;
			if (header)
			{
				this.pending[this.pendingCount++] = (byte)len;
				this.pending[this.pendingCount++] = (byte)(len >> 8);
				this.pending[this.pendingCount++] = (byte)(~(byte)len);
				this.pending[this.pendingCount++] = (byte)(~len >> 8);
			}
			this.put_bytes(this.window, buf, len);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00040E15 File Offset: 0x0003F015
		internal void flush_block_only(bool eof)
		{
			this._tr_flush_block((this.block_start >= 0) ? this.block_start : -1, this.strstart - this.block_start, eof);
			this.block_start = this.strstart;
			this._codec.flush_pending();
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x00040E54 File Offset: 0x0003F054
		internal BlockState DeflateNone(FlushType flush)
		{
			int num = 65535;
			if (num > this.pending.Length - 5)
			{
				num = this.pending.Length - 5;
			}
			while (true)
			{
				if (this.lookahead <= 1)
				{
					this._fillWindow();
					if (this.lookahead == 0 && flush == FlushType.None)
					{
						break;
					}
					if (this.lookahead == 0)
					{
						goto IL_DB;
					}
				}
				this.strstart += this.lookahead;
				this.lookahead = 0;
				int num2 = this.block_start + num;
				if (this.strstart == 0 || this.strstart >= num2)
				{
					this.lookahead = this.strstart - num2;
					this.strstart = num2;
					this.flush_block_only(false);
					if (this._codec.AvailableBytesOut == 0)
					{
						return BlockState.NeedMore;
					}
				}
				if (this.strstart - this.block_start >= this.w_size - DeflateManager.MIN_LOOKAHEAD)
				{
					this.flush_block_only(false);
					if (this._codec.AvailableBytesOut == 0)
					{
						return BlockState.NeedMore;
					}
				}
			}
			return BlockState.NeedMore;
			IL_DB:
			this.flush_block_only(flush == FlushType.Finish);
			if (this._codec.AvailableBytesOut == 0)
			{
				if (flush != FlushType.Finish)
				{
					return BlockState.NeedMore;
				}
				return BlockState.FinishStarted;
			}
			else
			{
				if (flush != FlushType.Finish)
				{
					return BlockState.BlockDone;
				}
				return BlockState.FinishDone;
			}
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00040F62 File Offset: 0x0003F162
		internal void _tr_stored_block(int buf, int stored_len, bool eof)
		{
			this.send_bits((DeflateManager.STORED_BLOCK << 1) + (eof ? 1 : 0), 3);
			this.copy_block(buf, stored_len, true);
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00040F84 File Offset: 0x0003F184
		internal void _tr_flush_block(int buf, int stored_len, bool eof)
		{
			int num = 0;
			int num2;
			int num3;
			if (this.compressionLevel > CompressionLevel.None)
			{
				if ((int)this.data_type == DeflateManager.Z_UNKNOWN)
				{
					this.set_data_type();
				}
				this.treeLiterals.build_tree(this);
				this.treeDistances.build_tree(this);
				num = this.build_bl_tree();
				num2 = this.opt_len + 3 + 7 >> 3;
				num3 = this.static_len + 3 + 7 >> 3;
				if (num3 <= num2)
				{
					num2 = num3;
				}
			}
			else
			{
				num3 = (num2 = stored_len + 5);
			}
			if (stored_len + 4 <= num2 && buf != -1)
			{
				this._tr_stored_block(buf, stored_len, eof);
			}
			else if (num3 == num2)
			{
				this.send_bits((DeflateManager.STATIC_TREES << 1) + (eof ? 1 : 0), 3);
				this.send_compressed_block(StaticTree.lengthAndLiteralsTreeCodes, StaticTree.distTreeCodes);
			}
			else
			{
				this.send_bits((DeflateManager.DYN_TREES << 1) + (eof ? 1 : 0), 3);
				this.send_all_trees(this.treeLiterals.max_code + 1, this.treeDistances.max_code + 1, num + 1);
				this.send_compressed_block(this.dyn_ltree, this.dyn_dtree);
			}
			this._InitializeBlocks();
			if (eof)
			{
				this.bi_windup();
			}
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x00041094 File Offset: 0x0003F294
		private void _fillWindow()
		{
			while (true)
			{
				int num = this.window_size - this.lookahead - this.strstart;
				int num2;
				if (num == 0 && this.strstart == 0 && this.lookahead == 0)
				{
					num = this.w_size;
				}
				else if (num == -1)
				{
					num--;
				}
				else if (this.strstart >= this.w_size + this.w_size - DeflateManager.MIN_LOOKAHEAD)
				{
					Array.Copy(this.window, this.w_size, this.window, 0, this.w_size);
					this.match_start -= this.w_size;
					this.strstart -= this.w_size;
					this.block_start -= this.w_size;
					num2 = this.hash_size;
					int num3 = num2;
					do
					{
						int num4 = (int)this.head[--num3] & 65535;
						this.head[num3] = (short)((num4 >= this.w_size) ? (num4 - this.w_size) : 0);
					}
					while (--num2 != 0);
					num2 = this.w_size;
					num3 = num2;
					do
					{
						int num4 = (int)this.prev[--num3] & 65535;
						this.prev[num3] = (short)((num4 >= this.w_size) ? (num4 - this.w_size) : 0);
					}
					while (--num2 != 0);
					num += this.w_size;
				}
				if (this._codec.AvailableBytesIn == 0)
				{
					break;
				}
				num2 = this._codec.read_buf(this.window, this.strstart + this.lookahead, num);
				this.lookahead += num2;
				if (this.lookahead >= DeflateManager.MIN_MATCH)
				{
					this.ins_h = (int)(this.window[this.strstart] & 255);
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 1] & 255)) & this.hash_mask);
				}
				if (this.lookahead >= DeflateManager.MIN_LOOKAHEAD || this._codec.AvailableBytesIn == 0)
				{
					return;
				}
			}
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x00041294 File Offset: 0x0003F494
		internal BlockState DeflateFast(FlushType flush)
		{
			int num = 0;
			while (true)
			{
				if (this.lookahead < DeflateManager.MIN_LOOKAHEAD)
				{
					this._fillWindow();
					if (this.lookahead < DeflateManager.MIN_LOOKAHEAD && flush == FlushType.None)
					{
						break;
					}
					if (this.lookahead == 0)
					{
						goto IL_2EA;
					}
				}
				if (this.lookahead >= DeflateManager.MIN_MATCH)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & 255)) & this.hash_mask);
					num = ((int)this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				if ((long)num != 0L && (this.strstart - num & 65535) <= this.w_size - DeflateManager.MIN_LOOKAHEAD && this.compressionStrategy != CompressionStrategy.HuffmanOnly)
				{
					this.match_length = this.longest_match(num);
				}
				bool flag;
				if (this.match_length >= DeflateManager.MIN_MATCH)
				{
					flag = this._tr_tally(this.strstart - this.match_start, this.match_length - DeflateManager.MIN_MATCH);
					this.lookahead -= this.match_length;
					if (this.match_length <= this.config.MaxLazy && this.lookahead >= DeflateManager.MIN_MATCH)
					{
						this.match_length--;
						do
						{
							this.strstart++;
							this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & 255)) & this.hash_mask);
							num = ((int)this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
						}
						while (--this.match_length != 0);
						this.strstart++;
					}
					else
					{
						this.strstart += this.match_length;
						this.match_length = 0;
						this.ins_h = (int)(this.window[this.strstart] & 255);
						this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 1] & 255)) & this.hash_mask);
					}
				}
				else
				{
					flag = this._tr_tally(0, (int)(this.window[this.strstart] & 255));
					this.lookahead--;
					this.strstart++;
				}
				if (flag)
				{
					this.flush_block_only(false);
					if (this._codec.AvailableBytesOut == 0)
					{
						return BlockState.NeedMore;
					}
				}
			}
			return BlockState.NeedMore;
			IL_2EA:
			this.flush_block_only(flush == FlushType.Finish);
			if (this._codec.AvailableBytesOut == 0)
			{
				if (flush == FlushType.Finish)
				{
					return BlockState.FinishStarted;
				}
				return BlockState.NeedMore;
			}
			else
			{
				if (flush != FlushType.Finish)
				{
					return BlockState.BlockDone;
				}
				return BlockState.FinishDone;
			}
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x000415B4 File Offset: 0x0003F7B4
		internal BlockState DeflateSlow(FlushType flush)
		{
			int num = 0;
			while (true)
			{
				if (this.lookahead < DeflateManager.MIN_LOOKAHEAD)
				{
					this._fillWindow();
					if (this.lookahead < DeflateManager.MIN_LOOKAHEAD && flush == FlushType.None)
					{
						break;
					}
					if (this.lookahead == 0)
					{
						goto IL_365;
					}
				}
				if (this.lookahead >= DeflateManager.MIN_MATCH)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & 255)) & this.hash_mask);
					num = ((int)this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				this.prev_length = this.match_length;
				this.prev_match = this.match_start;
				this.match_length = DeflateManager.MIN_MATCH - 1;
				if (num != 0 && this.prev_length < this.config.MaxLazy && (this.strstart - num & 65535) <= this.w_size - DeflateManager.MIN_LOOKAHEAD)
				{
					if (this.compressionStrategy != CompressionStrategy.HuffmanOnly)
					{
						this.match_length = this.longest_match(num);
					}
					if (this.match_length <= 5 && (this.compressionStrategy == CompressionStrategy.Filtered || (this.match_length == DeflateManager.MIN_MATCH && this.strstart - this.match_start > 4096)))
					{
						this.match_length = DeflateManager.MIN_MATCH - 1;
					}
				}
				if (this.prev_length >= DeflateManager.MIN_MATCH && this.match_length <= this.prev_length)
				{
					int num2 = this.strstart + this.lookahead - DeflateManager.MIN_MATCH;
					bool flag = this._tr_tally(this.strstart - 1 - this.prev_match, this.prev_length - DeflateManager.MIN_MATCH);
					this.lookahead -= this.prev_length - 1;
					this.prev_length -= 2;
					do
					{
						if (++this.strstart <= num2)
						{
							this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & 255)) & this.hash_mask);
							num = ((int)this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
						}
					}
					while (--this.prev_length != 0);
					this.match_available = 0;
					this.match_length = DeflateManager.MIN_MATCH - 1;
					this.strstart++;
					if (flag)
					{
						this.flush_block_only(false);
						if (this._codec.AvailableBytesOut == 0)
						{
							return BlockState.NeedMore;
						}
					}
				}
				else if (this.match_available != 0)
				{
					bool flag = this._tr_tally(0, (int)(this.window[this.strstart - 1] & 255));
					if (flag)
					{
						this.flush_block_only(false);
					}
					this.strstart++;
					this.lookahead--;
					if (this._codec.AvailableBytesOut == 0)
					{
						return BlockState.NeedMore;
					}
				}
				else
				{
					this.match_available = 1;
					this.strstart++;
					this.lookahead--;
				}
			}
			return BlockState.NeedMore;
			IL_365:
			if (this.match_available != 0)
			{
				bool flag = this._tr_tally(0, (int)(this.window[this.strstart - 1] & 255));
				this.match_available = 0;
			}
			this.flush_block_only(flush == FlushType.Finish);
			if (this._codec.AvailableBytesOut == 0)
			{
				if (flush == FlushType.Finish)
				{
					return BlockState.FinishStarted;
				}
				return BlockState.NeedMore;
			}
			else
			{
				if (flush != FlushType.Finish)
				{
					return BlockState.BlockDone;
				}
				return BlockState.FinishDone;
			}
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00041978 File Offset: 0x0003FB78
		internal int longest_match(int cur_match)
		{
			int num = this.config.MaxChainLength;
			int num2 = this.strstart;
			int num3 = this.prev_length;
			int num4 = (this.strstart > this.w_size - DeflateManager.MIN_LOOKAHEAD) ? (this.strstart - (this.w_size - DeflateManager.MIN_LOOKAHEAD)) : 0;
			int niceLength = this.config.NiceLength;
			int num5 = this.w_mask;
			int num6 = this.strstart + DeflateManager.MAX_MATCH;
			byte b = this.window[num2 + num3 - 1];
			byte b2 = this.window[num2 + num3];
			if (this.prev_length >= this.config.GoodLength)
			{
				num >>= 2;
			}
			if (niceLength > this.lookahead)
			{
				niceLength = this.lookahead;
			}
			do
			{
				int num7 = cur_match;
				if (this.window[num7 + num3] == b2 && this.window[num7 + num3 - 1] == b && this.window[num7] == this.window[num2] && this.window[++num7] == this.window[num2 + 1])
				{
					num2 += 2;
					num7++;
					while (this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && num2 < num6)
					{
					}
					int num8 = DeflateManager.MAX_MATCH - (num6 - num2);
					num2 = num6 - DeflateManager.MAX_MATCH;
					if (num8 > num3)
					{
						this.match_start = cur_match;
						num3 = num8;
						if (num8 >= niceLength)
						{
							break;
						}
						b = this.window[num2 + num3 - 1];
						b2 = this.window[num2 + num3];
					}
				}
			}
			while ((cur_match = ((int)this.prev[cur_match & num5] & 65535)) > num4 && --num != 0);
			if (num3 <= this.lookahead)
			{
				return num3;
			}
			return this.lookahead;
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x00041BEE File Offset: 0x0003FDEE
		// (set) Token: 0x06000BB3 RID: 2995 RVA: 0x00041BF6 File Offset: 0x0003FDF6
		internal bool WantRfc1950HeaderBytes
		{
			get
			{
				return this._WantRfc1950HeaderBytes;
			}
			set
			{
				this._WantRfc1950HeaderBytes = value;
			}
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00041BFF File Offset: 0x0003FDFF
		internal int Initialize(ZlibCodec codec, CompressionLevel level)
		{
			return this.Initialize(codec, level, 15);
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00041C0B File Offset: 0x0003FE0B
		internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits)
		{
			return this.Initialize(codec, level, bits, DeflateManager.MEM_LEVEL_DEFAULT, CompressionStrategy.Default);
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00041C1C File Offset: 0x0003FE1C
		internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits, CompressionStrategy compressionStrategy)
		{
			return this.Initialize(codec, level, bits, DeflateManager.MEM_LEVEL_DEFAULT, compressionStrategy);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00041C30 File Offset: 0x0003FE30
		internal int Initialize(ZlibCodec codec, CompressionLevel level, int windowBits, int memLevel, CompressionStrategy strategy)
		{
			this._codec = codec;
			this._codec.Message = null;
			if (windowBits < 9 || windowBits > 15)
			{
				throw new ZlibException("windowBits must be in the range 9..15.");
			}
			if (memLevel < 1 || memLevel > DeflateManager.MEM_LEVEL_MAX)
			{
				throw new ZlibException(string.Format("memLevel must be in the range 1.. {0}", DeflateManager.MEM_LEVEL_MAX));
			}
			this._codec.dstate = this;
			this.w_bits = windowBits;
			this.w_size = 1 << this.w_bits;
			this.w_mask = this.w_size - 1;
			this.hash_bits = memLevel + 7;
			this.hash_size = 1 << this.hash_bits;
			this.hash_mask = this.hash_size - 1;
			this.hash_shift = (this.hash_bits + DeflateManager.MIN_MATCH - 1) / DeflateManager.MIN_MATCH;
			this.window = new byte[this.w_size * 2];
			this.prev = new short[this.w_size];
			this.head = new short[this.hash_size];
			this.lit_bufsize = 1 << memLevel + 6;
			this.pending = new byte[this.lit_bufsize * 4];
			this._distanceOffset = this.lit_bufsize;
			this._lengthOffset = 3 * this.lit_bufsize;
			this.compressionLevel = level;
			this.compressionStrategy = strategy;
			this.Reset();
			return 0;
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00041D8C File Offset: 0x0003FF8C
		internal void Reset()
		{
			this._codec.TotalBytesIn = (this._codec.TotalBytesOut = 0L);
			this._codec.Message = null;
			this.pendingCount = 0;
			this.nextPending = 0;
			this.Rfc1950BytesEmitted = false;
			this.status = (this.WantRfc1950HeaderBytes ? DeflateManager.INIT_STATE : DeflateManager.BUSY_STATE);
			this._codec._Adler32 = Adler.Adler32(0u, null, 0, 0);
			this.last_flush = 0;
			this._InitializeTreeData();
			this._InitializeLazyMatch();
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00041E18 File Offset: 0x00040018
		internal int End()
		{
			if (this.status != DeflateManager.INIT_STATE && this.status != DeflateManager.BUSY_STATE && this.status != DeflateManager.FINISH_STATE)
			{
				return -2;
			}
			this.pending = null;
			this.head = null;
			this.prev = null;
			this.window = null;
			if (this.status != DeflateManager.BUSY_STATE)
			{
				return 0;
			}
			return -3;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x00041E7C File Offset: 0x0004007C
		private void SetDeflater()
		{
			switch (this.config.Flavor)
			{
			case DeflateFlavor.Store:
				this.DeflateFunction = new DeflateManager.CompressFunc(this.DeflateNone);
				return;
			case DeflateFlavor.Fast:
				this.DeflateFunction = new DeflateManager.CompressFunc(this.DeflateFast);
				return;
			case DeflateFlavor.Slow:
				this.DeflateFunction = new DeflateManager.CompressFunc(this.DeflateSlow);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00041EE0 File Offset: 0x000400E0
		internal int SetParams(CompressionLevel level, CompressionStrategy strategy)
		{
			int result = 0;
			if (this.compressionLevel != level)
			{
				DeflateManager.Config config = DeflateManager.Config.Lookup(level);
				if (config.Flavor != this.config.Flavor && this._codec.TotalBytesIn != 0L)
				{
					result = this._codec.Deflate(FlushType.Partial);
				}
				this.compressionLevel = level;
				this.config = config;
				this.SetDeflater();
			}
			this.compressionStrategy = strategy;
			return result;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00041F4C File Offset: 0x0004014C
		internal int SetDictionary(byte[] dictionary)
		{
			int num = dictionary.Length;
			int sourceIndex = 0;
			if (dictionary == null || this.status != DeflateManager.INIT_STATE)
			{
				throw new ZlibException("Stream error.");
			}
			this._codec._Adler32 = Adler.Adler32(this._codec._Adler32, dictionary, 0, dictionary.Length);
			if (num < DeflateManager.MIN_MATCH)
			{
				return 0;
			}
			if (num > this.w_size - DeflateManager.MIN_LOOKAHEAD)
			{
				num = this.w_size - DeflateManager.MIN_LOOKAHEAD;
				sourceIndex = dictionary.Length - num;
			}
			Array.Copy(dictionary, sourceIndex, this.window, 0, num);
			this.strstart = num;
			this.block_start = num;
			this.ins_h = (int)(this.window[0] & 255);
			this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[1] & 255)) & this.hash_mask);
			for (int i = 0; i <= num - DeflateManager.MIN_MATCH; i++)
			{
				this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[i + (DeflateManager.MIN_MATCH - 1)] & 255)) & this.hash_mask);
				this.prev[i & this.w_mask] = this.head[this.ins_h];
				this.head[this.ins_h] = (short)i;
			}
			return 0;
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00042098 File Offset: 0x00040298
		internal int Deflate(FlushType flush)
		{
			if (this._codec.OutputBuffer == null || (this._codec.InputBuffer == null && this._codec.AvailableBytesIn != 0) || (this.status == DeflateManager.FINISH_STATE && flush != FlushType.Finish))
			{
				this._codec.Message = DeflateManager._ErrorMessage[4];
				throw new ZlibException(string.Format("Something is fishy. [{0}]", this._codec.Message));
			}
			if (this._codec.AvailableBytesOut == 0)
			{
				this._codec.Message = DeflateManager._ErrorMessage[7];
				throw new ZlibException("OutputBuffer is full (AvailableBytesOut == 0)");
			}
			int num = this.last_flush;
			this.last_flush = (int)flush;
			if (this.status == DeflateManager.INIT_STATE)
			{
				int num2 = DeflateManager.Z_DEFLATED + (this.w_bits - 8 << 4) << 8;
				int num3 = (this.compressionLevel - CompressionLevel.BestSpeed & 255) >> 1;
				if (num3 > 3)
				{
					num3 = 3;
				}
				num2 |= num3 << 6;
				if (this.strstart != 0)
				{
					num2 |= DeflateManager.PRESET_DICT;
				}
				num2 += 31 - num2 % 31;
				this.status = DeflateManager.BUSY_STATE;
				this.pending[this.pendingCount++] = (byte)(num2 >> 8);
				this.pending[this.pendingCount++] = (byte)num2;
				if (this.strstart != 0)
				{
					this.pending[this.pendingCount++] = (byte)((this._codec._Adler32 & 4278190080u) >> 24);
					this.pending[this.pendingCount++] = (byte)((this._codec._Adler32 & 16711680u) >> 16);
					this.pending[this.pendingCount++] = (byte)((this._codec._Adler32 & 65280u) >> 8);
					this.pending[this.pendingCount++] = (byte)(this._codec._Adler32 & 255u);
				}
				this._codec._Adler32 = Adler.Adler32(0u, null, 0, 0);
			}
			if (this.pendingCount != 0)
			{
				this._codec.flush_pending();
				if (this._codec.AvailableBytesOut == 0)
				{
					this.last_flush = -1;
					return 0;
				}
			}
			else if (this._codec.AvailableBytesIn == 0 && flush <= (FlushType)num && flush != FlushType.Finish)
			{
				return 0;
			}
			if (this.status == DeflateManager.FINISH_STATE && this._codec.AvailableBytesIn != 0)
			{
				this._codec.Message = DeflateManager._ErrorMessage[7];
				throw new ZlibException("status == FINISH_STATE && _codec.AvailableBytesIn != 0");
			}
			if (this._codec.AvailableBytesIn != 0 || this.lookahead != 0 || (flush != FlushType.None && this.status != DeflateManager.FINISH_STATE))
			{
				BlockState blockState = this.DeflateFunction(flush);
				if (blockState == BlockState.FinishStarted || blockState == BlockState.FinishDone)
				{
					this.status = DeflateManager.FINISH_STATE;
				}
				if (blockState == BlockState.NeedMore || blockState == BlockState.FinishStarted)
				{
					if (this._codec.AvailableBytesOut == 0)
					{
						this.last_flush = -1;
					}
					return 0;
				}
				if (blockState == BlockState.BlockDone)
				{
					if (flush == FlushType.Partial)
					{
						this._tr_align();
					}
					else
					{
						this._tr_stored_block(0, 0, false);
						if (flush == FlushType.Full)
						{
							for (int i = 0; i < this.hash_size; i++)
							{
								this.head[i] = 0;
							}
						}
					}
					this._codec.flush_pending();
					if (this._codec.AvailableBytesOut == 0)
					{
						this.last_flush = -1;
						return 0;
					}
				}
			}
			if (flush != FlushType.Finish)
			{
				return 0;
			}
			if (!this.WantRfc1950HeaderBytes || this.Rfc1950BytesEmitted)
			{
				return 1;
			}
			this.pending[this.pendingCount++] = (byte)((this._codec._Adler32 & 4278190080u) >> 24);
			this.pending[this.pendingCount++] = (byte)((this._codec._Adler32 & 16711680u) >> 16);
			this.pending[this.pendingCount++] = (byte)((this._codec._Adler32 & 65280u) >> 8);
			this.pending[this.pendingCount++] = (byte)(this._codec._Adler32 & 255u);
			this._codec.flush_pending();
			this.Rfc1950BytesEmitted = true;
			if (this.pendingCount == 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x040005DB RID: 1499
		private static readonly int MEM_LEVEL_MAX = 9;

		// Token: 0x040005DC RID: 1500
		private static readonly int MEM_LEVEL_DEFAULT = 8;

		// Token: 0x040005DD RID: 1501
		private DeflateManager.CompressFunc DeflateFunction;

		// Token: 0x040005DE RID: 1502
		private static readonly string[] _ErrorMessage = new string[]
		{
			"need dictionary",
			"stream end",
			"",
			"file error",
			"stream error",
			"data error",
			"insufficient memory",
			"buffer error",
			"incompatible version",
			""
		};

		// Token: 0x040005DF RID: 1503
		private static readonly int PRESET_DICT = 32;

		// Token: 0x040005E0 RID: 1504
		private static readonly int INIT_STATE = 42;

		// Token: 0x040005E1 RID: 1505
		private static readonly int BUSY_STATE = 113;

		// Token: 0x040005E2 RID: 1506
		private static readonly int FINISH_STATE = 666;

		// Token: 0x040005E3 RID: 1507
		private static readonly int Z_DEFLATED = 8;

		// Token: 0x040005E4 RID: 1508
		private static readonly int STORED_BLOCK = 0;

		// Token: 0x040005E5 RID: 1509
		private static readonly int STATIC_TREES = 1;

		// Token: 0x040005E6 RID: 1510
		private static readonly int DYN_TREES = 2;

		// Token: 0x040005E7 RID: 1511
		private static readonly int Z_BINARY = 0;

		// Token: 0x040005E8 RID: 1512
		private static readonly int Z_ASCII = 1;

		// Token: 0x040005E9 RID: 1513
		private static readonly int Z_UNKNOWN = 2;

		// Token: 0x040005EA RID: 1514
		private static readonly int Buf_size = 16;

		// Token: 0x040005EB RID: 1515
		private static readonly int MIN_MATCH = 3;

		// Token: 0x040005EC RID: 1516
		private static readonly int MAX_MATCH = 258;

		// Token: 0x040005ED RID: 1517
		private static readonly int MIN_LOOKAHEAD = DeflateManager.MAX_MATCH + DeflateManager.MIN_MATCH + 1;

		// Token: 0x040005EE RID: 1518
		private static readonly int HEAP_SIZE = 2 * InternalConstants.L_CODES + 1;

		// Token: 0x040005EF RID: 1519
		private static readonly int END_BLOCK = 256;

		// Token: 0x040005F0 RID: 1520
		internal ZlibCodec _codec;

		// Token: 0x040005F1 RID: 1521
		internal int status;

		// Token: 0x040005F2 RID: 1522
		internal byte[] pending;

		// Token: 0x040005F3 RID: 1523
		internal int nextPending;

		// Token: 0x040005F4 RID: 1524
		internal int pendingCount;

		// Token: 0x040005F5 RID: 1525
		internal sbyte data_type;

		// Token: 0x040005F6 RID: 1526
		internal int last_flush;

		// Token: 0x040005F7 RID: 1527
		internal int w_size;

		// Token: 0x040005F8 RID: 1528
		internal int w_bits;

		// Token: 0x040005F9 RID: 1529
		internal int w_mask;

		// Token: 0x040005FA RID: 1530
		internal byte[] window;

		// Token: 0x040005FB RID: 1531
		internal int window_size;

		// Token: 0x040005FC RID: 1532
		internal short[] prev;

		// Token: 0x040005FD RID: 1533
		internal short[] head;

		// Token: 0x040005FE RID: 1534
		internal int ins_h;

		// Token: 0x040005FF RID: 1535
		internal int hash_size;

		// Token: 0x04000600 RID: 1536
		internal int hash_bits;

		// Token: 0x04000601 RID: 1537
		internal int hash_mask;

		// Token: 0x04000602 RID: 1538
		internal int hash_shift;

		// Token: 0x04000603 RID: 1539
		internal int block_start;

		// Token: 0x04000604 RID: 1540
		private DeflateManager.Config config;

		// Token: 0x04000605 RID: 1541
		internal int match_length;

		// Token: 0x04000606 RID: 1542
		internal int prev_match;

		// Token: 0x04000607 RID: 1543
		internal int match_available;

		// Token: 0x04000608 RID: 1544
		internal int strstart;

		// Token: 0x04000609 RID: 1545
		internal int match_start;

		// Token: 0x0400060A RID: 1546
		internal int lookahead;

		// Token: 0x0400060B RID: 1547
		internal int prev_length;

		// Token: 0x0400060C RID: 1548
		internal CompressionLevel compressionLevel;

		// Token: 0x0400060D RID: 1549
		internal CompressionStrategy compressionStrategy;

		// Token: 0x0400060E RID: 1550
		internal short[] dyn_ltree;

		// Token: 0x0400060F RID: 1551
		internal short[] dyn_dtree;

		// Token: 0x04000610 RID: 1552
		internal short[] bl_tree;

		// Token: 0x04000611 RID: 1553
		internal Tree treeLiterals = new Tree();

		// Token: 0x04000612 RID: 1554
		internal Tree treeDistances = new Tree();

		// Token: 0x04000613 RID: 1555
		internal Tree treeBitLengths = new Tree();

		// Token: 0x04000614 RID: 1556
		internal short[] bl_count = new short[InternalConstants.MAX_BITS + 1];

		// Token: 0x04000615 RID: 1557
		internal int[] heap = new int[2 * InternalConstants.L_CODES + 1];

		// Token: 0x04000616 RID: 1558
		internal int heap_len;

		// Token: 0x04000617 RID: 1559
		internal int heap_max;

		// Token: 0x04000618 RID: 1560
		internal sbyte[] depth = new sbyte[2 * InternalConstants.L_CODES + 1];

		// Token: 0x04000619 RID: 1561
		internal int _lengthOffset;

		// Token: 0x0400061A RID: 1562
		internal int lit_bufsize;

		// Token: 0x0400061B RID: 1563
		internal int last_lit;

		// Token: 0x0400061C RID: 1564
		internal int _distanceOffset;

		// Token: 0x0400061D RID: 1565
		internal int opt_len;

		// Token: 0x0400061E RID: 1566
		internal int static_len;

		// Token: 0x0400061F RID: 1567
		internal int matches;

		// Token: 0x04000620 RID: 1568
		internal int last_eob_len;

		// Token: 0x04000621 RID: 1569
		internal short bi_buf;

		// Token: 0x04000622 RID: 1570
		internal int bi_valid;

		// Token: 0x04000623 RID: 1571
		private bool Rfc1950BytesEmitted;

		// Token: 0x04000624 RID: 1572
		private bool _WantRfc1950HeaderBytes = true;

		// Token: 0x02000117 RID: 279
		// (Invoke) Token: 0x06000BC0 RID: 3008
		internal delegate BlockState CompressFunc(FlushType flush);

		// Token: 0x02000118 RID: 280
		internal class Config
		{
			// Token: 0x06000BC3 RID: 3011 RVA: 0x000425E7 File Offset: 0x000407E7
			private Config(int goodLength, int maxLazy, int niceLength, int maxChainLength, DeflateFlavor flavor)
			{
				this.GoodLength = goodLength;
				this.MaxLazy = maxLazy;
				this.NiceLength = niceLength;
				this.MaxChainLength = maxChainLength;
				this.Flavor = flavor;
			}

			// Token: 0x06000BC4 RID: 3012 RVA: 0x00042614 File Offset: 0x00040814
			public static DeflateManager.Config Lookup(CompressionLevel level)
			{
				return DeflateManager.Config.Table[(int)level];
			}

			// Token: 0x04000625 RID: 1573
			internal int GoodLength;

			// Token: 0x04000626 RID: 1574
			internal int MaxLazy;

			// Token: 0x04000627 RID: 1575
			internal int NiceLength;

			// Token: 0x04000628 RID: 1576
			internal int MaxChainLength;

			// Token: 0x04000629 RID: 1577
			internal DeflateFlavor Flavor;

			// Token: 0x0400062A RID: 1578
			private static readonly DeflateManager.Config[] Table = new DeflateManager.Config[]
			{
				new DeflateManager.Config(0, 0, 0, 0, DeflateFlavor.Store),
				new DeflateManager.Config(4, 4, 8, 4, DeflateFlavor.Fast),
				new DeflateManager.Config(4, 5, 16, 8, DeflateFlavor.Fast),
				new DeflateManager.Config(4, 6, 32, 32, DeflateFlavor.Fast),
				new DeflateManager.Config(4, 4, 16, 16, DeflateFlavor.Slow),
				new DeflateManager.Config(8, 16, 32, 32, DeflateFlavor.Slow),
				new DeflateManager.Config(8, 16, 128, 128, DeflateFlavor.Slow),
				new DeflateManager.Config(8, 32, 128, 256, DeflateFlavor.Slow),
				new DeflateManager.Config(32, 128, 258, 1024, DeflateFlavor.Slow),
				new DeflateManager.Config(32, 258, 258, 4096, DeflateFlavor.Slow)
			};
		}
	}
}
