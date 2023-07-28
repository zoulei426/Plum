namespace Plum.Windows.Convertors
{
    public class ObjectToSingleLineStringConverter : ValueConverterBase<object, string>
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override string Convert(object value) =>
            value?.ToString().Replace("\r\n", " ").Replace("\n", " ");

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override object ConvertBack(string value) => Convert(value);
    }
}