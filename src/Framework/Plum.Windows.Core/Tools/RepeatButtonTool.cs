﻿using Plum.Windows.Enums;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Plum.Windows.Tools
{
    public class RepeatButtonTool
    {
        #region Ctor

        static RepeatButtonTool()
        {
            EventManager.RegisterClassHandler(typeof(RepeatButton), RepeatButton.MouseEnterEvent, new RoutedEventHandler(OnRepeatButtonMouseEnter));
            EventManager.RegisterClassHandler(typeof(RepeatButton), RepeatButton.MouseLeaveEvent, new RoutedEventHandler(OnRepeatButtonMouseLeave));
        }

        #endregion Ctor

        #region RepeatButtonStyle

        public static RepeatButtonStyle GetRepeatButtonStyle(DependencyObject obj)
        {
            return (RepeatButtonStyle)obj.GetValue(RepeatButtonStyleProperty);
        }

        public static void SetRepeatButtonStyle(DependencyObject obj, RepeatButtonStyle value)
        {
            obj.SetValue(RepeatButtonStyleProperty, value);
        }

        public static readonly DependencyProperty RepeatButtonStyleProperty =
            DependencyProperty.RegisterAttached("RepeatButtonStyle", typeof(RepeatButtonStyle), typeof(RepeatButtonTool), new PropertyMetadata(RepeatButtonStyle.Standard));

        #endregion RepeatButtonStyle

        #region ClickStyle

        public static ClickStyle GetClickStyle(DependencyObject obj)
        {
            return (ClickStyle)obj.GetValue(ClickStyleProperty);
        }

        public static void SetClickStyle(DependencyObject obj, ClickStyle value)
        {
            obj.SetValue(ClickStyleProperty, value);
        }

        public static readonly DependencyProperty ClickStyleProperty =
            DependencyProperty.RegisterAttached("ClickStyle", typeof(ClickStyle), typeof(RepeatButtonTool), new PropertyMetadata(ClickStyle.None));

        #endregion ClickStyle

        #region HoverBrush

        public static Brush GetHoverBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(HoverBrushProperty);
        }

        public static void SetHoverBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(HoverBrushProperty, value);
        }

        public static readonly DependencyProperty HoverBrushProperty =
            DependencyProperty.RegisterAttached("HoverBrush", typeof(Brush), typeof(RepeatButtonTool));

        #endregion HoverBrush

        #region CornerRadius

        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(RepeatButtonTool));

        #endregion CornerRadius

        #region ClickCoverOpacity

        public static double GetClickCoverOpacity(DependencyObject obj)
        {
            return (double)obj.GetValue(ClickCoverOpacityProperty);
        }

        public static void SetClickCoverOpacity(DependencyObject obj, double value)
        {
            obj.SetValue(ClickCoverOpacityProperty, value);
        }

        public static readonly DependencyProperty ClickCoverOpacityProperty =
            DependencyProperty.RegisterAttached("ClickCoverOpacity", typeof(double), typeof(RepeatButtonTool));

        #endregion ClickCoverOpacity

        #region IsWaiting

        public static bool GetIsWaiting(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsWaitingProperty);
        }

        public static void SetIsWaiting(DependencyObject obj, bool value)
        {
            obj.SetValue(IsWaitingProperty, value);
        }

        public static readonly DependencyProperty IsWaitingProperty =
            DependencyProperty.RegisterAttached("IsWaiting", typeof(bool), typeof(RepeatButtonTool));

        #endregion IsWaiting

        #region WaitingContent

        public static object GetWaitingContent(DependencyObject obj)
        {
            return obj.GetValue(WaitingContentProperty);
        }

        public static void SetWaitingContent(DependencyObject obj, object value)
        {
            obj.SetValue(WaitingContentProperty, value);
        }

        public static readonly DependencyProperty WaitingContentProperty =
            DependencyProperty.RegisterAttached("WaitingContent", typeof(object), typeof(RepeatButtonTool));

        #endregion WaitingContent

        #region Icon

        public static object GetIcon(DependencyObject obj)
        {
            return obj.GetValue(IconProperty);
        }

        public static void SetIcon(DependencyObject obj, object value)
        {
            obj.SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(object), typeof(RepeatButtonTool));

        #endregion Icon

        #region Event Handler

        private static void OnRepeatButtonMouseEnter(object sender, RoutedEventArgs e)
        {
            var repeatButton = sender as RepeatButton;
            var repeatButtonStyle = GetRepeatButtonStyle(repeatButton);
            var hoverBrush = GetHoverBrush(repeatButton);

            if (hoverBrush == null)
                return;

            var dic = new Dictionary<DependencyProperty, Brush>();
            switch (repeatButtonStyle)
            {
                case RepeatButtonStyle.Standard:
                    dic.Add(RepeatButton.BackgroundProperty, hoverBrush);
                    break;

                case RepeatButtonStyle.Hollow:
                    dic.Add(RepeatButton.BackgroundProperty, hoverBrush);
                    dic.Add(RepeatButton.ForegroundProperty, Brushes.White);
                    dic.Add(IconTool.ForegroundProperty, Brushes.White);
                    break;

                case RepeatButtonStyle.Outline:
                    dic.Add(RepeatButton.BorderBrushProperty, hoverBrush);
                    dic.Add(RepeatButton.ForegroundProperty, hoverBrush);
                    dic.Add(IconTool.ForegroundProperty, hoverBrush);
                    break;

                case RepeatButtonStyle.Link:
                    dic.Add(RepeatButton.ForegroundProperty, hoverBrush);
                    dic.Add(IconTool.ForegroundProperty, hoverBrush);
                    break;
            }
            StoryboardTool.BeginBrushStoryboard(repeatButton, dic);
        }

        private static void OnRepeatButtonMouseLeave(object sender, RoutedEventArgs e)
        {
            var repeatButton = sender as RepeatButton;
            var repeatButtonStyle = GetRepeatButtonStyle(repeatButton);
            var hoverBrush = GetHoverBrush(repeatButton);

            if (hoverBrush == null)
                return;

            var list = new List<DependencyProperty>();
            switch (repeatButtonStyle)
            {
                case RepeatButtonStyle.Standard:
                    list.Add(RepeatButton.BackgroundProperty);
                    break;

                case RepeatButtonStyle.Hollow:
                    list.Add(RepeatButton.BackgroundProperty);
                    list.Add(RepeatButton.ForegroundProperty);
                    list.Add(IconTool.ForegroundProperty);
                    break;

                case RepeatButtonStyle.Outline:
                    list.Add(RepeatButton.BorderBrushProperty);
                    list.Add(RepeatButton.ForegroundProperty);
                    list.Add(IconTool.ForegroundProperty);
                    break;

                case RepeatButtonStyle.Link:
                    list.Add(RepeatButton.ForegroundProperty);
                    list.Add(IconTool.ForegroundProperty);
                    break;
            }
            StoryboardTool.BeginBrushStoryboard(repeatButton, list);
        }

        #endregion Event Handler
    }
}