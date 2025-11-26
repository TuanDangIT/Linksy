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
        private TextLandingPageItem(LandingPageItemType type, string content, string backgroundColor, string fontColor, int userId) : base(type, userId)
        {
            Content = content;
            BackgroundColor = backgroundColor;
            FontColor = fontColor;
        }
        private TextLandingPageItem() : base()
        {
            
        }
        public static TextLandingPageItem CreateTextLandingPageItem(LandingPageItemType type, string content, string backgroundColor, string fontColor, int userId)
            => new(type, content, backgroundColor, fontColor, userId);
    }
}
