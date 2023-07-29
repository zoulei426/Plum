using NPOI.POIFS.NIO;
using Plum.Modules.Libraries.Data;
using Plum.Modules.Libraries.Models;
using Plum.Windows.Controls;
using Plum.Windows.Mvvm;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plum.Modules.Libraries.Events.EventCenter;

namespace Plum.Modules.Libraries.Components
{
    internal class LibraryListPanelViewModel :
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

            DataSource = new LibraryPagerProvider(Container.Resolve<ILibraryRepository>());

            pdg = view.FindName("pdg") as PagableDataGrid;

            Check.NotNull(DataSource, pdg);
        }

        protected override void OnRefresh()
        {
            pdg.Refresh();
        }
    }
}