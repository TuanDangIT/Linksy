using Linksy.Application.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.SetActiveStatus
{
    public record class SetActiveStatus : ICommand
    {
        [SwaggerIgnore]
        public int UrlId { get; init; }
        public bool IsActive { get; init; }
        public SetActiveStatus(int urlId, bool isActive)
        {
            UrlId = urlId;
            IsActive = isActive;
        }
    }
}
