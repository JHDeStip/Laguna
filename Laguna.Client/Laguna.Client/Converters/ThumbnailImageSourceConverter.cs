using System;
using System.Globalization;

using Xamarin.Forms;

namespace JhDeStip.Laguna.Client.Converters
{
    public class ThumbnailImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var imgsrc = new UriImageSource();
                imgsrc.Uri = new Uri(value as string);

                return imgsrc;
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