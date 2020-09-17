using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using wpf_ui.Extends.Common;
using wpf_ui.Extends.Ucs;
using wpf_ui.Model;

namespace wpf_ui.View
{
    /// <summary>
    /// Weather.xaml 的交互逻辑
    /// </summary>
    public partial class Weather : Page
    {
        public Weather()
        {
            InitializeComponent();
        }

        private void SpForecast1H_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(SpForecast1H.DataContext is List<MWeather.MWeatherResultDataForecast1H> mWeatherResultDataForecast1Hs)
            ) return;
            mWeatherResultDataForecast1Hs.OrderBy(m => m.Update_Time.ToDateTime()).ToList().ForEach(m =>
            {
                SpForecast1H.Children.Add(new UcForecast1H { DataContext = m });
            });
        }

        private void SpForecast24H_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(SpForecast24H.DataContext is List<MWeather.MWeatherResultDataForecast24H> mWeatherResultDataForecast24Hs)
            ) return;
            mWeatherResultDataForecast24Hs.OrderBy(m => m.Time.ToDateTime()).ToList().ForEach(m =>
            {
                SpForecast24H.Children.Add(new UcForecast24H { DataContext = m });
            });
        }
    }
}
