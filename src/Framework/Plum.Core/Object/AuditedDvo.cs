using PropertyChanged;
using System;

namespace Plum.Object
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class AuditedDvo<TKey> : DataViewObject<TKey>
    {
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifierId { get; set; }
    }
}