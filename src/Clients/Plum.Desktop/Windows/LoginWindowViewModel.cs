﻿using AutoUpdaterDotNET;
using Plum.Consts;
using Plum.Events;
using Plum.Windows.Mvvm;
using Prism.Ioc;
using PropertyChanged;
using System.Windows.Controls;

namespace Plum.Windows
{
    [AddINotifyPropertyChangedInterface]
    public class LoginWindowViewModel : ViewModelBase, IViewLoadedAndUnloadedAware<LoginWindow>
    {
        #region Fields

        private TabItem SignInTabItem;

        #endregion Fields

        #region Properties

        public bool IsLoading { get; set; }

        #endregion Properties

        #region Ctor

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="container"></param>
        public LoginWindowViewModel(IContainerExtension container) : base(container)
        {
            EventAggregator.GetEvent<MainWindowLoadingEvent>().Subscribe(e => IsLoading = e);
            EventAggregator.GetEvent<SignUpSuccessEvent>().Subscribe(signUpInfo => SignInTabItem.IsSelected = true);
            EventAggregator.GetEvent<SettingSeccessEvent>().Subscribe(() => SignInTabItem.IsSelected = true);
        }

        public void OnLoaded(LoginWindow view)
        {
            this.SignInTabItem = view.FindName("SignInTabItem") as TabItem;

            var autoCheckUpdate = Configurator.GetValue<bool>(ConfigKeys.AutoCheckUpdate);
            if (autoCheckUpdate)
            {
                var httpClient = Container.Resolve<IPlumService>().CenterClient;

                AutoUpdater.Start($"{httpClient.BaseAddress}AutoUpdater.xml");
            }
        }

        public void OnUnloaded(LoginWindow view)
        {
        }

        #endregion Ctor
    }
}