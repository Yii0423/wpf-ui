using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using wpf_ui.Extends.Common;

namespace wpf_ui.Extends.Ucs
{
    /// <summary>
    /// UcMenuTop.xaml 的交互逻辑
    /// </summary>
    public partial class UcMenuLeft : UserControl
    {
        /// <summary>
        /// 顶部菜单集合
        /// </summary>
        public List<MenuTopItem> ItemSources { get; set; } = new List<MenuTopItem>();

        /// <summary>
        /// 指定Frame窗体
        /// </summary>
        public Frame FrmMain { get; set; }

        /// <summary>
        /// Logo
        /// </summary>
        public ImageSource Logo { get; set; }

        /// <summary>
        /// 圆角
        /// </summary>
        public CornerRadius CornerRadius { get; set; } = new CornerRadius(0);

        /// <summary>
        /// 标识淡入动画是否正在执行中(避免反复累加导致子菜单位置下移)
        /// </summary>
        private bool _isDo;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcMenuLeft()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //加载Logo
            if (Logo != null) ImgLogo.ImageSource = Logo;

            //设置圆角
            BdMain.CornerRadius = CornerRadius;

            LoadParentMenu();
        }

        /// <summary>
        /// 加载父级菜单
        /// </summary>
        private void LoadParentMenu()
        {
            //绑定顶部菜单鼠标事件
            SpMain.MouseEnter += SpMain_MouseEnter;
            SpMain.MouseLeave += SpMain_MouseLeave;
            //加载顶部菜单子项
            for (int i = 0; i < ItemSources.Count; i++)
            {
                MenuTopItem item = ItemSources[i];

                RadioButton rdb = new RadioButton();
                rdb.SetResourceReference(StyleProperty, "MiLeftParentPrimary");

                //绑定顶部菜单子项鼠标事件
                rdb.MouseEnter += Mtip_MouseEnter;
                rdb.MouseLeave += Mtip_MouseLeave;

                //子级菜单项不为空则添加子级菜单
                if (item.ChildItem.Count > 0)
                {
                    LoadChildMenu(item, rdb);
                    //添加标记用以显示下拉标志
                    rdb.Tag = "1";
                }
                else
                {
                    if (item.Title.Contains("Icon-"))
                    {
                        System.Windows.Shapes.Path path = new System.Windows.Shapes.Path
                        {
                            Width = 16,
                            Height = 16,
                            Data = FindResource(item.Title) as Geometry
                        };
                        path.SetResourceReference(StyleProperty, "Icon");
                        rdb.Content = path;
                    }
                    else rdb.Content = item.Title;
                }

                //若链接不为空则绑定导航事件
                if (!string.IsNullOrWhiteSpace(item.Url))
                    rdb.Click += delegate
                    {
                        FrmMain?.Navigate(new Uri($"/View/{item.Url}.xaml", UriKind.Relative));
                    };

                SpMain.Children.Add(rdb);

                if (i == 0)
                {
                    rdb.IsChecked = true;
                    MtiChecked.Height = rdb.ActualHeight;
                }
            }
        }

        /// <summary>
        /// 加载子级菜单
        /// </summary>
        /// <param name="items">父级菜单项</param>
        /// <param name="rdb">父级控件</param>
        private void LoadChildMenu(MenuTopItem item, RadioButton rdb)
        {
            Grid grid = new Grid();

            //添加标题
            if (item.Title.Contains("Icon-"))
            {
                System.Windows.Shapes.Path path = new System.Windows.Shapes.Path
                {
                    Width = 16,
                    Height = 16,
                    Data = FindResource(item.Title) as Geometry
                };
                path.SetResourceReference(StyleProperty, "Icon");
                grid.Children.Add(path);
            }
            else grid.Children.Add(new TextBlock
            {
                Text = item.Title,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            });

            #region  添加菜单

            Canvas canvas = new Canvas { Visibility = Visibility.Collapsed };
            if (item.IsTop) canvas.HorizontalAlignment = HorizontalAlignment.Left;

            StackPanel panel = new StackPanel();
            panel.SetResourceReference(BackgroundProperty, "BrushPrimary");

            Border border = new Border
            {
                Width = 100,
                Padding = new Thickness(5, 0, 0, 0),
                Background = Brushes.Transparent,
                Child = panel
            };

            if (item.IsTop) border.Effect = new DropShadowEffect
            {
                ShadowDepth = 0,
                BlurRadius = 5,
                Color = Colors.Black,
                Opacity = 0.8
            };

            for (int i = 0; i < item.ChildItem.Count; i++)
            {
                //菜单子项
                MenuTopItem itemc = item.ChildItem[i];

                RadioButton rdbc = new RadioButton();
                rdbc.SetResourceReference(StyleProperty, "MiLeftChildPrimary");

                //绑定顶部菜单子项鼠标事件
                rdbc.MouseEnter += Mtic_MouseEnter;
                rdbc.MouseLeave += Mtic_MouseLeave;

                //若链接不为空则绑定导航事件
                if (!string.IsNullOrWhiteSpace(itemc.Url))
                    rdbc.Click += (sender, e) =>
                    {
                        FrmMain?.Navigate(new Uri($"{itemc.Url}.xaml", UriKind.Relative));
                        e.Handled = true;
                    };

                //子级菜单项不为空则添加子级菜单
                if (item.ChildItem.Count > 0) LoadChildMenu(itemc, rdbc);
                else rdbc.Content = itemc.Title;

                panel.Children.Add(rdbc);
            }

            #endregion

            canvas.Children.Add(border);
            grid.Children.Add(canvas);

            rdb.Content = grid;
        }

        /// <summary>
        /// 父级菜单鼠标移入(显示选中条)
        /// </summary>
        private void SpMain_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation ani1 = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.2)));
            MtiChecked.BeginAnimation(OpacityProperty, ani1);
        }

        /// <summary>
        /// 父级菜单鼠标移出(隐藏选中条)
        /// </summary>
        private void SpMain_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation ani1 = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(1)));
            MtiChecked.BeginAnimation(OpacityProperty, ani1);
        }

        /// <summary>
        /// 父级菜单项鼠标移入(加载选中状态)
        /// </summary>
        private void Mtip_MouseEnter(object sender, MouseEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            Point point = radioButton.TranslatePoint(new Point(0, 0), SpMain);
            MtiChecked.Margin = new Thickness(MtiChecked.Margin.Left, point.Y, MtiChecked.Margin.Right, MtiChecked.Margin.Bottom);
            MtiChecked.Height = radioButton.ActualHeight;

            //位置动画
            ThicknessAnimation ani1 = new ThicknessAnimation(MtiChecked.Margin,
                new Thickness(MtiChecked.Margin.Left, point.Y, MtiChecked.Margin.Right, MtiChecked.Margin.Bottom), new Duration(TimeSpan.FromSeconds(0.2)));
            MtiChecked.BeginAnimation(MarginProperty, ani1);

            if (radioButton.FindChild<Canvas>() is Canvas childMenu)
            {
                //设置子菜单显示位置
                childMenu.Margin = new Thickness(61, -16, 0, 0);
                childMenu.Visibility = Visibility.Visible;

                //子菜单淡入动画
                DoubleAnimation ani3 = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.2)));
                childMenu.BeginAnimation(OpacityProperty, ani3);

                if (!_isDo)
                {
                    ThicknessAnimation _ani = new ThicknessAnimation(
                        new Thickness(childMenu.Margin.Left, childMenu.Margin.Top + 30, childMenu.Margin.Right, childMenu.Margin.Bottom),
                        childMenu.Margin, new Duration(TimeSpan.FromSeconds(0.2)));
                    _ani.Completed += delegate { _isDo = false; };
                    childMenu.BeginAnimation(MarginProperty, _ani);
                    //标识动画执行中
                    _isDo = true;
                }
            }
        }

        /// <summary>
        /// 父级菜单项鼠标移出(取消选中状态)
        /// </summary>
        private void Mtip_MouseLeave(object sender, MouseEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.FindChild<Canvas>() is Canvas childMenu)
            {
                childMenu.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 子级菜单鼠标移入(显示子级菜单)
        /// </summary>
        private void Mtic_MouseEnter(object sender, MouseEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.FindChild<Canvas>() is Canvas childMenu)
            {
                //设置子菜单显示位置
                childMenu.Margin = new Thickness(96, -9, 0, 0);
                childMenu.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 子级菜单鼠标移出(隐藏子级菜单)
        /// </summary>
        private void Mtic_MouseLeave(object sender, MouseEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.FindChild<Canvas>() is Canvas childMenu) childMenu.Visibility = Visibility.Collapsed;
        }
    }
}
