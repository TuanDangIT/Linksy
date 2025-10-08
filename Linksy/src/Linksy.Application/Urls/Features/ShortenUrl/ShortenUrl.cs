using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.ShortenUrl
{
    public sealed record ShortenUrl(string OriginalUrl, string? CustomCode, List<UmtParameterDto>? UmtParameters) : ICommand<ShortenedUrlDto>;
}
