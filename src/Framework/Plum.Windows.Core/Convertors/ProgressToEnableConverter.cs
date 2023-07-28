using System;

namespace Plum.Windows.Convertors
{
    public class ProgressToEnableConverter : ValueConverterBase<int, bool>
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override bool Convert(int value) =>
            value > 0 && value < 100 ? false : true;

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override int ConvertBack(bool value) => throw new NotImplementedException();
    }
}