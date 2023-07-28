namespace Plum.Windows.Convertors
{
    public class StringToBlankConverter : ValueConverterBase<string, string>
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override string Convert(string value) => value.IsNullOrEmpty() ? string.Empty : "      ";

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override string ConvertBack(string value) => Convert(value);
    }
}