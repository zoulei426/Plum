using Plum.Attributes;
using Plum.Object;
using System;

namespace Plum.Tasks
{
    [Serializable]
    public class TaskArgument : BindableObject
    {
        [Enabled(false)]
        public object UserState { get; set; }

        [Enabled(false)]
        public KeyValueList<string, object> Properties { get; set; }

        [Enabled(false)]
        public virtual string Error { get { return _Error; } }

        private string _Error;
    }
}