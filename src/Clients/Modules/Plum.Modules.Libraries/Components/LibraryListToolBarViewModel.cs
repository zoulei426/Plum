using Plum.Modules.Libraries.Consts;
using Plum.Modules.Libraries.Models;
using Plum.Windows.Mvvm;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plum.Modules.Libraries.Events.EventCenter;
using System.Windows.Input;
using PropertyChanged;
using Plum.Windows.Commands;

namespace Plum.Modules.Libraries.Components
{
    [AddINotifyPropertyChangedInterface]
    public class LibraryListToolBarViewModel : SelectedItemViewModel<LibraryDvo, SelectedLibraryChangedEvent, RefreshLibraryEvent>
    {
        #region Commands

        public ICommand DetailCommand { get; set; }

        #endregion Commands

        #region Ctor

        public LibraryListToolBarViewModel(IContainerExtension container) : base(container)
        {
        }

        #endregion Ctor

        #region Methods

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            DetailCommand = new RelayCommand(ExecuteDetail, CanDetail);
        }

        private bool CanDetail()
        {
            return SelectedItem is not null;
        }

        private void ExecuteDetail()
        {
            //await ShowObjectDialog($"动态库详情", SelectedItem, async args =>
            //{
            //    if (args.Result == ButtonResult.OK || args.Result == ButtonResult.Yes)
            //    {
            //        var @object = (args.Parameters as PropertyGridDialogParameters).Object;
            //    }
            //});

            Navigate(RegionNames.LIBRARY_TOOLBAR,
                typeof(LibraryDetailToolBar).FullName,
                null, new NavigationParameters { { NavParamKeys.CURRENT_LIBRARY, SelectedItem } });

            Navigate(RegionNames.LIBRARY_MAIN_CONTENT,
                typeof(LibraryDetailPanel).FullName,
                null, new NavigationParameters { { NavParamKeys.CURRENT_LIBRARY, SelectedItem } });
        }

        #endregion Methods
    }
}