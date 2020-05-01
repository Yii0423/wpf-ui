using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using wpf_ui.Extends.Ucs;

namespace wpf_ui
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            pop1.IsOpen = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //顶部菜单数据源
            MenuTop.ItemSources = new List<MenuTopItem>
            {
                new MenuTopItem { Title = "最新活动", IsTop = true, ChildItem = new List<MenuTopItem>{
                        new MenuTopItem { Title = "移动模块" },
                        new MenuTopItem { Title = "后台模块", ChildItem = new List<MenuTopItem>{
                                new MenuTopItem { Title = "后台顶部" },
                                new MenuTopItem { Title = "后台左侧" },
                                new MenuTopItem { Title = "后台下方" }
                            }
                         }
                    }
                },
                new MenuTopItem { Title = "产品", IsTop = true },
                new MenuTopItem { Title = "大数据", IsTop = true, ChildItem = new List<MenuTopItem>{
                        new MenuTopItem { Title = "移动模块" },
                        new MenuTopItem { Title = "后台模块", ChildItem = new List<MenuTopItem>{
                                new MenuTopItem { Title = "后台顶部" },
                                new MenuTopItem { Title = "后台左侧" },
                                new MenuTopItem { Title = "后台下方", ChildItem = new List<MenuTopItem>{
                                new MenuTopItem { Title = "后台顶部" },
                                new MenuTopItem { Title = "后台左侧" },
                                new MenuTopItem { Title = "后台下方" }
                            }
                                }
                            }
                        },
                        new MenuTopItem { Title = "电商平台" }
                    }
                },
                new MenuTopItem { Title = "解决方案", IsTop = true },
                new MenuTopItem { Title = "社区", IsTop = true, ChildItem = new List<MenuTopItem>{
                                new MenuTopItem { Title = "后台顶部" },
                                new MenuTopItem { Title = "后台左侧" },
                                new MenuTopItem { Title = "后台下方" }
                            }
                }
            };
            //侧边菜单数据源
            MenuLeft.ItemSources = new List<MenuTopItem>
            {
                new MenuTopItem { Title = "Icon-Home", IsTop = true, ChildItem = new List<MenuTopItem>{
                        new MenuTopItem { Title = "移动模块" },
                        new MenuTopItem { Title = "后台模块", ChildItem = new List<MenuTopItem>{
                                new MenuTopItem { Title = "后台顶部" },
                                new MenuTopItem { Title = "后台左侧" },
                                new MenuTopItem { Title = "后台下方" }
                            }
                         }
                    }
                },
                new MenuTopItem { Title = "Icon-Music", IsTop = true },
                new MenuTopItem { Title = "Icon-Calendar", IsTop = true, ChildItem = new List<MenuTopItem>{
                        new MenuTopItem { Title = "移动模块" },
                        new MenuTopItem { Title = "后台模块", ChildItem = new List<MenuTopItem>{
                                new MenuTopItem { Title = "后台顶部" },
                                new MenuTopItem { Title = "后台左侧" },
                                new MenuTopItem { Title = "后台下方", ChildItem = new List<MenuTopItem>{
                                new MenuTopItem { Title = "后台顶部" },
                                new MenuTopItem { Title = "后台左侧" },
                                new MenuTopItem { Title = "后台下方" }
                            }
                                }
                            }
                        },
                        new MenuTopItem { Title = "电商平台" }
                    }
                },
                new MenuTopItem { Title = "Icon-Rss", IsTop = true },
                new MenuTopItem { Title = "Icon-Plane", IsTop = true, ChildItem = new List<MenuTopItem>{
                                new MenuTopItem { Title = "后台顶部" },
                                new MenuTopItem { Title = "后台左侧" },
                                new MenuTopItem { Title = "后台下方" }
                            }
                }
            };
        }
    }
}
