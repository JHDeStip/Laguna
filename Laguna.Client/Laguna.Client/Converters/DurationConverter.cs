using System;
using System.Text;
using System.Globalization;

using Xamarin.Forms;

namespace JhDeStip.Laguna.Client.Converters
{
    public class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan)
            {
                var duration = (TimeSpan)value;
                StringBuilder builder = new StringBuilder();

                if (duration.Hours > 0)
                    builder.Append(duration.Hours + "h");

                if (duration.Minutes > 0 || builder.Length > 0)
                    builder.Append(duration.Minutes + "m");

                builder.Append(duration.Seconds + "s");

                return builder.ToString();
            }
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
