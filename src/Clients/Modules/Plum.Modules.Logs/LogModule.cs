using Plum.Validation;
using Plum.Windows.Consts;
using Plum.Windows.Mvvm;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Plum.Modules.Logs
{
    [Module(ModuleName = "109_日志", OnDemand = true)]
    public class LogModule : ModuleBase
    {
        public LogModule(IUnityContainer container) : base(container)
        {
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            RegionManager.RegisterViewWithRegion(SystemRegionNames.MAIN, typeof(LogPage));
        }
    }
}