using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Shared.DTO
{
    public record class CreateUrlDto(string OriginalUrl, string? CustomCode, IEnumerable<string>? Tags, List<UmtParameterDto>? UmtParameters);
}
