﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Plum
{
    public static class PropertyExtensions
    {
        public static PropertyInfo GetPropertyInfo<T>(this Expression<Func<T, object>> propertySelector)
        {
            var expression = propertySelector as LambdaExpression;

            if (expression is null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            var body = expression.Body.NodeType == ExpressionType.MemberAccess ?
                (MemberExpression)expression.Body :
                (MemberExpression)((UnaryExpression)expression.Body).Operand;

            return typeof(T).GetMember(body.Member.Name)[0] as PropertyInfo;
        }

        public static string GetDisplayName(this PropertyInfo pi)
        {
            if (pi is null) return string.Empty;
            var display = pi.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
            if (display.IsNullOrEmpty())
                display = pi.GetCustomAttribute<DescriptionAttribute>()?.Description;
            if (display.IsNullOrEmpty())
                display = pi.Name;

            return display;
        }
    }
}