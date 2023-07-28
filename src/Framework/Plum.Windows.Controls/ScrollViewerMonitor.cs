using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Plum.Windows.Controls
{
    /// <summary>
    /// 滚动条监听
    /// </summary>
    public class ScrollViewerMonitor
    {
        /// <summary>
        /// 滚动条到底触发命令
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static ICommand GetAtEndCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(AtEndCommandProperty);
        }

        /// <summary>
        /// Sets at end command.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetAtEndCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(AtEndCommandProperty, value);
        }

        /// <summary>
        /// At end command property
        /// </summary>
        public static readonly DependencyProperty AtEndCommandProperty =
            DependencyProperty.RegisterAttached("AtEndCommand", typeof(ICommand),
                typeof(ScrollViewerMonitor), new PropertyMetadata(OnAtEndCommandChanged));

        /// <summary>
        /// Called when [at end command changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void OnAtEndCommandChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)d;
            if (element != null)
            {
                element.Loaded -= element_Loaded;
                element.Loaded += element_Loaded;
            }
        }

        public static object GetAtEndCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(AtEndCommandParameterProperty);
        }

        /// <summary>
        /// Sets at end command.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetAtEndCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(AtEndCommandParameterProperty, value);
        }

        public static readonly DependencyProperty AtEndCommandParameterProperty =
            DependencyProperty.RegisterAttached("AtEndCommandParameter", typeof(object), typeof(ScrollViewerMonitor), new PropertyMetadata(0));

        private static void element_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;

            element.Loaded -= element_Loaded;

            ScrollViewer scrollViewer = FindChildOfType<ScrollViewer>(element);

            if (scrollViewer == null)
            {
                if ((scrollViewer = (ScrollViewer)element) is null)
                    return;
                //throw new InvalidOperationException("ScrollViewer not found.");
            }

            scrollViewer.ScrollChanged += delegate
            {
                bool atBottom = scrollViewer.VerticalOffset
                                 >= scrollViewer.ScrollableHeight;

                if (atBottom)
                {
                    var atEnd = GetAtEndCommand(element);
                    if (atEnd != null)
                    {
                        atEnd.Execute(GetAtEndCommandParameter(element));
                    }
                }
            };
        }

        private static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }
    }
}