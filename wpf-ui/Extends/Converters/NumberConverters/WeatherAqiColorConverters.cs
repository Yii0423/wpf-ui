using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using wpf_ui.Extends.Common;

namespace wpf_ui.Extends.Converters.NumberConverters
{
    public class WeatherAqiColorConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!double.TryParse(value.ToStringEx(), out double aqi)) return Brushes.Green;
            if (aqi >= 51 && aqi <= 100) return Brushes.YellowGreen;
            if (aqi >= 101 && aqi <= 150) return Brushes.Orange;
            if (aqi >= 151 && aqi <= 200) return Brushes.OrangeRed;
            if (aqi >= 201 && aqi <= 300) return Brushes.Purple;
            if (aqi >= 301 && aqi <= 500) return Brushes.Brown;
            return Brushes.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
