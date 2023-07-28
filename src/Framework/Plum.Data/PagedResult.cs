using System.Collections.Generic;

namespace Plum
{
    public class PagedResult
    {
        public long TotalCount { get; set; }

        public List<object> Items { get; set; }

        public PagedResult()
        {
            Items = new List<object>();
        }

        public PagedResult(long count, List<object> items)
        {
            TotalCount = count;
            Items = items;
        }
    }
}