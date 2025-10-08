using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.GetUrl
{
    internal class GetUrlValidator : AbstractValidator<GetUrl>
    {
        public GetUrlValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
        }
    }
}
