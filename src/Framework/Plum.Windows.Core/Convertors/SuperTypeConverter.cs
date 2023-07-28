using System;
using System.Windows.Data;

namespace Plum.Windows.Convertors
{
    public class SuperTypeConverter : IValueConverter
    {
        #region Properties

        public Type ConvertType { get; set; }
        public Type ConvertBackType { get; set; }

        #endregion Properties

        #region Methods

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new DotNetTypeConverter().To(value, ConvertType == null ? targetType : ConvertType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new DotNetTypeConverter().To(value, ConvertBackType == null ? targetType : ConvertBackType);
        }

        #endregion Methods
    }
}