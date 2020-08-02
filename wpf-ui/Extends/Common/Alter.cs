using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace wpf_ui.Extends.Common
{
    /// <summary>
    /// 消息
    /// </summary>
    public class Alter
    {
        /// <summary>
        /// 仅显示一个会自动关闭的结果
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="time">自动关闭的时间(默认3秒钟)</param>
        public static void Msg(string content, double time = 3)
        {
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
                //Opacity = 0d
                RenderTransform = new ScaleTransform
                {
                    ScaleX = 0.7d,
                    ScaleY = 0.7d
                },
                RenderTransformOrigin = new Point(0.5, 0.5)
            };
            (Client.MainShade.Parent as Grid)?.Children.Add(label);
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
        }
        public void Tip() { }
        public void Confirm() { }
        public void Prompt() { }
        public void Open() { }
        public void Loading() { }
    }
}
