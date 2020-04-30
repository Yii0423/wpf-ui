﻿using System.Windows;
using System.Windows.Media;

namespace wpf_ui.Extends.Common
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// 查找指定类型的父控件
        /// </summary>
        /// <typeparam name="T">要查找的控件类型</typeparam>
        /// <param name="root">查找源</param>
        /// <param name="name">要查找的控件名称</param>
        /// <returns></returns>
        public static T FindParent<T>(this DependencyObject root, string name = "") where T : DependencyObject
        {
            if (root == null) return null;
            DependencyObject d = VisualTreeHelper.GetParent(root);
            if (d == null) return null;
            if (!(d is T childType)
                || ((d is FrameworkElement) && !string.IsNullOrWhiteSpace(name) && (d as FrameworkElement).Name != name))
            {

                return FindParent<T>(d, name);
            }
            else
            {
                return childType;
            }
        }

        /// <summary>
        /// 查找指定类型的子控件
        /// </summary>
        /// <typeparam name="T">要查找的控件类型</typeparam>
        /// <param name="root">查找源</param>
        /// <param name="name">要查找的控件名称</param>
        /// <returns></returns>
        public static T FindChild<T>(this DependencyObject root, string name = "") where T : DependencyObject
        {
            if (root == null) return null;
            T founded = null;
            for (int j = 0; j < VisualTreeHelper.GetChildrenCount(root); j++)
            {
                DependencyObject d = VisualTreeHelper.GetChild(root, j);
                if (!(d is T childType)
                    || ((d is FrameworkElement) && !string.IsNullOrWhiteSpace(name) && (d as FrameworkElement).Name != name))
                {
                    founded = FindChild<T>(d, name);
                    if (founded != null) break;
                }
                else
                {
                    founded = childType;
                    break;
                }
            }
            return founded;
        }
    }
}
