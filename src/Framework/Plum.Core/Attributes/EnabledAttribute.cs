using System;

namespace Plum.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class EnabledAttribute : Attribute
    {
        #region Properties

        public bool Value { get; set; }

        #endregion Properties

        #region Ctor

        public EnabledAttribute(bool value)
        {
            Value = value;
        }

        #endregion Ctor

        #region Methods

        #endregion Methods
    }
}