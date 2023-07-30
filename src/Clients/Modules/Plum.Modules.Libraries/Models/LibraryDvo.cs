using Plum.Modules.Libraries.Consts;
using Plum.Modules.Libraries.Entities;
using Plum.Object;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Modules.Libraries.Models
{
    [AddINotifyPropertyChangedInterface]
    public class LibraryDvo : DynamicLinkLibrary
    {
        #region Properties

        [Description("本地版本")]
        public string? LocalVersion { get; set; }

        [Description("本地路径")]
        public string? LocalPath { get; set; }

        [Description("状态")]
        public LibraryStatus Status { get; set; }

        #endregion Properties

        #region Ctor

        public LibraryDvo()
        {
            Status = LibraryStatus.Uninstalled;
        }

        #endregion Ctor

        #region Methods

        public void Refresh()
        {
            var localPath = Path.Combine(LibraryConsts.LIBRARY_PATH, DllCode);
            var localDirInfo = new DirectoryInfo(localPath);
            if (localDirInfo.Exists)
            {
                var versionDirInfos = localDirInfo.GetDirectories();
                var latestVersionDirInfo = versionDirInfos
                    .OrderByDescending(x => x.Name)
                    .FirstOrDefault();

                if (latestVersionDirInfo != null)
                {
                    LocalVersion = latestVersionDirInfo.Name;
                    LocalPath = localDirInfo.FullName;
                    Status = LibraryStatus.Installed;

                    if (new Version(LocalVersion) < new Version(DllVersion ?? "0"))
                    {
                        Status = LibraryStatus.Renewable;
                    }
                }
            }
        }

        #endregion
    }
}