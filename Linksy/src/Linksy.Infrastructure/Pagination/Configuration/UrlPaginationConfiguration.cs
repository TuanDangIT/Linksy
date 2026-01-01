using Linksy.Application.Shared.Pagination;
using Linksy.Domain.Entities.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Configuration
{
    internal class UrlPaginationConfiguration : IPaginationConfiguration<Url>
    {
        public Dictionary<string, Expression<Func<Url, object>>> AllowedOrders => new(StringComparer.OrdinalIgnoreCase)
        {
            { "VisitCount", url => url.VisitCount },
            { "CreatedAt", url => url.CreatedAt },
            { "Id", url => url.Id },
        };

        public HashSet<string> AllowedFilters => new(StringComparer.OrdinalIgnoreCase)
        {
            "Code",
            "OriginalUrl",
            "Id",
            "CreatedAt",
            "UpdatedAt",
            "IsActive",
            "Tags",
        };
    }
}
