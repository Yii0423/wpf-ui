using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using wpf_ui.Extends.DiyControls;
using wpf_ui.View.Alters;

namespace wpf_ui.Extends.Common
{
    /// <summary>
    /// 消息
    /// </summary>
    public class Alter
    {
        /// <summary>
        /// 记录上一条消息(避免重叠显示)
        /// </summary>
        private static Label _msgLabel;

        /// <summary>
        /// 显示一个会自动关闭的消息弹窗
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="frameworkElement">显示消息的窗体</param>
        /// <param name="time">自动关闭的时间(默认3秒钟)</param>
        public static void Msg(object content, FrameworkElement frameworkElement, double time = 3)
        {
            if (string.IsNullOrWhiteSpace(content.ToStringEx()) || frameworkElement == null) return;
            Grid parentGrid = frameworkElement.FindChild<Grid>();
            if (parentGrid == null) return;
            if (_msgLabel != null) parentGrid.Children.Remove(_msgLabel);
            Label label = new Label
            {
                Height = 50,
                Content = content,
                Foreground = Brushes.White,
                Padding = new Thickness(15, 0, 15, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = new SolidColorBrush
                {
                    Color = Colors.Black,
                    Opacity = 0.5d
                },
                Opacity = 0d,
                RenderTransform = new ScaleTransform
                {
                    ScaleX = 0.7d,
                    ScaleY = 0.7d
                },
                RenderTransformOrigin = new Point(0.5, 0.5),
                IsHitTestVisible = false
            };
            //解决缩放过程中的字体模糊问题
            TextOptions.SetTextFormattingMode(label, TextFormattingMode.Display);
            parentGrid.Children.Add(label);
            _msgLabel = label;
            label.Loaded += delegate
            {
                //淡入显示
                label.ScaleIn(animateCompleted: () =>
                {
                    DispatcherTimer dTmr = new DispatcherTimer { Interval = TimeSpan.FromSeconds(time) };
                    dTmr.Tick += delegate
                    {
                        //淡出消失
                        label.ScaleOut(animateCompleted: () =>
                        {
                            (Client.MainShade.Parent as Grid)?.Children.Remove(label);
                            dTmr.Stop();
                        });
                        label.FadeOut();
                    };
                    dTmr.Start();
                });
                label.FadeIn();
            };
        }

        /// <summary>
        /// 记录上一个Popup(避免重复添加)
        /// </summary>
        private static Popup _tipPopup;

        /// <summary>
        /// 显示一个与控件绑定且有内容的提示窗体
        /// </summary>
        /// <param name="content">提示内容</param>
        /// <param name="uiElement">指定控件</param>
        /// <param name="showPlace">相对于控件的提示位置</param>
        /// <param name="maxWidth">显示最大宽度</param>
        public static void Tip(string content, UIElement uiElement, ShowPlace showPlace = ShowPlace.Top, double maxWidth = 200)
        {
            if (string.IsNullOrWhiteSpace(content)) return;
            Grid parentGrid = uiElement.FindParent<Grid>();
            if (parentGrid == null) return;
            if (_tipPopup != null) parentGrid.Children.Remove(_tipPopup);
            _tipPopup = new Popup
            {
                AllowsTransparency = true,
                StaysOpen = false,
                Placement = (PlacementMode)(int)showPlace,
                PlacementTarget = uiElement,
                PopupAnimation = PopupAnimation.Fade,
                IsOpen = true,
                Child = new DiyPopup
                {
                    ShowPlace = showPlace,
                    Content = new TextBlock
                    {
                        Text = content,
                        TextWrapping = TextWrapping.Wrap,
                        MaxWidth = maxWidth,
                    },
                    IsHitTestVisible = false
                }
            };
            parentGrid.Children.Add(_tipPopup);
        }

        /// <summary>
        /// 显示一个需要确认的弹窗(可自定义窗体按钮)
        /// </summary>
        /// <param name="content">提示内容</param>
        /// <param name="title">提示标题</param>
        /// <param name="list">自定义按钮</param>
        public static bool Confirm(string content, string title = "系统确认", List<DiyButton> list = null)
        {
            AConfirm confirm = new AConfirm
            {
                Title = title,
                Content = content,
                Btns = list
            };
            confirm.ShowDialog();
            return confirm.Result;
        }

        /// <summary>
        /// 显示一个需要反馈的弹窗(可自定义窗体按钮)
        /// </summary>
        /// <param name="title"></param>
        public static string Prompt(string title)
        {
            APrompt prompt = new APrompt { Title = title };
            prompt.ShowDialog();
            return prompt.Content;
        }

        /// <summary>
        /// 在指定区域内显示一个正在加载的弹窗
        /// </summary>
        /// <param name="grid">指定区域</param>
        /// <param name="imageIndex">加载中图片索引</param>
        public static void Loading(Grid grid, int imageIndex = 3)
        {
            if (grid.FindChild<Border>("loading") != null) return;
            Image image = new Image
            {
                Source = new BitmapImage(new Uri($"/Content/Images/loading/loading{imageIndex}.png", UriKind.RelativeOrAbsolute)),
                MaxWidth = 32,
                MaxHeight = 32,
                Margin = new Thickness(0, 10, 0, 10),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                RenderTransform = new RotateTransform { Angle = 0d },
                RenderTransformOrigin = new Point(0.5, 0.5)
            };
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
            Border border = new Border
            {
                Name = "loading",
                Background = new SolidColorBrush
                {
                    Color = Colors.Black,
                    Opacity = 0.5d
                },
                Child = image
            };
            grid.Children.Add(border);
            int rows = grid.RowDefinitions.Count;
            int cols = grid.ColumnDefinitions.Count;
            if (rows != 0) Grid.SetRowSpan(border, rows);
            if (cols != 0) Grid.SetColumnSpan(border, cols);
            image.Rotate(3);
        }

        /// <summary>
        /// 打开一个含有指向链接Frame的弹窗
        /// </summary>
        /// <param name="title">弹窗标题</param>
        /// <param name="url">Frame链接</param>
        public static bool Open(string title, Uri url)
        {
            AOpen open = new AOpen { Title = title, Url = url };
            open.ShowDialog();
            return open.Result;
        }
    }
}
