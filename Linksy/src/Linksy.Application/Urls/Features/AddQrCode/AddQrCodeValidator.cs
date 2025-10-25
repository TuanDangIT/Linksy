using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.AddQrCode
{
    internal class AddQrCodeValidator : AbstractValidator<AddQrCode>
    {
        public AddQrCodeValidator()
        {
            RuleFor(a => a.UrlId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
