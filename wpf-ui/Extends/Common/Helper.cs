using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using wpf_ui.Extends.Ucs;

namespace wpf_ui.Extends.Common
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// 查找指定类型的父控件
        /// </summary>
        /// <typeparam name="T">要查找的控件类型</typeparam>
        /// <param name="root">查找源</param>
        /// <param name="name">要查找的控件名称</param>
        /// <returns></returns>
        public static T FindParent<T>(this DependencyObject root, string name = "") where T : DependencyObject
        {
            if (root == null) return null;
            DependencyObject d = VisualTreeHelper.GetParent(root);
            if (d == null) return null;
            if (!(d is T childType) || (d is FrameworkElement element) && !string.IsNullOrWhiteSpace(name) && element.Name != name)
            {
                return FindParent<T>(d, name);
            }
            return childType;
        }

        /// <summary>
        /// 查找指定类型的子控件
        /// </summary>
        /// <typeparam name="T">要查找的控件类型</typeparam>
        /// <param name="root">查找源</param>
        /// <param name="name">要查找的控件名称</param>
        /// <returns></returns>
        public static T FindChild<T>(this DependencyObject root, string name = "") where T : DependencyObject
        {
            if (root == null) return null;
            T founded = null;
            for (int j = 0; j < VisualTreeHelper.GetChildrenCount(root); j++)
            {
                DependencyObject d = VisualTreeHelper.GetChild(root, j);
                if (!(d is T childType) || d is FrameworkElement element && !string.IsNullOrWhiteSpace(name) && element.Name != name)
                {
                    founded = FindChild<T>(d, name);
                    if (founded != null) break;
                }
                else
                {
                    founded = childType;
                    break;
                }
            }
            return founded;
        }

        /// <summary>
        /// 强化版ToString
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToStringEx(this object obj)
        {
            return obj == null ? "" : obj.ToString();
        }

        /// <summary>
        /// 在指定区域内加载loading
        /// </summary>
        /// <param name="grid">指定区域</param>
        public static void Loading(this Grid grid)
        {
            Alter.Loading(grid);
        }

        /// <summary>
        /// 清除指定区域内的loading
        /// </summary>
        /// <param name="grid">指定区域</param>
        public static void ClearLoading(this Grid grid)
        {
            grid.Children.Remove(grid.FindChild<Border>("loading"));
        }

        /// <summary>
        /// 对指定控件加载Tip
        /// </summary>
        /// <param name="uiElement">指定控件</param>
        /// <param name="content">提示内容</param>
        public static void Tip(this UIElement uiElement, string content)
        {
            Alter.Tip(content, uiElement);
        }

        /// <summary>
        /// 英文星期几转星期索引(0:周一...以此类推)
        /// </summary>
        /// <param name="week">英文星期几</param>
        /// <returns></returns>
        public static int ToWeekIndex(this DayOfWeek week)
        {
            return Convert.ToInt32(week.ToString().Replace("Monday", "1")
                                                  .Replace("Tuesday", "2")
                                                  .Replace("Wednesday", "3")
                                                  .Replace("Thursday", "4")
                                                  .Replace("Friday", "5")
                                                  .Replace("Saturday", "6")
                                                  .Replace("Sunday", "0"));
        }

        /// <summary>
        /// 时间控件
        /// </summary>
        /// <param name="txtBox">指定文本框</param>
        /// <param name="dateTime">初始时间</param>
        public static void DateTime(this TextBox txtBox, DateTime? dateTime = null)
        {
            if (!(txtBox.FindParent<Grid>() is Grid grid)) return;
            grid.Children.Remove(grid.FindChild<Popup>($"Popup_{txtBox.Name}"));
            UcDate ucDate = new UcDate(dateTime ?? System.DateTime.Now);
            Popup popup = new Popup
            {
                Name = $"Popup_{txtBox.Name}",
                PlacementTarget = txtBox,
                Placement = PlacementMode.Bottom,
                AllowsTransparency = true,
                Child = new Canvas
                {
                    Width = 276,
                    Height = 376,
                    Children = { ucDate }
                }
            };
            ucDate.Sure += dt =>
            {
                txtBox.Text = dt.ToString("yyyy-MM-dd");
                popup.IsOpen = false;
            };
            ucDate.Reset += () =>
            {
                txtBox.Clear();
                popup.IsOpen = false;
            };
            grid.Children.Add(popup);
            txtBox.GotFocus += delegate { popup.IsOpen = true; };
            txtBox.LostFocus += (sender, args) =>
            {
                Point point = Mouse.GetPosition(ucDate);
                if (point.X >= 0 && point.X <= ucDate.ActualWidth && point.Y >= 0 && point.Y <= ucDate.ActualHeight) return;
                popup.IsOpen = false;
            };
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static string HttpGet(this string url, string param = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + (param == "" ? "" : "?") + param);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            if (myResponseStream == null) return "";
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        /// <summary>
        /// POST/PUT/DELETE请求
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="postData">参数</param>
        /// <param name="httpType">Http请求方式(POST/PUT/DELETE)</param>
        /// <returns></returns>
        public static string HttpPost(this string url, string postData = "", HttpType httpType = HttpType.Post)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = httpType.ToString();
            request.ContentType = "application/json";
            Encoding encoding = Encoding.UTF8;
            byte[] postDatas = encoding.GetBytes(postData);
            request.ContentLength = postDatas.Length;
            Stream myRequestStream = request.GetRequestStream();
            myRequestStream.Write(postDatas, 0, postDatas.Length);
            myRequestStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            if (myResponseStream == null) return "";
            StreamReader myStreamReader = new StreamReader(myResponseStream, encoding);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        /// <summary>
        /// Http提交类型
        /// </summary>
        public enum HttpType
        {
            Post,
            Put,
            Delete
        }

        /// <summary>
        /// 字符串转化为DateTime
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value)
        {
            if ((value.Length != 10 || !value.Contains("-")) &&
                (value.Length != 19 || !value.Contains("-") || !value.Contains(":")))
            {
                if (value.Length != 8 && value.Length != 14) return null;
                if (value.Length >= 8) value = value.Insert(4, "-").Insert(7, "-");
                if (value.Length == 16) value = value.Insert(10, " ").Insert(13, ":").Insert(16, ":");
            }
            System.DateTime.TryParse(value, out DateTime dateTime);
            return dateTime;
        }
    }
}
