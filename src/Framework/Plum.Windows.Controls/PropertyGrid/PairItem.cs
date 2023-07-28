using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Plum.Windows.Controls
{
    public class PairItem : ContentControl
    {
        static PairItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PairItem), new FrameworkPropertyMetadata(typeof(PairItem)));
        }

        #region Properties

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(PairItem), new PropertyMetadata((s, a) =>
            {
            }));

        public bool IsRequired
        {
            get { return (bool)GetValue(IsRequiredProperty); }
            set { SetValue(IsRequiredProperty, value); }
        }

        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.Register("IsRequired", typeof(bool), typeof(PairItem));

        public double HeaderSpacing
        {
            get { return (double)GetValue(HeaderSpacingProperty); }
            set { SetValue(HeaderSpacingProperty, value); }
        }

        public static readonly DependencyProperty HeaderSpacingProperty =
            DependencyProperty.Register("HeaderSpacing", typeof(double), typeof(PairItem), new PropertyMetadata(0.0));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(PairItem));

        public Brush StarForeground
        {
            get { return (Brush)GetValue(StarForegroundProperty); }
            set { SetValue(StarForegroundProperty, value); }
        }

        public static readonly DependencyProperty StarForegroundProperty =
            DependencyProperty.Register("StarForeground", typeof(Brush), typeof(PairItem));

        public double StarSize
        {
            get { return (double)GetValue(StarSizeProperty); }
            set { SetValue(StarSizeProperty, value); }
        }

        public static readonly DependencyProperty StarSizeProperty =
            DependencyProperty.Register("StarSize", typeof(double), typeof(PairItem), new PropertyMetadata(20.0));

        public HorizontalAlignment StarAlignment
        {
            get { return (HorizontalAlignment)GetValue(StarAlignmentProperty); }
            set { SetValue(StarAlignmentProperty, value); }
        }

        public static readonly DependencyProperty StarAlignmentProperty =
            DependencyProperty.Register("StarAlignment", typeof(HorizontalAlignment), typeof(PairItem), new PropertyMetadata(HorizontalAlignment.Right));

        #endregion Properties

        #region Fields

        #endregion Fields

        #region Events

        #endregion Events

        #region Ctor

        public PairItem()
        {
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        #endregion Methods - Public

        #region Methods - Override

        #endregion Methods - Override

        #region Methods - Events

        #endregion Methods - Events

        #region Methods - Private

        #endregion Methods - Private

        #endregion Methods
    }
}