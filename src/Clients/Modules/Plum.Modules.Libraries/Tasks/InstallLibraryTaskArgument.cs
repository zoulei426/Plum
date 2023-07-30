using Plum.Modules.Libraries.Models;
using Plum.Tasks;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Modules.Libraries.Tasks
{
    [AddINotifyPropertyChangedInterface]
    public class InstallLibraryTaskArgument : TaskArgument
    {
        #region Properties

        public LibraryDvo Library { get; set; }

        public string Url { get; set; }

        public string SavePath { get; set; }

        public string InstallPath { get; set; }

        #endregion Properties
    }
}