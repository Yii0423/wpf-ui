using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace wpf_ui.Extends.Ucs
{
    /// <summary>
    /// Forecast24H.xaml 的交互逻辑
    /// </summary>
    public partial class UcForecast24H : UserControl
    {
        public UcForecast24H()
        {
            InitializeComponent();
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox txtBox) || !txtBox.Text.Trim().Equals("昨天")) return;
            if (!(txtBox.Parent is Grid grid)) return;
            foreach (UIElement child in grid.Children)
            {
                if (!(child is Label label)) continue;
                label.Foreground = FindResource("BrushFont") as Brush;
            }
        }
    }
}
