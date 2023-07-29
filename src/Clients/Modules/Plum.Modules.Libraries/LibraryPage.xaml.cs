using Plum.Modules.Libraries.Consts;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Plum.Modules.Libraries
{
    /// <summary>
    /// LibraryPage.xaml 的交互逻辑
    /// </summary>
    public partial class LibraryPage : TabItem
    {
        public LibraryPage(IRegionManager regionManager)
        {
            InitializeComponent();

            RegionManager.SetRegionName(ctr_toolbar, RegionNames.LIBRARY_TOOLBAR);
            RegionManager.SetRegionName(ctr_maincontent, RegionNames.LIBRARY_MAIN_CONTENT);

            RegionManager.SetRegionManager(ctr_toolbar, regionManager);
            RegionManager.SetRegionManager(ctr_maincontent, regionManager);
        }
    }
}