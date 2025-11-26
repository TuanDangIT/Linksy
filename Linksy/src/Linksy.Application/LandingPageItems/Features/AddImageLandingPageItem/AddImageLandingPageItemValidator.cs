using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Features.AddImageLandingPageItem
{
    internal class AddImageLandingPageItemValidator : AbstractValidator<AddImageLandingPageItem>
    {
        public AddImageLandingPageItemValidator()
        {
            RuleFor(a => a.LandingPageId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
            RuleFor(a => a.AltText)
                .NotNull()
                .NotEmpty()
                .MaximumLength(256);
            RuleFor(a => a.Image)
                .NotNull()
                .Must(f => f.Length > 0).WithMessage("File cannot be empty.")
                .Must(f => Regex.IsMatch(f.ContentType, "^image\\/(jpeg|png|gif|webp)$"))
                .WithMessage("Only JPEG, PNG, GIF, or WEBP images are allowed.");
        }
    }
}
