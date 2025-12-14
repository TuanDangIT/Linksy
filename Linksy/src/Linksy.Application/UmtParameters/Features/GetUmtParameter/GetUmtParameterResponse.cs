using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.GetUmtParameter
{
    public record class GetUmtParameterResponse
    {
        public int Id { get; init; }
        public bool IsActive { get; init; }
        public string? UmtSource { get; init; }
        public string? UmtMedium { get; init; }
        public string? UmtCampaign { get; init; }
        public int VisitsCount { get; init; }
        public GetUmtParameterUrlDto Url { get; init; } = default!;
        public GetUmtParameterQrCodeDto? QrCode { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public GetUmtParameterResponse(int id, bool isActive, string? umtSource, string? umtMedium, string? umtCampaign, int visitsCount, 
            GetUmtParameterUrlDto url, GetUmtParameterQrCodeDto? qrCode, DateTime createdAt, DateTime? updatedAt)
        {
            Id = id;
            IsActive = isActive;
            UmtSource = umtSource;
            UmtMedium = umtMedium;
            UmtCampaign = umtCampaign;
            VisitsCount = visitsCount;
            Url = url;
            QrCode = qrCode;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
