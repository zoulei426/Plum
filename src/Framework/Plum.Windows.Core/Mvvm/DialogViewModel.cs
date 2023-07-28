using Prism.Ioc;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;

namespace Plum.Windows.Mvvm
{
    [AddINotifyPropertyChangedInterface]
    public abstract class DialogViewModel :
        ViewModelBase,
        IViewLoadedAndUnloadedAware,
        IDialogAware
    {
        public string Title { get; set; }

        public bool IsBusy { get; set; }

        public virtual event Action<IDialogResult> RequestClose;

        public DialogViewModel(IContainerExtension container) : base(container)
        {
        }

        public virtual void OnLoaded()
        {
        }

        public virtual void OnUnloaded()
        {
            Title = null;
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("Title");
        }
    }

    [AddINotifyPropertyChangedInterface]
    public abstract class DialogViewModel<T> :
        ViewModelBase,
        IViewLoadedAndUnloadedAware<T>,
        IDialogAware
    {
        public string Title { get; set; }

        public virtual event Action<IDialogResult> RequestClose;

        public DialogViewModel(IContainerExtension container) : base(container)
        {
        }

        public virtual void OnLoaded(T view)
        {
        }

        public virtual void OnUnloaded(T view)
        {
            Title = null;
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("Title");
        }
    }
}