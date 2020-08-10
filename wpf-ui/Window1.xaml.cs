using System.Windows;
using wpf_ui.Extends.Common;

namespace wpf_ui
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public static Window MainWindow;

        public Window1()
        {
            InitializeComponent();
        }

        private void Window1_OnLoaded(object sender, RoutedEventArgs e)
        {
            MainWindow = this;
            TxtDate.DateTime();
        }
    }
}
