using Plum.Modules.Libraries.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Modules.Libraries.Events
{
    internal class EventCenter
    {
        internal class SelectedLibraryChangedEvent : PubSubEvent<LibraryDvo>
        {
        }

        internal class RefreshLibraryEvent : PubSubEvent
        {
        }
    }
}