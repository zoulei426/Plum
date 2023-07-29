using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Plum
{
    /// <summary>
    /// StringExtensions
    /// </summary>
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool IsNotNullOrWhiteSpace(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotNullAndEquals(this string @this, string value)
        {
            return @this != null && @this.Equals(value);
        }

        public static bool IsNotNullAndEquals(this string @this, string value, StringComparison comparisonType)
        {
            return @this != null && @this.Equals(value, comparisonType);
        }

        public static bool IsNotNullAndStartWith(this string @this, string value)
        {
            return @this != null && @this.StartsWith(value);
        }

        public static bool IsNotNullAndStartWith(this string @this, string value, StringComparison comparisonType)
        {
            return @this != null && @this.StartsWith(value, comparisonType);
        }

        public static int ToInt32(this string value)
        {
            if (value.IsNullOrWhiteSpace())
                return default;

            return Convert.ToInt32(value);
        }

        public static long ToLong(this string value)
        {
            if (value.IsNullOrWhiteSpace())
                return default;

            return Convert.ToInt64(value);
        }

        public static double ToDouble(this string value)
        {
            if (value.IsNullOrWhiteSpace())
                return default;

            return Convert.ToDouble(value);
        }

        public static Guid ToGuid(this string value)
        {
            if (value.IsNullOrWhiteSpace())
                return default;

            Guid.TryParse(value, out Guid result);
            return result;
        }

        public static DateTime ToDateTime(this string value)
        {
            if (value.IsNullOrWhiteSpace())
                return default;

            DateTime.TryParse(value, out DateTime result);
            return result;
        }

        public static bool ToBoolean(this string value)
        {
            if (value.IsNullOrWhiteSpace())
                return default;

            bool.TryParse(value, out bool result);
            return result;
        }

        public static T ToEnum<T>(this string value) where T : struct
        {
            if (value.IsNullOrWhiteSpace())
                return default;

            Enum.TryParse(value, out T result);
            return result;
        }

        /// <summary>
        /// 是否数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value)
        {
            if (value.IsNullOrEmpty()) return false;
            return Regex.IsMatch(value, @"^[0-9]*$");
        }

        /// <summary>
        /// 是否小数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsFraction(this string value)
        {
            if (value.IsNullOrEmpty()) return false;
            return Regex.IsMatch(value, @"^[0-9]*\.[0-9]*$");
        }

        public static string Left(this string value, int length)
        {
            if (value.IsNullOrEmpty()) return value;
            if (length > value.Length) length = value.Length;
            return value.Substring(0, length);
        }

        public static string Right(this string value, int length)
        {
            if (value.IsNullOrEmpty()) return value;
            if (length > value.Length) length = value.Length;
            return value.Substring(value.Length - length);
        }

        /// <summary>
        /// 格式化面积
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <param name="isUnknown"></param>
        /// <returns></returns>
        public static string AreaFormat(this string value, int digits = 2, bool isUnknown = true)
        {
            var isNumeric = value.IsNumeric();
            var isFraction = true;
            if (isUnknown)
            {
                isFraction = value.IsFraction();
            }
            if (!(isNumeric || isFraction))
            {
                return value;
            }
            if (isNumeric)
            {
                return $"{value}{".".PadRight(digits + 1, '0')}";
            }

            int index = value.IndexOf(".");
            if (index < 0)
            {
                return value;
            }
            string prefix = value.Substring(0, index + 1);
            string suffix = value.Substring(index + 1);

            if (suffix.Length > digits)
            {
                return (prefix + suffix).ToDouble().Format().ToString().AreaFormat(digits, false);
            }
            else
            {
                while (suffix.Length < digits)
                {
                    suffix += "0";
                }
                return prefix + suffix;
            }
        }

        /// <summary>
        /// 安全地截取字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubstringSafe(this string value, int startIndex, int length)
        {
            if (startIndex < 0 || length < 0 || value.Length < startIndex + length)
                return value;

            return value.Substring(startIndex, length);
        }

        /// <summary>
        /// 安全地截取字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string SubstringSafe(this string value, int startIndex)
        {
            if (startIndex < 0 || startIndex > value.Length)
                return value;

            return value.Substring(startIndex);
        }

        public static bool IsMatch(this string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }

        public static bool IsMatch(this string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        /// <summary>
        /// Converts given string to a byte array using <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        public static byte[] GetBytes(this string str)
        {
            return str.GetBytes(Encoding.UTF8);
        }

        /// <summary>
        /// Converts given string to a byte array using the given <paramref name="encoding"/>
        /// </summary>
        public static byte[] GetBytes(this string str, Encoding encoding)
        {
            Check.NotNull(str, nameof(str));
            Check.NotNull(encoding, nameof(encoding));

            return encoding.GetBytes(str);
        }

        /// <summary>
        /// 若替换后的字符串为 Null 或 Empty，则返回原值，否则返回替换后的值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ReplaceIfNotEmpty(this string value, string oldValue, string newValue)
        {
            var tmp = value.Replace(oldValue, newValue);
            return tmp.IsNullOrEmpty() ? value : tmp;
        }

        /// <summary>
        /// 中文转拼音首字母大写
        /// </summary>
        /// <param name="CnStr"></param>
        /// <returns></returns>
        public static string ToCYTPALL(this string CnStr)
        {
            var sb = new StringBuilder();
            foreach (var item in CnStr.ToCharArray())
            {
                sb.Append(GetSpellCode(item.ToString()));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母
        /// </summary>
        /// <param name="CnChar">单个汉字</param>
        /// <returns>单个大写字母</returns>
        private static string GetSpellCode(string CnChar)
        {
            var result = string.Empty;
            // 若不是中文，返回 Empty
            if (!CnChar.IsMatch(@"[\u4e00-\u9fa5]"))
            {
                return result;
            }
            long iCnChar;
            byte[] arrCN = System.Text.Encoding.Default.GetBytes(CnChar);

            short area = arrCN[0];
            short pos = arrCN[1];
            iCnChar = (area << 8) + pos;

            // iCnChar match the constant
            string letter = "ABCDEFGHJKLMNOPQRSTWXYZ";
            int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614,
                                   48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906,
                                   51387, 51446, 52218, 52698, 52980, 53689, 54481, 55290 };
            for (int i = 0; i < 23; i++)
            {
                if (areacode[i] <= iCnChar && iCnChar < areacode[i + 1])
                {
                    return letter.Substring(i, 1);
                }
            }

            return result;
        }

        private static Regex REGEX_NORMALIZE = new Regex(@"[^a-zA-Z0-9\u4e00-\u9fa5\s]", RegexOptions.Compiled);

        private const string EXCEPT_CHARS = ": ‘ ！ @ # % … & * （  ^  &  ￥  ， 。 , .）$ \r \n / \\";

        private static Regex REGEX_EXCEPT = new Regex($"[{Regex.Escape(EXCEPT_CHARS)}]", RegexOptions.Compiled);

        /// <summary>
        /// 去除特殊字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Normalized(this string input)
        {
            return REGEX_EXCEPT.Replace(input, "");
        }

        public static List<string> Normalized(this IEnumerable<string> input)
        {
            return input?.Where(x => x.IsNotNullOrEmpty())?.Distinct()?.ToList();
        }

        public static string Humanized(this string input)
        {
            if (input.IsNullOrEmpty())
                return "空";
            return input;
        }

        public static string GetBetween(this string value, string x, string y)
        {
            if (value == null || x == null)
            {
                throw new ArgumentNullException((value == null) ? "value" : "x");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            int num = value.IndexOf(x);
            int num2 = value.LastIndexOf(y);
            if (num == -1 || num2 == -1)
            {
                return string.Empty;
            }
            int num3 = num + x.Length;
            if (num3 < num2)
            {
                return value.Substring(num3, num2 - num3);
            }
            return string.Empty;
        }

        private static readonly char[] Delimeters = { ' ', '-', '_' };

        /// <summary>
        /// Converts to snakecase.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">source</exception>
        public static string ToSnakeCase(this string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return SymbolsPipe(
                source,
                '_',
                (s, disableFrontDelimeter) =>
                {
                    if (disableFrontDelimeter)
                    {
                        return new char[] { char.ToLowerInvariant(s) };
                    }

                    return new char[] { '_', char.ToLowerInvariant(s) };
                });
        }

        /// <summary>
        /// Symbolses the pipe.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="mainDelimeter">The main delimeter.</param>
        /// <param name="newWordSymbolHandler">The new word symbol handler.</param>
        /// <returns></returns>
        private static string SymbolsPipe(
            string source,
            char mainDelimeter,
            Func<char, bool, char[]> newWordSymbolHandler)
        {
            var builder = new StringBuilder();

            bool nextSymbolStartsNewWord = true;
            bool disableFrontDelimeter = true;
            foreach (var symbol in source)
            {
                if (Delimeters.Contains(symbol))
                {
                    if (symbol == mainDelimeter)
                    {
                        builder.Append(symbol);
                        disableFrontDelimeter = true;
                    }

                    nextSymbolStartsNewWord = true;
                }
                else if (!char.IsLetterOrDigit(symbol))
                {
                    builder.Append(symbol);
                    disableFrontDelimeter = true;
                    nextSymbolStartsNewWord = true;
                }
                else
                {
                    if (nextSymbolStartsNewWord || char.IsUpper(symbol))
                    {
                        builder.Append(newWordSymbolHandler(symbol, disableFrontDelimeter));
                        disableFrontDelimeter = false;
                        nextSymbolStartsNewWord = false;
                    }
                    else
                    {
                        builder.Append(symbol);
                    }
                }
            }

            return builder.ToString();
        }

        #region ICN

        /// <summary>
        /// 1男0女
        /// </summary>
        /// <param name="identityCard"></param>
        /// <returns></returns>
        public static int? GetGenderByICN(this string identityCard)
        {
            if (identityCard.Length != 15 && identityCard.Length != 18)
            {
                Trace.WriteLine($"身份证{identityCard}不合法！");
                return null;
            }

            string gender = string.Empty;
            if (identityCard.Length == 18)
            {
                gender = identityCard.Substring(14, 3);
            }
            if (identityCard.Length == 15)
            {
                gender = identityCard.Substring(12, 3);
            }

            if (int.Parse(gender) % 2 == 0)//性别代码为偶数是女性奇数为男性
            {
                return 0;
                //return "女";
            }
            else
            {
                return 1;
                //return "男";
            }
        }

        #endregion ICN
    }
}