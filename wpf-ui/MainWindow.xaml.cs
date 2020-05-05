using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using wpf_ui.Extends.Common;
using wpf_ui.Extends.Ucs;

namespace wpf_ui
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 窗体阴影宽度
        /// </summary>
        private readonly int customBorderThickness = 7;

        /// <summary>
        /// Corner width used in HitTest
        /// </summary>
        private readonly int cornerWidth = 8;

        /// <summary>
        /// Mouse point used by HitTest
        /// </summary>
        private Point mousePoint = new Point();

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            InitEvent();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitData();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            //设置Frame的Z-Index(避免遮挡菜单)
            Panel.SetZIndex(FrmMain, -1);
            //侧边菜单绑定Frame
            MenuLeft.FrmMain = FrmMain;
            //顶部菜单数据源
            MenuTop.ItemSources = new List<MenuTopItem>
            {
                new MenuTopItem { Title = "Icon-Indent-Right", IsTop = true },
                new MenuTopItem { Title = "Icon-Globe", IsTop = true },
                new MenuTopItem { Title = "Icon-Repeat", IsTop = true },
                new MenuTopItem { Title = "Icon-Ellipsis-Vertical", IsTop = true, IsRight = true },
                new MenuTopItem { Title = "超级管理员", IsTop = true, IsRight = true, ChildItem = new List<MenuTopItem>{
                                new MenuTopItem { Title = "基本资料" },
                                new MenuTopItem { Title = "修改密码" },
                                new MenuTopItem { Title = "退出" }
                            } },
                new MenuTopItem { Title = "Icon-Tag", IsTop = true, IsRight = true },
                new MenuTopItem { Title = "Icon-Dashboard", IsTop = true, IsRight = true },
                new MenuTopItem { Title = "Icon-Bell", IsTop = true, IsRight = true }
            };
            //侧边菜单数据源
            MenuLeft.ItemSources = new List<MenuTopItem>
            {
                new MenuTopItem { Title = "Icon-Home", IsTop = true, Url = "Index", ChildItem = new List<MenuTopItem>{
                        new MenuTopItem { Title = "移动模块" },
                        new MenuTopItem { Title = "后台模块", ChildItem = new List<MenuTopItem>{
                                new MenuTopItem { Title = "后台顶部" },
                                new MenuTopItem { Title = "后台左侧" },
                                new MenuTopItem { Title = "后台下方" }
                            }
                         }
                    }
                },
                new MenuTopItem { Title = "Icon-Music", IsTop = true },
                new MenuTopItem { Title = "Icon-Calendar", IsTop = true, ChildItem = new List<MenuTopItem>{
                        new MenuTopItem { Title = "移动模块" },
                        new MenuTopItem { Title = "后台模块", ChildItem = new List<MenuTopItem>{
                                new MenuTopItem { Title = "后台顶部" },
                                new MenuTopItem { Title = "后台左侧" },
                                new MenuTopItem { Title = "后台下方", ChildItem = new List<MenuTopItem>{
                                new MenuTopItem { Title = "后台顶部" },
                                new MenuTopItem { Title = "后台左侧" },
                                new MenuTopItem { Title = "后台下方" }
                            }
                                }
                            }
                        },
                        new MenuTopItem { Title = "电商平台" }
                    }
                },
                new MenuTopItem { Title = "Icon-Rss", IsTop = true },
                new MenuTopItem { Title = "Icon-Plane", IsTop = true, ChildItem = new List<MenuTopItem>{
                                new MenuTopItem { Title = "后台顶部" },
                                new MenuTopItem { Title = "后台左侧" },
                                new MenuTopItem { Title = "后台下方" }
                            }
                }
            };
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            Loaded += MainWindow_Loaded;
            MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            SourceInitialized += MainWindow_SourceInitialized;
            StateChanged += MainWindow_StateChanged;
            FrmMain.Navigated += FrmMain_Navigated;
        }

        /// <summary>
        /// 窗体尺寸改变事件
        /// </summary>
        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized) BorderThickness = new Thickness(0);
            else BorderThickness = new Thickness(customBorderThickness);
        }

        /// <summary>
        /// 窗体鼠标左键事件
        /// </summary>
        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
                else WindowState = WindowState.Maximized;
            }
            else
            {
                if (e.OriginalSource is Grid || e.OriginalSource is Border || e.OriginalSource is Window)
                {
                    WindowInteropHelper wih = new WindowInteropHelper(this);
                    Win32.SendMessage(wih.Handle, Win32.WM_NCLBUTTONDOWN, (int)Win32.HitTest.HTCAPTION, 0);
                    return;
                }
            }
        }

        /// <summary>
        /// Frame导航结束清除历史记录以防后退
        /// </summary>
        private void FrmMain_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (!FrmMain.CanGoBack) return;
            FrmMain.RemoveBackEntry();
        }

        #region 窗体状态切换/窗体拖动/窗体尺寸拖动变化

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            if (source == null) throw new Exception("Cannot get HwndSource instance.");
            source.AddHook(new HwndSourceHook(this.WndProc));
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WM_GETMINMAXINFO: // WM_GETMINMAXINFO message
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
                case Win32.WM_NCHITTEST: // WM_NCHITTEST message
                    return WmNCHitTest(lParam, ref handled);
            }
            return IntPtr.Zero;
        }

        private IntPtr WmNCHitTest(IntPtr lParam, ref bool handled)
        {
            // Update cursor point
            // The low-order word specifies the x-coordinate of the cursor.
            // #define GET_X_LPARAM(lp) ((int)(short)LOWORD(lp))
            this.mousePoint.X = (int)(short)(lParam.ToInt32() & 0xFFFF);
            // The high-order word specifies the y-coordinate of the cursor.
            // #define GET_Y_LPARAM(lp) ((int)(short)HIWORD(lp))
            this.mousePoint.Y = (int)(short)(lParam.ToInt32() >> 16);
            // Do hit test
            handled = true;
            if (Math.Abs(this.mousePoint.Y - this.Top) <= this.cornerWidth
                && Math.Abs(this.mousePoint.X - this.Left) <= this.cornerWidth)
            { // Top-Left
                return new IntPtr((int)Win32.HitTest.HTTOPLEFT);
            }
            else if (Math.Abs(this.ActualHeight + this.Top - this.mousePoint.Y) <= this.cornerWidth
                && Math.Abs(this.mousePoint.X - this.Left) <= this.cornerWidth)
            { // Bottom-Left
                return new IntPtr((int)Win32.HitTest.HTBOTTOMLEFT);
            }
            else if (Math.Abs(this.mousePoint.Y - this.Top) <= this.cornerWidth
                && Math.Abs(this.ActualWidth + this.Left - this.mousePoint.X) <= this.cornerWidth)
            { // Top-Right
                return new IntPtr((int)Win32.HitTest.HTTOPRIGHT);
            }
            else if (Math.Abs(this.ActualWidth + this.Left - this.mousePoint.X) <= this.cornerWidth
                && Math.Abs(this.ActualHeight + this.Top - this.mousePoint.Y) <= this.cornerWidth)
            { // Bottom-Right
                return new IntPtr((int)Win32.HitTest.HTBOTTOMRIGHT);
            }
            else if (Math.Abs(this.mousePoint.X - this.Left) <= this.customBorderThickness)
            { // Left
                return new IntPtr((int)Win32.HitTest.HTLEFT);
            }
            else if (Math.Abs(this.ActualWidth + this.Left - this.mousePoint.X) <= this.customBorderThickness)
            { // Right
                return new IntPtr((int)Win32.HitTest.HTRIGHT);
            }
            else if (Math.Abs(this.mousePoint.Y - this.Top) <= this.customBorderThickness)
            { // Top
                return new IntPtr((int)Win32.HitTest.HTTOP);
            }
            else if (Math.Abs(this.ActualHeight + this.Top - this.mousePoint.Y) <= this.customBorderThickness)
            { // Bottom
                return new IntPtr((int)Win32.HitTest.HTBOTTOM);
            }
            else
            {
                handled = false;
                return IntPtr.Zero;
            }
        }

        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            // MINMAXINFO structure
            Win32.MINMAXINFO mmi = (Win32.MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(Win32.MINMAXINFO));

            // Get handle for nearest monitor to this window
            WindowInteropHelper wih = new WindowInteropHelper(this);
            IntPtr hMonitor = Win32.MonitorFromWindow(wih.Handle, Win32.MONITOR_DEFAULTTONEAREST);

            // Get monitor info
            Win32.MONITORINFOEX monitorInfo = new Win32.MONITORINFOEX();
            monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
            Win32.GetMonitorInfo(new HandleRef(this, hMonitor), monitorInfo);

            // Get HwndSource
            HwndSource source = HwndSource.FromHwnd(wih.Handle);
            if (source == null) throw new Exception("Cannot get HwndSource instance.");
            if (source.CompositionTarget == null) throw new Exception("Cannot get HwndTarget instance.");

            // Get transformation matrix
            Matrix matrix = source.CompositionTarget.TransformFromDevice;

            // Convert working area
            Win32.RECT workingArea = monitorInfo.rcWork;
            Point dpiIndependentSize = matrix.Transform(new Point(
                                            workingArea.Right - workingArea.Left,
                                            workingArea.Bottom - workingArea.Top
                                        ));

            // Convert minimum size
            Point dpiIndenpendentTrackingSize = matrix.Transform(new Point(MinWidth, MinHeight));

            // Set the maximized size of the window
            mmi.ptMaxSize.x = (int)dpiIndependentSize.X;
            mmi.ptMaxSize.y = (int)dpiIndependentSize.Y;

            // Set the position of the maximized window
            mmi.ptMaxPosition.x = 0;
            mmi.ptMaxPosition.y = 0;

            // Set the minimum tracking size
            mmi.ptMinTrackSize.x = (int)dpiIndenpendentTrackingSize.X;
            mmi.ptMinTrackSize.y = (int)dpiIndenpendentTrackingSize.Y;

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        #endregion
    }
}
