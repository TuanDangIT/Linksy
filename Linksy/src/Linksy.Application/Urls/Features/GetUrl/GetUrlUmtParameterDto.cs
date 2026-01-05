using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.GetUrl
{
    public record class GetUrlUmtParameterDto
    {
        public int Id { get; init; }
        public string? UmtSource { get; init; } = string.Empty;
        public string? UmtMedium { get; init; } = string.Empty;
        public string? UmtCampaign { get; init; } = string.Empty;
        public int VisitCount { get; init; }    
        public int? QrCodeId { get; init; }  
        public int? QrCodeScanCount { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public GetUrlUmtParameterDto(int id, string? umtSource, string? umtMedium, string? umtCampaign, int visitCount, int? qrCodeId, int? qrCodeScanCount, DateTime createdAt, DateTime? updatedAt)
        {
            Id = id;
            UmtSource = umtSource;
            UmtMedium = umtMedium;
            UmtCampaign = umtCampaign;
            VisitCount = visitCount;
            QrCodeId = qrCodeId;
            QrCodeScanCount = qrCodeScanCount;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
