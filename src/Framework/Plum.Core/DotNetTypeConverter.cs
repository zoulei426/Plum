using Plum.Attributes;
using Plum.Windows.Convertors;
using System;

namespace Plum
{
    public class DotNetTypeConverter : TypeConverter
    {
        #region Properties

        public static DotNetTypeConverter Instance { get { return _Instance; } }
        private static DotNetTypeConverter _Instance = new DotNetTypeConverter();

        #endregion Properties

        #region Methods

        #region Methods - Static

        public static Type GetType(eDataType type)
        {
            return DotNetTypeAttribute.GetType(type);
        }

        public static eDataType GetType(Type type)
        {
            return DotNetTypeAttribute.GetType(type);
        }

        #endregion Methods - Static

        #region Methods - Public

        public virtual object ConvertTo(object source, eDataType type)
        {
            return To(source, GetType(type));
        }

        #endregion Methods - Public

        #region Methods - Converter

        [TypeConverter(typeof(double))]
        [TypeConverter(typeof(double?))]
        public virtual object ToDouble(object source)
        {
            if (source is double || source is double?)
                return source;

            if (source.ToString().IsNullOrWhiteSpace())
                return null;

            return Convert.ToDouble(source);
        }

        [TypeConverter(typeof(float))]
        [TypeConverter(typeof(float?))]
        [TypeConverter(typeof(Single))]
        [TypeConverter(typeof(Single?))]
        public virtual object ToFloat(object source)
        {
            if (source is float || source is float? || source is Single || source is Single?)
                return source;

            if (source.ToString().IsNullOrWhiteSpace())
                return null;

            return Convert.ToSingle(source);
        }

        [TypeConverter(typeof(decimal))]
        [TypeConverter(typeof(decimal?))]
        public virtual object ToDecimal(object source)
        {
            if (source is decimal || source is decimal?)
                return source;

            if (source.ToString().IsNullOrWhiteSpace())
                return null;

            return Convert.ToDecimal(source);
        }

        [TypeConverter(typeof(Guid))]
        [TypeConverter(typeof(Guid?))]
        public virtual object ToGuid(object source)
        {
            if (source is Guid || source is Guid?)
                return source;

            if (source.ToString().IsNullOrWhiteSpace())
                return null;

            return Guid.Parse(source.ToString());
        }

        [TypeConverter(typeof(long))]
        [TypeConverter(typeof(long?))]
        public virtual object ToInt64(object source)
        {
            if (source is long || source is long?)
                return source;

            if (source.ToString().IsNullOrWhiteSpace())
                return null;

            return Convert.ToInt64(source);
        }

        [TypeConverter(typeof(int))]
        [TypeConverter(typeof(int?))]
        public virtual object ToInt32(object source)
        {
            if (source is int || source is int?)
                return source;

            if (source.ToString().IsNullOrWhiteSpace())
                return null;

            return Convert.ToInt32(source);
        }

        [TypeConverter(typeof(short))]
        [TypeConverter(typeof(short?))]
        public virtual object ToInt16(object source)
        {
            if (source is short || source is short?)
                return source;

            if (source.ToString().IsNullOrWhiteSpace())
                return null;

            return Convert.ToInt16(source);
        }

        [TypeConverter(typeof(bool))]
        [TypeConverter(typeof(bool?))]
        public virtual object ToBoolean(object source)
        {
            if (source is bool || source is bool?)
                return source;

            if (source.ToString().IsNullOrWhiteSpace())
                return null;

            return Convert.ToBoolean(source);
        }

        [TypeConverter(typeof(string))]
        public virtual object ToString(object source)
        {
            if (source is string)
                return source;
            if (source is byte[])
                return Convert.ToBase64String((byte[])source);

            return source.ToString();
        }

        [TypeConverter(typeof(DateTime))]
        [TypeConverter(typeof(DateTime?))]
        public virtual object ToDateTime(object source)
        {
            if (source is DateTime || source is DateTime?)
                return source;

            if (source.ToString().IsNullOrWhiteSpace())
                return null;

            return DateTime.Parse(source.ToString());
        }

        [TypeConverter(typeof(TimeSpan))]
        [TypeConverter(typeof(TimeSpan?))]
        public virtual object ToTimeSpan(object source)
        {
            if (source is TimeSpan || source is TimeSpan?)
                return source;

            if (source.ToString().IsNullOrWhiteSpace())
                return null;

            return TimeSpan.Parse(source.ToString());
        }

        [TypeConverter(typeof(byte[]))]
        public virtual object ToBinary(object source)
        {
            if (source is byte[])
                return source;

            if (source.ToString().IsNullOrWhiteSpace())
                return null;

            return Convert.FromBase64String(source.ToString());
        }

        #endregion Methods - Converter

        #endregion Methods
    }
}