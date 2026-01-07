using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Features.AddUrlLandingPageItem
{
    internal class AddUrlLandingPageItemValidator : AbstractValidator<AddUrlLandingPageItem>
    {
        public AddUrlLandingPageItemValidator()
        {
            RuleFor(a => a.LandingPageId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
            RuleFor(a => a.Content)
                .NotNull()
                .NotEmpty()
                .MaximumLength(128);
            RuleFor(a => a.BackgroundColor)
                .NotNull()
                .NotEmpty()
                .MaximumLength(16)
                .Matches(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3}|[A-Fa-f0-9]{8})$")
                .WithMessage("Must be a valid hex color (e.g., #FFFFFF, #FFF, or #FFFFFFFF for colors with alpha)"); ;
            RuleFor(a => a.FontColor)
                .NotNull()
                .NotEmpty()
                .MaximumLength(16)
                .Matches(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3}|[A-Fa-f0-9]{8})$")
                .WithMessage("Must be a valid hex color (e.g., #FFFFFF, #FFF, or #FFFFFFFF for colors with alpha)"); ;
        }
    }
}
