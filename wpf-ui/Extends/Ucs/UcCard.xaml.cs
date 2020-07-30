using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using wpf_ui.Extends.DiyControls;
using wpf_ui.Model;

namespace wpf_ui.Extends.Ucs
{
    /// <summary>
    /// UcCard.xaml 的交互逻辑
    /// </summary>
    public partial class UcCard : UserControl
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 菜单项模板
        /// </summary>
        public string ControlStyle { get; set; }

        /// <summary>
        /// 菜单项
        /// </summary>
        public List<MMenuItem> ItemSources { get; set; } = new List<MMenuItem>();

        /// <summary>
        /// 每页显示菜单子项数
        /// </summary>
        public int MenuItemCount { get; set; }

        /// <summary>
        /// 自动切换选中项
        /// </summary>
        public bool AutoChange { get; set; }

        /// <summary>
        /// 上次选中的卡片页索引
        /// </summary>
        private int _oldCheckedIndex = -1;

        /// <summary>
        /// 自动切换用的定时器
        /// </summary>
        private DispatcherTimer _timer;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcCard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void UcCard_OnLoaded(object sender, RoutedEventArgs e)
        {
            LabTitle.Content = Title;
            UcCard_OnSizeChanged(null, null);
        }

        /// <summary>
        /// 尺寸变化时重新排列
        /// </summary>
        private void UcCard_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _timer?.Stop();
            DpMain.Children.Clear();
            SpMain.Children.Clear();
            if (string.IsNullOrWhiteSpace(ControlStyle)) return;
            //加载菜单子项
            WrapPanel curWrapPanel = null;
            for (var i = 0; i < ItemSources.Count; i++)
            {
                if (i % MenuItemCount == 0)
                {
                    //一页显示不下则新增菜单容器
                    curWrapPanel = new WrapPanel
                    {
                        Name = $"WpMain{i / MenuItemCount + 1}",
                        Height = ActualHeight - 40,
                        Width = ActualWidth
                    };
                    SpMain.Children.Add(curWrapPanel);
                }
                MMenuItem item = ItemSources[i];
                DiyButton curDiyButton = new DiyButton
                {
                    Content = item.Title,
                    SubTitle = item.SubTitle
                };
                if (!string.IsNullOrWhiteSpace(item.Icon) && item.Icon.StartsWith("Icon-"))
                {
                    curDiyButton.Icon = FindResource(item.Icon) as Geometry;
                }
                curDiyButton.SetResourceReference(StyleProperty, string.IsNullOrWhiteSpace(item.Style) ? ControlStyle : item.Style);
                if (item.IsTop) curDiyButton.Foreground = FindResource("BrushDanger") as Brush;
                curDiyButton.Width = (ActualWidth - (MenuItemCount / 2 + 1) * 10) / (MenuItemCount / 2 == 0 ? 1 : MenuItemCount / 2);
                curWrapPanel?.Children.Add(curDiyButton);
            }
            //右上角缩略切换按钮
            if (ItemSources.Count <= MenuItemCount) return;
            int allPage = ItemSources.Count / MenuItemCount;
            int remainder = ItemSources.Count % MenuItemCount;
            for (int i = remainder == 0 ? 1 : 0; i < allPage + 1; i++)
            {
                RadioButton radioButton = new RadioButton
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = i == allPage ? new Thickness(0) : new Thickness(5, 0, 0, 0),
                    IsChecked = i == allPage
                };
                radioButton.SetResourceReference(StyleProperty, "RbCard");
                DockPanel.SetDock(radioButton, Dock.Right);
                int i1 = i;
                bool isDo = false;//避免动画重复执行
                radioButton.Checked += delegate
                {
                    if (isDo) return;
                    ThicknessAnimation da = new ThicknessAnimation
                    {
                        From = SpMain.Margin,
                        To = new Thickness(-ActualWidth * (allPage - i1), 0, 0, 0),
                        Duration = TimeSpan.FromSeconds(0.2)
                    };
                    da.Completed += delegate
                    {
                        _oldCheckedIndex = i1;
                        isDo = false;
                    };
                    SpMain.BeginAnimation(MarginProperty, da);
                    isDo = true;
                };
                //自动切换开启时取消绑定鼠标移入事件
                if (!AutoChange) radioButton.MouseEnter += delegate { radioButton.IsChecked = true; };
                DpMain.Children.Add(radioButton);
            }
            //是否开启自动切换
            if (AutoChange)
            {
                if (_timer == null) SetTimer();
                else _timer.Start();
            }
            //尺寸变化后重新定位当前卡片页
            if (DpMain.Children.Count == 0 || _oldCheckedIndex == -1) return;
            ThicknessAnimation da2 = new ThicknessAnimation
            {
                From = SpMain.Margin,
                To = new Thickness(-ActualWidth * (allPage - _oldCheckedIndex), 0, 0, 0),
                Duration = TimeSpan.FromSeconds(0.001)
            };
            da2.Completed += delegate
            {
                //选中卡片右上角当前页标志
                if (!(DpMain.Children[remainder == 0 ? _oldCheckedIndex - 1 : _oldCheckedIndex] is RadioButton radioButton)) return;
                radioButton.IsChecked = true;
            };
            SpMain.BeginAnimation(MarginProperty, da2);
        }

        /// <summary>
        /// 设置定时器
        /// </summary>
        private void SetTimer()
        {
            if (DpMain.Children.Count <= 1) return;
            int index = DpMain.Children.Count - 2;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            _timer.Tick += delegate
            {
                if (index < 0) index = DpMain.Children.Count - 1;
                if (!(DpMain.Children[index] is RadioButton radioButton)) return;
                radioButton.IsChecked = true;
                index--;
            };
            _timer.Start();
        }
    }
}
