using System.Collections.Generic;
using System.Windows;

namespace wpf_ui
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class ControlsDemo : Window
    {
        public ControlsDemo()
        {
            InitializeComponent();
        }

        private void Window1_OnLoaded(object sender, RoutedEventArgs e)
        {
            MultiCombobox1.DisplayMemberPath = "Name";
            MultiCombobox1.SelectedValuePath = "Id";
            MultiCombobox1.ItemsSource = new List<Test>
            {
                new Test {Id = "1", Name = "zhangsan"},
                new Test {Id = "2", Name = "lisi"},
                new Test {Id = "3", Name = "wangwu"}
            };
        }
    }

    public class Test
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
