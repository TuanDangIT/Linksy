using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.GetUrl
{
    public record class GetUrlResponse
    {
        public int Id { get; init; }
        public string OriginalUrl { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public int VisitCount { get; init; }
        public bool IsActive { get; init; }
        public GetUrlQrCodeDto? QrCode { get; init; }
        public GetUrlBarcodeDto? Barcode { get; init; }
        public GetUrlLandingPageDto? LandingPage { get; init; }
        public GetUrlLandingPageItemDto? LandingPageItem { get; init; }
        public GetUrlUmtParameterDto[]? UmtParameters { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public GetUrlResponse(int id, string originalUrl, string code, int visitCount, bool isActive, GetUrlQrCodeDto? qrCode, GetUrlBarcodeDto? barcode, GetUrlLandingPageDto? landingPage,
            GetUrlLandingPageItemDto? landingPageItem, IEnumerable<GetUrlUmtParameterDto>? umtParameters)
        {
            Id = id;
            OriginalUrl = originalUrl;
            Code = code;
            VisitCount = visitCount;
            IsActive = isActive;
            QrCode = qrCode;
            Barcode = barcode;
            LandingPage = landingPage;
            LandingPageItem = landingPageItem;
            UmtParameters = umtParameters?.ToArray();
        }
    }
}
