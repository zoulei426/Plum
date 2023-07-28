using Prism.Services.Dialogs;

namespace Plum.Windows.Mvvm
{
    public interface IDialogContent
    {
        IDialogParameters Commit();
    }
}