using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.GetQrCode
{
    public record class GetQrCodeUrlDto
    {
        public int Id { get; init; }
        public string OriginalUrl { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public GetQrCodeUrlDto(int id, string originalUrl, string code)
        {
            Id = id;
            OriginalUrl = originalUrl;
            Code = code;
        }
    }
}
