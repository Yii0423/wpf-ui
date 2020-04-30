using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace wpf_ui.Extends.DiyControls
{
    public class DiyTextbox : TextBox
    {
        #region 水印

        public string WaterMark
        {
            get => (string)GetValue(WaterMarkProperty);
            set => SetValue(WaterMarkProperty, value);
        }

        public static readonly DependencyProperty WaterMarkProperty =
            DependencyProperty.Register("WaterMark", typeof(string), typeof(DiyTextbox), new PropertyMetadata(""));

        #endregion

        #region 圆角

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(DiyTextbox), new PropertyMetadata(new CornerRadius(0)));

        #endregion

        #region 图标

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(DiyTextbox), new PropertyMetadata(null));

        #endregion
    }
}
