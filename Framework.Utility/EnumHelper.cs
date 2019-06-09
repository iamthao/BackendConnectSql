using System;
using System.ComponentModel;

namespace Framework.Utility
{
    public static class EnumHelper
    {
        public static string GetNameByValue<TEnum>(this int value) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enum.");
            }
            return Enum.GetName(typeof(TEnum), value);
        }

        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null)
                return "";
            var attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}