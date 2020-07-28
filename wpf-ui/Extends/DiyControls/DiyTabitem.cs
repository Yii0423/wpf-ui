using System.Windows;
using System.Windows.Controls;
using wpf_ui.Extends.Common;

namespace wpf_ui.Extends.DiyControls
{
    /// <summary>
    /// 自定义选项卡
    /// </summary>
    public class DiyTabitem : TabItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DiyTabitem()
        {
            //加载样式
            InitializeStyle();
            //绑定关闭事件
            Loaded += InitializeEvent;
        }

        #region 是否显示关闭按钮

        public bool ShowCloseBtn
        {
            get => (bool)GetValue(ShowCloseBtnProperty);
            set => SetValue(ShowCloseBtnProperty, value);
        }

        public static readonly DependencyProperty ShowCloseBtnProperty =
            DependencyProperty.Register("ShowCloseBtn", typeof(bool), typeof(DiyTabitem), new PropertyMetadata(true));

        #endregion

        #region 事件

        /// <summary>
        /// 加载样式
        /// </summary>
        private void InitializeStyle()
        {
            Style = FindResource("TiPrimary") as Style;
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        private void InitializeEvent(object sender, RoutedEventArgs e)
        {
            if (!(Template.FindName("BtnClose", this) is Button btnClose)) return;
            btnClose.Click += delegate
            {
                btnClose.FindParent<TabControl>()?.Items.Remove(this);
            };
        }

        #endregion
    }
}
