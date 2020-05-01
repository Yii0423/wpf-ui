using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace wpf_ui.Extends.DiyControls
{
    /// <summary>
    /// 自定义进度条
    /// </summary>
    public class DiyProgressbar : ProgressBar
    {
        public DiyProgressbar()
        {
            Loaded += delegate
            {
                new Thread(() =>
                {
                    double startValue = 0, endValue = 0;
                    Dispatcher.Invoke(() => { endValue = Value; });
                    while (startValue < 100)
                    {
                        startValue++;
                        Dispatcher.Invoke(() =>
                        {
                            Value = startValue * endValue / 100;
                            Percentage = $"{(int)Value}%";
                        });
                        Thread.Sleep(5);
                    }
                }).Start();
            };
        }

        #region 百分比数值

        public string Percentage
        {
            get { return GetValue(PercentageProperty).ToString(); }
            private set { SetValue(PercentageProperty, value); }
        }

        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(string), typeof(DiyProgressbar), new PropertyMetadata("0%"));

        #endregion

        #region 百分比数值显示方式

        public ShowMode ShowMode
        {
            get { return (ShowMode)GetValue(ShowModeProperty); }
            set { SetValue(ShowModeProperty, value); }
        }

        public static readonly DependencyProperty ShowModeProperty =
            DependencyProperty.Register("ShowMode", typeof(ShowMode), typeof(DiyProgressbar), new PropertyMetadata(ShowMode.No));

        #endregion
    }

    /// <summary>
    /// 百分比数值显示方式
    /// </summary>
    public enum ShowMode
    {
        /// <summary>
        /// 不显示百分比
        /// </summary>
        No,
        /// <summary>
        /// 显示在顶部
        /// </summary>
        Top,
        /// <summary>
        /// 显示在进度条内部
        /// </summary>
        In
    }
}
