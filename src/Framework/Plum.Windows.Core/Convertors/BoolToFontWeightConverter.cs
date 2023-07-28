using System;
using System.Windows;

namespace Plum.Windows.Convertors
{
    public class BoolToFontWeightConverter : ValueConverterBase<bool, FontWeight>
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override FontWeight Convert(bool value) =>
            value ? FontWeights.Bold : FontWeights.Regular;

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override bool ConvertBack(FontWeight value) => throw new NotImplementedException();
    }

    public class NotBoolToFontWeightConverter : ValueConverterBase<bool, FontWeight>
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override FontWeight Convert(bool value) =>
            value ? FontWeights.Regular : FontWeights.Bold;

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override bool ConvertBack(FontWeight value) => throw new NotImplementedException();
    }
}