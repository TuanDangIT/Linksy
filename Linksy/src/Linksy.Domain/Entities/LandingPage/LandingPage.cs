using Linksy.Domain.Abstractions;
using Linksy.Domain.Entities.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.LandingPage
{
    public class LandingPage : BaseEntityWithMultitenancy, IAuditable
    {
        public string Code { get; private set; } = string.Empty;
        public bool IsActive { get; private set; } = false;
        public int EngagementCount { get; set; } = 0;
        public int ViewCount { get; set; } = 0;
        public string Title { get; private set; } = string.Empty;
        public string TitleFontColor { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public string? DescriptionFontColor { get; private set; }
        public string? ImageUrlPath { get; private set; }
        public string? BackgroundColor { get; private set; }
        public string? BackgroundImageUrl { get; private set; }
        public string? Tags { get; private set; } = string.Empty;
        public IEnumerable<string>? TagsList
        {
            get => Tags?.Split(',') ?? [];
            set => Tags = value != null ? string.Join(',', value) : null;
        }
        private readonly List<LandingPageItem> _landingPageItems = [];
        public IEnumerable<LandingPageItem> LandingPageItems => _landingPageItems;
        private readonly List<LandingPageEngagement> _engagements = [];
        public IEnumerable<LandingPageEngagement> Engagements => _engagements;
        private readonly List<LandingPageView> _views = [];
        public IEnumerable<LandingPageView> Views => _views;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        private LandingPage(string code, IEnumerable<LandingPageItem> landingPageItems, string title, string titleFontColor, string? description, string? descriptionFontColor, string? imageUrlPath, 
            string? backgroundColor, string? backgroundImageUrl, IEnumerable<string>? tags)
        {
            Code = code;
            _landingPageItems = [.. landingPageItems];
            Title = title;
            Description = description;
            TitleFontColor = titleFontColor;
            DescriptionFontColor = descriptionFontColor;
            ImageUrlPath = imageUrlPath;
            BackgroundColor = backgroundColor;
            BackgroundImageUrl = backgroundImageUrl;
            TagsList = tags?.ToList();
        }
        private LandingPage() { }
        public static LandingPage CreateLandingPage(string code, IEnumerable<LandingPageItem> landingPageItems, string title, string titleFontColor, string? description, string descriptionFontColor,
            string? imageUrlPath, string? backgroundColor, string? backgroundImageUrl, IEnumerable<string>? tags)
            => new(code, landingPageItems, title, titleFontColor, description, descriptionFontColor, imageUrlPath, backgroundColor, backgroundImageUrl, tags);   
        public void SetActive(bool isActive)
            => IsActive = isActive;
        public void AddEngagement(LandingPageEngagement engagement)
            => _engagements.Add(engagement);
        public void AddView(LandingPageView view)
            => _views.Add(view);
        public void IncrementEngagementCount()
            => EngagementCount++;
        public void IncrementViewCount()
            => ViewCount++;
        public void UpdateTags(IEnumerable<string> tags)
            => TagsList = [.. tags];

    }
}
