using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace Plum
{
    public static class TypeExtensions
    {
        #region Methods

        #region Methods - Traversal

        public static void TraversalMethodsInfo(this Type source, Func<MethodInfo, bool> method)
        {
            if (method == null || source == null)
                return;

            MethodInfo[] list = source.GetMethods();

            foreach (MethodInfo fi in list)
            {
                if (!method(fi))
                    return;
            }
        }

        public static void TraversalPropertiesInfo(this Type source, Func<PropertyInfo, bool> method, BindingFlags? flags = null)
        {
            if (method == null || source == null)
                return;

            PropertyInfo[] listPropertyInfo = flags == null ?
                source.GetProperties() : source.GetProperties(flags.Value);

            foreach (PropertyInfo pi in listPropertyInfo)
                if (!method(pi))
                    return;
        }

        public static void TraversalBaseType(this Type source, Func<Type, bool> method)
        {
            if (method == null || source == null)
                return;

            do
            {
                if (!method(source))
                    break;

                source = source.BaseType;
            } while (source != null);
        }

        #endregion Methods - Traversal

        #region Methods - Type

        public static bool IsEnumerable(this Type source)
        {
            return IsKindOf(source, typeof(IEnumerable));
        }

        public static bool IsKindOf(this Type source, Type target)
        {
            if (source == null)
                return false;

            if (source == target)
                return true;

            if (IsKindOf(source.BaseType, target))
                return true;

            foreach (Type tpInterface in source.GetInterfaces())
                if (IsKindOf(tpInterface, target))
                    return true;

            return false;
        }

        public static object GetDefaultValue(this Type source)
        {
            if (source.IsEnum)
                return Enum.ToObject(source, Enum.GetValues(source).GetValue(0));
            else if (source.IsValueType)
                return Activator.CreateInstance(source);

            return null;
        }

        public static object GetDefaultValueNoNull(this Type source)
        {
            object value = null;
            if (source == typeof(string))
                value = string.Empty;
            else if (source.IsNullableGeneric())
                value = Activator.CreateInstance(source.GetGenericTypeInNullable());
            else
                value = Activator.CreateInstance(source);

            return value;
        }

        public static bool IsValueTypeOrString(this Type source)
        {
            return source.IsValueType ||
                   source.IsEnum ||
                   source.IsKindOf(typeof(string));
        }

        public static bool IsNullable(this Type source)
        {
            return !source.IsValueType || source.IsNullableGeneric();
        }

        public static bool IsNullableGeneric(this Type source)
        {
            return source.FullName.StartsWith("System.Nullable`1") && source.IsGenericType;
        }

        public static Type GetGenericTypeInNullable(this Type source)
        {
            if (!source.IsNullableGeneric())
                return null;

            return source.GetGenericArguments()[0];
        }

        public static string GetAssemblyTypeName(this Type source)
        {
            return string.Format("{0}, {1}", source.FullName, source.Assembly.FullName);
        }

        public static string GetTypeNameWithoutQualified(this Type source)
        {
            if (!source.IsGenericType)
                return string.Format("{0}, {1}", source.FullName, source.Assembly.GetName().Name);

            var args = source.GetGenericArguments();

            return string.Format("{0}.{1}[{2}], {3}",
                source.Namespace,
                source.Name,
                BuildTypeName(args),
                source.Assembly.GetName().Name);
        }

        private static string BuildTypeName(Type[] args)
        {
            StringBuilder b = new StringBuilder(string.Format("[{0}]", args[0].GetTypeNameWithoutQualified()));
            for (int i = 1; i < args.Length; i++)
                b.AppendFormat(", [{0}]", args[i].GetTypeNameWithoutQualified());

            return b.ToString();
        }

        //private static object CreateInstance(this Type source, KeyValueList<string, object> values)
        //{
        //    object obj = Activator.CreateInstance(source);

        //    foreach (var pair in values)
        //        obj.SetPropertyValue(pair.Key, pair.Value);

        //    return obj;
        //}

        #endregion Methods - Type

        #endregion Methods
    }
}