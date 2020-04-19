using System.Windows;
using System.Windows.Controls;

namespace wpf_ui.Extends.DiyControls
{
    /// <summary>
    /// 自定义按钮
    /// </summary>
    public class DiyButton : Button
    {
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(DiyButton), new PropertyMetadata(new CornerRadius(0)));

    }
}
