using Plum.Attributes;
using Plum.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plum.Object
{
    public class ValidityDvo : DataViewObject, IValidityInfo
    {
        #region Properties

        [Enabled(false)]
        public bool IsValid
        {
            get
            {
                return Errors.Count == 0;
            }
        }

        [Enabled(false)]
        public string ValidInfo
        {
            get
            {
                return GetValidInfo();
            }
        }

        [Enabled(false)]
        public string Error { get; set; }

        [Enabled(false)]
        public Dictionary<string, List<string>> Errors { get; }

        #endregion Properties

        #region Fields

        private readonly IValidatorProvider validatorProvider;
        private IValidationContext validationContext;
        private IValidator validator { get; set; }

        #endregion Fields

        #region Ctor

        public ValidityDvo(IValidatorProvider validatorProvider)
        {
            this.validatorProvider = validatorProvider;
            Errors = new Dictionary<string, List<string>>();

            //ValidAsync();
        }

        #endregion Ctor

        #region Methods

        public async void ValidAsync()
        {
            if (validator is null)
            {
                validator = validatorProvider.GetValidator(this.GetType());
            }
            if (validator is null)
            {
                return;
            }

            if (validationContext is null)
            {
                validationContext = new ValidationContext<ValidityDvo>(this);
            }

            var result = await validator.ValidateAsync(validationContext);

            Errors.Clear();
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    if (Errors.ContainsKey(error.PropertyName))
                    {
                        Errors[error.PropertyName].Add(error.ErrorMessage);
                    }
                    else
                    {
                        Errors.Add(error.PropertyName, new List<string> { error.ErrorMessage });
                    }
                }
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (validator is null)
                {
                    validator = validatorProvider.GetValidator(this.GetType());
                }
                if (validator is null)
                {
                    return string.Empty;
                }

                if (validationContext is null)
                {
                    validationContext = new ValidationContext<ValidityDvo>(this);
                }

                var errors = validator.Validate(validationContext).Errors.Where(x => x.PropertyName == columnName);

                if (errors.Count() > 0)
                {
                    var errorMessages = errors.Select(x => x.ErrorMessage).ToList();
                    Errors[columnName] = errorMessages;
                    return errorMessages.JoinStrings(Environment.NewLine);
                }
                else
                {
                    Errors.Remove(columnName);
                    return string.Empty;
                }
            }
        }

        private string GetValidInfo()
        {
            if (Errors is null || Errors.Count == 0)
                return string.Empty;

            var result = new StringBuilder();
            foreach (var errorDic in Errors)
            {
                result.AppendLine(errorDic.Value.JoinStrings(Environment.NewLine));
            }

            return result.ToString();
        }

        #endregion Methods
    }
}