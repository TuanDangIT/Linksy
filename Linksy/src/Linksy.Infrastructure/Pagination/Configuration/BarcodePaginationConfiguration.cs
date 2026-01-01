using Linksy.Application.Shared.Pagination;
using Linksy.Domain.Entities.ScanCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Configuration
{
    internal class BarcodePaginationConfiguration : IPaginationConfiguration<Barcode>
    {
        public Dictionary<string, Expression<Func<Barcode, object>>> AllowedOrders => new(StringComparer.OrdinalIgnoreCase)
        {
            { "ScanCount", barcode => barcode.ScanCount },
            { "CreatedAt", barcode => barcode.CreatedAt },
            //{ "UpdatedAt", barcode => barcode.UpdatedAt },
            { "Id", barcode => barcode.Id },
        };

        public HashSet<string> AllowedFilters => new(StringComparer.OrdinalIgnoreCase)
        {
            "Url.Code",
            "Url.OriginalUrl",
            "Id",
            "CreatedAt",
            "UpdatedAt",
            "IsActive",
            "ScanCount",
            "Tags",
            "ImageUrlPath"
        };
    }
}
