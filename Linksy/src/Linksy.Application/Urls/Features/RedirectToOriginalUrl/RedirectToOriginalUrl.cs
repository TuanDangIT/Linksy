using Linksy.Application.Abstractions;
using Linksy.Application.Urls.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.RedirectToOriginalUrl
{
    public sealed record class RedirectToOriginalUrl(string Code) : IQuery<RedirectOriginalUrlDto>;
}
