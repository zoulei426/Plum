namespace Plum.Windows.Controls
{
    public class PropertyDescriptorBuilder
    {
        #region Properties

        #endregion Properties

        #region Fields

        #endregion Fields

        #region Events

        #endregion Events

        #region Ctor

        public PropertyDescriptorBuilder()
        {
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        public void PostPropertyValueChanged(PropertyDescriptor pd, string propertyName)
        {
            OnPropertyValueChanged(pd, propertyName);
        }

        #endregion Methods - Public

        #region Methods - Protected

        public virtual PropertyDescriptor Build(PropertyDescriptor defaultValue)
        {
            return defaultValue;
        }

        public virtual void OnPropertyValueChanged(PropertyDescriptor pd, string propertyName)
        {
        }

        #endregion Methods - Protected

        #endregion Methods
    }
}