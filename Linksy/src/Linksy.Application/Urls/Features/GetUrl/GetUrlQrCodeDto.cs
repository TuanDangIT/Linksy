using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.GetUrl
{
    public record class GetUrlQrCodeDto(int Id, ImageDto QrCodeImage, int ScanCount, DateTime CreatedAt, DateTime? UpdatedAt);
}
