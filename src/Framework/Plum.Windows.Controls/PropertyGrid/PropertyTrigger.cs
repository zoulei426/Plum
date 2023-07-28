using System;
using System.Windows.Media.Imaging;

namespace Plum.Windows.Controls
{
    public class PropertyTrigger
    {
        #region Properties

        #endregion Properties

        #region Fields

        #endregion Fields

        #region Events

        #endregion Events

        #region Ctor

        public PropertyTrigger()
        {
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        internal void PostPropertyValueChanged(PropertyDescriptor pd, string propertyName)
        {
            OnPropertyValueChanged(pd, propertyName);
        }

        internal void PostPropertyValueInstalled(PropertyDescriptor pd, string propertyName)
        {
            OnPropertyValueInstalled(pd, propertyName);
        }

        public virtual void OnPropertyValueChanged(PropertyDescriptor pd, string propertyName)
        {
        }

        public virtual void OnPropertyValueInstalled(PropertyDescriptor pd, string propertyName)
        {
        }

        #endregion Methods - Public

        #region Methods - Protected

        protected void RaiseAlert(PropertyDescriptor pd, eMessageGrade grade, string description)
        {
            pd.Designer.Dispatcher.Invoke(new Action(() =>
            {
                pd.Grade = grade;
                pd.DescriptionState = description;

                switch (grade)
                {
                    case eMessageGrade.Warn:
                        pd.ImageState = BitmapFrame.Create(new Uri(""));
                        break;

                    case eMessageGrade.Error:
                    case eMessageGrade.Exception:
                        pd.ImageState = BitmapFrame.Create(new Uri(""));
                        break;

                    case eMessageGrade.Infomation:
                    default:
                        pd.ImageState = BitmapFrame.Create(new Uri(""));
                        break;
                }

                pd.PropertyGrid.RaiseAlert(new PropertyGridAlertEventArgs()
                {
                    PropertyDescriptor = pd,
                    Grade = grade,
                    Description = description
                });
            }));
        }

        #endregion Methods - Protected

        #endregion Methods
    }
}