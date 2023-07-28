namespace Plum.Log
{
    /// <summary>
    /// 基于 Serilog 实现日志接口
    /// </summary>
    /// <seealso cref="Plum.ILogger" />
    public class SeriLogger : ILogger
    {
        /// <summary>
        /// Infomations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Infomation(string message)
        {
            Serilog.Log.Information(message);
        }

        /// <summary>
        /// Warnings the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warning(string message)
        {
            Serilog.Log.Warning(message);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message)
        {
            Serilog.Log.Error(message);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <param name="propertyValue">The property value.</param>
        public void Error<T>(string message, T propertyValue)
        {
            Serilog.Log.Error(message, propertyValue);
        }
    }
}