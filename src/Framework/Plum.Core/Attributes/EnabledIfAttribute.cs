using System;

namespace Plum.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class EnabledIfAttribute : Attribute
    {
        #region Properties

        //public bool Value { get; set; }

        public string PropertyName { get; set; }

        public object Value { get; set; }

        #endregion Properties

        #region Ctor

        public EnabledIfAttribute(string propertyName, object value)
        {
            PropertyName = propertyName;
            Value = value;
        }

        #endregion Ctor

        #region Methods

        #endregion Methods
    }
}