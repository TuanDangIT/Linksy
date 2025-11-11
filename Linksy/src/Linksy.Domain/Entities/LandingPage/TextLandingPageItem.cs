using Linksy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.LandingPage
{
    public class TextLandingPageItem : LandingPageItem
    {
        public string Content { get; private set; } = string.Empty;
        public string BackgroundColor { get; private set; } = string.Empty;
        public string FontColor { get; private set; } = string.Empty;
        private TextLandingPageItem(LandingPageItemType type, LandingPage landingPage, int order, string content, string backgroundColor, string fontColor) : base(type, landingPage, order)
        {
            Content = content;
            BackgroundColor = backgroundColor;
            FontColor = fontColor;
        }
        private TextLandingPageItem() : base()
        {
            
        }
        public static TextLandingPageItem CreateTextLandingPageItem(LandingPageItemType type, LandingPage landingPage, int order, string content, string backgroundColor, string fontColor)
            => new(type, landingPage, order, content, backgroundColor, fontColor);
    }
}
