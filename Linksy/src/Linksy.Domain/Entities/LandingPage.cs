using Linksy.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities
{
    public class LandingPage : BaseEntityWithMultitenancy, IAuditable
    {
        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public string? ImageUrlPath { get; private set; }
        public string? BackgroundColor { get; private set; }
        public string? BackgroundImageUrl { get; private set; }
        public Url Url { get; private set; } = default!;
        public int UrlId { get; private set; }
        private readonly List<LandingPageItem>? _landingPageItems = [];
        public IEnumerable<LandingPageItem>? LandingPageItems => _landingPageItems;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        private LandingPage(Url url, IEnumerable<LandingPageItem>? landingPageItems, string title, string? description, string? imageUrlPath, 
            string? backgroundColor, string? backgroundImageUrl)
        {
            Url = url;
            _landingPageItems = landingPageItems?.ToList();
            Title = title;
            Description = description;
            ImageUrlPath = imageUrlPath;
            BackgroundColor = backgroundColor;
            BackgroundImageUrl = backgroundImageUrl;
        }
        private LandingPage() { }
        public static LandingPage CreateLandingPage(Url url, IEnumerable<LandingPageItem> landingPageItems, string title, string? description, 
            string? imageUrlPath, string? backgroundColor, string? backgroundImageUrl)
            => new(url, landingPageItems, title, description, imageUrlPath, backgroundColor, backgroundImageUrl);   
    }
}
