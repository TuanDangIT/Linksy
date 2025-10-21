using Linksy.Application.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.ChangeOriginalUrl
{
    public sealed record class ChangeOriginalUrl : ICommand
    {
        [SwaggerIgnore]
        public int UrlId { get; init; }
        public string OriginalUrl { get; init; } = string.Empty;
    }
}
