using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.ShortenUrl
{
    internal class ShortenUrlValidator : AbstractValidator<ShortenUrl>
    {
        public ShortenUrlValidator()
        {
            RuleFor(s => s.OriginalUrl)
                .NotEmpty()
                .NotNull();
        }
    }
}
