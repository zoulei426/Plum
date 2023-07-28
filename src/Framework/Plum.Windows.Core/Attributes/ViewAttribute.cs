using System;

namespace Plum.Windows.Attributes
{
    public class ViewAttribute : Attribute
    {
        #region Properties

        public Type Type { get; private set; }

        #endregion Properties

        #region Ctor

        public ViewAttribute(Type typeView)
        {
            Type = typeView;
        }

        #endregion Ctor
    }
}