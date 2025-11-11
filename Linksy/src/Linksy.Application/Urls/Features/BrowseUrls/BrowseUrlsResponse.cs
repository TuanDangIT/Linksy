using Linksy.Application.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.BrowseUrls
{
    public record class BrowseUrlsResponse(PagedResult<BrowseUrlDto> PagedResult);
}
