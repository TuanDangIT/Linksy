using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Shared.DTO
{
    public record class ScanCodeDto
    {
        public int Id { get; init; }
        public int ScanCount { get; init; }
        public ImageDto Image { get; init; } = default!;
        public IEnumerable<string>? Tags { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public ScanCodeDto(int id, int scanCount, ImageDto image, IEnumerable<string>? tags, DateTime createdAt, DateTime? updatedAt)
        {
            Id = id;
            ScanCount = scanCount;
            Image = image;
            Tags = tags;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
