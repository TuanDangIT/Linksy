using Linksy.Domain.Enums;
using Linksy.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.LandingPage
{
    public class ImageLandingPageItem : LandingPageItem
    {
        public Image Image { get; private set; } = default!;
        public string AltText { get; private set; } = string.Empty;
        public Url.Url? Url { get; private set; } 
        public int? UrlId { get; private set; }
        private ImageLandingPageItem(LandingPageItemType type, Image image, string altText, int userId) : base(type, userId)
        {
            Type = type;
            Image = image;
            AltText = altText;
        }
        private ImageLandingPageItem(LandingPageItemType type, Image image, string altText, Url.Url? url, int userId) : this(type, image, altText, userId)
        {
            Url = url;
        }
        private ImageLandingPageItem() : base()
        {
            
        }
        public static ImageLandingPageItem CreateImageLandingPageItem(LandingPageItemType type, Image image, string altText, Url.Url? url, int userId)
            => new(type, image, altText, url, userId);
        public static ImageLandingPageItem CreateImageLandingPageItem(LandingPageItemType type, Image image, string altText, int userId)
            => new(type, image, altText, userId);
    }
}
