using Linksy.Domain.Abstractions;
using Linksy.Domain.Entities.Tracking;
using Linksy.Domain.Exceptions;
using Linksy.Domain.ValueObjects;
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
        public bool IsPublished { get; private set; } = false;
        public int EngagementCount { get; set; } = 0;
        public int ViewCount { get; set; } = 0;
        public string Title { get; private set; } = string.Empty;
        public string TitleFontColor { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public string? DescriptionFontColor { get; private set; }
        public Image? LogoImage { get; private set; }
        public string? BackgroundColor { get; private set; }
        public Image? BackgroundImage { get; private set; }
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
        private LandingPage(string code, string title, string titleFontColor, string? description, string? descriptionFontColor, Image? logoImage, 
            string? backgroundColor, Image? backgroundImage, IEnumerable<string>? tags, int userId) : base(userId)
        {
            if (backgroundColor is null && backgroundImage is null)
                throw new BackgroundRequiredException();
            if (backgroundColor is not null && backgroundImage is not null)
                throw new BackgroundConflictException();

            Code = code;
            Title = title;
            Description = description;
            TitleFontColor = titleFontColor;
            DescriptionFontColor = descriptionFontColor;
            LogoImage = logoImage;
            BackgroundColor = backgroundColor;
            BackgroundImage = backgroundImage;
            SetTags(tags ?? []);
        }
        private LandingPage(string title, string titleFontColor, string? description, string? descriptionFontColor,
            Image? logoImage, string? backgroundColor, Image? backgroundImage, IEnumerable<string>? tags, DateTime createdAt, IEnumerable<LandingPageItem> landingPageItems)
        {
            Title = title;
            Description = description;
            TitleFontColor = titleFontColor;
            DescriptionFontColor = descriptionFontColor;
            LogoImage = logoImage;
            BackgroundColor = backgroundColor;
            BackgroundImage = backgroundImage;
            SetTags(tags ?? []);
            CreatedAt = createdAt;
            _landingPageItems = [.. landingPageItems];
        }
        private LandingPage() { }
        public static LandingPage CreateLandingPage(string code, string title, string titleFontColor, string? description, string? descriptionFontColor,
            Image? logoImage, string? backgroundColor, Image? backgroundImage, IEnumerable<string>? tags, int userId)
            => new(code/*, landingPageItems*/, title, titleFontColor, description, descriptionFontColor, logoImage, backgroundColor, backgroundImage, tags, userId);

        public static LandingPage CreatePublishedLandingPage(string title, string titleFontColor, string? description, string? descriptionFontColor,
            Image? logoImage, string? backgroundColor, Image? backgroundImage, IEnumerable<string>? tags, DateTime createdAt, IEnumerable<LandingPageItem> landingPageItems)
            => new(title, titleFontColor, description, descriptionFontColor, logoImage, backgroundColor, backgroundImage, tags, createdAt, landingPageItems);

        //public static LandingPage CreateGetLandingPage(string code, bool isPublished, int engagementCount, int viewCount, string title, string titleFontColor, string? description,
        //    string? descriptionFontColor, Image? logoImage, string? backgroundColor, Image? backgroundImage, IEnumerable<string>? tags, DateTime createdAt, DateTime? updatedAt,
        //    IEnumerable<LandingPageItem> landingPageItems)
        //{
        //    var landingPage = new LandingPage(title, titleFontColor, description, descriptionFontColor, logoImage, backgroundColor, backgroundImage, tags, createdAt, landingPageItems)
        //    {
        //        Code = code,
        //        IsPublished = isPublished,
        //        EngagementCount = engagementCount,
        //        ViewCount = viewCount,
        //        CreatedAt = createdAt,
        //        UpdatedAt = updatedAt
        //    };
        //    return landingPage;
        //}
        public void Publish()
            => IsPublished = true;
        public void Unpublish()
            => IsPublished = false;
        public void AddEngagement(LandingPageEngagement engagement)
        {
            IncrementEngagementCount();
            _engagements.Add(engagement);
        }
        public void AddView(LandingPageView view)
        {
            IncrementViewCount();
            _views.Add(view);
        }
        public void IncrementEngagementCount()
            => EngagementCount++;
        public void IncrementViewCount()
            => ViewCount++;
        public void SetTags(IEnumerable<string> tags)
        {
            if(tags.Count() >= 8)
                throw new TagsLengthExceededException();
            TagsList = [.. tags];
        }
        public void AddLandingPageItem<T>(T landingPageItem) where T : LandingPageItem
        {
            var nextOrder = _landingPageItems.Count != 0
                ? _landingPageItems.Max(item => item.Order) + 1
                : 1;
            landingPageItem.SetOrder(nextOrder);
            _landingPageItems.Add(landingPageItem);
        }

    }
}
