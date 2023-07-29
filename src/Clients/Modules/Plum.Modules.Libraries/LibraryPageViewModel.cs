using Plum.Modules.Libraries.Components;
using Plum.Modules.Libraries.Consts;
using Plum.Windows.Mvvm;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Modules.Libraries
{
    public class LibraryPageViewModel : ViewModelBase, IViewLoadedAndUnloadedAware
    {
        #region Commands

        #endregion Commands

        public LibraryPageViewModel(IContainerExtension container) : base(container)
        {
        }

        protected override void RegisterCommands()
        {
        }

        public void OnLoaded()
        {
            RegionManager.RegisterViewWithRegion(RegionNames.LIBRARY_TOOLBAR, typeof(LibraryToolBar));
            RegionManager.RegisterViewWithRegion(RegionNames.LIBRARY_MAIN_CONTENT, typeof(LibraryListPanel));
        }

        public void OnUnloaded()
        {
        }
    }
}