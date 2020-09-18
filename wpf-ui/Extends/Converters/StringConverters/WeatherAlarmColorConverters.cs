using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using wpf_ui.Extends.Common;

namespace wpf_ui.Extends.Converters.StringConverters
{
    public class WeatherAlarmColorConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToStringEx())
            {
                case "黄色":
                    return Brushes.Gold;
                case "橙色":
                    return Brushes.Orange;
                case "红色":
                    return Brushes.OrangeRed;
                default://蓝色
                    return Brushes.DodgerBlue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
