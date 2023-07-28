using System;
using System.Text.RegularExpressions;

namespace Plum.Common
{
    public class PlumValidationRule
    {
        /// <summary>
        /// 证件号验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsICN(ICNType type, string value)
        {
            bool res = false;
            if (value is null)
                return res;
            switch (type)
            {
                case ICNType.SFZ:
                    // 身份证
                    string sfz = @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}(\d|x|X)$";
                    res = Regex.IsMatch(value, sfz);
                    break;

                case ICNType.GATSFZ:
                    // 港澳通行证
                    string gatxz = @"^[a-zA-Z0-9]{6,10}$";
                    // 台胞证
                    string tbz = @"^([0-9]{8}|[0-9]{10})$";
                    res = Regex.IsMatch(value, gatxz) || Regex.IsMatch(value, tbz);
                    break;

                case ICNType.HZ:
                    // 护照
                    string hz = @"^[a-zA-Z0-9]{5,17}$";
                    res = Regex.IsMatch(value, hz);
                    break;

                case ICNType.HKB:

                    res = true;
                    break;

                case ICNType.JGZ:
                    // 军官证
                    string jgz = @"^[0-9]{8}$";
                    res = Regex.IsMatch(value, jgz);
                    break;

                case ICNType.ZZJGDM:
                    // 组织机构代码
                    string zzjgdm = @"^[a-zA-Z0-9]{10,20}$";
                    res = Regex.IsMatch(value, zzjgdm);
                    break;

                case ICNType.YYZZ:
                    // 营业执照
                    string yyzz = @"^[a-zA-Z0-9]{10,20}$";
                    res = Regex.IsMatch(value, yyzz);
                    break;

                case ICNType.SJHZD:
                    res = true;
                    break;

                case ICNType.QT:

                    res = true;
                    break;

                default:
                    break;
            }
            return res;
        }

        public static bool IsAnd(float? a, float? b, float? c)
        {
            if (!a.HasValue || !b.HasValue || !c.HasValue)
                return false;
            return Math.Abs(a.Value + b.Value - c.Value) < 0.0001;
        }
    }
}