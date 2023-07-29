using Plum.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum
{
    public static class IntegerExtensions
    {
        public static T ToEnum<T>(this int value) where T : struct
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        private const int CARRYING_NUMBER = 1024;

        public static string ToByteDisplay(this long bytes, int decimalDigits = 0)
        {
            var unit = "B";
            var value = (double)bytes;
            if (value >= CARRYING_NUMBER)
            {
                value /= CARRYING_NUMBER;
                unit = "KB";
            }

            if (value >= CARRYING_NUMBER)
            {
                value /= CARRYING_NUMBER;
                unit = "MB";
            }

            if (value >= CARRYING_NUMBER)
            {
                value /= CARRYING_NUMBER;
                unit = "GB";
            }

            if (value >= CARRYING_NUMBER)
            {
                value /= CARRYING_NUMBER;
                unit = "TB";
            }

            return $"{(value.ToString($"F{decimalDigits}"))}{unit}";
        }

        public static string ToByteDisplay(this long? bytes, int decimalDigits = 0)
        {
            if (!bytes.HasValue)
            {
                return string.Empty;
            }

            return bytes.Value.ToByteDisplay(decimalDigits);
        }

        public static int? ToByteInt(this long? value, out ByteUnit unit)
        {
            unit = ByteUnit.B;

            if (value >= CARRYING_NUMBER)
            {
                value /= CARRYING_NUMBER;
                unit = ByteUnit.KB;
            }

            if (value >= CARRYING_NUMBER)
            {
                value /= CARRYING_NUMBER;
                unit = ByteUnit.MB;
            }

            if (value >= CARRYING_NUMBER)
            {
                value /= CARRYING_NUMBER;
                unit = ByteUnit.GB;
            }

            if (value >= CARRYING_NUMBER)
            {
                value /= CARRYING_NUMBER;
                unit = ByteUnit.TB;
            }
            return (int?)value;
        }

        public static long KB(this int value)
        {
            return value * 1024;
        }

        public static long? KB(this int? value)
        {
            return value * 1024;
        }

        public static long MB(this int value)
        {
            return value * 1024 * 1024;
        }

        public static long? MB(this int? value)
        {
            return value * 1024 * 1024;
        }

        public static long GB(this int value)
        {
            return value * 1024 * 1024 * 1024;
        }

        public static long? GB(this int? value)
        {
            return value * 1024 * 1024 * 1024;
        }

        public static long TB(this int value)
        {
            return value * 1024 * 1024 * 1024 * 1024;
        }

        public static long? TB(this int? value)
        {
            return value * 1024 * 1024 * 1024 * 1024;
        }

        public static int ByKB(this long value)
        {
            return (int)(value / 1024);
        }

        public static string ToCN(this int value)
        {
            string[] intArr = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", };
            string[] strArr = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", };
            string[] Chinese = { "", "十", "百", "千", "万", "十", "百", "千", "亿" };
            //金额
            //string[] Chinese = { "元", "十", "百", "千", "万", "十", "百", "千", "亿" };
            char[] tmpArr = value.ToString().ToArray();
            string tmpVal = "";
            for (int i = 0; i < tmpArr.Length; i++)
            {
                tmpVal += strArr[tmpArr[i] - 48];//ASCII编码 0为48
                tmpVal += Chinese[tmpArr.Length - 1 - i];//根据对应的位数插入对应的单位
            }

            return tmpVal;
        }
    }
}