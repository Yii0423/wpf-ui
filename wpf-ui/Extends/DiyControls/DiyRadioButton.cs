using System.Windows;
using System.Windows.Controls;
using wpf_ui.Extends.Common;

namespace wpf_ui.Extends.DiyControls
{
    /// <summary>
    /// 自定义单选框
    /// </summary>
    public class DiyRadioButton : RadioButton
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DiyRadioButton()
        {
            //加载样式
            InitializeStyle();
            //绑定关闭事件
            Loaded += InitializeEvent;
        }

        #region 是否显示关闭按钮

        public bool ShowCloseButton
        {
            get => (bool)GetValue(ShowCloseButtonProperty);
            set => SetValue(ShowCloseButtonProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowCloseButtonProperty =
            DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(DiyRadioButton), new PropertyMetadata(true));

        #endregion

        #region 事件

        /// <summary>
        /// 加载样式
        /// </summary>
        private void InitializeStyle()
        {
            Style = FindResource("RdbTab") as Style;
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        private void InitializeEvent(object sender, RoutedEventArgs e)
        {
            if (!(Template.FindName("BtnClose", this) is Button btnClose)) return;
            btnClose.Click += delegate
            {
                btnClose.FindParent<StackPanel>("SpMain")?.Children.Remove(this);
            };
        }

        #endregion
    }
}
