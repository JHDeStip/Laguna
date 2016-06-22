using System;
using System.Globalization;
using System.Windows.Data;
using Stannieman.AudioPlayer;

namespace JhDeStip.Laguna.Player.Converters
{
    [ValueConversion(typeof(TrackPosition), typeof(int))]
    public class PositionPerMilleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TrackPosition)
            {
                var position = (TrackPosition)value;
                if (position.Duration.Ticks == 0)
                    return 0;
                return (int)Math.Round(((double)position.CurrentTime.Ticks / (double)position.Duration.Ticks) * 1000);
            }
            else
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
