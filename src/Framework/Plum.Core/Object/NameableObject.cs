using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Object
{
    [Serializable]
    public class NameableObject : IDObject, IComparable
    {
        public virtual string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(object obj)
        {
            NameableObject objName = obj as NameableObject;
            if (objName == null)
                return 0;

            return Name.CompareTo(objName.Name);
        }

        public override void Dispose()
        {
            base.Dispose();

            Name = null;
        }
    }
}