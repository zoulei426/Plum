using AutoUpdaterDotNET;
using Plum.Windows.Attributes;
using Plum.Windows.Mvvm;
using Plum.Windows.Notify;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Plum.Windows.Apps.Components
{
    [AddINotifyPropertyChangedInterface]
    [View(typeof(AboutDialog))]
    public class AboutDialogViewModel : DialogViewModel
    {
        public Version Version { get; set; }

        public ICommand CheckCommand { get; set; }

        public override event Action<IDialogResult> RequestClose;

        public AboutDialogViewModel(IContainerExtension container) : base(container)
        {
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();
            CheckCommand = new DelegateCommand(ExecuteCheckUpdate);
        }

        public override void OnLoaded()
        {
            base.OnLoaded();

            Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            AutoUpdater.CheckForUpdateEvent += AutoUpdater_CheckForUpdateEvent;
        }

        private void ExecuteCheckUpdate()
        {
            var httpClient = Container.Resolve<IPlumService>().CenterClient;
            AutoUpdater.Start($"{httpClient.BaseAddress}AutoUpdater.xml");
        }

        private void AutoUpdater_CheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.Error == null)
            {
                if (args.IsUpdateAvailable)
                {
                    Notifier.Ask($"有新版本{args.CurrentVersion}可用，当前版本{args.InstalledVersion}，请问是否更新？", (res) =>
                    {
                        if (res)
                        {
                            try
                            {
                                if (AutoUpdater.DownloadUpdate(args))
                                {
                                    ProcessController.Shutdown();
                                }
                            }
                            catch (Exception ex)
                            {
                                Notifier.Error(ex.Message);
                            }
                        }

                        return true;
                    });
                }
                else
                {
                    Notifier.Success("当前已经是最新版本");
                }
            }
            else
            {
                if (args.Error is WebException)
                {
                    Notifier.Error("无法连接更新服务，请检查你的网络连接");
                }
                else
                {
                    Notifier.Error(args.Error.Message);
                }
            }
        }
    }
}