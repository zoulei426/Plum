using Prism.Ioc;

namespace Plum.Windows.Mvvm
{
    public abstract class SettingViewModel : ViewModelBase, IViewLoadedAndUnloadedAware
    {
        public SettingViewModel(IContainerExtension container) : base(container)
        {
        }

        public void OnLoaded()
        {
            LoadSettings();
        }

        public void OnUnloaded()
        {
            SaveSettings();
        }

        public abstract void LoadSettings();

        public abstract void SaveSettings();
    }
}