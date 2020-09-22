using System.Collections;
using System.Text;
using System.Windows.Controls;

namespace wpf_ui.Extends.DiyControls
{
    /// <summary>
    /// 自定义多选下拉框
    /// </summary>
    public class DiyMultiCombobox : ComboBox
    {
        public IList SelectedItems => _listBox.SelectedItems;

        private ListBox _listBox;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _listBox = Template.FindName("PART_ListBox", this) as ListBox;
            if (_listBox != null) _listBox.SelectionChanged += _ListBox_SelectionChanged;
        }

        private void _ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Test item in SelectedItems) sb.Append(item.Name).Append(";");
            Text = sb.ToString().TrimEnd(';');
        }
    }
}
