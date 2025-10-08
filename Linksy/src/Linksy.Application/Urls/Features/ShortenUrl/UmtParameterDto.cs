using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.ShortenUrl
{
    public record class UmtParameterDto(string? UmtSource, string? UmtMedium, string? UmtCampaign);
}
