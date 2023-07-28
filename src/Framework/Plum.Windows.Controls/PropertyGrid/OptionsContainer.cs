using System.Windows;
using System.Windows.Controls;

namespace Plum.Windows.Controls
{
    public partial class OptionsContainer : ContentControl
    {
        static OptionsContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OptionsContainer), new FrameworkPropertyMetadata(typeof(OptionsContainer)));
        }

        #region Properties

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(OptionsContainer));

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(OptionsContainer), new UIPropertyMetadata((s, a) =>
            {
                var item = s as OptionsContainer;
                item.OnChildChanged(a.OldValue, a.NewValue);
            }));

        #endregion Properties

        #region Ctor

        public OptionsContainer()
        {
        }

        #endregion Ctor

        #region Methods

        private void OnChildChanged(object oldValue, object newValue)
        {
            base.RemoveLogicalChild(oldValue);
            base.AddLogicalChild(newValue);
        }

        #endregion Methods
    }
}