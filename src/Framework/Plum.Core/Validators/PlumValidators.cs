using Plum.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace Plum.Validators
{
    public static class PlumValidators
    {
        public static IRuleBuilderOptions<T, TProperty> ICN<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder, Expression<Func<T, object>> propertySelector)
        {
            var pi = propertySelector.GetPropertyInfo();
            if (pi == null) throw new InvalidOperationException($"Cannot find the property specified by the selector.");

            return ruleBuilder.Must((instance, zjh) =>
                PlumValidationRule.IsICN(
                    pi.GetValue(instance)?.ToString().ToEnum<ICNType>() ?? ICNType.QT,
                    zjh as string))
                .WithMessage("证件号格式错误");
        }

        public static IRuleBuilderOptions<T, TProperty> IsSameTo<T, TProperty>(
           this IRuleBuilder<T, TProperty> ruleBuilder, Expression<Func<T, object>> propertySelector)
        {
            var pi = propertySelector.GetPropertyInfo();
            if (pi == null) throw new InvalidOperationException($"Cannot find the property specified by the selector.");

            return ruleBuilder.Must((instance, target) =>
                    pi.GetValue(instance)?.ToString() == target as string)
                .WithMessage($"两次输入不一致");
        }

        public static IRuleBuilderOptions<T, TProperty> IsAnd<T, TProperty>(
           this IRuleBuilder<T, TProperty> ruleBuilder, Expression<Func<T, object>> ps1, Expression<Func<T, object>> ps2)
        {
            var pi1 = ps1.GetPropertyInfo();
            if (pi1 == null) throw new InvalidOperationException($"Cannot find the property specified by the selector.");

            var pi2 = ps2.GetPropertyInfo();
            if (pi2 == null) throw new InvalidOperationException($"Cannot find the property specified by the selector.");

            return ruleBuilder.Must((instance, target) =>
            PlumValidationRule.IsAnd(pi1.GetValue(instance).ToNullableFloat(), pi2.GetValue(instance).ToNullableFloat(), target as float?))
                .WithMessage((a, b) =>
                {
                    return $"{pi1.GetDisplayName()}与{pi2.GetDisplayName()}之和不等于";
                });
        }

        public static IRuleBuilderOptions<T, TProperty> IsValidFile<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder, List<string> allowedTypes = null)
        {
            return ruleBuilder.Must(x =>
                File.Exists(x as string) &&
                (allowedTypes is null || allowedTypes.Count == 0 || allowedTypes.Contains(Path.GetExtension(x as string))))
                .WithMessage("文件不存在或文件类型错误");
        }

        public static IRuleBuilderOptions<T, TProperty> IsValidFile<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder, string allowedType = null)
        {
            return ruleBuilder.Must(x =>
                File.Exists(x as string) &&
                (allowedType.IsNullOrEmpty() || allowedType.Equals(Path.GetExtension(x as string))))
                .WithMessage("文件不存在或文件类型错误");
        }
    }
}