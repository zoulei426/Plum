using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Plum.Windows.Controls
{
    public class DataGridAttacher
    {
        #region Properties - CellPadding

        public static readonly DependencyProperty CellPaddingProperty =
            DependencyProperty.RegisterAttached("CellPadding", typeof(Thickness), typeof(DataGridAttacher), new PropertyMetadata(new Thickness(0)));

        public static Thickness GetCellPadding(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(CellPaddingProperty);
        }

        public static void SetCellPadding(DependencyObject obj, Thickness pt)
        {
            obj.SetValue(CellPaddingProperty, pt);
        }

        #endregion Properties - CellPadding

        #region Properties - RowHeaderPadding

        public static readonly DependencyProperty RowHeaderPaddingProperty =
            DependencyProperty.RegisterAttached("RowHeaderPadding", typeof(Thickness), typeof(DataGridAttacher), new PropertyMetadata(new Thickness(4)));

        public static Thickness GetRowHeaderPadding(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(RowHeaderPaddingProperty);
        }

        public static void SetRowHeaderPadding(DependencyObject obj, Thickness pt)
        {
            obj.SetValue(RowHeaderPaddingProperty, pt);
        }

        #endregion Properties - RowHeaderPadding

        #region Properties - ColumnHeaderPadding

        public static readonly DependencyProperty ColumnHeaderPaddingProperty =
            DependencyProperty.RegisterAttached("ColumnHeaderPadding", typeof(Thickness), typeof(DataGridAttacher), new PropertyMetadata(new Thickness(5, 3, 5, 3)));

        public static Thickness GetColumnHeaderPadding(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(ColumnHeaderPaddingProperty);
        }

        public static void SetColumnHeaderPadding(DependencyObject obj, Thickness pt)
        {
            obj.SetValue(ColumnHeaderPaddingProperty, pt);
        }

        #endregion Properties - ColumnHeaderPadding

        #region Properties - ContentBorderThickness

        public static readonly DependencyProperty ContentBorderThicknessProperty =
            DependencyProperty.RegisterAttached("ContentBorderThickness", typeof(Thickness), typeof(DataGridAttacher), new PropertyMetadata(new Thickness(0, 0, 0, 0)));

        public static Thickness GetContentBorderThickness(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(ContentBorderThicknessProperty);
        }

        public static void SetContentBorderThickness(DependencyObject obj, Thickness pt)
        {
            obj.SetValue(ContentBorderThicknessProperty, pt);
        }

        #endregion Properties - ContentBorderThickness

        #region Properties - VerticalScrollBarMargin

        public static readonly DependencyProperty VerticalScrollBarMarginProperty =
            DependencyProperty.RegisterAttached("VerticalScrollBarMargin", typeof(Thickness), typeof(DataGridAttacher), new PropertyMetadata(new Thickness(0, 0, 0, 0)));

        public static Thickness GetVerticalScrollBarMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(VerticalScrollBarMarginProperty);
        }

        public static void SetVerticalScrollBarMargin(DependencyObject obj, Thickness pt)
        {
            obj.SetValue(VerticalScrollBarMarginProperty, pt);
        }

        #endregion Properties - VerticalScrollBarMargin

        #region Properties - HeaderBackgroundDefault

        public static readonly DependencyProperty HeaderBackgroundDefaultProperty =
            DependencyProperty.RegisterAttached("HeaderBackgroundDefault", typeof(Brush), typeof(DataGridAttacher));

        public static Brush GetHeaderBackgroundDefault(DependencyObject obj)
        {
            return (Brush)obj.GetValue(HeaderBackgroundDefaultProperty);
        }

        public static void SetHeaderBackgroundDefault(DependencyObject obj, Brush pt)
        {
            obj.SetValue(HeaderBackgroundDefaultProperty, pt);
        }

        #endregion Properties - HeaderBackgroundDefault

        #region Properties - AlternatingRowBackground

        public static readonly DependencyProperty AlternatingRowBackgroundProperty =
            DependencyProperty.RegisterAttached("AlternatingRowBackground", typeof(Brush), typeof(DataGridAttacher));

        public static Brush GetAlternatingRowBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(AlternatingRowBackgroundProperty);
        }

        public static void SetAlternatingRowBackground(DependencyObject obj, Brush pt)
        {
            obj.SetValue(AlternatingRowBackgroundProperty, pt);
        }

        #endregion Properties - AlternatingRowBackground

        #region Properties - ColumnHeaderThumbWidth

        public static readonly DependencyProperty ColumnHeaderThumbWidthProperty =
            DependencyProperty.RegisterAttached("ColumnHeaderThumbWidth", typeof(int), typeof(DataGridAttacher));

        public static int GetColumnHeaderThumbWidth(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnHeaderThumbWidthProperty);
        }

        public static void SetColumnHeaderThumbWidth(DependencyObject obj, int pt)
        {
            obj.SetValue(ColumnHeaderThumbWidthProperty, pt);
        }

        #endregion Properties - ColumnHeaderThumbWidth

        #region Properties - ColumnHeaderFontWeight

        public static readonly DependencyProperty ColumnHeaderFontWeightProperty =
            DependencyProperty.RegisterAttached("ColumnHeaderFontWeight", typeof(FontWeight), typeof(DataGridAttacher), new PropertyMetadata(FontWeights.Regular));

        public static int GetColumnHeaderFontWeight(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnHeaderFontWeightProperty);
        }

        public static void SetColumnHeaderFontWeight(DependencyObject obj, FontWeight pt)
        {
            obj.SetValue(ColumnHeaderFontWeightProperty, pt);
        }

        #endregion Properties - ColumnHeaderFontWeight

        #region Properties - ColumnHeaderHorizontalContentAlignment

        public static readonly DependencyProperty ColumnHeaderHorizontalContentAlignmentProperty =
            DependencyProperty.RegisterAttached("ColumnHeaderHorizontalContentAlignment", typeof(HorizontalAlignment), typeof(DataGridAttacher), new PropertyMetadata(HorizontalAlignment.Left));

        public static int GetColumnHeaderHorizontalContentAlignment(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnHeaderHorizontalContentAlignmentProperty);
        }

        public static void SetColumnHeaderHorizontalContentAlignment(DependencyObject obj, HorizontalAlignment pt)
        {
            obj.SetValue(ColumnHeaderHorizontalContentAlignmentProperty, pt);
        }

        #endregion Properties - ColumnHeaderHorizontalContentAlignment

        #region Properties - UseScrollViewerTemplate

        public static readonly DependencyProperty UseScrollViewerTemplateProperty =
            DependencyProperty.RegisterAttached("UseScrollViewerTemplate", typeof(bool), typeof(DataGridAttacher), new PropertyMetadata(true));

        public static bool GetUseScrollViewerTemplate(DependencyObject obj)
        {
            return (bool)obj.GetValue(UseScrollViewerTemplateProperty);
        }

        public static void SetUseScrollViewerTemplate(DependencyObject obj, bool pt)
        {
            obj.SetValue(UseScrollViewerTemplateProperty, pt);
        }

        #endregion Properties - UseScrollViewerTemplate

        #region Properties - CanUseFilter

        public static readonly DependencyProperty CanUseFilterProperty =
            DependencyProperty.RegisterAttached("CanUseFilter", typeof(bool), typeof(DataGridAttacher), new PropertyMetadata(false));

        public static bool GetCanUseFilter(DependencyObject obj)
        {
            return (bool)obj.GetValue(CanUseFilterProperty);
        }

        public static void SetCanUseFilter(DependencyObject obj, bool pt)
        {
            obj.SetValue(CanUseFilterProperty, pt);
        }

        #endregion Properties - CanUseFilter

        #region Properties - FilterExtensions

        public static readonly DependencyProperty FilterExtensionsProperty =
            DependencyProperty.RegisterAttached("FilterExtensions", typeof(StackPanel), typeof(DataGridAttacher), new PropertyMetadata(null));

        public static StackPanel GetFilterExtensions(DependencyObject obj)
        {
            return (StackPanel)obj.GetValue(FilterExtensionsProperty);
        }

        public static void SetFilterExtensions(DependencyObject obj, StackPanel pt)
        {
            obj.SetValue(FilterExtensionsProperty, pt);
        }

        #endregion Properties - FilterExtensions
    }
}