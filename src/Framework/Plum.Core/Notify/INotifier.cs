using System;

namespace Plum.Notify
{
    /// <summary>
    /// 通知接口
    /// </summary>
    public interface INotifier
    {
        void Ask(string message, Func<bool, bool> confirmed, bool global = false);

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message, bool global = false);

        /// <summary>
        /// Successes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Success(string message, bool global = false);

        /// <summary>
        /// Warnings the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warning(string message, bool global = false);

        /// <summary>
        /// Errors this instance.
        /// </summary>
        void Error(string message, bool global = false);

        void Clear();
    }
}