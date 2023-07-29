using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum
{
    public static class DictionaryExtensions
    {
        public static void AddAll<TKey, TValue>(this IDictionary<TKey, TValue> @this, IDictionary<TKey, TValue> value)
        {
            foreach (var item in value)
            {
                @this.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 如果键不存在，则添加
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="this"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddIfNotContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue value)
        {
            if (!@this.ContainsKey(key))
            {
                @this.Add(key, value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 如果键不存在，则添加，否则更新
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="this"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue value)
        {
            if (!@this.ContainsKey(key))
            {
                @this.Add(new KeyValuePair<TKey, TValue>(key, value));
            }
            else
            {
                @this[key] = value;
            }

            return @this[key];
        }

        public static TValue AddOrUpdateIfNotEmpty<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue value)
        {
            if (!@this.ContainsKey(key))
            {
                @this.Add(new KeyValuePair<TKey, TValue>(key, value));
            }
            else
            {
                if (value != null && value.ToString().IsNotNullOrEmpty())
                    @this[key] = value;
            }

            return @this[key];
        }

        public static void AddInListIfNotEmpty<TKey, TValue>(this IDictionary<TKey, List<TValue>> @this, TKey key, TValue value)
        {
            if (value == null || value.ToString().IsNullOrEmpty())
                return;

            if (!@this.ContainsKey(key))
            {
                @this.Add(new KeyValuePair<TKey, List<TValue>>(key, new List<TValue>
                {
                    value
                }));
            }
            else
            {
                @this.GetValueOrDefault(key)?.Add(value);
            }
        }

        /// <summary>
        /// 如果键存在则移除
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="this"></param>
        /// <param name="key"></param>
        public static void RemoveIfContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key)
        {
            if (@this.ContainsKey(key))
            {
                @this.Remove(key);
            }
        }

        /// <summary>
        /// 根据键获取值，如果键不存在，则返回默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="this"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key)
        {
            if (key == null) return default;

            @this.TryGetValue(key, out TValue value);

            return value;
        }

        public static TValue GetValueOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
        {
            TValue obj;
            if (dictionary.TryGetValue(key, out obj))
            {
                return obj;
            }

            return dictionary[key] = factory(key);
        }

        public static TValue GetValueOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> factory)
        {
            return dictionary.GetValueOrAdd(key, k => factory());
        }

        public static List<TValue> ToValueList<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary)
        {
            var result = new List<TValue>();
            dictionary.ForEach(x => result.AddRange(x.Value));
            return result;
        }
    }
}