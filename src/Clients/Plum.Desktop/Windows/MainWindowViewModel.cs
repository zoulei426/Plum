using Plum.Settings;
using Plum.Windows.Consts;
using Plum.Windows.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Modularity;
using PropertyChanged;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Plum.Windows
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel : ViewModelBase, IViewLoadedAndUnloadedAware
    {
        private bool _IsDarkTheme;

        public bool IsDarkTheme
        {
            get { return _IsDarkTheme; }
            set
            {
                _IsDarkTheme = value;

                var resources = Application.Current.Resources.MergedDictionaries;

                var existingResourceDictionary = Application.Current.Resources.MergedDictionaries
                                                .Where(rd => rd.Source != null)
                                                .SingleOrDefault(rd => Regex.Match(rd.Source.OriginalString, @"(\/Themes\/MaterialDesign((Light)|(Dark))Theme)").Success);

                var source = $"pack://application:,,,/Plum.Windows.Controls;component/Themes/MaterialDesign{(value ? "Dark" : "Light")}Theme.xaml";
                var newResourceDictionary = new ResourceDictionary() { Source = new Uri(source) };

                Application.Current.Resources.MergedDictionaries.Remove(existingResourceDictionary);
                Application.Current.Resources.MergedDictionaries.Add(newResourceDictionary);

                //ModifyTheme(theme => theme.SetBaseTheme(value ? Theme.Dark : Theme.Light));
            }
        }

        public MainWindowViewModel(IContainerExtension container) : base(container)
        {
        }

        public void OnLoaded()
        {
            RegionManager.RegisterViewWithRegion(SystemRegionNames.SETTINGS, typeof(CommonSetting));

            var moduleManager = Container.Resolve<IModuleManager>();

            foreach (var item in moduleManager.Modules.OrderBy(x => x.ModuleName))
            {
                moduleManager.LoadModule(item.ModuleName);
            }
        }

        public void OnUnloaded()
        {
        }

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
    }
}