﻿using Microsoft.Extensions.Localization;
using Plum.Config;
using Plum.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Plum.Windwos.Localization
{
    /// <summary>
    /// LocalizerManager
    /// </summary>
    public class LocalizerManager
    {
        private readonly IConfigurator configurator;
        private readonly IStringLocalizerFactory localizerFactory;

        /// <summary>
        /// Occurs when [current UI culture changed].
        /// </summary>
        public event Action CurrentUICultureChanged;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static LocalizerManager Instance { get; private set; }

        /// <summary>
        /// Initializes the specified configure.
        /// </summary>
        /// <param name="configure">The configure.</param>
        /// <param name="localizerFactory">The localizer factory.</param>
        public static void Initialize(IConfigurator configure, IStringLocalizerFactory localizerFactory)
        {
            Instance = new LocalizerManager(configure, localizerFactory);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizerManager"/> class.
        /// </summary>
        /// <param name="configurator">The configure.</param>
        /// <param name="localizerFactory">The localizer factory.</param>
        private LocalizerManager(IConfigurator configurator, IStringLocalizerFactory localizerFactory)
        {
            this.configurator = configurator;
            this.localizerFactory = localizerFactory;
        }

        /// <summary>
        /// Gets the available culture infos.
        /// </summary>
        /// <value>
        /// The available culture infos.
        /// </value>
        public IEnumerable<CultureInfo> AvailableCultureInfos => new[]
        {
            new CultureInfo("zh-CN"),
            new CultureInfo("en-US")
        };

        /// <summary>
        /// Gets or sets the current UI culture.
        /// </summary>
        /// <value>
        /// The current UI culture.
        /// </value>
        public CultureInfo CurrentUICulture
        {
            get => CultureInfo.CurrentUICulture;
            set
            {
                if (EqualityComparer<CultureInfo>.Default.Equals(value, CultureInfo.CurrentUICulture)) return;

                CultureInfo.CurrentUICulture = value;
                //CultureInfo.DefaultThreadCurrentUICulture = value;
                configurator.SetValue(SystemConst.LANGUAGE, value);
                OnCurrentUICultureChanged();
            }
        }

        /// <summary>
        /// Called when [current UI culture changed].
        /// </summary>
        private void OnCurrentUICultureChanged() => CurrentUICultureChanged?.Invoke();

        private static string BASENAME_SEPARATOR = ".";

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string Get(string key)
        {
            var baseName = string.Empty;
            if (key.IndexOf(BASENAME_SEPARATOR) > 0)
            {
                baseName = key.Substring(0, key.IndexOf(BASENAME_SEPARATOR));
                key = key.Substring(key.IndexOf(BASENAME_SEPARATOR) + 1);
            }
            var localizer = localizerFactory.Create(baseName, string.Empty);
            return localizer[key].Value;
        }
    }
}