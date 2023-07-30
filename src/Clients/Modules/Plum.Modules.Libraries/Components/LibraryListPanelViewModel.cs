using NPOI.POIFS.NIO;
using Plum.Modules.Libraries.Data;
using Plum.Modules.Libraries.Entities;
using Plum.Modules.Libraries.Models;
using Plum.Windows.Controls;
using Plum.Windows.Mvvm;
using Prism.Ioc;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plum.Modules.Libraries.Events.EventCenter;

namespace Plum.Modules.Libraries.Components
{
    [AddINotifyPropertyChangedInterface]
    public class LibraryListPanelViewModel :
         PagableViewModel<
             LibraryListPanel,
             LibraryDvo,
             SelectedLibraryChangedEvent,
             RefreshLibraryEvent>
    {
        private PagableDataGrid pdg;

        public LibraryListPanelViewModel(IContainerExtension container) : base(container)
        {
        }

        public override void OnLoaded(LibraryListPanel view)
        {
            base.OnLoaded(view);

            //DataSource = new LibraryPagerProvider(Container.Resolve<ILibraryRepository>());
            DataSource = new LibraryPagerProvider(new LibraryRepository());

            pdg = view.FindName("pdg") as PagableDataGrid;

            Check.NotNull(DataSource, pdg);
        }

        protected override void OnRefresh()
        {
            pdg.Refresh();
        }
    }
}