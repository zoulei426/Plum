using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Plum.Object
{
    [Serializable()]
    public class ObservableKeyValueList<TKey, TValue> : ObservableCollection<KeyValue<TKey, TValue>>
    {
        #region Properties

        public virtual TValue this[TKey key]
        {
            get { return GetValue(key); }
            set { SetValue(key, value); }
        }

        #endregion Properties

        #region Ctor

        public ObservableKeyValueList()
        {
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        public bool ContainsKey(TKey key)
        {
            return this.Any(c => c.Key.Equals(key));
        }

        public void Remove(TKey key)
        {
            for (int i = 0; i < this.Count; i++)
            {
                var item = this[i];
                if (item.Key.Equals(key))
                {
                    this.RemoveAt(i);
                    break;
                }
            }
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