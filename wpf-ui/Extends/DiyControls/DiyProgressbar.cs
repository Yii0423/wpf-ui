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
                InitWeight();
                LoadAnimate();
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

        #region 圆形进度条的进度值

        public double CircularPercentage
        {
            get => (double)GetValue(CircularPercentageProperty);
            private set => SetValue(CircularPercentageProperty, value);
        }

        public static readonly DependencyProperty CircularPercentageProperty =
            DependencyProperty.Register("CircularPercentage", typeof(double), typeof(DiyProgressbar), new PropertyMetadata(0d));

        #endregion

        #region 圆形进度条的粗度(跟宽度相等时则为扇形)

        public double CircularWeight
        {
            get => (double)GetValue(CircularWeightProperty);
            set => SetValue(CircularWeightProperty, value);
        }

        public static readonly DependencyProperty CircularWeightProperty =
            DependencyProperty.Register("CircularWeight", typeof(double), typeof(DiyProgressbar), new PropertyMetadata(0d));

        #endregion

        #region 根据圆形进度条的粗度同步的Arc粗度

        public double ArcWeight
        {
            get => (double)GetValue(ArcWeightProperty);
            private set => SetValue(ArcWeightProperty, value);
        }

        public static readonly DependencyProperty ArcWeightProperty =
            DependencyProperty.Register("ArcWeight", typeof(double), typeof(DiyProgressbar), new PropertyMetadata(0d));

        #endregion

        #region 是否为扇形进度条(跟宽度相等时则为True)

        public bool IsFanShaped
        {
            get => (bool)GetValue(IsFanShapedProperty);
            set => SetValue(IsFanShapedProperty, value);
        }

        public static readonly DependencyProperty IsFanShapedProperty =
            DependencyProperty.Register("IsFanShaped", typeof(bool), typeof(DiyProgressbar), new PropertyMetadata(false));

        #endregion

        #region 百分比数值显示方式

        public ShowMode ShowMode
        {
            get => (ShowMode)GetValue(ShowModeProperty);
            set => SetValue(ShowModeProperty, value);
        }

        public static readonly DependencyProperty ShowModeProperty =
            DependencyProperty.Register("ShowMode", typeof(ShowMode), typeof(DiyProgressbar), new PropertyMetadata(ShowMode.No));

        #endregion

        #region 事件

        /// <summary>
        /// 根据圆形进度条的粗度同步Arc粗度
        /// </summary>
        private void InitWeight()
        {
            if (CircularWeight > ActualWidth) CircularWeight = ActualWidth;
            IsFanShaped = CircularWeight.Equals(ActualWidth);
            ArcWeight = CircularWeight / ActualWidth * 2;
        }

        /// <summary>
        /// 加载动画
        /// </summary>
        private void LoadAnimate()
        {
            new Thread(() =>
            {
                double startValue = 0, endValue = 0;
                Dispatcher.Invoke(() => { endValue = Value; });
                while (startValue < 100)
                {
                    startValue++;
                    var value = startValue;
                    Dispatcher.Invoke(() =>
                    {
                        Value = value * endValue / 100;
                        Percentage = $"{(int)Value}%";
                        CircularPercentage = Value * 3.6;
                    });
                    Thread.Sleep(5);
                }
            }).Start();
        }

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
