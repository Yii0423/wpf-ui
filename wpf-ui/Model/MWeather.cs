using System.Collections.Generic;

namespace wpf_ui.Model
{
    /// <summary>
    /// 天气
    /// </summary>
    public class MWeather
    {
        /// <summary>
        /// 接口返回-天气结果
        /// </summary>
        public class MWeatherResult
        {
            /// <summary>
            /// 接口返回-天气数据
            /// </summary>
            public MWeatherResultData Data { get; set; }
            /// <summary>
            /// 接口返回-消息
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 接口返回-状态
            /// </summary>
            public string Status { get; set; }
        }

        /// <summary>
        /// 天气数据
        /// </summary>
        public class MWeatherResultData
        {
            /// <summary>
            /// 空气指数
            /// </summary>
            public MWeatherResultDataAir Air { get; set; }
            /// <summary>
            /// 日出日落时间点
            /// </summary>
            public List<MWeatherResultDataRise> Rise { get; set; }
        }

        /// <summary>
        /// 天气数据-空气指数
        /// </summary>
        public class MWeatherResultDataAir
        {
            public int Aqi { get; set; }
            public int Aqi_Level { get; set; }
            public string Aqi_Name { get; set; }
            public string Co { get; set; }
            public string No2 { get; set; }
            public string O3 { get; set; }
            public string Pm10 { get; set; }
            public string Pm2_5 { get; set; }
            public string So2 { get; set; }
            public string Update_Time { get; set; }
        }

        /// <summary>
        /// 天气数据-日出日落时间点
        /// </summary>
        public class MWeatherResultDataRise
        {
            /// <summary>
            /// 日出
            /// </summary>
            public string Sunrise { get; set; }
            /// <summary>
            /// 日落
            /// </summary>
            public string Sunset { get; set; }
            /// <summary>
            /// 日期
            /// </summary>
            public string Time { get; set; }
        }
    }
}
