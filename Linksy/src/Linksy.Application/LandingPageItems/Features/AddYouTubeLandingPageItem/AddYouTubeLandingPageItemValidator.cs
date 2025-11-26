using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Features.AddYouTubeLandingPageItem
{
    internal class AddYouTubeLandingPageItemValidator : AbstractValidator<AddYouTubeLandingPageItem>
    {
        public AddYouTubeLandingPageItemValidator()
        {
            RuleFor(a => a.LandingPageId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
            RuleFor(a => a.YouTubeUrl)
                .NotNull()
                .NotEmpty()
                .MaximumLength(512);
        }
    }
}
