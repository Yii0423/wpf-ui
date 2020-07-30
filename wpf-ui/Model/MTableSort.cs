namespace wpf_ui.Model
{
    /// <summary>
    /// 列排序状态
    /// </summary>
    public class MTableSort
    {
        /// <summary>
        /// 
        /// </summary>
        public string SortName { get; set; }
        /// <summary>
        /// 排序状态
        /// </summary>
        public ThSort SortState { get; set; }
    }

    /// <summary>
    /// 列排序
    /// </summary>
    public enum ThSort
    {
        /// <summary>
        /// 未排序
        /// </summary>
        None,
        /// <summary>
        /// 正序
        /// </summary>
        Asc,
        /// <summary>
        /// 倒序
        /// </summary>
        Desc
    }
}
