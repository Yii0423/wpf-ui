using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using wpf_ui.Model;

namespace wpf_ui.View
{
    /// <summary>
    /// Index.xaml 的交互逻辑
    /// </summary>
    public partial class Index : Page
    {
        public Index()
        {
            InitializeComponent();
        }

        private void Index_OnLoaded(object sender, RoutedEventArgs e)
        {
            #region 卡片1

            Card1.Title = "快捷方式";
            Card1.MenuItemCount = 8;
            Card1.ControlStyle = "BtnCard1";
            Card1.ItemSources = new List<MMenuItem>
            {
                new MMenuItem
                {
                    Title = "主页一",
                    Icon = "Icon-Dashboard"
                },
                new MMenuItem
                {
                    Title = "主页二",
                    Icon = "Icon-Terminal"
                },
                new MMenuItem
                {
                    Title = "弹层",
                    Icon = "Icon-Reorder"
                },
                new MMenuItem
                {
                    Title = "聊天",
                    Icon = "Icon-Phone"
                },
                new MMenuItem
                {
                    Title = "进度条",
                    Icon = "Icon-Magnet"
                },
                new MMenuItem
                {
                    Title = "工单",
                    Icon = "Icon-File-Alt"
                },
                new MMenuItem
                {
                    Title = "用户",
                    Icon = "Icon-User"
                },
                new MMenuItem
                {
                    Title = "设置",
                    Icon = "Icon-Cog"
                },
                new MMenuItem
                {
                    Title = "其他1",
                    Icon = "Icon-Cogs"
                },
                new MMenuItem
                {
                    Title = "其他2",
                    Icon = "Icon-Light-Bulb"
                }
            };

            #endregion

            #region 卡片2

            Card2.Title = "待办事项";
            Card2.MenuItemCount = 4;
            Card2.ControlStyle = "BtnCard2";
            Card2.ItemSources = new List<MMenuItem>
            {
                new MMenuItem
                {
                    Title = "待审评论",
                    SubTitle = "66"
                },
                new MMenuItem
                {
                    Title = "待审帖子",
                    SubTitle = "12"
                },
                new MMenuItem
                {
                    Title = "待审商品",
                    SubTitle = "99"
                },
                new MMenuItem
                {
                    Title = "待发货",
                    SubTitle = "20"
                },
                new MMenuItem
                {
                    Title = "待审友情链接",
                    SubTitle = "5",
                    IsTop = true
                }
            };

            #endregion

            #region 卡片3

            Card3.Title = "产品动态";
            Card3.MenuItemCount = 1;
            Card3.ControlStyle = "BtnPrimary";
            Card3.AutoChange = true;
            Card3.ItemSources = new List<MMenuItem>
            {
                new MMenuItem
                {
                    Title = "layuiAdmin 快速上手文档",
                    Style = "BtnAutoSize"
                },
                new MMenuItem
                {
                    Title = "layuiAdmin 会员讨论专区",
                    Style = "BtnAutoSize2"
                },
                new MMenuItem
                {
                    Title = "获得 layuiAdmin 官方后台模板系统",
                    Style = "BtnAutoSize3"
                }
            };

            #endregion

            #region 数据表1

            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("KeyWords");
            dt.Columns.Add("SearchTimes");
            dt.Columns.Add("UserCounts");
            for (int i = 0; i < 10; i++) dt.Rows.Add(i * 10, $"关键词关键词关键词关键词关键词关键词关键词关键词{i * 99}", $"/Content/Images/avatar{i}.jpg", i * 60);
            Table1.DataSource = dt;

            #endregion

            #region 数据表1编辑/删除/排序事件

            Table1.Edit += id => { MessageBox.Show($"编辑事件：{id}"); };

            Table1.Delete += id => { MessageBox.Show($"删除事件：{id}"); };

            Table1.Sort

            #endregion

            #region 数据表1绑定列样式事件

            ThId.Converter = content =>
            {
                if (content.Contains("20")) return new MTdConverter { Style = "PbDefault" };
                if (content.Contains("30")) return new MTdConverter { Style = "PbNormal" };
                if (content.Contains("50")) return new MTdConverter { Style = "PbWarm" };
                if (content.Contains("60")) return new MTdConverter { Style = "PbDanger" };
                return null;
            };

            ThKeyWords.Converter = content =>
            {
                if (content.Contains("3")) return new MTdConverter { Style = "BtnHyperlinkDisabled" };
                return null;
            };

            ThUserCounts.Converter = content =>
            {
                if (content.Contains("2")) return new MTdConverter { Content = $"{content}%", Style = "TagPrimary" };
                if (content.Contains("8")) return new MTdConverter { Style = "TagDefault" };
                if (content.Contains("4")) return new MTdConverter { Style = "TagNormal" };
                if (content.Contains("0")) return new MTdConverter { Style = "TagDisabled" };
                if (content.Contains("6")) return new MTdConverter { Content = $"{content}$", Style = "TagWarm" };
                if (content.Contains("5")) return new MTdConverter { Content = $"{content}.00", Style = "TagDanger" };
                return null;
            };

            #endregion

            #region 数据表1绑定Button列点击事件

            ThKeyWords.Click = id => { MessageBox.Show($"自定义事件：{id}"); };

            #endregion
        }

        private void BtnGetVip_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("KeyWords");
            dt.Columns.Add("SearchTimes");
            dt.Columns.Add("UserCounts");
            for (int i = 0; i < 10; i++)
                dt.Rows.Add(random.Next(0, 100),
                                        $"关键词关键词关键词关键词关键词关键词关键词关键词{i * 99}",
                                        $"/Content/Images/avatar{random.Next(0, 9)}.jpg",
                                        i * random.Next(30, 60));
            Table1.DataSource = dt;
        }
    }
}
