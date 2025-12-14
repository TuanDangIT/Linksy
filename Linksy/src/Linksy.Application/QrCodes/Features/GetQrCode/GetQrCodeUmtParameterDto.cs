using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.GetQrCode
{
    public record class GetQrCodeUmtParameterDto
    {
        public int Id { get; init; }
        public string? UmtSource { get; init; }
        public string? UmtMedium { get; init; }
        public string? UmtCampaign { get; init; }
        public int UrlId { get; init; }
        public string OriginalUrl { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public GetQrCodeUmtParameterDto(int id, string? umtSource, string? umtMedium, string? umtCampaign, int urlId, string originalUrl, string code)
        {
            Id = id;
            UmtSource = umtSource;
            UmtMedium = umtMedium;
            UmtCampaign = umtCampaign;
            UrlId = urlId;
            OriginalUrl = originalUrl;
            Code = code;
        }
    }
}
