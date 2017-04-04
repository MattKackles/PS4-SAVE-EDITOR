using System;

namespace Rss
{
	// Token: 0x0200008D RID: 141
	[Serializable]
	public struct DBBool
	{
		// Token: 0x060006BB RID: 1723 RVA: 0x000267AC File Offset: 0x000249AC
		private DBBool(int value)
		{
			this.value = (sbyte)value;
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x000267B6 File Offset: 0x000249B6
		public bool IsNull
		{
			get
			{
				return this.value == 0;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x000267C1 File Offset: 0x000249C1
		public bool IsFalse
		{
			get
			{
				return this.value < 0;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x000267CC File Offset: 0x000249CC
		public bool IsTrue
		{
			get
			{
				return this.value > 0;
			}
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000267D7 File Offset: 0x000249D7
		public static implicit operator DBBool(bool x)
		{
			if (!x)
			{
				return DBBool.False;
			}
			return DBBool.True;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000267E7 File Offset: 0x000249E7
		public static explicit operator bool(DBBool x)
		{
			if (x.value == 0)
			{
				throw new InvalidOperationException();
			}
			return x.value > 0;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00026802 File Offset: 0x00024A02
		public static DBBool operator ==(DBBool x, DBBool y)
		{
			if (x.value == 0 || y.value == 0)
			{
				return DBBool.Null;
			}
			if (x.value != y.value)
			{
				return DBBool.False;
			}
			return DBBool.True;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00026837 File Offset: 0x00024A37
		public static DBBool operator !=(DBBool x, DBBool y)
		{
			if (x.value == 0 || y.value == 0)
			{
				return DBBool.Null;
			}
			if (x.value == y.value)
			{
				return DBBool.False;
			}
			return DBBool.True;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0002686C File Offset: 0x00024A6C
		public static DBBool operator !(DBBool x)
		{
			return new DBBool((int)(-(int)x.value));
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0002687B File Offset: 0x00024A7B
		public static DBBool operator &(DBBool x, DBBool y)
		{
			return new DBBool((int)((x.value < y.value) ? x.value : y.value));
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x000268A2 File Offset: 0x00024AA2
		public static DBBool operator |(DBBool x, DBBool y)
		{
			return new DBBool((int)((x.value > y.value) ? x.value : y.value));
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x000268C9 File Offset: 0x00024AC9
		public static bool operator true(DBBool x)
		{
			return x.value > 0;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x000268D5 File Offset: 0x00024AD5
		public static bool operator false(DBBool x)
		{
			return x.value < 0;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000268E4 File Offset: 0x00024AE4
		public override bool Equals(object o)
		{
			bool result;
			try
			{
				result = (bool)(this == (DBBool)o);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00026920 File Offset: 0x00024B20
		public override int GetHashCode()
		{
			return (int)this.value;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00026928 File Offset: 0x00024B28
		public override string ToString()
		{
			switch (this.value)
			{
			case -1:
				return "false";
			case 0:
				return "DBBool.Null";
			case 1:
				return "true";
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0400031A RID: 794
		public static readonly DBBool Null = new DBBool(0);

		// Token: 0x0400031B RID: 795
		public static readonly DBBool False = new DBBool(-1);

		// Token: 0x0400031C RID: 796
		public static readonly DBBool True = new DBBool(1);

		// Token: 0x0400031D RID: 797
		private sbyte value;
	}
}
