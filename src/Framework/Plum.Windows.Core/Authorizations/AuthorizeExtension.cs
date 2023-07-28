using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Plum.Windows.Authorizations
{
    /// <summary>
    /// 认证
    /// </summary>
    [MarkupExtensionReturnType(typeof(string))]
    public class AuthorizeExtension : MarkupExtension
    {
        public string PolicyName { get; }

        public AuthorizeExtension(string policyName)
        {
            PolicyName = policyName;
        }

        /// <summary>
        /// Provides the value.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"The {nameof(serviceProvider)} must implement {nameof(IProvideValueTarget)} interface.</exception>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget provideValueTarget)
                throw new ArgumentException(
                    $"The {nameof(serviceProvider)} must implement {nameof(IProvideValueTarget)} interface.");

            if (provideValueTarget.TargetObject.GetType().FullName == "System.Windows.SharedDp") return this;

            var frameworkElement = provideValueTarget.TargetObject is DependencyObject dependencyObject
                ? dependencyObject as FrameworkElement ?? dependencyObject.TryFindParent<FrameworkElement>()
                : null;

            return new Binding(nameof(AuthorizeSource.Visibility))
            {
                Source = new AuthorizeSource(PolicyName, frameworkElement),
                Mode = BindingMode.OneWay
            }.ProvideValue(serviceProvider);
        }
    }
}