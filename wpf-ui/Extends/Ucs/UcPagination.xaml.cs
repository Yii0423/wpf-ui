using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using wpf_ui.Extends.Common;
using wpf_ui.Extends.DiyControls;

namespace wpf_ui.Extends.Ucs
{
    /// <summary>
    /// 分页回调
    /// </summary>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageCounts">每页数据数</param>
    public delegate void PagingDelegate(int pageIndex, int pageCounts);

    /// <summary>
    /// UcPagination.xaml 的交互逻辑
    /// </summary>
    public partial class UcPagination : UserControl
    {
        /// <summary>
        /// 分页回调事件
        /// </summary>
        public PagingDelegate Paging;

        /// <summary>
        /// 数据总条数
        /// </summary>
        public int DataCounts { get; set; }

        /// <summary>
        /// 单页显示条数
        /// </summary>
        private int _pageCounts = 10;

        /// <summary>
        /// 总页数
        /// </summary>
        private int _allPages;

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                _pageIndex = value;
                InitPages();
            }
        }

        /// <summary>
        /// 当前页数
        /// </summary>
        private int _pageIndex = 1;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcPagination()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void UcPagination_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataCounts <= 0) Visibility = Visibility.Collapsed;
            //现实总条数
            LabDataCounts.Content = DataCounts;
            InitPages();
        }

        /// <summary>
        /// 构造分页按钮显示
        /// </summary>
        private void InitPages()
        {
            //清空原分页按钮
            SpPages.Children.Clear();
            //总页数
            _allPages = DataCounts % _pageCounts == 0 ? DataCounts / _pageCounts : DataCounts / _pageCounts + 1;
            //先移除上一页事件
            BtnPrev.Click -= BtnPrev_OnClick;
            //非第一页则启用前一页按钮并添加点击事件
            BtnPrev.Style = FindResource(_pageIndex == 1 ? "BtnPaginationDisabled" : "BtnPagination") as Style;
            if (_pageIndex > 1) BtnPrev.Click += BtnPrev_OnClick;
            //先移除下一页事件
            BtnNext.Click -= BtnNext_OnClick;
            //非最后一页则启用下一页按钮并添加点击事件
            BtnNext.Style = FindResource(_pageIndex == _allPages ? "BtnPaginationDisabled" : "BtnPagination") as Style;
            if (_pageIndex < _allPages) BtnNext.Click += BtnNext_OnClick;
            //统计分页按钮
            List<string> pages = new List<string>();
            if (_allPages <= 5)
            {
                //小于5页
                for (int i = 1; i <= _allPages; i++) pages.Add(i.ToString());
            }
            else
            {
                //大于5页
                pages.Add("1");
                if (_pageIndex <= 3)
                {
                    pages.Add("2");
                    pages.Add("3");
                    if (_pageIndex == 3) pages.Add("4");
                    pages.Add("...");
                }
                else if (_allPages - _pageIndex >= 3)
                {
                    pages.Add("...");
                    pages.Add((_pageIndex - 1).ToString());
                    pages.Add(_pageIndex.ToString());
                    pages.Add((_pageIndex + 1).ToString());
                    pages.Add("...");
                }
                else
                {
                    pages.Add("...");
                    if (_allPages - _pageIndex == 2) pages.Add((_allPages - 3).ToString());
                    pages.Add((_allPages - 2).ToString());
                    pages.Add((_allPages - 1).ToString());
                }
                pages.Add(_allPages.ToString());
            }
            //开始添加分页按钮
            foreach (string page in pages)
            {
                //当前页
                if (page.Equals(_pageIndex.ToString())) SpPages.Children.Add(new Label
                {
                    Style = FindResource("TagDefault") as Style,
                    Content = page,
                    MinWidth = 20,
                    Margin = new Thickness(5, 0, 0, 0),
                    FontSize = 12
                });
                //省略号
                else if (page.Equals("...")) SpPages.Children.Add(new Label
                {
                    Style = FindResource("TagPrimary") as Style,
                    Content = page,
                    MinWidth = 20,
                    Margin = new Thickness(5, 0, 0, 0),
                    BorderThickness = new Thickness(0),
                    FontSize = 12
                });
                //可点击分页
                else
                {
                    DiyButton btnPage = new DiyButton
                    {
                        Style = FindResource("BtnPagination") as Style,
                        Content = page
                    };
                    //点击事件
                    btnPage.Click += delegate
                    {
                        _pageIndex = Convert.ToInt32(page);
                        InitPages();
                    };
                    SpPages.Children.Add(btnPage);
                }
            }
            //触发分页回调事件
            Paging?.Invoke(_pageIndex, _pageCounts);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        private void BtnPrev_OnClick(object sender, RoutedEventArgs e)
        {
            _pageIndex--;
            InitPages();
        }

        /// <summary>
        /// 下一页
        /// </summary>
        private void BtnNext_OnClick(object sender, RoutedEventArgs e)
        {
            _pageIndex++;
            InitPages();
        }

        /// <summary>
        /// 跳转指定页
        /// </summary>
        private void BtnTurn_OnClick(object sender, RoutedEventArgs e)
        {
            int.TryParse(TxtPageIndex.Text.Trim(), out int pageIndex);
            if (pageIndex <= 0) pageIndex = 1;
            if (pageIndex > _allPages) pageIndex = _allPages;
            TxtPageIndex.Text = pageIndex.ToString();
            _pageIndex = pageIndex;
            InitPages();
        }

        /// <summary>
        /// 更改单页显示数据
        /// </summary>
        private void CmbPaging_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string pageCounts = (CmbPaging.SelectedItem as ComboBoxItem)?.Content.ToStringEx().Replace("条/页", "");
            int.TryParse(pageCounts, out _pageCounts);
            _pageIndex = 1;
            InitPages();
        }
    }
}
