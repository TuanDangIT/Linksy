using Linksy.Application.Abstractions;
using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Features.AddTextLandingPageItem
{
    public record class AddTextLandingPageItem : AddLandingPageItemDto, ICommand<AddLandingPageResponse>
    {
        public string Content { get; init; } = string.Empty;
        public string BackgroundColor { get; init; } = string.Empty;
        public string FontColor { get; init; } = string.Empty;
    }
}
