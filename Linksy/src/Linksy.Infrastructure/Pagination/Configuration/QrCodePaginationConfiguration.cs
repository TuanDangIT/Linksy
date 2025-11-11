using Linksy.Domain.Entities.ScanCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Configuration
{
    internal class QrCodePaginationConfiguration : IPaginationConfiguration<QrCode>
    {
        public Dictionary<string, Expression<Func<QrCode, object>>> AllowedOrders => new(StringComparer.OrdinalIgnoreCase)
        {
            { "ScanCount", qrCode => qrCode.ScanCount },
            { "CreatedAt", qrCode => qrCode.CreatedAt },
            //{ "UpdatedAt", qrCode => qrCode.UpdatedAt },
            { "Id", qrCode => qrCode.Id },
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
