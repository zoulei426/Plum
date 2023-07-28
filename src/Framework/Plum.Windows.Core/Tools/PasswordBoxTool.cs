using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace Plum.Windows.Tools
{
    /// <summary>
    /// PasswordBoxHelper
    /// </summary>
    public static class PasswordBoxTool
    {
        /// <summary>
        /// The password property
        /// </summary>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(PasswordBoxTool),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        /// <summary>
        /// Called when [password property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            string password = (string)e.NewValue;

            if (passwordBox != null && passwordBox.Password != password)
            {
                passwordBox.Password = password;
            }
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <param name="dp">The dp.</param>
        /// <returns></returns>
        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <param name="dp">The dp.</param>
        /// <param name="value">The value.</param>
        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }
    }

    /// <summary>
    /// PasswordBoxBehavior
    /// </summary>
    /// <seealso cref="Microsoft.Xaml.Behaviors.Behavior{System.Windows.Controls.PasswordBox}" />
    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        /// <summary>
        /// Called when [attached].
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        /// <summary>
        /// Called when [password changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            string password = PasswordBoxTool.GetPassword(passwordBox);

            if (passwordBox != null && passwordBox.Password != password)
            {
                PasswordBoxTool.SetPassword(passwordBox, passwordBox.Password);
            }
        }

        /// <summary>
        /// Called when [detaching].
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if(AssociatedObject != null)
                AssociatedObject.PasswordChanged -= OnPasswordChanged;
        }
    }
}