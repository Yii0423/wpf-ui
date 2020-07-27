using System.Collections.Generic;

namespace wpf_ui.Model
{
    /// <summary>
    /// 菜单项
    /// </summary>
    public class MMenuItem
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 是否居右显示
        /// </summary>
        public bool IsRight { get; set; }
        /// <summary>
        /// 菜单子项
        /// </summary>
        public List<MMenuItem> ChildItem { get; set; } = new List<MMenuItem>();
        /// <summary>
        /// 是否为顶级菜单
        /// </summary>
        public bool IsTop { get; set; }
    }
}
