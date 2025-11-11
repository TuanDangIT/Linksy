using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Shared.Pagination
{
    public sealed record class PagedResult<T>
    {
        public IEnumerable<T> Items { get; private set; }
        public int CurrentPageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public int ItemsFrom { get; private set; }
        public int ItemsTo { get; private set; }
        public int TotalItemsCount { get; private set; }

        public PagedResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            CurrentPageNumber = pageNumber;
            TotalItemsCount = totalCount;
            ItemsFrom = pageSize * (pageNumber - 1) + 1;
            ItemsTo = ItemsFrom + pageSize - 1;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
