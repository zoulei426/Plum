using NPOI.SS.Formula.Functions;
using System.Collections.Generic;

namespace Plum
{
    public class Paged
    {
        public long Count { get; set; }

        public List<object> Items { get; set; }

        public Paged()
        {
            Count = 0;
            Items = new List<object>();
        }

        public Paged(long count, List<object> items)
        {
            Count = count;
            Items = items;
        }
    }

    public class Paged<T>
    {
        public long Count { get; set; }

        public List<T> Items { get; set; }

        public Paged()
        {
            Count = 0;
            Items = new List<T>();
        }

        public Paged(long count, List<T> items)
        {
            Count = count;
            Items = items;
        }
    }
}