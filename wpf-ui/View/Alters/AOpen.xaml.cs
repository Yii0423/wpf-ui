using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using wpf_ui.Extends.Common;
using wpf_ui.Extends.DiyControls;
using wpf_ui.Model;

namespace wpf_ui.View.Alters
{
    /// <summary>
    /// AOpen.xaml 的交互逻辑
    /// </summary>
    public partial class AOpen : Window
    {
        /// <summary>
        /// 窗体属性对象
        /// </summary>
        public MOpen MOpen { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AOpen()
        {
            InitializeComponent();
            //设定父窗体
            Owner = Application.Current.MainWindow;
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void AOpen_OnLoaded(object sender, RoutedEventArgs e)
        {
            //显示遮罩
            Client.MainShade.Visibility = Visibility.Visible;
            //动画
            GridMain.FadeIn();
            GridMain.ScaleIn();
            //加载窗体属性
            Width = MOpen.Width;
            Height = MOpen.Height;
            TxtTitle.Text = MOpen.Title;
            if (MOpen.Url != null) FrmMain.Navigate(MOpen.Url);
            if (MOpen.FixedSize) ResizeMode = ResizeMode.NoResize;
            if (MOpen.StartMax) WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// 窗体拖动
        /// </summary>
        private void LabTitle_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.ClickCount)
            {
                case 1:
                    DragMove();
                    break;
                case 2:
                    WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                    break;
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
        /// 窗体按钮事件
        /// </summary>
        private void SpWindows_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is DiyButton diyButton)) return;
            switch (diyButton.Tag.ToStringEx())
            {
                case "Close":
                    BeforeClose();
                    break;
            }
        }

        /// <summary>
        /// 窗体关闭前执行动画
        /// </summary>
        private void BeforeClose()
        {
            GridMain.ScaleOut();
            GridMain.FadeOut(animateCompleted: Close);
        }

        /// <summary>
        /// 窗体关闭时关闭遮罩
        /// </summary>
        private void AOpen_OnClosing(object sender, CancelEventArgs e)
        {
            //关闭遮罩
            Client.MainShade.Visibility = Visibility.Collapsed;
        }
    }
}
