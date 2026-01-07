using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.GetPublishedLandingPage
{
    public record class GetPublishedYoutubeLandingPageItemDto : GetPublishedLandingPageItemDto
    {
        public string VideoUrl { get; init; } = string.Empty;
        public GetPublishedYoutubeLandingPageItemDto(int id, string type, int order, string videoUrl) : base(id, type, order)
        {
            VideoUrl = videoUrl;
        }
    }
}
