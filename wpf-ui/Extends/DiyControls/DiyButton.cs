using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace wpf_ui.Extends.DiyControls
{
    /// <summary>
    /// 自定义按钮
    /// </summary>
    public class DiyButton : Button
    {
        #region 圆角

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(DiyButton), new PropertyMetadata(new CornerRadius(0)));

        #endregion

        #region 图标

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(DiyButton), new PropertyMetadata(null));

        #endregion

        #region 副标题

        public string SubTitle
        {
            get => (string)GetValue(SubTitleProperty);
            set => SetValue(SubTitleProperty, value);
        }

        public static readonly DependencyProperty SubTitleProperty =
            DependencyProperty.Register("SubTitle", typeof(string), typeof(DiyButton), new PropertyMetadata(""));

        #endregion

        #region 链接

        public string Url
        {
            get => (string)GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }

        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(DiyButton), new PropertyMetadata(""));

        #endregion

        #region 关闭按钮

        public bool IsClose
        {
            get => (bool)GetValue(IsCloseProperty);
            set => SetValue(IsCloseProperty, value);
        }

        public static readonly DependencyProperty IsCloseProperty =
            DependencyProperty.Register("IsClose", typeof(bool), typeof(DiyButton), new PropertyMetadata(false));

        #endregion
    }
}
