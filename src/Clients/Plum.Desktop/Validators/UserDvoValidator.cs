using Plum.Models;
using FluentValidation;

namespace Plum.Validators
{
    public class UserDvoValidator : AbstractValidator<UserDvo>
    {
        public UserDvoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}