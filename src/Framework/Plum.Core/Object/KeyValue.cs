using System;
using System.Runtime.Serialization;

namespace Plum.Object
{
    [Serializable]
    [DataContract]
    public class KeyValue<TKey, TValue> : ObjectBase
    {
        #region Properties

        [DataMember]
        public TKey Key { get; set; }

        [DataMember]
        public TValue Value { get; set; }

        #endregion Properties

        #region Ctor

        public KeyValue()
        {
        }

        public KeyValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Override

        public override string ToString()
        {
            return string.Format("{0} : {1}", Key, Value);
        }

        #endregion Methods - Override

        #endregion Methods
    }
}