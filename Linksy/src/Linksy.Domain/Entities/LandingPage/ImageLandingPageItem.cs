using Linksy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.LandingPage
{
    public class ImageLandingPageItem : LandingPageItem
    {
        public string ImageUrlPath { get; private set; } = string.Empty;
        public string AltText { get; private set; } = string.Empty;
        public Url.Url? Url { get; private set; } 
        public int? UrlId { get; private set; }
        private ImageLandingPageItem(LandingPageItemType type, LandingPage landingPage, int order, string imageUrlPath, string altText, Url.Url? url) : base(type, landingPage, order)
        {
            ImageUrlPath = imageUrlPath;
            AltText = altText;
            Url = url;
        }
        private ImageLandingPageItem() : base()
        {
            
        }
        public static ImageLandingPageItem CreateImageLandingPageItem(LandingPageItemType type, LandingPage landingPage, int order, string imageUrlPath, string altText, Url.Url? url)
            => new(type, landingPage, order, imageUrlPath, altText, url);
    }
}
