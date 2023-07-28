using Plum.Notify;
using HandyControl.Controls;
using System;
using System.Windows;

namespace Plum.Windows.Notify
{
    /// <summary>
    /// 通知器
    /// </summary>
    public class Notifier : INotifier
    {
        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message, bool global = false)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (global)
                    Growl.InfoGlobal(message);
                else
                    Growl.Info(message);
            });
        }

        /// <summary>
        /// Successes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Success(string message, bool global = false)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (global)
                    Growl.SuccessGlobal(message);
                else
                    Growl.Success(message);
            });
        }

        /// <summary>
        /// Warnings the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warning(string message, bool global = false)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (global)
                    Growl.WarningGlobal(message);
                else
                    Growl.Warning(message);
            });
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message, bool global = false)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (global)
                    Growl.ErrorGlobal(message);
                else
                    Growl.Error(message);
            });
        }

        public void Clear()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Growl.Clear();
                Growl.ClearGlobal();
            });
        }

        public void Ask(string message, Func<bool, bool> confirmed, bool global = false)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (global)
                    Growl.AskGlobal(message, confirmed);
                else
                    Growl.Ask(message, confirmed);
            });
        }
    }
}