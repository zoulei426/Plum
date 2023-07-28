using System;
using System.Linq.Expressions;

namespace Plum
{
    public class LambdaPropertyNotifier
    {
        #region Methods

        public static void NotifyPropertyChanged<T>(Expression<Func<T>> lambda, Action<string> notifyCallback)
        {
            notifyCallback?.Invoke((lambda.Body as MemberExpression).Member.Name);
        }

        #endregion Methods
    }
}