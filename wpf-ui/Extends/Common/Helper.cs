using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            return Convert.ToInt32(week.ToString().Replace("Monday", "0")
                                                  .Replace("Tuesday", "1")
                                                  .Replace("Wednesday", "2")
                                                  .Replace("Thursday", "3")
                                                  .Replace("Friday", "4")
                                                  .Replace("Saturday", "5")
                                                  .Replace("Sunday", "6"));
        }

        /// <summary>
        /// 时间控件
        /// </summary>
        /// <param name="txtBox">指定文本框</param>
        public static void DateTime(this TextBox txtBox)
        {
            if (!(txtBox.FindParent<Grid>() is Grid grid)) return;
            grid.Children.Remove(grid.FindChild<Popup>($"Popup_{txtBox.Name}"));
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
                    Children = { new UcDate() }
                }
            };
            grid.Children.Add(popup);
            txtBox.GotFocus += delegate { popup.IsOpen = true; };
            txtBox.LostFocus += delegate { popup.IsOpen = false; };
        }
    }
}
