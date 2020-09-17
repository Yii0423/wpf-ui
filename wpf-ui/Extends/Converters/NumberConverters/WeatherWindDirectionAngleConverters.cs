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
                    return -45;
                case MWeather.WindDirection.东北:
                    return 0;
                case MWeather.WindDirection.东:
                    return 45;
                case MWeather.WindDirection.东南:
                    return 90;
                case MWeather.WindDirection.南:
                    return 135;
                case MWeather.WindDirection.西南:
                    return 180;
                case MWeather.WindDirection.西:
                    return 225;
                case MWeather.WindDirection.西北:
                    return 270;
                default:
                    return -45;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
