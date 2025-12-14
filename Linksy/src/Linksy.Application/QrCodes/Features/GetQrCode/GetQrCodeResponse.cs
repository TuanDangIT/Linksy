using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.GetQrCode
{
    public record class GetQrCodeResponse : ScanCodeDto
    {
        public GetQrCodeUrlDto? Url { get; init; } = default!;
        public GetQrCodeUmtParameterDto? UmtParameter { get; init; }
        public GetQrCodeResponse(int id, int scanCount, ImageDto image, IEnumerable<string>? tags, DateTime createdAt, DateTime? updatedAt, 
            GetQrCodeUrlDto? url, GetQrCodeUmtParameterDto? umtParameter) : 
            base(id, scanCount, image, tags, createdAt, updatedAt)
        {
            Url = url;
            UmtParameter = umtParameter;
        }
    }
}
