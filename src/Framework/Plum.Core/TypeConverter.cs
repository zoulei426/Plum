using Plum.Attributes;
using Plum.Object;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Plum.Windows.Convertors
{
    public abstract class TypeConverter : CDObject
    {
        #region Fields

        private Dictionary<Type, MethodInfo> htc;

        #endregion Fields

        #region Ctor

        public TypeConverter()
        {
            htc = TypeConverterAttribute.Create(this.GetType());
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        public T To<T>(object source)
        {
            return (T)To(source, typeof(T));
        }

        public virtual object To(object source, Type target)
        {
            if (source == null)
                return null;

            if (source == DBNull.Value)
                return target.GetDefaultValue();

            if (htc.ContainsKey(target))
                return (htc[target] as MethodInfo).Invoke(this, new object[] { source });

            if (target.IsEnum)
                return ToEnum(source, target);
            else if (target.IsNullableGeneric() && (target = target.GetGenericTypeInNullable()).IsEnum)
                return ToEnum(source, target);

            return source;
        }

        #endregion Methods - Public

        #region Methods - Private

        private object ToEnum(object source, Type target)
        {
            return source is string ?
                Enum.Parse(target, (string)source) :
                Enum.ToObject(target, Convert.ToInt32(source));
        }

        #endregion Methods - Private

        #endregion Methods
    }
}