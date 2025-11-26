using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Shared.DTO
{
    public record class AddLandingPageItemDto
    {
        [SwaggerIgnore]
        public int LandingPageId { get; init; }
    }
}
