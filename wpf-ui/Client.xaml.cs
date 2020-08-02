using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using wpf_ui.Extends.Common;
using wpf_ui.Model;

namespace wpf_ui
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Client : Window
    {
        /// <summary>
        /// 主窗体
        /// </summary>
        public static Window MainWindow;

        /// <summary>
        /// 遮罩层
        /// </summary>
        public static Border MainShade;

        /// <summary>
        /// 窗体阴影宽度
        /// </summary>
        private const int CustomBorderThickness = 7;

        /// <summary>
        /// 尺寸可调节感应区的宽度
        /// </summary>
        private const int CornerWidth = 8;

        /// <summary>
        /// 调节尺寸时鼠标的坐标
        /// </summary>
        private Point _mousePoint;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Client()
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
            SetFrameWidth();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            //遮罩赋值
            MainShade = BorderShade;
            //设置Frame的Z-Index(避免遮挡菜单)
            if (FrmMain.Parent is ScrollViewer scrollViewer) Panel.SetZIndex(scrollViewer, -1);
            //侧边菜单绑定Frame
            MenuLeft.FrmMain = FrmMain;
            //顶部菜单数据源
            MenuTop.ItemSources = new List<MMenuItem>
            {
                new MMenuItem {Title = "Icon-Indent-Right", IsTop = true},
                new MMenuItem {Title = "Icon-Globe", IsTop = true, IsHasNew = true},
                new MMenuItem {Title = "Icon-Repeat", IsTop = true},
                new MMenuItem {Title = "Icon-Ellipsis-Vertical", IsTop = true, IsRight = true},
                new MMenuItem
                {
                    Title = "超级管理员", IsTop = true, IsRight = true, ChildItem = new List<MMenuItem>
                    {
                        new MMenuItem {Title = "基本资料"},
                        new MMenuItem {Title = "修改密码"},
                        new MMenuItem {Title = "退出"}
                    }
                },
                new MMenuItem {Title = "Icon-Tag", IsTop = true, IsRight = true},
                new MMenuItem {Title = "Icon-Dashboard", IsTop = true, IsRight = true},
                new MMenuItem {Title = "Icon-Bell", IsTop = true, IsRight = true, IsHasNew = true}
            };
            //侧边菜单数据源
            MenuLeft.ItemSources = new List<MMenuItem>
            {
                new MMenuItem
                {
                    Title = "Icon-Home", IsTop = true, Url = "Index", ChildItem = new List<MMenuItem>
                    {
                        new MMenuItem {Title = "移动模块"},
                        new MMenuItem
                        {
                            Title = "后台模块", ChildItem = new List<MMenuItem>
                            {
                                new MMenuItem {Title = "后台顶部"},
                                new MMenuItem {Title = "后台左侧"},
                                new MMenuItem {Title = "后台下方"}
                            }
                        }
                    }
                },
                new MMenuItem {Title = "Icon-Music", IsTop = true},
                new MMenuItem
                {
                    Title = "Icon-Calendar", IsTop = true, ChildItem = new List<MMenuItem>
                    {
                        new MMenuItem {Title = "移动模块"},
                        new MMenuItem
                        {
                            Title = "后台模块", ChildItem = new List<MMenuItem>
                            {
                                new MMenuItem {Title = "后台顶部"},
                                new MMenuItem {Title = "后台左侧"},
                                new MMenuItem
                                {
                                    Title = "后台下方", ChildItem = new List<MMenuItem>
                                    {
                                        new MMenuItem {Title = "后台顶部"},
                                        new MMenuItem {Title = "后台左侧"},
                                        new MMenuItem {Title = "后台下方", Url = "Index"}
                                    }
                                }
                            }
                        },
                        new MMenuItem {Title = "电商平台"}
                    }
                },
                new MMenuItem {Title = "Icon-Rss", IsTop = true},
                new MMenuItem
                {
                    Title = "Icon-Plane", IsTop = true, ChildItem = new List<MMenuItem>
                    {
                        new MMenuItem {Title = "后台顶部"},
                        new MMenuItem {Title = "后台左侧"},
                        new MMenuItem {Title = "后台下方"}
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
            SizeChanged += MainWindow_SizeChanged;
        }

        /// <summary>
        /// 窗体尺寸改变后事件
        /// </summary>
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetFrameWidth();
        }

        /// <summary>
        /// 窗体尺寸改变前事件
        /// </summary>
        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            BorderThickness = WindowState == WindowState.Maximized ? new Thickness(0) : new Thickness(CustomBorderThickness);
        }

        /// <summary>
        /// 窗体鼠标左键事件
        /// </summary>
        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
            else
            {
                if (!(e.OriginalSource is Grid) && !(e.OriginalSource is Border) && !(e.OriginalSource is Window)) return;
                WindowInteropHelper wih = new WindowInteropHelper(this);
                Win32.SendMessage(wih.Handle, Win32.WM_NCLBUTTONDOWN, (int)Win32.HitTest.HTCAPTION, 0);
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

        /// <summary>
        /// 设置Frame最大宽度
        /// </summary>
        private void SetFrameWidth()
        {
            if (!(FrmMain.Parent is ScrollViewer scrollViewer)) return;
            FrmMain.Width = scrollViewer.ActualWidth;
        }

        #region 窗体状态切换/窗体拖动/窗体尺寸拖动变化

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            if (source == null) throw new Exception("Cannot get HwndSource instance.");
            source.AddHook(WndProc);
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
                    return WmNcHitTest(lParam, ref handled);
            }
            return IntPtr.Zero;
        }

        private IntPtr WmNcHitTest(IntPtr lParam, ref bool handled)
        {
            // Update cursor point
            // The low-order word specifies the x-coordinate of the cursor.
            // #define GET_X_LPARAM(lp) ((int)(short)LOWORD(lp))
            _mousePoint.X = (int)(short)(lParam.ToInt32() & 0xFFFF);
            // The high-order word specifies the y-coordinate of the cursor.
            // #define GET_Y_LPARAM(lp) ((int)(short)HIWORD(lp))
            _mousePoint.Y = (int)(short)(lParam.ToInt32() >> 16);
            // Do hit test
            handled = true;
            if (Math.Abs(_mousePoint.Y - Top) <= CornerWidth
                && Math.Abs(_mousePoint.X - Left) <= CornerWidth)
            { // Top-Left
                return new IntPtr((int)Win32.HitTest.HTTOPLEFT);
            }

            if (Math.Abs(ActualHeight + Top - _mousePoint.Y) <= CornerWidth
                && Math.Abs(_mousePoint.X - Left) <= CornerWidth)
            { // Bottom-Left
                return new IntPtr((int)Win32.HitTest.HTBOTTOMLEFT);
            }
            if (Math.Abs(_mousePoint.Y - Top) <= CornerWidth
                && Math.Abs(ActualWidth + Left - _mousePoint.X) <= CornerWidth)
            { // Top-Right
                return new IntPtr((int)Win32.HitTest.HTTOPRIGHT);
            }
            if (Math.Abs(ActualWidth + Left - _mousePoint.X) <= CornerWidth
                && Math.Abs(ActualHeight + Top - _mousePoint.Y) <= CornerWidth)
            { // Bottom-Right
                return new IntPtr((int)Win32.HitTest.HTBOTTOMRIGHT);
            }
            if (Math.Abs(_mousePoint.X - Left) <= CustomBorderThickness)
            { // Left
                return new IntPtr((int)Win32.HitTest.HTLEFT);
            }
            if (Math.Abs(ActualWidth + Left - _mousePoint.X) <= CustomBorderThickness)
            { // Right
                return new IntPtr((int)Win32.HitTest.HTRIGHT);
            }
            if (Math.Abs(_mousePoint.Y - Top) <= CustomBorderThickness)
            { // Top
                return new IntPtr((int)Win32.HitTest.HTTOP);
            }
            if (Math.Abs(ActualHeight + Top - _mousePoint.Y) <= CustomBorderThickness)
            { // Bottom
                return new IntPtr((int)Win32.HitTest.HTBOTTOM);
            }
            handled = false;
            return IntPtr.Zero;
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
