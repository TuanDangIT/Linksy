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
        public int VisitCount { get; private set; } = 0;
        public string Code { get; private set; } = string.Empty;
        private readonly List<LandingPageItem> _landingPageItems = [];
        public IEnumerable<LandingPageItem> LandingPageItems => _landingPageItems;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public LandingPage(string code, IEnumerable<LandingPageItem> landingPageItems, string title, string? description, string? imageUrlPath, string? backgroundColor, string? backgroundImageUrl)
        {
            Code = code;
            _landingPageItems = landingPageItems.ToList();
            Title = title;
            Description = description;
            ImageUrlPath = imageUrlPath;
            BackgroundColor = backgroundColor;
            BackgroundImageUrl = backgroundImageUrl;
        }
        private LandingPage() { }
        public void IncrementVisits() 
            => VisitCount++;
    }
}
