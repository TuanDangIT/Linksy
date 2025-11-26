using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.GetLandingPage
{
    public record class GetTextLandingPageItemDto : GetLandingPageItemDto
    {
        public string Content { get; init; } = string.Empty;
        public string BackgroundColor { get; init; } = string.Empty;
        public string FontColor { get; init; } = string.Empty;
        public GetTextLandingPageItemDto(string type, int order, int clickCount, DateTime createdAt, DateTime? updatedAt,
            string content, string backgroundColor, string fontColor) : base(type, order, clickCount, createdAt, updatedAt)
        {
            Content = content;
            BackgroundColor = backgroundColor;
            FontColor = fontColor;
        }
    }
}
