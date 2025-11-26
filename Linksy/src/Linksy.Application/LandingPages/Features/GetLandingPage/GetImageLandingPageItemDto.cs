using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.GetLandingPage
{
    public record class GetImageLandingPageItemDto : GetLandingPageItemDto
    {
        public string ImageUrl { get; init; } = string.Empty;
        public string AltText { get; init; } = string.Empty;
        public string? Url { get; init; } = string.Empty;
        public GetImageLandingPageItemDto(string type, int order, int clickCount, DateTime createdAt, DateTime? updatedAt,
            string imageUrl, string altText, string? url) : base(type, order, clickCount, createdAt, updatedAt)
        {
            ImageUrl = imageUrl;
            AltText = altText;
            Url = url;
        }
    }
}
