using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.PublishLandingPage
{
    public record class PublishLandingPage(int LandingPageId, bool IsPublished) : ICommand;
}
