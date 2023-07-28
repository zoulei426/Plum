using FluentValidation;
using System;

namespace Plum.Validation
{
    public interface IValidatorProvider
    {
        IValidator GetValidator(Type type);
    }
}