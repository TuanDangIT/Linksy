using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.GetLandingPage
{
    public abstract record class GetLandingPageItemDto
    {
        public int Id { get; init; }
        public string Type { get; init; } = string.Empty;
        public int Order { get; init; }
        public int ClickCount { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public GetLandingPageItemDto(int id, string type, int order, int clickCount, DateTime createdAt, DateTime? updatedAt)
        {
            Id = id;
            Type = type;
            Order = order;
            ClickCount = clickCount;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
