using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Shared.DTO
{
    public record class UmtParameterDto(string? UmtSource, string? UmtMedium, string? UmtCampaign);
}
