using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.AddLandingPageEngagement
{
    public record class AddLandingPageEngagement(int LandingPageId, int LandingPageItemId, string? IpAddress) : ICommand;
}
