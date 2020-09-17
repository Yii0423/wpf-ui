using System.Collections.Generic;

namespace wpf_ui.Model
{
    /// <summary>
    /// 天气
    /// </summary>
    public class MWeather
    {
        #region 空气结果

        public class MWeatherAirResult
        {
            /// <summary>
            /// 接口返回-空气数据
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

        public class MWeatherResultDataAir
        {
            /// <summary>
            /// 空气质量指数
            /// </summary>
            public int Aqi { get; set; }
            /// <summary>
            /// 空气质量指数水平
            /// </summary>
            public int Aqi_Level { get; set; }
            /// <summary>
            /// 空气质量
            /// </summary>
            public string Aqi_Name { get; set; }
            /// <summary>
            /// 一氧化碳
            /// </summary>
            public string Co { get; set; }
            /// <summary>
            /// 二氧化氮
            /// </summary>
            public string No2 { get; set; }
            /// <summary>
            /// 臭氧
            /// </summary>
            public string O3 { get; set; }
            /// <summary>
            /// PM10
            /// </summary>
            public string Pm10 { get; set; }
            /// <summary>
            /// PM2.5
            /// </summary>
            public string Pm2_5 { get; set; }
            /// <summary>
            /// 二氧化硫
            /// </summary>
            public string So2 { get; set; }
            /// <summary>
            /// 更新时间
            /// </summary>
            public string Update_Time { get; set; }
        }

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

        #endregion

        #region 天气结果

        public class MWeatherResult
        {
            /// <summary>
            /// 接口返回-天气数据
            /// </summary>
            public MWeatherResultData2 Data { get; set; }
            /// <summary>
            /// 接口返回-消息
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 接口返回-状态
            /// </summary>
            public string Status { get; set; }
        }

        public class MWeatherResultData2
        {
            /// <summary>
            /// 预警信息
            /// </summary>
            public List<MWeatherResultDataAlarm> Alarm { get; set; }
            /// <summary>
            /// 未来24小时预报
            /// </summary>
            public List<MWeatherResultDataForecast1H> Forecast_1H { get; set; }
            /// <summary>
            /// 未来一周预报
            /// </summary>
            public List<MWeatherResultDataForecast24H> Forecast_24H { get; set; }
            /// <summary>
            /// 指数
            /// </summary>
            public MWeatherResultDataIndex Index { get; set; }
            /// <summary>
            /// 限行
            /// </summary>
            public MWeatherResultDataLimit Limit { get; set; }
            /// <summary>
            /// 实时天气信息
            /// </summary>
            public MWeatherResultDataObserve Observe { get; set; }
            /// <summary>
            /// 日出日落
            /// </summary>
            public List<MWeatherResultDataRise> Rise { get; set; }
            /// <summary>
            /// 温馨提示
            /// </summary>
            public MWeatherResultDataTips Tips { get; set; }
        }

        public class MWeatherResultDataAlarm
        {
            public string City { get; set; }
            public string County { get; set; }
            public string Detail { get; set; }
            public string Info { get; set; }
            public string Level_Code { get; set; }
            public string Level_Name { get; set; }
            public string Province { get; set; }
            public string Type_Code { get; set; }
            public string Type_Name { get; set; }
            public string Update_Time { get; set; }
            public string Url { get; set; }
        }

        public class MWeatherResultDataForecast1H
        {
            public string Degree { get; set; }
            public string Update_Time { get; set; }
            public string Weather { get; set; }
            public string Weather_Code { get; set; }
            public string Weather_Short { get; set; }
            public string Wind_Direction { get; set; }
            public string Wind_Power { get; set; }
        }

        public class MWeatherResultDataForecast24H
        {
            public string Day_Weather { get; set; }
            public string Day_Weather_Code { get; set; }
            public string Day_Weather_Short { get; set; }
            public string Day_Wind_Direction { get; set; }
            public string Day_Wind_Direction_Code { get; set; }
            public string Day_Wind_Power { get; set; }
            public string Day_Wind_Power_Code { get; set; }
            public string Max_Degree { get; set; }
            public string Min_Degree { get; set; }
            public string Night_Weather { get; set; }
            public string Night_Weather_Code { get; set; }
            public string Night_Weather_Short { get; set; }
            public string Night_Wind_Direction { get; set; }
            public string Night_Wind_Direction_Code { get; set; }
            public string Night_Wind_Power { get; set; }
            public string Night_Wind_Power_Code { get; set; }
            public string Time { get; set; }
        }

        public class MWeatherResultDataIndex
        {
            public MWeatherResultDataIndexInfo Airconditioner { get; set; }
            public MWeatherResultDataIndexInfo Allergy { get; set; }
            public MWeatherResultDataIndexInfo Carwash { get; set; }
            public MWeatherResultDataIndexInfo Chill { get; set; }
            public MWeatherResultDataIndexInfo Clothes { get; set; }
            public MWeatherResultDataIndexInfo Cold { get; set; }
            public MWeatherResultDataIndexInfo Comfort { get; set; }
            public MWeatherResultDataIndexInfo Diffusion { get; set; }
            public MWeatherResultDataIndexInfo Dry { get; set; }
            public MWeatherResultDataIndexInfo Drying { get; set; }
            public MWeatherResultDataIndexInfo Fish { get; set; }
            public MWeatherResultDataIndexInfo Heatstroke { get; set; }
            public MWeatherResultDataIndexInfo Makeup { get; set; }
            public MWeatherResultDataIndexInfo Mood { get; set; }
            public MWeatherResultDataIndexInfo Morning { get; set; }
            public MWeatherResultDataIndexInfo Sports { get; set; }
            public MWeatherResultDataIndexInfo Sunglasses { get; set; }
            public MWeatherResultDataIndexInfo Sunscreen { get; set; }
            public MWeatherResultDataIndexInfo Tourism { get; set; }
            public MWeatherResultDataIndexInfo Traffic { get; set; }
            public MWeatherResultDataIndexInfo Ultraviolet { get; set; }
            public MWeatherResultDataIndexInfo Umbrella { get; set; }
            public string Time { get; set; }
        }

        public class MWeatherResultDataIndexInfo
        {
            public string Detail { get; set; }
            public string Info { get; set; }
            public string Name { get; set; }
        }

        public class MWeatherResultDataLimit
        {
            public string Tail_Number { get; set; }
            public string Time { get; set; }
        }

        public class MWeatherResultDataObserve
        {
            /// <summary>
            /// 实时温度
            /// </summary>
            public string Degree { get; set; }
            /// <summary>
            /// 湿度
            /// </summary>
            public string Humidity { get; set; }
            /// <summary>
            /// 降水量
            /// </summary>
            public string Precipitation { get; set; }
            /// <summary>
            /// 压强
            /// </summary>
            public string Pressure { get; set; }
            /// <summary>
            /// 更新时间
            /// </summary>
            public string Update_Time { get; set; }
            /// <summary>
            /// 天气情况(完整)
            /// </summary>
            public string Weather { get; set; }
            /// <summary>
            /// 天气代码
            /// </summary>
            public string Weather_Code { get; set; }
            /// <summary>
            /// 天气情况(简化)
            /// </summary>
            public string Weather_Short { get; set; }
            /// <summary>
            /// 风向
            /// </summary>
            public WindDirection Wind_Direction { get; set; }
            /// <summary>
            /// 风力
            /// </summary>
            public string Wind_Power { get; set; }
        }

        public class MWeatherResultDataTips
        {
            public List<string> Observe { get; set; }
            public List<string> Forecast_1H { get; set; }
            public List<string> Forecast_24H { get; set; }
        }

        public enum WindDirection
        {
            东北 = 1,
            东 = 2,
            东南 = 3,
            南 = 4,
            西南 = 5,
            西 = 6,
            西北 = 7,
            北 = 8
        }

        #endregion
    }
}
