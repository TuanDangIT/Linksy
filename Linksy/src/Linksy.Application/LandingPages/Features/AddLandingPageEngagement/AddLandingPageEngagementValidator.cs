using FluentValidation;
using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.AddLandingPageEngagement
{
    internal class AddLandingPageEngagementValidator : AbstractValidator<AddLandingPageEngagement> {
        public AddLandingPageEngagementValidator()
        {
            RuleFor(a => a.LandingPageId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(a => a.LandingPageItemId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
