using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using wpf_ui.Extends.Common;
using wpf_ui.Model;

namespace wpf_ui.ViewModel
{
    /// <summary>
    /// 天气
    /// </summary>
    public class WeatherViewModel : ViewModelBase
    {
        private string _province;

        /// <summary>
        /// 省
        /// </summary>
        public string Province
        {
            get => _province;
            set => Set("Province", ref _province, value);
        }

        private string _city;

        /// <summary>
        /// 市
        /// </summary>
        public string City
        {
            get => _city;
            set
            {
                Set("City", ref _city, value);
                if (!string.IsNullOrWhiteSpace(_city)) LoadWeather();
            }
        }

        private List<MAddress> _provinces;

        /// <summary>
        /// 省集合
        /// </summary>
        public List<MAddress> Provinces
        {
            get => _provinces;
            set => Set("Provinces", ref _provinces, value);
        }

        private List<MAddress> _cities;

        /// <summary>
        /// 市集合
        /// </summary>
        public List<MAddress> Cities
        {
            get => _cities;
            set => Set("Cities", ref _cities, value);
        }

        private MWeather.MWeatherAirResult _weatherAir;

        /// <summary>
        /// 空气
        /// </summary>
        public MWeather.MWeatherAirResult WeatherAir
        {
            get => _weatherAir;
            set => Set("WeatherAir", ref _weatherAir, value);
        }

        private MWeather.MWeatherResult _weather;

        /// <summary>
        /// 天气
        /// </summary>
        public MWeather.MWeatherResult Weather
        {
            get => _weather;
            set => Set("Weather", ref _weather, value);
        }

        private string _tip;

        /// <summary>
        /// 提示
        /// </summary>
        public string Tip
        {
            get => _tip;
            set => Set("Tip", ref _tip, value);
        }

        private bool _popup1IsOpen;

        /// <summary>
        /// 省份Popup开关
        /// </summary>
        public bool Popup1IsOpen
        {
            get => _popup1IsOpen;
            set => Set("Popup1IsOpen", ref _popup1IsOpen, value);
        }

        private bool _popup2IsOpen;

        /// <summary>
        /// 市级Popup开关
        /// </summary>
        public bool Popup2IsOpen
        {
            get => _popup2IsOpen;
            set => Set("Popup2IsOpen", ref _popup2IsOpen, value);
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public WeatherViewModel()
        {
            LoadAddress();
        }

        /// <summary>
        /// 加载地址信息
        /// </summary>
        private void LoadAddress()
        {
            Provinces = GetAddresses(1);
            if (Provinces == null || !Provinces.Any()) return;
            Cities = GetAddresses(Provinces.First().Id);
            if (Cities == null || !Cities.Any()) return;
            Province = Provinces.First().Name;
            City = Cities.First().Name;
        }

        /// <summary>
        /// 加载天气信息
        /// </summary>
        private void LoadWeather()
        {
            Messenger.Default.Send("");
            new Thread(() =>
            {
                try
                {
                    //空气
                    string result = "https://wis.qq.com/weather/common".HttpGet($"source=pc&weather_type=air%7Crise&province={Province}&city={City}");
                    result = result.Replace("\"pm2.5\":", "\"pm2_5\":");
                    result = result.Replace("\"rise\":{", "\"rise\":[");
                    for (int i = 0; i <= 15; i++) result = result.Replace($"\"{i}\":", "");
                    result = result.Replace("}}}", "}]}");
                    WeatherAir = Newtonsoft.Json.JsonConvert.DeserializeObject<MWeather.MWeatherAirResult>(result);
                    //天气
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
                    Tip = Weather.Data.Tips.Observe.FirstOrDefault();
                    _tipIndex = 0;
                }
                catch (Exception)
                {
                    Province = "获取天气信息失败，请稍后重试哦";
                    City = "";
                }
            }).Start();
        }

        /// <summary>
        /// 根据父地址ID获取子地址集合
        /// </summary>
        /// <param name="pid">父地址ID</param>
        /// <returns></returns>
        private List<MAddress> GetAddresses(int pid)
        {
            string result = "http://106.12.154.196/api/address".HttpGet($"pid={pid}");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<MAddress>>(result);
        }

        public RelayCommand<string> OpenPopup
        {
            get
            {
                return new RelayCommand<string>(index =>
                {
                    switch (index)
                    {
                        case "1":
                            Popup1IsOpen = true;
                            break;
                        case "2":
                            Popup2IsOpen = true;
                            break;
                    }
                });
            }
        }

        public RelayCommand<int> LoadChildAddresses
        {
            get
            {
                return new RelayCommand<int>(id =>
               {
                   Province = Provinces.FirstOrDefault(m => m.Id == id)?.Name;
                   if (string.IsNullOrWhiteSpace(Province)) return;
                   Cities = GetAddresses(id);
                   if (Cities == null || !Cities.Any()) return;
                   City = Cities.First().Name;
                   Popup1IsOpen = false;
               });
            }
        }

        public RelayCommand<int> CityForWeather
        {
            get
            {
                return new RelayCommand<int>(id =>
               {
                   City = Cities.FirstOrDefault(m => m.Id == id)?.Name;
                   Popup2IsOpen = false;
               });
            }
        }

        private int _tipIndex;
        public RelayCommand ChangeTips
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _tipIndex++;
                    List<string> tips = Weather.Data.Tips.Observe;
                    if (_tipIndex >= tips.Count) _tipIndex = 0;
                    Tip = tips[_tipIndex];
                });
            }
        }
    }
}