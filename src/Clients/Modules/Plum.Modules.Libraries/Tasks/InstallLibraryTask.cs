using Downloader;
using Plum.Common;
using Plum.Tasks;
using Plum.Windows.Notify;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Modules.Libraries.Tasks
{
    public class InstallLibraryTask : Plum.Tasks.Task
    {
        #region Fields

        private string installPath;

        #endregion Fields

        #region Methods

        protected override void OnGo()
        {
            var args = Argument as InstallLibraryTaskArgument;

            if (args.Url.IsNullOrEmpty())
            {
                this.ReportError("动态库下载地址不能为空");
                return;
            }

            installPath = args.InstallPath;

            var download = DownloadBuilder.New()
               .WithUrl(args.Url)
               .WithDirectory(args.SavePath)
               .Build();

            download.DownloadStarted += DownloadStarted;
            download.DownloadProgressChanged += DownloadProgressChanged;
            download.DownloadFileCompleted += DownloadFileCompleted;

            download.StartAsync();
        }

        private void DownloadStarted(object? sender, DownloadStartedEventArgs e)
        {
            this.ReportInfomation($"开始下载文件：{Path.GetFileName(e.FileName)}，文件大小：{e.TotalBytesToReceive.ToByteDisplay()}");
        }

        private void DownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs e)
        {
            this.ReportProgress((int)e.ProgressPercentage);
        }

        private void DownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            var package = e.UserState as DownloadPackage;
            if (package is null)
            {
                return;
            }

            var fileName = Path.GetFileName(package.FileName);

            if (e.Error != null)
            {
                this.ReportError($"文件 {fileName} 下载失败：{e.Error.Message}");
                return;
            }

            this.ReportSuccess($"文件 {fileName} 下载完成");

            DirectoryHelper.CreateIfNotExists(installPath);

            if (".zip".Equals(Path.GetExtension(fileName), StringComparison.OrdinalIgnoreCase))
            {
                UnZipFile(package.FileName, installPath);
            }
            else
            {
                File.Copy(package.FileName, Path.Combine(installPath, fileName), true);
            }

            File.Delete(package.FileName);
        }

        private void UnZipFile(string fileName, string targetPath)
        {
            //this.ReportInfomation($"开始解压文件：{Path.GetFileName(fileName)}");

            ZipTool.DeZip(fileName, targetPath);

            this.ReportSuccess($"文件 {Path.GetFileName(fileName)} 解压完成");
        }

        #endregion Methods
    }
}