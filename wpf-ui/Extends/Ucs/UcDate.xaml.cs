using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using wpf_ui.Extends.Common;
using wpf_ui.Extends.DiyControls;

namespace wpf_ui.Extends.Ucs
{
    /// <summary>
    /// UcDate.xaml 的交互逻辑
    /// </summary>
    public partial class UcDate : UserControl
    {
        /// <summary>
        /// 当前选择时间
        /// </summary>
        public DateTime DateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcDate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void UcDate_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.FadeIn(animateCompleted: InitDays);
            this.Translation(new Thickness(Margin.Left, Margin.Top + 50, 0, 0), isReverse: true);
        }

        /// <summary>
        /// 加载年份列表
        /// </summary>
        private void InitYears()
        {
            GridDays.Visibility = GridMonths.Visibility = BtnPrevMonth.Visibility
                                = BtnNextMonth.Visibility = BtnMonth.Visibility = Visibility.Collapsed;
            GridYears.Visibility = Visibility.Visible;
            WpYears.Children.Clear();
            //加载年份
            int startYear = DateTime.AddYears(-7).Year;
            int stopYear = DateTime.AddYears(7).Year;
            BtnYear.Content = $"{startYear}年 - {stopYear}年";
            BtnYear.Width = 125;
            for (int i = startYear; i <= stopYear; i++)
            {
                WpYears.Children.Add(new DiyButton
                {
                    Style = FindResource("BtnCurMonth") as Style,
                    Content = $"{i}年",
                    Tag = i,
                    Width = 84,
                    Height = 42,
                    IsSelected = i == DateTime.Year
                });
            }
        }

        /// <summary>
        /// 加载月份列表
        /// </summary>
        private void InitMonths()
        {
            GridDays.Visibility = GridYears.Visibility = BtnPrevMonth.Visibility
                                = BtnNextMonth.Visibility = BtnMonth.Visibility = Visibility.Collapsed;
            GridMonths.Visibility = Visibility.Visible;
            WpMonths.Children.Clear();
            BtnYear.Width = 58;
            //加载年份
            for (int i = 1; i <= 12; i++)
            {
                WpMonths.Children.Add(new DiyButton
                {
                    Style = FindResource("BtnCurMonth") as Style,
                    Content = $"{i:00}月",
                    Tag = i,
                    Width = 84,
                    Height = 52,
                    IsSelected = i == DateTime.Month
                });
            }
        }

        /// <summary>
        /// 加载日期列表
        /// </summary>
        private void InitDays()
        {
            GridYears.Visibility = GridMonths.Visibility = Visibility.Collapsed;
            GridDays.Visibility = BtnPrevMonth.Visibility = BtnMonth.Visibility
                                = BtnNextMonth.Visibility = Visibility.Visible;
            WpDays.Children.Clear();
            BtnYear.Width = 58;
            //加载年月
            BtnYear.Content = DateTime.ToString("yyyy年");
            BtnMonth.Content = DateTime.ToString("MM月");
            //计算当月第一天是周几
            int startDay = Convert.ToDateTime(DateTime.ToString("yyyy-MM-01")).DayOfWeek.ToWeekIndex();
            //计算本月总天数
            int curMonthDays = DateTime.DaysInMonth(DateTime.Year, DateTime.Month);
            //计算上月总天数
            int lastMonthDays = DateTime.DaysInMonth(DateTime.Year, DateTime.AddMonths(-1).Month);
            //添加上月日期
            for (int i = lastMonthDays - startDay + 1; i <= lastMonthDays; i++)
            {
                WpDays.Children.Add(new DiyButton
                {
                    Style = FindResource("BtnOthMonth") as Style,
                    Content = i,
                    Tag = new DateTime(DateTime.Year, DateTime.AddMonths(-1).Month, i)
                });
            }
            //添加本月日期
            for (int i = 1; i <= curMonthDays; i++)
            {
                DiyButton diyButton = new DiyButton
                {
                    Style = FindResource("BtnCurMonth") as Style,
                    Content = i,
                    Tag = new DateTime(DateTime.Year, DateTime.Month, i)
                };
                WpDays.Children.Add(diyButton);
                //选中当前日期
                if (i != DateTime.Day) continue;
                diyButton.IsSelected = true;
                new Thread(() =>
                {
                    Thread.Sleep(10);
                    Dispatcher.Invoke(() =>
                    {
                        //触发点击事件
                        Point point = diyButton.TranslatePoint(new Point(), WpDays);
                        DaySelected.Translation(new Thickness(point.X, point.Y, 0, 0));
                    });
                }).Start();
            }
            //添加下月日期
            int nextMonthDays = 42 - WpDays.Children.Count;
            for (int i = 1; i <= nextMonthDays; i++)
            {
                WpDays.Children.Add(new DiyButton
                {
                    Style = FindResource("BtnOthMonth") as Style,
                    Content = i,
                    Tag = new DateTime(DateTime.Year, DateTime.AddMonths(1).Month, i)
                });
            }
        }

        /// <summary>
        /// 处理顶部日期操作
        /// </summary>
        private void BtnDealDay_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is DiyButton diyButton)) return;
            switch (diyButton.Tag.ToStringEx())
            {
                case "Y-":
                    if (GridYears.Visibility == Visibility.Visible)
                    {
                        DateTime = DateTime.AddYears(-15);
                        InitYears();
                    }
                    if (GridMonths.Visibility == Visibility.Visible)
                    {
                        DateTime = DateTime.AddYears(-1);
                        BtnYear.Content = DateTime.ToString("yyyy年");
                    }
                    if (GridDays.Visibility == Visibility.Visible)
                    {
                        DateTime = DateTime.AddYears(-1);
                        InitDays();
                    }
                    break;
                case "M-":
                    DateTime = DateTime.AddMonths(-1);
                    InitDays();
                    break;
                case "M+":
                    DateTime = DateTime.AddMonths(1);
                    InitDays();
                    break;
                case "Y+":
                    if (GridYears.Visibility == Visibility.Visible)
                    {
                        DateTime = DateTime.AddYears(15);
                        InitYears();
                    }
                    if (GridMonths.Visibility == Visibility.Visible)
                    {
                        DateTime = DateTime.AddYears(1);
                        BtnYear.Content = DateTime.ToString("yyyy年");
                    }
                    if (GridDays.Visibility == Visibility.Visible)
                    {
                        DateTime = DateTime.AddYears(1);
                        InitDays();
                    }
                    break;
                case "Y":
                    if (GridYears.Visibility == Visibility.Collapsed) InitYears();
                    break;
                case "M":
                    if (GridMonths.Visibility == Visibility.Collapsed) InitMonths();
                    break;
            }
        }

        /// <summary>
        /// 处理年份单击事件
        /// </summary>
        private void WpYears_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is DiyButton diyButton)) return;
            DateTime = Convert.ToDateTime(DateTime.ToString($"{diyButton.Tag}-MM-dd HH:mm:ss"));
            InitDays();
        }

        /// <summary>
        /// 处理月份单击事件
        /// </summary>
        private void WpMonths_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is DiyButton diyButton)) return;
            DateTime = Convert.ToDateTime(DateTime.ToString($"yyyy-{diyButton.Tag:00}-dd HH:mm:ss"));
            InitDays();
        }

        /// <summary>
        /// 处理日期单击事件
        /// </summary>
        private void WpDays_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is DiyButton diyButton)) return;
            //清除之前的选中
            foreach (UIElement child in WpDays.Children)
            {
                if (!(child is DiyButton dayButton)) continue;
                dayButton.IsSelected = false;
            }
            //选中点击日期
            Point point = diyButton.TranslatePoint(new Point(), WpDays);
            DaySelected.Translation(new Thickness(point.X, point.Y, 0, 0), animateCompleted: () =>
            {
                diyButton.IsSelected = true;
                DateTime dtNow = Convert.ToDateTime(diyButton.Tag);
                //如果选中日期不为当月则切换月份
                bool isInit = dtNow.ToString("yyyyMM").Equals(DateTime.ToString("yyyyMM"));
                DateTime = dtNow;
                if (!isInit) InitDays();
            });
        }

        /// <summary>
        /// 确定
        /// </summary>
        private void BtnSure_OnClick(object sender, RoutedEventArgs e)
        {
            Alter.Msg($"您选择的时间是：{DateTime:yyyy-MM-dd}", this);
        }

        /// <summary>
        /// 现在
        /// </summary>
        private void BtnNow_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime = DateTime.Now;
            InitDays();
        }

        /// <summary>
        /// 重置
        /// </summary>
        private void BtnReset_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
