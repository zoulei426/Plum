using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Plum
{
    [DebuggerStepThrough]
    public static class Check
    {
        public static object NotNull(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            return parameter;
        }

        public static T NotNull<T>(T parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            return parameter;
        }

        public static void NotNull(params object[] parameters)
        {
            if (parameters.Any(item => item == null))
                throw new ArgumentNullException();
        }

        /// <summary>
        /// Throws if null or empty.
        /// </summary>
        /// <param name="strings">The strings.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void NotNullOrEmpty(params string[] strings)
        {
            if (strings.Any(string.IsNullOrEmpty))
                throw new ArgumentNullException();
        }

        /// <summary>
        /// Throws if null or empty.
        /// </summary>
        /// <param name="strings">The strings.</param>
        public static void NotNullOrEmpty(IEnumerable<string> strings)
        {
            NotNull(strings);
            NotNullOrEmpty(strings.ToArray());
        }

        /// <summary>
        /// Throws if file not found.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="FileNotFoundException">Can not found the specified file path.</exception>
        public static void FileExisted(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Can not found the specified file path. ", path);
        }

        /// <summary>
        /// Throws if folder not fount.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="DirectoryNotFoundException">Can not found the specified path {path}.</exception>
        public static void FolderExisted(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Can not found the specified path {path}. ");
        }

        /// <summary>
        /// Throws if invalid path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="DirectoryNotFoundException">The specified path is not a valid file or directory.  ({path})</exception>
        public static void PathValid(string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
                throw new DirectoryNotFoundException($"The specified path is not a valid file or directory.  ({path})");
        }
    }
}