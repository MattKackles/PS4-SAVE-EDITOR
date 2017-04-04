using System;
using System.ComponentModel;
using System.Reflection;

namespace Ionic
{
	// Token: 0x0200013A RID: 314
	internal sealed class EnumUtil
	{
		// Token: 0x06000C75 RID: 3189 RVA: 0x00044876 File Offset: 0x00042A76
		private EnumUtil()
		{
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00044880 File Offset: 0x00042A80
		internal static string GetDescription(Enum value)
		{
			FieldInfo field = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] array = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (array.Length > 0)
			{
				return array[0].Description;
			}
			return value.ToString();
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x000448CB File Offset: 0x00042ACB
		internal static object Parse(Type enumType, string stringRepresentation)
		{
			return EnumUtil.Parse(enumType, stringRepresentation, false);
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x000448D8 File Offset: 0x00042AD8
		internal static object Parse(Type enumType, string stringRepresentation, bool ignoreCase)
		{
			if (ignoreCase)
			{
				stringRepresentation = stringRepresentation.ToLower();
			}
			foreach (Enum @enum in Enum.GetValues(enumType))
			{
				string text = EnumUtil.GetDescription(@enum);
				if (ignoreCase)
				{
					text = text.ToLower();
				}
				if (text == stringRepresentation)
				{
					return @enum;
				}
			}
			return Enum.Parse(enumType, stringRepresentation, ignoreCase);
		}
	}
}
