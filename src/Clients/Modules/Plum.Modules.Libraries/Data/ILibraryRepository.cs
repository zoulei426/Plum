using Plum.Modules.Libraries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Modules.Libraries.Data
{
    internal interface ILibraryRepository
    {
        long Count(string filter = null);

        List<LibraryDvo> Page(int pageIndex, int pageSize, string sorting, string filter = null);
    }
}