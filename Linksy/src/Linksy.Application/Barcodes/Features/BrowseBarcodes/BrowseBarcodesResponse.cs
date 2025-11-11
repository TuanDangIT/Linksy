using Linksy.Application.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.BrowseBarcodes
{
    public record class BrowseBarcodesResponse(PagedResult<BrowseBarcodeDto> PagedResult);
}
