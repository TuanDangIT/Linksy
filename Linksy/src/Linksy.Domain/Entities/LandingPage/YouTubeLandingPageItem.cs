using Linksy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.LandingPage
{
    public class YouTubeLandingPageItem : LandingPageItem
    {
        public string VideoUrl { get; set; } = string.Empty;
        private YouTubeLandingPageItem(LandingPageItemType type, string videoUrl, int userId) : base(type, userId)
        {
            VideoUrl = videoUrl;
        }
        private YouTubeLandingPageItem() : base()
        {
        }
        public static YouTubeLandingPageItem CreateYouTubeLandingPageItem(LandingPageItemType type, string videoUrl, int userId)
            => new(type, videoUrl, userId);
    }
}
