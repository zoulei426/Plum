using System;

namespace Plum.Windows.Controls
{
    public class InitializePropertyDescriptorEventArgs : EventArgs
    {
        #region Properties

        public PropertyDescriptor PropertyDescriptor { get; set; }
        public bool Cancel { get; set; }

        #endregion Properties

        #region Ctor

        public InitializePropertyDescriptorEventArgs(PropertyDescriptor property)
        {
            PropertyDescriptor = property;
            Cancel = true;
        }

        #endregion Ctor
    }
}