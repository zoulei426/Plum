using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum
{
    public static class DoubleExtensions
    {
        public static float? ToNullableFloat(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSingle(@this);
        }

        public static int ToInt32(this double value)
        {
            return Convert.ToInt32(value);
        }

        public static int ToInt32(this double? value)
        {
            return value.GetValueOrDefault().ToInt32();
        }

        public static double Format(this double value, int digits = 2)
        {
            return RoundFormat(value, digits);
        }

        public static double? Format(this double? value, int digits = 2)
        {
            return value.HasValue ? Format(value.Value, digits) : value;
        }

        /// <summary>
        /// 保留小数位数
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="digits">位数</param>
        /// <returns></returns>
        public static double RoundFormat(this double value, int digits = 2)
        {
            //double number = value + 0.00000001;
            //double numeric = Convert.ToInt64(number * 100) / 100.0;
            double number;
            double numeric;
            switch (digits)
            {
                case 2:
                default:
                    number = value + 0.00001;
                    numeric = Convert.ToInt64(number * 100) / 100.0;
                    break;

                case 3:
                    number = value + 0.000001;
                    numeric = Convert.ToInt64(number * 1000) / 1000.0;
                    break;

                case 4:
                    number = value + 0.0000001;
                    numeric = Convert.ToInt64(number * 10000) / 10000.0;
                    break;

                case 5:
                    number = value + 0.00000001;
                    numeric = Convert.ToInt64(number * 100000) / 100000.0;
                    break;

                case 6:
                    number = value + 0.000000001;
                    numeric = Convert.ToInt64(number * 1000000) / 1000000.0;
                    break;

                case 7:
                    number = value + 0.0000000001;
                    numeric = Convert.ToInt64(number * 10000000) / 10000000.0;
                    break;

                case 8:
                    number = value + 0.00000000001;
                    numeric = Convert.ToInt64(number * 100000000) / 100000000.0;
                    break;
            }
            return numeric;
        }

        /// <summary>
        /// 保留小数位数
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="digits">位数</param>
        /// <returns></returns>
        public static double? RoundFormat(this double? value, int digits = 2)
        {
            return value.HasValue ? RoundFormat(value.Value, digits) : value;
        }

        /// <summary>
        /// 截取小数位数
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="digits">位数</param>
        /// <returns></returns>
        public static double CutFormat(this double value, int digits = 2)
        {
            double cel = Math.Pow(10, digits);
            double tempValue = Math.Truncate(value * cel);
            return tempValue / cel;
        }

        /// <summary>
        /// 截取小数位数
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="digits">位数</param>
        /// <returns></returns>
        public static double? CutFormat(this double? value, int digits = 2)
        {
            return value.HasValue ? CutFormat(value.Value, digits) : value;
        }

        /// <summary>
        /// min < x < max
        /// </summary>
        /// <param name="this"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static bool Between(this double @this, double minValue, double maxValue)
        {
            return minValue.CompareTo(@this) == -1 && @this.CompareTo(maxValue) == -1;
        }

        /// <summary>
        /// min ≤ x ≤ max
        /// </summary>
        /// <param name="this"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static bool InRange(this double @this, double minValue, double maxValue)
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        /// <summary>
        /// 平方米转为亩
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static double ToAcre(this double @this)
        {
            return @this * 0.0015;
        }

        /// <summary>
        /// 平方米转为亩
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static double ToAcre(this double? @this)
        {
            if (@this is null) return 0;
            return @this.Value * 0.0015;
        }

        public static bool Equal(this double value, double cValue, double digts = 0.00001)
        {
            if ((value + digts) > cValue && (value - digts) < cValue)
                return true;
            return false;
        }

        public static bool NotEqual(this double value, double cValue, double digts = 0.00001)
        {
            if ((value + digts) > cValue && (value - digts) < cValue)
                return false;
            return true;
        }

        public static bool NotEquals(this double? value, double cValue, double digts = 0.00001)
        {
            value = value.GetValueOrDefault();
            if ((value + digts) > cValue && (value - digts) < cValue)
                return false;
            return true;
        }

        public static bool NotEquals(this double value, double? cValue, double digts = 0.00001)
        {
            cValue = cValue.GetValueOrDefault();
            if ((value + digts) > cValue && (value - digts) < cValue)
                return false;
            return true;
        }

        public static bool NotEquals(this double? value, double? cValue, double digts = 0.00001)
        {
            value = value.GetValueOrDefault();
            cValue = cValue.GetValueOrDefault();
            if ((value + digts) > cValue && (value - digts) < cValue)
                return false;
            return true;
        }
    }
}