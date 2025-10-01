using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.RedirectToOriginalUrl
{
    internal class RedirectToOriginalUrlValidator : AbstractValidator<RedirectToOriginalUrl>
    {
        public RedirectToOriginalUrlValidator()
        {
            RuleFor(r => r.Code)
                .NotEmpty()
                .NotNull();
        }
    }
}
