using Plum.Consts;
using Plum.Windows.Mvvm;
using Prism.Ioc;
using PropertyChanged;

namespace Plum.Settings
{
    [AddINotifyPropertyChangedInterface]
    public class CommonSettingViewModel : SettingViewModel
    {
        public bool AutoCheckUpdate { get; set; }

        public CommonSettingViewModel(IContainerExtension container) : base(container)
        {
        }

        public override void LoadSettings()
        {
            AutoCheckUpdate = Configurator.GetValue<bool>(ConfigKeys.AutoCheckUpdate);
        }

        public override void SaveSettings()
        {
            Configurator.SetValue(ConfigKeys.AutoCheckUpdate, AutoCheckUpdate);
        }
    }
}