using System;
using System.Windows;

namespace Plum.Windows.Convertors
{
    public class ProgressToVisibilityConverter : ValueConverterBase<int, Visibility>
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override Visibility Convert(int value) =>
            value > 0 && value < 100 ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override int ConvertBack(Visibility value) => throw new NotImplementedException();
    }
}