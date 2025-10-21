using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.ChangeOriginalUrl
{
    internal class ChangeOriginalUrlValidator : AbstractValidator<ChangeOriginalUrl>
    {
        public ChangeOriginalUrlValidator()
        {
            RuleFor(c =>  c.UrlId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(c => c.OriginalUrl)
                .NotNull()
                .NotEmpty()
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("The OriginalUrl must be a valid absolute URL.");
        }
    }
}
