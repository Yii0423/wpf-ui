using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using wpf_ui.Extends.Common;
using wpf_ui.Extends.DiyControls;

namespace wpf_ui.View.Alters
{
    /// <summary>
    /// AConfirm.xaml 的交互逻辑
    /// </summary>
    public partial class AConfirm : Window
    {
        /// <summary>
        /// 标题
        /// </summary>
        public new string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public new string Content { get; set; }

        /// <summary>
        /// 自定义按钮
        /// </summary>
        public List<DiyButton> Btns { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AConfirm()
        {
            InitializeComponent();
            //设定父窗体
            Owner = Client.MainWindow;
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void AConfirm_OnLoaded(object sender, RoutedEventArgs e)
        {
            //显示遮罩
            Client.MainShade.Visibility = Visibility.Visible;
            //动画
            GridMain.FadeIn();
            GridMain.ScaleIn();
            //加载内容
            TxtTitle.Text = Title;
            TxtContent.Text = Content;
            //加载自定义按钮
            if (Btns == null || !Btns.Any()) return;
            DpMain.Children.Clear();
            for (int i = Btns.Count - 1; i >= 0; i--)
            {
                DiyButton diyButton = Btns[i];
                diyButton.Margin = new Thickness(5, 0, 0, 0);
                DockPanel.SetDock(diyButton, Dock.Right);
                DpMain.Children.Add(diyButton);
            }
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
        private void BtnSure_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            BeforeClose();
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            BeforeClose();
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
        private void AConfirm_OnClosing(object sender, CancelEventArgs e)
        {
            //关闭遮罩
            Client.MainShade.Visibility = Visibility.Collapsed;
        }
    }
}
