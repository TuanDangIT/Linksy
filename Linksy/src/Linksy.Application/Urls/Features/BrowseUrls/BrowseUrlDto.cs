using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.BrowseUrls
{
    public record class BrowseUrlDto(int Id, string? OriginalUrl, string Code, int VisitsCount, bool IsActive, bool HasQrCode, bool HasBarcode, bool HasLandingPageItem, bool HasUmtParameter, IEnumerable<string>? Tags, DateTime CreatedAt, DateTime? UpdatedAt);
}
