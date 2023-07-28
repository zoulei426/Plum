using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace Plum.Windows.Controls
{
    public class ColorRegion : ContentControl
    {
        static ColorRegion()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorRegion), new FrameworkPropertyMetadata(typeof(ColorRegion)));
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
          nameof(Mode), typeof(ColorZoneMode), typeof(ColorRegion), new PropertyMetadata(default(ColorZoneMode)));

        public ColorZoneMode Mode
        {
            get => (ColorZoneMode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(CornerRadius), typeof(ColorRegion), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}