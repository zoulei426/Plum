using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Plum
{
    public class ZipTool
    {
        public static void DeCompress(string zipFile, string resultPath, Action<string, double, double> progress = null)
        {
            if (!File.Exists(zipFile))
            {
                throw new FileNotFoundException($"文件{zipFile}不存在");
            }
            if (!Directory.Exists(resultPath))
            {
                throw new DirectoryNotFoundException($"路径{resultPath}不存在");
            }

            // 解决中文路径乱码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding("GB2312");
            ZipStrings.CodePage = encoding.CodePage;

            using var fs = new FileStream(zipFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var zis = new ZipInputStream(fs);
            ZipEntry entry;
            var totalLength = fs.Length;
            var decompressedLength = 0L;
            while ((entry = zis.GetNextEntry()) != null)
            {
                var fileName = Path.Combine(resultPath, entry.Name);
                var directoryName = Path.GetDirectoryName(fileName);
                Directory.CreateDirectory(directoryName);
                //如果文件的压缩后大小为0那么说明这个文件是空的,因此不需要进行读出写入
                if (entry.CompressedSize == 0 || entry.IsDirectory)
                    continue;

                decompressedLength += entry.CompressedSize;
                var totalProgress = decompressedLength * 1.0 / totalLength * 100;

                using var streamWriter = File.Create(fileName);
                int size = 4096;
                byte[] data = new byte[size];
                var decompressedSize = 0L;
                var totalSize = entry.Size;
                while (true)
                {
                    size = zis.Read(data, 0, data.Length);

                    if (size > 0)
                    {
                        streamWriter.Write(data, 0, size);

                        decompressedSize += size;

                        if (progress is not null)
                        {
                            progress.Invoke(
                            Path.GetFileName(fileName),
                            decompressedSize * 1.0 / totalSize * 100,
                            totalProgress);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            progress(string.Empty, 100, 100);
        }

        public static async Task DeCompressAsync(string zipFile, string resultPath, Action<string, double, double> progress = null)
        {
            await Task.Run(() =>
            {
                DeCompress(zipFile, resultPath, progress);
            });
        }
    }
}