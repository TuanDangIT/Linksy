using Linksy.Application.Abstractions;
using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Features.AddYouTubeLandingPageItem
{
    public record class AddYouTubeLandingPageItem : AddLandingPageItemDto, ICommand<AddLandingPageResponse>
    {
        public string YouTubeUrl { get; init; } = string.Empty;
    }
}
