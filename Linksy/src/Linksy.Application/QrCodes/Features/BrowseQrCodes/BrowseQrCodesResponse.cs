using Linksy.Application.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.BrowseQrCodes
{
    public record class BrowseQrCodeResponse(PagedResult<BrowseQrCodeDto> PagedResult);
}
