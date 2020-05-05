﻿using System.Windows;
using System.Windows.Controls;

namespace wpf_ui.Extends.DiyControls
{
    /// <summary>
    /// 自定义悬浮弹窗
    /// </summary>
    public class DiyPopup : Label
    {
        #region 悬浮弹窗位置

        public ShowPlace ShowPlace
        {
            get { return (ShowPlace)GetValue(ShowPlaceProperty); }
            set { SetValue(ShowPlaceProperty, value); }
        }

        public static readonly DependencyProperty ShowPlaceProperty =
            DependencyProperty.Register("ShowPlace", typeof(ShowPlace), typeof(DiyPopup), new PropertyMetadata(ShowPlace.Top));

        #region 圆角

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(DiyPopup), new PropertyMetadata(new CornerRadius(0)));

        #endregion

        #endregion

    }

    /// <summary>
    /// 悬浮弹窗位置
    /// </summary>
    public enum ShowPlace
    {
        /// <summary>
        /// 上
        /// </summary>
        Top,
        /// <summary>
        /// 左
        /// </summary>
        Left,
        /// <summary>
        /// 右
        /// </summary>
        Right,
        /// <summary>
        /// 下
        /// </summary>
        Bottom
    }
}