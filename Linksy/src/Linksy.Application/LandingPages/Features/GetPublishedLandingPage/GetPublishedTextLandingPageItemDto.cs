using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.GetPublishedLandingPage
{
    public record class GetPublishedTextLandingPageItemDto : GetPublishedLandingPageItemDto
    {
        public string Content { get; init; } = string.Empty;
        public string BackgroundColor { get; init; } = string.Empty;
        public string FontColor { get; init; } = string.Empty;
        public GetPublishedTextLandingPageItemDto(string type, int order, string content, string backgroundColor, string fontColor) : base(type, order)
        {
            Content = content;
            BackgroundColor = backgroundColor;
            FontColor = fontColor;
        }
    }
}
