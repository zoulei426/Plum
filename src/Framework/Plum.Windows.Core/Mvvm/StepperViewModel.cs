using MaterialDesignExtensions.Controls;
using Prism.Ioc;
using PropertyChanged;

namespace Plum.Windows.Mvvm
{
    [AddINotifyPropertyChangedInterface]
    public class StepperViewModel : ViewModelBase
    {
        public bool ContentAnimationsEnabled { get; set; }

        public bool IsLinear { get; set; }

        public StepperLayout Layout { get; set; }

        public StepperViewModel(IContainerExtension container) : base(container)
        {
            Layout = StepperLayout.Horizontal;
            ContentAnimationsEnabled = false;
            IsLinear = false;
        }
    }
}