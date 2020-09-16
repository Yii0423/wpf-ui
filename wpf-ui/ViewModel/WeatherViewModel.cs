using GalaSoft.MvvmLight;
using wpf_ui.Extends.Common;
using wpf_ui.Model;

namespace wpf_ui.ViewModel
{
    /// <summary>
    /// 天气
    /// </summary>
    public class WeatherViewModel : ViewModelBase
    {
        private MWeather.MWeatherResult _weather2;

        public MWeather.MWeatherResult Weather2
        {
            get => _weather2;
            set => Set("Weather2", ref _weather2, value);
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public WeatherViewModel()
        {
            string result = "https://wis.qq.com/weather/common".HttpGet("source=pc&weather_type=air%7Crise&province=浙江&city=杭州");
            result = result.Replace("\"pm2.5\":", "\"pm2_5\":");
            result = result.Replace("\"rise\":{", "\"rise\":[");
            result = result.Replace("\"0\":", "");
            result = result.Replace("\"1\":", "");
            result = result.Replace("\"2\":", "");
            result = result.Replace("\"3\":", "");
            result = result.Replace("\"4\":", "");
            result = result.Replace("\"5\":", "");
            result = result.Replace("\"6\":", "");
            result = result.Replace("\"7\":", "");
            result = result.Replace("\"8\":", "");
            result = result.Replace("\"9\":", "");
            result = result.Replace("\"10\":", "");
            result = result.Replace("\"11\":", "");
            result = result.Replace("\"12\":", "");
            result = result.Replace("\"13\":", "");
            result = result.Replace("\"14\":", "");
            result = result.Replace("\"15\":", "");
            result = result.Replace("}}}", "}]}");
            Weather2 = Newtonsoft.Json.JsonConvert.DeserializeObject<MWeather.MWeatherResult>(result);
        }
    }
}