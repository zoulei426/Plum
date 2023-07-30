using Plum.Modules.Libraries.Consts;
using Plum.Modules.Libraries.Models;
using Plum.Windows.Mvvm;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plum.Modules.Libraries.Events.EventCenter;
using System.Windows.Input;
using PropertyChanged;
using Plum.Windows.Commands;
using Downloader;
using Plum.Tools;
using Plum.Windows;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using Plum.Modules.Libraries.Tasks;
using Plum.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Plum.Modules.Libraries.Entities;
using Prism.Services.Dialogs;

namespace Plum.Modules.Libraries.Components
{
    [AddINotifyPropertyChangedInterface]
    public class LibraryListToolBarViewModel : SelectedItemViewModel<LibraryDvo, SelectedLibraryChangedEvent, RefreshLibraryEvent>
    {
        #region Commands

        public ICommand DetailCommand { get; set; }

        public ICommand InstallCommand { get; set; }

        public ICommand UpdateCommand { get; set; }

        public ICommand UninstallCommand { get; set; }

        #endregion Commands

        #region Ctor

        public LibraryListToolBarViewModel(IContainerExtension container) : base(container)
        {
        }

        #endregion Ctor

        #region Methods

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            DetailCommand = new RelayCommand(OnDetail, CanDetail);
            InstallCommand = new RelayCommand(OnInstall, CanInstall);
            UpdateCommand = new RelayCommand(OnUpdate, CanUpdate);
            UninstallCommand = new RelayCommand(OnUninstall, CanUninstall);
        }

        private bool CanDetail()
        {
            return SelectedItem is not null;
        }

        private void OnDetail()
        {
            //await ShowObjectDialog($"动态库详情", SelectedItem, async args =>
            //{
            //    if (args.Result == ButtonResult.OK || args.Result == ButtonResult.Yes)
            //    {
            //        var @object = (args.Parameters as PropertyGridDialogParameters).Object;
            //    }
            //});

            Navigate(RegionNames.LIBRARY_TOOLBAR,
                typeof(LibraryDetailToolBar).FullName,
                null, new NavigationParameters { { NavParamKeys.CURRENT_LIBRARY, SelectedItem } });

            Navigate(RegionNames.LIBRARY_MAIN_CONTENT,
                typeof(LibraryDetailPanel).FullName,
                null, new NavigationParameters { { NavParamKeys.CURRENT_LIBRARY, SelectedItem } });
        }

        private bool CanInstall()
        {
            return SelectedItem is not null && SelectedItem.Status == LibraryStatus.Uninstalled;
        }

        private void OnInstall()
        {
            var url = SelectedItem.DllTargetPath;
            var savePath = Path.Combine(SystemPath.Data);
            var installPath = Path.Combine(LibraryConsts.LIBRARY_PATH, SelectedItem.DllCode, SelectedItem.DllVersion);

            var args = new InstallLibraryTaskArgument
            {
                Library = SelectedItem,
                Url = url,
                SavePath = savePath,
                InstallPath = installPath,
            };
            var task = new InstallLibraryTask
            {
                Argument = args
            };

            task.Alert += TaskAlert;
            task.Completed += TaskCompleted;
            task.StartAsync();
        }

        private void TaskCompleted(object sender, TaskCompletedEventArgs e)
        {
            var args = e.Argument as InstallLibraryTaskArgument;
            if (args != null)
            {
                args.Library.Refresh();
            }
        }

        private bool CanUpdate()
        {
            return SelectedItem is not null && SelectedItem.Status == LibraryStatus.Renewable;
        }

        private void OnUpdate()
        {
            OnInstall();
        }

        private bool CanUninstall()
        {
            return SelectedItem is not null && SelectedItem.Status >= LibraryStatus.Installed;
        }

        private async void OnUninstall()
        {
            await ShowConfirmDialogAsync(null, async dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK || dialogResult.Result == ButtonResult.Yes)
                {
                    Directory.Delete(SelectedItem.LocalPath, true);
                    SelectedItem.Refresh();

                    var message = $"卸载 {SelectedItem} 完成";
                    Notifier.Success(message);
                    Logger.Infomation(message);
                }
            });
        }

        #endregion Methods
    }
}