using System;
using System.Windows;

namespace Plum.Windows.Convertors
{
    public class BoolToVisibilityConverter : ValueConverterBase<bool, Visibility>
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override Visibility Convert(bool value) =>
            value ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override bool ConvertBack(Visibility value) => throw new NotImplementedException();
    }

    public class NotBoolToVisibilityConverter : ValueConverterBase<bool, Visibility>
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override Visibility Convert(bool value) =>
            value ? Visibility.Collapsed : Visibility.Visible;

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override bool ConvertBack(Visibility value) => throw new NotImplementedException();
    }
}