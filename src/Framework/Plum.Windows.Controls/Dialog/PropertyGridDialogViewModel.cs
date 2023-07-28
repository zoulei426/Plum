using Plum.Validation;
using Plum.Windows.Attributes;
using Plum.Windows.Commands;
using Plum.Windows.Mvvm;
using Plum.Windows.Params;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Threading;
using System.Windows.Input;

namespace Plum.Windows.Controls.Dialog
{
    [AddINotifyPropertyChangedInterface]
    [View(typeof(PropertyGridDialog))]
    public class PropertyGridDialogViewModel :
        DialogViewModel<PropertyGridDialog>
    {
        public object Object { get; set; }

        public override event Action<IDialogResult> RequestClose;

        private AutoResetEvent are = new(false);

        private PropertyGrid pg;

        public ICommand HintCommand { get; set; }

        public ICommand ConfirmCommand { get; set; }

        public PropertyGridDialogViewModel(IContainerExtension container) : base(container)
        {
        }

        protected override void RegisterCommands()
        {
            HintCommand = new DelegateCommand(ExecuteHint);
            ConfirmCommand = new RelayCommand(ExecuteConfirm, CanConfirm);
        }

        private void ExecuteHint()
        {
            if (Object is not IValidityInfo validInfo || validInfo.Errors.Count == 0)
            {
                Notifier.Success("验证通过");
            }
            else
            {
                Notifier.Warning(validInfo.ValidInfo);
            }
        }

        private bool CanConfirm()
        {
            //if (Object is not IValidityInfo validInfo || !validInfo.IsValid)
            //{
            //    return false;
            //}
            return true;
        }

        private void ExecuteConfirm()
        {
            if (Object is not IValidityInfo validInfo || validInfo.Errors.Count == 0)
            {
                RequestClose.Invoke(new DialogResult(ButtonResult.OK, new PropertyGridDialogParameters(Object)));
                MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(true, null);
            }
            else
            {
                Notifier.Warning(validInfo.ValidInfo);
            }
        }

        public override void OnLoaded(PropertyGridDialog view)
        {
            pg = view.FindName("pg") as PropertyGrid;

            if (pg is not null)
            {
                pg.InitializeBegin += Pg_InitializeBegin;
                pg.InitializeEnd += Pg_InitializeEnd;
            }
            base.OnLoaded(view);

            //are.WaitOne();
        }

        public override void OnUnloaded(PropertyGridDialog view)
        {
            base.OnUnloaded(view);
            Object = null;

            //if (pg is not null)
            //{
            //    pg.InitializeBegin -= Pg_InitializeBegin;
            //    pg.InitializeEnd -= Pg_InitializeEnd;
            //}
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            Object = parameters.GetValue<object>("Object");
        }

        private void Pg_InitializeEnd(object sender, EventArgs e)
        {
            IsBusy = false;
            //are.Set();
        }

        private void Pg_InitializeBegin(object sender, EventArgs e)
        {
            IsBusy = true;
        }
    }
}