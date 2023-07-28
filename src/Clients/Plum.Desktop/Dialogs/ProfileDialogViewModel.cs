using Plum.Models;
using Plum.Validation;
using Plum.Windows.Attributes;
using Plum.Windows.Commands;
using Plum.Windows.Mvvm;
using Prism.Ioc;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Windows.Input;

namespace Plum.Dialogs
{
    [AddINotifyPropertyChangedInterface]
    [View(typeof(ProfileDialog))]
    public class ProfileDialogViewModel : DialogViewModel
    {
        public UserDvo User { get; set; }
        public ChangePasswordDvo ChangePassword { get; set; }

        public override event Action<IDialogResult> RequestClose;

        public ICommand ConfirmCommand { get; set; }

        public ProfileDialogViewModel(IContainerExtension container) : base(container)
        {
        }

        protected override void RegisterCommands()
        {
            ConfirmCommand = new RelayCommand(ExecuteConfirm, CanConfirm);
        }

        private bool CanConfirm()
        {
            if (User is not IValidityInfo validInfo || !validInfo.IsValid)
            {
                return false;
            }
            if (ChangePassword is not IValidityInfo validInfo2 || !validInfo2.IsValid)
            {
                return false;
            }
            return true;
        }

        private void ExecuteConfirm()
        {
            RequestClose.Invoke(new DialogResult(
                ButtonResult.OK,
                new DialogParameters {
                    { "User", User },
                    { "ChangePassword", ChangePassword }
                }));

            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(true, null);
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
        }

        public override void OnUnloaded()
        {
            base.OnUnloaded();
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            User = parameters.GetValue<UserDvo>("User");
            ChangePassword = new ChangePasswordDvo(Container.Resolve<IValidatorProvider>());
        }
    }
}