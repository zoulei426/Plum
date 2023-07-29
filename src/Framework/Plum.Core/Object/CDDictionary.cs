using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Object
{
    [Serializable]
    public class CDDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ICD
    {
        #region Methods

        public virtual object Clone()
        {
            Dictionary<TKey, TValue> list = Activator.CreateInstance(this.GetType()) as Dictionary<TKey, TValue>;

            foreach (KeyValuePair<TKey, TValue> pair in this)
                list.Add(pair.Key, (TValue)CDObject.TryClone(pair.Value));

            return list;
        }

        public virtual void Dispose()
        {
        }

        #endregion Methods
    }
}