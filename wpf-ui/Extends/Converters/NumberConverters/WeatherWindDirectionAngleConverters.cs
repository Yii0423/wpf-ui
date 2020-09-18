using System;
using System.Globalization;
using System.Windows.Data;
using wpf_ui.Model;

namespace wpf_ui.Extends.Converters.NumberConverters
{
    public class WeatherWindDirectionAngleConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return -45;
            switch ((Model.MWeather.WindDirection)value)
            {
                case MWeather.WindDirection.北:
                    return -45 + 180;
                case MWeather.WindDirection.东北:
                    return 0 + 180;
                case MWeather.WindDirection.东:
                    return 45 + 180;
                case MWeather.WindDirection.东南:
                    return 90 + 180;
                case MWeather.WindDirection.南:
                    return 135 + 180;
                case MWeather.WindDirection.西南:
                    return 180 + 180;
                case MWeather.WindDirection.西:
                    return 225 + 180;
                case MWeather.WindDirection.西北:
                    return 270 + 180;
                default:
                    return -45 + 180;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
