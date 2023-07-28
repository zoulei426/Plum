namespace Plum.Windows.Convertors
{
    /// <summary>
    /// 非转换器
    /// </summary>
    /// <seealso cref="Plum.Windows.Convertors.ValueConverterBase{System.Boolean, System.Boolean}" />
    public class NotConverter : ValueConverterBase<bool, bool>
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override bool Convert(bool value) => !value;

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override bool ConvertBack(bool value) => Convert(value);
    }
}