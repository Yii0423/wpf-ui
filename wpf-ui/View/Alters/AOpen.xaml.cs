using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using wpf_ui.Extends.Common;
using wpf_ui.Extends.DiyControls;

namespace wpf_ui.View.Alters
{
    /// <summary>
    /// AOpen.xaml 的交互逻辑
    /// </summary>
    public partial class AOpen : Window
    {
        /// <summary>
        /// 页面链接
        /// </summary>
        public Uri Url { get; set; }

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
            Owner = Client.MainWindow;
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
            //加载内容
            TxtTitle.Text = Title;
            if (Url != null) FrmMain.Navigate(Url);
        }

        /// <summary>
        /// 窗体拖动
        /// </summary>
        private void LabTitle_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
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
