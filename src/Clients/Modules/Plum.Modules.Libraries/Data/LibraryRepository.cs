using Microsoft.EntityFrameworkCore;
using Plum.Modules.Libraries.Entities;
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
        private LibraryDbContext context = new LibraryDbContext();

        public LibraryRepository()
        {
        }

        public long Count(string filter = null)
        {
            return context.Libraries.LongCount();
        }

        public List<DynamicLinkLibrary> Page(int pageIndex, int pageSize, string sorting, string filter = null)
        {
            return context.Libraries
                .OrderBy(x => x.DllId)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}