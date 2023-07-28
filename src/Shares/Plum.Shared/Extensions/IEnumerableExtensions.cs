using System.Collections.Generic;

namespace Plum
{
    /// <summary>
    /// 可枚举扩展
    /// </summary>
    public static class IEnumerableExtensions
    {
        #region Methods

        public static void AddFriendly<TKey, TValue>(this Dictionary<TKey, List<TValue>> dic, TKey key, TValue value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key].Add(value);
            }
            else
            {
                dic.Add(key, new List<TValue> { value });
            }
        }

        #endregion Methods
    }
}