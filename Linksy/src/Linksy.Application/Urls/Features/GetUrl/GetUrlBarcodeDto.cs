using Linksy.Application.Shared.DTO;
using Linksy.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.GetUrl
{
    public record class GetUrlBarcodeDto(int Id, ImageDto BarcodeImage, int ScanCount, DateTime CreatedAt, DateTime? UpdatedAt);
}
