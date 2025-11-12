using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.AddQrCodeToUmtParameter
{
    internal class AddQrCodeToUmtParameterValidator : AbstractValidator<AddQrCodeToUmtParameter>
    {
        public AddQrCodeToUmtParameterValidator()
        {
            RuleFor(a => a.UrlId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(a => a.UmtParameterId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
