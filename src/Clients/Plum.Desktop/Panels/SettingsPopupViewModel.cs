﻿using Plum.Dialogs;
using Plum.Models;
using Plum.Users;
using Plum.Validation;
using Plum.Windows.Mvvm;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Volo.Abp.Account;
using Volo.Abp.Identity;

namespace Plum.Panels
{
    [AddINotifyPropertyChangedInterface]
    public class SettingsPopupViewModel : ViewModelBase, IViewLoadedAndUnloadedAware<SettingsPopup>
    {
        #region Properties

        public PlumUser User { get; set; }

        #endregion Properties

        #region Fields

        private SettingsPopup view;

        private Desktop.IPlumApi api;

        #endregion Fields

        #region Commands

        public ICommand ProfileCommand { get; set; }

        public ICommand SettingsCommand { get; set; }

        public ICommand HelpCommand { get; set; }

        public ICommand OpenOfficialSiteCommand { get; set; }

        public ICommand AboutCommand { get; set; }

        public ICommand SignOutCommand { get; set; }

        #endregion Commands

        #region Ctor

        public SettingsPopupViewModel(IContainerExtension container) : base(container)
        {
            User = new PlumUser();
        }

        #endregion Ctor

        #region Methods

        protected override void RegisterCommands()
        {
            ProfileCommand = new DelegateCommand(ExecuteChangeProfile);

            SettingsCommand = new DelegateCommand(async () => { await ShowDialog<SettingsDialog>(); });

            AboutCommand = new DelegateCommand(async () => { await ShowDialog<AboutDialog>(); });

            HelpCommand = new DelegateCommand(() =>
            {
                string helpfile = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Helper.chm";
                var pi = new ProcessStartInfo(helpfile);
                pi.UseShellExecute = true;
                Process.Start(pi);
            });

            SignOutCommand = new DelegateCommand(async () =>
            {
                await api.LogoutAsync().RunApi();

                Configurator.SetValue("AutoSignIn", false);
                ProcessController.Restart();
            });
        }

        public async void OnLoaded(SettingsPopup view)
        {
            this.view = view;
            api = Container.Resolve<Desktop.IPlumApi>();

            var profile = await api.GetMyProfileAsync().RunApi();
            User.CopyPropertiesFrom(profile);
        }

        public void OnUnloaded(SettingsPopup view)
        {
            User = null;
        }

        protected override async Task ShowDialog<TView>(string title = null, IDialogParameters parameters = null, Action<IDialogResult> callback = null, object dialogIdentifier = null)
        {
            view.SetValue(System.Windows.Controls.Primitives.Popup.IsOpenProperty, false);
            await base.ShowDialog<TView>(title, parameters, callback, dialogIdentifier);
        }

        #region Methods - Private

        private async void ExecuteChangeProfile()
        {
            var user = new UserDvo(Container.Resolve<IValidatorProvider>());
            user.CopyPropertiesFrom(User);

            await ShowDialog<ProfileDialog>($"个人信息", new DialogParameters { { "User", user } }, async args =>
              {
                  if (args.Result == ButtonResult.OK || args.Result == ButtonResult.Yes)
                  {
                      var result = args.Parameters.GetValue<UserDvo>("User");
                      var updateProfile = result.ConvertTo<UpdateProfileDto>();
                      var profile = await api.UpdateMyProfileAsync(updateProfile).RunApi();
                      if (profile is not null)
                      {
                          User.CopyPropertiesFrom(profile);
                          Notifier.Success("个人信息更新成功");
                      }

                      var changePassword = args.Parameters.GetValue<ChangePasswordDvo>("ChangePassword");
                      if (changePassword is null ||
                          changePassword.CurrentPassword.IsNullOrEmpty() ||
                          changePassword.NewPassword.IsNullOrEmpty())
                      {
                          return;
                      }

                      var success = await api.ChangePasswordAsync(changePassword.ConvertTo<ChangePasswordInput>()).RunApi();
                      if (success)
                      {
                          Notifier.Success("密码更新成功");
                      }
                  }
              });
        }

        #endregion Methods - Private

        #endregion Methods
    }
}