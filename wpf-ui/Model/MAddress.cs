using System.Collections.Generic;

namespace wpf_ui.Model
{
    /// <summary>
    /// 地址
    /// </summary>
    public class MAddress
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 父类主键
        /// </summary>
        public int PId { get; set; }
        /// <summary>
        /// 地区名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 地区级别
        /// </summary>
        public int Level { get; set; } = 0;
        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDel { get; set; } = false;
        /// <summary>
        /// 子级地址集合
        /// </summary>
        public List<MAddress> ListChild { get; set; }
    }
}
