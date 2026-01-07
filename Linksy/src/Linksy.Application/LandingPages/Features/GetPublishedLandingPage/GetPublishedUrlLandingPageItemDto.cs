using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.GetPublishedLandingPage
{
    public record class GetPublishedUrlLandingPageItemDto : GetPublishedLandingPageItemDto
    {
        public string Content { get; init; } = string.Empty;
        public string BackgroundColor { get; init; } = string.Empty;
        public string FontColor { get; init; } = string.Empty;
        public string UrlCode { get; init; } = string.Empty;
        public GetPublishedUrlLandingPageItemDto(int id, string type, int order, string content, string backgroundColor, string fontColor, string urlCode) : base(id, type, order)
        {
            Content = content;
            BackgroundColor = backgroundColor;
            FontColor = fontColor;
            UrlCode = urlCode;
        }
    }
}
