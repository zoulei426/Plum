using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Object
{
    [Serializable]
    public class IDObject : CDObject
    {
        public virtual Guid ID { get; set; }

        public IDObject()
        {
            ID = Guid.NewGuid();
        }
    }
}