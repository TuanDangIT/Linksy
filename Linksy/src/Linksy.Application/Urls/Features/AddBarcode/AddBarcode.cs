using Linksy.Application.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.AddBarcode
{
    public record class AddBarcode : ICommand<AddBarcodeResponse>
    {
        [SwaggerIgnore]
        public int UrlId { get; init; }
        public IEnumerable<string>? Tags { get; init; } = [];
    }
}
