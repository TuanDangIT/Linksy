using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.BrowseLandingPages
{
    public record class BrowseLandingPageDto
    {
        public int Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public bool IsPublished { get; init; }
        public int EngagementCount { get; init; }
        public int ViewCount { get; init; }
        public string Title { get; init; } = string.Empty;
        public IEnumerable<string>? Tags { get; init; } 
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public BrowseLandingPageDto(int id, string code, bool isPublished, int engagementCount, int viewCount, string title, IEnumerable<string>? tags, DateTime createdAt, DateTime? updatedAt)
        {
            Id = id;
            Code = code;
            IsPublished = isPublished;
            EngagementCount = engagementCount;
            ViewCount = viewCount;
            Title = title;
            Tags = tags;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
