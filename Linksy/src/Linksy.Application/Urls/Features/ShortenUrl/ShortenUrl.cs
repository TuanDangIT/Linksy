using Linksy.Application.Abstractions;
using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.ShortenUrl
{
    public sealed record ShortenUrl(string OriginalUrl, string? CustomCode, IEnumerable<string> Tags, IEnumerable<UmtParameterDto>? UmtParameters) : ICommand<ShortenUrlResponse>;
}
