using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.DeleteLandingPage
{
    internal class DeleteLandingPageValidator : AbstractValidator<DeleteLandingPage>
    {
        public DeleteLandingPageValidator()
        {
            RuleFor(d => d.LandingPageId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
