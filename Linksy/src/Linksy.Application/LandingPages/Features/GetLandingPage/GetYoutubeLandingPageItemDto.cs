using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.GetLandingPage
{
    public record class GetYoutubeLandingPageItemDto : GetLandingPageItemDto
    {
        public string VideoUrl { get; init; } = string.Empty;
        public GetYoutubeLandingPageItemDto(int id, string type, int order, int clickCount, DateTime createdAt, DateTime? updatedAt, string videoUrl) : 
            base(id, type, order, clickCount, createdAt, updatedAt)
        {
            VideoUrl = videoUrl;
        }
    }
}
