using System.Collections.Generic;
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
        }
    }
}
