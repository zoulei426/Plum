using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Plum
{
    /// <summary>
    /// ObjectExtensions
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 容差
        /// </summary>
        private const double Tolerance = 1e-6;

        /// <summary>
        /// Casts to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T CastTo<T>(this object value)
        {
            if (typeof(T).IsEnum)
            {
                return value is null ? default : (T)Enum.Parse(typeof(T), value.ToString());
            }
            return typeof(T).IsValueType && value != null
                ? (T)Convert.ChangeType(value, typeof(T))
                : value is T typeValue ? typeValue : default;
        }

        /// <summary>
        /// 数据塑形
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Not found property {propertyName} in {typeof(TSource)}</exception>
        public static ExpandoObject ShapeData<TSource>(this TSource source, string fields)
        {
            Check.NotNull(source);

            var expandoObject = new ExpandoObject();

            if (fields.IsNullOrWhiteSpace())
            {
                var propertyInfos = typeof(TSource).GetProperties(
                    BindingFlags.IgnoreCase |
                    BindingFlags.Public |
                    BindingFlags.Instance);

                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)expandoObject).Add(propertyInfo.Name, propertyValue);
                }
            }
            else
            {
                var splitedFields = fields.Split(",");
                foreach (var field in splitedFields)
                {
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(TSource).GetProperty(propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo is null)
                    {
                        throw new Exception($"Not found property {propertyName} in {typeof(TSource)}");
                    }

                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)expandoObject).Add(propertyInfo.Name, propertyValue);
                }
            }

            return expandoObject;
        }

        #region Copy

        /// <summary>
        /// Converts to.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">source</exception>
        public static object ConvertTo(this object source, Type targetType)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            object target = Activator.CreateInstance(targetType);
            Type tType = targetType;

            source.TraversalPropertiesInfo((pi, val, t) =>
            {
                var tpi = tType.GetProperty(pi.Name);
                if (tpi == null)
                    return true;
                if (!tpi.CanWrite)
                    return true;
                if (pi.PropertyType != tpi.PropertyType)
                    return true;

                tpi.SetValue(t, val, null);

                return true;
            }, target);

            return target;
        }

        /// <summary>
        /// Converts to.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">source</exception>
        public static object ConvertTo(this object source, Type targetType, Dictionary<string, string> mapping)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            object target = Activator.CreateInstance(targetType);
            Type tType = targetType;

            source.TraversalPropertiesInfo((pi, val, t) =>
            {
                if (!mapping.ContainsKey(pi.Name))
                    return true;

                var nameTarget = mapping[pi.Name];
                var tpi = tType.GetProperty(nameTarget);
                if (tpi == null)
                    return true;
                if (!tpi.CanWrite)
                    return true;
                if (pi.PropertyType != tpi.PropertyType)
                    return true;

                tpi.SetValue(t, val, null);

                return true;
            }, target);

            return target;
        }

        /// <summary>
        /// Converts to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">source</exception>
        public static T ConvertTo<T>(this object source) where T : new()
        {
            if (source == null)
                throw new ArgumentNullException("source");

            T target = new T();
            Type tType = typeof(T);

            source.TraversalPropertiesInfo((pi, val, t) =>
            {
                var tpi = tType.GetProperty(pi.Name);
                if (tpi == null)
                    return true;
                if (!tpi.CanWrite)
                    return true;
                if (pi.PropertyType != tpi.PropertyType)
                    return true;

                tpi.SetValue(t, val, null);

                return true;
            }, target);

            return target;
        }

        /// <summary>
        /// Converts to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object source, Dictionary<string, string> mapping) where T : new()
        {
            return (T)ConvertTo(source, typeof(T), mapping);
        }

        /// <summary>
        /// Copies the properties from.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom(this object target, object source)
        {
            if (source == null || target == null)
                return;

            Type typeSource = source.GetType();
            Type typeTarget = target.GetType();

            PropertyInfo[] listSourceProperty = typeSource.GetProperties();
            PropertyInfo[] listTargetProperty = typeTarget.GetProperties();

            foreach (PropertyInfo piSource in listSourceProperty)
            {
                if (!piSource.CanRead)
                    continue;

                var pms = piSource.GetIndexParameters();
                if (pms != null && pms.Length > 0)
                    continue;

                object val = piSource.GetValue(source, null);
                foreach (PropertyInfo piTarget in listTargetProperty)
                    if (piTarget.Name == piSource.Name &&
                        piTarget.PropertyType.FullName == piSource.PropertyType.FullName &&
                        piTarget.CanWrite)
                    {
                        piTarget.SetValue(target, val, null);
                        break;
                    }
            }
        }

        /// <summary>
        /// Copies the properties from.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <param name="mapping">The mapping.</param>
        public static void CopyPropertiesFrom(this object target, object source, Dictionary<string, string> mapping)
        {
            if (source == null || target == null)
                return;

            Type typeSource = source.GetType();
            Type typeTarget = target.GetType();

            PropertyInfo[] listSourceProperty = typeSource.GetProperties();
            PropertyInfo[] listTargetProperty = typeTarget.GetProperties();

            foreach (PropertyInfo piSource in listSourceProperty)
            {
                if (!mapping.ContainsKey(piSource.Name))
                    continue;
                if (!piSource.CanRead)
                    continue;

                var pms = piSource.GetIndexParameters();
                if (pms != null && pms.Length > 0)
                    continue;

                string nameTarget = mapping[piSource.Name];
                object val = piSource.GetValue(source, null);
                foreach (PropertyInfo piTarget in listTargetProperty)
                    if (piTarget.Name == nameTarget &&
                        piTarget.PropertyType.FullName == piSource.PropertyType.FullName &&
                        piTarget.CanWrite)
                    {
                        piTarget.SetValue(target, val, null);
                        break;
                    }
            }
        }

        #endregion Copy

        #region Traversal

        public static void TraversalMethodsInfo(this object source, Func<MethodInfo, bool> method)
        {
            TypeExtensions.TraversalMethodsInfo(source.GetType(), method);
        }

        public static void TraversalFieldsInfo(this object source, Func<FieldInfo, object, bool> method)
        {
            if (method == null || source == null)
                return;

            FieldInfo[] listFieldInfo = source.GetType().GetFields();

            foreach (FieldInfo fi in listFieldInfo)
            {
                object val = fi.GetValue(source);
                if (!method(fi, val))
                    return;
            }
        }

        public static void TraversalPropertiesInfo(this object source, Func<string, object, bool> method)
        {
            if (method == null || source == null)
                return;

            PropertyInfo[] listPropertyInfo = source.GetType().GetProperties();

            foreach (PropertyInfo pi in listPropertyInfo)
            {
                if (!pi.CanRead)
                    continue;

                if (pi.GetIndexParameters().Length > 0)
                    continue;

                object val = pi.GetValue(source, null);
                if (!method(pi.Name, val))
                    return;
            }
        }

        public static void TraversalPropertiesInfo(this object source, Func<PropertyInfo, object, bool> method)
        {
            if (method == null || source == null)
                return;

            PropertyInfo[] listPropertyInfo = source.GetType()
                .GetProperties(
                    BindingFlags.IgnoreCase |
                    BindingFlags.Public |
                    BindingFlags.Instance)
                .OrderBy(c => c.MetadataToken).ToArray();

            foreach (PropertyInfo pi in listPropertyInfo)
            {
                if (!pi.CanRead)
                    continue;

                if (pi.GetIndexParameters().Length > 0)
                    continue;

                try
                {
                    object val = pi.GetValue(source);
                    if (!method(pi, val))
                        return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    continue;
                }
            }
        }

        public static void TraversalPropertiesInfo(this object source, Func<PropertyInfo, object, object, bool> method, object argument)
        {
            if (method == null || source == null)
                return;

            PropertyInfo[] listPropertyInfo = source.GetType().GetProperties();

            foreach (PropertyInfo pi in listPropertyInfo)
            {
                if (!pi.CanRead)
                    continue;

                if (pi.GetIndexParameters().Length > 0)
                    continue;

                try
                {
                    object val = pi.GetValue(source, null);
                    if (!method(pi, val, argument))
                        return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    continue;
                }
            }
        }

        #endregion Traversal
    }
}