using Linksy.Application.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.GetPublishedLandingPage
{
    public record class GetPublishedLandingPage(string Code, string? IpAddress) : IQuery<GetPublishedLandingPageResponse?>;
}
