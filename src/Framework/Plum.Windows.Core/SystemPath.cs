﻿using System;
using System.IO;

namespace Plum.Windows
{
    /// <summary>
    /// 系统路径
    /// </summary>
    public static class SystemPath
    {
        /// <summary>
        /// It represents the path where the "Plum.Desktop" is located.
        /// </summary>
        public static readonly string Application = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Data
        /// </summary>
        public static readonly string Data = Path.Combine(Application, nameof(Data));

        /// <summary>
        /// Logs
        /// </summary>
        public static readonly string Logs = Path.Combine(Application, nameof(Logs));

        /// <summary>
        /// Configs
        /// </summary>
        public static readonly string Configs = Path.Combine(Application, nameof(Configs));

        /// <summary>
        /// %AppData%\Plum
        /// </summary>
        public static readonly string AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Plum");

        /// <summary>
        /// %AppData%\Plum\Apps
        /// </summary>
        public static readonly string Apps = Path.Combine(AppData, nameof(Apps));

        /// <summary>
        /// %AppData%\Plum\Users
        /// </summary>
        public static readonly string Users = Path.Combine(AppData, nameof(Users));

        /// <summary>
        /// Initializes the <see cref="SystemPath"/> class.
        /// </summary>
        static SystemPath()
        {
            Directory.CreateDirectory(Data);
            Directory.CreateDirectory(Logs);
            Directory.CreateDirectory(Configs);
            Directory.CreateDirectory(Apps);
            Directory.CreateDirectory(Users);
        }
    }
}