using Plum.Extensions;
using System;

namespace Plum.Windows.Convertors
{
    public class EnumDescriptionConverter : ValueConverterBase<Enum, string>
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override string Convert(Enum value) =>
            value.GetDisplayName();

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override Enum ConvertBack(string value) => throw new NotImplementedException();
    }
}