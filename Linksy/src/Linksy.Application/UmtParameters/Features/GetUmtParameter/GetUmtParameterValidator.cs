using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.GetUmtParameter
{
    internal class GetUmtParameterValidator : AbstractValidator<GetUmtParameter>
    {
        public GetUmtParameterValidator()
        {
            RuleFor(x => x.UmtParameterId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.UrlId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
        }
    }
}
