using Plum.Models;
using FluentValidation;

namespace Plum.Validators
{
    public class ChangePasswordDvoValidator : AbstractValidator<ChangePasswordDvo>
    {
        public ChangePasswordDvoValidator()
        {
            //RuleFor(x => x.NewPassword).Must(BeSame).WithMessage("请确认两次密码输入一致");
            //RuleFor(x => x.NewPassword).IsSameTo(x => x.ConfirmPassword);
            RuleFor(x => x.ConfirmPassword).IsSameTo(x => x.NewPassword);
        }

        private bool BeSame(ChangePasswordDvo x, string arg)
        {
            return x.ConfirmPassword == arg;
        }
    }
}