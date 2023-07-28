using Plum.Events;
using Plum.Models;
using Plum.Windows.Mvvm;
using Prism.Commands;
using Prism.Ioc;
using PropertyChanged;
using System.Windows.Input;

namespace Plum.Panels
{
    [AddINotifyPropertyChangedInterface]
    public class UrlSettingPanelViewModel : ViewModelBase, IViewLoadedAndUnloadedAware
    {
        #region Properties

        public ClientSettingDvo ClientSetting { get; set; }

        #endregion Properties

        #region Fields

        private Desktop.IPlumApi api;

        #endregion Fields

        #region Commands

        /// <summary>
        /// 确认
        /// </summary>
        public ICommand ConfirmCommand { get; set; }

        #endregion Commands

        #region Ctor

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="container"></param>
        public UrlSettingPanelViewModel(IContainerExtension container) : base(container)
        {
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();
            ConfirmCommand = new DelegateCommand(ExecuteConfirm);
        }

        #endregion Ctor

        #region Methods

        public void OnLoaded()
        {
            api = Container.Resolve<Desktop.IPlumApi>();
            ClientSetting = Configurator.GetValue<ClientSettingDvo>();
        }

        public void OnUnloaded()
        {
        }

        #region Methods - Override

        #endregion Methods - Override

        #region Methods - Private

        private void ExecuteConfirm()
        {
            if (ClientSetting.CenterClient.Url.IsNullOrWhiteSpace())
            {
                MessageQueue.Enqueue("省服务器地址不能为空");
                return;
            }
            if (ClientSetting.DomainClient.Url.IsNullOrWhiteSpace())
            {
                MessageQueue.Enqueue("域服务器地址不能为空");
                return;
            }

            Configurator.SetValue(ClientSetting);

            var PlumService = Container.Resolve<IPlumService>();
            PlumService.Load();

            MessageQueue.Enqueue("设置成功");

            EventAggregator.GetEvent<SettingSeccessEvent>().Publish();
        }

        #endregion Methods - Private

        #endregion Methods
    }
}