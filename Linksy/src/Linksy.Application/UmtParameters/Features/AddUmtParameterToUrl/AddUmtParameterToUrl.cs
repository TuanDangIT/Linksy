using Linksy.Application.Abstractions;
using Linksy.Application.Shared.DTO;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.AddUmtParameterToUrl
{
    public record class AddUmtParameterToUrl : ICommand<AddUmtParameterToUrlResponse>
    {
        [SwaggerIgnore]
        public int UrlId { get; init; }
        public UmtParameterDto UmtParameter { get; init; } = default!;
    }
}
