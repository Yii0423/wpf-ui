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
