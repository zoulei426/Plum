using Plum.Modules.Libraries.Data;
using Plum.Validation;
using Plum.Windows.Consts;
using Plum.Windows.Mvvm;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Plum.Modules.Libraries
{
    [Module(ModuleName = "101_动态库", OnDemand = true)]
    public class LibraryModule : ModuleBase
    {
        public LibraryModule(IUnityContainer container) : base(container)
        {
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(typeof(LibraryRepository));

            var currentAssembly = Assembly.GetAssembly(this.GetType());

            containerRegistry.RegisterInstance(typeof(IValidatorLoader), ValidatorLoader.GetInstance().Load(currentAssembly));

            //containerRegistry.RegisterNavigations(currentAssembly);
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            RegionManager.RegisterViewWithRegion(SystemRegionNames.MAIN, typeof(LibraryPage));
        }
    }
}