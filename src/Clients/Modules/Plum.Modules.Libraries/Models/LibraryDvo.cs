using Plum.Object;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Modules.Libraries.Models
{
    [AddINotifyPropertyChangedInterface]
    public class LibraryDvo : DataViewObject
    {
        public string Name { get; set; }
    }
}