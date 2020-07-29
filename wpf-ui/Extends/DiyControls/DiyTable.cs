using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace wpf_ui.Extends.DiyControls
{
    /// <summary>
    /// 自定义表格
    /// </summary>
    public class Table : Grid
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Table()
        {
            //绑定初始化事件
            Loaded += InitializeEvent;
        }

        #region 数据源(目前仅支持DataTable)

        public DataTable DataSource
        {
            get => (DataTable)GetValue(DataSourceProperty);
            set => SetValue(DataSourceProperty, value);
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(DataTable), typeof(Table), new PropertyMetadata(null));

        #endregion

        #region 是否显示列头

        public bool ShowHeader
        {
            get => (bool)GetValue(ShowHeaderProperty);
            set => SetValue(ShowHeaderProperty, value);
        }

        public static readonly DependencyProperty ShowHeaderProperty =
            DependencyProperty.Register("ShowHeader", typeof(bool), typeof(Table), new PropertyMetadata(false));

        #endregion

        #region 行数

        public int RowCount
        {
            get => (int)GetValue(RowCountProperty);
            private set => SetValue(RowCountProperty, value);
        }

        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.Register("RowCount", typeof(int), typeof(Table), new PropertyMetadata(0));

        #endregion

        #region 列数

        public int ColCount
        {
            get => (int)GetValue(ColCountProperty);
            private set => SetValue(ColCountProperty, value);
        }

        public static readonly DependencyProperty ColCountProperty =
            DependencyProperty.Register("ColCount", typeof(int), typeof(Table), new PropertyMetadata(0));

        #endregion

        #region 事件

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitializeEvent(object sender, RoutedEventArgs e)
        {
            //获取行数和列数
            if (Children.Count == 0 || !(Children[0] is Tr firstTr)) return;
            if (DataSource == null)
            {
                //纯展示表格
                RowCount = Children.Count - (ShowHeader ? 0 : 1);
                ColCount = firstTr.Children.Count;
            }
            else
            {
                //非纯展示表格
                RowCount = DataSource.Rows.Count + (ShowHeader ? 1 : 0);
                ColCount = DataSource.Columns.Count;
            }
            if (RowCount <= 0 || ColCount <= 0) return;
            //构造行比例
            for (int i = 0; i < RowCount; i++) RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //构造单元格
            for (int i = 0; i < RowCount; i++)
            {
                //构造行
                int ii = ShowHeader ? i : i + 1;
                if (Children.Count <= ii || !(Children[ii] is Tr curTr))
                {
                    curTr = new Tr();
                    Children.Add(curTr);
                    //非纯展示表格需设置最大行高
                    RowDefinitions[i].Height = new GridLength(38, GridUnitType.Pixel);
                }
                SetRow(curTr, i);
                //构造列比例
                for (int k = 0; k < ColCount; k++)
                {
                    double width = 1;
                    if (firstTr.Children.Count > k && firstTr.Children[k] is Th th && !double.IsNaN(th.Width)) width = th.Width;
                    curTr.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width, GridUnitType.Star) });
                }
                //构造列
                for (int l = 0; l < ColCount; l++)
                {
                    //单元格内添加Border用以设置样式
                    Border border = new Border
                    {
                        Padding = new Thickness(2.5),
                        BorderBrush = FindResource("BrushTableBoder") as Brush,
                        BorderThickness = new Thickness(1),
                        Margin = new Thickness(l == 0 ? 0 : -1, i == 0 ? 0 : -1, 0, 0)
                    };
                    //列头
                    if (ShowHeader && i == 0)
                    {
                        if (curTr.Children.Count <= l || !(curTr.Children[l] is Th curTh))
                        {
                            curTh = new Th();
                            curTr.Children.Add(curTh);
                        }
                        if (curTh.Children.Count == 0)
                        {
                            //构造列头内容(默认Label)
                            Label label = new Label { Content = curTh.Title };
                            curTh.Children.Add(label);
                        }
                        SetColumn(curTh, l);
                        curTh.Children.Add(border);
                    }
                    else//数据列
                    {
                        if (curTr.Children.Count <= l || !(curTr.Children[l] is Td curTd))
                        {
                            curTd = new Td();
                            curTr.Children.Add(curTd);
                        }
                        SetColumn(curTd, l);
                        curTd.Children.Add(border);
                    }
                }
            }
            //构造外边框
            Border slideBorder = new Border
            {
                BorderBrush = FindResource("BrushTableBoder") as Brush,
                BorderThickness = new Thickness(1)
            };
            Children.Add(slideBorder);
            SetRowSpan(slideBorder, RowCount);
            SetColumnSpan(slideBorder, ColCount);
        }

        #endregion
    }

    /// <summary>
    /// 自定义表格列头
    /// </summary>
    public class Th : Grid
    {
        #region 字段名

        public string Filed
        {
            get => (string)GetValue(FiledProperty);
            set => SetValue(FiledProperty, value);
        }

        public static readonly DependencyProperty FiledProperty =
            DependencyProperty.Register("Filed", typeof(string), typeof(Th), new PropertyMetadata(""));

        #endregion

        #region 标题

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Th), new PropertyMetadata(""));

        #endregion

        //#region 绑定字段名

        //public string Filed
        //{
        //    get => (string)GetValue(FiledProperty);
        //    set => SetValue(FiledProperty, value);
        //}

        //public static readonly DependencyProperty FiledProperty =
        //    DependencyProperty.Register("Filed", typeof(string), typeof(Th), new PropertyMetadata(""));

        //#endregion
    }

    /// <summary>
    /// 自定义表格数据行
    /// </summary>
    public class Tr : Grid
    {

    }

    /// <summary>
    /// 自定义表格单元格
    /// </summary>
    public class Td : Grid
    {

    }
}
