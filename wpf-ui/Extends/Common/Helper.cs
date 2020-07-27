using System;
using System.Reflection;
using System.Windows;
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
            if (!(d is T childType) || (d is FrameworkElement element) && !string.IsNullOrWhiteSpace(name) && element.Name != name)
            {
                return FindParent<T>(d, name);
            }
            return childType;
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
                if (!(d is T childType) || d is FrameworkElement element && !string.IsNullOrWhiteSpace(name) && element.Name != name)
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

        /// <summary>
        /// 浅拷贝当前对象
        /// </summary>
        /// <typeparam name="TA">目标对象类型</typeparam>
        /// <typeparam name="TB">源对象类型</typeparam>
        /// <param name="b">源对象</param>
        /// <returns></returns>
        public static TA ShallowCopy<TA, TB>(this TB b)
        {
            TA a = Activator.CreateInstance<TA>();
            Type typeb = b.GetType();//获得类型  
            Type typea = typeof(TA);
            foreach (PropertyInfo sp in typeb.GetProperties())//获得类型的属性字段  
            {
                foreach (PropertyInfo ap in typea.GetProperties())
                {
                    if (ap.Name == sp.Name)//判断属性名是否相同  
                    {
                        try
                        {
                            ap.SetValue(a, sp.GetValue(b, null), null); //获得b对象属性的值复制给a对象的属性  
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }
            }
            return a;
        }
    }
}
