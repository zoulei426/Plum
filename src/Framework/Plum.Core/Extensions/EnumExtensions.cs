using Plum.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Extensions
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtensions
    {
        private static Dictionary<Enum, string> enumStringValueDic = new Dictionary<Enum, string>();
        private static Dictionary<Enum, string> enumDisplayNameDic = new Dictionary<Enum, string>();

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <param name="enum">The enum.</param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum @enum)
        {
            if (@enum is null) return string.Empty;
            var display = enumDisplayNameDic.GetValueOrDefault(@enum);
            if (!display.IsNullOrEmpty())
                return display;

            display = @enum.GetCustomAttribute<DescriptionAttribute>()?.Description;
            //if (display.IsNullOrEmpty())
            //    display = @enum.GetCustomAttribute<EnumNameAttribute>()?.Description;
            if (display.IsNullOrEmpty())
                display = @enum.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
            if (display.IsNullOrEmpty())
                display = @enum.ToString();

            enumDisplayNameDic.Add(@enum, display);

            return display;
        }

        public static string GetStringValue(this Enum @enum)
        {
            if (@enum is null) return string.Empty;
            var value = enumStringValueDic.GetValueOrDefault(@enum);
            if (!value.IsNullOrEmpty())
                return value;

            value = @enum.GetCustomAttribute<StringValueAttribute>()?.Value;
            if (value.IsNullOrEmpty())
                value = ((int)Enum.Parse(@enum.GetType(), @enum.ToString())).ToString();

            enumStringValueDic.AddIfNotContainsKey(@enum, value);

            return value;
        }

        /// <summary>
        /// Gets the enum value.
        /// </summary>
        /// <param name="enum">The enum.</param>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">转换枚举{@enum}失败</exception>
        public static Enum GetEnumValue(this string @enum, Type enumType)
        {
            if (@enum is null) return default;

            var value = string.Empty;
            foreach (var field in enumType.GetFields())
            {
                var display = field.GetCustomAttribute<DescriptionAttribute>()?.Description;
                //if (display.IsNullOrEmpty())
                //    display = field.GetCustomAttribute<EnumNameAttribute>()?.Description;
                if (display.IsNullOrEmpty())
                    display = field.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                if (@enum.Equals(display))
                {
                    value = field.Name;
                    break;
                }
            }

            if (value.IsNullOrEmpty())
                throw new ArgumentException($"转换枚举{@enum}失败");

            return Enum.Parse(enumType, value) as Enum;
        }

        ///// <summary>
        ///// 格式化枚举值
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static string Format(this Enum value, int digit = 2)
        //{
        //    return value.ToInt32().ToString().PadLeft(digit, '0');
        //}
    }
}