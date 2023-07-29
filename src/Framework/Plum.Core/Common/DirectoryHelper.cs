using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Common
{
    public static class DirectoryHelper
    {
        public static void CreateIfNotExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public static void DeleteIfExists(string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory);
            }
        }

        public static void DeleteIfExists(string directory, bool recursive)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive);
            }
        }

        public static void CreateIfNotExists(DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        public static bool IsSubDirectoryOf(string parentDirectoryPath, string childDirectoryPath)
        {
            Check.NotNull(parentDirectoryPath, nameof(parentDirectoryPath));
            Check.NotNull(childDirectoryPath, nameof(childDirectoryPath));

            return IsSubDirectoryOf(
                new DirectoryInfo(parentDirectoryPath),
                new DirectoryInfo(childDirectoryPath)
            );
        }

        public static bool IsSubDirectoryOf(DirectoryInfo parentDirectory,
            DirectoryInfo childDirectory)
        {
            Check.NotNull(parentDirectory, nameof(parentDirectory));
            Check.NotNull(childDirectory, nameof(childDirectory));

            if (parentDirectory.FullName == childDirectory.FullName)
            {
                return true;
            }

            var parentOfChild = childDirectory.Parent;
            if (parentOfChild == null)
            {
                return false;
            }

            return IsSubDirectoryOf(parentDirectory, parentOfChild);
        }
    }
}