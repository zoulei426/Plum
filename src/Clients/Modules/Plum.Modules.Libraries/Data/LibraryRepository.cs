using Plum.Modules.Libraries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Modules.Libraries.Data
{
    public class LibraryRepository : ILibraryRepository
    {
        public LibraryRepository()
        {
        }

        public long Count(string filter = null)
        {
            return 1;
        }

        public List<LibraryDvo> Page(int pageIndex, int pageSize, string sorting, string filter = null)
        {
            return new List<LibraryDvo> { new LibraryDvo { Name = "Test" } };
        }
    }
}