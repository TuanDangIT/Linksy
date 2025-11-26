using Linksy.Domain.Entities.Url;
using Linksy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.LandingPage
{
    public class UrlLandingPageItem : LandingPageItem
    {
        public string Content { get; private set; } = string.Empty;
        public string BackgroundColor { get; private set; } = string.Empty;
        public string FontColor { get; private set; } = string.Empty;
        public Url.Url Url { get; private set; } = default!;
        public int UrlId { get; private set; }
        private UrlLandingPageItem(LandingPageItemType type, string content, string fontColor, string backgroundColor, Url.Url url, int userId) : base(type, userId)
        {
            Content = content;
            FontColor = fontColor;
            BackgroundColor = backgroundColor;
            Url = url;
        }
        private UrlLandingPageItem() : base()
        {
        }
        public static UrlLandingPageItem CreateUrlLandingPageItem(LandingPageItemType type, string content, string fontColor, string backgroundColor, Url.Url url, int userId)
            => new(type, content, fontColor, backgroundColor, url, userId);
    }
}
