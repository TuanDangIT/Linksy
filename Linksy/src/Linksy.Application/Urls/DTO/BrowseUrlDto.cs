using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.DTO
{
    public record class BrowseUrlDto(int Id, string OriginalUrl, string Code, int VisitsCount, bool IsActive, bool HasScanCode, bool HasLandingPageItem);
}
