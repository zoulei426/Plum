using System;
using System.Collections.Generic;
using System.Linq;

namespace Plum.Object
{
    [Serializable()]
    public class KeyValueList<TKey, TValue> : List<KeyValue<TKey, TValue>>
    {
        #region Properties

        public virtual TValue this[TKey key]
        {
            get { return GetValue(key); }
            set { SetValue(key, value); }
        }

        #endregion Properties

        #region Ctor

        public KeyValueList()
        {
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        //public void Add(TKey key, TValue value)
        //{
        //    Add(new KeyValue<TKey, TValue>(key, value));
        //}

        public bool ContainsKey(TKey key)
        {
            return this.Any(c => c.Key.Equals(key));
        }

        public void Remove(TKey key)
        {
            var index = this.FindIndex(c => c.Key.Equals(key));
            if (index >= 0)
                this.RemoveAt(index);
        }

        #endregion Methods - Public

        #region Methods - Private

        private TValue GetValue(TKey key)
        {
            var pair = this.Where(c => c.Key.Equals(key)).FirstOrDefault();
            if (pair == null)
            {
                pair = new KeyValue<TKey, TValue>() { Key = key };
                Add(pair);
            }

            return pair.Value;
        }

        private void SetValue(TKey key, TValue value)
        {
            var pair = this.Where(c => c.Key.Equals(key)).FirstOrDefault();
            if (pair == null)
            {
                pair = new KeyValue<TKey, TValue>() { Key = key };
                Add(pair);
            }

            pair.Value = value;
        }

        #endregion Methods - Private

        #endregion Methods
    }
}