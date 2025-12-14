using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.GetQrCode
{
    internal class GetQrCodeValidator : AbstractValidator<GetQrCode>
    {
        public GetQrCodeValidator()
        {
            RuleFor(x => x.QrCodeId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
        }
    }
}
