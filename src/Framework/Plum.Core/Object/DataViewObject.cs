using PropertyChanged;
using System;

namespace Plum.Object
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class DataViewObject<TKey> : BindableObject
    {
        //[Enabled(false)]
        public TKey Id { get; set; }
    }

    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class DataViewObject : BindableObject
    {
    }
}