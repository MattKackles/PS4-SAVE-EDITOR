using System;
using System.Globalization;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
	// Token: 0x02000019 RID: 25
	[XmlRoot("cheat")]
	public class cheat
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00009B78 File Offset: 0x00007D78
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00009B80 File Offset: 0x00007D80
		public string id
		{
			get;
			set;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00009B89 File Offset: 0x00007D89
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00009B91 File Offset: 0x00007D91
		public string name
		{
			get;
			set;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00009B9A File Offset: 0x00007D9A
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00009BA2 File Offset: 0x00007DA2
		public string note
		{
			get;
			set;
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00009BAB File Offset: 0x00007DAB
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00009BB3 File Offset: 0x00007DB3
		public bool Selected
		{
			get;
			set;
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00009BBC File Offset: 0x00007DBC
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00009BC4 File Offset: 0x00007DC4
		public string code
		{
			get;
			set;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00009BCD File Offset: 0x00007DCD
		public cheat()
		{
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00009BD5 File Offset: 0x00007DD5
		public cheat(string id, string description, string comment)
		{
			this.id = id;
			this.name = description;
			this.note = comment;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00009BF4 File Offset: 0x00007DF4
		public string ToEditableString()
		{
			string text = "";
			string[] array = this.code.Split(new char[]
			{
				' '
			});
			for (int i = 0; i < array.Length; i += 2)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					array[i],
					" ",
					array[i + 1],
					"\n"
				});
			}
			return text;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00009C6C File Offset: 0x00007E6C
		public string ToString(bool _protected = false)
		{
			string text = "";
			if (this.Selected)
			{
				if (_protected)
				{
					if (!string.IsNullOrEmpty(this.id))
					{
						text += string.Format("<id>{0}</id>", this.id);
					}
				}
				else
				{
					if (this.code != null)
					{
						return string.Format("<code>{0}</code>", this.code);
					}
					return string.Format("<id>{0}</id>", this.id);
				}
			}
			return text;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00009CDC File Offset: 0x00007EDC
		internal static cheat Copy(cheat cheat)
		{
			return new cheat
			{
				id = cheat.id,
				name = cheat.name,
				note = cheat.note,
				code = cheat.code
			};
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00009D20 File Offset: 0x00007F20
		internal static byte GetBitCodeBytes(int bitCode)
		{
			switch (bitCode)
			{
			case 0:
				return 1;
			case 1:
				return 2;
			case 2:
				return 4;
			default:
				return 4;
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00009D4C File Offset: 0x00007F4C
		internal static long GetMemLocation(string value, out int bitWriteCode)
		{
			long num;
			long.TryParse(value, NumberStyles.HexNumber, null, out num);
			long result = num & 268435455L;
			bitWriteCode = (int)(num >> 28);
			return result;
		}
	}
}
