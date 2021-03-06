﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using wpf_ui.Extends.Common;

namespace wpf_ui.Extends.Converters.StringConverters
{
    /// <summary>
    /// 图片路径转换
    /// </summary>
    public class WeatherImagePathConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string folder = string.IsNullOrWhiteSpace(parameter.ToStringEx()) ? "weather" : parameter.ToStringEx();
            return new BitmapImage(new Uri($"/Content/Images/{folder}/{value}.png", UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
