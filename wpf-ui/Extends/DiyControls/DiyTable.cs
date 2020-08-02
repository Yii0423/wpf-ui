using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wpf_ui.Extends.Common;
using wpf_ui.Model;

namespace wpf_ui.Extends.DiyControls
{
    /// <summary>
    /// 自定义表格
    /// </summary>
    public class Table : Grid
    {
        /// <summary>
        /// 编辑删除事件
        /// </summary>
        /// <param name="id">列对应主键</param>
        /// <returns></returns>
        public delegate void DealDelegate(string id);

        /// <summary>
        /// 列头排序事件
        /// </summary>
        /// <param name="tableSort">列排序对象</param>
        public delegate void SortDelegate(MTableSort tableSort);

        /// <summary>
        /// 构造函数
        /// </summary>
        public Table()
        {
            //绑定初始化事件
            Loaded += delegate { Init(); };
        }

        #region 数据源(目前仅支持DataTable)

        public DataTable DataSource
        {
            get => (DataTable)GetValue(DataSourceProperty);
            set
            {
                SetValue(DataSourceProperty, value);
                Init();
            }
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

        #region 行功能启用

        public TrOperation TrOperation
        {
            get => (TrOperation)GetValue(TrOperationProperty);
            set => SetValue(TrOperationProperty, value);
        }

        public static readonly DependencyProperty TrOperationProperty =
            DependencyProperty.Register("TrOperation", typeof(TrOperation), typeof(Table), new PropertyMetadata(TrOperation.None));

        #endregion

        #region 编辑事件

        public DealDelegate Edit
        {
            get => (DealDelegate)GetValue(EditProperty);
            set => SetValue(EditProperty, value);
        }

        public static readonly DependencyProperty EditProperty =
            DependencyProperty.Register("Edit", typeof(DealDelegate), typeof(Table), new PropertyMetadata(null));

        #endregion

        #region 删除事件

        public DealDelegate Delete
        {
            get => (DealDelegate)GetValue(DeleteProperty);
            set => SetValue(DeleteProperty, value);
        }

        public static readonly DependencyProperty DeleteProperty =
            DependencyProperty.Register("Delete", typeof(DealDelegate), typeof(Table), new PropertyMetadata(null));

        #endregion

        #region 当前排序状态

        public MTableSort TableSort
        {
            get => (MTableSort)GetValue(TableSortProperty);
            private set => SetValue(TableSortProperty, value);
        }

        public static readonly DependencyProperty TableSortProperty =
            DependencyProperty.Register("TableSort", typeof(MTableSort), typeof(Table), new PropertyMetadata(null));

        #endregion

        #region 列排序事件

        public SortDelegate Sort
        {
            get => (SortDelegate)GetValue(SortProperty);
            set => SetValue(SortProperty, value);
        }

        public static readonly DependencyProperty SortProperty =
            DependencyProperty.Register("Sort", typeof(SortDelegate), typeof(Table), new PropertyMetadata(null));

        #endregion

        #region 选中数据行

        public List<DataRow> SelectRows
        {
            get => (List<DataRow>)GetValue(SelectRowsProperty);
            private set => SetValue(SelectRowsProperty, value);
        }

        public static readonly DependencyProperty SelectRowsProperty =
            DependencyProperty.Register("SelectRows", typeof(List<DataRow>), typeof(Table), new PropertyMetadata(null));

        #endregion

        #region 事件

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            //获取行数和列数
            if (Children.Count == 0 || !(Children[0] is Tr firstTr)) return;
            //初始化选中数据行集合
            SelectRows = new List<DataRow>();
            //非纯展示表格构造前清空原数据
            if (Children.Count > 1 && DataSource != null) Children.RemoveRange(1, Children.Count - 1);
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
                ColCount = DataSource.Columns.Count + (TrOperation != TrOperation.None ? 1 : 0);
            }
            if (RowCount <= 0 || ColCount <= 0) return;
            //先清空
            RowDefinitions.Clear();
            //再构造行比例
            for (int i = 0; i < RowCount; i++)
            {
                RowDefinition rowDefinition = new RowDefinition
                {
                    Height = DataSource != null || ShowHeader && i == 0 ? new GridLength(38, GridUnitType.Pixel) : new GridLength(1, GridUnitType.Star)
                };
                RowDefinitions.Add(rowDefinition);
            }
            //构造表格
            for (int i = 0; i < RowCount; i++)
            {
                //构造行
                int ii = ShowHeader ? i : i + 1;
                if (Children.Count <= ii || !(Children[ii] is Tr curTr))
                {
                    curTr = new Tr();
                    Children.Add(curTr);
                }
                //设置行
                InitTr(curTr, i);
                //先清空
                curTr.ColumnDefinitions.Clear();
                //再构造列比例
                for (int k = 0; k < ColCount; k++)
                {
                    double proportion = 1;
                    if (firstTr.Children.Count > k && firstTr.Children[k] is Th th && !double.IsNaN(th.Proportion)) proportion = th.Proportion;
                    //若开启了行功能则最后一列默认大小
                    if (TrOperation != TrOperation.None && k == ColCount - 1) proportion = -96;
                    //负值为像素值，正直为比例值(星值)
                    ColumnDefinition columnDefinition = new ColumnDefinition { Width = new GridLength(Math.Abs(proportion), proportion < 0 ? GridUnitType.Pixel : GridUnitType.Star) };
                    curTr.ColumnDefinitions.Add(columnDefinition);
                }
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
                        SetColumn(curTh, l);
                        //清空子控件避免重复初始化
                        curTh.Children.Clear();
                        //设置列头背景色
                        border.Background = FindResource("BrushBackground") as Brush;
                        curTh.Children.Add(border);
                        //后加标题以防被Border遮挡
                        if (curTh.Children.Count == 1) InitTh(curTh);
                    }
                    else//数据列
                    {
                        if (curTr.Children.Count <= l || !(curTr.Children[l] is Td curTd))
                        {
                            curTd = new Td();
                            curTr.Children.Add(curTd);
                        }
                        SetColumn(curTd, l);
                        //清空子控件避免重复初始化(注意纯展示表格的原始数据)
                        if (DataSource != null) curTd.Children.Clear();
                        curTd.Children.Add(border);
                        //绑定数据
                        if (DataSource != null)
                        {
                            //取该列绑定的字段名
                            if (firstTr.Children.Count <= l || !(firstTr.Children[l] is Th th))
                            {
                                th = new Th();
                            }
                            if (TrOperation != TrOperation.None && l == ColCount - 1) th.ThType = ThType.Deal;
                            InitTd(th, curTd, i - (ShowHeader ? 1 : 0));
                        }
                    }
                }
            }
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化列头
        /// </summary>
        /// <param name="th">列头</param>
        private void InitTh(Th th)
        {
            DockPanel dockPanel = new DockPanel();
            //构造列头内容
            switch (th.ThType)
            {
                case ThType.CheckBox://复选框
                    CheckBox checkBox = new CheckBox { HorizontalAlignment = HorizontalAlignment.Center, IsHitTestVisible = false };
                    if (!(th.Children[0] is Border border)) return;
                    border.MouseLeftButtonDown += delegate
                    {
                        bool isChecked = !(checkBox.IsChecked ?? false);
                        checkBox.IsChecked = isChecked;
                        //全选/取消全选
                        foreach (var child in Children)
                        {
                            if (!(child is Tr tr)) continue;
                            CheckBox checkBox1 = tr.FindChild<CheckBox>();
                            if (checkBox1 != null) checkBox1.IsChecked = isChecked;
                        }
                    };
                    th.Cursor = Cursors.Hand;
                    dockPanel.Children.Add(checkBox);
                    break;
                default://仅文本
                    Label label = new Label { Content = th.Title, HorizontalAlignment = HorizontalAlignment.Left };
                    dockPanel.Children.Add(label);
                    break;
            }
            //构造排序
            if (th.SortEnable)
            {
                //设置鼠标形状
                dockPanel.Cursor = Cursors.Hand;
                //排序标志区域
                StackPanel stackPanel1 = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                //Asc排序标志
                string style = TableSort != null && TableSort.SortState == ThSort.Asc && TableSort.SortName.Equals(th.Filed) ? "BrushMenu" : "BrushFont";
                Path pathAsc = new Path
                {
                    Style = FindResource("Icon") as Style,
                    Data = FindResource("Icon-Sort-Up") as Geometry,
                    Fill = FindResource(style) as Brush,
                    Height = 6
                };
                stackPanel1.Children.Add(pathAsc);
                //Desc排序标志
                style = TableSort != null && TableSort.SortState == ThSort.Desc && TableSort.SortName.Equals(th.Filed) ? "BrushMenu" : "BrushFont";
                Path pathDesc = new Path
                {
                    Margin = new Thickness(0, 1, 0, 0),
                    Style = FindResource("Icon") as Style,
                    Data = FindResource("Icon-Sort-Down") as Geometry,
                    Fill = FindResource(style) as Brush,
                    Height = 6
                };
                stackPanel1.Children.Add(pathDesc);
                //外部构造StackPanel用以填充满剩余部分以便触发排序单击事件
                StackPanel stackPanel2 = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Background = Brushes.Transparent
                };
                stackPanel2.Children.Add(stackPanel1);
                DockPanel.SetDock(stackPanel2, Dock.Right);
                dockPanel.Children.Add(stackPanel2);
                //绑定单击事件
                dockPanel.MouseLeftButtonUp += delegate
                {
                    if (TableSort == null) TableSort = new MTableSort();
                    //切换排序列则清空上次排序状态
                    if (!th.Filed.Equals(TableSort.SortName)) TableSort.SortState = ThSort.Asc;
                    else TableSort.SortState = TableSort.SortState == ThSort.None ? ThSort.Asc : TableSort.SortState == ThSort.Asc ? ThSort.Desc : ThSort.None;
                    //更新排序列
                    TableSort.SortName = th.Filed;
                    Sort?.Invoke(TableSort);
                };
            }
            //合体
            th.Children.Add(dockPanel);
        }

        /// <summary>
        /// 初始化行
        /// </summary>
        /// <param name="tr">行</param>
        /// <param name="index">行索引</param>
        private void InitTr(Tr tr, int index = 0)
        {
            //设置行所处的RowDefinition
            SetRow(tr, index);
            //显示列头的情况下跳过
            if (index == 0 && ShowHeader) return;
            //鼠标移入行
            void MouseEnter(object sender, MouseEventArgs e) => tr.Background = FindResource("BrushBackground") as Brush;
            tr.RemoveHandler(MouseEnterEvent, (MouseEventHandler)MouseEnter);
            tr.AddHandler(MouseEnterEvent, (MouseEventHandler)MouseEnter);
            //鼠标移出行
            void MouseLeave(object sender, MouseEventArgs e) => tr.Background = Brushes.Transparent;
            tr.RemoveHandler(MouseLeaveEvent, (MouseEventHandler)MouseLeave);
            tr.AddHandler(MouseLeaveEvent, (MouseEventHandler)MouseLeave);
        }

        /// <summary>
        /// 初始化单元格
        /// </summary>
        /// <param name="th">列头</param>
        /// <param name="td">单元格</param>
        /// <param name="index">行索引</param>
        /// <returns></returns>
        private void InitTd(Th th, Td td, int index = 0)
        {
            string id = DataSource.Rows[index][0].ToStringEx();
            string content = string.IsNullOrWhiteSpace(th.Filed) ? "" : DataSource.Rows[index][th.Filed].ToStringEx();
            MTdConverter tdStyle = th.Converter?.Invoke(content);
            string style = tdStyle?.Style;
            content = string.IsNullOrWhiteSpace(tdStyle?.Content) ? content : tdStyle.Content;
            Thickness thickness = new Thickness(5, 0, 5, 0);
            UIElement uiElement;
            switch (th.ThType)
            {
                case ThType.Button://按钮
                    DiyButton button = new DiyButton
                    {
                        Margin = thickness,
                        Content = content,
                        Style = FindResource(style ?? "BtnPrimary") as Style
                    };
                    if (th.Click != null && !style.ToStringEx().Contains("Disabled")) button.Click += delegate { th.Click(id); };
                    uiElement = button;
                    break;
                case ThType.Progressbar://进度条
                    double.TryParse(content, out double value);
                    DiyProgressbar progressbar = new DiyProgressbar
                    {
                        Margin = thickness,
                        Value = value,
                        Style = FindResource(style ?? "PbDefault") as Style
                    };
                    uiElement = progressbar;
                    break;
                case ThType.Hyperlink://超链接
                    DiyButton hyperlink = new DiyButton
                    {
                        Margin = thickness,
                        Content = content,
                        Style = FindResource(style ?? "BtnHyperlink") as Style,
                        HorizontalAlignment = HorizontalAlignment.Left
                    };
                    if (th.Click != null && !style.ToStringEx().Contains("Disabled"))
                    {
                        hyperlink.Click += delegate { th.Click(id); };
                    }
                    uiElement = hyperlink;
                    break;
                case ThType.Image://图片
                    Image image = new Image
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(5),
                        Source = new BitmapImage(new Uri(content, UriKind.RelativeOrAbsolute))
                    };
                    RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                    uiElement = image;
                    break;
                case ThType.CheckBox://复选框
                    CheckBox checkBox = new CheckBox { HorizontalAlignment = HorizontalAlignment.Center, IsHitTestVisible = false };
                    checkBox.Checked += delegate { SelectRows.Add(DataSource.Rows[index]); };
                    checkBox.Unchecked += delegate { SelectRows.Remove(DataSource.Rows[index]); };
                    uiElement = checkBox;
                    td.MouseLeftButtonDown += delegate { checkBox.IsChecked = !(checkBox.IsChecked ?? false); };
                    td.Cursor = Cursors.Hand;
                    break;
                case ThType.Deal://操作列
                    StackPanel spDeal = new StackPanel { Orientation = Orientation.Horizontal };
                    if (TrOperation == TrOperation.Both || TrOperation == TrOperation.Edit)
                    {
                        //编辑
                        DiyButton btnEdit = new DiyButton
                        {
                            Content = "编辑",
                            Style = FindResource("TableBtnEdit") as Style
                        };
                        btnEdit.Click += delegate { Edit?.Invoke(id); };
                        spDeal.Children.Add(btnEdit);
                    }
                    if (TrOperation == TrOperation.Both || TrOperation == TrOperation.Delete)
                    {
                        //删除
                        DiyButton btnDelete = new DiyButton
                        {
                            Content = "删除",
                            Style = FindResource("TableBtnDelete") as Style
                        };
                        btnDelete.Click += delegate { Delete?.Invoke(id); };
                        spDeal.Children.Add(btnDelete);
                    }
                    uiElement = spDeal;
                    break;
                default://仅文本
                    Label label = new Label
                    {
                        Margin = thickness,
                        Content = content,
                        Style = FindResource(style ?? "TagPrimary") as Style
                    };
                    uiElement = label;
                    break;
            }
            td.Children.Add(uiElement);
        }

        #endregion
    }

    /// <summary>
    /// 自定义表格列头
    /// </summary>
    public class Th : Grid
    {
        /// <summary>
        /// 列样式委托
        /// </summary>
        /// <param name="value">列对应数据内容</param>
        /// <returns></returns>
        public delegate MTdConverter ConverterDelegate(string value);

        /// <summary>
        /// 列点击事件
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public delegate void ClickDelegate(string id);

        #region 所占比例

        public double Proportion
        {
            get => (double)GetValue(ProportionProperty);
            set => SetValue(ProportionProperty, value);
        }

        public static readonly DependencyProperty ProportionProperty =
            DependencyProperty.Register("Proportion", typeof(double), typeof(Th), new PropertyMetadata(1d));

        #endregion

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

        #region 列类型

        public ThType ThType
        {
            get => (ThType)GetValue(ThTypeProperty);
            set => SetValue(ThTypeProperty, value);
        }

        public static readonly DependencyProperty ThTypeProperty =
            DependencyProperty.Register("ThType", typeof(ThType), typeof(Th), new PropertyMetadata(ThType.Normal));

        #endregion

        #region 列转换事件

        public ConverterDelegate Converter
        {
            get => (ConverterDelegate)GetValue(ConverterProperty);
            set => SetValue(ConverterProperty, value);
        }

        public static readonly DependencyProperty ConverterProperty =
            DependencyProperty.Register("Converter", typeof(ConverterDelegate), typeof(Th), new PropertyMetadata(null));

        #endregion

        #region 列点击事件

        public ClickDelegate Click
        {
            get => (ClickDelegate)GetValue(ClickProperty);
            set => SetValue(ClickProperty, value);
        }

        public static readonly DependencyProperty ClickProperty =
            DependencyProperty.Register("Click", typeof(ClickDelegate), typeof(Th), new PropertyMetadata(null));

        #endregion

        #region 是否启用排序

        public bool SortEnable
        {
            get => (bool)GetValue(SortEnableProperty);
            set => SetValue(SortEnableProperty, value);
        }

        public static readonly DependencyProperty SortEnableProperty =
            DependencyProperty.Register("SortEnable", typeof(bool), typeof(Th), new PropertyMetadata(false));

        #endregion
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

    /// <summary>
    /// 行功能启用
    /// </summary>
    public enum TrOperation
    {
        /// <summary>
        /// 全部不启用
        /// </summary>
        None,
        /// <summary>
        /// 启用编辑
        /// </summary>
        Edit,
        /// <summary>
        /// 启用删除
        /// </summary>
        Delete,
        /// <summary>
        /// 全部启用
        /// </summary>
        Both
    }

    /// <summary>
    /// 列类型
    /// </summary>
    public enum ThType
    {
        /// <summary>
        /// 默认(仅文本)
        /// </summary>
        Normal,
        /// <summary>
        /// 按钮
        /// </summary>
        Button,
        /// <summary>
        /// 进度条
        /// </summary>
        Progressbar,
        /// <summary>
        /// 超链接
        /// </summary>
        Hyperlink,
        /// <summary>
        /// 图片
        /// </summary>
        Image,
        /// <summary>
        /// 复选框
        /// </summary>
        CheckBox,
        /// <summary>
        /// 操作列(编辑和删除)
        /// </summary>
        Deal
    }
}
