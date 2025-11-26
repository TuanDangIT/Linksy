using Linksy.Application.Abstractions;
using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.GetPublishedLandingPage
{
    public record class GetPublishedLandingPageResponse
    {
        public string Title { get; init; } = string.Empty;
        public string TitleFontColor { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string? DescriptionFontColor { get; init; }
        public ImageDto? LogoImage { get; init; }
        public string? BackgroundColor { get; init; }
        public ImageDto? BackgroundImage { get; init; }
        public IEnumerable<string>? Tags { get; init; }
        public IEnumerable<object> LandingPageItems { get; init; } = [];
        public DateTime CreatedAt { get; init; }
        public GetPublishedLandingPageResponse(string title, string titleFontColor, string? description, string? descriptionFontColor, 
            ImageDto? logoImage, string? backgroundColor, ImageDto? backgroundImage, IEnumerable<string>? tags, DateTime createdAt, IEnumerable<object> landingPageItems)
        {
            Title = title;
            TitleFontColor = titleFontColor;
            Description = description;
            DescriptionFontColor = descriptionFontColor;
            LogoImage = logoImage;
            BackgroundColor = backgroundColor;
            BackgroundImage = backgroundImage;
            Tags = tags;
            LandingPageItems = landingPageItems;
            CreatedAt = createdAt;
        }
    }
}
