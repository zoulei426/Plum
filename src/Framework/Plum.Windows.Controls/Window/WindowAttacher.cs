using System.Windows;

namespace Plum.Windows.Controls
{
    public class WindowAttacher : DependencyObject
    {
        #region Properties - IsDarkTheme

        public static readonly DependencyProperty IsDarkThemeProperty =
            DependencyProperty.RegisterAttached("IsDarkTheme", typeof(bool), typeof(WindowAttacher), new PropertyMetadata(false, OnChanged));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public static bool GetIsDarkTheme(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDarkThemeProperty);
        }

        public static void SetIsDarkTheme(DependencyObject obj, bool pt)
        {
            obj.SetValue(IsDarkThemeProperty, pt);
        }

        #endregion Properties - IsDarkTheme
    }
}