using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.GetLandingPage
{
    internal class GetLandingPageValidator : AbstractValidator<GetLandingPage>
    {
        public GetLandingPageValidator()
        {
            RuleFor(g => g.LandingPageId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
        }
    }
}
