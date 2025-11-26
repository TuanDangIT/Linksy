using Linksy.Application.Abstractions;
using Linksy.Application.Shared.DTO;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Features.AddImageLandingPageItem
{
    public record class AddImageLandingPageItem : AddLandingPageItemDto, ICommand<AddImageLandingPageItemResponse>
    {
        public IFormFile Image { get; init; } = default!;
        public string AltText { get; init; } = string.Empty;
        public int? UrlId { get; init; }
    }
}
