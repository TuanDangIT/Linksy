using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Features.DeleteLandingPageItem
{
    internal class DeleteLandingPageItemValidator : AbstractValidator<DeleteLandingPageItem>
    {
        public DeleteLandingPageItemValidator()
        {
            RuleFor(d => d.LandingPageItemId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
