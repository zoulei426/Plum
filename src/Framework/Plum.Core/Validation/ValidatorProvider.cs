using FluentValidation;
using System;
using System.Linq;

namespace Plum.Validation
{
    public class ValidatorProvider : IValidatorProvider
    {
        private readonly IValidatorLoader validatorLoader;

        public ValidatorProvider(IValidatorLoader validatorLoader)
        {
            Check.NotNull(validatorLoader);

            this.validatorLoader = validatorLoader;
        }

        public IValidator GetValidator(Type type)
        {
            var result = validatorLoader.ScanResults.FirstOrDefault(x => x.InterfaceType.GenericTypeArguments.Contains(type));
            if (result is null) return null;
            return Activator.CreateInstance(result.ValidatorType) as IValidator;
        }
    }
}