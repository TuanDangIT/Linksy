using Linksy.Application.QrCodes.Features.BrowseQrCodes;
using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.BrowseBarcodes
{
    public record class BrowseBarcodeDto
    {
        public int Id { get; init; }
        public bool IsActive { get; init; }
        public IEnumerable<string>? Tags { get; init; } 
        public BrowseScanCodesUrlDto Url { get; init; } = default!;
        public int ScanCount { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public BrowseBarcodeDto(int id, BrowseScanCodesUrlDto url, bool isActive, IEnumerable<string>? tags, int scanCount, DateTime createdAt, DateTime? updatedAt)
        {
            Id = id;
            IsActive = isActive;
            Url = url;
            Tags = tags;
            ScanCount = scanCount;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}

