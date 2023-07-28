using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Plum.Windows.Convertors
{
    /// <summary>
    /// Uri To ImageSource Converter
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class ByteToImageSourceConverter : ValueConverterBase<byte[], BitmapImage>
    {
        protected override BitmapImage Convert(byte[] value)
        {
            if (value is null || value.Length == 0)
                return null;

            BitmapImage bmp = null;
            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(value);
                bmp.EndInit();
            }
            catch
            {
                return null;
            }

            return bmp;
        }

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        protected override byte[] ConvertBack(BitmapImage value) => throw new NotImplementedException();
    }
}