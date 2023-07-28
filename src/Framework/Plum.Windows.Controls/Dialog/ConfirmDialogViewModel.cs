using Plum.Windows.Attributes;
using Plum.Windows.Mvvm;
using Prism.Ioc;
using PropertyChanged;

namespace Plum.Windows.Controls.Dialog
{
    [AddINotifyPropertyChangedInterface]
    [View(typeof(ConfirmDialog))]
    public class ConfirmDialogViewModel : DialogViewModel
    {
        public ConfirmDialogViewModel(IContainerExtension container) : base(container)
        {
        }
    }
}