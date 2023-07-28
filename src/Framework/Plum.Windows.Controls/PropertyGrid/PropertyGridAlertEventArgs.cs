using System;

namespace Plum.Windows.Controls
{
    [Serializable]
    public class PropertyGridAlertEventArgs : EventArgs
    {
        #region Properties

        public PropertyDescriptor PropertyDescriptor { get; set; }
        public eMessageGrade Grade { get; set; }
        public string Description { get; set; }
        public object UserState { get; set; }

        #endregion Properties

        #region Ctor

        public PropertyGridAlertEventArgs()
        {
            Grade = eMessageGrade.Infomation;
        }

        #endregion Ctor

        #region Methods

        public override string ToString()
        {
            return string.Format("{0}, {1}", Grade, Description);
        }

        #endregion Methods
    }
}