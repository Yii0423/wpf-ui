using System.Windows;
using System.Windows.Controls;

namespace wpf_ui.Extends.Ucs
{
    /// <summary>
    /// UcTab.xaml 的交互逻辑
    /// </summary>
    public partial class UcTab : UserControl
    {
        public UcTab()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(this, -1);
        }
    }
}
