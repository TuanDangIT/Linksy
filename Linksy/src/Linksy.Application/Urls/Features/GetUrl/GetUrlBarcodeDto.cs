using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.GetUrl
{
    public record class GetUrlBarcodeDto(int Id, string ImageUrl, int ScanCount, DateTime CreatedAt, DateTime? UpdatedAt);
}
