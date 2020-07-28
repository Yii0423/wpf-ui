using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace wpf_ui.Extends.DiyControls
{
    /// <summary>
    /// 自定义表格
    /// </summary>
    public class DiyTable : Grid
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DiyTable()
        {
            InitializeStyle();
            //绑定初始化事件
            Loaded += InitializeEvent;
        }

        #region 行数

        public int RowCount
        {
            get => (int)GetValue(RowCountProperty);
            set => SetValue(RowCountProperty, value);
        }

        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.Register("RowCount", typeof(int), typeof(DiyTable), new PropertyMetadata(1));

        #endregion

        #region 列数

        public int ColCount
        {
            get => (int)GetValue(ColCountProperty);
            set => SetValue(ColCountProperty, value);
        }

        public static readonly DependencyProperty ColCountProperty =
            DependencyProperty.Register("ColCount", typeof(int), typeof(DiyTable), new PropertyMetadata(1));

        #endregion

        #region 事件

        /// <summary>
        /// 初始化样式
        /// </summary>
        private void InitializeStyle()
        {

        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitializeEvent(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < RowCount; i++) RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            for (int i = 0; i < ColCount; i++)
            {
                double width = 1;
                if (Children.Count > i && Children[i] is Border border) width = border.MinWidth;
                ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width, GridUnitType.Star) });
            }
            int cur = 0;
            for (int i = 0; i < RowCount; i++)
            {
                for (int l = 0; l < ColCount; l++)
                {
                    if (Children.Count <= cur) break;
                    if (!(Children[cur] is Border border)) continue;
                    //给Border加边框
                    border.Padding = new Thickness(2.5);
                    border.BorderBrush = FindResource("BrushTableBoder") as Brush;
                    border.BorderThickness = new Thickness(1, 1, 0, 0);
                    SetRow(border, i);
                    SetColumn(border, l);
                    cur++;
                }
            }
        }

        #endregion
    }
}
