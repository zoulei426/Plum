using Plum.Config;
using Plum.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plum.Windows.Config
{
    public class Configurator : IConfigurator
    {
        private JObject _storage;
        private string _filePath = Path.Combine(SystemPath.Configs, "system.config");

        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string key) => _storage.Values().Any(token => token.Path == key);

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T GetValue<T>(string key) => (_storage[key]?.ToString() ?? string.Empty).ToObject<T>();

        public T GetValue<T>() where T : new()
        {
            var key = typeof(T).FullName;
            var result = GetValue<T>(key);
            return result ?? new T();
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IConfigurator SetValue<T>(string key, T value)
        {
            if (EqualityComparer<T>.Default.Equals(GetValue<T>(key), value)) return this;

            _storage[key] = value.ToJson(Formatting.Indented);
            Save();
            ValueChanged?.Invoke(this, new ValueChangedEventArgs(key));

            return this;
        }

        public IConfigurator SetValue<T>(T value)
        {
            var key = typeof(T).FullName;
            return SetValue(key, value);
        }

        /// <summary>
        /// Loads the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public IConfigurator Load(string filePath = null)
        {
            if (!string.IsNullOrEmpty(filePath)) _filePath = filePath;

            if (!File.Exists(_filePath))
            {
                _storage = new JObject(JObject.Parse("{}"));
                Save();
            }
            _storage = JObject.Parse(File.ReadAllText(_filePath));

            return this;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        /// <returns></returns>
        public IConfigurator Clear()
        {
            _storage = new JObject();
            Save();
            return this;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void Delete()
        {
            Clear();
            File.Delete(_filePath);
        }

        private void Save() => WriteToLocal(_filePath, _storage.ToString(Formatting.Indented));

        /// <summary>
        /// Writes to local.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="text">The text.</param>
        private void WriteToLocal(string path, string text)
        {
            try
            {
                File.WriteAllText(path, text);
            }
            catch (IOException)
            {
                WriteToLocal(path, text);
            }
        }
    }
}