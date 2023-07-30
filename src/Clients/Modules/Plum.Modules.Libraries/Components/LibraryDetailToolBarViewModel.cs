using Panuon.UI.Core;
using Plum.Modules.Libraries.Consts;
using Plum.Windows.Mvvm;
using Prism.Ioc;
using Prism.Regions;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Plum.Modules.Libraries.Components
{
    [AddINotifyPropertyChangedInterface]
    public class LibraryDetailToolBarViewModel : NavigableViewModel
    {
        #region Commands

        public ICommand BackCommand { get; set; }

        #endregion Commands

        #region Ctor

        public LibraryDetailToolBarViewModel(IContainerExtension container) : base(container)
        {
        }

        #endregion Ctor

        #region Methods

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            BackCommand = new RelayCommand(ExecuteBack);
        }

        private void ExecuteBack(object obj)
        {
            Navigate(RegionNames.LIBRARY_TOOLBAR,
                  typeof(LibraryListToolBar).FullName,
                  null, new NavigationParameters { });

            Navigate(RegionNames.LIBRARY_MAIN_CONTENT,
                typeof(LibraryListPanel).FullName,
                null, new NavigationParameters { });
        }

        #endregion Methods
    }
}