using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.AddUmtParameterToUrl
{
    internal class AddUmtParameterToUrlValidator : AbstractValidator<AddUmtParameterToUrl>
    {
        public AddUmtParameterToUrlValidator()
        {
            RuleFor(a => a.UrlId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(a => a.UmtParameter)
                .NotNull();
        }
    }
}
