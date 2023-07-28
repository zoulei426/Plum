using System;

namespace Plum.Windows.Attributes
{
    public class ViewModelAttribute : Attribute
    {
        #region Properties

        public Type Type { get; private set; }

        #endregion Properties

        #region Ctor

        public ViewModelAttribute(Type typeModel)
        {
            Type = typeModel;
        }

        #endregion Ctor
    }
}