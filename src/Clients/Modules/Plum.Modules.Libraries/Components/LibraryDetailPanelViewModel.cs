using Plum.Modules.Libraries.Consts;
using Plum.Modules.Libraries.Models;
using Plum.Windows.Controls;
using Plum.Windows.Controls.Dialog;
using Plum.Windows.Mvvm;
using Prism.Ioc;
using Prism.Regions;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Modules.Libraries.Components
{
    [AddINotifyPropertyChangedInterface]
    public class LibraryDetailPanelViewModel : NavigableViewModel, IViewLoadedAndUnloadedAware<LibraryDetailPanel>
    {
        #region Properties

        public LibraryDvo SelectedItem { get; set; }

        #endregion Properties

        #region Fields

        private PropertyGrid pg;

        #endregion Fields

        #region Ctor

        public LibraryDetailPanelViewModel(IContainerExtension container) : base(container)
        {
        }

        #endregion Ctor

        #region Methods

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            SelectedItem = NavParams.GetValue<LibraryDvo>(NavParamKeys.CURRENT_LIBRARY);

            InitializeApis();
        }

        public void OnLoaded(LibraryDetailPanel view)
        {
            pg = view.FindName("pg") as PropertyGrid;

            if (pg is not null)
            {
                pg.InitializeBegin += Pg_InitializeBegin;
                pg.InitializeEnd += Pg_InitializeEnd;
            }
        }

        public void OnUnloaded(LibraryDetailPanel view)
        {
            SelectedItem = null;
        }

        private void Pg_InitializeEnd(object sender, EventArgs e)
        {
            IsBusy = false;
        }

        private void Pg_InitializeBegin(object sender, EventArgs e)
        {
            IsBusy = true;
        }

        private void InitializeApis()
        {
            if (SelectedItem.Status < Entities.LibraryStatus.Installed)
            {
                return;
            }

            var path = Path.Combine(SelectedItem.LocalPath, SelectedItem.LocalVersion);
            var dirInfo = new DirectoryInfo(path);
            dirInfo.GetFiles().ForEach(file =>
            {
                if (file.Extension.Equals(".dll", StringComparison.OrdinalIgnoreCase)
                || file.Extension.Equals(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    var assembly1 = Assembly.LoadFrom(file.FullName);
                    var assembly2 = Assembly.LoadFile(file.FullName);
                }
            });
        }

        #endregion Methods
    }
}