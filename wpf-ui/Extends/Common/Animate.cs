using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace wpf_ui.Extends.Common
{
    /// <summary>
    /// 动画辅助类
    /// </summary>
    public static class Animate
    {
        /// <summary>
        /// 默认动画时间
        /// </summary>
        private const double AnimateTime = 0.15d;

        /// <summary>
        /// 动画缓动类型
        /// </summary>
        private static readonly EasingFunctionBase EasingFunction = new PowerEase
        {
            EasingMode = EasingMode.EaseInOut,
            //Power = 10
        };

        /// <summary>
        /// 动画完成回调函数
        /// </summary>
        public delegate void AnimateCompleted();

        /// <summary>
        /// 淡入
        /// </summary>
        /// <param name="uiElement">主控件</param>
        /// <param name="time">动画时间</param>
        /// <param name="animateCompleted">回调</param>
        public static void FadeIn(this UIElement uiElement, double time = AnimateTime, AnimateCompleted animateCompleted = null)
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = 0d,
                To = 1d,
                Duration = TimeSpan.FromSeconds(time),
                EasingFunction = EasingFunction
            };
            da.Completed += delegate { animateCompleted?.Invoke(); };
            uiElement.BeginAnimation(UIElement.OpacityProperty, da);
        }

        /// <summary>
        /// 淡出
        /// </summary>
        /// <param name="uiElement">主控件</param>
        /// <param name="time">动画时间</param>
        /// <param name="animateCompleted">回调</param>
        public static void FadeOut(this UIElement uiElement, double time = AnimateTime, AnimateCompleted animateCompleted = null)
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = 1d,
                To = 0d,
                Duration = TimeSpan.FromSeconds(time),
                EasingFunction = EasingFunction
            };
            da.Completed += delegate { animateCompleted?.Invoke(); };
            uiElement.BeginAnimation(UIElement.OpacityProperty, da);
        }

        /// <summary>
        /// 缩放进入
        /// </summary>
        /// <param name="uiElement">主控件</param>
        /// <param name="time">动画时间</param>
        /// <param name="animateCompleted">回调</param>
        public static void ScaleIn(this UIElement uiElement, double time = AnimateTime, AnimateCompleted animateCompleted = null)
        {
            if (!(uiElement.RenderTransform is ScaleTransform scaleTransform)) return;
            DoubleAnimation da = new DoubleAnimation
            {
                From = 0.7d,
                To = 1d,
                Duration = TimeSpan.FromSeconds(time),
                EasingFunction = EasingFunction
            };
            da.Completed += delegate { animateCompleted?.Invoke(); };
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, da);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, da);
        }

        /// <summary>
        /// 缩放退出
        /// </summary>
        /// <param name="uiElement">主控件</param>
        /// <param name="time">动画时间</param>
        /// <param name="animateCompleted">回调</param>
        public static void ScaleOut(this UIElement uiElement, double time = AnimateTime, AnimateCompleted animateCompleted = null)
        {
            if (!(uiElement.RenderTransform is ScaleTransform scaleTransform)) return;
            DoubleAnimation da = new DoubleAnimation
            {
                From = 1d,
                To = 0.7d,
                Duration = TimeSpan.FromSeconds(time),
                EasingFunction = EasingFunction
            };
            da.Completed += delegate { animateCompleted?.Invoke(); };
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, da);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, da);
        }

        /// <summary>
        /// 旋转动画
        /// </summary>
        /// <param name="uiElement">主控件</param>
        /// <param name="time">动画时间</param>
        public static void Rotate(this UIElement uiElement, double time = AnimateTime)
        {
            if (!(uiElement.RenderTransform is RotateTransform rotateTransform)) return;
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, new DoubleAnimation
            {
                From = 0d,
                To = 360d,
                Duration = TimeSpan.FromSeconds(time),
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = EasingFunction
            });
        }

        /// <summary>
        /// 平移动画
        /// </summary>
        /// <param name="frameworkElement">主控件</param>
        /// <param name="thickness">移动之后的位置(开始默认为控件本身的Margin)</param>
        /// <param name="time">动画时间</param>
        /// <param name="animateCompleted">回调</param>
        /// <param name="isReverse">是否反向移动(从指定位置移动至原始位置)</param>
        public static void Translation(this FrameworkElement frameworkElement, Thickness thickness, double time = AnimateTime,
                                            AnimateCompleted animateCompleted = null, bool isReverse = false)
        {
            ThicknessAnimation ta = new ThicknessAnimation
            {
                From = frameworkElement.Margin,
                To = thickness,
                Duration = TimeSpan.FromSeconds(time),
                EasingFunction = EasingFunction
            };
            if (isReverse)
            {
                ta.From = thickness;
                ta.To = frameworkElement.Margin;
            }
            ta.Completed += delegate { animateCompleted?.Invoke(); };
            frameworkElement.BeginAnimation(FrameworkElement.MarginProperty, ta);
        }
    }
}
