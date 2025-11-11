using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.BrowseQrCodes
{
    public record class BrowseQrCodeDto
    {
        public int Id { get; init; }
        public bool IsActive { get; init; }
        public IEnumerable<string>? Tags { get; init; }
        public BrowseScanCodeUrlDto Url { get; init; } = default!;
        public bool HasUmtParameters { get; init; }
        public int ScanCount { get; init; } 
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public BrowseQrCodeDto(int id, BrowseScanCodeUrlDto url, bool hasUmtParameters, bool isActive, IEnumerable<string>? tags, int scanCount, DateTime createdAt, DateTime? updatedAt)
        {
            Id = id;
            IsActive = isActive;
            Url = url;
            Tags = tags;
            HasUmtParameters = hasUmtParameters;
            ScanCount = scanCount;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
