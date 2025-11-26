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
        public string Url { get; init; } = string.Empty;
        public GetPublishedUrlLandingPageItemDto(string type, int order, string content, string backgroundColor, string fontColor, string url) : base(type, order)
        {
            Content = content;
            BackgroundColor = backgroundColor;
            FontColor = fontColor;
            Url = url;
        }
    }
}
