using System;
using System.Threading;
using GalaSoft.MvvmLight;
using wpf_ui.Extends.Common;
using wpf_ui.Model;

namespace wpf_ui.ViewModel
{
    /// <summary>
    /// ����
    /// </summary>
    public class WeatherViewModel : ViewModelBase
    {
        private string _province;

        /// <summary>
        /// ʡ
        /// </summary>
        public string Province
        {
            get => _province;
            set => Set("Province", ref _province, value);
        }

        private string _city;

        /// <summary>
        /// ��
        /// </summary>
        public string City
        {
            get => _city;
            set => Set("City", ref _city, value);
        }

        private MWeather.MWeatherAirResult _weatherAir;

        /// <summary>
        /// ����
        /// </summary>
        public MWeather.MWeatherAirResult WeatherAir
        {
            get => _weatherAir;
            set => Set("WeatherAir", ref _weatherAir, value);
        }

        private MWeather.MWeatherResult _weather;

        /// <summary>
        /// ����
        /// </summary>
        public MWeather.MWeatherResult Weather
        {
            get => _weather;
            set => Set("Weather", ref _weather, value);
        }

        /// <summary>
        /// ���췽��
        /// </summary>
        public WeatherViewModel()
        {
            Province = "����";
            City = "";

            new Thread(() =>
            {
                try
                {
                    //����
                    string result = "https://wis.qq.com/weather/common".HttpGet($"source=pc&weather_type=air%7Crise&province={Province}&city={City}");
                    result = result.Replace("\"pm2.5\":", "\"pm2_5\":");
                    result = result.Replace("\"rise\":{", "\"rise\":[");
                    for (int i = 0; i <= 15; i++) result = result.Replace($"\"{i}\":", "");
                    result = result.Replace("}}}", "}]}");
                    WeatherAir = Newtonsoft.Json.JsonConvert.DeserializeObject<MWeather.MWeatherAirResult>(result);
                    //����
                    result = "https://wis.qq.com/weather/common".HttpGet($"source=pc&weather_type=observe%7Cforecast_1h%7Cforecast_24h%7Cindex%7Calarm%7Climit%7Ctips%7Crise&province={Province}&city={City}");
                    result = result.Replace("\"alarm\":{", "\"alarm\":[");
                    result = result.Replace("},\"forecast_1h\":{", "],\"forecast_1h\":[");
                    result = result.Replace("},\"forecast_24h\":{", "],\"forecast_24h\":[");
                    result = result.Replace("},\"index\":{", "],\"index\":{");
                    result = result.Replace("\"rise\":{", "\"rise\":[");
                    result = result.Replace("},\"tips\":{", "],\"tips\":{");
                    result = result.Replace("\"forecast_1h\":{", "\"forecast_1h\":[");
                    result = result.Replace("\"forecast_24h\":{", "\"forecast_24h\":[");
                    result = result.Replace("},\"observe\":{\"0\"", "],\"observe\":[\"0\"");
                    result = result.Replace("\"observe\":{\"0\"", "\"observe\":[\"0\"");
                    for (int i = 0; i <= 50; i++) result = result.Replace($"\"{i}\":", "");
                    result = result.Replace("}}}", "]}}");
                    Weather = Newtonsoft.Json.JsonConvert.DeserializeObject<MWeather.MWeatherResult>(result);
                }
                catch (Exception)
                {
                    Province = "";
                    City = "��ȡ������Ϣʧ�ܣ����Ժ�����Ŷ";
                }
            }).Start();
        }
    }
}