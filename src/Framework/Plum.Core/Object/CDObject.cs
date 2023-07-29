using System;
using System.Collections;
using System.Reflection;

namespace Plum.Object
{
    public interface ICD : ICloneable, IDisposable
    {
    }

    /// <summary>
    /// 核心类
    /// </summary>
    [Serializable]
    public class CDObject : ICD
    {
        #region Methods

        #region Methods - Static

        /// <summary>
        /// TryClone
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object TryClone(object obj)
        {
            if (obj is ICloneable ic)
                return ic?.Clone();

            if (obj is IList list)
                return list?.Clone();

            return obj;
        }

        #endregion Methods - Static

        #region Methods - Virtual

        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            object newObj = MemberwiseClone();
            newObj.TraversalPropertiesInfo(ClonePropertyHandler, newObj);
            return newObj;
        }

        /// <summary>
        /// 回收
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private bool ClonePropertyHandler(PropertyInfo pi, object value, object target)
        {
            if (!pi.CanWrite)
                return true;

            pi.SetValue(target, CDObject.TryClone(value), null);

            return true;
        }

        #endregion Methods - Virtual

        #endregion Methods
    }
}