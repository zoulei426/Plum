using Plum.Object;
using Plum.Validation;
using PropertyChanged;

namespace Plum.Models
{
    [AddINotifyPropertyChangedInterface]
    public class ChangePasswordDvo : ValidityDvo
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }

        public ChangePasswordDvo(IValidatorProvider validatorProvider) : base(validatorProvider)
        {
        }
    }
}