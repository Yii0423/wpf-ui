using System;
using System.Globalization;
using System.Windows.Data;
using wpf_ui.Extends.Common;

namespace wpf_ui.Extends.Converters.StringConverters
{
    public class WeatherTimeForChineseConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? dt = value.ToStringEx().ToDateTime();
            if (dt == null) return "";
            switch (dt.Value.Day - DateTime.Now.Day)
            {
                case -1:
                    return "昨天";
                case 0:
                    return "今天";
                case 1:
                    return "明天";
                case 2:
                    return "后天";
                default:
                    return dt.Value.DayOfWeek.ToStringEx().Replace("Monday", "周一")
                                                          .Replace("Tuesday", "周二")
                                                          .Replace("Wednesday", "周三")
                                                          .Replace("Thursday", "周四")
                                                          .Replace("Friday", "周五")
                                                          .Replace("Saturday", "周六")
                                                          .Replace("Sunday", "周日");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
