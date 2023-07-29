using Plum.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Plum
{
    public static class FastObjectAccessor
    {
        #region Fields

        private static readonly Dictionary<BinaryKey<Type, string>, Type> _typeOfs = new Dictionary<BinaryKey<Type, string>, Type>();
        private static readonly Dictionary<BinaryKey<Type, string>, Func<object, object>> _getters = new Dictionary<BinaryKey<Type, string>, Func<object, object>>();
        private static readonly Dictionary<BinaryKey<Type, string>, Action<object, object>> _setters = new Dictionary<BinaryKey<Type, string>, Action<object, object>>();

        #endregion Fields

        #region Methods

        #region Methods - Public

        /// <summary>
        /// 显式为指定类型生成属性访问器
        /// </summary>
        /// <param name="type">类型</param>
        public static void MakeForType(Type type)
        {
            var properties = type.GetProperties();
            Array.ForEach(properties, item =>
            {
                var key = new BinaryKey<Type, string>(type, item.Name);
                var canAdd = item.CanRead && !_getters.ContainsKey(key);
                if (canAdd)
                {
                    GenerateGetter(key);
                }

                canAdd = item.CanWrite && !_setters.ContainsKey(key);
                if (canAdd)
                {
                    GenerateSetter(key);
                }
            });
        }

        /// <summary>
        /// 显式为指定类型生成属性访问器
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static void MakeForType<T>()
        {
            MakeForType(typeof(T));
        }

        /// <summary>
        /// 获取实例属性的类型
        /// </summary>
        /// <param name="obj">对象实例</param>
        /// <param name="name">属性名</param>
        /// <returns>属性的类型</returns>
        public static Type FastGetType(this object obj, string name)
        {
            return obj?.GetType().FastGetPropertyType(name);
        }

        /// <summary>
        /// 获取实例属性的类型
        /// </summary>
        /// <param name="type">对象实例的类型</param>
        /// <param name="name">属性名</param>
        /// <returns>属性的类型</returns>
        public static Type FastGetPropertyType(this Type type, string name)
        {
            var key = new BinaryKey<Type, string>(type, name);

            // 若不存在则生成
            if (!_typeOfs.TryGetValue(key, out Type propertyType))
            {
                propertyType = GenerateTypeOf(key);
            }

            return propertyType;
        }

        /// <summary>
        /// 获取实例属性的值
        /// </summary>
        /// <param name="obj">对象实例</param>
        /// <param name="name">属性名</param>
        /// <returns>属性的值</returns>
        public static object FastGetValue(this object obj, string name)
        {
            var key = new BinaryKey<Type, string>(obj.GetType(), name);

            // 若不存在则生成
            if (!_getters.TryGetValue(key, out Func<object, object> getter))
            {
                getter = GenerateGetter(key);
            }

            return getter?.Invoke(obj);
        }

        /// <summary>
        /// 获取实例属性的值
        /// </summary>
        /// <typeparam name="T">属性的类型</typeparam>
        /// <param name="obj">对象实例</param>
        /// <param name="name">属性名</param>
        /// <returns>属性的值</returns>
        public static T FastGetValue<T>(this object obj, string name)
        {
            return (T)obj.FastGetValue(name);
        }

        /// <summary>
        /// 设置实例属性的值
        /// </summary>
        /// <param name="obj">对象实例</param>
        /// <param name="name">属性名</param>
        /// <param name="value">属性的值</param>
        public static void FastSetValue(this object obj, string name, object value)
        {
            var key = new BinaryKey<Type, string>(obj.GetType(), name);

            // 若不存在则生成
            if (!_setters.TryGetValue(key, out Action<object, object> setter))
            {
                setter = GenerateSetter(key);
            }

            setter?.Invoke(obj, value);
        }

        #endregion Methods - Public

        #region Methods - Private

        /// <summary>
        /// 为指定属性生成 typeof 访问器
        /// </summary>
        /// <param name="key">存储键（对象类型 + 属性名）</param>
        /// <returns>属性的类型</returns>
        private static Type GenerateTypeOf(BinaryKey<Type, string> key)
        {
            var objParamExpr = Expression.Parameter(key.Primary);
            var propertyExpr = Expression.Property(objParamExpr, key.Secondary);
            var type = propertyExpr.Type;

            _typeOfs.Add(key, type);

            return type;
        }

        /// <summary>
        /// 为指定属性生成读访问器
        /// </summary>
        /// <param name="key">存储键（对象类型 + 属性名）</param>
        /// <returns>生成的读访问器委托</returns>
        private static Func<object, object> GenerateGetter(BinaryKey<Type, string> key)
        {
            try
            {
                var objParamExpr = Expression.Parameter(typeof(object));
                var propertyExpr = Expression.Property(Expression.Convert(objParamExpr, key.Primary), key.Secondary);
                var convertExpr = Expression.Convert(propertyExpr, typeof(object));
                var getterExpr = Expression.Lambda<Func<object, object>>(convertExpr, objParamExpr);
                var getter = getterExpr.Compile();

                _getters.Add(key, getter);

                return getter;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 为指定属性生成写访问器
        /// </summary>
        /// <param name="key">存储键（对象类型 + 属性名）</param>
        /// <returns>生成的写访问器委托</returns>
        private static Action<object, object> GenerateSetter(BinaryKey<Type, string> key)
        {
            try
            {
                var objParamExpr = Expression.Parameter(typeof(object));
                var valueParamExpr = Expression.Parameter(typeof(object));
                var propertyExpr = Expression.Property(Expression.Convert(objParamExpr, key.Primary), key.Secondary);
                var propertyAssignExpr = Expression.Assign(propertyExpr, Expression.Convert(valueParamExpr, propertyExpr.Type));
                var convertExpr = Expression.Convert(propertyAssignExpr, typeof(object));
                var setterExpr = Expression.Lambda<Action<object, object>>(convertExpr, objParamExpr, valueParamExpr);
                var setter = setterExpr.Compile();

                _setters.Add(key, setter);

                return setter;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        #endregion Methods - Private

        #endregion Methods
    }
}