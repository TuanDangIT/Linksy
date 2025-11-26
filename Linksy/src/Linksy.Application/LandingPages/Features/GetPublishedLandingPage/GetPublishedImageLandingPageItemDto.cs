using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.GetPublishedLandingPage
{
    public record class GetPublishedImageLandingPageItemDto : GetPublishedLandingPageItemDto
    {
        public string ImageUrl { get; init; } = string.Empty;
        public string AltText { get; init; } = string.Empty;
        public string? Url { get; init; } = string.Empty;
        public GetPublishedImageLandingPageItemDto(string type, int order, string imageUrl, string altText, string? Url) : base(type, order)
        {
            ImageUrl = imageUrl;
            AltText = altText;
            this.Url = Url;
        }
    }
}
