using System;

namespace wpf_ui.Model
{
    /// <summary>
    /// 窗体属性类
    /// </summary>
    public class MOpen
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public Uri Url { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public double Width { get; set; } = 600;
        /// <summary>
        /// 高度
        /// </summary>
        public double Height { get; set; } = 400;
        /// <summary>
        /// 是否固定窗体大小
        /// </summary>
        public bool FixedSize { get; set; }
        /// <summary>
        /// 是否单击遮罩自动关闭
        /// </summary>
        public bool WhiteSpaceClose { get; set; }
        /// <summary>
        /// 是否打开即最大化
        /// </summary>
        public bool StartMax { get; set; }
    }
}
