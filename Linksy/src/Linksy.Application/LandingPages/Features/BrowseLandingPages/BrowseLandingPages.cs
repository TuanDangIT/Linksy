using Linksy.Application.Abstractions;
using Linksy.Application.Shared.ModelBinders;
using Linksy.Application.Shared.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.BrowseLandingPages
{
    public record class BrowseLandingPages : Browse<BrowseLandingPagesResponse>
    {
    }
}
