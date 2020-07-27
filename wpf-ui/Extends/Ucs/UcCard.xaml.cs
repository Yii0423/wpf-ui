using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
                curDiyButton.SetResourceReference(StyleProperty, ControlStyle);
                if (item.IsTop) curDiyButton.Foreground = FindResource("BrushDanger") as Brush;
                curDiyButton.Width = (ActualWidth - (Convert.ToInt32(MenuItemCount / 2) + 1) * 10) / Convert.ToInt32(MenuItemCount / 2);
                curWrapPanel?.Children.Add(curDiyButton);
            }
            //右上角缩略切换按钮
            if (ItemSources.Count <= MenuItemCount) return;
            int allPage = ItemSources.Count / MenuItemCount;
            for (int i = 0; i < allPage + 1; i++)
            {
                DiyRadioButton diyRadioButton = new DiyRadioButton
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = i == allPage ? new Thickness(0) : new Thickness(5, 0, 0, 0),
                    IsChecked = i == allPage
                };
                diyRadioButton.SetResourceReference(StyleProperty, "RbCard");
                DockPanel.SetDock(diyRadioButton, Dock.Right);
                int i1 = i;
                bool isDo = false;//避免动画重复执行
                diyRadioButton.MouseEnter += delegate
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
                        diyRadioButton.IsChecked = true;
                        isDo = false;
                    };
                    SpMain.BeginAnimation(MarginProperty, da);
                    isDo = true;
                };
                DpMain.Children.Add(diyRadioButton);
            }
            SpMain.Margin = new Thickness(0);
        }
    }
}
