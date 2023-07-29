using Plum.Modules.Libraries.Entities;
using Plum.Modules.Libraries.Models;
using Plum.Object;
using Plum.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Modules.Libraries.Data
{
    public class LibraryPagerProvider : IDataPagerProvider
    {
        private ILibraryRepository libRepo;

        public LibraryPagerProvider(ILibraryRepository libRepo)
        {
            this.libRepo = Check.NotNull(libRepo);
        }

        public string GetDefaultOrderPropertyName()
        {
            return nameof(DynamicLinkLibrary.DllId);
        }

        public DataColumn[] GetProperties()
        {
            return typeof(DynamicLinkLibrary).GetDataColumnsInclude<DynamicLinkLibrary>(
                x => x.DllId, x => x.DllCode, x => x.DllVersion, x => x.DllUnitName,
                x => x.DllDesc, x => x.DllClzName
                ).ToArray();
        }

        public string GetSumaryStatistic()
        {
            return string.Empty;
        }

        public Paged PagingAsync(int pageIndex, int pageSize, string sorting, string filter = null)
        {
            var count = libRepo.Count(filter);
            if (count == 0)
            {
                return new Paged();
            }

            var list = libRepo.Page(pageIndex, pageSize, sorting, filter);
            return new Paged(count, list.ConvertAll(x => x.CastTo<object>()));
        }
    }
}