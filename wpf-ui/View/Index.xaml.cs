﻿using System.Collections.Generic;
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
            for (int i = 0; i < 10; i++) dt.Rows.Add(i * 10, $"关键词{i * 99}", $"/Content/Images/avatar{i}.jpg", i * 60);
            Table1.DataSource = dt;

            #endregion

            #region 数据表1编辑删除事件

            Table1.Edit += id => { MessageBox.Show($"编辑事件：{id}"); };

            Table1.Delete += id => { MessageBox.Show($"删除事件：{id}"); };

            #endregion

            #region 数据表1绑定列样式事件

            ThId.InitStyle = content =>
            {
                if (content.Contains("20")) return "PbDefault";
                if (content.Contains("30")) return "PbNormal";
                if (content.Contains("50")) return "PbWarm";
                if (content.Contains("60")) return "PbDanger";
                return null;
            };

            ThKeyWords.InitStyle = content =>
            {
                if (content.Contains("3")) return "BtnHyperlinkDisabled";
                return null;
            };

            ThUserCounts.InitStyle = content =>
            {
                if (content.Contains("120")) return "TagPrimary";
                if (content.Contains("180")) return "TagDefault";
                if (content.Contains("240")) return "TagNormal";
                if (content.Contains("300")) return "TagDisabled";
                if (content.Contains("360")) return "TagWarm";
                if (content.Contains("420")) return "TagDanger";
                return null;
            };

            #endregion

            #region 数据表1绑定Button列点击事件

            ThKeyWords.BtnClick = id => { MessageBox.Show($"自定义事件：{id}"); };

            #endregion
        }
    }
}
