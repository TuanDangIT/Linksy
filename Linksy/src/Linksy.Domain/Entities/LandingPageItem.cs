using Linksy.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities
{
    public class LandingPageItem : BaseEntity, IAuditable
    {
        public string Content { get; private set; } = string.Empty;
        public string BackgroundColor { get; private set; } = string.Empty;
        public string FontColor { get; private set; } = string.Empty;
        public int Order { get; private set; }
        public Url? Url { get; private set; } = default!;
        public int? UrlId { get; private set; }
        public QrCode? QrCode { get; private set; } = default!;
        public int? QrCodeId { get; private set; }
        public LandingPage LandingPage { get; private set; } = default!;
        public int LandingPageId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        private LandingPageItem(Url? url, QrCode? qrCode, LandingPage landingPage, string content, string fontColor, string backgroundColor, int order)
        {
            Url = url;
            QrCode = qrCode;
            LandingPage = landingPage;
            Content = content;
            Order = order;
            FontColor = fontColor;
            BackgroundColor = backgroundColor;
        }
        private LandingPageItem() { }
        public static LandingPageItem CreateLandingPageItem(Url? url, QrCode? qrCode, LandingPage landingPage, string content, string fontColor, string backgroundColor, int order)
            => new(url, qrCode, landingPage, content, fontColor, backgroundColor, order);
    }
}
