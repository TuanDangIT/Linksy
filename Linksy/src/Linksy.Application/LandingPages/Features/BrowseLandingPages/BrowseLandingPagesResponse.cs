using Linksy.Application.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.BrowseLandingPages
{
    public record class BrowseLandingPagesResponse(PagedResult<BrowseLandingPageDto> PagedResult);
}
