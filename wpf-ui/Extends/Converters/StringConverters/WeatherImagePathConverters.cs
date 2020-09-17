using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace wpf_ui.Extends.Converters.StringConverters
{
    /// <summary>
    /// 图片路径转换
    /// </summary>
    public class WeatherImagePathConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BitmapImage(new Uri($"/Content/Images/weather/{value}.png", UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
