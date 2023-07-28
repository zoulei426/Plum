using System;
using System.IO;

namespace Plum.Tools
{
    public class PathTool
    {
        public static string GetNewFileName(string fileName)
        {
            string directory = Path.GetDirectoryName(fileName);
            string filename = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            int counter = 1;
            int MAX_COUNT = 1024;
            var newFileName = fileName;

            while (File.Exists(newFileName))
            {
                if (counter > MAX_COUNT)
                {
                    throw new Exception($"此目录下同名文件已超过{MAX_COUNT}个，无法重命名");
                }
                string newFilename = "{0}({1}){2}".FormatWith(filename, counter, extension);
                newFileName = Path.Combine(directory, newFilename);
                counter++;
            }

            return newFileName;
        }
    }
}