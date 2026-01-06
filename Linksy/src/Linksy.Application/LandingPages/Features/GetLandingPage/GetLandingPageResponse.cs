using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.GetLandingPage
{
    public record class GetLandingPageResponse
    {
        public int Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public bool IsActive { get; init; } = false;
        public int EngagementCount { get; init; } 
        public int ViewCount { get; init; } 
        public string Title { get; init; } = string.Empty;
        public string TitleFontColor { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string? DescriptionFontColor { get; init; }
        public ImageDto? LogoImage { get; init; }
        public string? BackgroundColor { get; init; }
        public ImageDto? BackgroundImage { get; init; }
        public IEnumerable<string>? Tags { get; init; }
        public IEnumerable<object>? LandingPageItems { get; init; } 
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public GetLandingPageResponse(int id, string code, bool isActive, int engagementCount, int viewCount, string title, string titleFontColor, string? description, 
            string? descriptionFontColor, ImageDto? logoImage, string? backgroundColor, ImageDto? backgroundImage, IEnumerable<string>? tags, DateTime createdAt, DateTime? updatedAt, IEnumerable<object> landingPageItems) 
        {
            Id = id;
            Title = title;
            Code = code;
            IsActive = isActive;
            EngagementCount = engagementCount;
            ViewCount = viewCount;
            TitleFontColor = titleFontColor;
            Description = description;
            DescriptionFontColor = descriptionFontColor;
            LogoImage = logoImage;
            BackgroundColor = backgroundColor;
            BackgroundImage = backgroundImage;
            Tags = tags;
            LandingPageItems = landingPageItems;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
