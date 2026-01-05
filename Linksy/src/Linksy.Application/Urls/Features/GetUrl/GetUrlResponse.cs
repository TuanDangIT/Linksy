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
        public IEnumerable<string>? Tags { get; init; }
        public GetUrlQrCodeDto? QrCode { get; init; }
        public GetUrlBarcodeDto? Barcode { get; init; }
        public IEnumerable<GetUrlLandingPageItemDto>? LandingPageItems { get; init; }
        public IEnumerable<GetUrlUmtParameterDto>? UmtParameters { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public GetUrlResponse(int id, string originalUrl, string code, int visitCount, bool isActive, IEnumerable<string>? tags, GetUrlQrCodeDto? qrCode, GetUrlBarcodeDto? barcode,
            IEnumerable<GetUrlLandingPageItemDto>? landingPageItems, IEnumerable<GetUrlUmtParameterDto>? umtParameters, DateTime createdAt, DateTime? updatedAt)
        {
            Id = id;
            OriginalUrl = originalUrl;
            Code = code;
            VisitCount = visitCount;
            IsActive = isActive;
            Tags = tags;
            QrCode = qrCode;
            Barcode = barcode;
            LandingPageItems = landingPageItems;
            UmtParameters = umtParameters?.ToArray();
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
