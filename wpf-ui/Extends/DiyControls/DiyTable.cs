using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using wpf_ui.Extends.Common;

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
            for (int i = 0; i < RowCount; i++)
            {
                RowDefinition rowDefinition = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
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
                if (DataSource != null) RowDefinitions[i].Height = new GridLength(38, GridUnitType.Pixel);
                SetRow(curTr, i);
                //构造列比例
                for (int k = 0; k < ColCount; k++)
                {
                    double proportion = 1;
                    if (firstTr.Children.Count > k && firstTr.Children[k] is Th th && !double.IsNaN(th.Proportion)) proportion = th.Proportion;
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
                            if (!(firstTr.Children[l] is Th th)) return;
                            curTd.Children.Add(InitTd(th, i - 1));
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
        /// 初始化单元格内容
        /// </summary>
        /// <param name="th">对应列</param>
        /// <param name="index">行索引</param>
        /// <returns></returns>
        private UIElement InitTd(Th th, int index = 0)
        {
            string id = DataSource.Rows[index][0].ToStringEx();
            string content = DataSource.Rows[index][th.Filed].ToStringEx();
            string style = th.InitStyle?.Invoke(content);
            Thickness thickness = new Thickness(5, 0, 5, 0);
            switch (th.ThType)
            {
                case ThType.Button://按钮
                    DiyButton button = new DiyButton
                    {
                        Margin = thickness,
                        Content = content,
                        Style = FindResource(style ?? "TableBtnPrimary") as Style
                    };
                    if (th.BtnClick != null && !style.ToStringEx().Contains("Disabled")) button.Click += delegate { th.BtnClick(id); };
                    return button;
                case ThType.Progressbar://进度条
                    double.TryParse(content, out double value);
                    DiyProgressbar progressbar = new DiyProgressbar
                    {
                        Margin = thickness,
                        Value = value,
                        Style = FindResource(style ?? "PbDefault") as Style
                    };
                    return progressbar;
                case ThType.Hyperlink://超链接
                    return new DiyButton { Margin = thickness, Content = content, Style = FindResource(style ?? "BtnHyperlink") as Style };
                default://仅文本
                    Label label = new Label
                    {
                        Margin = thickness,
                        Content = content,
                        Style = FindResource(style ?? "TagPrimary") as Style
                    };
                    return label;
            }
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
        Hyperlink
    }
}
