using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using wpf_ui.Extends.Common;
using wpf_ui.Extends.DiyControls;
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
            SpForecast1H.Children.Clear();
            mWeatherResultDataForecast1Hs.OrderBy(m => m.Update_Time.ToDateTime()).ToList().ForEach(m =>
            {
                SpForecast1H.Children.Add(new UcForecast1H { DataContext = m });
            });
        }

        private void SpForecast24H_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(SpForecast24H.DataContext is List<MWeather.MWeatherResultDataForecast24H> mWeatherResultDataForecast24Hs)
            ) return;
            SpForecast24H.Children.Clear();
            mWeatherResultDataForecast24Hs.OrderBy(m => m.Time.ToDateTime()).ToList().ForEach(m =>
            {
                SpForecast24H.Children.Add(new UcForecast24H { DataContext = m });
            });
        }

        private void BtnAqi_OnClick(object sender, RoutedEventArgs e)
        {
            PopupAqi.IsOpen = true;
        }

        private void AlarmButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is DiyButton diyButton) || !(diyButton.FindChild<Popup>() is Popup popup)) return;
            popup.PlacementTarget = diyButton;
            popup.IsOpen = true;
        }
    }
}
