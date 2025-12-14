using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.GetUmtParameter
{
    public record class GetUmtParameterUrlDto
    {
        public int Id { get; init; }
        public string OriginalUrl { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public GetUmtParameterUrlDto(int id, string originalUrl, string code)
        {
            Id = id;
            OriginalUrl = originalUrl;
            Code = code;
        }
    }
}
