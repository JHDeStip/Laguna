using System;
using System.Globalization;
using System.Windows.Media.Imaging;
using System.Windows.Data;

namespace JhDeStip.Laguna.Player.Converters
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class ThumbnailImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(value as string);
                bmp.EndInit();

                return bmp;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
