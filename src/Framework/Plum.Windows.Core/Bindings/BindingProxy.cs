using System.Windows;

namespace Plum.Windows.Bindings
{
    public class BindingProxy : Freezable
    {
        #region Properties

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        #endregion Properties

        #region Methods

        #region Methods - Override

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        #endregion Methods - Override

        #endregion Methods
    }
}