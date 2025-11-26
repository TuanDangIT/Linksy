using Linksy.Application.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.AddQrCodeToUmtParameter
{
    public record class AddQrCodeToUmtParameter : ICommand<AddQrCodeToUmtParameterResponse>
    {
        [NotMapped]
        [SwaggerIgnore]
        public int UmtParameterId { get; init; }
        public IEnumerable<string>? Tags { get; init; } = [];
    }
}
