using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using wpf_ui.Extends.Common;

namespace wpf_ui.View.Alters
{
    /// <summary>
    /// APrompt.xaml 的交互逻辑
    /// </summary>
    public partial class APrompt : Window
    {
        /// <summary>
        /// 内容
        /// </summary>
        public new string Content { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public APrompt()
        {
            InitializeComponent();
            //设定父窗体
            Owner = Application.Current.MainWindow;
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void APrompt_OnLoaded(object sender, RoutedEventArgs e)
        {
            //显示遮罩
            Client.MainShade.Visibility = Visibility.Visible;
            //动画
            GridMain.FadeIn();
            GridMain.ScaleIn();
            //加载内容
            TxtTitle.Text = Title;
            //获取焦点
            TxtContent.Focus();
        }

        /// <summary>
        /// 窗体关闭前执行动画
        /// </summary>
        private void BeforeClose()
        {
            GridMain.FadeOut();
            GridMain.ScaleOut(animateCompleted: Close);
        }

        /// <summary>
        /// 窗体拖动
        /// </summary>
        private void LabTitle_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// 确定
        /// </summary>
        private void BtnSure_OnClick(object sender, RoutedEventArgs e)
        {
            string content = TxtContent.Text.Trim();
            if (string.IsNullOrWhiteSpace(content))
            {
                TxtContent.Tip("必填项不可为空");
                TxtContent.Style = FindResource("TbDanger") as Style;
                TxtContent.Focus();
                return;
            }
            Content = content;
            BeforeClose();
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            BeforeClose();
        }

        /// <summary>
        /// 窗体关闭时关闭遮罩
        /// </summary>
        private void APrompt_OnClosing(object sender, CancelEventArgs e)
        {
            //关闭遮罩
            Client.MainShade.Visibility = Visibility.Collapsed;
        }
    }
}
