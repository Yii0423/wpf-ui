using System;
using System.Globalization;
using System.Windows.Data;
using wpf_ui.Extends.Common;

namespace wpf_ui.Extends.Converters.StringConverters
{
    /// <summary>
    /// 时间字符串转换
    /// </summary>
    public class WeatherUpdateTimeConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToStringEx().ToDateTime()?.ToString(parameter.ToStringEx());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
