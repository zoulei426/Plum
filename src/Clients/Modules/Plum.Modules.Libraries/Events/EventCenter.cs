using Plum.Modules.Libraries.Entities;
using Plum.Modules.Libraries.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Modules.Libraries.Events
{
    public class EventCenter
    {
        public class SelectedLibraryChangedEvent : PubSubEvent<DynamicLinkLibrary>
        {
        }

        public class RefreshLibraryEvent : PubSubEvent
        {
        }
    }
}