using Plum.Object;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace Plum.Windows.Objects
{
    public class BindableDependencyObject : DependencyObject, ICloneable, IDisposable, INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        public virtual object Clone()
        {
            object newObj = MemberwiseClone();

            newObj.TraversalPropertiesInfo(ClonePropertyHandler, newObj);

            return newObj;
        }

        public virtual void Dispose()
        {
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            var evt = PropertyChanged;
            if (evt == null)
                return;

            evt(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyPropertyChanged<T>(System.Linq.Expressions.Expression<Func<T>> lambda)
        {
            LambdaPropertyNotifier.NotifyPropertyChanged(
                lambda, name => NotifyPropertyChanged(name));
        }

        private bool ClonePropertyHandler(PropertyInfo pi, object value, object target)
        {
            if (!pi.CanWrite)
                return true;

            pi.SetValue(target, ObjectBase.TryClone(value), null);

            return true;
        }

        private bool DisposePropertyHandler(string name, object value)
        {
            IDisposable id = value as IDisposable;
            if (id == null)
                return true;

            id.Dispose();

            return true;
        }
    }
}