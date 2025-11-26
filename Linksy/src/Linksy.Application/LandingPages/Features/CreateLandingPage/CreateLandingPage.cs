using Linksy.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.CreateLandingPage
{
    public record class CreateLandingPage : ICommand<CreateLandingPageResponse>
    {
        public string Code { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string TitleFontColor { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string? DescriptionFontColor { get; init; }
        public IFormFile? LogoImage { get; init; }
        public string? BackgroundColor { get; init; }
        public IFormFile? BackgroundImage { get; init; }
        public IEnumerable<string>? Tags { get; init; } = null;
    }
}
