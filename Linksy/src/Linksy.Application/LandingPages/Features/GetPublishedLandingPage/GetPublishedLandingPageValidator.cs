using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.GetPublishedLandingPage
{
    internal class GetPublishedLandingPageValidator : AbstractValidator<GetPublishedLandingPage>    
    {
        public GetPublishedLandingPageValidator()
        {
            RuleFor(g => g.Code)
                .NotEmpty()
                .NotNull();
        }
    }
}
