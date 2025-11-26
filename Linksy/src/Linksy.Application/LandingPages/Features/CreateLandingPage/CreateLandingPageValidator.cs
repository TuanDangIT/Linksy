using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.CreateLandingPage
{
    internal class CreateLandingPageValidator : AbstractValidator<CreateLandingPage>
    {
        public CreateLandingPageValidator()
        {
            RuleFor(c => c.Code)
                .NotEmpty()
                .NotNull()
                .MaximumLength(128);
            RuleFor(c => c.Title)
                .NotEmpty()
                .NotNull()
                .MaximumLength(64);
            RuleFor(c => c.TitleFontColor)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(16);
            RuleFor(c => c.Description)
                .MaximumLength(1024);
            RuleFor(c => c.DescriptionFontColor)
                .MinimumLength(3)
                .MaximumLength(16)
                .When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("Description font color is required when Description is provided.");
            RuleFor(c => c)
                .Must(c => !string.IsNullOrEmpty(c.BackgroundColor) || c.BackgroundImage != null)
                .WithMessage("Either BackgroundColor or BackgroundImage must be provided.")
                .Must(c => string.IsNullOrEmpty(c.BackgroundColor) || c.BackgroundImage == null)
                .WithMessage("BackgroundColor and BackgroundImage cannot both be provided.");
            RuleFor(c => c.Tags)
                .Must(tags => tags == null || tags.Count() <= 8)
                .WithMessage("A maximum of 8 tags is allowed.");
        }
    }
}
