using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using wpf_ui.Extends.Common;

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

        #region 事件

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
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
                ColCount = DataSource.Columns.Count + (TrOperation != TrOperation.None ? 1 : 0);
            }
            if (RowCount <= 0 || ColCount <= 0) return;
            //构造行比例
            for (int i = 0; i < RowCount; i++)
            {
                RowDefinition rowDefinition = new RowDefinition
                {
                    Height = DataSource == null ? new GridLength(1, GridUnitType.Star) : new GridLength(38, GridUnitType.Pixel)
                };
                if (RowDefinitions.Count > i) RowDefinitions[i] = rowDefinition; else RowDefinitions.Add(rowDefinition);
            }
            //构造单元格
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
                //构造列比例
                for (int k = 0; k < ColCount; k++)
                {
                    double proportion = 1;
                    if (firstTr.Children.Count > k && firstTr.Children[k] is Th th && !double.IsNaN(th.Proportion)) proportion = th.Proportion;
                    //若开启了行功能则最后一行默认大小
                    if (TrOperation != TrOperation.None && k == ColCount - 1) proportion = -96;
                    //负值为像素值，正直为比例值(星值)
                    ColumnDefinition columnDefinition = new ColumnDefinition { Width = new GridLength(Math.Abs(proportion), proportion < 0 ? GridUnitType.Pixel : GridUnitType.Star) };
                    if (curTr.ColumnDefinitions.Count > k) curTr.ColumnDefinitions[k] = columnDefinition; else curTr.ColumnDefinitions.Add(columnDefinition);
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
                        if (curTh.Children.Count == 1)
                        {
                            //构造列头内容(默认Label)
                            Label label = new Label { Content = curTh.Title };
                            curTh.Children.Add(label);
                        }
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
            //构造外边框
            Border slideBorder = new Border
            {
                Name = "yii",
                BorderBrush = FindResource("BrushTableBoder") as Brush,
                BorderThickness = new Thickness(1)
            };
            if (this.FindChild<Border>("yii") != null) return;
            Children.Add(slideBorder);
            SetRowSpan(slideBorder, RowCount);
            SetColumnSpan(slideBorder, ColCount);
        }

        #endregion

        #region 自定义方法

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
            string id = DataSource.Rows[index]["id"].ToStringEx();
            string content = string.IsNullOrWhiteSpace(th.Filed) ? "" : DataSource.Rows[index][th.Filed].ToStringEx();
            string style = th.InitStyle?.Invoke(content);
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
                    if (th.BtnClick != null && !style.ToStringEx().Contains("Disabled")) button.Click += delegate { th.BtnClick(id); };
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
                    if (th.BtnClick != null && !style.ToStringEx().Contains("Disabled"))
                    {
                        hyperlink.Click += delegate { th.BtnClick(id); };
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
        public delegate string StyleDelegate(string value);

        /// <summary>
        /// Button列点击事件
        /// </summary>
        /// <param name="id">列对应主键</param>
        /// <returns></returns>
        public delegate void BtnClickDelegate(string id);

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

        #region 列样式事件

        public StyleDelegate InitStyle
        {
            get => (StyleDelegate)GetValue(InitStyleProperty);
            set => SetValue(InitStyleProperty, value);
        }

        public static readonly DependencyProperty InitStyleProperty =
            DependencyProperty.Register("TiInitStyletle", typeof(StyleDelegate), typeof(Th), new PropertyMetadata(null));

        #endregion

        #region Button列点击事件

        public BtnClickDelegate BtnClick
        {
            get => (BtnClickDelegate)GetValue(BtnClickProperty);
            set => SetValue(BtnClickProperty, value);
        }

        public static readonly DependencyProperty BtnClickProperty =
            DependencyProperty.Register("BtnClick", typeof(BtnClickDelegate), typeof(Th), new PropertyMetadata(null));

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
        /// 操作列(编辑和删除)
        /// </summary>
        Deal
    }
}
