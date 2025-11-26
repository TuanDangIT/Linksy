using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.PublishLandingPage
{
    internal class PublishLandingPageValidator : AbstractValidator<PublishLandingPage>
    {
        public PublishLandingPageValidator()
        {
            RuleFor(p => p.LandingPageId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(p => p.IsPublished)
                .NotNull();
        }
    }
}
