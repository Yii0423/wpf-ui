using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using wpf_ui.Extends.DiyControls;

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
        /// <param name="time">自动关闭的时间(默认3秒钟)</param>
        public static void Msg(string content, double time = 3)
        {
            if (string.IsNullOrWhiteSpace(content)) return;
            if (_msgLabel != null) (Client.MainShade.Parent as Grid)?.Children.Remove(_msgLabel);
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
            (Client.MainShade.Parent as Grid)?.Children.Add(label);
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
        /// <param name="uiElement">指定控件</param>
        /// <param name="content">提示内容</param>
        /// <param name="showPlace">相对于控件的提示位置</param>
        /// <param name="maxWidth">显示最大宽度</param>
        public static void Tip(UIElement uiElement, string content, ShowPlace showPlace = ShowPlace.Top, double maxWidth = 200)
        {
            if (string.IsNullOrWhiteSpace(content)) return;
            if (_tipPopup != null) (Client.MainShade.Parent as Grid)?.Children.Remove(_tipPopup);
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
            (Client.MainShade.Parent as Grid)?.Children.Add(_tipPopup);
        }

        public static void Confirm() { }
        public static void Prompt() { }
        public static void Open() { }
        public static void Loading() { }
    }
}
