using System;

namespace Plum.Windows.Controls
{
    public class GridDescriptorAttribute : Attribute
    {
        #region Properties

        public int Column { get; set; }

        #endregion Properties

        #region Ctor

        public GridDescriptorAttribute()
        {
            Column = 2;
        }

        #endregion Ctor

        #region Methods

        #endregion Methods
    }
}