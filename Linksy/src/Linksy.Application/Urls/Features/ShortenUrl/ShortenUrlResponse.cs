using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.ShortenUrl
{
    public record class ShortenUrlResponse(int UrlId, string ShortenedUrl);
}
