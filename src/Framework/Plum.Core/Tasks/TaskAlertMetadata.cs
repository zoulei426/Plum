using Plum.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Tasks
{
    public class TaskAlertMetadata : NameableObject
    {
        #region Properties

        public int CountValue { get; set; }

        #endregion Properties

        #region Ctor

        public TaskAlertMetadata()
        {
            CountValue = 1;
        }

        #endregion Ctor
    }
}