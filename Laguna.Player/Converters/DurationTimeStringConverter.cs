using System;
using System.Globalization;
using System.Windows.Data;

namespace JhDeStip.Laguna.Player.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class DurationTimeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan)
            {
                var duration = (TimeSpan)value;
                var minutes = (duration.Hours * 60 + duration.Minutes).ToString();
                var seconds = (duration.Seconds).ToString();

                if (minutes.Length < 2)
                    minutes = "0" + minutes;

                if (seconds.Length < 2)
                    seconds = "0" + seconds;
                
                return minutes + ":" + seconds;
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
